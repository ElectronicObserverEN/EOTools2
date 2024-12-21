using Avalonia.Controls;
using EOToolsWeb.ViewModels.ApplicationLog;

namespace EOToolsWeb.Views.ApplicationLog;

public partial class ApplicationLogsManagerView : UserControl
{
    public ApplicationLogsManagerViewModel? ViewModel => DataContext as ApplicationLogsManagerViewModel;

    public ApplicationLogsManagerView()
    {
        InitializeComponent();
    }
}