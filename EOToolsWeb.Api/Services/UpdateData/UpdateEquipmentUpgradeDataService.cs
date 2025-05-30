using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Models.EquipmentUpgrades;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Shared.EquipmentUpgrades;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Services.UpdateData;

public class UpdateEquipmentUpgradeDataService(IGitManagerService git, EoToolsDbContext db, JsonSerializerOptions options, DatabaseSyncService databaseSyncService) : TranslationUpdateService
{
    private IGitManagerService GitManager { get; } = git;
    private DatabaseSyncService DatabaseSyncService { get; } = databaseSyncService;
    private EoToolsDbContext Database => db;
    private JsonSerializerOptions SerializationOptions => options;

    private string UpdateDataFilePath => Path.Combine(GitManager.FolderPath, "update.json");
    private string EquipmentUpgradesFilePath => Path.Combine(GitManager.FolderPath, "Data", "EquipmentUpgrades.json");

    public async Task UpdateEquipmentUpgrades()
    {
        await GitManager.Pull();

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(UpdateDataFilePath));

        if (updateJson is null) return;

        int version = updateJson["EquipmentUpgrades"].GetValue<int>() + 1;
        updateJson["EquipmentUpgrades"] = version;
        
        List<EquipmentUpgradeModel> allUpgrades = GetAllUpgrades();
        CleanUpDbBeforePush(allUpgrades);

        List<EquipmentUpgradeDataModel> upgradesJson = allUpgrades
            .Select(upg => new EquipmentUpgradeDataModel(upg))
            .ToList();

        upgradesJson.ForEach(upgrade => PopulateProperties(upgrade, allUpgrades));

        upgradesJson = upgradesJson
            .OrderBy(eq => eq.EquipmentId)
            .Where(eq => eq.UpgradeFor.Any() || eq.Improvement.Count > 0)
            .ToList();


        // --- Stage & push
        await DatabaseSyncService.StageDatabaseChangesToGit();

        await File.WriteAllTextAsync(EquipmentUpgradesFilePath, JsonSerializer.Serialize(upgradesJson, SerializationOptions), Encoding.UTF8);
        await File.WriteAllTextAsync(UpdateDataFilePath, JsonSerializer.Serialize(updateJson, SerializationOptions), Encoding.UTF8);

        await GitManager.Push($"Equipment upgrades - {version}");
    }

    private List<EquipmentUpgradeModel> GetAllUpgrades()
    {
        return Database.EquipmentUpgrades
            .Include("Improvement.ConversionData")
            .Include("Improvement.Helpers.CanHelpOnDays")
            .Include("Improvement.Helpers.ShipIds")
            .Include("Improvement.Costs.Cost0To5.ConsumableDetail")
            .Include("Improvement.Costs.Cost0To5.EquipmentDetail")
            .Include("Improvement.Costs.Cost6To9.ConsumableDetail")
            .Include("Improvement.Costs.Cost6To9.EquipmentDetail")
            .Include("Improvement.Costs.CostMax.ConsumableDetail")
            .Include("Improvement.Costs.CostMax.EquipmentDetail")

            // Extra Costs
            .Include(eq => eq.Improvement)
            .ThenInclude(cost => cost.Costs.ExtraCost)
            .ThenInclude(extraCost => extraCost.Consumables)

            .Include(eq => eq.Improvement)
            .ThenInclude(cost => cost.Costs.ExtraCost)
            .ThenInclude(extraCost => extraCost.Levels)

            .ToList();
    }

    private void CleanUpDbBeforePush(List<EquipmentUpgradeModel> upgrades)
    {
        // Noticed duplicate entries for some equipments ...
        List<EquipmentUpgradeModel> duplicates = upgrades
            .Where(elementInList =>
                upgrades.Count(element =>
                    elementInList.EquipmentId == element.EquipmentId) > 1)
            .ToList();

        if (duplicates.Count > 0)
        {
            // do the cleanup
            foreach (IGrouping<int, EquipmentUpgradeModel> model in duplicates.GroupBy(upgrade => upgrade.EquipmentId))
            {
                EquipmentUpgradeModel? firstUpgrade = model.FirstOrDefault(upg => upg.Improvement.Any());
                if (firstUpgrade is null) firstUpgrade = model.First();

                // delete everything but the "first upgrade"
                foreach (EquipmentUpgradeModel upg in model)
                {
                    if (upg != firstUpgrade)
                    {
                        Database.Remove(upg);
                        upgrades.Remove(upg);
                    }
                }
            }

            // Then do another check
            duplicates = upgrades
                .Where(elementInList =>
                    upgrades.Count(element =>
                        elementInList.EquipmentId == element.EquipmentId) > 1)
                .ToList();

            if (duplicates.Count > 0)
            {
                string equipmentName = Database.Equipments.FirstOrDefault(eq => eq.ApiId == duplicates[0].EquipmentId)?.NameEN ?? "";
                throw new Exception($"Duplicate entries found for equipment {equipmentName}");
            }
        }
    }

    private void PopulateProperties(EquipmentUpgradeDataModel model, List<EquipmentUpgradeModel> upgrades)
    {
        model.ConvertTo = model.Improvement
            .Where(imp => imp.ConversionData != null)
            .Select(imp => new EquipmentUpgradeConversionModel()
            {
                IdEquipmentAfter = imp.ConversionData?.IdEquipmentAfter ?? 0,
                EquipmentLevelAfter = imp.ConversionData?.EquipmentLevelAfter ?? 0,
            })
            .ToList();

        model.UpgradeFor = upgrades
            .Where(upg => upg.Improvement.Any(improvment => UseEquipmentInUpgrades(model, improvment)))
            .Select(upg => upg.EquipmentId)
            .Where(id => id != model.EquipmentId)
            .ToList();
    }

    private bool UseEquipmentInUpgrades(EquipmentUpgradeDataModel model, EquipmentUpgradeImprovmentModel improvment)
    {
        if (UseEquipmentInUpgradesCost(model, improvment.Costs.Cost0To5)) return true;
        if (UseEquipmentInUpgradesCost(model, improvment.Costs.Cost6To9)) return true;
        if (improvment.Costs.CostMax is not null && UseEquipmentInUpgradesCost(model, improvment.Costs.CostMax)) return true;

        return false;
    }

    private bool UseEquipmentInUpgradesCost(EquipmentUpgradeDataModel model, EquipmentUpgradeImprovmentCostDetail cost)
    {
        return cost.EquipmentDetail.Any(eq => eq.ItemId == model.EquipmentId);
    }
}
