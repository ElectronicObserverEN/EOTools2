using EOToolsWeb.Api.Database;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers.Equipments;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
[Route("[controller]")]
public class EquipmentUpgradeImprovmentModelController(EoToolsDbContext db) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;

    [HttpGet("{upgradeId}")]
    public async Task<IActionResult> GetImprovments(int upgradeId)
    {
        EquipmentUpgradeImprovmentModel? result = Database.Improvments
            .Include("ConversionData")
            .Include("Helpers.CanHelpOnDays")
            .Include("Helpers.ShipIds")
            .Include("Costs.Cost0To5.ConsumableDetail")
            .Include("Costs.Cost0To5.EquipmentDetail")
            .Include("Costs.Cost6To9.ConsumableDetail")
            .Include("Costs.Cost6To9.EquipmentDetail")
            .Include("Costs.CostMax.ConsumableDetail")
            .Include("Costs.CostMax.EquipmentDetail")
            .FirstOrDefault(upg => upg.Id == upgradeId);

        if (result is null)
        {
            return NotFound();
        }


        return Ok(result);
    }

    [HttpPost("{equipmentId}")]
    public async Task<IActionResult> Post(int equipmentId, EquipmentUpgradeImprovmentModel equipmentUpgrade)
    {
        EquipmentUpgradeModel? model = await Database.EquipmentUpgrades.FirstOrDefaultAsync(upgData => upgData.EquipmentId == equipmentId);

        bool newModel = model is null;

        if (model is null)
        {
            model = new()
            {
                EquipmentId = equipmentId,
                Improvement = [],
            };
        }

        Database.AddRange(equipmentUpgrade.Helpers);
        Database.AddRange(equipmentUpgrade.Helpers.SelectMany(helpers => helpers.CanHelpOnDays));
        Database.AddRange(equipmentUpgrade.Helpers.SelectMany(helpers => helpers.ShipIds));

        Database.AddRange(equipmentUpgrade.Costs);

        Database.AddRange(equipmentUpgrade.Costs.Cost0To5);
        Database.AddRange(equipmentUpgrade.Costs.Cost0To5.ConsumableDetail);
        Database.AddRange(equipmentUpgrade.Costs.Cost0To5.EquipmentDetail);

        Database.AddRange(equipmentUpgrade.Costs.Cost6To9);
        Database.AddRange(equipmentUpgrade.Costs.Cost6To9.ConsumableDetail);
        Database.AddRange(equipmentUpgrade.Costs.Cost6To9.EquipmentDetail);

        if (equipmentUpgrade.Costs.CostMax is not null)
        {
            Database.AddRange(equipmentUpgrade.Costs.CostMax);
            Database.AddRange(equipmentUpgrade.Costs.CostMax.ConsumableDetail);
            Database.AddRange(equipmentUpgrade.Costs.CostMax.EquipmentDetail);
        }

        model.Improvement.Add(equipmentUpgrade);

        if (newModel)
        {
            Database.EquipmentUpgrades.Add(model);
        }
        else
        {
            Database.EquipmentUpgrades.Update(model);
        }

        await Database.SaveChangesAsync();

        return Ok(equipmentUpgrade);
    }

    [HttpPut]
    public async Task<IActionResult> Put(EquipmentUpgradeImprovmentModel equipmentUpgrade)
    {
        EquipmentUpgradeImprovmentModel? model = Database.Improvments
            .Include("ConversionData")
            .Include("Helpers.CanHelpOnDays")
            .Include("Helpers.ShipIds")
            .Include("Costs.Cost0To5.ConsumableDetail")
            .Include("Costs.Cost0To5.EquipmentDetail")
            .Include("Costs.Cost6To9.ConsumableDetail")
            .Include("Costs.Cost6To9.EquipmentDetail")
            .Include("Costs.CostMax.ConsumableDetail")
            .Include("Costs.CostMax.EquipmentDetail")
            .FirstOrDefault(upg => upg.Id == equipmentUpgrade.Id);

        if (model is null)
        {
            return NotFound();
        }

        Database.RemoveRange(model.Costs);
        Database.RemoveRange(model.Helpers);

        if (model.ConversionData is not null)
        {
            Database.RemoveRange(model.ConversionData);
        }

        equipmentUpgrade.Helpers.ForEach(help => help.Id = 0);

        foreach (var entity in equipmentUpgrade.Helpers.SelectMany(helpers => helpers.CanHelpOnDays))
        {
            entity.Id = 0;
        }

        foreach (var entity in equipmentUpgrade.Helpers.SelectMany(helpers => helpers.ShipIds))
        {
            entity.Id = 0;
        }

        Database.AddRange(equipmentUpgrade.Helpers);
        Database.AddRange(equipmentUpgrade.Helpers.SelectMany(helpers => helpers.CanHelpOnDays));
        Database.AddRange(equipmentUpgrade.Helpers.SelectMany(helpers => helpers.ShipIds));

        equipmentUpgrade.Costs.Id = 0;

        equipmentUpgrade.Costs.Cost0To5.Id = 0;

        foreach (var entity in equipmentUpgrade.Costs.Cost0To5.EquipmentDetail)
        {
            entity.Id = 0;
        }

        foreach (var entity in equipmentUpgrade.Costs.Cost0To5.ConsumableDetail)
        {
            entity.Id = 0;
        }

        equipmentUpgrade.Costs.Cost6To9.Id = 0;

        foreach (var entity in equipmentUpgrade.Costs.Cost6To9.EquipmentDetail)
        {
            entity.Id = 0;
        }

        foreach (var entity in equipmentUpgrade.Costs.Cost6To9.ConsumableDetail)
        {
            entity.Id = 0;
        }

        Database.AddRange(equipmentUpgrade.Costs);

        Database.AddRange(equipmentUpgrade.Costs.Cost0To5);
        Database.AddRange(equipmentUpgrade.Costs.Cost0To5.ConsumableDetail);
        Database.AddRange(equipmentUpgrade.Costs.Cost0To5.EquipmentDetail);

        Database.AddRange(equipmentUpgrade.Costs.Cost6To9);
        Database.AddRange(equipmentUpgrade.Costs.Cost6To9.ConsumableDetail);
        Database.AddRange(equipmentUpgrade.Costs.Cost6To9.EquipmentDetail);

        if (equipmentUpgrade.Costs.CostMax is not null)
        {
            equipmentUpgrade.Costs.CostMax.Id = 0;

            foreach (var entity in equipmentUpgrade.Costs.CostMax.EquipmentDetail)
            {
                entity.Id = 0;
            }

            foreach (var entity in equipmentUpgrade.Costs.CostMax.ConsumableDetail)
            {
                entity.Id = 0;
            }

            Database.AddRange(equipmentUpgrade.Costs.CostMax);
            Database.AddRange(equipmentUpgrade.Costs.CostMax.ConsumableDetail);
            Database.AddRange(equipmentUpgrade.Costs.CostMax.EquipmentDetail);
        }

        if (equipmentUpgrade.ConversionData is not null)
        {
            equipmentUpgrade.ConversionData.Id = 0;

            Database.AddRange(equipmentUpgrade.ConversionData);
        }

        model.Helpers = equipmentUpgrade.Helpers;
        model.ConversionData = equipmentUpgrade.ConversionData;
        model.Costs = equipmentUpgrade.Costs;

        Database.Improvments.Update(model);

        await Database.SaveChangesAsync();

        return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Put(int id)
    {
        EquipmentUpgradeImprovmentModel? model = await Database.Improvments.FindAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        Database.Improvments.Remove(model);
        await Database.SaveChangesAsync();

        return Ok();
    }
}
