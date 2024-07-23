using Avalonia.Controls;
using Avalonia.Interactivity;
using EOToolsWeb.ViewModels.Equipments;

namespace EOToolsWeb.Views.Equipments;

public partial class EquipmentEditView : Window
{
    public EquipmentViewModel? ViewModel => DataContext as EquipmentViewModel;

    public EquipmentEditView()
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