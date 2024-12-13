using EOToolsWeb.Api.Database;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers.Ships;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class ShipClassesController(EoToolsDbContext db) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;

    [HttpGet]
    public List<ShipClassModel> GetClasses()
    {
        return Database.ShipClasses.ToList();
    }

    [HttpPut]
    public async Task<IActionResult> PutShipClass(ShipClassModel shipClass)
    {
        ShipClassModel? savedShipClass = await Database.ShipClasses.FindAsync(shipClass.Id);

        if (savedShipClass is null)
        {
            return NotFound();
        }

        savedShipClass.NameEnglish = shipClass.NameEnglish;
        savedShipClass.NameJapanese = shipClass.NameJapanese;
        savedShipClass.ApiId = shipClass.ApiId;

        Database.ShipClasses.Update(savedShipClass);
        await Database.SaveChangesAsync();

        return Ok(savedShipClass);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> PostShipClass(ShipClassModel shipClass)
    {
        await Database.ShipClasses.AddAsync(shipClass);
        await Database.SaveChangesAsync();

        return Ok(shipClass);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> DeleteClass(int id)
    {
        ShipClassModel? actualClass = await Database.ShipClasses.FindAsync(id);

        if (actualClass is null)
        {
            return NotFound();
        }

        Database.ShipClasses.Remove(actualClass);
        await Database.SaveChangesAsync();

        return Ok();
    }
}
