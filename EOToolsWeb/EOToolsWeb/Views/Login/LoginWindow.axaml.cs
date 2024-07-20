using Avalonia.Controls;
using Avalonia.Interactivity;
using EOToolsWeb.ViewModels.Login;

namespace EOToolsWeb.Views.Login;

public partial class LoginWindow : Window
{
    private ILoginViewModel? ViewModel => DataContext as ILoginViewModel;

    public LoginWindow()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (ViewModel is null) return;

        ViewModel.OnAfterLogin += (_, _) => Close(true);
    }
}
