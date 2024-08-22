using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Models.Settings;
using ReactiveUI;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EOToolsWeb.ViewModels.Settings;

public partial class SettingsViewModel(SettingsModel settings, JsonSerializerOptions jsonOptions) : ViewModelBase
{
    private SettingsModel Settings { get; } = settings;

    [ObservableProperty]
    private string _kancolleEoApiFolder = "";

    [ObservableProperty]
    private string _eoApiUrl = "";

    [ObservableProperty]
    private string _eoApiKey = "";

    private string SettingsPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EOTools", "Configuration.json");

    public Interaction<string?, IStorageFolder?> ShowFolderPicker { get; } = new();

    public async Task Initialize()
    {
        if (Path.GetDirectoryName(SettingsPath) is not {} path) return;

        Directory.CreateDirectory(path);

        if (!File.Exists(SettingsPath)) return;

        SettingsModel? settings = JsonSerializer.Deserialize<SettingsModel>(await File.ReadAllTextAsync(SettingsPath));

        if (settings is null) return;

        Settings.KancolleEoApiFolder = settings.KancolleEoApiFolder;
        Settings.EoApiUrl = settings.EoApiUrl;
        Settings.EoApiKey = settings.EoApiKey;

        LoadFromModel();
    }

    public void LoadFromModel()
    {
        KancolleEoApiFolder = Settings.KancolleEoApiFolder;
        EoApiUrl = Settings.EoApiUrl;
        EoApiKey = Settings.EoApiKey;
    }

    public override async Task OnViewClosing()
    {
        await SaveChanges();

        await base.OnViewClosing();
    }

    private async Task SaveChanges()
    {
        Settings.KancolleEoApiFolder = KancolleEoApiFolder;
        Settings.EoApiUrl = EoApiUrl;
        Settings.EoApiKey = EoApiKey;

        await File.WriteAllTextAsync(SettingsPath, JsonSerializer.Serialize(Settings, jsonOptions));
    }

    [RelayCommand]
    private async Task OpenEoApiFolderPicker()
    {
        if (await ShowFolderPicker.Handle(null) is not {} folder) return;
        
        KancolleEoApiFolder = folder.TryGetLocalPath() ?? "";
    }
}
