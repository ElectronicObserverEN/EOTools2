using System;
using Avalonia.Controls;
using EOToolsWeb.ViewModels.Equipments;

namespace EOToolsWeb.Views.Equipments;

public partial class EquipmentPickerView : Window
{
    private EquipmentPickerViewModel? ViewModel => DataContext as EquipmentPickerViewModel;

    public EquipmentPickerView()
    {
        InitializeComponent();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        if (ViewModel is null) return;

        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(ViewModel.PickedEquipment)) return;

        Close(ViewModel?.PickedEquipment);
    }
}