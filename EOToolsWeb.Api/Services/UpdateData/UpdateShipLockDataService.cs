using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Models.ShipLocks;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Shared.ShipLocks;

namespace EOToolsWeb.Api.Services.UpdateData;

public class UpdateShipLockDataService(EoToolsDbContext db, JsonSerializerOptions jsonOptions, IGitManagerService git) : TranslationUpdateService
{
    private EoToolsDbContext Database { get; } = db;
    private JsonSerializerOptions JsonSerializerOptions { get; } = jsonOptions;
    private IGitManagerService GitManagerService { get; } = git;

    private string ShipLockDataPath => Path.Combine(GitManagerService.FolderPath, "Data", "Locks.json");
    private string UpdateDataFilePath => Path.Combine(GitManagerService.FolderPath, "update.json");
    private string ShipLockTranslationsPath => Path.Combine(GitManagerService.FolderPath, "Translations", "en-US", "Locks.json");
    private string UpdateTranslationFilePath => Path.Combine(GitManagerService.FolderPath, "Translations", "en-US", "update.json");

    public async Task UpdateLockData()
    {
        await GitManagerService.Pull();
        
        ShipLocksPhasesModel lockAndPhases = new()
        {
            Locks = GetLocks(),
            Phases = GetPhases(),
        };

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(UpdateDataFilePath));

        if (updateJson?["Locks"] is null) return;

        int version = updateJson["Locks"].GetValue<int>() + 1;

        updateJson["Locks"] = version;

        await File.WriteAllTextAsync(ShipLockDataPath, JsonSerializer.Serialize(lockAndPhases, JsonSerializerOptions), Encoding.UTF8);
        await File.WriteAllTextAsync(UpdateDataFilePath, JsonSerializer.Serialize(updateJson, JsonSerializerOptions), Encoding.UTF8);

        await GitManagerService.Push($"Locks data - {version}");
    }

    private List<ShipLockModelForDataRepo> GetLocks()
    {
        int lastEventId = Database.Events.OrderByDescending(ev => ev.ApiId).First().Id;

        return Database.Locks
           .Where(locks => locks.EventId == lastEventId)
           .OrderBy(locks => locks.ApiId)
           .Select(locks => new ShipLockModelForDataRepo(locks))
           .ToList();
    }

    private List<ShipLockPhaseModelForDataRepo> GetPhases()
    {
        int lastEventId = Database.Events.OrderByDescending(ev => ev.ApiId).First().Id;

        return Database.LockPhases
            .Where(phase => phase.EventId == lastEventId)
            .OrderBy(phase => phase.SortId)
            .Select(locks => new ShipLockPhaseModelForDataRepo(locks))
            .ToList();
    }

    public async Task UpdateLockTranslations()
    {
        await GitManagerService.Pull();
        
        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(UpdateTranslationFilePath));

        if (updateJson?["Locks"] is null) return;

        int version = updateJson["Locks"].GetValue<int>() + 1;

        updateJson["Locks"] = version;

        int lastEventId = Database.Events.OrderByDescending(ev => ev.ApiId).First().Id;
        List<ShipLockModel> locks = Database.Locks.Where(locks => locks.EventId == lastEventId).OrderBy(locks => locks.ApiId).ToList();

        foreach (string language in AllLanguages)
        {
            await UpdateLockTranslation(language, locks);
        }

        await GitManagerService.Push($"Locks translations - {version}");
    }

    private async Task UpdateLockTranslation(string lang, List<ShipLockModel> locks)
    {
        string translationPath = ShipLockTranslationsPath.Replace("en-US", lang);
        string updatePath = UpdateTranslationFilePath.Replace("en-US", lang);

        JsonObject? currentTranslations = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(translationPath));

        if (currentTranslations is null) return;

        foreach (ShipLockModel translationData in locks)
        {
            if (lang is "en-US" || !currentTranslations.ContainsKey(translationData.NameJapanese))
            {
                currentTranslations[translationData.NameJapanese] = translationData.NameEnglish;
            }
        }

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(updatePath));

        if (updateJson?["Locks"] is null) return;

        int version = updateJson["Locks"].GetValue<int>() + 1;

        updateJson["Locks"] = version;

        await File.WriteAllTextAsync(translationPath, JsonSerializer.Serialize(currentTranslations, JsonSerializerOptions), Encoding.UTF8);
        await File.WriteAllTextAsync(updatePath, JsonSerializer.Serialize(updateJson, JsonSerializerOptions), Encoding.UTF8);

    }
}
