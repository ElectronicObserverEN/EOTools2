using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
[Route("[controller]")]
public class EquipmentsController(EoToolsDbContext db, UpdateEquipmentDataService updateService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private UpdateEquipmentDataService UpdateEquipmentDataService { get; } = updateService;

    [HttpGet]
    public List<EquipmentModel> Get()
    {
        return Database.Equipments.ToList();
    }

    [HttpPut]
    public async Task<IActionResult> Put(EquipmentModel equipment)
    {
        EquipmentModel? savedEquipment = await Database.Equipments.FindAsync(equipment.Id);

        if (savedEquipment is null)
        {
            return NotFound();
        }

        savedEquipment.NameEN = equipment.NameEN;
        savedEquipment.NameJP = equipment.NameJP;
        savedEquipment.CanBeCrafted = equipment.CanBeCrafted;
        savedEquipment.ApiId = equipment.ApiId;

        Database.Equipments.Update(savedEquipment);
        await Database.SaveChangesAsync();

        return Ok(savedEquipment);
    }

    [HttpPost]
    public async Task<IActionResult> Post(EquipmentModel equipment)
    {
        await Database.Equipments.AddAsync(equipment);
        await Database.SaveChangesAsync();

        return Ok(equipment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        EquipmentModel? savedEquipment = await Database.Equipments.FindAsync(id);

        if (savedEquipment is null)
        {
            return NotFound();
        }

        Database.Equipments.Remove(savedEquipment);
        await Database.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("pushEquipments")]
    public async Task<IActionResult> UpdateEquipments()
    {
        await UpdateEquipmentDataService.UpdateEquipmentTranslations();

        return Ok();
    }
}
