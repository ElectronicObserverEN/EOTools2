using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Translations;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Services.UpdateData;

public class UpdateShipDataService(IGitManagerService git, EoToolsDbContext db, JsonSerializerOptions options, DatabaseSyncService databaseSyncService) : TranslationUpdateService
{
    private IGitManagerService GitManager { get; } = git;
    private DatabaseSyncService DatabaseSyncService { get; } = databaseSyncService;
    private EoToolsDbContext Database => db;
    private JsonSerializerOptions SerializationOptions => options;

    private string UpdateFilePath => Path.Combine(GitManager.FolderPath, "Translations", "en-US", "update.json");
    public string ShipTranslationsFilePath => Path.Combine(GitManager.FolderPath, "Translations", "en-US", "ship.json");
    
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

        foreach (Language lang in AllLanguagesTyped)
        {
            await UpdateOtherLanguage(lang);
        }
        
        await DatabaseSyncService.StageDatabaseChangesToGit();
        
        await GitManager.Push($"Ships - {version}");
    }

    public async Task UpdateOtherLanguage(Language language)
    {
        string updatePath = UpdateFilePath.Replace(Language.English.GetCulture(), language.GetCulture());
        string shipPath = ShipTranslationsFilePath.Replace(Language.English.GetCulture(), language.GetCulture());

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(updatePath));

        if (updateJson?["ship"] is null) return;

        string version = (int.Parse(updateJson["ship"].GetValue<string>()) + 1).ToString();

        updateJson["ship"] = version;

        List<ShipNameTranslationModel> ships = Database.ShipTranslations
            .Include(nameof(ShipNameTranslationModel.Translations))
            .ToList();

        List<ShipSuffixTranslationModel> suffixes = Database.ShipSuffixTranslations
            .Include(nameof(ShipNameTranslationModel.Translations))
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
        Dictionary<string, string> translationsSuffixes = toSerialize.Suffixes;

        foreach (ShipNameTranslationModel model in ships)
        {
            TranslationModel? jp = model.Translations.FirstOrDefault(t => t.Language is Language.Japanese);
            TranslationModel? tl = model.Translations.FirstOrDefault(t => t.Language == language);

            if (jp?.Translation is {} jpTranslation && !translationsShips.ContainsKey(jpTranslation))
            {
                translationsShips.Add(jpTranslation, tl?.Translation ?? "");
            }
        }

        foreach (ShipSuffixTranslationModel model in suffixes)
        {
            TranslationModel? jp = model.Translations.FirstOrDefault(t => t.Language is Language.Japanese);
            TranslationModel? tl = model.Translations.FirstOrDefault(t => t.Language == language);

            if (jp?.Translation is { } jpTranslation && !translationsSuffixes.ContainsKey(jpTranslation))
            {
                translationsSuffixes.Add(jpTranslation, tl?.Translation ?? "");
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
}
