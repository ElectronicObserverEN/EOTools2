using System;
using EOToolsWeb.ViewModels;
using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.ViewModels.Updates;
using EOToolsWeb.Views.Login;
using EOToolsWeb.Views.Updates;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Threading.Tasks;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Updates;
using EOToolsWeb.ViewModels.Equipments;
using EOToolsWeb.ViewModels.EquipmentUpgrades;
using EOToolsWeb.ViewModels.Events;
using EOToolsWeb.ViewModels.ShipLocks;
using EOToolsWeb.ViewModels.Ships;
using EOToolsWeb.Views.Equipments;
using EOToolsWeb.Views.EquipmentUpgrades;
using EOToolsWeb.Views.Events;
using EOToolsWeb.Views.ShipLocks;
using EOToolsWeb.Views.Ships;

namespace EOToolsWeb.Views;

public partial class MainWindow : ReactiveWindow<MainViewModel>
{
    private LoginViewModel? LoginViewModel => DataContext is MainViewModel vm ? vm.Login : null;
    private MainViewModel? MainViewModel => ViewModel;

    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.Updates.ShowEditDialog.RegisterHandler(DoShowEditDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.EventViewModel.ShowUpdatePickerDialog.RegisterHandler(DoShowPickerDialogAsync)));
        this.WhenActivated(d => d(ViewModel!.Events.ShowEditDialog.RegisterHandler(DoShowEditDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.ShipClassManager.ShowEditDialog.RegisterHandler(DoShowEditDialogAsync)));
        this.WhenActivated(d => d(ViewModel!.ShipClassManager.ShowPicker.RegisterHandler(DoShowPickerDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.ShipManager.ShowEditDialog.RegisterHandler(DoShowEditDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.EquipmentManager.ShowEditDialog.RegisterHandler(DoShowEditDialogAsync)));
        this.WhenActivated(d => d(ViewModel!.EquipmentViewModel.ShowUpgradeEditDialog.RegisterHandler(DoShowEditDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.ShipLocksManager.ShowShipLockEditDialog.RegisterHandler(DoShowEditDialogAsync)));
        this.WhenActivated(d => d(ViewModel!.ShipLocksManager.ShowShipLockPhaseEditDialog.RegisterHandler(DoShowEditDialogAsync)));
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

    private async Task DoShowEditDialogAsync(IInteractionContext<UpdateViewModel, bool> interaction)
    {
        UpdateEditView dialog = new();
        dialog.DataContext = interaction.Input;

        bool result = await dialog.ShowDialog<bool?>(this) is true;
        interaction.SetOutput(result);
    }

    private async Task DoShowEditDialogAsync(IInteractionContext<EventViewModel, bool> interaction)
    {
        EventEditView dialog = new();
        dialog.DataContext = interaction.Input;

        bool result = await dialog.ShowDialog<bool?>(this) is true;
        interaction.SetOutput(result);
    }

    private async Task DoShowPickerDialogAsync(IInteractionContext<object?, UpdateModel?> interaction)
    {
        if (MainViewModel?.UpdateList is null) return;

        await MainViewModel.UpdateList.Initialize();

        UpdateListView dialog = new();
        dialog.DataContext = MainViewModel.UpdateList;

        UpdateModel? result = await dialog.ShowDialog<UpdateModel?>(this);
        interaction.SetOutput(result);
    }

    private async Task DoShowEditDialogAsync(IInteractionContext<ShipClassViewModel, bool> interaction)
    {
        ShipClassEditView dialog = new();
        dialog.DataContext = interaction.Input;

        bool result = await dialog.ShowDialog<bool?>(this) is true;
        interaction.SetOutput(result);
    }

    private async Task DoShowPickerDialogAsync(IInteractionContext<object?, ShipClassModel?> interaction)
    {
        if (MainViewModel?.ShipClassList is null) return;

        await MainViewModel.ShipClassList.Initialize();

        ShipClassListView dialog = new();
        dialog.DataContext = MainViewModel.ShipClassList;

        ShipClassModel? result = await dialog.ShowDialog<ShipClassModel?>(this);
        interaction.SetOutput(result);
    }

    private async Task DoShowEditDialogAsync(IInteractionContext<ShipViewModel, bool> interaction)
    {
        ShipEditView dialog = new();
        dialog.DataContext = interaction.Input;

        bool result = await dialog.ShowDialog<bool?>(this) is true;
        interaction.SetOutput(result);
    }

    private async Task DoShowEditDialogAsync(IInteractionContext<EquipmentViewModel, bool> interaction)
    {
        EquipmentEditView dialog = new();
        dialog.DataContext = interaction.Input;

        bool result = await dialog.ShowDialog<bool?>(this) is true;
        interaction.SetOutput(result);
    }

    private async Task DoShowEditDialogAsync(IInteractionContext<EquipmentUpgradeImprovmentModel, bool> interaction)
    {
        EquipmentUpgradeEditView dialog = new();

        EquipmentUpgradeImprovmentViewModel? vm = MainViewModel?.EquipmentUpgradeImprovmentViewModel;

        if (vm is null) return;

        vm.Model = interaction.Input;
        await vm.LoadFromModel();

        dialog.DataContext = vm;

        bool result = await dialog.ShowDialog<bool?>(this) is true;

        if (result)
        {
            vm.SaveChanges();
        }

        interaction.SetOutput(result);
    }


    private async Task DoShowEditDialogAsync(IInteractionContext<ShipLockViewModel, bool> interaction)
    {
        ShipLockEditView dialog = new();
        dialog.DataContext = interaction.Input;

        bool result = await dialog.ShowDialog<bool?>(this) is true;
        interaction.SetOutput(result);
    }

    private async Task DoShowEditDialogAsync(IInteractionContext<ShipLockPhaseViewModel, bool> interaction)
    {
        ShipLockPhaseEditView dialog = new();
        dialog.DataContext = interaction.Input;

        bool result = await dialog.ShowDialog<bool?>(this) is true;
        interaction.SetOutput(result);
    }
}