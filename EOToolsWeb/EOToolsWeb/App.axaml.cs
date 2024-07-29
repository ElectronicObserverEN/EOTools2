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
using EOToolsWeb.ViewModels.Equipments;
using EOToolsWeb.ViewModels.Events;
using EOToolsWeb.ViewModels.ShipLocks;
using EOToolsWeb.ViewModels.Ships;
using EOToolsWeb.ViewModels.EquipmentUpgrades;

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

            // Locks
            collection.AddScoped<ShipLocksManagerViewModel>();
            collection.AddScoped<ShipLockViewModel>();
            collection.AddScoped<ShipLockPhaseViewModel>();

            collection.AddSingleton<HttpClient>();

            // Creates a ServiceProvider containing services from the provided IServiceCollection
            var services = collection.BuildServiceProvider();

            var vm = services.GetRequiredService<MainViewModel>();

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