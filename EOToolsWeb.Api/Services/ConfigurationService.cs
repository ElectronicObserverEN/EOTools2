using System.Text.Json;

namespace EOToolsWeb.Api.Services;

public class ConfigurationService
{
    public string DataRepoUrl { get; set; } = "";

    public async Task Initialize()
    {
        Dictionary<string, string>? config = JsonSerializer.Deserialize<Dictionary<string, string>>(await File.ReadAllTextAsync("Config.json"));

        DataRepoUrl = config?["dataRepoUrl"] ?? "";
    }
}
