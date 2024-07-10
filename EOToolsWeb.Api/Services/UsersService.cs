using EOToolsWeb.Api.Models;
using System.Security.Cryptography;
using EOToolsWeb.Api.Database;

namespace EOToolsWeb.Api.Services;

public class UsersService(EoToolsDbContext db)
{
    private EoToolsDbContext Database => db;

    public async Task<UserConnection> Login(string username, string password)
    {
        UserModel? user = Database.Users.FirstOrDefault(u => u.Username == username);

        if (user is null)
        {
            throw new UnauthorizedAccessException();
        }

        string savedPasswordHash = user.Password;

        byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
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

    public string GetPasswordHashed(string password)
    {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        byte[] hash = pbkdf2.GetBytes(20);

        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        return Convert.ToBase64String(hashBytes);
    }
}