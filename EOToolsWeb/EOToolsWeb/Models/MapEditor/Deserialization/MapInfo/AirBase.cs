using System.Text.Json.Serialization; 
namespace EOToolsWeb.Models.MapEditor.Deserialization.MapInfo
{ 

    public class AirBase
    {
        [JsonPropertyName("phase")]
        public int? Phase { get; set; }

        [JsonPropertyName("point")]
        public Point Point { get; set; }
    }

}