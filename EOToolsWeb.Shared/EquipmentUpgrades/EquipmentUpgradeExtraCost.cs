using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.EquipmentUpgrades;

public class EquipmentUpgradeExtraCost
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonPropertyName("levels")]
    public List<EquipmentUpgradeExtraCostLevel> Levels { get; set; } = [];

    [JsonPropertyName("consumable")]
    public List<EquipmentUpgradeImprovmentCostItemDetail> Consumables { get; set; } = [];

    [JsonPropertyName("equips")]
    public List<EquipmentUpgradeImprovmentCostItemDetail> Equipments { get; set; } = [];
}