using System.Text.Json.Serialization; 
namespace EOToolsWeb.Models.MapEditor.Deserialization.MapInfo{ 

    public class Enemy
    {
        [JsonPropertyName("no")]
        public int? No { get; set; }

        [JsonPropertyName("x")]
        public int? X { get; set; }

        [JsonPropertyName("y")]
        public int? Y { get; set; }

        [JsonPropertyName("img")]
        public string Img { get; set; }
    }

}