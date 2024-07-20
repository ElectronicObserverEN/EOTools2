using Avalonia.Controls;
using Avalonia.Interactivity;
using EOToolsWeb.ViewModels;
using EOToolsWeb.ViewModels.Login;

namespace EOToolsWeb.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private ILoginViewModel? LoginViewModel => DataContext is MainViewModel vm ? vm.Login : null;
        private MainViewModel? MainViewModel => DataContext as MainViewModel;

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            if (LoginViewModel is null || MainViewModel is null)
            {
                return;
            }

            MainViewModel.PropertyChanged += MainViewModel_PropertyChanged;
            MainViewModel.CurrentViewModel = LoginViewModel;
        }

        private void MainViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is not nameof(MainViewModel.CurrentViewModel)) return;

            MainContent.Content = new ViewLocator().Build(MainViewModel?.CurrentViewModel);
        }
    }
}