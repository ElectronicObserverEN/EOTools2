using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Translations;
using System.Text.Json.Nodes;
using System.Text.Json;
using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.GitManager;
using Microsoft.EntityFrameworkCore;
using System.Text;
using EOToolsWeb.Shared.Maps;

namespace EOToolsWeb.Api.Services.UpdateData;

public class OperationUpdateService(IGitManagerService git, EoToolsDbContext db, JsonSerializerOptions options, DatabaseSyncService databaseSyncService) : TranslationUpdateService
{
    private IGitManagerService GitManager { get; } = git;
    private DatabaseSyncService DatabaseSyncService { get; } = databaseSyncService;
    private EoToolsDbContext Database => db;
    private JsonSerializerOptions SerializationOptions => options;

    private string UpdateFilePath => Path.Combine(GitManager.FolderPath, "Translations", "en-US", "update.json");
    public string ShipTranslationsFilePath => Path.Combine(GitManager.FolderPath, "Translations", "en-US", "operation.json");

    public async Task PushTranslations()
    {
        await GitManager.Pull();

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(UpdateFilePath));

        if (updateJson?["operation"] is null) return;

        string version = (int.Parse(updateJson["operation"].GetValue<string>()) + 1).ToString();

        ShipTranslationModel? toSerialize = JsonSerializer.Deserialize<ShipTranslationModel>(await File.ReadAllTextAsync(ShipTranslationsFilePath));

        if (toSerialize is null) return;

        foreach (Language lang in LanguageExtensions.AllLanguagesTyped)
        {
            await UpdateOtherLanguage(lang);
        }

        await DatabaseSyncService.StageDatabaseChangesToGit();

        await GitManager.Push($"Map translations - {version}");
    }

    public async Task UpdateOtherLanguage(Language language)
    {
        string updatePath = UpdateFilePath.Replace(Language.English.GetCulture(), language.GetCulture());
        string operationFilePath = ShipTranslationsFilePath.Replace(Language.English.GetCulture(), language.GetCulture());

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(updatePath));

        if (updateJson?["operation"] is null) return;

        string version = (int.Parse(updateJson["operation"].GetValue<string>()) + 1).ToString();

        updateJson["operation"] = version;

        List<MapNameTranslationModel> maps = Database.Maps
            .Include(nameof(MapNameTranslationModel.Translations))
            .ToList();

        List<FleetNameTranslationModel> fleets = Database.Fleets
            .Include(nameof(FleetNameTranslationModel.Translations))
            .ToList();

        MapsTranslationModel? toSerialize = JsonSerializer.Deserialize<MapsTranslationModel>(await File.ReadAllTextAsync(operationFilePath));

        if (toSerialize is null) return;

        toSerialize.Version = version;
        Dictionary<string, string> translationsMaps = toSerialize.Maps;
        Dictionary<string, string> translationsFleets = toSerialize.Fleets;

        foreach (MapNameTranslationModel model in maps)
        {
            TranslationModel? jp = model.Translations.FirstOrDefault(t => t.Language is Language.Japanese);
            TranslationModel? tl = model.Translations.FirstOrDefault(t => t.Language == language);

            if (jp?.Translation is { } jpTranslation && !translationsMaps.ContainsKey(jpTranslation))
            {
                translationsMaps.Add(jpTranslation, tl?.Translation ?? "");
            }
        }

        foreach (FleetNameTranslationModel model in fleets)
        {
            TranslationModel? jp = model.Translations.FirstOrDefault(t => t.Language is Language.Japanese);
            TranslationModel? tl = model.Translations.FirstOrDefault(t => t.Language == language);

            if (jp is not { } jpTranslation) continue;
            if (tl is not { } langTranslation) continue;

            if (!translationsFleets.ContainsKey(jpTranslation.Translation))
            {
                translationsFleets.Add(jpTranslation.Translation, tl.Translation);
            }
            else if (langTranslation.IsPendingChange)
            {
                translationsFleets[jpTranslation.Translation] = tl.Translation;
            }
                
            langTranslation.IsPendingChange = false;
            Database.Update(langTranslation);
        }
        
        await File.WriteAllTextAsync(operationFilePath, JsonSerializer.Serialize(toSerialize, SerializationOptions), Encoding.UTF8);
        await File.WriteAllTextAsync(updatePath, JsonSerializer.Serialize(updateJson, SerializationOptions), Encoding.UTF8);
        
        await Database.SaveChangesAsync();
    }
}