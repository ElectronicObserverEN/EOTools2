namespace EOToolsWeb.Api.Services.GitManager;

public interface IGitManagerService
{
    public string FolderPath { get; }

    public Task Initialize();

    public Task Pull();

    public Task Push(string commitMessage);
}
