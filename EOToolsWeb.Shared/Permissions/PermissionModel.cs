using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.Permissions;

public abstract class PermissionModel
{
    [JsonIgnore]
    public int Id { get; set; }

    public bool CanAdd { get; set; } = false;
    public bool CanDelete { get; set; } = false;
    public bool CanEdit { get; set; } = false;
    public bool CanRead { get; set; } = false;
}
