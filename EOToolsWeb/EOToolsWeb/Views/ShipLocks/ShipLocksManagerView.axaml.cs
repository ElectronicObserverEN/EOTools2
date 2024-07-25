using Avalonia.Controls;
using EOToolsWeb.ViewModels.ShipLocks;

namespace EOToolsWeb.Views.ShipLocks;

public partial class ShipLocksManagerView : UserControl
{
    public ShipLocksManagerViewModel? ViewModel => DataContext as ShipLocksManagerViewModel;

    public ShipLocksManagerView()
    {
        InitializeComponent();
    }
}