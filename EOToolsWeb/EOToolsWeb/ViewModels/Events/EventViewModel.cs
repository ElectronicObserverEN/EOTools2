using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Events;
using EOToolsWeb.Shared.Updates;
using EOToolsWeb.ViewModels.Updates;
using ReactiveUI;

namespace EOToolsWeb.ViewModels.Events;

public partial class EventViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private int _apiId;

    [ObservableProperty]
    private int? _startOnUpdateId;

    public string StartOnUpdateDisplay => StartOnUpdate?.Name ?? "Select an update";

    public UpdateModel? StartOnUpdate => StartOnUpdateId switch
    {
        { } id => Updates.UpdateList.Find(upd => upd.Id == id),
        _ => null,
    };

    [ObservableProperty]
    private int? _endOnUpdateId;
    public UpdateModel? EndOnUpdate => EndOnUpdateId switch
    {
        { } id => Updates.UpdateList.Find(upd => upd.Id == id),
        _ => null,
    };
    public string EndOnUpdateDisplay => EndOnUpdate?.Name ?? "Select an update";

    public EventModel Model { get; set; } = new();

    private UpdateManagerViewModel Updates { get; }

    public EventViewModel(UpdateManagerViewModel updates)
    {
        Updates = updates;

        PropertyChanged += EventViewModel_PropertyChanged;
    }

    private void EventViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(EndOnUpdateId)) OnPropertyChanged(nameof(EndOnUpdateDisplay));
        if (e.PropertyName is nameof(StartOnUpdateId)) OnPropertyChanged(nameof(StartOnUpdateDisplay));
    }

    public async Task LoadFromModel()
    {
        await Updates.LoadAllUpdates();

        Name = Model.Name;
        EndOnUpdateId = Model.EndOnUpdateId;
        StartOnUpdateId = Model.StartOnUpdateId;
        ApiId = Model.ApiId;
    }

    public void SaveChanges()
    {
        Model.Name = Name;
        Model.StartOnUpdateId = StartOnUpdateId;
        Model.EndOnUpdateId = EndOnUpdateId;
        Model.ApiId = ApiId;
    }

    [RelayCommand]
    public async Task OpenAddedOnUpdateList()
    {
        if (await Updates.ShowPickerDialog.Handle(null) is { } upd)
        {
            StartOnUpdateId = upd.Id;
        }
    }

    [RelayCommand]
    public async Task OpenRemovedOnUpdateList()
    {
        if (await Updates.ShowPickerDialog.Handle(null) is { } upd)
        {
            EndOnUpdateId = upd.Id;
        }
    }

    [RelayCommand]
    public void ClearAddedOnUpdate()
    {
        StartOnUpdateId = null;
    }

    [RelayCommand]
    public void ClearRemovedOnUpdate()
    {
        EndOnUpdateId = null;
    }
}
