using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.EquipmentUpgrades
{
    public class EquipmentUpgradeHelpersModel
    {
        [JsonIgnore]
        public int EquipmentUpgradeImprovmentModelId { get; set; }

        [JsonIgnore]
        public EquipmentUpgradeImprovmentModel Improvment { get; set; }

        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Ids of the helpers
        /// </summary>
        [JsonPropertyName("ship_ids")]
        [NotMapped]
        public List<int> ShipIdsList => ShipIds.Select(m => m.ShipId).ToList();
        //public List<int> ShipIdsList { get; set; } = new();

        [JsonIgnore]
        public List<EquipmentUpgradeHelpersShipModel> ShipIds { get; set; } = new List<EquipmentUpgradeHelpersShipModel>();

        /// <summary>
        /// Days those helpers can help
        /// </summary>
        [JsonPropertyName("days")]
        [NotMapped]
        public List<DayOfWeek> CanHelpOnDaysList => CanHelpOnDays.Select(m => m.Day).ToList();

        [JsonIgnore]
        public List<EquipmentUpgradeHelpersDayModel> CanHelpOnDays { get; set; } = new();
    }
}
