using System;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.ViewModels.Equipments;
using EOToolsWeb.ViewModels.EquipmentUpgrades;
using EOToolsWeb.ViewModels.Events;
using EOToolsWeb.ViewModels.FitBonus;
using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.ViewModels.Quests;
using EOToolsWeb.ViewModels.Seasons;
using EOToolsWeb.ViewModels.Settings;
using EOToolsWeb.ViewModels.ShipLocks;
using EOToolsWeb.ViewModels.Ships;
using EOToolsWeb.ViewModels.Translations;
using EOToolsWeb.ViewModels.Updates;
using EOToolsWeb.ViewModels.UseItem;
using EOToolsWeb.ViewModels.Users;
using Microsoft.Extensions.DependencyInjection;

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
    public MapTranslationManager MapTranslationManager { get; }

    public FitBonusCheckerViewModel FitBonusChecker { get; }

    public QuestManagerViewModel QuestManager { get; }

    public SeasonManagerViewModel SeasonManager { get; }
    public SeasonListViewModel SeasonList { get; }

    public SettingsViewModel Settings { get; }
    
    private IServiceProvider ServiceProvider { get; }

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
        TranslationManagerViewModel translations,
        FitBonusCheckerViewModel fitBonusChecker,
        SeasonManagerViewModel seasonManager,
        SeasonListViewModel seasonList,
        QuestManagerViewModel quests,
        MapTranslationManager mapTranslationManager,
        IServiceProvider provider)
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
        MapTranslationManager = mapTranslationManager;
        Settings = settings;
        FitBonusChecker = fitBonusChecker;
        SeasonManager = seasonManager;
        SeasonList = seasonList;
        QuestManager = quests;

        ServiceProvider = provider;

        PropertyChanging += ViewModelChanging;
        PropertyChanged += ViewModelChanged;
    }

    private void ViewModelChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(CurrentViewModel)) return;

        if (CurrentViewModel is null) return;

        CurrentViewModel.ShowDialogService = ShowDialogService;
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
    private async Task OpenSeasonManager()
    {
        CurrentViewModel = SeasonManager;
        await SeasonManager.LoadSeasonList();
    }

    [RelayCommand]
    private async Task OpenFitBonusChecker()
    {
        await FitBonusChecker.Initialize();
        CurrentViewModel = FitBonusChecker;
    }

    [RelayCommand]
    private async Task OpenQuestManager()
    {
        await QuestManager.LoadQuests();
        CurrentViewModel = QuestManager;
    }
    
    [RelayCommand]
    private void OpenSettings()
    {
        CurrentViewModel = Settings;
    }

    [RelayCommand]
    private async Task OpenUsers()
    {
        UsersManagerViewModel vm = ServiceProvider.GetRequiredService<UsersManagerViewModel>();
        await vm.LoadAllUsers();

        CurrentViewModel = vm;
    }

    [RelayCommand]
    private async Task UpdateMapsTranslation()
    {
        await MapTranslationManager.Initialize();
        await MapTranslationManager.GetNonTranslatedDataAndPushItToTheApi();
    }
}
