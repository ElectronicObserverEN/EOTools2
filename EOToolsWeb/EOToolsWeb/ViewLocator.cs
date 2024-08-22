using Avalonia.Controls;
using Avalonia.Controls.Templates;
using EOToolsWeb.ViewModels;
using System;
using AvaloniaControl = Avalonia.Controls.Control;

namespace EOToolsWeb
{
    public class ViewLocator : IDataTemplate
    {
        public AvaloniaControl? Build(object? data)
        {
            if (data is null)
                return null;

            if (!Match(data))
            {
                return new TextBlock { Text = "Data doesn't herit from ViewModelBase" };
            }

            var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                AvaloniaControl ctrl = (AvaloniaControl)Activator.CreateInstance(type)!;
                ctrl.DataContext = data;
                return ctrl;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}