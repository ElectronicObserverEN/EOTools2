using System.Text.Json.Serialization; 
namespace EOToolsWeb.Models.MapEditor.Deserialization.MapInfo{ 

    public class AirRaid
    {
        [JsonPropertyName("no")]
        public int? No { get; set; }

        [JsonPropertyName("type")]
        public int? Type { get; set; }

        [JsonPropertyName("from")]
        public From From { get; set; }

        [JsonPropertyName("to")]
        public To To { get; set; }
    }

}