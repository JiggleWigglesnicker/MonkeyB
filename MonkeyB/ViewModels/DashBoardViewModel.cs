using Apex.MVVM;
using MonkeyB.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MonkeyB.ViewModels
{
    class DashBoardViewModel
    {
        private Command indexCommand;
        public Command IndexCommand
        {
            get { return indexCommand; }
        }

        private Command buySellCommand;
        public Command BuySellCommand
        {
            get { return buySellCommand; }
        }

        private Command newsCommand;
        public Command NewsCommand
        {
            get { return newsCommand; }
        }

        private Command exitCommand;
        public Command ExitCommand
        {
            get { return exitCommand; }
        }

        public DashBoardViewModel(Window window)
        {
            indexCommand = new Command(() =>
            {
                var index = new IndexView();
                index.Show();
                window.Close();
            });


            buySellCommand = new Command(() =>
            {
                var buySell = new BuySellView();
                buySell.Show();
                window.Close();
            });

            newsCommand = new Command(() =>
            {
                var news = new NewsView();
                news.Show();
                window.Close();
            });

            exitCommand = new Command(() =>
            {
                window.Close();
            });

        }
    }
}
