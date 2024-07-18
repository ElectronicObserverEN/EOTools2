using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Updates;

namespace EOToolsWeb.ViewModels.Updates;

public partial class UpdateListViewModel(UpdateManagerViewModel updates) : ObservableObject
{
    public List<UpdateModel> UpdateList { get; set; } = [];

    public UpdateModel? SelectedUpdate { get; set; }

    [ObservableProperty]
    private UpdateModel? _pickedUpdate;

    private UpdateManagerViewModel Updates { get; } = updates;

    public async Task Initialize()
    {
        await Updates.LoadAllUpdates();

        PickedUpdate = null;
        SelectedUpdate = null;

        // Load updates
        UpdateList = Updates.UpdateList
            .OrderByDescending(upd => upd.UpdateDate)
            .ToList();
    }
    
    [RelayCommand]
    private void SelectUpdate()
    {
        PickedUpdate = SelectedUpdate;
    }

    [RelayCommand]
    private void Cancel()
    {
        OnPropertyChanged(nameof(PickedUpdate));
    }
}
