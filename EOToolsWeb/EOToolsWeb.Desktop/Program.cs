using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace EOToolsWeb.Desktop
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // TODO : Need to replace the nuget Avalonia.ReactiveUI by ReactiveUI.Avalonia
        // This can't be done for the moment cause of an issue on startup
        // See https://github.com/reactiveui/ReactiveUI/issues/4123

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();
    }
}
