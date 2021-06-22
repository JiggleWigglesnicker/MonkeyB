using LiveCharts;
using LiveCharts.Wpf;
using MonkeyB.Commands;
using MonkeyB.Database;
using MonkeyB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonkeyB.ViewModels
{
    class WalletViewModel : BaseViewModel
    {
        public ICommand DashBoardCommand { get; set; }

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
