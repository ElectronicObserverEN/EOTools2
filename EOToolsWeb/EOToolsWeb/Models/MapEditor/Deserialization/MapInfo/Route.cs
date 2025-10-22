using System.Text.Json.Serialization;

namespace EOToolsWeb.Models.MapEditor.Deserialization.MapInfo;

public class Route
{
    [JsonPropertyName("img")]
    public string? Image { get; set; }
}