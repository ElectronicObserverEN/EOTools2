using Avalonia.Controls;
using EOToolsWeb.ViewModels;
using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.Views.Login;
using System;

namespace EOToolsWeb.Views;

public partial class MainWindow : Window
{
    private LoginViewModel? LoginViewModel => DataContext is MainViewModel vm ? vm.Login : null;
    private MainViewModel? MainViewModel => DataContext is MainViewModel vm ? vm : null;

    public MainWindow()
    {
        InitializeComponent();
    }

    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        if (LoginViewModel is null || MainViewModel is null)
        {
            Close();
            return;
        }

        LoginView login = new(LoginViewModel);

        if (await login.ShowDialog<bool?>(this) is not true)
        {
            Close();
            return;
        }

        MainViewModel.PropertyChanged += MainViewModel_PropertyChanged;
    }

    private void MainViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(MainViewModel.CurrentViewModel)) return;

        MainContent.Content = new ViewLocator().Build(MainViewModel?.CurrentViewModel);
    }
}