using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Shared.Ships;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EOToolsWeb.Api.Services.UpdateData;

public class DatabaseSyncService(IGitManagerService git, EoToolsDbContext db, JsonSerializerOptions options)
{
    private EoToolsDbContext Database => db;
    private JsonSerializerOptions SerializationOptions => options;

    private string UpdatesFilePath => Path.Combine(git.FolderPath, "Data", "Updates.json");
    private string EventsFilePath => Path.Combine(git.FolderPath, "Data", "Events.json");
    private string DataBaseRepoPath => Path.Combine(git.FolderPath, "Data", "Data.db");
    private string ShipFilePath => Path.Combine(git.FolderPath, "Data", "Ships.json");
    private string ShipClassFilePath => Path.Combine(git.FolderPath, "Data", "ShipClass.json");

    public static string DataBaseLocalPath => Path.Combine("Data", "EOTools.db");

    public async Task StageDatabaseChangesToGit()
    {
        await File.WriteAllTextAsync(UpdatesFilePath, JsonSerializer.Serialize(Database.Updates.ToList(), SerializationOptions));
        await File.WriteAllTextAsync(EventsFilePath, JsonSerializer.Serialize(Database.Events.ToList(), SerializationOptions));
        await File.WriteAllTextAsync(ShipFilePath, JsonSerializer.Serialize(Database.Ships.Include(nameof(ShipModel.ShipClass)).ToList(), SerializationOptions));
        await File.WriteAllTextAsync(ShipClassFilePath, JsonSerializer.Serialize(Database.ShipClasses.ToList(), SerializationOptions));

        File.Copy(DataBaseLocalPath, DataBaseRepoPath, true);
    }
}
