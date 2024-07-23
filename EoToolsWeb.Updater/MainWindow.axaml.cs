using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EoToolsWeb.Updater
{
    public partial class MainWindow : Window
    {
        public UpdaterViewModel ViewModel { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = ViewModel;
        }

        protected override async void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            await ViewModel.CheckForUpdate();

            if (ViewModel.UpdateDone)
            {
                string currentDir = Directory.GetCurrentDirectory();

                Process.Start(new ProcessStartInfo()
                {
                    FileName = Path.Combine(currentDir, "EoTools", "EOToolsWeb.Desktop.exe"),
                    WorkingDirectory = Path.Combine(currentDir, "EoTools"),
                    
                });

                Close();
            }
        }
    }
}