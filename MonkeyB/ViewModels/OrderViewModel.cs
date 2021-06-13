using MonkeyB.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MonkeyB.Database;

namespace MonkeyB.ViewModels
{
    class OrderViewModel : BaseViewModel
    {

        public ICommand DashBoardCommand { get; set; }
        public ICommand SellCommand { get; set; }

        public ObservableCollection<string> Coins { get; set; }

        private float coinAmount;
        public float CoinAmount
        {
            get => coinAmount;
            set
            {
                coinAmount = value;
                OnPropertyChanged("CoinAmount");
            }
        }

        private float euroAmount;
        public float EuroAmount
        {
            get => euroAmount;
            set
            {
                euroAmount = value;
                OnPropertyChanged("EuroAmount");
            }
        }


        public OrderViewModel(NavigationStore navigationStore)
        {
            FillSelectBox();

            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });

        }

        public void FillSelectBox() {
            Coins = new ObservableCollection<string>();
            Dictionary<String,float> coinsDictonary = DataBaseAccess.GetCoinsInWallet(App.UserID);
            foreach (var coin in coinsDictonary) {
                Coins.Add("Type: "+coin.Key+" Amount: "+ coin.Value);
            }
        }

    }
}
