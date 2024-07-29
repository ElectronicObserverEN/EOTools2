using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Equipments;

namespace EOToolsWeb.ViewModels.Equipments;

public partial class EquipmentPickerViewModel : ObservableObject
{
	[ObservableProperty]
	private string _nameFilter = "";

    private List<EquipmentModel> AllEquipments { get; set; } = [];

    [ObservableProperty]
    private List<EquipmentModel> _equipmentsFiltered = new();

	public EquipmentModel? SelectedEquipment { get; set; }
	public EquipmentModel? PickedEquipment { get; set; }

    public EquipmentPickerViewModel()
	{
        PropertyChanged += EquipmentPickerViewModel_PropertyChanged;
    }

    public void Initialize(List<EquipmentModel> allEquips)
    {
        AllEquipments = allEquips;

        NameFilter = "";
        SelectedEquipment = null;
        PickedEquipment = null;

        RefreshList();
    }

    private void EquipmentPickerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(NameFilter)) return;
        RefreshList();
    }

    private void RefreshList()
        => EquipmentsFiltered = new(AllEquipments.Where(eq => string.IsNullOrEmpty(NameFilter) || eq.NameEN.ToUpper().Contains(NameFilter.ToUpper())).OrderBy(s => s.ApiId));


    [RelayCommand]
    public void SelectEquipment()
    {
        PickedEquipment = SelectedEquipment;
        OnPropertyChanged(nameof(PickedEquipment));
    }

    [RelayCommand]
    public void Cancel()
    {
        PickedEquipment = null;
        OnPropertyChanged(nameof(PickedEquipment));
    }
}
