using EOToolsWeb.Api.Database;
using EOToolsWeb.Shared.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class EventController(EoToolsDbContext db) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;

    [HttpGet]
    public List<EventModel> GetUpdates()
    {
        return Database.Events.ToList();
    }

    [HttpPut]
    public async Task<IActionResult> PutUpdate(EventModel eventModel)
    {
        EventModel? actualEvent = await Database.Events.FindAsync(eventModel.Id);

        if (actualEvent is null)
        {
            return NotFound();
        }

        actualEvent.StartOnUpdateId = eventModel.StartOnUpdateId;
        actualEvent.EndOnUpdateId = eventModel.EndOnUpdateId;
        actualEvent.ApiId = eventModel.ApiId;
        actualEvent.Name = eventModel.Name;

        Database.Events.Update(actualEvent);
        await Database.SaveChangesAsync();

        return Ok(actualEvent);
    }

    [HttpPost]
    public async Task<IActionResult> PostUpdate(EventModel eventModel)
    {
        await Database.Events.AddAsync(eventModel);
        await Database.SaveChangesAsync();

        return Ok(eventModel);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUpdate(EventModel eventModel)
    {
        EventModel? actualEvent = await Database.Events.FindAsync(eventModel.Id);

        if (actualEvent is null)
        {
            return NotFound();
        }

        Database.Events.Remove(actualEvent);
        await Database.SaveChangesAsync();

        return Ok();
    }
}
