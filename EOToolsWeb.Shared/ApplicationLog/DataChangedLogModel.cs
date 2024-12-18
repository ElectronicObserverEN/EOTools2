using System.ComponentModel.DataAnnotations.Schema;
using EOToolsWeb.Shared.Users;

namespace EOToolsWeb.Shared.ApplicationLog;

public class DataChangedLogModel : LogModel
{
    public int EntityId { get; set; }

    public string EntityName { get; set; } = string.Empty;

    public required UserModel User { get; set; }

    [NotMapped]
    public override string LogDetail => $"{User.Username} changed {EntityName} (id {Id}): \n\r{Changes}";

    public string Changes { get; set; } = "";
}