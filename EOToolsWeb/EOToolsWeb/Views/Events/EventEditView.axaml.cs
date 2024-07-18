using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EOToolsWeb.Views.Events;

public partial class EventEditView : Window
{
    public EventEditView()
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