using EOToolsWeb.ViewModels.Login;
using EOToolsWeb.ViewModels.Updates;

namespace EOToolsWeb.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public LoginViewModel Login { get; }
        public UpdateManagerViewModel Updates { get; }

        public MainViewModel(LoginViewModel login, UpdateManagerViewModel updates)
        {
            Login = login;
            Updates = updates;

            login.OnAfterLogin += Login_OnAfterLogin;
        }

        private void Login_OnAfterLogin(object? sender, System.EventArgs e)
        {
            Updates.LoadAllUpdates();
        }
    }
}
