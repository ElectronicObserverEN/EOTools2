using System.Text.Json.Serialization;
using EOToolsWeb.Shared.Ships;

namespace EOToolsWeb.Shared.FitBonus
{
    public class FitBonusDataModel
    {
        [JsonPropertyName("shipClass")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<int>? ShipClasses { get; set; } = null;

        /// <summary>
        /// Master id = exact id of the ship
        /// </summary>
        [JsonPropertyName("shipX")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<int>? ShipMasterIds { get; set; } = null;

        /// <summary>
        /// Base id of the ship (minimum remodel), bonus applies to all of the ship forms
        /// </summary>
        [JsonPropertyName("shipS")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<int>? ShipIds { get; set; } = null;

        [JsonPropertyName("shipType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ShipTypes>? ShipTypes { get; set; } = null;

        [JsonPropertyName("shipNationality")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ShipNationality>? ShipNationalities { get; set; }

        [JsonPropertyName("requires")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<int>? EquipmentRequired { get; set; } = null;

        [JsonPropertyName("requiresLevel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? EquipmentRequiresLevel { get; set; } = null;

        [Obsolete("Use NumberOfEquipmentsRequiredAfterOtherFilters instead", true)]
        [JsonPropertyName("requiresNum")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? NumberOfEquipmentsRequired { get; set; }


        [JsonPropertyName("requiresType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<int>? EquipmentTypesRequired { get; set; } = null;

        [JsonPropertyName("requiresNumType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? NumberOfEquipmentTypesRequired { get; set; }

        /// <summary>
        /// Improvment level of the equipment required
        /// </summary>
        [JsonPropertyName("level")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? EquipmentLevel { get; set; }

        /// <summary>
        /// Number Of Equipments Required after applying the improvment filter
        /// </summary>
        [JsonPropertyName("num")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? NumberOfEquipmentsRequiredAfterOtherFilters { get; set; }

        /// <summary>
        /// Bonuses to apply
        /// Applied x times, x being the number of equipment matching the conditions of the bonus fit 
        /// If NumberOfEquipmentsRequiredAfterOtherFilters or EquipmentRequired or EquipmentTypesRequired, bonus is applied only once
        /// </summary>
        [JsonPropertyName("bonus")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public FitBonusValueModel? Bonuses { get; set; } = null;


        /// <summary>
        /// Bonuses to apply if ship has a radar with LOS >= 5
        /// </summary>
        [JsonPropertyName("bonusSR")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] 
        public FitBonusValueModel? BonusesIfSurfaceRadar { get; set; }

        /// <summary>
        /// Bonuses to apply if ship has a radar with AA >= 2
        /// </summary>
        [JsonPropertyName("bonusAR")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public FitBonusValueModel? BonusesIfAirRadar { get; set; }

        /// <summary>
        /// Bonuses to apply if ship has a radar with ACC >= 8
        /// </summary>
        [JsonPropertyName("bonusAccR")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public FitBonusValueModel? BonusesIfAccuracyRadar { get; set; }
    }
}