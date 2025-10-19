using EOToolsWeb.Shared.Ships;
using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.EquipmentDevelopment;

public class EquipmentRecipeModel
{
    [JsonIgnore]
    public int Id { get; set; }

    public int Rank { get; set; }

    public int EquipmentId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ShipTypes>? ShipTypes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<int>? Ships { get; set; }

    public int Fuel { get; set; }
    public int Ammo { get; set; }
    public int Steel { get; set; }
    public int Bauxite { get; set; }
}
