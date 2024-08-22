using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Control.Grid;
using EOToolsWeb.Models.FitBonus;
using EOToolsWeb.Shared.FitBonus;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.ViewModels.Equipments;
using EOToolsWeb.ViewModels.Ships;

namespace EOToolsWeb.ViewModels.FitBonus;

public partial class FitBonusIssueViewModel : ObservableObject, IGridRowFetched
{
    private FitBonusIssueModel Model { get; }

    private EquipmentManagerViewModel EquipmentManagerViewModel { get; }
    private ShipManagerViewModel ShipManagerViewModel { get; }

    [ObservableProperty]
    private ShipModel _ship = new();

    [ObservableProperty]
    private List<EquipmentWithStatsViewModel> _equipments = new();

    [ObservableProperty]
    private FitBonusValueModel _expectedValue = new();

    [ObservableProperty]
    private FitBonusValueModel _actualValue = new();

    public int Id => Model.Id;

    public FitBonusIssueViewModel(FitBonusIssueModel model, ShipManagerViewModel ships, EquipmentManagerViewModel equipments)
    {
        Model = model;
        EquipmentManagerViewModel = equipments;
        ShipManagerViewModel = ships;

        LoadFromModel();
    }

    private void LoadFromModel()
    {
        Ship = ShipManagerViewModel.Ships
            .FirstOrDefault(ship => ship.ApiId == Model.Ship.ShipId) ?? new ShipModel();

        Equipments = Model.Equipments
            .Select(eq => new EquipmentWithStatsViewModel(eq, EquipmentManagerViewModel))
            .ToList();

        ExpectedValue = Model.ExpectedBonus;
        ActualValue = Model.ActualBonus;
    }
}