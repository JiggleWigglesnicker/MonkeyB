using System.Windows.Input;
using MonkeyB.Commands;
using MonkeyB.Database;

namespace MonkeyB.ViewModels
{
    class SettingViewModel : BaseViewModel
    {
        public float settingText;

        /// <summary>
        ///     Sets the dashboard and applybutton actions.
        ///     And allows users to set the money amount with which to start using the application.
        /// </summary>
        /// <param name="navigationStore">Stores the currently selected viewmodel which is used to display a view</param>
        public SettingViewModel(NavigationStore navigationStore)
        {
            ThemeCommand = new RelayCommand(UpdateAppTheme);

            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });

            ApplyCommand = new RelayCommand(o =>
            {
                if (SettingText >= 0)
                {
                    DataBaseAccess.UpdateEuroAmount(SettingText);
                }
            });
        }

        public ICommand DashBoardCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand ThemeCommand { get; set; }

        public float SettingText
        {
            get => settingText;
            set
            {
                settingText = value;
                OnPropertyChanged("SettingText");
            }
        }

        /// <summary>
        ///     updates the selected theme when the users selects a theme in the view
        /// </summary>
        /// <param name="parameter"></param>
        public void UpdateAppTheme(object parameter)
        {
            Properties.Settings.Default.ThemeSetting = (string) parameter;
            Properties.Settings.Default.Save();
        }
    }
}