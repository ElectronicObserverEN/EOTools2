using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.EquipmentUpgrades;

public class EquipmentUpgradeImprovmentCostDetail
{
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// Devmat cost
    /// </summary>
    [JsonPropertyName("devmats")]
    public int DevmatCost { get; set; }

    /// <summary>
    /// Devmat cost if slider is used
    /// </summary>
    [JsonPropertyName("devmats_sli")]
    public int SliderDevmatCost { get; set; }

    /// <summary>
    /// Screw cost
    /// </summary>
    [JsonPropertyName("screws")]
    public int ImproveMatCost { get; set; }

    /// <summary>
    /// Screw cost if slider is used
    /// </summary>
    [JsonPropertyName("screws_sli")]
    public int SliderImproveMatCost { get; set; }

    [JsonPropertyName("equips")]
    public List<EquipmentUpgradeImprovmentCostItemDetail> EquipmentDetail { get; set; } = new List<EquipmentUpgradeImprovmentCostItemDetail>();

    [JsonPropertyName("consumable")]
    public List<EquipmentUpgradeImprovmentCostItemDetail> ConsumableDetail { get; set; } = new List<EquipmentUpgradeImprovmentCostItemDetail>();

}