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

        /// <summary>
        /// Sets the dashboard button of the view and loads the achievements from the database into the view of a specific user and sets the navigationstore
        /// </summary>
        /// <param name="navigationStore">Stores the currently selected viewmodel which is used to display a view</param>
        public AchievementViewModel(NavigationStore navigationStore)
        {
            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });

            addAchievementsToList();
        }

        /// <summary>
        /// Adds the achievements in the database to the listbox of the view
        /// </summary>
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

        /// <summary>
        /// Checks if a user has completed an achievement and stores the result in the database
        /// </summary>
        /// <param name="model">AchievementModel which holds the achievement data</param>
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
