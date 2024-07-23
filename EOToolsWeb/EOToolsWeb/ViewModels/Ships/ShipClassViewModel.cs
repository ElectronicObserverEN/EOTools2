using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Shared.Ships;

namespace EOToolsWeb.ViewModels.Ships;

public partial class ShipClassViewModel : ObservableObject
{
    public ShipClassModel Model { get; set; } = new();

    [ObservableProperty]
    private string _nameEnglish = "";

    [ObservableProperty]
    private string _nameJapanese = "";

    [ObservableProperty]
    private int _apiId;
    
    public void LoadModel()
    {
        NameEnglish = Model.NameEnglish;
        NameJapanese = Model.NameJapanese;
        ApiId = Model.ApiId;
    }

    public void SaveChanges()
    {
        Model.NameEnglish = NameEnglish;
        Model.NameJapanese = NameJapanese;
        Model.ApiId = ApiId;
    }
}
