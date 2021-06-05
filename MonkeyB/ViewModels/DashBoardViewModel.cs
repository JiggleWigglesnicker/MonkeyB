using MonkeyB.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonkeyB.ViewModels
{
    class DashBoardViewModel : BaseViewModel
    {
        public ICommand IndexCommand { get; set; }
        public ICommand BuySellCommand { get; set; }
        public ICommand NewsCommand { get; set; }
        public ICommand AchievementCommand { get; set; }
        public ICommand SettingCommand { get; set; }

        public DashBoardViewModel(NavigationStore navigationStore)
        {
            IndexCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new IndexViewModel(navigationStore);
            }); 

            BuySellCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new BuySellViewModel(navigationStore);
            });

            NewsCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new NewsViewModel(navigationStore);
            });

            AchievementCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new AchievementViewModel(navigationStore);
            });

            SettingCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new SettingViewModel(navigationStore);
            });
        }

    }
}
