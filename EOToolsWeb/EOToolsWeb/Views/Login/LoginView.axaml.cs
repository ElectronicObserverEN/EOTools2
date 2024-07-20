using Avalonia.Controls;
using EOToolsWeb.ViewModels.Login;

namespace EOToolsWeb.Views.Login;

public partial class LoginView : UserControl
{
    private ILoginViewModel? ViewModel => DataContext as ILoginViewModel;

    public LoginView()
    {
        InitializeComponent();
    }
}
