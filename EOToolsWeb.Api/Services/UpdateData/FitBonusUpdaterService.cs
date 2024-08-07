using System.Text.Json;
using System.Text.Json.Nodes;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Shared.FitBonus;
using EOToolsWeb.Shared.FitBonus.FitBonusSourceV1;
using EOToolsWeb.Shared.Ships;

namespace EOToolsWeb.Api.Services.UpdateData;

public class FitBonusUpdaterService(ConfigurationService config, IGitManagerService git, JsonSerializerOptions options)
{
    private string SourceUrl => config.FitBonusSourceUrl;
    private HttpClient HttpClient { get; } = new();
    private JsonSerializerOptions JsonSerializerOptions { get; } = options;
    private IGitManagerService GitManagerService { get; } = git;

    public string FitBonusFilePath => Path.Combine(GitManagerService.FolderPath, "Data", "FitBonuses.json");
    public string UpdateFilePath => Path.Combine(GitManagerService.FolderPath, "update.json");

    public async Task UpdateThenSaveFileThenPush()
    {
        List<FitBonusPerEquipmentModel>? bonuses = await GetFitBonuses();
        if (bonuses is null) return;

        await File.WriteAllTextAsync(FitBonusFilePath, JsonSerializer.Serialize(bonuses, JsonSerializerOptions));

        // --- Change update.json too
        JsonObject? update = JsonSerializer.Deserialize<JsonObject>(await File.ReadAllTextAsync(UpdateFilePath));

        if (update is null) return;

        JsonNode fitBonusUpdateVersion = update["FitBonuses"];
        int version = fitBonusUpdateVersion.GetValue<int>() + 1;
        update["FitBonuses"] = version;

        await File.WriteAllTextAsync(UpdateFilePath, JsonSerializer.Serialize(update, JsonSerializerOptions));

        await GitManagerService.Push($"Fit bonuses - {version}");
    }

    private async Task<List<FitBonusPerEquipmentModel>?> GetFitBonuses()
    {
        List<FitBonusSourceV1>? bonuses = await HttpClient.GetFromJsonAsync<List<FitBonusSourceV1>>(SourceUrl);

        if (bonuses is null) return null;

        List<FitBonusPerEquipmentModel> eoBonuses = new();

        foreach (FitBonusSourceV1 bonus in bonuses)
        {
            eoBonuses.Add(new()
            {
                EquipmentIds = bonus.Ids,
                EquipmentTypes = bonus.Types,
                Bonuses = bonus.Bonuses.Select(ConvertBonus).ToList(),
            });
        }

        return eoBonuses;
    }

    private FitBonusDataModel ConvertBonus(FitBonusSourceV1_FitBonus bonus)
    {
        FitBonusDataModel model = new()
        {
            EquipmentLevel = bonus.Level,

            EquipmentRequired = bonus.RequiresId,
            EquipmentRequiresLevel = bonus.RequiresIdLevel,
            NumberOfEquipmentsRequiredAfterOtherFilters = bonus.RequiresIdNum switch
            {
                { } => bonus.RequiresIdNum,
                _ => bonus.Num,
            },

            EquipmentTypesRequired = bonus.RequiresType,
            NumberOfEquipmentTypesRequired = bonus.RequiresType switch
            {
                { } => 1,
                _ => null,
            },

            ShipClasses = bonus.ShipClass,
            ShipMasterIds = bonus.ShipId,
            ShipIds = bonus.ShipBase,
            ShipNationalities = bonus.ShipCountry?.Select(ConvertNationality).ToList(),
            ShipTypes = bonus.ShipType?.Select(st => (ShipTypes)st).ToList(),
        };

        if (bonus.RequiresAR > 0)
        {
            model.BonusesIfAirRadar = ConvertBonusValue(bonus.Bonus);
        }
        else if (bonus.RequiresSR > 0)
        {
            model.BonusesIfSurfaceRadar = ConvertBonusValue(bonus.Bonus);
        }
        else if (bonus.RequiresAccR > 0)
        {
            model.BonusesIfAccuracyRadar = ConvertBonusValue(bonus.Bonus);
        }
        else
        {
            model.Bonuses = ConvertBonusValue(bonus.Bonus);
        }

        return model;
    }

    private static FitBonusValueModel ConvertBonusValue(FitBonusSourceV1_BonusValue bonus)
    {
        return new()
        {
            Firepower = bonus.Houg,
            Torpedo = bonus.Raig,
            AntiAir = bonus.Tyku,
            Armor = bonus.Souk,
            Evasion = bonus.Kaih,
            ASW = bonus.Tais,
            LOS = bonus.Saku,
            Bombing = bonus.Baku,
            Accuracy = bonus.Houm,
            Range = bonus.Leng,
        };
    }

    private static ShipNationality ConvertNationality(string nationality)
    {
        return nationality switch
        {
            "JP" => ShipNationality.Japanese,
            "DE" => ShipNationality.German,
            "IT" => ShipNationality.Italian,
            "US" => ShipNationality.American,
            "GB" => ShipNationality.British,
            "FR" => ShipNationality.French,
            "AU" => ShipNationality.Australian,
            /*
               "" => ShipNationality.Russian,
               "" => ShipNationality.Swedish,
               "" => ShipNationality.Dutch,
            */
            _ => throw new NotImplementedException(),
        };
    }
}