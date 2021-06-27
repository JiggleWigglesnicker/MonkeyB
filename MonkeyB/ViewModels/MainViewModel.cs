namespace MonkeyB.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private NavigationStore navigationStore;

        /// <summary>
        ///     Sets the navigationstore of the mainwindow and constanly checks if the selectedviewmodel has changed in the
        ///     navigationstore
        /// </summary>
        /// <param name="navigationStore">Stores the currently selected viewmodel which is used to display a view</param>
        public MainViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            this.navigationStore.SelectedViewModelChanged += OnSelectedViewModelChanged;
        }

        public BaseViewModel SelectedViewModel => navigationStore.SelectedViewModel;

        /// <summary>
        ///     Triggers when the selectedviewmodel property has changed
        /// </summary>
        private void OnSelectedViewModelChanged()
        {
            OnPropertyChanged(nameof(SelectedViewModel));
        }
    }
}