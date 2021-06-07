using MonkeyB.Commands;
using MonkeyB.Models;
using System;
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
            AchievementList = new ObservableCollection<AchievementModel>();
            AchievementList.Add(new AchievementModel("First Bitcoin", "Buying your first bitcoin", false));
            AchievementList.Add(new AchievementModel("First Ethereum", "Buying your first Ethereum", false));
            AchievementList.Add(new AchievementModel("First Dogecoin", "buying your first Dogecoin", false));
            AchievementList.Add(new AchievementModel("Make Profit", "Making profit on a stock", true));
        }

        public void checkIfAchievementCompleted()
        {
            
        }

    }
}
