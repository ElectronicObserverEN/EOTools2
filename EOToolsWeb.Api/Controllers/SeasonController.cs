using EOToolsWeb.Api.Database;
using EOToolsWeb.Shared.Quests;
using EOToolsWeb.Shared.Seasons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class SeasonController(EoToolsDbContext db) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;

    [HttpGet]
    public List<SeasonModel> GetUpdates()
    {
        return Database.Seasons.ToList();
    }

    [HttpPut]
    public async Task<IActionResult> PutUpdate(SeasonModel update)
    {
        SeasonModel? actualSeason = await Database.Seasons.FindAsync(update.Id);

        if (actualSeason is null)
        {
            return NotFound();
        }

        actualSeason.Name = update.Name;
        actualSeason.RemovedOnUpdateId = update.RemovedOnUpdateId;
        actualSeason.AddedOnUpdateId = update.AddedOnUpdateId;

        Database.Seasons.Update(actualSeason);
        await Database.SaveChangesAsync();

        return Ok(actualSeason);
    }

    [HttpPost]
    public async Task<IActionResult> PostUpdate(SeasonModel update)
    {
        await Database.Seasons.AddAsync(update);
        await Database.SaveChangesAsync();

        return Ok(update);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        SeasonModel? actualSeason = await Database.Seasons.FindAsync(id);

        if (actualSeason is null)
        {
            return NotFound();
        }

        Database.Seasons.Remove(actualSeason);
        await Database.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("{id}/EndQuests")]
    public async Task<IActionResult> EndQuests(int id)
    {
        SeasonModel? actualSeason = await Database.Seasons.FindAsync(id);

        if (actualSeason is null)
        {
            return NotFound();
        }

        if (actualSeason.RemovedOnUpdateId is null) return Ok();

        List<QuestModel> quests = Database.Quests.Where(q => q.SeasonId == id).ToList();

        foreach (QuestModel quest in quests)
        {
            quest.RemovedOnUpdateId = actualSeason.RemovedOnUpdateId;
            db.Update(quest);
        }

        await db.SaveChangesAsync();

        return Ok();
    }
}
