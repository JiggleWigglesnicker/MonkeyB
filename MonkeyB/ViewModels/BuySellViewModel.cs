using MonkeyB.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MonkeyB.Database;
using System.Collections.ObjectModel;
using MonkeyB.Models;
using System.Diagnostics;

namespace MonkeyB.ViewModels
{
    class BuySellViewModel : BaseViewModel
    {
        public ICommand DashBoardCommand { get; set; }
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

        public string warningLabel;
        public string WarningLabel
        {
            get => warningLabel;
            set
            {
                warningLabel = value;
                OnPropertyChanged("WarningLabel");
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
        private CryptoCurrencyModel dogeCoinModel = new();
        private CryptoCurrencyModel liteCoinModel = new();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationStore">Stores the currently selected viewmodel which is used to display a view</param>
        public BuySellViewModel(NavigationStore navigationStore)
        {
            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });

            BuyCoinCommand = new RelayCommand(o =>
            {
                Debug.WriteLine(CurrencyName);
                BuyCrypto(CurrencyName, Amount);
                RefreshCoinRates();
            });

            SellCoinCommand = new RelayCommand(o =>
            {
                SellCrypto(CurrencyName, Amount);
                RefreshCoinRates();
            });

            DataBaseAccess.InitializeCoins();
            CurrencyNames = new ObservableCollection<string>() { "bitcoin", "dogecoin", "litecoin" };
            RefreshCoinRates();
            RefreshCryptoToEuro();
        }

        /// <summary>
        /// Gets the amount of crypto from the database.
        /// </summary>
        private void RefreshCoinRates()
        {
            EurRate = GetCoinAmount("eur");
            BitcoinRate = GetCoinAmount("bitcoin");
            DogeCoinRate = GetCoinAmount("dogecoin");
            LiteCoinRate = GetCoinAmount("litecoin");
        }

        /// <summary>
        /// Refreshes the CryptoCurrency Object with the most recent exchange rate.
        /// </summary>
        private async void RefreshCryptoToEuro()
        {
            eurModel = await apiHandler.GetCoinValue("eur");
            bitCoinModel = await apiHandler.GetCoinValue("bitcoin");
            dogeCoinModel = await apiHandler.GetCoinValue("dogecoin");
            liteCoinModel = await apiHandler.GetCoinValue("liteCoin");
        }

        /// <summary>
        /// Returns the exchange rate in euro
        /// </summary>
        /// <param name="CoinName"></param>
        /// <returns>Exchange rate in euro</returns>
        public float GetCoinRateInEuro(string CoinName)
        {
            switch (CoinName)
            {
                case "bitcoin":
                    return bitCoinModel.bitcoin.eur;
                case "dogecoin":
                    return dogeCoinModel.dogecoin.eur;
                case "litecoin":
                    return liteCoinModel.litecoin.eur;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Add crypto to wallet and remove euro from wallet
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="amount"></param>
        /// <returns>Bool that indicates if successfull</returns>
        public bool BuyCrypto(string currency, float amount)
        {
            if (currency != null && currency != "")
            {
                if (CheckIfBuyTransactionIsValid(currency, amount) == true)
                {
                    DataBaseAccess.SellEuro(amount * GetCoinRateInEuro(currency));
                    DataBaseAccess.BuyCoin(currency, amount, App.UserID);

                    WarningLabel = "Bought " + amount + " " + currency;
                    return true;
                }
                else
                {
                    WarningLabel = "Not enought euro in wallet.";
                    return false;
                }
                
            }
            return false;
        }

        /// <summary>
        /// Add euro to wallet and remove crypto
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="amount"></param>
        /// <returns>Bool that indicates if succesfull</returns>
        public bool SellCrypto(string currency, float amount)
        {
            if (CheckIfSellTransactionIsValid(currency, amount) == true)
            {
                DataBaseAccess.SellCoin(currency, amount, App.UserID);
                DataBaseAccess.BuyEuro(amount * GetCoinRateInEuro(currency));

                WarningLabel = "Sold " + amount + " " + currency;
                return true;
            }
            else
            {
                WarningLabel = "Not enough " + currency + " in wallet.";
                return false;
            }
        }

        /// <summary>
        /// Check if user has enough euro to complete the transaction
        /// </summary>
        /// <param name="ToCurrency"></param>
        /// <param name="amount"></param>
        /// <returns>Bool that indicates if transaction is valid</returns>
        private bool CheckIfBuyTransactionIsValid(string ToCurrency, float amount)
        {
            float euroAmount = DataBaseAccess.GetEuroAmount();


            float rate = GetCoinRateInEuro(ToCurrency);

            float cost = amount * rate;

            if (euroAmount >= cost)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if user has enough crypto in the wallet to complete the transaction
        /// </summary>
        /// <param name="FromCurrency"></param>
        /// <param name="amount"></param>
        /// <returns>Bool that indicates if transaction is valid</returns>
        private static bool CheckIfSellTransactionIsValid(string FromCurrency, float amount)
        {
            float CryptoAmount = DataBaseAccess.GetCoinAmount(FromCurrency, App.UserID);

            if (CryptoAmount >= amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// CGet the amount of coin that a user has from the database
        /// </summary>
        /// <param name="currency"></param>
        /// <returns>The amount as a float</returns>
        public static float GetCoinAmount(string currency)
        {
            if (currency == "eur")
            {
                return DataBaseAccess.GetEuroAmount();
            }
            else
            {
                return DataBaseAccess.GetCoinAmount(currency, App.UserID);
            }
        }

    }
}