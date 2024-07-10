using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EOToolsWeb.ViewModels.Login;

public partial class LoginViewModel : ObservableObject
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    [ObservableProperty]
    private string _loginMessage = "";

    public LoginViewModel()
    {
        if (!File.Exists("Config.json"))
        {
            LoginMessage = "Config file error";
        }
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

            LoginMessage = $"Success, token is {await response.Content.ReadAsStringAsync()}";
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

