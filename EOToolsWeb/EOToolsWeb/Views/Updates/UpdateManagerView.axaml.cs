using Avalonia.Controls;
using EOToolsWeb.ViewModels.Updates;

namespace EOToolsWeb.Views.Updates;

public partial class UpdateManagerView : UserControl
{
    public UpdateManagerViewModel? ViewModel => DataContext as UpdateManagerViewModel;

    public UpdateManagerView()
    {
        InitializeComponent();
    }
}