using Avalonia.Controls;
using Avalonia.Controls.Templates;
using EOToolsWeb.ViewModels;
using System;

namespace EOToolsWeb
{
    public class ViewLocator : IDataTemplate
    {
        public Control? Build(object? data)
        {
            if (data is null)
                return null;

            if (!Match(data))
            {
                return new TextBlock { Text = "Data doesn't herit from ViewModelBase" };
            }
            var name = data.GetType().FullName!.Replace("BrowserViewModel", "View", StringComparison.Ordinal);
            name = name.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                Control ctrl = (Control)Activator.CreateInstance(type)!;
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