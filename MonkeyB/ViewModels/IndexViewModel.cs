using MonkeyB.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts;

namespace MonkeyB.ViewModels
{
    class IndexViewModel : BaseViewModel
    {
        public ICommand DashBoardCommand { get; set; }
        
        public ChartValues<float> CoinValue { get; set; }
        public DateTime[] CoinDate { get; set; }

        public async Task Load()
        {
            // IEnumerable<Market>
        }

        // public ObservableCollection<string> CoinValue { get; }

        public IndexViewModel(NavigationStore navigationStore)
        {
            DashBoardCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
            });
        }
    }
}
