using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.Quests;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers.Quest;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
public class QuestController(EoToolsDbContext db, UpdateQuestDataService updateService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private UpdateQuestDataService UpdateDataService { get; } = updateService;

    [HttpGet]
    public List<QuestModel> Get()
    {
        return Database.Quests.ToList();
    }

    [HttpPut]
    public async Task<IActionResult> Put(QuestModel quest)
    {
        QuestModel? savedQuest = await Database.Quests.FindAsync(quest.Id);

        if (savedQuest is null)
        {
            return NotFound();
        }

        savedQuest.ApiId = quest.ApiId;
        savedQuest.Code = quest.Code;

        string nameBefore = savedQuest.NameEN;
        string descBefore = savedQuest.DescEN;

        savedQuest.NameEN = quest.NameEN;
        savedQuest.NameJP = quest.NameJP;

        savedQuest.DescEN = quest.DescEN;
        savedQuest.DescJP = quest.DescJP;

        savedQuest.AddedOnUpdateId = quest.AddedOnUpdateId;
        savedQuest.RemovedOnUpdateId = quest.RemovedOnUpdateId;

        savedQuest.Tracker = quest.Tracker;

        savedQuest.SeasonId = quest.SeasonId;
        savedQuest.QuestProgressResetType = quest.QuestProgressResetType;

        Database.Quests.Update(savedQuest);
        await Database.SaveChangesAsync();

        await UpdateDataService.EditTranslation(quest, nameBefore, descBefore);

        return Ok(savedQuest);
    }

    [HttpPost]
    public async Task<IActionResult> Post(QuestModel quest)
    {
        await Database.Quests.AddAsync(quest);
        await Database.SaveChangesAsync();

        await UpdateDataService.AddTitleTranslation(quest);
        await UpdateDataService.AddDescTranslation(quest);

        return Ok(quest);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        QuestModel? savedQuest = await Database.Quests.FindAsync(id);

        if (savedQuest is null)
        {
            return NotFound();
        }

        Database.Quests.Remove(savedQuest);
        await Database.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("pushQuests")]
    public async Task<IActionResult> Update()
    {
        await UpdateDataService.UpdateQuestTranslations();
        await UpdateDataService.UpdateQuestTrackers();

        return Ok();
    }
}
