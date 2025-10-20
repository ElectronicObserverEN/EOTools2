using System.Text.Json.Serialization; 
namespace EOToolsWeb.Models.MapEditor.Deserialization.MapInfo{ 

    public class Point
    {
        [JsonPropertyName("x")]
        public int? X { get; set; }

        [JsonPropertyName("y")]
        public int? Y { get; set; }
    }

}