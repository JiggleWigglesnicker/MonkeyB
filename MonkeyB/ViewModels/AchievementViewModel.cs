using MonkeyB.Commands;
using MonkeyB.Database;
using MonkeyB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonkeyB.ViewModels
{
    class AchievementViewModel : BaseViewModel
    {
        public ICommand DashBoardCommand { get; set; }

        public ObservableCollection<AchievementModel> AchievementList { get; set; }

        public AchievementViewModel(NavigationStore navigationStore)
        {
            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });

            addAchievementsToList();
        }

        public void addAchievementsToList()
        {
            List<AchievementModel> list = DataBaseAccess.FetchAchievements(App.UserID);
            AchievementList = new ObservableCollection<AchievementModel>();
            foreach (var achievement in list)
            {
                AchievementList.Add(achievement);
            }

            checkIfAchievementCompleted();
        }

        public void checkIfAchievementCompleted()
        {
            

        }

    }
}
