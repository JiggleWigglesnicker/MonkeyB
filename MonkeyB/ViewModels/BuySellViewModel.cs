using MonkeyB.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MonkeyB.Database;
using System.Collections.ObjectModel;

namespace MonkeyB.ViewModels
{
    class BuySellViewModel : BaseViewModel
    {
        public ICommand DashBoardCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand BuyCoinCommand { get; set; }
        public ICommand SellCoinCommand { get; set; }

        public ObservableCollection<string> CurrencyNames { get; set; }

        public string currencyName;
        public string CurrencyName
        {
            get => currencyName;
            set
            {
                currencyName = value;
                OnPropertyChanged("CurrencyName");
            }
        }

        public float bitcoinRate;
        public float BitcoinRate
        {
            get => bitcoinRate;
            set
            {
                bitcoinRate = value;
                OnPropertyChanged("BitcoinRate");
            }
        }

        public float liteCoinRate;
        public float LiteCoinRate
        {
            get => liteCoinRate;
            set
            {
                liteCoinRate = value;
                OnPropertyChanged("LiteCoinRate");
            }
        }

        public float dogeCoinRate;
        public float DogeCoinRate
        {
            get => dogeCoinRate;
            set
            {
                dogeCoinRate = value;
                OnPropertyChanged("DogeCoinRate");
            }
        }


        public float amount;
        public float Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }

        // Constructor
        public BuySellViewModel(NavigationStore navigationStore)
        {
            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });

            RefreshCommand = new RelayCommand(o =>
            {
                RefreshCoinRatesAsync();
            });

            BuyCoinCommand = new RelayCommand(o =>
            {
                RefreshCoinRatesAsync();
                BuyCrypto(CurrencyName, Amount);
            });

            SellCoinCommand = new RelayCommand(o =>
            {
                RefreshCoinRatesAsync();
                SellCrypto("bitcoin", Amount);
            });

            CurrencyNames = new ObservableCollection<string>() { "bitcoin", "dogecoin", "litecoin" };
            RefreshCoinRatesAsync();
        }

        private async void RefreshCoinRatesAsync()
        {
            ApiHandler apiHandler = new ApiHandler();

            CryptoCurrencyModel bitCoinModel = await apiHandler.GetCoinValue("bitcoin");
            CryptoCurrencyModel dogeCoinModel = await apiHandler.GetCoinValue("dogecoin");
            CryptoCurrencyModel liteCoinModel = await apiHandler.GetCoinValue("liteCoin");



            BitcoinRate = GetCoinAmount("bitcoin");
            DogeCoinRate = dogeCoinModel.dogecoin.eur;
            LiteCoinRate = liteCoinModel.litecoin.eur;
        }

        
        public float GetCoinRate(string CoinName)
        {
            if(CoinName == "Bitcoin")
            {
                return 40000;
            }
            else if(CoinName == "Dogecoin"){
                return 20;
            }
            else if (CoinName == "Eth")
            {
                return 3000;
            }
            else if (CoinName == "Lite")
            {
                return 666;
            }
            {
                return 0;
            }
        }

        public static void BuyCrypto(string currency, float amount)
        {
            int userId = App.UserID;
            DataBaseAccess.BuyCoin(currency, amount, userId);
        }

        public static void SellCrypto(string currency, float amount)
        {
            int userId = App.UserID;
            DataBaseAccess.SellCoin(currency, amount, userId);
        }

        public static float GetCoinAmount(string currency)
        {
            int userId = App.UserID;
            return DataBaseAccess.GetCoinAmount(currency, userId);
        }

    }
}
