using System.Diagnostics;

namespace EOToolsWeb.Api.Services.GitManager;

public class GitManagerService : IGitManagerService
{
    public string FolderPath { get; set; } = Path.Combine("Data", "DataRepo");

    public async Task Initialize()
    {
        await Pull();
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
