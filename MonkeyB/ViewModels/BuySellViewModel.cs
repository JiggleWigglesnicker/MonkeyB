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

        public float eurRate;
        public float EurRate
        {
            get => eurRate;
            set
            {
                eurRate = value;
                OnPropertyChanged("EurRate");
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


        private ApiHandler apiHandler = new ApiHandler();

        private CryptoCurrencyModel eurModel = new();
        private CryptoCurrencyModel bitCoinModel = new();
        private  CryptoCurrencyModel dogeCoinModel = new();
        private CryptoCurrencyModel liteCoinModel = new();

        // Constructor
        public BuySellViewModel(NavigationStore navigationStore)
        {
            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });

            RefreshCommand = new RelayCommand(o =>
            {
                RefreshCoinRates();
            });

            BuyCoinCommand = new RelayCommand(o =>
            {
                RefreshCoinRates();
                BuyCrypto(CurrencyName, Amount);
            });

            SellCoinCommand = new RelayCommand(o =>
            {
                RefreshCoinRates();
                SellCrypto("bitcoin", Amount);
            });

            CurrencyNames = new ObservableCollection<string>() { "bitcoin", "dogecoin", "litecoin" };
            RefreshCoinRates();
            RefreshCryptoToEuro();
        }

        private void RefreshCoinRates()
        {
            EurRate             = GetCoinAmount("eur");
            BitcoinRate         = GetCoinAmount("bitcoin");
            DogeCoinRate        = GetCoinAmount("dogecoin");
            LiteCoinRate        = GetCoinAmount("litecoin");
        }

        private async void RefreshCryptoToEuro()
        {
            eurModel = await apiHandler.GetCoinValue("eur");
            bitCoinModel = await apiHandler.GetCoinValue("bitcoin");
            dogeCoinModel = await apiHandler.GetCoinValue("dogecoin");
            liteCoinModel = await apiHandler.GetCoinValue("liteCoin");
        }

        
       // public async Task<float> GetCoinRateAsync(string CoinName)
        //{
            

        //}

        public static bool BuyCrypto(string currency, float amount)
        {
            if (CheckIfTransactionIsValid(currency) == true)
            {
                DataBaseAccess.SellCoin("eur", amount, App.UserID);
                DataBaseAccess.BuyCoin(currency, amount, App.UserID);

                return true;
            } else
            {
                return false;
            }
        }

        public static bool SellCrypto(string currency, float amount)
        {
            if (CheckIfTransactionIsValid(currency) == true)
            {
                DataBaseAccess.SellCoin(currency, amount, App.UserID);
                DataBaseAccess.BuyCoin("eur", amount, App.UserID);

                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckIfTransactionIsValid(string fromCurrency, string toCurrency = "eur")
        {
            float fromAmount = DataBaseAccess.GetCoinAmount(fromCurrency, App.UserID);
            float toAmount = DataBaseAccess.GetCoinAmount(toCurrency, App.UserID);

            if(fromAmount >= toAmount)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public static float GetCoinAmount(string currency)
        {
            int userId = App.UserID;
            return DataBaseAccess.GetCoinAmount(currency, userId);
        }

    }
}