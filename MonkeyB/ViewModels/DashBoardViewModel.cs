using System;
using System.Collections.ObjectModel;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using MonkeyB.Commands;
using MonkeyB.Database;
using MonkeyB.Models;

namespace MonkeyB.ViewModels
{
    class DashBoardViewModel : BaseViewModel
    {
        /// <summary>
        ///     Sets all of the actions of the buttons in the view and displays a RSS feed.
        /// </summary>
        /// <param name="navigationStore"></param>
        public DashBoardViewModel(NavigationStore navigationStore)
        {
            DataBaseAccess.InitializeAchievements();

            IndexCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new IndexViewModel(navigationStore);
            });

            BuySellCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new BuySellViewModel(navigationStore);
            });

            WalletCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new WalletViewModel(navigationStore);
            });

            OrderCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new OrderViewModel(navigationStore);
            });

            AchievementCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new AchievementViewModel(navigationStore);
            });

            SettingCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new SettingViewModel(navigationStore);
            });

            RSSList = new ObservableCollection<RSSModel>();
            ReadRSSNodes();
        }

        public ICommand IndexCommand { get; set; }
        public ICommand BuySellCommand { get; set; }
        public ICommand WalletCommand { get; set; }
        public ICommand AchievementCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ICommand OrderCommand { get; set; }

        public ObservableCollection<RSSModel> RSSList { get; set; }

        /// <summary>
        ///     Reads the XML nodes in the RSS feed and displays it in the view.
        /// </summary>
        public async void ReadRSSNodes()
        {
            await Task.Run(() =>
            {
                try
                {
                    string url = "https://www.nu.nl/rss/Economie";
                    XmlReader reader = XmlReader.Create(url);
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();
                    foreach (SyndicationItem item in feed.Items)
                    {
                        String subject = item.Title.Text;
                        String summary = item.Summary.Text;
                        int index = summary.IndexOf("<");
                        if (index >= 0)
                            summary = summary.Substring(0, index);
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                            new ThreadStart(delegate { RSSList.Add(new RSSModel(subject, summary)); }));
                    }
                }
                catch (Exception e)
                {
                    _ = e.StackTrace;
                }
            });
        }
    }
}