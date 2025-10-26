using System.Text.Json.Serialization;

namespace EOToolsWeb.Models.MapEditor.Serialization;

public record ExportNodeDataModel
{
    [JsonPropertyName("type")]
    public int Type { get; set; }
    
    [JsonPropertyName("x")]
    public int X { get; set; }
    
    [JsonPropertyName("y")]
    public int Y { get; set; }
}