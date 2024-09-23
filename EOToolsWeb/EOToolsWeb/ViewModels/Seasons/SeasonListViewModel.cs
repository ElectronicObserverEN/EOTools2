using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Seasons;

namespace EOToolsWeb.ViewModels.Seasons;

public partial class SeasonListViewModel(SeasonManagerViewModel seasons) : ObservableObject
{
    public List<SeasonModel> SeasonList { get; set; } = [];

    public SeasonModel? SelectedSeason { get; set; }

    [ObservableProperty]
    private SeasonModel? _pickedSeason;

    private SeasonManagerViewModel Seasons { get; } = seasons;

    public async Task Initialize()
    {
        await Seasons.LoadSeasonList();

        PickedSeason = null;
        SelectedSeason = null;

        // Load updates
        SeasonList = Seasons.SeasonList
            .OrderByDescending(upd => upd.Id)
            .ToList();
    }
    
    [RelayCommand]
    private void SelectSeason()
    {
        PickedSeason = SelectedSeason;
    }

    [RelayCommand]
    private void Cancel()
    {
        OnPropertyChanged(nameof(PickedSeason));
    }
}
