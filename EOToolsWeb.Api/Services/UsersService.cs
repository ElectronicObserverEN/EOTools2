using EOToolsWeb.Api.Models;
using System.Security.Cryptography;
using EOToolsWeb.Api.Database;
using EOToolsWeb.Shared.Users;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Services;

public class UsersService(EoToolsUsersDbContext db)
{
    private EoToolsUsersDbContext Database => db;

    public async Task<UserConnection> Login(string username, string password)
    {
        UserModel? user = Database.Users.FirstOrDefault(u => u.Username == username);

        if (user is null || string.IsNullOrEmpty(user.Password))
        {
            throw new UnauthorizedAccessException();
        }

        string savedPasswordHash = user.Password;

        byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA1);
        byte[] hash = pbkdf2.GetBytes(20);

        for (int i = 0; i < 20; i++)
            if (hashBytes[i + 16] != hash[i])
                throw new UnauthorizedAccessException();

        string token = Guid.NewGuid().ToString();

        UserConnection con = new()
        {
            User = user,
            Token = token,
        };

        await db.UserConnections.AddAsync(con);
        await db.SaveChangesAsync();

        return con;
    }

    public async Task<UserConnection?> GetConnectionByToken(string token)
    {
        return await db.UserConnections
            .Include(nameof(UserConnection.User))
            .FirstOrDefaultAsync(c => c.Token == token);
    }
}