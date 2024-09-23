using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Updates;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.Updates;

public partial class UpdateManagerViewModel(HttpClient client) : ViewModelBase
{
    public ObservableCollection<UpdateModel> UpdateListSorted { get; set; } = new();
    public List<UpdateModel> UpdateList { get; set; } = [];

    public Interaction<UpdateViewModel, bool> ShowEditDialog { get; } = new();
    public Interaction<object?, UpdateModel?> ShowPickerDialog { get; } = new();

    private HttpClient HttpClient { get; } = client;

    public async Task LoadAllUpdates()
    {
        if (UpdateList.Count > 0) return;

        UpdateList = await HttpClient.GetFromJsonAsync<List<UpdateModel>>("Update") ?? [];
        
        ReloadUpdateList();
    }

    public void ReloadUpdateList()
    {
        UpdateListSorted.Clear();

        List<UpdateModel> updates = UpdateList.OrderByDescending(update => update.UpdateDate).ToList();

        foreach (UpdateModel update in updates)
        {
            UpdateListSorted.Add(update);
        }
    }

    [RelayCommand]
    private async Task AddUpdate()
    {
        UpdateModel model = new();
        UpdateViewModel vm = new();
        vm.Model = model;
        vm.LoadFromModel();

        if (await ShowEditDialog.Handle(vm))
        {
            vm.SaveChanges();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("Update", model);

            response.EnsureSuccessStatusCode();

            UpdateModel? postedModel = await response.Content.ReadFromJsonAsync<UpdateModel>();

            if (postedModel is not null)
            {
                UpdateList.Add(postedModel);

                ReloadUpdateList();
            }
        }
    }

    [RelayCommand]
    public async Task EditUpdate(UpdateModel vm)
    {
        UpdateViewModel vmEdit = new();
        vmEdit.Model = vm;
        vmEdit.LoadFromModel();

        if (await ShowEditDialog.Handle(vmEdit))
        {
            vmEdit.SaveChanges();

            await HttpClient.PutAsJsonAsync("Update", vm);
        }
    }

    [RelayCommand]
    public async Task RemoveUpdate(UpdateModel vm)
    {
        await HttpClient.DeleteAsync($"Update/{vm.Id}");

        UpdateList.Remove(vm);
        ReloadUpdateList();
    }

    [RelayCommand]
    public async Task UpdateUpdate()
    {
        await HttpClient.PutAsync("Update/pushUpdate", null);
    }
}
