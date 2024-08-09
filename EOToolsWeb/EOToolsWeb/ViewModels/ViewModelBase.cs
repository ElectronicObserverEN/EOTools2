using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Services;

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

        public ShowDialogService? ShowDialogService { get; set; }

        /// <summary>
        /// Show an error message when an exception is thrown
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected async Task HandleException(Exception ex)
        {
            if (ShowDialogService is null)
            {
                throw ex;
            }

            await ShowDialogService.ShowMessage("Error", ex.Message);
        }
    }
}
