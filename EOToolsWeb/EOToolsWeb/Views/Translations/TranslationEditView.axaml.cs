using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EOToolsWeb.Views.Translations;

public partial class TranslationEditView : Window
{
    public TranslationEditView()
    {
        InitializeComponent();
    }
    private void OnConfirmClick(object sender, RoutedEventArgs e)
    {
        Close(true);
    }

    private void OnCancelClick(object sender, RoutedEventArgs e)
    {
        Close(false);
    }
}