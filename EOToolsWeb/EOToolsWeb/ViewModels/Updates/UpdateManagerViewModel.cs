using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Updates;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace EOToolsWeb.ViewModels.Updates;

public partial class UpdateManagerViewModel
{
    public ObservableCollection<UpdateViewModel> UpdateListSorted { get; set; } = new();
    public List<UpdateViewModel> UpdateList { get; set; } = [];

    private HttpClient HttpClient { get; }

    public UpdateManagerViewModel(HttpClient client)
    {
        HttpClient = client;
    }

    public async void LoadAllUpdates()
    {
        List<UpdateModel>? json = await HttpClient.GetFromJsonAsync<List<UpdateModel>>("Update");

        UpdateList = new(json
            .Select(update => new UpdateViewModel(update))
            .ToList());

        ReloadUpdateList();
    }

    public void ReloadUpdateList()
    {
        UpdateListSorted.Clear();

        List<UpdateViewModel> updates = UpdateList.OrderByDescending(update => update.UpdateDate).ToList();

        foreach (UpdateViewModel update in updates)
        {
            UpdateListSorted.Add(update);
        }
    }

    [RelayCommand]
    public void AddUpdate()
    {

    }


    [RelayCommand]
    public void EditUpdate(UpdateViewModel vm)
    {

    }

    [RelayCommand]
    public void RemoveUpdate(UpdateViewModel vm)
    {

    }
}
