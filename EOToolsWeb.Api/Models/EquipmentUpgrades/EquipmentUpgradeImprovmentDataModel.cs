using System.Text.Json.Serialization;
using EOToolsWeb.Shared.EquipmentUpgrades;

namespace EOToolsWeb.Api.Models.EquipmentUpgrades
{
    public class EquipmentUpgradeImprovmentDataModel(EquipmentUpgradeImprovmentModel model)
    {
        [JsonPropertyName("convert")]
        public EquipmentUpgradeConversionModel? ConversionData { get; set; } = model.ConversionData;

        [JsonPropertyName("helpers")]
        public List<EquipmentUpgradeHelpersDataModel> Helpers { get; set; } = model.Helpers.Select(m => new EquipmentUpgradeHelpersDataModel(m)).ToList();

        [JsonPropertyName("costs")]
        public EquipmentUpgradeImprovmentCost Costs { get; set; } = model.Costs;
    }
}
