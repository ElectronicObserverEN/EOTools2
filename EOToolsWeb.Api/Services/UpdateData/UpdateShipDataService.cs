using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Translations;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

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
        await GitManager.Pull();

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(UpdateFilePath));

        if (updateJson?["ship"] is null) return;

        string version = (int.Parse(updateJson["ship"].GetValue<string>()) + 1).ToString();

        ShipTranslationModel? toSerialize = JsonSerializer.Deserialize<ShipTranslationModel>(await File.ReadAllTextAsync(ShipTranslationsFilePath));

        if (toSerialize is null) return;


        List<ShipNameTranslationModel> ships = Database.ShipTranslations
            .Where(tl => tl.Translations.Any(tlModel => tlModel.IsPendingChange))
            .Include(nameof(ShipNameTranslationModel.Translations))
            .ToList();

        List<ShipSuffixTranslationModel> suffixes = Database.ShipSuffixTranslations
            .Where(tl => tl.Translations.Any(tlModel => tlModel.IsPendingChange))
            .Include(nameof(ShipNameTranslationModel.Translations))
            .ToList();

        foreach (Language lang in AllLanguagesTyped)
        {
            await UpdateOtherLanguage(lang, ships, suffixes);
        }

        await DatabaseSyncService.StageDatabaseChangesToGit();

        await GitManager.Push($"Ships - {version}");

        foreach (TranslationModel tlWithChange in ships.SelectMany(tl => tl.Translations.Where(tlModel => tlModel.IsPendingChange)))
        {
            tlWithChange.IsPendingChange = false;
            Database.Update(tlWithChange);
        }

        foreach (TranslationModel tlWithChange in suffixes.SelectMany(tl => tl.Translations.Where(tlModel => tlModel.IsPendingChange)))
        {
            tlWithChange.IsPendingChange = false;
            Database.Update(tlWithChange);
        }

        await Database.SaveChangesAsync();
    }

    public async Task UpdateOtherLanguage(Language language, List<ShipNameTranslationModel> ships, List<ShipSuffixTranslationModel> suffixes)
    {
        string updatePath = UpdateFilePath.Replace(Language.English.GetCulture(), language.GetCulture());
        string shipPath = ShipTranslationsFilePath.Replace(Language.English.GetCulture(), language.GetCulture());

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(updatePath));

        if (updateJson?["ship"] is null) return;

        string version = (int.Parse(updateJson["ship"].GetValue<string>()) + 1).ToString();

        updateJson["ship"] = version;

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

            if (jp is not null && tl is not null)
            {
                if (!translationsShips.TryAdd(jp.Translation, tl.Translation))
                {
                    translationsShips[jp.Translation] = tl.Translation;
                }
            }
        }

        foreach (ShipSuffixTranslationModel model in suffixes)
        {
            TranslationModel? jp = model.Translations.FirstOrDefault(t => t.Language is Language.Japanese);
            TranslationModel? tl = model.Translations.FirstOrDefault(t => t.Language == language);

            if (jp is not null && tl is not null)
            {
                if (!translationsSuffixes.TryAdd(jp.Translation, tl.Translation))
                {
                    translationsSuffixes[jp.Translation] = tl.Translation;
                }
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
