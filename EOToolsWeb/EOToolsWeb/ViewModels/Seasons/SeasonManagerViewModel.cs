using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Models.ShipLocks;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.Seasons;
using EOToolsWeb.ViewModels.ShipLocks;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.Seasons;

public partial class SeasonManagerViewModel : ViewModelBase
{
    private HttpClient HttpClient { get; }

    public ObservableCollection<SeasonModel> SeasonListSorted { get; set; } = new();

    [ObservableProperty]
    private SeasonModel? _selectedSeason;

    public List<SeasonModel> SeasonList { get; set; } = [];

    private SeasonViewModel SeasonViewModel { get; }

    public Interaction<SeasonViewModel, bool> ShowEditDialog { get; } = new();

    public SeasonManagerViewModel(HttpClient client, SeasonViewModel viewModel)
    {
        HttpClient = client;
        SeasonViewModel = viewModel;
    }

    public async Task LoadSeasonList()
    {
        if (SeasonList.Count > 0) return;

        SeasonList = await HttpClient.GetFromJsonAsync<List<SeasonModel>>("Season") ?? [];

        ReloadSeasonList();
    }

    private void ReloadSeasonList()
    {
        SeasonListSorted.Clear();

        List<SeasonModel> updates = SeasonList.OrderByDescending(update => update.Id).ToList();

        foreach (SeasonModel update in updates)
        {
            SeasonListSorted.Add(update);
        }
    }

    [RelayCommand]
    private async Task AddSeason()
    {
        try
        {
            SeasonModel model = new();

            SeasonViewModel.Model = model;
            await SeasonViewModel.LoadFromModel();

            if (await ShowEditDialog.Handle(SeasonViewModel))
            {
                SeasonViewModel.SaveChanges();

                HttpResponseMessage response = await HttpClient.PostAsJsonAsync("Season", model);

                response.EnsureSuccessStatusCode();

                SeasonModel? postedModel = await response.Content.ReadFromJsonAsync<SeasonModel>();

                if (postedModel is not null)
                {
                    SeasonList.Add(postedModel);
                    ReloadSeasonList();
                }
            }
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    [RelayCommand]
    private async Task EditSeason(SeasonModel model)
    {
        try
        {
            SeasonViewModel.Model = model;
            await SeasonViewModel.LoadFromModel();

            if (await ShowEditDialog.Handle(SeasonViewModel))
            {
                SeasonViewModel.SaveChanges();

                HttpResponseMessage response = await HttpClient.PutAsJsonAsync("Season", model);

                response.EnsureSuccessStatusCode();
            }
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    [RelayCommand]
    private async Task RemoveSeason(SeasonModel vm)
    {
        try
        {
            await HttpClient.DeleteAsync($"Season/{vm.Id}");

            SeasonList.Remove(vm);
            ReloadSeasonList();
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    [RelayCommand]
    private async Task EndQuests(SeasonModel vm)
    {
        try
        {
            await HttpClient.PutAsync($"Season/{vm.Id}/EndQuests", null);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }
}