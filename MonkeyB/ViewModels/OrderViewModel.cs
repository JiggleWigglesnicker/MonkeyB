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

        private string sellOrderFeedbackText;
        public string SellOrderFeedbackText
        {
            get => sellOrderFeedbackText;
            set
            {
                sellOrderFeedbackText = value;
                OnPropertyChanged("SellOrderFeedbackText");
            }
        }

        private string sellOrderFeedbackColor;
        public string SellOrderFeedbackColor
        {
            get => sellOrderFeedbackColor;
            set
            {
                sellOrderFeedbackColor = value;
                OnPropertyChanged("SellOrderFeedbackColor");
            }
        }

        private bool sellOrderFeedbackVisible;
        public bool SellOrderFeedbackVisible
        {
            get => sellOrderFeedbackVisible;
            set
            {
                sellOrderFeedbackVisible = value;
                OnPropertyChanged("SellOrderFeedbackVisible");
            }
        }

        private string buyOrderFeedbackText;
        public string BuyOrderFeedbackText
        {
            get => buyOrderFeedbackText;
            set
            {
                buyOrderFeedbackText = value;
                OnPropertyChanged("BuyOrderFeedbackText");
            }
        }

        private string buyOrderFeedbackColor;
        public string BuyOrderFeedbackColor
        {
            get => buyOrderFeedbackColor;
            set
            {
                buyOrderFeedbackColor = value;
                OnPropertyChanged("BuyOrderFeedbackColor");
            }
        }

        private bool buyOrderFeedbackVisible;
        public bool BuyOrderFeedbackVisible
        {
            get => buyOrderFeedbackVisible;
            set
            {
                buyOrderFeedbackVisible = value;
                OnPropertyChanged("BuyOrderFeedbackVisible");
            }
        }

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
            if (CryptoWalletCoins != null && CryptoWalletCoins.Count > 0)
            {
                EuroAmountLabel = $"Total amount of euro: {CryptoWalletCoins[0].euroAmount}";
            }

            BuyCommand = new RelayCommand(o =>
            {
                if (CryptoWalletCoins != null && SelectedBuyOrder != null)
                {

                    if (CryptoWalletCoins[0].euroAmount >= selectedBuyOrder.EuroAmount || App.UserID == SelectedBuyOrder.UserID)
                    {
                        BuyOrder(App.UserID, SelectedBuyOrder.ID, SelectedBuyOrder);
                        navigationStore.SelectedViewModel = new OrderViewModel(navigationStore);
                    }
                    else
                    {
                        BuyOrderFeedbackColor = "Red";
                        BuyOrderFeedbackText = "Order could not be purchased, Insufficent funds";
                        BuyOrderFeedbackVisible = true;
                    }
                }
                else
                {
                    BuyOrderFeedbackColor = "Red";
                    BuyOrderFeedbackText = "Order could not be purchased";
                    BuyOrderFeedbackVisible = true;
                }
            });

            SellCommand = new RelayCommand(o =>
            {
                if (SelectedItem != null)
                {
                    if (CoinAmount <= selectedItem.coinAmount && CoinAmount != 0)
                    {
                        PlaceSellOrder(SelectedItem.coinName, CoinAmount, EuroAmount, App.UserID);
                        navigationStore.SelectedViewModel = new OrderViewModel(navigationStore);
                        
                    }
                    else if (CoinAmount == 0.0)
                    {
                        SellOrderFeedbackColor = "Red";
                        SellOrderFeedbackText = "Sell order Could not be made, no crypto amount given";
                        SellOrderFeedbackVisible = true;
                    }
                    else
                    {
                        SellOrderFeedbackColor = "Red";
                        SellOrderFeedbackText = "Sell order Could not be made, Insufficent crypto";
                        SellOrderFeedbackVisible = true;
                    }
                }
                else
                {
                    SellOrderFeedbackColor = "Red";
                    SellOrderFeedbackText = "Sell order Could not be made, empty input";
                    SellOrderFeedbackVisible = true;
                }
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
            DataBaseAccess.BuyOrder(userID, orderModel);
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
