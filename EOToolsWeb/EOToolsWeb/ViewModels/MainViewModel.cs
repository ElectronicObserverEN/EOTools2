using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.ViewModels.Updates;

namespace EOToolsWeb.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public LoginViewModel Login { get; }
    public UpdateManagerViewModel Updates { get; }

    [ObservableProperty]
    private ViewModelBase? _currentViewModel;

    public MainViewModel(LoginViewModel login, UpdateManagerViewModel updates)
    {
        Login = login;
        Updates = updates;
    }

    [RelayCommand]
    private void OpenUpdates()
    {
        CurrentViewModel = Updates;
        Updates.LoadAllUpdates();
    }
}
