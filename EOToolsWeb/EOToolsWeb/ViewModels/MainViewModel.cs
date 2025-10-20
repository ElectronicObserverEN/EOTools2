using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Services;
using EOToolsWeb.Shared.Sessions;
using EOToolsWeb.Shared.Users;
using EOToolsWeb.ViewModels.ApplicationLog;
using EOToolsWeb.ViewModels.Equipments;
using EOToolsWeb.ViewModels.EquipmentUpgrades;
using EOToolsWeb.ViewModels.Events;
using EOToolsWeb.ViewModels.FitBonus;
using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.ViewModels.MapEditor;
using EOToolsWeb.ViewModels.Quests;
using EOToolsWeb.ViewModels.Seasons;
using EOToolsWeb.ViewModels.Settings;
using EOToolsWeb.ViewModels.ShipLocks;
using EOToolsWeb.ViewModels.Ships;
using EOToolsWeb.ViewModels.Translations;
using EOToolsWeb.ViewModels.Updates;
using EOToolsWeb.ViewModels.UseItem;
using EOToolsWeb.ViewModels.Users;
using EOToolsWeb.Views.Login;
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
    
    public MapEditorViewModel MapEditor { get; }

    private UsersManagerViewModel UsersManager { get; }
    private ICurrentSession CurrentSession { get; }

    private IServiceProvider ServiceProvider { get; }

    public UserKind CurrentUserKind => CurrentSession.User?.Kind ?? UserKind.Contributor;

    [ObservableProperty]
    public partial ViewModelBase? CurrentViewModel { get; set; }

    public Action CloseApplication = () => {};
    
    /// <inheritdoc/>
    public MainViewModel(IServiceProvider provider)
    {
        Login = provider.GetRequiredService<LoginViewModel>(); 
        Updates = provider.GetRequiredService<UpdateManagerViewModel>(); 
        UpdateList = provider.GetRequiredService<UpdateListViewModel>(); 
        Events = provider.GetRequiredService<EventManagerViewModel>(); 
        EventViewModel = provider.GetRequiredService<EventViewModel>(); 
        ShipClassManager = provider.GetRequiredService<ShipClassManagerViewModel>(); 
        ShipClassList = provider.GetRequiredService<ShipClassListViewModel>();
        ShipManager = provider.GetRequiredService<ShipManagerViewModel>(); 
        EquipmentManager = provider.GetRequiredService<EquipmentManagerViewModel>();
        EquipmentViewModel = provider.GetRequiredService<EquipmentViewModel>();
        EquipmentUpgradeImprovmentViewModel = provider.GetRequiredService<EquipmentUpgradeImprovmentViewModel>();
        UseItemManager = provider.GetRequiredService<UseItemManagerViewModel>();
        EquipmentPicker = provider.GetRequiredService<EquipmentPickerViewModel>();
        ShipLocksManager = provider.GetRequiredService<ShipLocksManagerViewModel>();
        Settings = provider.GetRequiredService<SettingsViewModel>();
        TranslationManager = provider.GetRequiredService<TranslationManagerViewModel>();
        FitBonusChecker = provider.GetRequiredService<FitBonusCheckerViewModel>();
        SeasonManager = provider.GetRequiredService<SeasonManagerViewModel>();
        SeasonList = provider.GetRequiredService<SeasonListViewModel>();
        QuestManager = provider.GetRequiredService<QuestManagerViewModel>();
        MapTranslationManager = provider.GetRequiredService<MapTranslationManager>();
        UsersManager = provider.GetRequiredService<UsersManagerViewModel>();
        CurrentSession = provider.GetRequiredService<ICurrentSession>();
        MapEditor = provider.GetRequiredService<MapEditorViewModel>();
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
        await UsersManager.LoadAllUsers();

        CurrentViewModel = UsersManager;
    }

    [RelayCommand]
    private async Task UpdateMapsTranslation()
    {
        await MapTranslationManager.Initialize();
        await MapTranslationManager.GetNonTranslatedDataAndPushItToTheApi();
    }

    [RelayCommand]
    private async Task OpenMapEditor()
    {
        CurrentViewModel = MapEditor;
    }
    
    [RelayCommand]
    private async Task OpenUserSettings()
    {
        await UsersManager.EditCurrentUser();
    }

    [RelayCommand]
    private async Task OpenLogView()
    {
        ApplicationLogsManagerViewModel logs = ServiceProvider.GetRequiredService<ApplicationLogsManagerViewModel>();

        await logs.LoadAllLogs();
        CurrentViewModel = logs;
    }


    public async Task ShowLogInDialog()
    {
        CurrentSession.User = null;

        LoginView login = new(Login);

        if (ShowDialogService is null) return;

        if (await ShowDialogService.ShowWindow(login) is not true)
        {
            CloseApplication();
        }

        OnPropertyChanged(nameof(CurrentUserKind));
    }

    [RelayCommand]
    public async Task LogOut()
    {
        // Remove auto login
        Dictionary<string, object> json = [];

        if (File.Exists("../UpdaterConfig.json"))
        {
            json = JsonSerializer.Deserialize<Dictionary<string, object>>(await File.ReadAllTextAsync("../UpdaterConfig.json")) ?? [];
        }

        json.Remove("token");

        await File.WriteAllTextAsync("../UpdaterConfig.json", JsonSerializer.Serialize(json));

        // Restart app 
        // https://github.com/AvaloniaUI/Avalonia/discussions/15363
        string exePath = Process.GetCurrentProcess()!.MainModule!.FileName; 

        Process.Start(new ProcessStartInfo(exePath)
        {
            UseShellExecute = true,

        }); 

        Environment.Exit(0);
    }
}
