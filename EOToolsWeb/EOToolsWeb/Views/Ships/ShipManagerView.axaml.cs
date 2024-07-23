using Avalonia.Controls;
using EOToolsWeb.ViewModels.Ships;

namespace EOToolsWeb.Views.Ships;

public partial class ShipManagerView : UserControl
{
    public ShipManagerViewModel? ViewModel => DataContext as ShipManagerViewModel;

    public ShipManagerView()
    {
        InitializeComponent();
    }
}