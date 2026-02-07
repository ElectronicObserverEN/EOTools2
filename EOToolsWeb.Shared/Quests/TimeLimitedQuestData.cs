using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.Quests;

public class TimeLimitedQuestData
{
    [JsonPropertyName("id")]
    public required int ApiId { get; set; }

    [JsonPropertyName("endTime")]
    public DateTime? EndTime { get; set; }

    [JsonPropertyName("dailyReset")]
    public bool ProgressResetsDaily { get; set; }
}
