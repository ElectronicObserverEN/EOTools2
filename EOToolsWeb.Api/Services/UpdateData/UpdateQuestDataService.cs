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
using EOToolsWeb.Shared.Equipments;

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
    public string TimeLimitedQuestFilePath => Path.Combine(GitManager.FolderPath, "Data", "TimeLimitedQuests.json");
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

        List<QuestTitleTranslationModel> titles = await Database.QuestTitleTranslations
            .Include(nameof(QuestTitleTranslationModel.Translations))
            .Where(tl => tl.Translations.Any(tlModel => tlModel.IsPendingChange))
            .ToListAsync();

        List<QuestDescriptionTranslationModel> descriptions = await Database.QuestDescriptionTranslations
            .Include(nameof(QuestDescriptionTranslationModel.Translations))
            .Where(tl => tl.Translations.Any(tlModel => tlModel.IsPendingChange))
            .ToListAsync();

        foreach (Language lang in LanguageExtensions.AllLanguagesTyped)
        {
            await UpdateOneLanguage(lang, questlist, titles, descriptions);
        }

        await UpdateLimeLimitedQuestData(questlist);

        await DatabaseSyncService.StageDatabaseChangesToGit();
        
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

    private bool IsTimeLimitedQuest(QuestModel quest)
    {
        if (quest.SeasonId is null) return false;

        return true;
    }

    private async Task UpdateLimeLimitedQuestData(List<QuestModel> questlist)
    {
        List<TimeLimitedQuestData> timeLimitedQuestList = [];

        foreach (QuestModel quest in questlist.Where(IsTimeLimitedQuest))
        {
            DateTime? endTime = null;

            if (quest.RemovedOnUpdateId is not null)
            {
                UpdateModel? update = Database.Updates.Find(quest.RemovedOnUpdateId);

                if (update?.UpdateDate is { } date && update.UpdateEndTime is { } end)
                {
                    endTime = date.Date.Add(end);
                }
            }

            timeLimitedQuestList.Add(new TimeLimitedQuestData()
            {
                ApiId = quest.ApiId,
                EndTime = endTime,
                ProgressResetsDaily = quest.ProgressResetsDaily ?? false,
            });
        }

        await File.WriteAllTextAsync(TimeLimitedQuestFilePath, JsonSerializer.Serialize(timeLimitedQuestList, SerializationOptions), Encoding.UTF8);
    }

    private async Task UpdateOneLanguage(Language language, List<QuestModel> questlist, List<QuestTitleTranslationModel> titles, List<QuestDescriptionTranslationModel> descriptions)
    {
        string updatePath = UpdateFilePath.Replace("en-US", language.GetCulture());
        string questPath = QuestsTranslationsFilePath.Replace("en-US", language.GetCulture());

        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(updatePath));

        if (updateJson?["quest"] is null) return;

        string version = (int.Parse(updateJson["quest"].GetValue<string>()) + 1).ToString();

        string questFile = await File.ReadAllTextAsync(questPath);
        JsonObject? previousQuestData = JsonSerializer.Deserialize<JsonObject>(questFile);

        if (previousQuestData is null) return;

        Dictionary<string, object> toSerialize = new()
        {
            { "version", version },
        };

        updateJson["quest"] = version;
        
        foreach (QuestModel quest in questlist)
        {
            QuestTitleTranslationModel? title = titles.Find(q => q.QuestId == quest.Id);
            QuestDescriptionTranslationModel? desc = descriptions.Find(q => q.QuestId == quest.Id);

            if (previousQuestData.ContainsKey(quest.ApiId.ToString()))
            {
                JsonObject? questData = previousQuestData[quest.ApiId.ToString()]?.AsObject();

                if (questData is not null)
                {
                    toSerialize.Add(quest.ApiId.ToString(), questData);

                    if (title is not null)
                    {
                        questData["name"] = title?.Translations.Find(t => t.Language == language)?.Translation ?? "";
                    }

                    if (desc is not null)
                    {
                        questData["desc"] = desc?.Translations.Find(t => t.Language == language)?.Translation ?? "";
                    }

                    questData["code"] = quest.Code;
                    questData["name_jp"] = quest.NameJP;
                    questData["desc_jp"] = quest.DescJP;
                }
            }
            else
            {
                toSerialize.Add(quest.ApiId.ToString(), new QuestData(quest.ApiId)
                {
                    Code = quest.Code,
                    DescEN = desc?.Translations.Find(t => t.Language == language)?.Translation ?? "",
                    DescJP = quest.DescJP,
                    NameEN = title?.Translations.Find(t => t.Language == language)?.Translation ?? "",
                    NameJP = quest.NameJP,
                });
            }
        }

        await File.WriteAllTextAsync(questPath, JsonSerializer.Serialize(toSerialize, SerializationOptions), Encoding.UTF8);
        await File.WriteAllTextAsync(updatePath, JsonSerializer.Serialize(updateJson, SerializationOptions), Encoding.UTF8);
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

    public async Task AddTitleTranslation(QuestModel quest)
    {
        QuestTitleTranslationModel tl = new()
        {
            QuestId = quest.Id,
            Translations = LanguageExtensions.AllLanguagesTyped
                .Select(lang => new TranslationModel()
                {
                    Language = lang,
                    Translation = quest.NameEN,
                    IsPendingChange = true,
                })
                .ToList(),
        };

        await Database.AddAsync(tl);
        await Database.SaveChangesAsync();
    }

    public async Task AddDescTranslation(QuestModel quest)
    {
        QuestDescriptionTranslationModel tl = new()
        {
            QuestId = quest.Id,
            Translations = LanguageExtensions.AllLanguagesTyped
                .Select(lang => new TranslationModel()
                {
                    Language = lang,
                    Translation = quest.DescEN,
                    IsPendingChange = true,
                })
                .ToList(),
        };

        await Database.AddAsync(tl);
        await Database.SaveChangesAsync();
    }

    public async Task EditTranslation(QuestModel quest, string nameBefore, string descBefore)
    {
        QuestTitleTranslationModel? titleTlsFound = await Database.QuestTitleTranslations
            .Include(tl => tl.Translations)
            .FirstOrDefaultAsync(tl => tl.QuestId == quest.Id);

        if (titleTlsFound is null)
        {
            await AddTitleTranslation(quest);
        }
        else
        {
            foreach (TranslationModel tlModel in titleTlsFound.Translations.Where(tl => tl.Translation == nameBefore))
            {
                tlModel.Translation = quest.NameEN;
                tlModel.IsPendingChange = true;

                Database.Update(tlModel);
            }

            await Database.SaveChangesAsync();
        }

        QuestDescriptionTranslationModel? destTlsFound = await Database.QuestDescriptionTranslations
            .Include(tl => tl.Translations)
            .FirstOrDefaultAsync(tl => tl.QuestId == quest.Id);

        if (destTlsFound is null)
        {
            await AddDescTranslation(quest);
        }
        else
        {
            foreach (TranslationModel tlModel in destTlsFound.Translations.Where(tl => tl.Translation == descBefore))
            {
                tlModel.Translation = quest.DescEN;
                tlModel.IsPendingChange = true;

                Database.Update(tlModel);
            }

            await Database.SaveChangesAsync();
        }
    }
}
