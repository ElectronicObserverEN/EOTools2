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

public partial class MapDisplayViewModel : ObservableObject
{
    public ObservableCollection<MapElementModel> MapImages { get; } = [];
    
    public ObservableCollection<PathDisplayViewModel> Paths { get; } = [];

    [ObservableProperty]
    public partial bool DisplayMap { get; set; } = true;

    [ObservableProperty]
    public partial int ExportWidth { get; set; } = 1;
    
    [ObservableProperty]
    public partial int ExportHeight { get; set; } = 1;
    
    [ObservableProperty]
    public partial int OffsetWidth { get; set; } = 0;
    
    [ObservableProperty]
    public partial int OffsetHeight { get; set; } = 0;
    
    public MemoryStream GetImageMerged()
    {
        // image manipulation is hard for me :c
        // some of this stuff is probably not very optimized, but it seems to be working ...
        // We first build the image by combining all the layers, then crop it if needed, then scale it if needed
            
        // https://github.com/AvaloniaUI/Avalonia/discussions/13772
        List<MapElementModel> images = GetElementsToDisplay();

        if (images.Count == 0) return new MemoryStream();
        
        CroppedBitmap firstCroppedBitmap = (CroppedBitmap)MapImages[0].Image;
        using var skBitmapCrop = new SKBitmap(firstCroppedBitmap.SourceRect.Width, firstCroppedBitmap.SourceRect.Height);
        using var skCanvas = new SKCanvas(skBitmapCrop);
        
        foreach (var element in images)
        {
            var croppedBitmap = element.Image as CroppedBitmap;
            var image = croppedBitmap.Source as Bitmap;
            Stream xxStream = new MemoryStream();
            image.Save(xxStream);
            xxStream.Seek(0, SeekOrigin.Begin);
            var skBitmapSource = SKBitmap.Decode(xxStream);

            var source = new SKRect(croppedBitmap.SourceRect.X, croppedBitmap.SourceRect.Y, croppedBitmap.SourceRect.Width+ croppedBitmap.SourceRect.X, croppedBitmap.SourceRect.Height+ croppedBitmap.SourceRect.Y);
            var dest = new SKRect((float)element.X, (float)element.Y, croppedBitmap.SourceRect.Width + (float)element.X, croppedBitmap.SourceRect.Height + (float)element.Y);

            skCanvas.DrawBitmap(skBitmapSource, source, dest);
        }
        
        // Apply crop : 
        // https://stackoverflow.com/questions/74216749/extracting-image-subset-cropping-with-skiasharp

        using var pixmap =  new SKPixmap(skBitmapCrop.Info, skBitmapCrop.GetPixels());
        SkiaSharp.SKRectI rectI = new SkiaSharp.SKRectI(OffsetWidth, OffsetHeight, firstCroppedBitmap.SourceRect.Width - OffsetWidth, firstCroppedBitmap.SourceRect.Height - OffsetHeight);

        var subset = pixmap.ExtractSubset(rectI);
        
        // Apply scaling : 
        using var skBitmapCrop2 = new SKBitmap(ExportWidth, ExportHeight);
        
        using var skBitmapCrop3 = SKBitmap.FromImage(SKImage.FromEncodedData(subset.Encode(SkiaSharp.SKPngEncoderOptions.Default)));
        
        if (!skBitmapCrop3.ScalePixels(skBitmapCrop2, SKFilterQuality.High))
        {
            Debug.Assert(true);
        }
            
        // Write to memory stream (before finally save file)
        MemoryStream memoryStream = new MemoryStream();
            
        SKData d = SKImage.FromBitmap(skBitmapCrop2).Encode(SKEncodedImageFormat.Png, 100);
        d.SaveTo(memoryStream);
            
        return memoryStream;
    }
    
    public List<CroppedBitmap> GetImages()
    {
        return [.. MapImages
            .Select(part => part.Image)
            .OfType<CroppedBitmap>(), ..Paths.SelectMany(path => path.GetImages())];
    }
    
    public List<MapElementModel> GetElementsToDisplay()
    {
        if (DisplayMap)
        {
            return [.. MapImages.Select(part => part), ..Paths.Where(part => part.Shown).SelectMany(path => path.PathParts)];
        }
        else
        {
            return [..Paths.Where(part => part.Shown).SelectMany(path => path.PathParts)];
        }
    }
}