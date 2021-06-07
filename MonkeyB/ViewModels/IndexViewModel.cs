using MonkeyB.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;

namespace MonkeyB.ViewModels
{
    class IndexViewModel : BaseViewModel
    {
        public ICommand DashBoardCommand { get; set; }
        
        

        public IndexViewModel(NavigationStore navigationStore)
        {
            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });
            GetIndexData();
        }
        
        public static readonly Uri BaseUri = new Uri("https://api.coingecko.com/api/v3/coins/bitcoin/market_chart/range?vs_currency=eur&from=1392577232&to=1422577232");

        
        public MarketGraph marketmodel = new MarketGraph();
        private void GetIndexData()
        {

        }
    }
}
