using Avalonia.Controls;
using EOToolsWeb.ViewModels.Users;

namespace EOToolsWeb.Views.Users;

public partial class UsersManagerView : UserControl
{
    public UsersManagerViewModel? ViewModel => DataContext as UsersManagerViewModel;

    public UsersManagerView()
    {
        InitializeComponent();
    }
}