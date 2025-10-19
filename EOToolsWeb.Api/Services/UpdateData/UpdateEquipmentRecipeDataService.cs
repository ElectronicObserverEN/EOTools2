using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Models.EquipmentUpgrades;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Shared.EquipmentDevelopment;
using EOToolsWeb.Shared.EquipmentUpgrades;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Services.UpdateData;

public class UpdateEquipmentRecipeDataService(IGitManagerService git, EoToolsDbContext db, JsonSerializerOptions options, DatabaseSyncService databaseSyncService) : TranslationUpdateService
{
    private IGitManagerService GitManager { get; } = git;
    private DatabaseSyncService DatabaseSyncService { get; } = databaseSyncService;
    private EoToolsDbContext Database => db;
    private JsonSerializerOptions SerializationOptions => options;

    private string UpdateDataFilePath => Path.Combine(GitManager.FolderPath, "update.json");
    private string EquipmentUpgradesFilePath => Path.Combine(GitManager.FolderPath, "Data", "EquipmentRecipes.json");

    public async Task UpdateRecipes()
    {
        await GitManager.Pull();

        // Get version
        JsonObject? updateJson = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(UpdateDataFilePath));

        if (updateJson is null) return;

        if (updateJson["EquipmentRecipes"]?.GetValue<int>() is not { } version)
        {
            version = 1;
        }
        else
        {
            version += 1;
        }

        updateJson["EquipmentRecipes"] = version;

        List<IGrouping<int, EquipmentRecipeModel>> allRecipesGroupedByEquipment = await Database.EquipmentRecipes
            .OrderBy(r => r.EquipmentId)
            .GroupBy(r => r.EquipmentId)
            .ToListAsync();

        upgradesJson = upgradesJson
            .OrderBy(eq => eq.EquipmentId)
            .Where(eq => eq.UpgradeFor.Any() || eq.Improvement.Count > 0)
            .ToList();

        await DatabaseSyncService.StageDatabaseChangesToGit();

        await File.WriteAllTextAsync(EquipmentUpgradesFilePath, JsonSerializer.Serialize(upgradesJson, SerializationOptions), Encoding.UTF8);
        await File.WriteAllTextAsync(UpdateDataFilePath, JsonSerializer.Serialize(updateJson, SerializationOptions), Encoding.UTF8);

        await GitManager.Push($"Equipment recipes - {version}");
    }
}
