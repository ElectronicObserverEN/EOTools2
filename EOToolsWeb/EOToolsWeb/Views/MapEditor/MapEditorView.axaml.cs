using Avalonia.Controls;
using EOToolsWeb.ViewModels.MapEditor;

namespace EOToolsWeb.Views.MapEditor;

public partial class MapEditorView : UserControl
{
    public MapEditorViewModel? ViewModel => DataContext as MapEditorViewModel;

    public MapEditorView()
    {
        InitializeComponent();
    }
}