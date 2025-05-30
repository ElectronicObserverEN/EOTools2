using EOToolsWeb.Shared.EquipmentUpgrades;
using System.Text.Json.Serialization;

namespace EOToolsWeb.Api.Models.EquipmentUpgrades;

public class EquipmentUpgradeImprovmentCostDataModel(EquipmentUpgradeImprovmentCost cost)
{
    [JsonPropertyName("fuel")]
    public int Fuel => cost.Fuel;

    [JsonPropertyName("ammo")]
    public int Ammo => cost.Ammo;

    [JsonPropertyName("steel")]
    public int Steel => cost.Steel;

    [JsonPropertyName("baux")]
    public int Bauxite => cost.Bauxite;

    /// <summary>
    /// Costs for level 0 -> 6
    /// </summary>
    [JsonPropertyName("p1")]
    public EquipmentUpgradeImprovmentCostDetail Cost0To5 => cost.Cost0To5;

    /// <summary>
    /// Costs for level 7 -> 10
    /// </summary>
    [JsonPropertyName("p2")]
    public EquipmentUpgradeImprovmentCostDetail Cost6To9 => cost.Cost6To9;

    /// <summary>
    /// Costs for conversion
    /// </summary>
    [JsonPropertyName("conv")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EquipmentUpgradeImprovmentCostDetail? CostMax => cost.CostMax;

    /// <summary>
    /// Extra costs
    /// </summary>
    [JsonPropertyName("extra")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<EquipmentUpgradeExtraCostDataModel>? ExtraCost => cost.ExtraCost.Count is 0 ? null : cost.ExtraCost.Select(extraCost => new EquipmentUpgradeExtraCostDataModel(extraCost)).ToList();
}
