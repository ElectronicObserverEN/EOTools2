using Avalonia.Media;
using EOToolsWeb.Shared.ShipLocks;

namespace EOToolsWeb.ViewModels.ShipLocks;

public class ShipLockViewModel
{
    public Color Color { get; set; }

    public string NameJapanese { get; set; } = "";

    public string NameEnglish { get; set; } = "";

    public int ApiId { get; set; }

    public ShipLockModel Model { get; set; } = new();

    public void LoadFromModel()
    {
        ApiId = Model.ApiId;
        NameJapanese = Model.NameJapanese;
        NameEnglish = Model.NameEnglish;
        Color = Color.FromArgb(Model.ColorA, Model.ColorR, Model.ColorG, Model.ColorB);
    }

    public void SaveChanges()
    {
        Model.ApiId = ApiId;
        Model.NameJapanese = NameJapanese;
        Model.NameEnglish = NameEnglish;
        Model.ColorA = Color.A;
        Model.ColorR = Color.R;
        Model.ColorG = Color.G;
        Model.ColorB = Color.B;
    }
}
