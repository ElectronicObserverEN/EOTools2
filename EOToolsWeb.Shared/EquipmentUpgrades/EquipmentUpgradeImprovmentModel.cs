using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.EquipmentUpgrades
{
    public class EquipmentUpgradeImprovmentModel
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("convert")]
        public EquipmentUpgradeConversionModel? ConversionData { get; set; }

        [JsonPropertyName("helpers")]
        public List<EquipmentUpgradeHelpersModel> Helpers { get; set; } = new List<EquipmentUpgradeHelpersModel>();

        [JsonPropertyName("costs")]
        public EquipmentUpgradeImprovmentCost Costs { get; set; } = new EquipmentUpgradeImprovmentCost();
    }
}
