using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Ships;
using System.ComponentModel;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.Ships;

public partial class ShipViewModel : ObservableObject
{
    public ShipModel Model { get; set; } = new();

    [ObservableProperty] private string _nameEN = "";

    [ObservableProperty] private string _nameJP = "";

    [ObservableProperty] private int _apiId;

    [ObservableProperty] private ShipClassModel? _shipClass;

    public string ClassName => ShipClass switch
    {
        { } shipClass => shipClass.NameEnglish,
        _ => "Select a class"
    };

    private ShipClassManagerViewModel ShipClassManagerViewModel { get; }
    
    public ShipViewModel(ShipClassManagerViewModel classManager)
    {
        ShipClassManagerViewModel = classManager;

        PropertyChanged += OnClassIdChanged;
    }

    private void OnClassIdChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(ShipClass)) return;

        OnPropertyChanged(nameof(ClassName));
    }

    public async Task LoadModel()
    {
        NameEN = Model.NameEN;
        NameJP = Model.NameJP;
        ApiId = Model.ApiId;

        await ShipClassManagerViewModel.LoadAllClasses();

        ShipClass = Model.ShipClassId switch
        {
            { } id => ShipClassManagerViewModel.ShipClass.Find(shipClass => shipClass.Id == id),
            _ => null,
        };
    }

    public void SaveChanges()
    {
        Model.NameEN = NameEN;
        Model.NameJP = NameJP;
        Model.ApiId = ApiId;
        Model.ShipClass = ShipClass;
    }

    public bool IsFriendly => Model.IsFriendly;

    [RelayCommand]
    private async Task OpenClassPicker()
    {
        await ShipClassManagerViewModel.LoadAllClasses();
        ShipClassModel? classModel = await ShipClassManagerViewModel.OpenClassPicker();

        if (classModel is null) return;

        ShipClass = classModel;
    }
}
