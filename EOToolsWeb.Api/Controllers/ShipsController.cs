using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.Ships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class ShipsController(EoToolsDbContext db, UpdateShipDataService dataUpdateService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private UpdateShipDataService DataUpdateService { get; } = dataUpdateService;

    [HttpGet]
    public List<ShipModel> GetShips()
    {
        return Database.Ships.Include(nameof(ShipModel.ShipClass)).ToList();
    }

    [HttpPut]
    public async Task<IActionResult> PutShip(ShipModel ship)
    {
        ShipModel? actualShip = await Database.Ships.FindAsync(ship.Id);

        if (actualShip is null)
        {
            return NotFound();
        }

        actualShip.NameEN = ship.NameEN;
        actualShip.NameJP = ship.NameJP;
        actualShip.ApiId = ship.ApiId;
        actualShip.ShipClassId = ship.ShipClassId;

        if (ship.ShipClassId > 0)
        {
            actualShip.ShipClass = await Database.ShipClasses.FindAsync(ship.ShipClassId);

            if (actualShip.ShipClass is null)
            {
                return NotFound("Ship class not found");
            }
        }
        else
        {
            actualShip.ShipClass = null;
        }

        Database.Ships.Update(actualShip);
        await Database.SaveChangesAsync();

        return Ok(actualShip);
    }

    [HttpPut("pushShips")]
    public async Task<IActionResult> UpdateShips()
    {
        await DataUpdateService.UpdateShipTranslations();

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> PostShip(ShipModel ship)
    {
        if (ship.ShipClassId > 0)
        {
            ship.ShipClass = await Database.ShipClasses.FindAsync(ship.ShipClassId);

            if (ship.ShipClass is null)
            {
                return NotFound("Ship class not found");
            }
        }
        else
        {
            ship.ShipClass = null;
        }

        await Database.Ships.AddAsync(ship);
        await Database.SaveChangesAsync();

        return Ok(ship);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShips(int id)
    {
        ShipModel? actualShip = await Database.Ships.FindAsync(id);

        if (actualShip is null)
        {
            return NotFound();
        }

        Database.Ships.Remove(actualShip);
        await Database.SaveChangesAsync();

        return Ok();
    }
}
