using MonkeyB.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using LiveCharts;

namespace MonkeyB.ViewModels
{
    class IndexViewModel : BaseViewModel
    {
        public ICommand DashBoardCommand { get; set; }
        
        public ChartValues<double> CoinValue { get; set; }
        public ObservableCollection<string> CoinDate { get; set; }

        public ObservableCollection<string> CurrencyNames { get; set; }
        
        private ApiHandler api = new ApiHandler();

        public IndexViewModel(NavigationStore navigationStore)
        {
            
            CurrencyNames = new ObservableCollection<string>() { "bitcoin", "dogecoin", "litecoin" };
            getMarketData();
            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });
        }

        private async void getMarketData()
        {
            MarketGraph marketGraph = await api.GetMarketData("bitcoin", "eur", 91);
            List<string> dates = new();
            List<double> prices = new();
            foreach (var price in marketGraph.prices)
            {
                prices.Add(price[1]);
                dates.Add(ToDateTime((long)price[0]).ToString());
            }
            CoinValue = new ChartValues<double>(prices);
            CoinDate = new ObservableCollection<string>(dates);
        }
        
        public static DateTime ToDateTime(long unixTime) {  
            return new DateTime(1970, 1, 1).Add(TimeSpan.FromMilliseconds(unixTime));  
        }  
    }
}
