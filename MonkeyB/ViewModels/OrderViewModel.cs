using MonkeyB.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MonkeyB.Database;
using MonkeyB.Models;

namespace MonkeyB.ViewModels
{
    class OrderViewModel : BaseViewModel
    {

        public ICommand DashBoardCommand { get; set; }
        public ICommand SellCommand { get; set; }
        public ICommand BuyCommand { get; set; }

        public ObservableCollection<CryptoWalletModel> CryptoWalletCoins { get; set; }
        public ObservableCollection<OrderModel> OrderList { get; set; }

        private string euroAmountLabel;
        public string EuroAmountLabel
        {
            get => euroAmountLabel;
            set
            {
                euroAmountLabel = value;
                OnPropertyChanged("EuroAmountLabel");
            }
        }

        private string coinAmountLabel;
        public string CoinAmountLabel
        {
            get => coinAmountLabel;
            set
            {
                coinAmountLabel = value;
                OnPropertyChanged("CoinAmountLabel");
            }
        }

        private CryptoWalletModel selectedItem;
        public CryptoWalletModel SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                CoinAmountLabel = $"Total {selectedItem.coinName} in wallet: {selectedItem.coinAmount}";
                EuroAmountLabel = $"Total Euro available: {selectedItem.euroAmount}";
                OnPropertyChanged("SelectedItem");
            }
        }

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
            FillListViewWithOrders();
            BuyCommand = new RelayCommand(o =>
            {

            });

            SellCommand = new RelayCommand(o =>
            {
                PlaceSellOrder(SelectedItem.coinName, CoinAmount, EuroAmount, App.UserID);
            });

            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });

        }

        public void FillSelectBox()
        {

            CryptoWalletCoins = new ObservableCollection<CryptoWalletModel>();
            List<CryptoWalletModel> coinsDictonary = DataBaseAccess.FetchCoinsInWallet(App.UserID);
            foreach (var coin in coinsDictonary)
            {
                CryptoWalletCoins.Add(coin);
            }
        }

        public void PlaceSellOrder(string type, float coinAmount, float euroAmount, int id)
        {
            DataBaseAccess.CreateNewSellOrder(type, coinAmount, euroAmount, id);
        }

        public void FillListViewWithOrders()
        {
            OrderList = new ObservableCollection<OrderModel>();
            foreach (var order in DataBaseAccess.FetchOrders(App.UserID))
            {
                OrderList.Add(order);
            }

        }


    }
}
