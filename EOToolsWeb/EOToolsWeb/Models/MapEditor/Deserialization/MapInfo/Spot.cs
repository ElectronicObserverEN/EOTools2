using System.Collections.Generic;
using System.Text.Json.Serialization; 
namespace EOToolsWeb.Models.MapEditor.Deserialization.MapInfo{ 

    public class Spot
    {
        [JsonPropertyName("no")]
        public int? No { get; set; }

        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }

        [JsonPropertyName("line")]
        public Line? Line { get; set; }

        [JsonPropertyName("offsets")]
        public Dictionary<string, Point>? Offsets { get; set; }

        [JsonPropertyName("color")]
        public int? Color { get; set; }

        [JsonPropertyName("branch")]
        public Branch Branch { get; set; }

        [JsonPropertyName("direction")]
        public string Direction { get; set; }
        
        [JsonPropertyName("route")]
        public Route? Route { get; set; }

        [JsonPropertyName("comment")]
        public Comment Comment { get; set; }
    }

}