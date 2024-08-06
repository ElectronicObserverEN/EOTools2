using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.ViewModels.Equipments;
using EOToolsWeb.ViewModels.EquipmentUpgrades;
using EOToolsWeb.ViewModels.Events;
using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.ViewModels.Settings;
using EOToolsWeb.ViewModels.ShipLocks;
using EOToolsWeb.ViewModels.Ships;
using EOToolsWeb.ViewModels.Translations;
using EOToolsWeb.ViewModels.Updates;
using EOToolsWeb.ViewModels.UseItem;

namespace EOToolsWeb.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public LoginViewModel Login { get; }

    public UpdateManagerViewModel Updates { get; }
    public UpdateListViewModel UpdateList { get; }

    public EventManagerViewModel Events { get; }
    public EventViewModel EventViewModel { get; }

    public ShipClassManagerViewModel ShipClassManager { get; }
    public ShipClassListViewModel ShipClassList { get; }

    public ShipManagerViewModel ShipManager { get; }

    public EquipmentManagerViewModel EquipmentManager { get; }
    public EquipmentViewModel EquipmentViewModel { get; }
    public EquipmentUpgradeImprovmentViewModel EquipmentUpgradeImprovmentViewModel { get; }
    public UseItemManagerViewModel UseItemManager { get; }

    public EquipmentPickerViewModel EquipmentPicker { get; }

    public ShipLocksManagerViewModel ShipLocksManager { get; }

    public TranslationManagerViewModel TranslationManager { get; }

    public SettingsViewModel Settings { get; }

    [ObservableProperty]
    private ViewModelBase? _currentViewModel;

    /// <inheritdoc/>
    public MainViewModel(LoginViewModel login, 
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
        EquipmentPickerViewModel equipmentPicker,
        UseItemManagerViewModel useItemManager,
        ShipLocksManagerViewModel shipLockManager,
        SettingsViewModel settings,
        TranslationManagerViewModel translations)
    {
        Login = login;
        Updates = updates;
        UpdateList = updateList;
        Events = events;
        EventViewModel = eventViewModel;
        ShipClassManager = shipClassManager;
        ShipClassList = shipClassList;
        ShipManager = shipManager;
        EquipmentManager = equipmentManager;
        EquipmentViewModel = equipmentViewModel;
        EquipmentUpgradeImprovmentViewModel = equipmentUpgradeViewModel;
        UseItemManager = useItemManager;
        EquipmentPicker = equipmentPicker;
        ShipLocksManager = shipLockManager;
        TranslationManager = translations;
        Settings = settings;

        PropertyChanging += ViewModelChanging;
    }

    private async void ViewModelChanging(object? sender, System.ComponentModel.PropertyChangingEventArgs e)
    {
        if (e.PropertyName is not nameof(CurrentViewModel)) return;

        if (CurrentViewModel is null) return;

        await CurrentViewModel.OnViewClosing();
    }

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

    [RelayCommand]
    private async Task OpenTranslationsManager()
    {
        CurrentViewModel = TranslationManager;
        await TranslationManager.LoadTranslations();
    }

    [RelayCommand]
    private async Task OpenSettings()
    {
        CurrentViewModel = Settings;
    }
}
