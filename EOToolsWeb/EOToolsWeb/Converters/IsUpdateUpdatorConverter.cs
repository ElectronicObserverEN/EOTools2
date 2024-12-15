﻿using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;
using EOToolsWeb.Shared.Users;

namespace EOToolsWeb.Converters;

public class IsUpdateUpdatorConverter : IValueConverter
{
    public static readonly IsUpdateUpdatorConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not UserKind kind) return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        if (!targetType.IsAssignableTo(typeof(bool))) return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

        return kind is UserKind.Admin or UserKind.UpdateUpdator;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
