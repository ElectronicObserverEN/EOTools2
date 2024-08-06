using System;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using EOToolsWeb.ViewModels.Settings;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EOToolsWeb.Views.Settings;

public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
{
    public SettingsView()
    {
        InitializeComponent();

        this.WhenActivated(d => d(ViewModel!.ShowFolderPicker.RegisterHandler(DoShowFolderPickerAsync)));
    }

    private async Task DoShowFolderPickerAsync(IInteractionContext<string?, IStorageFolder?> interaction)
    {
        IStorageProvider storage = TopLevel.GetTopLevel(this)!.StorageProvider;

        IStorageFolder? folder = string.IsNullOrEmpty(interaction.Input) ? null : await storage.TryGetFolderFromPathAsync(new Uri(interaction.Input));

        IReadOnlyList<IStorageFolder> result = await storage.OpenFolderPickerAsync(new()
        {
            AllowMultiple = false,
            SuggestedStartLocation = folder,
            Title = "Pick a folder",
        });

        interaction.SetOutput(result.FirstOrDefault());
    }
}