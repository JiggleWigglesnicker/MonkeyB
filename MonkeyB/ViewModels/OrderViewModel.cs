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

        private OrderModel selectedBuyOrder;
        public OrderModel SelectedBuyOrder
        {
            get => selectedBuyOrder;
            set
            {
                selectedBuyOrder = value;
                OnPropertyChanged("SelectedBuyOrder");
            }
        }

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
            EuroAmountLabel = $"Total amount of euro: {CryptoWalletCoins[0].euroAmount}";
            BuyCommand = new RelayCommand(o =>
            {
                if (CryptoWalletCoins[0].euroAmount > selectedBuyOrder.EuroAmount)
                    BuyOrder(App.UserID, SelectedBuyOrder.ID, SelectedBuyOrder);

            });

            SellCommand = new RelayCommand(o =>
            {

                if (CoinAmount <= selectedItem.coinAmount)
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

        public void BuyOrder(int userID, int orderID, OrderModel orderModel)
        {
            DataBaseAccess.BuyOrder(userID, orderID, orderModel);
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
