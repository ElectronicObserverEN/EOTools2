using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Events;
using EOToolsWeb.ViewModels.Updates;
using ReactiveUI;

namespace EOToolsWeb.ViewModels.Events;

public partial class EventManagerViewModel(HttpClient client, UpdateListViewModel updates, EventViewModel eventViewModel) : ViewModelBase
{
    public ObservableCollection<EventModel> EventListSorted { get; set; } = new();
    public List<EventModel> EventList { get; set; } = [];

    private HttpClient HttpClient { get; } = client;
    private UpdateListViewModel UpdateListViewModel { get; } = updates;
    private EventViewModel EventViewModel { get; } = eventViewModel;

    public Interaction<EventViewModel, bool> ShowEditDialog { get; } = new();

    public async Task LoadAllEvents()
    {
        if (EventList.Count > 0) return;

        EventList = await HttpClient.GetFromJsonAsync<List<EventModel>>("Event") ?? [];

        await ReloadEventList();
    }

    public async Task ReloadEventList()
    {
        EventListSorted.Clear();

        await UpdateListViewModel.Initialize();

        List<EventModel> updates = EventList
            .OrderBy(GetStartDate)
            .ToList();

        foreach (EventModel update in updates)
        {
            EventListSorted.Add(update);
        }
    }

    public DateTimeOffset? GetStartDate(EventModel evt) => evt.StartOnUpdateId switch
    {
        { } id => UpdateListViewModel.UpdateList.Find(upd => upd.Id == id)?.UpdateDate,
        _ => null,
    };

    [RelayCommand]
    public async Task AddEvent()
    {
        EventModel model = new();
        EventViewModel.Model = model;
        await EventViewModel.LoadFromModel();

        if (await ShowEditDialog.Handle(EventViewModel))
        {
            EventViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("Event", model);

            response.EnsureSuccessStatusCode();

            EventModel? postedModel = await response.Content.ReadFromJsonAsync<EventModel>();

            if (postedModel is not null)
            {
                EventList.Add(postedModel);

                await ReloadEventList();
            }
        }
    }

    [RelayCommand]
    public async Task EditEvent(EventModel vm)
    {
        EventViewModel.Model = vm;
        await EventViewModel.LoadFromModel();

        if (await ShowEditDialog.Handle(EventViewModel))
        {
            EventViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"Event", vm);

            response.EnsureSuccessStatusCode();

            await ReloadEventList();
        }
    }

    [RelayCommand]
    public async Task RemoveEvent(EventModel vm)
    {
        await HttpClient.DeleteAsync($"Event/{vm.Id}");

        EventList.Remove(vm);

        await ReloadEventList();
    }
}