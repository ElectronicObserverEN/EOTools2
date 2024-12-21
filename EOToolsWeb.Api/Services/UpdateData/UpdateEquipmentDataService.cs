using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using EOTools.Models;
using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.Quests;
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
        
        await UpdateOtherLanguage(Language.English);

        foreach (Language lang in OtherLanguagesTyped)
        {
            await UpdateOtherLanguage(lang);
        }

        await DatabaseSyncService.StageDatabaseChangesToGit();

        await GitManager.Push($"Equipments - {version}");
    }

    private async Task UpdateOtherLanguage(Language language)
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

        List<EquipmentModel> equipments = Database.Equipments
            .AsEnumerable()
            .OrderBy(eq => eq.ApiId)
            .ToList();

        await UpdateMissingTranslations(language, translationPath);

        List<EquipmentTranslationModel> tls = await GetAllEquipmentTranslations();

        JsonObject? prevTranslations = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(translationPath));
        toSerialize.Add("equiptype", prevTranslations["equiptype"]);
        Dictionary<string, string> equipmentsToSerialize = [];
        toSerialize.Add("equipment", equipmentsToSerialize);

        foreach (EquipmentTranslationModel model in tls)
        {
            string jp = model.Translations.Find(t => t.Language == Language.Japanese)?.Translation ?? "";
            string tl = model.Translations.Find(t => t.Language == language)?.Translation ?? "";

            if (!equipmentsToSerialize.ContainsKey(jp))
            {
                equipmentsToSerialize.Add(jp, tl);
            }
        }

        await File.WriteAllTextAsync(translationPath, JsonSerializer.Serialize(toSerialize, SerializationOptions), Encoding.UTF8);
        await File.WriteAllTextAsync(updatePath, JsonSerializer.Serialize(updateJson, SerializationOptions), Encoding.UTF8);
    }

    public async Task<List<EquipmentTranslationModel>> GetAllEquipmentTranslations()
    {
        List<EquipmentModel> equipments = await Database.Equipments
            .ToListAsync();

        List<EquipmentTranslationModel> tls = await Database.EquipmentTranslations
            .Include(nameof(EquipmentTranslationModel.Translations))
            .ToListAsync();

        foreach (EquipmentModel eq in equipments)
        {
            EquipmentTranslationModel? equipmentTranslation = tls.Find(e => e.EquipmentId == eq.Id);

            if (equipmentTranslation is null)
            {
                equipmentTranslation = new()
                {
                    EquipmentId = eq.Id,
                    Translations = new(),
                };

                Database.EquipmentTranslations.Add(equipmentTranslation);
            }
        }

        await Database.SaveChangesAsync();

        tls = await Database.EquipmentTranslations
            .Include(nameof(EquipmentTranslationModel.Translations))
            .ToListAsync();

        foreach (EquipmentModel eq in equipments)
        {
            EquipmentTranslationModel? equipmentTranslation = tls.Find(e => e.EquipmentId == eq.Id);

            if (equipmentTranslation is not null)
            {
                equipmentTranslation.Translations.Add(new()
                {
                    Language = Language.English,
                    Translation = eq.NameEN,
                });

                equipmentTranslation.Translations.Add(new()
                {
                    Language = Language.Japanese,
                    Translation = eq.NameJP,
                });
            }
        }

        return tls;
    }

    private async Task UpdateMissingTranslations(Language lang, string translationPath)
    {
        Dictionary<string, string> translations = await LoadEquipmentTranslations(translationPath);

        List<EquipmentTranslationModel> tls = await Database.EquipmentTranslations
            .Include(nameof(EquipmentTranslationModel.Translations))
            .ToListAsync();

        List<EquipmentModel> equipments = Database.Equipments
            .AsEnumerable()
            .OrderBy(eq => eq.ApiId)
            .ToList();

        foreach (EquipmentModel equipment in equipments)
        {
            EquipmentTranslationModel? tl = tls.Find(q => q.EquipmentId == equipment.Id);

            if (tl is not null)
            {
                if (tl.Translations.All(t => t.Language != lang))
                {
                    if (!translations.TryGetValue(equipment.NameJP, out string? oldValue))
                    {
                        oldValue = "";
                    }

                    TranslationModel questTranslation = new()
                    {
                        Language = lang,
                        Translation = oldValue,
                    };

                    tl.Translations.Add(questTranslation);

                    Database.Update(tl);
                    Database.Add(questTranslation);
                }
            }
        }

        await Database.SaveChangesAsync();
    }

    private async Task<Dictionary<string, string>> LoadEquipmentTranslations(string path)
    {
        JsonObject? translationsJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(path));
        JsonObject? jsonEquipList = translationsJson?["equipment"] as JsonObject;

        Dictionary<string, string> results = new();

        if (jsonEquipList is null) return results;

        // --- Equips
        foreach (KeyValuePair<string, JsonNode>? equip in jsonEquipList)
        {
            string? nameEN = equip?.Key;
            string? nameJP = equip?.Value?.ToString();

            results.Add(nameEN ?? "", nameJP ?? "");
        }

        return results;
    }
}
