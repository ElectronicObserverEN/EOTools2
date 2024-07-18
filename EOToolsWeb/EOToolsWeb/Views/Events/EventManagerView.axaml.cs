using Avalonia.Controls;
using EOToolsWeb.ViewModels.Events;

namespace EOToolsWeb.Views.Events;

public partial class EventManagerView : UserControl
{
    public EventManagerViewModel? ViewModel => DataContext as EventManagerViewModel;

    public EventManagerView()
    {
        InitializeComponent();
    }
}