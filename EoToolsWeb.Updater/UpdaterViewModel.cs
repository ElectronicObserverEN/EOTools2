using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;

namespace EoToolsWeb.Updater;

public partial class UpdaterViewModel : ObservableObject
{
    [ObservableProperty]
    private string _currentVersion = "???";

    [ObservableProperty]
    private string _latestVersion = "???";

    [ObservableProperty]
    private string _updateText = "";

    private string ErrorText { get; set; } = "";

    public bool UpdateDone { get; set; } = false;

    private string ServerUrl { get; set; } = "";

    public async Task CheckForUpdate()
    {
        UpdateText = "Checking for update...";

        Dictionary<string, string>? config = JsonSerializer.Deserialize<Dictionary<string, string>>(await File.ReadAllTextAsync("UpdaterConfig.json"));

        ServerUrl = config?["serverUrl"] ?? "";

        CurrentVersion = GetAssemblyVersion();
        LatestVersion = await GetLatestVersion();
        
        if (LatestVersion is "???")
        {
            return;
        }

        if (CurrentVersion == LatestVersion)
        {
            UpdateText = "Already up to date !";
            UpdateDone = true;
            return;
        }

        await DownloadUpdate();
        ExtractUpdate();

        if (UpdateDone)
        {
            UpdateText = "Update complete !";
        }
    }

    private void ExtractUpdate()
    {
        try
        {
            if (!File.Exists("./EoTools.zip")) return;

            UpdateText = "Deleting previous version...";

            if (Directory.Exists("./EoTools"))
            {
                Directory.Delete("./EoTools", true);
            }

            UpdateText = "Extracting Update...";

            ZipFile.ExtractToDirectory("EoTools.zip", "./EoTools");

            UpdateDone = true;
        }
        catch (Exception ex)
        {
            ErrorText = ex.Message;
            UpdateText = "Failed to extract update";
        }
    }

    private async Task DownloadUpdate()
    {
        try
        {
            HttpClient client = new();

            UpdateText = "Downloading Update...";

            HttpResponseMessage response = await client.GetAsync($"{ServerUrl}Tools/GetUpdate");
            response.EnsureSuccessStatusCode();

            await using Stream stream = await response.Content.ReadAsStreamAsync();
            File.Delete("EoTools.zip");
            await using Stream destination = File.OpenWrite("EoTools.zip");
            await stream.CopyToAsync(destination);
        }
        catch (Exception ex)
        {
            ErrorText = ex.Message;
            UpdateText = "Failed to download update";
        }
    }

    private async Task<string> GetLatestVersion()
    {
        try
        {
            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync($"{ServerUrl}Tools/GetVersion");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            ErrorText = ex.Message;
            UpdateText = "Failed to get latest version number";
            return "???";
        }
    }

    private string GetAssemblyVersion()
    {
        try
        {
            FileVersionInfo infos = FileVersionInfo.GetVersionInfo("./EoTools/EOToolsWeb.dll");
            return infos.FileVersion ?? "0.0.0.0";
        }
        catch
        {
            return "0.0.0.0";
        }
    }

    [RelayCommand]
    private async Task CopyError()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { Clipboard: {} } window })
        {
            await window.Clipboard.SetTextAsync(ErrorText);
        }
    }
}