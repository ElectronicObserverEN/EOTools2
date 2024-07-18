using System;
using Avalonia.Controls;
using EOToolsWeb.ViewModels.Updates;

namespace EOToolsWeb.Views.Updates;

public partial class UpdateListView : Window
{
    private UpdateListViewModel? ViewModel => DataContext as UpdateListViewModel;

    public UpdateListView()
    {
        InitializeComponent();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        if (ViewModel is null) return;

        ViewModel.PropertyChanged += (sender, args) => Close(ViewModel.PickedUpdate);
    }
}