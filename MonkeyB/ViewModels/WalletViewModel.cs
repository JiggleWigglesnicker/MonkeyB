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

        public WalletViewModel(NavigationStore navigationStore)
        {
            LoadWalletIntoChart(App.UserID);
            List<TransactionHistoryModel> cryptoWallet = DataBaseAccess.FetchTransactionHistory(App.UserID);

            CryptoWalletList = new ObservableCollection<TransactionHistoryModel>();

            ApiHandler apiHandler = new ApiHandler();

            CryptoCurrencyModel model;

            Task.Run(() =>
            {
                foreach (var crypto in cryptoWallet)
                {
                    model = apiHandler.GetCoinValue(crypto.coinName).Result;

                    switch (crypto.coinName)
                    {
                        case "bitcoin":
                            crypto.coinValue = model.bitcoin.eur;
                            break;
                        case "litecoin":
                            crypto.coinValue = model.litecoin.eur;
                            break;
                        case "dogecoin":
                            crypto.coinValue = model.dogecoin.eur;
                            break;
                    }

                    crypto.calculatePercentage();

                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        CryptoWalletList.Add(crypto);
                    }));
                }
            });

            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });
        }

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
