using Avalonia.Controls;
using EOToolsWeb.ViewModels.Translations;

namespace EOToolsWeb.Views.Translations;

public partial class TranslationManagerView : UserControl
{
    public TranslationManagerViewModel ViewModel => (DataContext as TranslationManagerViewModel)!;

    public TranslationManagerView()
    {
        InitializeComponent();
    }
}