using System.Security.Cryptography;
using EOToolsWeb.Api.Database;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers.Users;

[ApiController]
[Route("[controller]")]
public class UsersController(EoToolsDbContext db) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;

    [HttpGet]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<ActionResult> Get()
    {
        List<UserModel> users = await Database.Users
            .AsNoTracking()
            .ToListAsync();

        users.ForEach(user => user.Password = "");

        return Ok(users);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> Put(UserModel user)
    {
        UserModel? savedUser = await Database.Users.FindAsync(user.Id);

        if (savedUser is null)
        {
            return NotFound();
        }

        return await EditAnUser(user, savedUser);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> Post(UserModel user)
    {
        TryValidateModel(user);

        user.Password = GetPasswordHashed(user.Password ?? "");

        await Database.Users.AddAsync(user);
        await Database.SaveChangesAsync();

        return Ok(user with
        {
            Password = "",
        });
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> Delete(int id)
    {
        UserModel? savedUser = await Database.Users.FindAsync(id);

        if (savedUser is null)
        {
            return NotFound();
        }

        Database.Users.Remove(savedUser);
        await Database.SaveChangesAsync();

        return Ok();
    }

    private string GetPasswordHashed(string password)
    {
        byte[] salt;
        RandomNumberGenerator.Create().GetBytes(salt = new byte[16]);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA1);
        byte[] hash = pbkdf2.GetBytes(20);

        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        return Convert.ToBase64String(hashBytes);
    }

    [HttpGet("currentUser")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication")]
    public async Task<IActionResult> GetCurrentUserInformation()
    {
        string token = Request.Headers["X-TOKEN-EO-TOOLS-WEB-X"].ToString();

        UserConnection? connection = await Database.UserConnections
            .Include(nameof(UserConnection.User))
            .AsNoTracking()
            .FirstOrDefaultAsync(con => con.Token == token);

        if (connection is null)
        {
            return Unauthorized();
        }

        return Ok(connection.User with
        {
            Password = "",
        });
    }

    [HttpPut("currentUser")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication")]
    public async Task<IActionResult> EditCurrentUser(UserModel user)
    {
        string token = Request.Headers["X-TOKEN-EO-TOOLS-WEB-X"].ToString();

        UserConnection? connection = await Database.UserConnections
            .Include(nameof(UserConnection.User))
            .FirstOrDefaultAsync(con => con.Token == token);

        if (connection is null)
        {
            return NotFound();
        }

        // user can't change its own role
        user.Kind = connection.User.Kind;

        return await EditAnUser(user, connection.User);
    }

    private async Task<IActionResult> EditAnUser(UserModel user, UserModel savedUser)
    {
        if (!string.IsNullOrEmpty(user.Password))
        {
            savedUser.Password = GetPasswordHashed(user.Password);

            string token = Request.Headers["X-TOKEN-EO-TOOLS-WEB-X"].ToString();

            List<UserConnection> connections = await Database.UserConnections.Where(con => con.Token != token && con.User.Id == user.Id).ToListAsync();
            Database.UserConnections.RemoveRange(connections);
        }

        savedUser.Kind = user.Kind;
        savedUser.Username = user.Username;

        Database.Users.Update(savedUser);
        await Database.SaveChangesAsync();

        return Ok(savedUser with
        {
            Password = "",
        });
    }
}
