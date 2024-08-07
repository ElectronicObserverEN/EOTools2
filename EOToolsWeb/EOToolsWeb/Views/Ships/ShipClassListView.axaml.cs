using System;
using Avalonia.Controls;
using EOToolsWeb.ViewModels.Ships;

namespace EOToolsWeb.Views.Ships;

public partial class ShipClassListView : Window
{
    private ShipClassListViewModel? ViewModel => DataContext as ShipClassListViewModel;

    public ShipClassListView()
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
        if (e.PropertyName is not nameof(ViewModel.PickedClass)) return;

        Close(ViewModel?.PickedClass);
    }
}