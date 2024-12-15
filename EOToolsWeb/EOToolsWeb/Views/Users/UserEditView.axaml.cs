using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EOToolsWeb.Views.Users;

public partial class UserEditView : Window
{
    public UserEditView()
    {
        InitializeComponent();
    }

    private void OnConfirmClick(object sender, RoutedEventArgs e)
    {
        Close(true);
    }

    private void OnCancelClick(object sender, RoutedEventArgs e)
    {
        Close(false);
    }
}