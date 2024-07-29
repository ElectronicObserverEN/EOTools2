using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Ships;

namespace EOToolsWeb.ViewModels.Ships;

public partial class ShipClassListViewModel : ObservableObject
{
    private List<ShipClassModel> ClassList { get; set; } = [];

    [ObservableProperty]
    private List<ShipClassModel> _shipListFiltered = new();

    [ObservableProperty]
    private string _filter = "";

    public ShipClassModel? SelectedClass { get; set; }
    public ShipClassModel? PickedClass { get; set; }

    private ShipClassManagerViewModel ShipClassManagerViewModel { get; }

    public ShipClassListViewModel(ShipClassManagerViewModel manager)
    {
        ShipClassManagerViewModel = manager;

        PropertyChanged += ShipListViewModel_PropertyChanged;
    }

    public async Task Initialize()
    {
        await ShipClassManagerViewModel.LoadAllClasses();

        ClassList = ShipClassManagerViewModel.ShipClass;

        SelectedClass = null;
        PickedClass = null;

        RefreshList();
    }

    private void ShipListViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(Filter)) return;

        RefreshList();
    }

    private void RefreshList()
    {
        ShipListFiltered = ClassList
            .Where(ship => string.IsNullOrEmpty(Filter) || ship.NameEnglish.ToUpperInvariant().Contains(Filter.ToUpperInvariant()))
            .OrderBy(ship => ship.ApiId)
            .ToList();
    }

    [RelayCommand]
    public void SelectShip()
    {
        PickedClass = SelectedClass;
        OnPropertyChanged(nameof(PickedClass));
    }

    [RelayCommand]
    public void Cancel()
    {
        PickedClass = null;
        OnPropertyChanged(nameof(PickedClass));
    }
}
