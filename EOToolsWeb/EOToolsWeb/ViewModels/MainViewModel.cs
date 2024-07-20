using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.ViewModels.Events;
using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.ViewModels.Updates;

namespace EOToolsWeb.ViewModels;

public partial class MainViewModel(ILoginViewModel login, UpdateManagerViewModel updates, EventManagerViewModel events, UpdateListViewModel updateList, EventViewModel eventViewModel) : ViewModelBase
{
    public ILoginViewModel Login { get; } = login;

    public UpdateManagerViewModel Updates { get; } = updates;
    public UpdateListViewModel UpdateList { get; } = updateList;

    public EventManagerViewModel Events { get; } = events;
    public EventViewModel EventViewModel { get; } = eventViewModel;

    [ObservableProperty]
    private IViewModelBase? _currentViewModel;

    [RelayCommand]
    private async Task OpenUpdates()
    {
        CurrentViewModel = Updates;
        await Updates.LoadAllUpdates();
    }

    [RelayCommand]
    private async Task OpenEvents()
    {
        CurrentViewModel = Events;
        await Events.LoadAllEvents();
    }
}
