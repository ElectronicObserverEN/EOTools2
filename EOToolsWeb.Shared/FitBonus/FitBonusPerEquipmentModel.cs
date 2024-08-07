using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.FitBonus;

public class FitBonusPerEquipmentModel
{
    [JsonPropertyName("types")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<int>? EquipmentTypes { get; set; } = null;

    [JsonPropertyName("ids")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<int>? EquipmentIds { get; set; } = null;

    [JsonPropertyName("bonuses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<FitBonusDataModel>? Bonuses { get; set; } = null;
}