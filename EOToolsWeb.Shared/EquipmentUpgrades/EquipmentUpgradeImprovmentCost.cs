using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.EquipmentUpgrades;

public class EquipmentUpgradeImprovmentCost
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonPropertyName("fuel")]
    public int Fuel { get; set; }

    [JsonPropertyName("ammo")]
    public int Ammo { get; set; }

    [JsonPropertyName("steel")]
    public int Steel { get; set; }

    [JsonPropertyName("baux")]
    public int Bauxite { get; set; }

    /// <summary>
    /// Costs for level 0 -> 6
    /// </summary>
    [JsonPropertyName("p1")]
    public EquipmentUpgradeImprovmentCostDetail Cost0To5 { get; set; } = new();

    /// <summary>
    /// Costs for level 7 -> 10
    /// </summary>
    [JsonPropertyName("p2")]
    public EquipmentUpgradeImprovmentCostDetail Cost6To9 { get; set; } = new ();

    /// <summary>
    /// Costs for conversion
    /// </summary>
    [JsonPropertyName("conv")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EquipmentUpgradeImprovmentCostDetail? CostMax { get; set; } = null;

    /// <summary>
    /// Extra costs
    /// </summary>
    [JsonPropertyName("extra")]
    public List<EquipmentUpgradeExtraCost> ExtraCost { get; set; } = [];
}
