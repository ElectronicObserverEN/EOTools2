using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.ShipLocks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EOToolsWeb.Models.ShipLocks;

namespace EOToolsWeb.ViewModels.ShipLocks;

public partial class ShipLockPhaseViewModel : ViewModelBase
{
    public int Map { get; set; }

    public int SortId { get; set; }

    public ObservableCollection<ShipLockEditRowModel> LockGroups { get; set; } = [];

    public string PhaseName { get; set; } = "";

    public ShipLockPhaseModel Model { get; set; } = new();

    public List<ShipLockEditRowModel> Locks { get; set; } = [];

    public ShipLockEditRowModel? SelectedLock { get; set; }

    public void Initialize(List<ShipLockEditRowModel> locks)
    {
        Locks = locks;
        SelectedLock = null;
    }

    public void LoadFromModel()
    {
        PhaseName = Model.PhaseName;
        LockGroups = new(Model.LockGroups.Select(locks => Locks.FirstOrDefault(locks2 => locks2.ApiId == locks) ?? new()).ToList());

        Map = Model.SortId / 1000;
        SortId = Model.SortId - (Map * 1000);
    }

    public void SaveChanges()
    {
        Model.PhaseName = PhaseName;
        Model.LockGroups = LockGroups.Select(locks => locks.ApiId).ToList();

        Model.SortId = SortId + (Map * 1000);
    }

    [RelayCommand]
    private void AddLock()
    {
        if (SelectedLock is null) return;

        LockGroups.Add(SelectedLock);
    }

    [RelayCommand]
    private void DeleteLock(ShipLockEditRowModel locks)
    {
        LockGroups.Remove(locks);
    }
}
