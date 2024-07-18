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

    private HttpClient HttpClient { get; } = client;

    public async void LoadAllUpdates()
    {
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
        UpdateViewModel vm = new(model);

        if (await ShowEditDialog.Handle(vm) is true)
        {
            vm.SaveChanges();

            await HttpClient.PostAsJsonAsync("Update", vm.Model);

            UpdateList.Add(vm.Model);
            ReloadUpdateList();
        }
    }

    [RelayCommand]
    public async Task EditUpdate(UpdateModel vm)
    {
        UpdateViewModel vmEdit = new(new()
        {
            Description = vm.Description,
            Name = vm.Name,
            UpdateDate = vm.UpdateDate,
            WasLiveUpdate = vm.WasLiveUpdate,
            UpdateStartTime = vm.UpdateStartTime,
            UpdateEndTime = vm.UpdateEndTime,

            EndTweetLink = vm.EndTweetLink,
            StartTweetLink = vm.StartTweetLink,
        });
        
        if (await ShowEditDialog.Handle(vmEdit) is true)
        {
            vm.Description = vmEdit.Description;
            vm.Name = vmEdit.Name;
            vm.UpdateDate = vmEdit.UpdateDate;
            vm.WasLiveUpdate = vmEdit.WasLiveUpdate;
            vm.UpdateStartTime = vmEdit.UpdateStartTime;
            vm.UpdateEndTime = vmEdit.UpdateEndTime;
            vm.EndTweetLink = vmEdit.EndTweet;
            vm.StartTweetLink = vmEdit.StartTweet;

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
}
