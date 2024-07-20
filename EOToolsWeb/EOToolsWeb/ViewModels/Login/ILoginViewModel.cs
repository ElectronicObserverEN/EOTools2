using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace EOToolsWeb.ViewModels.Login;

public interface ILoginViewModel : IViewModelBase
{
    public string Username { get; set; }
    public string Password { get; set; }

    public event EventHandler? OnAfterLogin;

    public bool IsLoggedIn { get; set; }

    public string LoginMessage { get; set; }

    public Task Login();
    public IAsyncRelayCommand LoginCommand { get; }
}