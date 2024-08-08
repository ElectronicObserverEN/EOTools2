using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.Maps;

public class MapsTranslationModel
{
    [JsonPropertyName("version")]
    public string Version { get; set; } = "";

    [JsonPropertyName("map")]
    public Dictionary<string, string> Maps { get; set; } = new();

    [JsonPropertyName("fleet")]
    public Dictionary<string, string> Fleets { get; set; } = new();
}