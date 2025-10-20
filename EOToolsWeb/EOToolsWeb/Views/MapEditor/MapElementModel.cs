using Avalonia;
using Avalonia.Media;

namespace EOToolsWeb.Views.MapEditor;

public class MapElementModel(IImage image, double x, double y)
{
    public IImage Image { get; } = image;

    public double X { get; } = x;

    public double Y { get; } = y;
}