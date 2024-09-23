using System;
using Avalonia.Controls;
using EOToolsWeb.ViewModels.Seasons;

namespace EOToolsWeb.Views.Seasons;

public partial class SeasonListView : Window
{
    private SeasonListViewModel? ViewModel => DataContext as SeasonListViewModel;

    public SeasonListView()
    {
        InitializeComponent();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        if (ViewModel is null) return;

        ViewModel.PropertyChanged += (sender, args) => Close(ViewModel.PickedSeason);
    }
}