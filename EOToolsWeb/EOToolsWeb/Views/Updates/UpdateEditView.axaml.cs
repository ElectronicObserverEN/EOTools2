using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EOToolsWeb.Views.Updates;

public partial class UpdateEditView : Window
{
    public UpdateEditView()
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