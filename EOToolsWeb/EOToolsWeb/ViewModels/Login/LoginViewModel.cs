using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.ViewModels.Settings;

namespace EOToolsWeb.ViewModels.Login;

public partial class LoginViewModel : ObservableObject
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    public event EventHandler? OnAfterLogin;

    [ObservableProperty]
    private string _loginMessage = "";

    private HttpClient ClientApi { get; }
    private SettingsViewModel Settings { get; }

    public LoginViewModel(HttpClient clientApi, SettingsViewModel settings)
    {
        if (!File.Exists("Config.json"))
        {
            LoginMessage = "Config file error";
        }

        ClientApi = clientApi;
        Settings = settings;
    }

    public async Task LoginFromConfig()
    {
        if (!File.Exists("../UpdaterConfig.json")) return;

        Dictionary<string, string>? config = JsonSerializer.Deserialize<Dictionary<string, string>>(await File.ReadAllTextAsync("../UpdaterConfig.json"));

        string login = config?["login"] ?? "";
        string password = config?["password"] ?? "";

        if (string.IsNullOrEmpty(login)) return;

        LoginMessage = "Login in from config ...";

        Username = login;
        Password = password;

        await LoginCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task Login()
    {
        Dictionary<string, string>? config = JsonSerializer.Deserialize<Dictionary<string, string>>(await File.ReadAllTextAsync("Config.json"));

        string url = config?["serverUrl"] ?? "";

        string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Username}:{Password}"));

        HttpClient apiClient = new();
        apiClient.DefaultRequestHeaders.Authorization = new("Basic", base64);

        try
        {
            HttpResponseMessage response = await apiClient.GetAsync($"{url}Login");

            response.EnsureSuccessStatusCode();

            string token = await response.Content.ReadAsStringAsync();
            LoginMessage = "";

            ClientApi.BaseAddress = new Uri(url);
            ClientApi.DefaultRequestHeaders.Authorization = new("Basic", token);

            LoginMessage = "Loading settings ...";
            await Settings.Initialize();

            OnAfterLogin?.Invoke(this, null);
        }
        catch (HttpRequestException ex)
        {
            LoginMessage = $"{ex.StatusCode} ({(int?)ex.StatusCode}): {ex.Message}";
        }
        catch (Exception ex)
        {
            LoginMessage = ex.Message;
        }
    }
}

