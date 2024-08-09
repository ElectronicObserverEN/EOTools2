using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EOToolsWeb.Views;

public partial class MessageWindow : Window
{
    public MessageWindow()
    {
        InitializeComponent();
    }

    private void OnCloseClick(object sender, RoutedEventArgs e)
    {
        Close(false);
    }
}