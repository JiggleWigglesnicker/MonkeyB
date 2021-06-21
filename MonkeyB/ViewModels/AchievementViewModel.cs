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
            AchievementList = new ObservableCollection<AchievementModel>();
            AchievementList.Add(new AchievementModel("First Bitcoin", "Buying your first bitcoin", false));
            AchievementList.Add(new AchievementModel("First Ethereum", "Buying your first Ethereum", false));
            AchievementList.Add(new AchievementModel("First Dogecoin", "buying your first Dogecoin", false));
            AchievementList.Add(new AchievementModel("Make Profit", "Making profit on a stock", false));
            checkIfAchievementCompleted();
            checkIfAchievementCompleted();
        }

        public void checkIfAchievementCompleted()
        {
            List<CryptoWalletModel> cWalletList = DataBaseAccess.FetchCoinsInWallet(App.UserID);
            if (cWalletList.Exists(e => e.coinName == "bitcoin"))
            {
                AchievementList[0] = new AchievementModel("First Bitcoin", "Buying your first bitcoin", true); 
            }
            else if (cWalletList.Exists(e => e.coinName == "etherium"))
            {
                AchievementList[1] = new AchievementModel("First Ethereum", "Buying your first Ethereum", true);
            }
            else if (cWalletList.Exists(e => e.coinName == "dogecoin"))
            {
                AchievementList[2] = new AchievementModel("First Dogecoin", "buying your first Dogecoin", true);
            }
            else if (cWalletList.Exists(e => e.euroAmount >= 10000))
            {
                AchievementList[3] = new AchievementModel("€€€ 10K €€€ CLUB", "Own € 10.000 ", true);
            }

        }

    }
}
