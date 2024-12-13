using EOToolsWeb.Shared.Permissions;

namespace EOToolsWeb.Api.Services.Permissions;

public interface IPermissionsService
{
    public Task<PermissionsPerUserModel> GetPermission(int userId);
}