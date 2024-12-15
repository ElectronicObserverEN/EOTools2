using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;
using EOToolsWeb.Shared.Users;

namespace EOToolsWeb.Converters;

public class UserKindConverter : IValueConverter
{
    public static readonly UserKindConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not UserKind kind) return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        if (!targetType.IsAssignableTo(typeof(string))) return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

        return kind switch
        {
            UserKind.UpdateUpdator => "Update manager",
            _ => kind.ToString(),
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
