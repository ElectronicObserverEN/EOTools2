using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.Quests;

public class QuestMetadata
{
    [JsonPropertyName("id")]
    public required int ApiId { get; set; }

    [JsonPropertyName("endTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? EndTime { get; set; }

    [JsonPropertyName("resetType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public QuestProgressResetType? QuestProgressResetType { get; set; }
}
