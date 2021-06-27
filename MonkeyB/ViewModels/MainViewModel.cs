using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MonkeyB.Commands;

namespace MonkeyB.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        private NavigationStore navigationStore;

        public BaseViewModel SelectedViewModel => navigationStore.SelectedViewModel;

        /// <summary>
        /// Sets the navigationstore of the mainwindow and constanly checks if the selectedviewmodel has changed in the navigationstore
        /// </summary>
        /// <param name="navigationStore">Stores the currently selected viewmodel which is used to display a view</param>
        public MainViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            this.navigationStore.SelectedViewModelChanged += OnSelectedViewModelChanged;
        }

        /// <summary>
        /// Triggers when the selectedviewmodel property has changed
        /// </summary>
        private void OnSelectedViewModelChanged()
        {
            OnPropertyChanged(nameof(SelectedViewModel));
        }

    }
}
