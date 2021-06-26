using MonkeyB.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using LiveCharts;
using MonkeyB.Models;
using MonkeyB.Database;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Threading;

namespace MonkeyB.ViewModels
{
    class IndexViewModel : BaseViewModel
    {
        public ICommand DashBoardCommand { get; set; }
        public ChartValues<double> CoinValue { get; set; }
        public ObservableCollection<string> CoinDate { get; set; }
        public ObservableCollection<string> CurrencyNames { get; set; }
        public ObservableCollection<TransactionHistoryModel> CryptoWalletList { get; set; }

        private ApiHandler api = new ApiHandler();
        
        /// <summary>
        /// Constructor for the IndexViewModel
        /// </summary>
        /// <param name="navigationStore">Object which stores the currently selected view</param>
        public IndexViewModel(NavigationStore navigationStore)
        {
            DisplayGrowthDeclinePercentage();
            CurrencyNames = new ObservableCollection<string>() { "bitcoin", "dogecoin", "litecoin" };
            GetMarketData(CurrencyNames[0]);
            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });
        }
        
        /// <summary>
        /// Gets current data from the api and splices them into 2 seperate lists, dates get converted from unxitime to
        /// DateTime. The lists are then put in their respective collections.
        /// </summary>
        /// <param name="id"></param>
        private async void GetMarketData(string id)
        {
            MarketGraph marketGraph = await api.GetMarketData(id, "eur", 91);
            List<string> dates = new();
            List<double> prices = new();
            foreach (var price in marketGraph.prices)
            {
                prices.Add(price[1]);
                dates.Add(ToDateTime((long)price[0]).ToString(CultureInfo.CurrentCulture));
            }
            CoinValue = new ChartValues<double>(prices);
            CoinDate = new ObservableCollection<string>(dates);
            OnPropertyChanged(nameof(CoinValue));
        }

        /// <summary>
        /// Takes a unixtime value and returns a DateTime
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns>A new DateTime object</returns>
        private static DateTime ToDateTime(long unixTime) {  
            return new DateTime(1970, 1, 1).Add(TimeSpan.FromMilliseconds(unixTime));  
        }

        /// <summary>
        /// Displays and calculates growth and decline rate of cryptos
        /// </summary>
        public void DisplayGrowthDeclinePercentage() {
            List<TransactionHistoryModel> cryptoWallet = DataBaseAccess.FetchcoinAndAmount(App.UserID);

            CryptoWalletList = new ObservableCollection<TransactionHistoryModel>();
            ApiHandler apiHandler = new ApiHandler();

            CryptoCurrencyModel model;
            MarketGraph marketModel;

            Task.Run(() =>
            {
                foreach (var crypto in cryptoWallet)
                {
                    model = apiHandler.GetCoinValue(crypto.coinName).Result;
                    marketModel = apiHandler.GetMarketData(crypto.coinName, "eur", 7).Result;
                    switch (crypto.coinName)
                    {
                        case "bitcoin":
                            crypto.coinValue = model.bitcoin.eur;
                            crypto.oldCoinValue = (float)marketModel.prices[6][1];
                            break;
                        case "litecoin":
                            crypto.coinValue = model.litecoin.eur;
                            crypto.oldCoinValue = (float)marketModel.prices[6][1];
                            break;
                        case "dogecoin":
                            crypto.coinValue = model.dogecoin.eur;
                            crypto.oldCoinValue = (float)marketModel.prices[6][1];
                            break;
                    }


                    crypto.calculatePercentage();

                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        CryptoWalletList.Add(crypto);
                    }));
                }
            });
        }
        
        /// <summary>
        /// Gets called when a different value is selected in the listbox
        /// </summary>
        private string _selectedCurrencyName;
        public string SelectedCurrencyName
        {
            get => _selectedCurrencyName;
            set
            {
                if (_selectedCurrencyName == value) return;
                _selectedCurrencyName = value;
                GetMarketData(_selectedCurrencyName);
            }
        }
    }
}
