using Avalonia.Controls;
using Avalonia.Interactivity;
using EOToolsWeb.ViewModels.Login;

namespace EOToolsWeb.Views.Login;

public partial class LoginView : Window
{
    public LoginViewModel? ViewModel => DataContext as LoginViewModel;

    public LoginView(LoginViewModel vm)
    {
        InitializeComponent();

        DataContext = vm;

        vm.OnAfterLogin += (_, _) => Close(true);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        ViewModel?.LoginFromConfig();
    }
}
