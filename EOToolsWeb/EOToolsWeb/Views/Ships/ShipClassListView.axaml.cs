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

        ViewModel.PropertyChanged += (sender, args) => Close(ViewModel.PickedClass);
    }
}