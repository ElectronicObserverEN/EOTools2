using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers.Equipments;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
[Route("[controller]")]
public class EquipmentUpgradesController(EoToolsDbContext db, UpdateEquipmentUpgradeDataService updateService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private UpdateEquipmentUpgradeDataService UpdateEquipmentDataService { get; } = updateService;

    [HttpGet("{equipmentId}")]
    public List<int> Get(int equipmentId)
    {
        return Database.EquipmentUpgrades
            .Where(upgData => upgData.EquipmentId == equipmentId)
            .Include("Improvement")
            .SelectMany(upg => upg.Improvement)
            .Select(imp => imp.Id)
            .ToList();
    }

    [HttpPost]
    public async Task<IActionResult> Post(EquipmentUpgradeModel equipmentUpgrade)
    {
        await Database.EquipmentUpgrades.AddAsync(equipmentUpgrade);
        await Database.SaveChangesAsync();

        return Ok(equipmentUpgrade);
    }

    [HttpPut("pushEquipments")]
    public async Task<IActionResult> UpdateEquipments()
    {
        await UpdateEquipmentDataService.UpdateEquipmentUpgrades();

        return Ok();
    }
}
