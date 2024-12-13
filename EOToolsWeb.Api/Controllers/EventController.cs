using EOToolsWeb.Api.Database;
using EOToolsWeb.Shared.Events;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUpdate(int id)
    {
        EventModel? actualEvent = await Database.Events.FindAsync(id);

        if (actualEvent is null)
        {
            return NotFound();
        }

        Database.Events.Remove(actualEvent);
        await Database.SaveChangesAsync();

        return Ok();
    }
}
