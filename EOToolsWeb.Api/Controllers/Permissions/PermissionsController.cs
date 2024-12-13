using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.Permissions;
using EOToolsWeb.Shared.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers.Permissions;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class PermissionsController(EoToolsDbContext db, IPermissionsService permissionsService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private IPermissionsService PermissionsService { get; } = permissionsService;

    [HttpGet]
    public async Task<PermissionsPerUserModel> Get(int userId)
    {
        return await PermissionsService.GetPermission(userId);
    }

    [HttpPut("permissions")]
    public async Task<IActionResult> PutPermissions(int userId, PermissionsPermissionsModel permissions)
    {
        PermissionsPerUserModel permissionsPerUser = await PermissionsService.GetPermission(userId);

        PermissionModel savedPermissions = permissionsPerUser.Permissions;

        await UpdatePermissions(permissions, savedPermissions);

        return Ok(savedPermissions);
    }


    [HttpPut("updates")]
    public async Task<IActionResult> PutUpdates(int userId, UpdatesPermissionsModel permissions)
    {
        PermissionsPerUserModel permissionsPerUser = await PermissionsService.GetPermission(userId);

        PermissionModel savedPermissions = permissionsPerUser.Updates;

        await UpdatePermissions(permissions, savedPermissions);

        return Ok(savedPermissions);
    }

    private async Task UpdatePermissions(PermissionModel permissions, PermissionModel savedPermissions)
    {
        savedPermissions.CanRead = permissions.CanRead;
        savedPermissions.CanAdd = permissions.CanAdd;
        savedPermissions.CanEdit = permissions.CanEdit;
        savedPermissions.CanDelete = permissions.CanDelete;

        if (savedPermissions.Id is 0)
        {
            await Database.AddAsync(savedPermissions);
        }

        await Database.SaveChangesAsync();
    }
}
