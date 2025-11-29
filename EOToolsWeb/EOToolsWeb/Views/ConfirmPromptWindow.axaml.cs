using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EOToolsWeb.Views;

public partial class ConfirmPromptWindow : Window
{
    public ConfirmPromptWindow()
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