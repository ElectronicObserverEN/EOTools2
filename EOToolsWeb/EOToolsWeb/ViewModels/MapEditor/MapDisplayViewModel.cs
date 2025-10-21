using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media.Imaging;
using EOToolsWeb.Views.MapEditor;

namespace EOToolsWeb.ViewModels.MapEditor;

public class MapDisplayViewModel
{
    public ObservableCollection<MapElementModel> MapImages { get; } = [];

    //public Func<Task<Bitmap>>? GetImage { get; set; }

    public CroppedBitmap GetMapImage()
    {
        return MapImages.FirstOrDefault()?.Image switch
        {
            CroppedBitmap croppedBitmap => croppedBitmap,
            _ => throw new NotSupportedException(),
        };
    }
}