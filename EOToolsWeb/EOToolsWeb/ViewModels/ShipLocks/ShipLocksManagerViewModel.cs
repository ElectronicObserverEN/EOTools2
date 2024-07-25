using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Models;
using EOToolsWeb.Shared.Events;
using EOToolsWeb.ViewModels.Events;

namespace EOToolsWeb.ViewModels.ShipLocks;

public partial class ShipLocksManagerViewModel : ViewModelBase
{
    private EventManagerViewModel EventManager { get; }
    private HttpClient HttpClient { get; }

    [ObservableProperty]
    private EventModel? _selectedEvent;

    [ObservableProperty]
    private List<ShipLockEditRowModel> _locks = [];

    [ObservableProperty]
    private List<ShipLockPhaseEditRowModel> _phases = [];

    [ObservableProperty]
    private List<EventModel> _eventList = [];

    public ShipLocksManagerViewModel(EventManagerViewModel events, HttpClient client)
    {
        EventManager = events;
        HttpClient = client;

        PropertyChanged += OnEventChanged;
    }

    private async void OnEventChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedEvent)) return;

        if (SelectedEvent is null)
        {
            Locks = [];
            Phases = [];
            return;
        }

        Locks = await HttpClient.GetFromJsonAsync<List<ShipLockEditRowModel>>($"ShipLock?eventId={SelectedEvent.Id}") ?? [];
        Phases = await HttpClient.GetFromJsonAsync<List<ShipLockPhaseEditRowModel>>($"ShipLockPhase?eventId={SelectedEvent.Id}") ?? [];
    }

    public async Task Initialize()
    {
        await EventManager.LoadAllEvents();

        EventList = EventManager.EventList;

        SelectedEvent = null;
    }
}