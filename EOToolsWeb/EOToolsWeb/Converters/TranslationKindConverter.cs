using Avalonia.Data;
using Avalonia.Data.Converters;
using EOToolsWeb.Extensions.Translations;
using EOToolsWeb.Shared.Translations;
using System;
using System.Globalization;

namespace EOToolsWeb.Converters;

public class TranslationKindConverter : IValueConverter
{
    public static readonly TranslationKindConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not TranslationKind kind) return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        if (!targetType.IsAssignableTo(typeof(string))) return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

        return kind.GetName();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
