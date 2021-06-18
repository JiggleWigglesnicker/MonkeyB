using MonkeyB.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using LiveCharts;

namespace MonkeyB.ViewModels
{
    class IndexViewModel : BaseViewModel
    {
        private ICommand DashBoardCommand { get; set; }
        public ChartValues<double> CoinValue { get; set; }
        private ObservableCollection<string> CoinDate { get; set; }
        private ObservableCollection<string> CurrencyNames { get; set; }

        private ApiHandler api = new ApiHandler();
        
        /// <summary>
        /// Constructor for the IndexViewModel
        /// </summary>
        /// <param name="navigationStore"></param>
        public IndexViewModel(NavigationStore navigationStore)
        {
            
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
        /// <returns></returns>
        private static DateTime ToDateTime(long unixTime) {  
            return new DateTime(1970, 1, 1).Add(TimeSpan.FromMilliseconds(unixTime));  
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
