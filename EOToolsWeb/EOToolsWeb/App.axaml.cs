using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using EOToolsWeb.ViewModels;
using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.ViewModels.Updates;
using EOToolsWeb.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using EOToolsWeb.Models.Settings;
using EOToolsWeb.ViewModels.Equipments;
using EOToolsWeb.ViewModels.Events;
using EOToolsWeb.ViewModels.ShipLocks;
using EOToolsWeb.ViewModels.Ships;
using EOToolsWeb.ViewModels.EquipmentUpgrades;
using EOToolsWeb.ViewModels.Settings;
using EOToolsWeb.ViewModels.UseItem;
using EOToolsWeb.ViewModels.Translations;
using System.Text.Encodings.Web;
using System.Text.Json;
using EOToolsWeb.Services;
using EOToolsWeb.Shared.Sessions;
using EOToolsWeb.ViewModels.ApplicationLog;
using EOToolsWeb.ViewModels.FitBonus;
using EOToolsWeb.ViewModels.Quests;
using EOToolsWeb.ViewModels.Seasons;
using EOToolsWeb.ViewModels.Users;

namespace EOToolsWeb
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // If you use CommunityToolkit, line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            // Register all the services needed for the application to run
            ServiceCollection collection = new();
            collection.AddTransient<MainViewModel>();
            collection.AddTransient<LoginViewModel>();

            // Updates
            collection.AddScoped<UpdateManagerViewModel>();
            collection.AddScoped<UpdateListViewModel>();

            // Events
            collection.AddScoped<EventViewModel>();
            collection.AddScoped<EventManagerViewModel>();

            // Ship class
            collection.AddScoped<ShipClassManagerViewModel>();
            collection.AddScoped<ShipClassViewModel>();
            collection.AddScoped<ShipClassListViewModel>();

            // Ships
            collection.AddScoped<ShipManagerViewModel>();
            collection.AddScoped<ShipViewModel>();

            // Equipments
            collection.AddScoped<EquipmentManagerViewModel>();
            collection.AddScoped<EquipmentViewModel>();
            collection.AddScoped<EquipmentPickerViewModel>();

            // Equipment Upgrades
            collection.AddScoped<EquipmentUpgradeImprovmentViewModel>();

            // Use item
            collection.AddScoped<UseItemManagerViewModel>();

            // Locks
            collection.AddScoped<ShipLocksManagerViewModel>();
            collection.AddScoped<ShipLockViewModel>();
            collection.AddScoped<ShipLockPhaseViewModel>();

            // Translations
            collection.AddScoped<TranslationManagerViewModel>();
            collection.AddScoped<TranslationViewModelOld>();
            collection.AddScoped<ShipTranslationManager>();
            collection.AddScoped<MapTranslationManager>();

            // Fit bonus
            collection.AddScoped<FitBonusCheckerViewModel>();
            collection.AddScoped<FitBonusIssuesFetcher>();

            // Seasons
            collection.AddScoped<SeasonManagerViewModel>();
            collection.AddScoped<SeasonListViewModel>();
            collection.AddScoped<SeasonViewModel>();

            // Quests
            collection.AddScoped<QuestManagerViewModel>();
            collection.AddScoped<QuestViewModel>();

            // Settings
            collection.AddScoped<SettingsModel>();
            collection.AddScoped<SettingsViewModel>();

            // Users
            collection.AddScoped<UsersManagerViewModel>();
            collection.AddScoped<UserViewModel>();
            collection.AddScoped<ICurrentSession, CurrentSession>();

            // Logs
            collection.AddScoped<ApplicationLogsManagerViewModel>();

            collection.AddSingleton<HttpClient>();

            collection.AddScoped<IAvaloniaShowDialogService, ShowDialogService>();

            collection.AddSingleton(_ => new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true,
            });

            // Creates a ServiceProvider containing services from the provided IServiceCollection
            var services = collection.BuildServiceProvider();

            var vm = services.GetRequiredService<MainViewModel>();
            var messages = services.GetRequiredService<IAvaloniaShowDialogService>();

            vm.ShowDialogService = messages;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow()
                {
                    DataContext = vm
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = vm
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}