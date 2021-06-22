using MonkeyB.Commands;
using MonkeyB.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonkeyB.ViewModels
{
    class SettingViewModel : BaseViewModel
    {
        public ICommand DashBoardCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand ThemeCommand { get; set; }

        public float settingText;
        public float SettingText
        {
            get => settingText;
            set
            {
                settingText = value;
                OnPropertyChanged("SettingText");
            }
        }

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
                    DataBaseAccess.updateEuroAmount(SettingText);
                }

            });
        }

        public void UpdateAppTheme(object parameter)
        {
            Properties.Settings.Default.ThemeSetting = (string)parameter;
            Properties.Settings.Default.Save();
        }


    }
}
