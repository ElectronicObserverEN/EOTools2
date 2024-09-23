using Avalonia.Controls;
using EOToolsWeb.ViewModels.Seasons;

namespace EOToolsWeb.Views.Seasons;

public partial class SeasonManagerView : UserControl
{
    public SeasonManagerViewModel? ViewModel => DataContext as SeasonManagerViewModel;

    public SeasonManagerView()
    {
        InitializeComponent();
    }
}