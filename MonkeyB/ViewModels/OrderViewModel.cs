﻿using MonkeyB.Commands;
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

        /// <summary>
        /// Sets the buy, sell and dashboardbuttons of the view and fills the selectbox and listbox of the view with the orders of all users and the crypto's in the current user wallet.
        /// </summary>
        /// <param name="navigationStore">Stores the currently selected viewmodel which is used to display a view</param>
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
                if (CryptoWalletCoins != null && SelectedBuyOrder != null && CryptoWalletCoins.Count > 0)
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

        /// <summary>
        /// Fills the 'sell order' selectbox with the users currently owned cryptocurrency.
        /// </summary>
        public void FillSelectBox()
        {

            CryptoWalletCoins = new ObservableCollection<CryptoWalletModel>();
            List<CryptoWalletModel> coinsDictonary = DataBaseAccess.FetchCoinsInWallet(App.UserID);
            foreach (var coin in coinsDictonary)
            {
                CryptoWalletCoins.Add(coin);
            }
        }

        /// <summary>
        /// creates a sell order and stores the order in the database.
        /// </summary>
        /// <param name="type"> type of cryptocurrency</param>
        /// <param name="coinAmount">amount of cryptocurrency</param>
        /// <param name="euroAmount">selling price</param>
        /// <param name="id"> the id of the user</param>
        public void PlaceSellOrder(string type, float coinAmount, float euroAmount, int id)
        {
            DataBaseAccess.CreateNewSellOrder(type, coinAmount, euroAmount, id);
        }

        /// <summary>
        /// Allows a user to buy an order and updates the order in the database to no longer being an outstanding order.
        /// </summary>
        /// <param name="userID"> the id of the user</param>
        /// <param name="orderID">the id of the order</param>
        /// <param name="orderModel"> the order object which is currently selected in the listview</param>
        public void BuyOrder(int userID, int orderID, OrderModel orderModel)
        {
            DataBaseAccess.BuyOrder(userID, orderModel);
        }

        /// <summary>
        /// Fills the listview with the currently outstanding orders in the database.
        /// </summary>
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
