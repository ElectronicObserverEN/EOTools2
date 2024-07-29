using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.ViewModels.Equipments;
using EOToolsWeb.ViewModels.EquipmentUpgrades;
using EOToolsWeb.ViewModels.Events;
using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.ViewModels.ShipLocks;
using EOToolsWeb.ViewModels.Ships;
using EOToolsWeb.ViewModels.Updates;

namespace EOToolsWeb.ViewModels;

public partial class MainViewModel(
    LoginViewModel login, 
    UpdateManagerViewModel updates, 
    EventManagerViewModel events, 
    UpdateListViewModel updateList, 
    EventViewModel eventViewModel, 
    ShipClassManagerViewModel shipClassManager, 
    ShipManagerViewModel shipManager, 
    ShipClassListViewModel shipClassList, 
    EquipmentManagerViewModel equipmentManager,
    EquipmentViewModel equipmentViewModel,
    EquipmentUpgradeImprovmentViewModel equipmentUpgradeViewModel,
    ShipLocksManagerViewModel shipLockManager
) : ViewModelBase
{
    public LoginViewModel Login { get; } = login;

    public UpdateManagerViewModel Updates { get; } = updates;
    public UpdateListViewModel UpdateList { get; } = updateList;

    public EventManagerViewModel Events { get; } = events;
    public EventViewModel EventViewModel { get; } = eventViewModel;

    public ShipClassManagerViewModel ShipClassManager { get; } = shipClassManager;
    public ShipClassListViewModel ShipClassList { get; } = shipClassList;

    public ShipManagerViewModel ShipManager { get; } = shipManager;

    public EquipmentManagerViewModel EquipmentManager { get; } = equipmentManager;
    public EquipmentViewModel EquipmentViewModel { get; } = equipmentViewModel;
    public EquipmentUpgradeImprovmentViewModel EquipmentUpgradeImprovmentViewModel { get; } = equipmentUpgradeViewModel;

    public ShipLocksManagerViewModel ShipLocksManager { get; } = shipLockManager;

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

    [RelayCommand]
    private async Task OpenShips()
    {
        CurrentViewModel = ShipManager;
        await ShipManager.LoadAllShips();
    }

    [RelayCommand]
    private async Task OpenEquipments()
    {
        CurrentViewModel = EquipmentManager;
        await EquipmentManager.LoadAllEquipments();
    }

    [RelayCommand]
    private async Task OpenShipLockManager()
    {
        CurrentViewModel = ShipLocksManager;
        await ShipLocksManager.Initialize();
    }
}
