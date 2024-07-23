using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.ViewModels.Events;
using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.ViewModels.Ships;
using EOToolsWeb.ViewModels.Updates;

namespace EOToolsWeb.ViewModels;

public partial class MainViewModel(LoginViewModel login, UpdateManagerViewModel updates, EventManagerViewModel events, UpdateListViewModel updateList, EventViewModel eventViewModel, ShipClassManagerViewModel shipClassManager) : ViewModelBase
{
    public LoginViewModel Login { get; } = login;

    public UpdateManagerViewModel Updates { get; } = updates;
    public UpdateListViewModel UpdateList { get; } = updateList;

    public EventManagerViewModel Events { get; } = events;
    public EventViewModel EventViewModel { get; } = eventViewModel;

    public ShipClassManagerViewModel ShipClassManager { get; } = shipClassManager;

    [ObservableProperty]
    private ViewModelBase? _currentViewModel;

    [RelayCommand]
    private async Task OpenUpdates()
    {
        CurrentViewModel = Updates;
        await Updates.LoadAllUpdates();
    }

    [RelayCommand]
    private async Task OpenEvents()
    {
        CurrentViewModel = Events;
        await Events.LoadAllEvents();
    }

    [RelayCommand]
    private async Task OpenShipClasses()
    {
        CurrentViewModel = ShipClassManager;
        await ShipClassManager.LoadAllClasses();
    }
}
