using System.Text.Json.Serialization; 
namespace EOToolsWeb.Models.MapEditor.Deserialization.MapInfo{ 

    public class Branch
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("x")]
        public int? X { get; set; }

        [JsonPropertyName("y")]
        public int? Y { get; set; }

        [JsonPropertyName("beak")]
        public string Beak { get; set; }
    }

}