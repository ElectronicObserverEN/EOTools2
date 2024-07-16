using Avalonia.Controls;
using EOToolsWeb.ViewModels.Login;

namespace EOToolsWeb.Views.Login;

public partial class LoginView : Window
{
    public LoginView(LoginViewModel vm)
    {
        InitializeComponent();

        DataContext = vm;

        vm.OnAfterLogin += (_, _) => Close(true);
    }
}
