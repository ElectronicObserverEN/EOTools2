using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.Updates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class UpdateController(EoToolsDbContext db, UpdateMaintenanceDataService updateService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private UpdateMaintenanceDataService UpdateService { get; } = updateService;

    [HttpGet]
    public List<UpdateModel> GetUpdates()
    {
        return Database.Updates.ToList();
    }

    [HttpPut]
    public async Task<IActionResult> PutUpdate(UpdateModel update)
    {
        UpdateModel? actualUpdate = await Database.Updates.FindAsync(update.Id);

        if (actualUpdate is null)
        {
            return NotFound();
        }

        actualUpdate.Name = update.Name;
        actualUpdate.Description = update.Description;
        actualUpdate.WasLiveUpdate = update.WasLiveUpdate;

        actualUpdate.UpdateDate = update.UpdateDate;
        actualUpdate.UpdateStartTime = update.UpdateStartTime;
        actualUpdate.UpdateEndTime = update.UpdateEndTime;

        actualUpdate.EndTweetLink = update.EndTweetLink;
        actualUpdate.StartTweetLink = update.StartTweetLink;

        Database.Updates.Update(actualUpdate);
        await Database.SaveChangesAsync();

        return Ok(actualUpdate);
    }

    [HttpPut("pushUpdate")]
    public async Task<IActionResult> UpdateUpdates()
    {
        await UpdateService.UpdateMaintenanceData();

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> PostUpdate(UpdateModel update)
    {
        await Database.Updates.AddAsync(update);
        await Database.SaveChangesAsync();

        return Ok(update);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUpdate(UpdateModel update)
    {
        UpdateModel? actualUpdate = await Database.Updates.FindAsync(update.Id);

        if (actualUpdate is null)
        {
            return NotFound();
        }

        Database.Updates.Remove(actualUpdate);
        await Database.SaveChangesAsync();

        return Ok();
    }
}
