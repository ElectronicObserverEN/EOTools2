using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using EOToolsWeb.Control.Grid;
using EOToolsWeb.Models.FitBonus;
using EOToolsWeb.ViewModels.Equipments;
using EOToolsWeb.ViewModels.Settings;
using EOToolsWeb.ViewModels.Ships;

namespace EOToolsWeb.ViewModels.FitBonus;

public class FitBonusIssuesFetcher(SettingsViewModel settings, EquipmentManagerViewModel equipments, ShipManagerViewModel ships) : IDataFetcher
{
    private HttpClient Client { get; } = new();
    private bool Initialized { get; set; } = false;

    private SettingsViewModel Settings { get; } = settings;
    private EquipmentManagerViewModel EquipmentManagerViewModel { get; } = equipments;
    private ShipManagerViewModel ShipManagerViewModel { get; } = ships;

    public async Task<PaginatedResultModel<IGridRowFetched>?> LoadData(int skip, int take)
    {
        if (!Initialized)
        {
            Client.BaseAddress = new Uri(Settings.EoApiUrl);

            var authenticationString = $"{Settings.EoApiKey}:";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

            await EquipmentManagerViewModel.LoadAllEquipments();
            await ShipManagerViewModel.LoadAllShips();

            Initialized = true;
        }

        PaginatedResultModel<FitBonusIssueModel>? result = await Client.GetFromJsonAsync<PaginatedResultModel<FitBonusIssueModel>>($"FitBonusIssues?issueState=1&skip={skip}&take={take}");

        if (result is null) return null;
        
        return new()
        {
            Results = result.Results.Select(model => new FitBonusIssueViewModel(model, ShipManagerViewModel, EquipmentManagerViewModel)),
            TotalCount = result.TotalCount,
        };
    }
}