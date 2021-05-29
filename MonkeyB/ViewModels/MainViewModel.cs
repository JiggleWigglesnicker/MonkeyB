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


        public MainViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            this.navigationStore.SelectedViewModelChanged += OnSelectedViewModelChanged;
        }

        private void OnSelectedViewModelChanged()
        {
            OnPropertyChanged(nameof(SelectedViewModel));
        }

    }
}
