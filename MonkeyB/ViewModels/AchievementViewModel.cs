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
                checkIfAchievementCompleted(achievement);
                AchievementList.Add(achievement);
            }


        }

        public void checkIfAchievementCompleted(AchievementModel model)
        {
            switch (model.Name)
            {
                case "10 Doge":
                    if (DataBaseAccess.GetCoinAmount("dogecoin", App.UserID) >= 10)
                        model.IsCompleted = true;
                    break;
                case "10 litecoin":
                    if (DataBaseAccess.GetCoinAmount("litecoin", App.UserID) >= 10)
                        model.IsCompleted = true;
                    break;
                case "10 bit":
                    if (DataBaseAccess.GetCoinAmount("bitcoin", App.UserID) >= 10)
                        model.IsCompleted = true;
                    break;
                case "10k CLUB":
                    if (DataBaseAccess.GetEuroAmount() >= 10000)
                        model.IsCompleted = true;
                    break;

            }

        }

    }
}
