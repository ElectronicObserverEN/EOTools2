using EOToolsWeb.ViewModels.Login;

namespace EOToolsWeb.ViewModels
{
    public class MainViewModel(LoginViewModel login) : ViewModelBase
    {
        public LoginViewModel Login => login;
    }
}
