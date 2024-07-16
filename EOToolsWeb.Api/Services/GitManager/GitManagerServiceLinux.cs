using System.Diagnostics;

namespace EOToolsWeb.Api.Services.GitManager;

public class GitManagerServiceLinux : IGitManagerService
{
    public string FolderPath { get; set; } = Path.Combine("Data", "DataRepo");

    private string FolderPathScripts { get; set; } = Path.Combine("Data");

    public async Task Initialize()
    {
        await Pull();
    }

    private async Task ExecuteCommand(string filename) => await ExecuteCommand(filename, "");

    private async Task ExecuteCommand(string filename, string args)
    {
        await Process.Start(new ProcessStartInfo()
        {
            FileName = Path.Combine(FolderPathScripts, filename),
            Arguments = args
        })!.WaitForExitAsync();
    }

    public async Task Pull()
    {
        await ExecuteCommand("pull.sh");
    }

    public async Task Push(string commitMessage)
    {
        await ExecuteCommand("push.sh", commitMessage);
    }
}
