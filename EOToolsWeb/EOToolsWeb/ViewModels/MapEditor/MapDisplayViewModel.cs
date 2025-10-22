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
        // https://github.com/AvaloniaUI/Avalonia/discussions/13772
        List<CroppedBitmap> images = GetImages();

        if (images.Count == 0) return new MemoryStream();
        
        CroppedBitmap firstCroppedBitmap = images[0];
        using var skBitmapCrop = new SKBitmap(firstCroppedBitmap.SourceRect.Width, firstCroppedBitmap.SourceRect.Height);
        using var skCanvas = new SKCanvas(skBitmapCrop);
        
        foreach (var croppedBitmap in images)
        {
            var image = croppedBitmap.Source as Bitmap;
            Stream xxStream = new MemoryStream();
            image.Save(xxStream);
            xxStream.Seek(0, SeekOrigin.Begin);
            var skBitmapSource = SKBitmap.Decode(xxStream);

            var source = new SKRect(croppedBitmap.SourceRect.X, croppedBitmap.SourceRect.Y, croppedBitmap.SourceRect.Width+ croppedBitmap.SourceRect.X, croppedBitmap.SourceRect.Height+ croppedBitmap.SourceRect.Y);
            var dest = new SKRect(0, 0, croppedBitmap.SourceRect.Width , croppedBitmap.SourceRect.Height);

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
    
    private List<CroppedBitmap> GetImages()
    {
        return MapImages.Select(image =>
        {
            return image.Image switch
            {
                CroppedBitmap croppedBitmap => croppedBitmap,
                _ => throw new NotSupportedException(),
            };
        }).ToList();
    }
}