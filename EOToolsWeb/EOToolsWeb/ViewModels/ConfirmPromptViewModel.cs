using CommunityToolkit.Mvvm.Input;

namespace EOToolsWeb.ViewModels;

public class ConfirmPromptViewModel : MessageViewModel
{
    public RelayCommand? ConfirmedCommand { get; set; }
    public RelayCommand? CancelCommand { get; set; }
}