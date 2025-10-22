using System.Text.Json.Serialization;

namespace EOToolsWeb.Models.MapEditor.Deserialization.MapInfo;

public class Label
{
    [JsonPropertyName("x")] public int X { get; set; }
    [JsonPropertyName("y")] public int Y { get; set; }
    [JsonPropertyName("img")] public string? Image { get; set; }
}