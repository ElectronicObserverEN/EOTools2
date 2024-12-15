using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Services;
using EOToolsWeb.Shared.Users;
using EOToolsWeb.ViewModels.Settings;

namespace EOToolsWeb.ViewModels.Login;

public partial class LoginViewModel : ObservableObject
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    [ObservableProperty]
    public partial bool StayLoggedIn { get; set; }

    public event EventHandler? OnAfterLogin;

    [ObservableProperty]
    private string _loginMessage = "";

    private HttpClient ClientApi { get; }
    private SettingsViewModel Settings { get; }
    private ICurrentSession Session { get; }

    public LoginViewModel(HttpClient clientApi, SettingsViewModel settings, ICurrentSession session)
    {
        if (!File.Exists("Config.json"))
        {
            LoginMessage = "Config file error";
        }

        ClientApi = clientApi;
        Settings = settings;
        Session = session;
    }

    public async Task LoginFromConfig()
    {
        if (!File.Exists("../UpdaterConfig.json")) return;

        Dictionary<string, string>? config = JsonSerializer.Deserialize<Dictionary<string, string>>(await File.ReadAllTextAsync("../UpdaterConfig.json"));

        string token = config?["token"] ?? "";

        if (string.IsNullOrEmpty(token)) return;

        StayLoggedIn = true;
        LoginMessage = "Login in ...";

        try
        {
            config = JsonSerializer.Deserialize<Dictionary<string, string>>(await File.ReadAllTextAsync("Config.json"));

            string url = config?["serverUrl"] ?? "";

            ClientApi.BaseAddress = new Uri(url);
            ClientApi.DefaultRequestHeaders.Add("X-TOKEN-EO-TOOLS-WEB-X", token);

            LoginMessage = "Loading settings ...";

            UserModel? user = await GetCurrentUser();

            if (user is null)
            {
                LoginMessage = "Error while loading user data";
                return;
            }

            Session.User = user;

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

    private async Task<UserModel?> GetCurrentUser() => await ClientApi.GetFromJsonAsync<UserModel>("Users/currentUser");

    private async Task SaveTokenForNextLogin(string token)
    {
        if (!StayLoggedIn) return;

        Dictionary<string, object> json = [];

        if (File.Exists("../UpdaterConfig.json"))
        {
            json = JsonSerializer.Deserialize<Dictionary<string, object>>(await File.ReadAllTextAsync("../UpdaterConfig.json")) ?? [];
        }

        if (!json.TryAdd("token", token))
        {
            json["token"] = token;
        }

        await File.WriteAllTextAsync("../UpdaterConfig.json", JsonSerializer.Serialize(json));
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
            ClientApi.DefaultRequestHeaders.Add("X-TOKEN-EO-TOOLS-WEB-X", token);

            LoginMessage = "Loading settings ...";

            UserModel? user = await GetCurrentUser();

            if (user is null)
            {
                LoginMessage = "Error while loading user data";
                return;
            }

            Session.User = user;

            await SaveTokenForNextLogin(token);

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

