using System.Diagnostics;

namespace EOToolsWeb.Api.Services;

public class GitManagerService(ConfigurationService configuration)
{
    public string FolderPath { get; set; } = Path.Combine("Data", "DataRepo");

    private string Url => configuration.DataRepoUrl;

    public async Task Initialize()
    {
        /*if (Directory.Exists(FolderPath))
        {
            Directory.Delete(FolderPath, true);
        }*/

        await Process.Start(new ProcessStartInfo()
        {
            FileName = "git",
            Arguments = $"clone {Url} {FolderPath}",
        })!.WaitForExitAsync();

        await ExecuteCommand("pull");
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
