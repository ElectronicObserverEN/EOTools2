using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.EquipmentUpgrades;

public class EquipmentUpgradeExtraCostLevel
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonPropertyName("level")]
    public UpgradeLevel Level { get; set; }
}
