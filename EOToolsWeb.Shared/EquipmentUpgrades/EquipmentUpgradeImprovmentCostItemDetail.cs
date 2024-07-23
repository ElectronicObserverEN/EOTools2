using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.EquipmentUpgrades;

public class EquipmentUpgradeImprovmentCostItemDetail
{
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// Id of the item
    /// </summary>
    [JsonPropertyName("id")]
    public int ItemId { get; set; }

    /// <summary>
    /// Number of this equipment required
    /// </summary>
    [JsonPropertyName("eq_count")]
    public int Count { get; set; }
}
