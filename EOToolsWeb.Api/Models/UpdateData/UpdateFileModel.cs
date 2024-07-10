using System.Text.Json.Serialization;

namespace EOToolsWeb.Api.Models.UpdateData;

public record UpdateFileModel
{
    [JsonPropertyName("bld_date")]
    public string bld_date { get; set; } = "";

    [JsonPropertyName("ver")]
    public string ver { get; set; } = "";

    [JsonPropertyName("note")]
    public string note { get; set; } = "";

    [JsonPropertyName("url")]
    public string url { get; set; } = "";

    [JsonPropertyName("ApiServer")]
    public string ApiServer { get; set; } = "";

    [JsonPropertyName("hash")]
    public string hash { get; set; } = "";

    [JsonPropertyName("nodes")]
    public int nodes { get; set; }

    [JsonPropertyName("QuestTrackers")]
    public int QuestTrackers { get; set; }

    [JsonPropertyName("Locks")]
    public int Locks { get; set; }

    [JsonPropertyName("FitBonuses")]
    public int FitBonuses { get; set; }

    [JsonPropertyName("EquipmentUpgrades")]
    public int EquipmentUpgrades { get; set; }

    [JsonPropertyName("kancolle_mt")]
    public string kancolle_mt { get; set; } = "";

    [JsonPropertyName("event_state")]
    public int event_state { get; set; }

    [JsonPropertyName("MaintInfoLink")]
    public string MaintInfoLink { get; set; } = "";

    [JsonPropertyName("MaintStart")]
    public string MaintStart { get; set; } = "";

    [JsonPropertyName("MaintEnd")]
    public string MaintEnd { get; set; } = "";

    [JsonPropertyName("MaintEventState")]
    public int MaintEventState { get; set; }
}
