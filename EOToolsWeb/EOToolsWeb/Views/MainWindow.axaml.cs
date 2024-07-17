using System;
using EOToolsWeb.ViewModels;
using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.ViewModels.Updates;
using EOToolsWeb.Views.Login;
using EOToolsWeb.Views.Updates;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Threading.Tasks;

namespace EOToolsWeb.Views;

public partial class MainWindow : ReactiveWindow<MainViewModel>
{
    private LoginViewModel? LoginViewModel => DataContext is MainViewModel vm ? vm.Login : null;
    private MainViewModel? MainViewModel => ViewModel;

    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.Updates.ShowEditDialog.RegisterHandler(DoShowUpdateEditDialogAsync)));
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

    private async Task DoShowUpdateEditDialogAsync(IInteractionContext<UpdateViewModel, bool> interaction)
    {
        UpdateEditView dialog = new();
        dialog.DataContext = interaction.Input;

        bool result = await dialog.ShowDialog<bool?>(this) is true;
        interaction.SetOutput(result);
    }
}