using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EOToolsWeb.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        /// <summary>
        /// Trigerred when the view is changed in the main window
        /// </summary>
        /// <returns></returns>
        public virtual Task OnViewClosing()
        {
            return Task.CompletedTask;
        }
    }
}
