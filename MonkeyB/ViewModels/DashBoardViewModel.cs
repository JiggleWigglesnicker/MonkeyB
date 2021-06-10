using MonkeyB.Commands;
using MonkeyB.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;

namespace MonkeyB.ViewModels
{
    class DashBoardViewModel : BaseViewModel
    {
        public ICommand IndexCommand { get; set; }
        public ICommand BuySellCommand { get; set; }
        public ICommand NewsCommand { get; set; }
        public ICommand AchievementCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ObservableCollection<RSSModel> RSSList { get; set; }

        //private string rSSTitle;
        //public string RSSTitle
        //{
        //    get => rSSTitle;

        //    set
        //    {
        //        rSSTitle = value;
        //        OnPropertyChanged("RSSTitle");
        //    }
        //}

        //private string rSSContent;
        //public string RSSContent
        //{
        //    get => rSSContent;

        //    set
        //    {
        //        rSSContent = value;
        //        OnPropertyChanged("RSSContent");
        //    }
        //}



        public DashBoardViewModel(NavigationStore navigationStore)
        {
            IndexCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new IndexViewModel(navigationStore);
            });

            BuySellCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new BuySellViewModel(navigationStore);
            });

            NewsCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new NewsViewModel(navigationStore);
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

        public async void ReadRSSNodes()
        {

            await Task.Run(async () =>
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
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        RSSList.Add(new RSSModel(subject, summary));
                    }));

                }

            });


        }




    }
}
