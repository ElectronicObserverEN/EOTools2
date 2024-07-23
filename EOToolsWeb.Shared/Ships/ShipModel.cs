using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.Ships;

public class ShipModel
{
    public int Id { get; set; }

    public string NameEN { get; set; } = "";

    public string NameJP { get; set; } = "";

    public int ApiId { get; set; }

    [JsonIgnore]
    public ShipClassModel? ShipClass { get; set; }

    [NotMapped]
    public int? ShipClassId
    {
        get => ShipClass?.Id;
        set => ShipClass = value switch
        {
            { } => new()
            {
                Id = value ?? 0,
            },
            _ => null,
        };
    }

    public int? ShipClassApiId => ShipClass?.ApiId;
    
    [NotMapped]
    public bool IsFriendly => ApiId < 1500;
}
