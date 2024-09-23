using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Seasons;
using EOToolsWeb.Shared.Updates;
using EOToolsWeb.ViewModels.Updates;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.Seasons;

public partial class SeasonViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private int? _addedOnUpdateId;

    public string AddedOnUpdateDisplay => AddedOnUpdate?.Name ?? "Select an update";

    public UpdateModel? AddedOnUpdate => AddedOnUpdateId switch
    {
        int id => UpdateManager.UpdateList.Find(upd => upd.Id == id),
        _ => null,
    };

    [ObservableProperty]
    private int? _removedOnUpdateId;

    public UpdateModel? RemovedOnUpdate => RemovedOnUpdateId switch
    {
        int id => UpdateManager.UpdateList.Find(upd => upd.Id == id),
        _ => null,
    };
    public string RemovedOnUpdateDisplay => RemovedOnUpdate?.Name ?? "Select an update";

    public SeasonModel? Model { get; set; }

    private UpdateManagerViewModel UpdateManager { get; }

    public SeasonViewModel(UpdateManagerViewModel updateManager)
    {
        UpdateManager = updateManager;

        PropertyChanged += SeasonViewModel_PropertyChanged;
    }

    public async Task LoadFromModel()
    {
        await UpdateManager.LoadAllUpdates();

        if (Model is null) return;

        Name = Model.Name;
        RemovedOnUpdateId = Model.RemovedOnUpdateId;
        AddedOnUpdateId = Model.AddedOnUpdateId;
    }

    private void SeasonViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(AddedOnUpdateId)) OnPropertyChanged(nameof(AddedOnUpdateDisplay));
        if (e.PropertyName is nameof(RemovedOnUpdateId)) OnPropertyChanged(nameof(RemovedOnUpdateDisplay));
    }

    public void SaveChanges()
    {
        if (Model is null) return;

        Model.Name = Name;

        Model.RemovedOnUpdateId = RemovedOnUpdateId;

        Model.AddedOnUpdateId = AddedOnUpdateId;
    }

    [RelayCommand]
    public async Task OpenAddedOnUpdateList()
    {
        if (await UpdateManager.ShowPickerDialog.Handle(null) is { } upd)
        {
            AddedOnUpdateId = upd.Id;
        }
    }

    [RelayCommand]
    public async Task OpenRemovedOnUpdateList()
    {
        if (await UpdateManager.ShowPickerDialog.Handle(null) is { } upd)
        {
            RemovedOnUpdateId = upd.Id;
        }
    }

    [RelayCommand]
    public void ClearAddedOnUpdate()
    {
        AddedOnUpdateId = null;
    }

    [RelayCommand]
    public void ClearRemovedOnUpdate()
    {
        RemovedOnUpdateId = null;
    }
}
