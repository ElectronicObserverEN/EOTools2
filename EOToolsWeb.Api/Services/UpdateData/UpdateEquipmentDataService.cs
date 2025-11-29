using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.Translations;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Services.UpdateData;

public class UpdateEquipmentDataService(IGitManagerService git, EoToolsDbContext db, JsonSerializerOptions options, DatabaseSyncService databaseSyncService) : TranslationUpdateService
{
    private IGitManagerService GitManager { get; } = git;
    private DatabaseSyncService DatabaseSyncService { get; } = databaseSyncService;
    private EoToolsDbContext Database => db;
    private JsonSerializerOptions SerializationOptions => options;

    private string UpdateFilePath => Path.Combine(git.FolderPath, "Translations", "en-US", "update.json");
    public string EquipmentTranslationsFilePath => Path.Combine(git.FolderPath, "Translations", "en-US", "equipment.json");


    public async Task UpdateEquipmentTranslations()
    {
        await GitManager.Pull();

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(UpdateFilePath));
        string version = (int.Parse(updateJson["equipment"].GetValue<string>()) + 1).ToString();

        List<EquipmentTranslationModel> tls = await GetAllEquipmentTranslations(true);

        await UpdateOtherLanguage(Language.English, tls);

        foreach (Language lang in LanguageExtensions.OtherLanguagesTyped)
        {
            await UpdateOtherLanguage(lang, tls);
        }

        await DatabaseSyncService.StageDatabaseChangesToGit();

        await GitManager.Push($"Equipments - {version}");

        foreach (TranslationModel tlWithChange in tls.SelectMany(tl => tl.Translations.Where(tlModel => tlModel.IsPendingChange)))
        {
            tlWithChange.IsPendingChange = false;
            Database.Update(tlWithChange);
        }

        await Database.SaveChangesAsync();
    }

    private async Task UpdateOtherLanguage(Language language, List<EquipmentTranslationModel> tls)
    {
        string updatePath = UpdateFilePath.Replace("en-US", language.GetCulture());
        string translationPath = EquipmentTranslationsFilePath.Replace("en-US", language.GetCulture());

        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(updatePath));
        string version = (int.Parse(updateJson["equipment"].GetValue<string>()) + 1).ToString();

        Dictionary<string, object> toSerialize = new()
        {
            { "version", version },
        };

        updateJson["equipment"] = version;
        
        JsonObject? prevTranslations = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(translationPath));
        toSerialize.Add("equiptype", prevTranslations["equiptype"]);
        toSerialize.Add("equipment", prevTranslations["equipment"]);

        foreach (EquipmentTranslationModel model in tls)
        {
            string jp = model.Translations.Find(t => t.Language == Language.Japanese)?.Translation ?? "";
            string tl = model.Translations.Find(t => t.Language == language)?.Translation ?? "";

            prevTranslations["equipment"][jp] = tl;
        }

        await File.WriteAllTextAsync(translationPath, JsonSerializer.Serialize(toSerialize, SerializationOptions), Encoding.UTF8);
        await File.WriteAllTextAsync(updatePath, JsonSerializer.Serialize(updateJson, SerializationOptions), Encoding.UTF8);
    }

    public async Task<List<EquipmentTranslationModel>> GetAllEquipmentTranslations() => await GetAllEquipmentTranslations(false);

    public async Task<List<EquipmentTranslationModel>> GetAllEquipmentTranslations(bool onlyWithPendingChanges)
    {
        List<EquipmentModel> equipments = await Database.Equipments
            .ToListAsync();

        List<EquipmentTranslationModel> tls = await Database.EquipmentTranslations
            .Include(nameof(EquipmentTranslationModel.Translations))
            .Where(tl => !onlyWithPendingChanges || tl.Translations.Any(tlModel => tlModel.IsPendingChange))
            .ToListAsync();

        foreach (EquipmentTranslationModel equipmentTranslation in tls)
        {
            EquipmentModel? eq = equipments.Find(e => e.ApiId == equipmentTranslation.EquipmentId);

            if (eq is not null)
            {
                if (equipmentTranslation.Translations.All(tl => tl.Language != Language.English))
                {
                    equipmentTranslation.Translations.Add(new()
                    {
                        Language = Language.English,
                        Translation = eq.NameEN,
                    });
                }

                equipmentTranslation.Translations.Add(new()
                {
                    Language = Language.Japanese,
                    Translation = eq.NameJP,
                });
            }
        }

        return tls;
    }

    public async Task AddTranslation(EquipmentModel eq)
    {
        EquipmentTranslationModel tl = new()
        {
            EquipmentId = eq.ApiId,
            Translations = LanguageExtensions.AllLanguagesTyped
                .Select(lang => new TranslationModel()
                {
                    Language = lang,
                    Translation = eq.NameEN,
                    IsPendingChange = true,
                })
                .ToList(),
        };

        await Database.AddAsync(tl);
        await Database.SaveChangesAsync();
    }

    public async Task EditTranslation(EquipmentModel eq, string nameBefore)
    {
        EquipmentTranslationModel? tlFound = await Database.EquipmentTranslations
            .Include(tl => tl.Translations)
            .FirstOrDefaultAsync(tl => tl.EquipmentId == eq.Id);

        if (tlFound is null)
        {
            await AddTranslation(eq);
            return;
        }

        foreach (TranslationModel tlModel in tlFound.Translations.Where(tl => tl.Translation == nameBefore))
        {
            tlModel.Translation = eq.NameEN;
            tlModel.IsPendingChange = true;

            Database.Update(tlModel);
        }

        await Database.SaveChangesAsync();
    }
}
