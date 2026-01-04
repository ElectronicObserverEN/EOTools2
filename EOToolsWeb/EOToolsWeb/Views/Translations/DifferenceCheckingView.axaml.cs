using Avalonia.Controls;
using EOToolsWeb.ViewModels.Translations.DifferenceChecking;

namespace EOToolsWeb.Views.Translations;

public partial class DifferenceCheckingView : Window
{
    public DifferenceCheckingViewModel ViewModel => (DataContext as DifferenceCheckingViewModel)!;

    public DifferenceCheckingView()
    {
        InitializeComponent();
    }
}