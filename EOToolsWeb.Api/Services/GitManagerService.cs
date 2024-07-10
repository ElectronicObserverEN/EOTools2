using System.Diagnostics;

namespace EOToolsWeb.Api.Services;

public class GitManagerService(ConfigurationService configuration)
{
    private string FolderPath { get; set; } = "";

    private string Url => configuration.DataRepoUrl;

    public async Task Initialize()
    {
        if (Directory.Exists(FolderPath))
        {
            Directory.Delete(FolderPath, true);
        }

        string path = Path.Combine(FolderPath, "DataRepo");

        await ExecuteCommand($"clone {Url} {FolderPath}");

        FolderPath = path;
    }

    private async Task ExecuteCommand(string command)
    {
        await Process.Start(new ProcessStartInfo()
        {
            FileName = "git",
            WorkingDirectory = FolderPath,
            Arguments = command,
        })!.WaitForExitAsync();
    }

    public async Task Pull()
    {
        await ExecuteCommand("pull");
    }

    public async Task Push(string commitMessage)
    {
        await ExecuteCommand("add -A");
        await ExecuteCommand($"commit -m \"{commitMessage}\"");
        await ExecuteCommand("push");
    }
}
