using Avalonia.Controls;
using Avalonia.Interactivity;
using EOToolsWeb.ViewModels.Quests;

namespace EOToolsWeb.Views.Quests;

public partial class QuestEditView : Window
{
    public QuestViewModel? ViewModel => DataContext as QuestViewModel;

    public QuestEditView()
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