using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using EOTools.Models.ShipTranslation;
using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Shared.Ships;

namespace EOToolsWeb.Api.Services.UpdateData;

public class UpdateShipDataService(IGitManagerService git, EoToolsDbContext db, JsonSerializerOptions options, DatabaseSyncService databaseSyncService) : TranslationUpdateService
{
    private IGitManagerService GitManager { get; } = git;
    private DatabaseSyncService DatabaseSyncService { get; } = databaseSyncService;
    private EoToolsDbContext Database => db;
    private JsonSerializerOptions SerializationOptions => options;

    private string UpdateFilePath => Path.Combine(GitManager.FolderPath, "Translations", "en-US", "update.json");
    public string ShipTranslationsFilePath => Path.Combine(GitManager.FolderPath, "Translations", "en-US", "ship.json");

    private Dictionary<string, string> Suffixes { get; set; } = [];

    public async Task UpdateShipTranslations()
    {
        // --- Stage & push
        await GitManager.Pull();

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(UpdateFilePath));

        if (updateJson?["ship"] is null) return;

        string version = (int.Parse(updateJson["ship"].GetValue<string>()) + 1).ToString();

        ShipTranslationModel? toSerialize = JsonSerializer.Deserialize<ShipTranslationModel>(await File.ReadAllTextAsync(ShipTranslationsFilePath));

        if (toSerialize is null) return;

        Suffixes = toSerialize.Suffixes;

        await UpdateOtherLanguage("en-US");

        foreach (string lang in OtherLanguages)
        {
            await UpdateOtherLanguage(lang);
        }
        
        await DatabaseSyncService.StageDatabaseChangesToGit();
        
        await GitManager.Push($"Ships - {version}");
    }

    public async Task UpdateOtherLanguage(string language)
    {
        string updatePath = UpdateFilePath.Replace("en-US", language);
        string shipPath = ShipTranslationsFilePath.Replace("en-US", language);

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(updatePath));

        if (updateJson?["ship"] is null) return;

        string version = (int.Parse(updateJson["ship"].GetValue<string>()) + 1).ToString();

        updateJson["ship"] = version;

        List<ShipModel> ships = Database.Ships
            .AsEnumerable()
            .OrderBy(ship => ship.ApiId)
            .ToList();

        List<ShipClassModel> classes = Database.ShipClasses
            .AsEnumerable()
            .OrderBy(shipClass => shipClass.ApiId)
            .ToList();

        ShipTranslationModel? toSerialize = JsonSerializer.Deserialize<ShipTranslationModel>(await File.ReadAllTextAsync(shipPath));

        if (toSerialize is null) return;

        toSerialize.Version = version;
        Dictionary<string, string> translationsShips = toSerialize.Ships;
        Dictionary<string, string> translationsClasses = toSerialize.Classes;
        
        foreach (ShipModel model in ships)
        {
            if (!translationsShips.ContainsKey(model.NameJP) && ShouldBeTranslated(model))
            {
                translationsShips.Add(model.NameJP, model.NameEN);
            }
        }

        foreach (ShipClassModel model in classes)
        {
            if (!translationsClasses.ContainsKey(model.NameJapanese))
            {
                translationsClasses.Add(model.NameJapanese, model.NameEnglish);
            }
        }

        await File.WriteAllTextAsync(shipPath, JsonSerializer.Serialize(toSerialize, SerializationOptions), Encoding.UTF8);
        await File.WriteAllTextAsync(updatePath, JsonSerializer.Serialize(updateJson, SerializationOptions), Encoding.UTF8);
    }

    private bool ShouldBeTranslated(ShipModel model)
    {
        if (model.NameJP == model.NameEN) return false;

        foreach (KeyValuePair<string, string> suffix in Suffixes)
        {
            if (model.NameJP.Contains(suffix.Key) && model.NameEN.Contains(suffix.Value))
            {
                return false;
            }
        }

        return true;
    }
}
