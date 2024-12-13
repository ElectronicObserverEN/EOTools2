using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EOToolsWeb.Shared.Users;

namespace EOToolsWeb.Shared.Permissions;

public class PermissionsPerUserModel
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(UserModel))]
    public int UserId { get; set; }

    [JsonPropertyName("permissions")]
    public PermissionsPermissionsModel Permissions { get; set; } = new();

    [JsonPropertyName("updates")]
    public PermissionsPermissionsModel Updates { get; set; } = new();
}
