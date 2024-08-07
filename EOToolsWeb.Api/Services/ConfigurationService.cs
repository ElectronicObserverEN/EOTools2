using System.Text.Json;

namespace EOToolsWeb.Api.Services;

public class ConfigurationService
{
    public string DataRepoUrl { get; private set; } = "";
    public string FitBonusSourceUrl { get; private set; } = "";

    public async Task Initialize()
    {
        Dictionary<string, string>? config = JsonSerializer.Deserialize<Dictionary<string, string>>(await File.ReadAllTextAsync(Path.Combine("Assets", "Config.json")));

        DataRepoUrl = config?["dataRepoUrl"] ?? "";
        FitBonusSourceUrl = config?["FitBonusSourceUrl"] ?? "";
    }

    public async Task<string> GetSoftwareVersion()
    {
        Dictionary<string, string>? config = JsonSerializer.Deserialize<Dictionary<string, string>>(await File.ReadAllTextAsync(Path.Combine("Assets", "Config.json")));

        return config?["softwareVersion"] ?? "0.0.0.0";
    } 
}
