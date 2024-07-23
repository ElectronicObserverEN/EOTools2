using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.EquipmentUpgrades
{
    public class EquipmentUpgradeConversionModel
    {
        [JsonIgnore]
        public EquipmentUpgradeImprovmentModel ImprovmentModel { get; set; }

        [JsonIgnore]
        public int ImprovmentModelId { get; set; }

        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("id_after")]
        public int IdEquipmentAfter { get; set; }

        [JsonPropertyName("lvl_after")]
        public int EquipmentLevelAfter { get; set; }
    }
}
