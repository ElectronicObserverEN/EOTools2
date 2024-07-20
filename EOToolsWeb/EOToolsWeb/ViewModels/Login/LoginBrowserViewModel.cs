using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Models;

namespace EOToolsWeb.ViewModels.Login;

public partial class LoginBrowserViewModel(HttpClient clientApi) : ViewModelBase, ILoginViewModel
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    public event EventHandler? OnAfterLogin;

    [ObservableProperty]
    private bool _isLoggedIn = false;

    [ObservableProperty]
    private string _loginMessage = "";

    private HttpClient ClientApi { get; } = clientApi;

    [RelayCommand]
    public async Task Login()
    {
        try
        {
            Stream stream = AssetLoader.Open(new Uri("avares://EOToolsWeb/Assets/Config.json"));

            ConfigModel? config = await JsonSerializer.DeserializeAsync<ConfigModel>(stream, new JsonSerializerOptions()
            {
                TypeInfoResolver = SourceGenerationContext.Default,
            });

            stream.Close();

            if (config is null)
            {
                LoginMessage = "Error loading config";
                return;
            }
            
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Username}:{Password}"));

            HttpClient apiClient = new();
            apiClient.DefaultRequestHeaders.Authorization = new("Basic", base64);

            HttpResponseMessage response = await apiClient.GetAsync($"{config.ServerUrl}Login");

            response.EnsureSuccessStatusCode();

            string token = await response.Content.ReadAsStringAsync();
            LoginMessage = "";

            ClientApi.BaseAddress = new Uri(config.ServerUrl);
            ClientApi.DefaultRequestHeaders.Authorization = new("Basic", token);

            IsLoggedIn = true;
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

