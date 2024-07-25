using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.ShipLocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class ShipLockController(EoToolsDbContext db, UpdateShipLockDataService dataUpdateService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private UpdateShipLockDataService DataUpdateService { get; } = dataUpdateService;

    [HttpGet]
    public List<ShipLockModel> Get(int eventId)
    {
        return Database.Locks.Where(locks => locks.EventId == eventId).ToList();
    }

    [HttpPut]
    public async Task<IActionResult> Put(ShipLockModel lockData)
    {
        ShipLockModel? savedLock = await Database.Locks.FindAsync(lockData.Id);

        if (savedLock is null)
        {
            return NotFound();
        }

        savedLock.ColorR = lockData.ColorR;
        savedLock.ColorG = lockData.ColorG;
        savedLock.ColorB = lockData.ColorB;
        savedLock.ColorA = lockData.ColorA;

        savedLock.NameEnglish = lockData.NameEnglish;
        savedLock.NameJapanese = lockData.NameJapanese;

        savedLock.ApiId = lockData.ApiId;
        savedLock.EventId = lockData.EventId;
        
        Database.Locks.Update(savedLock);
        await Database.SaveChangesAsync();

        return Ok(savedLock);
    }

    [HttpPut("pushLockData")]
    public async Task<IActionResult> UpdateData()
    {
        await DataUpdateService.UpdateLockData();
        await DataUpdateService.UpdateLockTranslations();

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Post(ShipLockModel shipLock)
    {
        await Database.Locks.AddAsync(shipLock);
        await Database.SaveChangesAsync();

        return Ok(shipLock);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ShipLockModel? actualLock = await Database.Locks.FindAsync(id);

        if (actualLock is null)
        {
            return NotFound();
        }

        Database.Locks.Remove(actualLock);
        await Database.SaveChangesAsync();

        return Ok();
    }
}