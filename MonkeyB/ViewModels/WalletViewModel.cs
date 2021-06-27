using LiveCharts;
using LiveCharts.Wpf;
using MonkeyB.Commands;
using MonkeyB.Database;
using MonkeyB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MonkeyB.ViewModels
{
    class WalletViewModel : BaseViewModel
    {
        public ICommand DashBoardCommand { get; set; }
        public ObservableCollection<TransactionHistoryModel> CryptoWalletList { get; set; }


        public SeriesCollection seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => seriesCollection;

            set
            {
                seriesCollection = value;
                OnPropertyChanged("SeriesCollection");
            }
        }

        /// <summary>
        ///  Sets the button of the dashboard button, and loads the cryptocurrencies 
        ///  in the piechart and displays the profits/loss in the view
        /// </summary>
        /// <param name="navigationStore">Stores the currently selected viewmodel which is used to display a view</param>
        public WalletViewModel(NavigationStore navigationStore)
        {
            LoadWalletIntoChart(App.UserID);
            DisplayProfitLose();
            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });
        }

        /// <summary>
        /// Retrieves the transactionhistory of the user from the database and displays the calculated profit or loss realtime in the view.
        /// </summary>
        public void DisplayProfitLose()
        {
            List<TransactionHistoryModel> transactionHistoryList = DataBaseAccess.FetchTransactionHistory(App.UserID);

            CryptoWalletList = new ObservableCollection<TransactionHistoryModel>();
            ApiHandler apiHandler = new ApiHandler();

            CryptoCurrencyModel model;

            Task.Run(async () =>
            {
                foreach (var transaction in transactionHistoryList)
                {
                    model = await apiHandler.GetCoinValue(transaction.coinName);
                    switch (transaction.coinName)
                    {
                        case "bitcoin":
                            transaction.coinValue = model.bitcoin.eur;
                            break;
                        case "litecoin":
                            transaction.coinValue = model.litecoin.eur;
                            break;
                        case "dogecoin":
                            transaction.coinValue = model.dogecoin.eur;
                            break;
                    }


                    transaction.calculateProfitOrLoss();

                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        CryptoWalletList.Add(transaction);
                    }));
                }
            });
        }

        /// <summary>
        /// Loads the cryptocurrencies from the users cryptowallet into the piechart in the view
        /// </summary>
        /// <param name="id"> id of the current user</param>
        public void LoadWalletIntoChart(int id)
        {

            List<CryptoWalletModel> walletList = DataBaseAccess.FetchCoinsInWallet(id);
            SeriesCollection = new SeriesCollection();
            foreach (CryptoWalletModel model in walletList)
            {
                SeriesCollection.Add(new PieSeries()
                {
                    Title = model.coinName,
                    Values = new ChartValues<float> { model.coinAmount },
                    DataLabels = true
                });

            }

        }

    }
}
