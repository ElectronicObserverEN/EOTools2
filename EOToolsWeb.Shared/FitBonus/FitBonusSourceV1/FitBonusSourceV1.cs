using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.FitBonus.FitBonusSourceV1;

public class FitBonusSourceV1
{
    [JsonPropertyName("types")]
    public List<int>? Types { get; set; }

    [JsonPropertyName("bonuses")] 
    public List<FitBonusSourceV1_FitBonus> Bonuses { get; set; } = new();

    [JsonPropertyName("ids")]
    public List<int>? Ids { get; set; }
}