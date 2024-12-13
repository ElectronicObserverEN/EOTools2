using EOToolsWeb.Api.Database;
using EOToolsWeb.Shared.Permissions;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Services.Permissions;

public class PermissionsService(EoToolsDbContext db) : IPermissionsService
{
    private EoToolsDbContext Database { get; } = db;

    public async Task<PermissionsPerUserModel> GetPermission(int userId)
    {
        PermissionsPerUserModel? permissions = await Database.Permissions
            .Include("Permissions")
            .Include("Updates")
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (permissions is not null) return permissions;

        permissions = new PermissionsPerUserModel()
        {
            UserId = userId,
        };

        await Database.Permissions.AddAsync(permissions);
        await Database.SaveChangesAsync();

        return permissions;
    }
}
