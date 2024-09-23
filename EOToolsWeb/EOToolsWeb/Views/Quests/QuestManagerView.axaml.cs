using Avalonia.Controls;
using EOToolsWeb.ViewModels.Quests;

namespace EOToolsWeb.Views.Quests;

public partial class QuestManagerView : UserControl
{
    public QuestManagerViewModel? ViewModel => DataContext as QuestManagerViewModel;

    public QuestManagerView()
    {
        InitializeComponent();
    }
}