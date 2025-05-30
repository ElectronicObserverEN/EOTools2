using EOToolsWeb.Shared.EquipmentUpgrades;
using System.Text.Json.Serialization;

namespace EOToolsWeb.Api.Models.EquipmentUpgrades;

public class EquipmentUpgradeExtraCostDataModel(EquipmentUpgradeExtraCost extraCost)
{
    [JsonPropertyName("levels")]
    public List<UpgradeLevel> Levels => extraCost.Levels.Select(level => level.Level).ToList();

    [JsonPropertyName("consumable")]
    public List<EquipmentUpgradeImprovmentCostItemDetail> Consumables => extraCost.Consumables;
}
