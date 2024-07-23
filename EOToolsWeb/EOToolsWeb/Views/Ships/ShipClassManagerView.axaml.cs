using Avalonia.Controls;
using EOToolsWeb.ViewModels.Ships;

namespace EOToolsWeb.Views.Ships;

public partial class ShipClassManagerView : UserControl
{
    public ShipClassManagerViewModel? ViewModel => DataContext as ShipClassManagerViewModel;

    public ShipClassManagerView()
    {
        InitializeComponent();
    }
}