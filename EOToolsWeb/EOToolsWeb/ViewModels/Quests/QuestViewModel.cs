using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Quests;
using EOToolsWeb.Shared.Seasons;
using EOToolsWeb.Shared.Updates;
using EOToolsWeb.ViewModels.Seasons;
using EOToolsWeb.ViewModels.Updates;

namespace EOToolsWeb.ViewModels.Quests;

public partial class QuestViewModel : ObservableObject
{
    [ObservableProperty]
    private string _code = "";

    [ObservableProperty]
    private string _nameJP = "";

    [ObservableProperty]
    private string _nameEN = "";

    [ObservableProperty]
    private string _descJP = "";

    [ObservableProperty]
    private string _descEN = "";

    [ObservableProperty]
    private string _tracker = "";

    [ObservableProperty]
    private int _apiId;

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


    [ObservableProperty]
    private int? _seasonId;

    public string SeasonDisplay => Season?.Name ?? "Select a season";
    public SeasonModel? Season => SeasonId switch
    {
        int id => SeasonManager.SeasonList.Find(season => season.Id == id),
        _ => null,
    };

    public QuestModel? Model { get; set; }

    public QuestModel? SelectedTemplate { get; set; } = null;
    public static QuestModel[] QuestTemplates => new[]
    {
        new QuestModel
        {
            NameEN = "Battle model",
            DescEN = "Organize a fleet with XX in your fleet and score an S rank at the boss nodes on XX, and XX (Part X)"
        },
        new QuestModel
        {
            NameEN = "Battle model with 1-6",
            DescEN = "Organize a fleet with XX in your fleet and score an S rank at the boss nodes on XX, and reach the anchor node N on 1-6."
        },
        new QuestModel
        {
            NameEN = "PVP Model",
            DescEN = "Organize a fleet with XX in your fleet and score S rank XX times in PvP"
        },
        new QuestModel
        {
            NameEN = "Arsenal Model 1",
            DescEN = "Have your secretary flagship XX equip XX in the first slot and then proceed to scrap XX and have XX (All resources and Materials will be consumed upon completion)"
        },
        new QuestModel
        {
            NameEN = "Arsenal Model 2",
            DescEN = "Have XX (All resources and Materials will be consumed upon completion)"
        },
        new QuestModel
        {
            NameEN = "Expedition model",
            DescEN = "Complete expeditions XX and XX XXX times"
        },
    };

    private UpdateManagerViewModel UpdateManager { get; }
    private SeasonManagerViewModel SeasonManager { get; }

    public QuestViewModel(UpdateManagerViewModel updates, SeasonManagerViewModel seasons)
    {
        UpdateManager = updates;
        SeasonManager = seasons;

        PropertyChanged += QuestViewModel_PropertyChanged;
    }

    public async Task LoadFromModel()
    {
        if (Model is null) return;

        await UpdateManager.LoadAllUpdates();
        await SeasonManager.LoadSeasonList();

        ApiId = Model.ApiId;

        Code = Model.Code;

        NameJP = Model.NameJP;
        NameEN = Model.NameEN;
        DescJP = Model.DescJP;
        DescEN = Model.DescEN;

        Tracker = Model.Tracker;

        RemovedOnUpdateId = Model.RemovedOnUpdateId;
        AddedOnUpdateId = Model.AddedOnUpdateId;

        SeasonId = Model.SeasonId;
    }

    private void QuestViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(AddedOnUpdateId)) OnPropertyChanged(nameof(AddedOnUpdateDisplay));
        if (e.PropertyName is nameof(RemovedOnUpdateId)) OnPropertyChanged(nameof(RemovedOnUpdateDisplay));
        if (e.PropertyName is nameof(SeasonId)) OnPropertyChanged(nameof(SeasonDisplay));
    }

    public void SaveChanges()
    {
        if (Model is null) return;

        Model.ApiId = ApiId;
        Model.Code = Code;

        Model.NameJP = NameJP;
        Model.NameEN = NameEN;
        Model.DescJP = DescJP;
        Model.DescEN = DescEN;

        Model.Tracker = Tracker;

        Model.RemovedOnUpdateId = RemovedOnUpdateId;
        Model.AddedOnUpdateId = AddedOnUpdateId;
        Model.SeasonId = SeasonId;
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

    [RelayCommand]
    public async Task OpenSeasonList()
    {
        if (await SeasonManager.ShowPickerDialog.Handle(null) is { } season)
        {
            SeasonId = season.Id;

            AddedOnUpdateId = season.AddedOnUpdateId;
            RemovedOnUpdateId = season.RemovedOnUpdateId;
        }
    }


    [RelayCommand]
    public void ClearSeason()
    {
        SeasonId = null;
    }

    [RelayCommand]
    public void ApplyTemplate()
    {
        if (SelectedTemplate is null) return;
        DescEN = SelectedTemplate.DescEN;
    }
}
