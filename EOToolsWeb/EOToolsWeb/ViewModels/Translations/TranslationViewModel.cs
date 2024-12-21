using CommunityToolkit.Mvvm.ComponentModel;

namespace EOToolsWeb.ViewModels.Translations;

public partial class TranslationViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Translation { get; set; } = "";
}