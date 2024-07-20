using System.Text.Json.Serialization;

namespace EOToolsWeb.Models;

public class ConfigModel
{
    [JsonPropertyName("serverUrl")] 
    public string ServerUrl { get; set; } = "";
}
