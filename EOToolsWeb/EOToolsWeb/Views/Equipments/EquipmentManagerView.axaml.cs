using Avalonia.Controls;
using EOToolsWeb.ViewModels.Equipments;

namespace EOToolsWeb.Views.Equipments;

public partial class EquipmentManagerView : UserControl
{
    public EquipmentManagerViewModel? ViewModel => DataContext as EquipmentManagerViewModel;

    public EquipmentManagerView()
    {
        InitializeComponent();
    }
}