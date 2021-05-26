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
    class LoginViewModel
    {
        public ApiHandler api = new ApiHandler();

        private Command loginCommand;
        public Command LoginCommand
        {
            get { return loginCommand; }
        }

        public LoginViewModel(Window window)
        {
            api.GetApiData();

            loginCommand = new Command(() =>
            {
                var dash = new DashboardView();
                dash.Show();
                window.Close();
            });

        }

    }
}
