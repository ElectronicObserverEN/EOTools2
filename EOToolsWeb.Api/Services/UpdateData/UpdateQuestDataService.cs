using EOTools.Models;
using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Shared.Quests;
using EOToolsWeb.Shared.Updates;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using EOToolsWeb.Shared.Translations;

namespace EOToolsWeb.Api.Services.UpdateData;

public class UpdateQuestDataService(IGitManagerService git, EoToolsDbContext db, JsonSerializerOptions options, DatabaseSyncService databaseSyncService) : TranslationUpdateService
{
    private IGitManagerService GitManager { get; } = git;
    private DatabaseSyncService DatabaseSyncService { get; } = databaseSyncService;
    private EoToolsDbContext Database => db;
    private JsonSerializerOptions SerializationOptions => options;

    private string UpdateFilePath => Path.Combine(GitManager.FolderPath, "Translations", "en-US", "update.json");
    private string UpdateDataFilePath => Path.Combine(GitManager.FolderPath, "update.json");
    public string TrackersFilePath => Path.Combine(GitManager.FolderPath, "Data", "QuestTrackers.json");
    public string QuestsTranslationsFilePath => Path.Combine(GitManager.FolderPath, "Translations", "en-US", "quest.json");

    public async Task UpdateQuestTranslations()
    {
        // --- Stage & push
        await GitManager.Pull();

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(UpdateFilePath));

        if (updateJson?["quest"] is null) return;

        string version = (int.Parse(updateJson["quest"].GetValue<string>()) + 1).ToString();

        Dictionary<string, object> toSerialize = new()
        {
            { "version", version }
        };

        updateJson["quest"] = version;

        List<QuestModel> questlist = Database.Quests
            .AsEnumerable()
            .Where(quest => !HasQuestEnded(quest))
            .OrderBy(quest => quest.ApiId)
            .ToList();

        foreach (QuestModel _quest in questlist)
        {
            toSerialize.Add(_quest.ApiId.ToString(), new QuestData(_quest.ApiId)
            {
                Code = _quest.Code,
                DescEN = _quest.DescEN,
                DescJP = _quest.DescJP,
                NameEN = _quest.NameEN,
                NameJP = _quest.NameJP,
            });
        }
        
        foreach (Language lang in OtherLanguagesTyped)
        {
            await UpdateOtherLanguage(lang);
        }

        await DatabaseSyncService.StageDatabaseChangesToGit();

        await File.WriteAllTextAsync(QuestsTranslationsFilePath, JsonSerializer.Serialize(toSerialize, SerializationOptions), Encoding.UTF8);
        await File.WriteAllTextAsync(UpdateFilePath, JsonSerializer.Serialize(updateJson, SerializationOptions), Encoding.UTF8);
        
        await GitManager.Push($"Quests - {version}");

    }

    private bool HasQuestEnded(QuestModel quest)
    {
        if (quest.RemovedOnUpdateId is null) return false;

        return IsQuestEndUpdateStarted(quest);
    }

    private bool IsQuestEndUpdateStarted(QuestModel quest)
    {
        if (quest.RemovedOnUpdateId is null) return false;

        UpdateModel? update = Database.Updates.Find(quest.RemovedOnUpdateId);

        if (update is null) return false;

        return !update.UpdateIsComing() || update.UpdateInProgress();
    }

    private async Task UpdateOtherLanguage(Language language)
    {
        string updatePath = UpdateFilePath.Replace("en-US", language.GetCulture());
        string questPath = QuestsTranslationsFilePath.Replace("en-US", language.GetCulture());

        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(updatePath));

        if (updateJson?["quest"] is null) return;

        string version = (int.Parse(updateJson["quest"].GetValue<string>()) + 1).ToString();

        Dictionary<string, object> toSerialize = new()
        {
            { "version", version },
        };

        updateJson["quest"] = version;
        
        List<QuestModel> questlist = Database.Quests
            .AsEnumerable()
            .Where(quest => !HasQuestEnded(quest))
            .OrderBy(quest => quest.ApiId)
            .ToList();
        
        List<QuestTitleTranslationModel> titles = await Database.QuestTitleTranslations
            .Include(nameof(QuestTitleTranslationModel.Translations))
            .ToListAsync();

        List<QuestDescriptionTranslationModel> descriptions = await Database.QuestDescriptionTranslations
            .Include(nameof(QuestDescriptionTranslationModel.Translations))
            .ToListAsync();

        await UpdateMissingTranslations(language, questPath);

        foreach (QuestModel quest in questlist)
        {
            QuestTitleTranslationModel? title = titles.Find(q => q.QuestId == quest.Id);
            QuestDescriptionTranslationModel? desc = descriptions.Find(q => q.QuestId == quest.Id);

            toSerialize.Add(quest.ApiId.ToString(), new QuestData(quest.ApiId)
            {
                Code = quest.Code,
                DescEN = desc?.Translations.Find(t => t.Language == language)?.Translation ?? "",
                DescJP = quest.DescJP,
                NameEN = title?.Translations.Find(t => t.Language == language)?.Translation ?? "",
                NameJP = quest.NameJP,
            });
        }

        await File.WriteAllTextAsync(questPath, JsonSerializer.Serialize(toSerialize, SerializationOptions), Encoding.UTF8);
        await File.WriteAllTextAsync(updatePath, JsonSerializer.Serialize(updateJson, SerializationOptions), Encoding.UTF8);
    }

    private async Task UpdateMissingTranslations(Language lang, string questPath)
    {
        List<QuestData> translations = await GetDataFromQuestsTranslations(questPath) ?? [];

        List<QuestTitleTranslationModel> titles = await Database.QuestTitleTranslations
            .Include(nameof(QuestTitleTranslationModel.Translations))
            .ToListAsync();

        List<QuestDescriptionTranslationModel> descriptions = await Database.QuestDescriptionTranslations
            .Include(nameof(QuestDescriptionTranslationModel.Translations))
            .ToListAsync();

        List<QuestModel> questlist = Database.Quests
            .AsEnumerable()
            .Where(quest => !HasQuestEnded(quest))
            .OrderBy(quest => quest.ApiId)
            .ToList();

        foreach (QuestModel quest in questlist)
        {
            QuestTitleTranslationModel? title = titles.Find(q => q.QuestId == quest.Id);
            QuestDescriptionTranslationModel? desc = descriptions.Find(q => q.QuestId == quest.Id);

            if (title is not null && desc is not null)
            {
                if (title.Translations.All(t => t.Language != lang))
                {
                    TranslationModel questTranslation = new()
                    {
                        Language = lang,
                        Translation = translations.Find(q => q.Code == quest.Code)?.NameEN ?? "",
                    };

                    title.Translations.Add(questTranslation);

                    Database.Update(title);
                    Database.Add(questTranslation);
                }

                if (desc.Translations.All(t => t.Language != lang))
                {
                    TranslationModel questTranslation = new()
                    {
                        Language = lang,
                        Translation = translations.Find(q => q.Code == quest.Code)?.DescEN ?? "",
                    };

                    desc.Translations.Add(questTranslation);

                    Database.Update(desc);
                    Database.Add(questTranslation);
                }
            }
        }

        await Database.SaveChangesAsync();
    }

    private async Task<List<QuestData>?> GetDataFromQuestsTranslations(string path)
    {
        JsonObject? json = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(path));

        if (json is null) return null;

        List<QuestData> listOfQuests = new();

        foreach (KeyValuePair<string, JsonNode?> quest in json.Skip(1))
        {
            QuestData? newQuest = JsonSerializer.Deserialize<QuestData>(quest.Value.ToString());

            if (newQuest != null)
                listOfQuests.Add(newQuest);
        }

        return listOfQuests;
    }

    public async Task UpdateQuestTrackers()
    {
        // --- Stage & push
        await GitManager.Pull();

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(UpdateDataFilePath));

        if (updateJson?["QuestTrackers"] is null) return;

        int version = updateJson["QuestTrackers"].GetValue<int>() + 1;

        updateJson["QuestTrackers"] = version;

        List<QuestModel> questlist = Database.Quests
            .AsEnumerable()
            .Where(quest => !HasQuestEnded(quest))
            .Where(quest => !string.IsNullOrEmpty(quest.Tracker))
            .OrderBy(quest => quest.ApiId)
            .ToList();

        string toSerialize = string.Join(",\n\t", questlist.Select(q => q.Tracker));
        toSerialize = $"[\n\t{toSerialize}\n]";

        await DatabaseSyncService.StageDatabaseChangesToGit();

        await File.WriteAllTextAsync(TrackersFilePath, toSerialize, Encoding.UTF8);

        await File.WriteAllTextAsync(UpdateDataFilePath, JsonSerializer.Serialize(updateJson, SerializationOptions), Encoding.UTF8);

        await GitManager.Push($"Quest trackers - {version}");
    }
}
