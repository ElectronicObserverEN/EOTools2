using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.Shared.Seasons;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Updates;
using EOToolsWeb.Shared.UseItem;
using EOToolsWeb.ViewModels;
using EOToolsWeb.ViewModels.Equipments;
using EOToolsWeb.ViewModels.EquipmentUpgrades;
using EOToolsWeb.ViewModels.Events;
using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.ViewModels.Quests;
using EOToolsWeb.ViewModels.Seasons;
using EOToolsWeb.ViewModels.ShipLocks;
using EOToolsWeb.ViewModels.Ships;
using EOToolsWeb.ViewModels.Translations;
using EOToolsWeb.ViewModels.Updates;
using EOToolsWeb.Views.Equipments;
using EOToolsWeb.Views.EquipmentUpgrades;
using EOToolsWeb.Views.Events;
using EOToolsWeb.Views.Login;
using EOToolsWeb.Views.Quests;
using EOToolsWeb.Views.Seasons;
using EOToolsWeb.Views.ShipLocks;
using EOToolsWeb.Views.Ships;
using EOToolsWeb.Views.Translations;
using EOToolsWeb.Views.Updates;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Media.Imaging;

namespace EOToolsWeb.Views;

public partial class MainWindow : ReactiveWindow<MainViewModel>
{
    private LoginViewModel? LoginViewModel => DataContext is MainViewModel vm ? vm.Login : null;
    private MainViewModel? MainViewModel => ViewModel;

    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.Updates.ShowEditDialog.RegisterHandler(DoShowEditDialogAsync)));
        this.WhenActivated(d => d(ViewModel!.Updates.ShowPickerDialog.RegisterHandler(DoShowPickerDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.Events.ShowEditDialog.RegisterHandler(DoShowEditDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.ShipClassManager.ShowEditDialog.RegisterHandler(DoShowEditDialogAsync)));
        this.WhenActivated(d => d(ViewModel!.ShipClassManager.ShowPicker.RegisterHandler(DoShowPickerDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.ShipManager.ShowEditDialog.RegisterHandler(DoShowEditDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.EquipmentManager.ShowEditDialog.RegisterHandler(DoShowEditDialogAsync)));
        this.WhenActivated(d => d(ViewModel!.EquipmentManager.ShowPicker.RegisterHandler(DoShowPickerDialogAsync)));
        this.WhenActivated(d => d(ViewModel!.EquipmentViewModel.ShowUpgradeEditDialog.RegisterHandler(DoShowEditDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.UseItemManager.ShowPicker.RegisterHandler(DoShowPickerDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.ShipLocksManager.ShowShipLockEditDialog.RegisterHandler(DoShowEditDialogAsync)));
        this.WhenActivated(d => d(ViewModel!.ShipLocksManager.ShowShipLockPhaseEditDialog.RegisterHandler(DoShowEditDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.TranslationManager.ShowEditDialog.RegisterHandler(DoShowEditDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.SeasonManager!.ShowEditDialog.RegisterHandler(DoShowEditDialogAsync)));
        this.WhenActivated(d => d(ViewModel!.SeasonManager.ShowPickerDialog.RegisterHandler(DoShowPickerDialogAsync)));

        this.WhenActivated(d => d(ViewModel!.QuestManager!.ShowEditDialog.RegisterHandler(DoShowEditDialogAsync)));
        this.WhenActivated(d => d(ViewModel!.QuestManager.ReadClipboard.RegisterHandler(ReadClipboard)));
    }

    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        ViewModel!.ShowDialogService!.ShowDialog = DoShowDialog;
        ViewModel!.ShowDialogService!.ShowWindow = DoShowWindow;
        ViewModel!.ShowDialogService!.ShowFolderPicker = DoShowFolderPickerAsync;
        ViewModel!.ShowDialogService!.SaveFileImplementation = SaveFile;

        ViewModel!.ClipboardService!.CopyToClipboardImplementation = SetClipboard;

        if (LoginViewModel is null || MainViewModel is null)
        {
            Close();
            return;
        }

        ViewModel.CloseApplication = Close;
        await ViewModel.ShowLogInDialog();

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

    private async Task DoShowPickerDialogAsync(IInteractionContext<object?, SeasonModel?> interaction)
    {
        if (MainViewModel?.SeasonList is null) return;

        await MainViewModel.SeasonList.Initialize();

        SeasonListView dialog = new();
        dialog.DataContext = MainViewModel.SeasonList;

        SeasonModel? result = await dialog.ShowDialog<SeasonModel?>(this);
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

    private async Task DoShowPickerDialogAsync(IInteractionContext<object?, EquipmentModel?> interaction)
    {
        if (MainViewModel?.EquipmentPicker is null) return;

        await MainViewModel.EquipmentManager.LoadAllEquipments();
        MainViewModel.EquipmentPicker.Initialize(MainViewModel.EquipmentManager.AllEquipments);

        EquipmentPickerView dialog = new();
        dialog.DataContext = MainViewModel.EquipmentPicker;

        EquipmentModel? result = await dialog.ShowDialog<EquipmentModel?>(this);
        interaction.SetOutput(result);
    }

    private async Task DoShowPickerDialogAsync(IInteractionContext<object?, UseItemId?> interaction)
    {
        if (MainViewModel?.EquipmentPicker is null) return;

        List<EquipmentModel> items = Enum.GetValues<UseItemId>().Select(enu => new EquipmentModel()
        {
            ApiId = (int)enu,
            NameEN = enu.ToString(),
        }).ToList();

        MainViewModel.EquipmentPicker.Initialize(items);

        EquipmentPickerView dialog = new();
        dialog.DataContext = MainViewModel.EquipmentPicker;

        EquipmentModel? result = await dialog.ShowDialog<EquipmentModel?>(this);
        interaction.SetOutput((UseItemId?)result?.ApiId);
    }

    private async Task DoShowEditDialogAsync(IInteractionContext<ShipViewModel, bool> interaction)
    {
        ShipEditView dialog = new();
        dialog.DataContext = interaction.Input;

        bool result = await dialog.ShowDialog<bool?>(this) is true;
        interaction.SetOutput(result);
    }

    private async Task DoShowEditDialogAsync(IInteractionContext<SeasonViewModel, bool> interaction)
    {
        SeasonEditView dialog = new();
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

    private async Task DoShowEditDialogAsync(IInteractionContext<TranslationViewModelOld, bool> interaction)
    {
        TranslationEditView dialog = new();
        dialog.DataContext = interaction.Input;

        bool result = await dialog.ShowDialog<bool?>(this) is true;
        interaction.SetOutput(result);
    }

    private async Task DoShowDialog(MessageViewModel messageVm)
    {
        MessageWindow message = new();
        message.DataContext = messageVm;

        await message.ShowDialog(this);
    }

    private async Task ReadClipboard(IInteractionContext<object?, string?> interaction)
    {
        TopLevel? topLevel = GetTopLevel(this);

        if (topLevel is null) return;
        if (topLevel.Clipboard is null) return;

        interaction.SetOutput(await topLevel.Clipboard.GetTextAsync());
    }
    
    private async Task SetClipboard(string text)
    {
        TopLevel? topLevel = GetTopLevel(this);

        if (topLevel is null) return;
        if (topLevel.Clipboard is null) return;

        await topLevel.Clipboard.SetTextAsync(text);
    }
    
    /// <summary>
    /// This isn't tested yet
    /// </summary>
    /// <returns></returns>
    /*private async Task<object?> ReadClipboard()
    {
        TopLevel? topLevel = GetTopLevel(this);

        if (topLevel is null) return null; 
        if (topLevel.Clipboard is null) return null;

        using IAsyncDataTransfer? data = await topLevel.Clipboard.TryGetDataAsync();
        
        if (data is null) return null;
        if (data.Items.Count is 0)  return null;

        return await data.Items[0].TryGetRawAsync(data.Formats[0]);
    }*/
    
    private async Task SaveFile(object? content, string format)
    {
        TopLevel? topLevel = GetTopLevel(this);

        if (topLevel is null) return; 
        if (topLevel.Clipboard is null) return;

        using DataTransfer data = new DataTransfer();

        if (content is Bitmap image)
        {
            IStorageFile? file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                DefaultExtension = format,
                ShowOverwritePrompt = true,
            });
            
            if (file is null) return;

            await using Stream stream = await file.OpenWriteAsync();
            image.Save(stream);
        }
        else if (content is Stream stream)
        {
            IStorageFile? file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                DefaultExtension = format,
                ShowOverwritePrompt = true,
            });
            
            if (file is null) return;

            await using Stream streamDest = await file.OpenWriteAsync();
            stream.Position = 0;
            await stream.CopyToAsync(streamDest);
        }
        else
        {
            // TODO
            throw new NotSupportedException();
        }
    }

    private async Task DoShowEditDialogAsync(IInteractionContext<QuestViewModel, bool> interaction)
    {
        QuestEditView dialog = new();
        dialog.DataContext = interaction.Input;

        bool result = await dialog.ShowDialog<bool?>(this) is true;
        interaction.SetOutput(result);
    }

    private async Task<IStorageFolder?> DoShowFolderPickerAsync(string? baseFolder)
    {
        IStorageProvider storage = TopLevel.GetTopLevel(this)!.StorageProvider;

        IStorageFolder? folder = string.IsNullOrEmpty(baseFolder) ? null : await storage.TryGetFolderFromPathAsync(new Uri(baseFolder));

        IReadOnlyList<IStorageFolder> result = await storage.OpenFolderPickerAsync(new()
        {
            AllowMultiple = false,
            SuggestedStartLocation = folder,
            Title = "Pick a folder",
        });

        return result.FirstOrDefault();
    }

    private async Task<bool> DoShowWindow(Window window)
    {
        return await window.ShowDialog<bool?>(this) is true;
    }

    protected override async void OnClosed(EventArgs e)
    {
        await MainViewModel!.Settings.OnViewClosing();

        base.OnClosed(e);
    }
}