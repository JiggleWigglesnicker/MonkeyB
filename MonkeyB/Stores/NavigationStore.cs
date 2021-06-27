using System;
using MonkeyB.ViewModels;

namespace MonkeyB
{
    public class NavigationStore
    {
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

        public event Action SelectedViewModelChanged;

        private void OnSelectedViewModelChanged()
        {
            SelectedViewModelChanged?.Invoke();
        }
    }
}