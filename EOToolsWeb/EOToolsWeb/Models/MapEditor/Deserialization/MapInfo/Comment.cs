using System.Text.Json.Serialization; 
namespace EOToolsWeb.Models.MapEditor.Deserialization.MapInfo{ 

    public class Comment
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

}