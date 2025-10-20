using System.Text.Json.Serialization;

namespace EOToolsWeb.Models.MapEditor.Deserialization.SpriteSheet;

public class SpriteSourceSizeModel
{
    [JsonPropertyName("x")]
    public int X { get; set; }
    
    [JsonPropertyName("y")]
    public int Y { get; set; }
    
    [JsonPropertyName("w")]
    public int Width { get; set; }
    
    [JsonPropertyName("h")]
    public int Height { get; set; }
}