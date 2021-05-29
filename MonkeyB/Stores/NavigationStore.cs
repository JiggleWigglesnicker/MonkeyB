using MonkeyB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB
{
    public class NavigationStore
    {
        public event Action SelectedViewModelChanged;

        private BaseViewModel selectedViewModel { get; set; }
        public BaseViewModel SelectedViewModel
        {
            get => selectedViewModel;

            set
            {
                selectedViewModel = value;
                OnSelectedViewModelChanged();
            }
        }

        private void OnSelectedViewModelChanged()
        {
            SelectedViewModelChanged?.Invoke();
        }

    }
}
