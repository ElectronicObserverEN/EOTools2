using Avalonia.Media;
using EOToolsWeb.Shared.ShipLocks;

namespace EOToolsWeb.Models;

public class ShipLockEditRowModel : ShipLockModel
{
    public Color Color => Color.FromArgb(ColorA, ColorR, ColorG, ColorB);
}
