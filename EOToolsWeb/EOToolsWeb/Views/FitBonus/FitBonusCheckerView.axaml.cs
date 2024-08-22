using Avalonia.Controls;
using EOToolsWeb.ViewModels.FitBonus;

namespace EOToolsWeb.Views.FitBonus;

public partial class FitBonusCheckerView : UserControl
{
    public FitBonusCheckerViewModel? ViewModel => DataContext as FitBonusCheckerViewModel;

    public FitBonusCheckerView()
    {
        InitializeComponent();
    }
}