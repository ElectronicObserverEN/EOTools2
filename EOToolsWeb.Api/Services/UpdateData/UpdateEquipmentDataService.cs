using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Shared.Equipments;

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
        // --- Stage & push
        await GitManager.Pull();

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(UpdateFilePath));
        string version = (int.Parse(updateJson["equipment"].GetValue<string>()) + 1).ToString();
        
        await UpdateOtherLanguage("en-US");

        foreach (string lang in OtherLanguages)
        {
            await UpdateOtherLanguage(lang);
        }

        await DatabaseSyncService.StageDatabaseChangesToGit();

        await GitManager.Push($"Equipments - {version}");
    }

    private async Task UpdateOtherLanguage(string language)
    {
        string updatePath = UpdateFilePath.Replace("en-US", language);
        string translationPath = EquipmentTranslationsFilePath.Replace("en-US", language);

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

        Dictionary<string, string> translations = await LoadEquipmentTranslations(translationPath);
        toSerialize.Add("equipment", translations);

        // TODO : eqtype manager
        JsonObject? prevTranslations = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(translationPath));
        toSerialize.Add("equiptype", prevTranslations["equiptype"]);

        foreach (EquipmentModel model in equipments)
        {
            if (!translations.ContainsKey(model.NameJP))
            {
                translations.Add(model.NameJP, model.NameEN);
            }
        }

        await File.WriteAllTextAsync(translationPath, JsonSerializer.Serialize(toSerialize, SerializationOptions), Encoding.UTF8);
        await File.WriteAllTextAsync(updatePath, JsonSerializer.Serialize(updateJson, SerializationOptions), Encoding.UTF8);
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
