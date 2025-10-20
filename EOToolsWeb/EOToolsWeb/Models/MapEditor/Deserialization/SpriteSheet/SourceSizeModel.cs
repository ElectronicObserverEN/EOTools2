using System.Text.Json.Serialization;

namespace EOToolsWeb.Models.MapEditor.Deserialization.SpriteSheet;

public class SourceSizeModel
{
    [JsonPropertyName("w")]
    public int Width { get; set; }
    
    [JsonPropertyName("h")]
    public int Height { get; set; }
}