using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Views.MapEditor;
using SkiaSharp;

namespace EOToolsWeb.ViewModels.MapEditor;

public partial class PathDisplayViewModel : ObservableObject
{
    public ObservableCollection<MapElementModel> PathParts { get; } = [];

    public List<CroppedBitmap> GetImages()
    {
        return PathParts
            .Select(part => part.Image)
            .OfType<CroppedBitmap>()
            .ToList();
    }
}