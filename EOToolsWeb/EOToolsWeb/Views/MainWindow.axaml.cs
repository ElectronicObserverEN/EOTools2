using Avalonia.Controls;
using EOToolsWeb.ViewModels;
using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.Views.Login;
using System;

namespace EOToolsWeb.Views;

public partial class MainWindow : Window
{
    private LoginViewModel? LoginViewModel => DataContext is MainViewModel vm ? vm.Login : null;

    public MainWindow()
    {
        InitializeComponent();
    }

    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        if (LoginViewModel is null)
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
    }
}