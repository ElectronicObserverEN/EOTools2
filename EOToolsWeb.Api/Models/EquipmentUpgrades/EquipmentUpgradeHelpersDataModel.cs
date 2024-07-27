using EOToolsWeb.Shared.EquipmentUpgrades;
using System.Text.Json.Serialization;

namespace EOToolsWeb.Api.Models.EquipmentUpgrades
{
    public class EquipmentUpgradeHelpersDataModel(EquipmentUpgradeHelpersModel model)
    {
        /// <summary>
        /// Ids of the helpers
        /// </summary>
        [JsonPropertyName("ship_ids")]
        public List<int> ShipIdsList => model.ShipIds.Select(m => m.ShipId).ToList();

        /// <summary>
        /// Days those helpers can help
        /// </summary>
        [JsonPropertyName("days")]
        public List<DayOfWeek> CanHelpOnDaysList => model.CanHelpOnDays.Select(m => m.Day).ToList();
    }
}
