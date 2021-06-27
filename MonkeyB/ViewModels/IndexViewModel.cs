using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using LiveCharts;
using MonkeyB.Commands;
using MonkeyB.Database;
using MonkeyB.Models;

namespace MonkeyB.ViewModels
{
    internal class IndexViewModel : BaseViewModel
    {
        /// <summary>
        ///     Gets called when a different value is selected in the listbox
        /// </summary>
        private string _selectedCurrencyName;

        private readonly ApiHandler api = new();

        /// <summary>
        ///     Constructor for the IndexViewModel
        /// </summary>
        /// <param name="navigationStore">Object which stores the currently selected view</param>
        public IndexViewModel(NavigationStore navigationStore)
        {
            DisplayGrowthDeclinePercentage();
            CurrencyNames = new ObservableCollection<string> {"bitcoin", "dogecoin", "litecoin"};
            GetMarketData(CurrencyNames[0]);
            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });
        }

        public ICommand DashBoardCommand { get; set; }

        /// <summary>
        ///     Collection of all values of the coins
        /// </summary>
        public ChartValues<double> CoinValue { get; set; }

        /// <summary>
        ///     Collection of all coin dates
        /// </summary>
        public ObservableCollection<string> CoinDate { get; set; }

        /// <summary>
        ///     Collection of currency names
        /// </summary>
        public ObservableCollection<string> CurrencyNames { get; set; }

        public ObservableCollection<TransactionHistoryModel> CryptoWalletList { get; set; }

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

        /// <summary>
        ///     Gets current data from the api and splices them into 2 seperate lists, dates get converted from unxitime to
        ///     DateTime. The lists are then put in their respective collections.
        /// </summary>
        /// <param name="id">the id of the requested coin, eg: "bitcoin" "litecoin"</param>
        private async void GetMarketData(string id)
        {
            var marketGraph = await api.GetMarketData(id, "eur", 91);
            List<string> dates = new();
            List<double> prices = new();
            foreach (var price in marketGraph.prices)
            {
                prices.Add(price[1]);
                dates.Add(ToDateTime((long) price[0]).ToString(CultureInfo.CurrentCulture));
            }

            CoinValue = new ChartValues<double>(prices);
            CoinDate = new ObservableCollection<string>(dates);
            OnPropertyChanged(nameof(CoinValue));
        }

        /// <summary>
        ///     Takes a unixtime value and returns a DateTime
        /// </summary>
        /// <param name="unixTime">unixtime in miliseconds</param>
        /// <returns>A new DateTime object</returns>
        private static DateTime ToDateTime(long unixTime)
        {
            return new DateTime(1970, 1, 1).Add(TimeSpan.FromMilliseconds(unixTime));
        }

        /// <summary>
        ///     Displays and calculates growth and decline rate of cryptos
        /// </summary>
        public void DisplayGrowthDeclinePercentage()
        {
            var cryptoWallet = DataBaseAccess.FetchcoinAndAmount(App.UserID);

            CryptoWalletList = new ObservableCollection<TransactionHistoryModel>();
            var apiHandler = new ApiHandler();

            CryptoCurrencyModel model;
            MarketGraph marketModel;

            Task.Run(async () =>
            {
                foreach (var crypto in cryptoWallet)
                {
                    model = await apiHandler.GetCoinValue(crypto.coinName);
                    marketModel = await apiHandler.GetMarketData(crypto.coinName, "eur", 7);
                    switch (crypto.coinName)
                    {
                        case "bitcoin":
                            crypto.coinValue = model.bitcoin.eur;
                            crypto.oldCoinValue = (float) marketModel.prices[6][1];
                            break;
                        case "litecoin":
                            crypto.coinValue = model.litecoin.eur;
                            crypto.oldCoinValue = (float) marketModel.prices[6][1];
                            break;
                        case "dogecoin":
                            crypto.coinValue = model.dogecoin.eur;
                            crypto.oldCoinValue = (float) marketModel.prices[6][1];
                            break;
                    }


                    crypto.calculatePercentage();

                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                        new ThreadStart(delegate { CryptoWalletList.Add(crypto); }));
                }
            });
        }
    }
}