using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Models.ShipLocks;
using EOToolsWeb.Shared.Events;
using EOToolsWeb.ViewModels.Events;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.ShipLocks;

public partial class ShipLocksManagerViewModel : ViewModelBase
{
    private EventManagerViewModel EventManager { get; }
    private HttpClient HttpClient { get; }

    [ObservableProperty]
    private EventModel? _selectedEvent;

    [ObservableProperty]
    private ObservableCollection<ShipLockEditRowModel> _locks = [];

    [ObservableProperty]
    private ObservableCollection<ShipLockPhaseEditRowModel> _phases = [];

    [ObservableProperty]
    private List<EventModel> _eventList = [];

    private ShipLockViewModel ShipLockViewModel { get; }
    private ShipLockPhaseViewModel ShipLockPhaseViewModel { get; }

    public Interaction<ShipLockViewModel, bool> ShowShipLockEditDialog { get; } = new();
    public Interaction<ShipLockPhaseViewModel, bool> ShowShipLockPhaseEditDialog { get; } = new();

    public ShipLocksManagerViewModel(EventManagerViewModel events, HttpClient client, ShipLockViewModel viewModel, ShipLockPhaseViewModel phaseViewModel)
    {
        EventManager = events;
        HttpClient = client;
        ShipLockViewModel = viewModel;
        ShipLockPhaseViewModel = phaseViewModel;

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

        List<ShipLockEditRowModel> locks = await HttpClient.GetFromJsonAsync<List<ShipLockEditRowModel>>($"ShipLock?eventId={SelectedEvent.Id}") ?? [];
        List<ShipLockPhaseEditRowModel> phases = await HttpClient.GetFromJsonAsync<List<ShipLockPhaseEditRowModel>>($"ShipLockPhase?eventId={SelectedEvent.Id}") ?? [];

        Locks = new(locks.OrderBy(locks => locks.ApiId));
        Phases = new(phases.OrderBy(locks => locks.SortId));
    }

    public async Task Initialize()
    {
        await EventManager.LoadAllEvents();

        EventList = EventManager.EventList;

        SelectedEvent = null;
    }

    [RelayCommand]
    private async Task AddLock()
    {
        if (SelectedEvent is null) return;

        ShipLockEditRowModel model = new()
        {
            EventId = SelectedEvent.Id,
        };

        ShipLockViewModel.Model = model;
        ShipLockViewModel.LoadFromModel();

        if (await ShowShipLockEditDialog.Handle(ShipLockViewModel))
        {
            ShipLockViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("ShipLock", model);

            response.EnsureSuccessStatusCode();

            ShipLockEditRowModel? postedModel = await response.Content.ReadFromJsonAsync<ShipLockEditRowModel>();

            if (postedModel is not null)
            {
                Locks.Add(postedModel);
            }
        }
    }

    [RelayCommand]
    private async Task EditLock(ShipLockEditRowModel model)
    {
        ShipLockViewModel.Model = model;
        ShipLockViewModel.LoadFromModel();

        if (await ShowShipLockEditDialog.Handle(ShipLockViewModel))
        {
            ShipLockViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("ShipLock", model);

            response.EnsureSuccessStatusCode();
        }
    }

    [RelayCommand]
    private async Task RemoveLock(ShipLockEditRowModel vm)
    {
        await HttpClient.DeleteAsync($"ShipLock/{vm.Id}");

        Locks.Remove(vm);
    }

    [RelayCommand]
    private async Task AddPhase()
    {
        if (SelectedEvent is null) return;

        ShipLockPhaseEditRowModel model = new()
        {
            EventId = SelectedEvent.Id,
        };

        ShipLockPhaseViewModel.Initialize(Locks.ToList());
        ShipLockPhaseViewModel.Model = model;
        ShipLockPhaseViewModel.LoadFromModel();

        if (await ShowShipLockPhaseEditDialog.Handle(ShipLockPhaseViewModel))
        {
            ShipLockPhaseViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("ShipLockPhase", model);

            response.EnsureSuccessStatusCode();

            ShipLockPhaseEditRowModel? postedModel = await response.Content.ReadFromJsonAsync<ShipLockPhaseEditRowModel>();

            if (postedModel is not null)
            {
                Phases.Add(postedModel);
            }
        }
    }

    [RelayCommand]
    private async Task EditPhase(ShipLockPhaseEditRowModel model)
    {
        ShipLockPhaseViewModel.Initialize(Locks.ToList());

        ShipLockPhaseViewModel.Model = model;
        ShipLockPhaseViewModel.LoadFromModel();

        if (await ShowShipLockPhaseEditDialog.Handle(ShipLockPhaseViewModel))
        {
            ShipLockPhaseViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("ShipLockPhase", model);

            response.EnsureSuccessStatusCode();
        }
    }

    [RelayCommand]
    private async Task RemovePhase(ShipLockPhaseEditRowModel vm)
    {
        await HttpClient.DeleteAsync($"ShipLockPhase/{vm.Id}");

        Phases.Remove(vm);
    }

    [RelayCommand]
    private async Task PushLocks()
    {
        await HttpClient.PutAsync("ShipLock/pushLockData", null);
    }
}