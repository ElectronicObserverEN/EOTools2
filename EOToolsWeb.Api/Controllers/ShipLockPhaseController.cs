using EOToolsWeb.Api.Database;
using EOToolsWeb.Shared.ShipLocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class ShipLockPhaseController(EoToolsDbContext db) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;

    [HttpGet]
    public List<ShipLockPhaseModel> Get(int eventId)
    {
        return Database.LockPhases.Where(locks => locks.EventId == eventId).ToList();
    }

    [HttpPut]
    public async Task<IActionResult> Put(ShipLockPhaseModel lockData)
    {
        ShipLockPhaseModel? savedLock = await Database.LockPhases.FindAsync(lockData.Id);

        if (savedLock is null)
        {
            return NotFound();
        }
        
        savedLock.EventId = lockData.EventId;
        savedLock.SortId = lockData.SortId;
        savedLock.LockGroups = lockData.LockGroups;
        savedLock.PhaseName = lockData.PhaseName;

        Database.LockPhases.Update(savedLock);
        await Database.SaveChangesAsync();

        return Ok(savedLock);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ShipLockPhaseModel shipLock)
    {
        await Database.LockPhases.AddAsync(shipLock);
        await Database.SaveChangesAsync();

        return Ok(shipLock);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ShipLockPhaseModel? actualLock = await Database.LockPhases.FindAsync(id);

        if (actualLock is null)
        {
            return NotFound();
        }

        Database.LockPhases.Remove(actualLock);
        await Database.SaveChangesAsync();

        return Ok();
    }
}