using System.Text.Json.Serialization;
using EOToolsWeb.Shared.EquipmentUpgrades;

namespace EOToolsWeb.Api.Models.EquipmentUpgrades;

public class EquipmentUpgradeDataModel(EquipmentUpgradeModel model)
{
    [JsonPropertyName("eq_id")] public int EquipmentId => model.EquipmentId;

    /// <summary>
    /// Improvments possibles for this equipment
    /// </summary>
    [JsonPropertyName("improvement")]
    public List<EquipmentUpgradeImprovmentDataModel> Improvement { get; set; } = model.Improvement
        .Select(imp => new EquipmentUpgradeImprovmentDataModel(imp))
        .ToList();

    /// <summary>
    /// This equipment can be converted to those equipments
    /// </summary>
    [JsonPropertyName("convert_to")]
    public List<EquipmentUpgradeConversionModel> ConvertTo { get; set; } = [];

    /// <summary>
    /// This equipment is use in those equipments upgrades
    /// </summary>
    [JsonPropertyName("upgrade_for")]
    public List<int> UpgradeFor { get; set; } = [];
}