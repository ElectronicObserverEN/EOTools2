using Avalonia.Media;
using EOToolsWeb.Shared.ShipLocks;

namespace EOToolsWeb.Models.ShipLocks;

public class ShipLockEditRowModel : ShipLockModel
{
    public Color Color => Color.FromArgb(ColorA, ColorR, ColorG, ColorB);
}
