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
    class LoginViewModel : BaseViewModel
    {


        public ICommand LoginCommand { get; set; }

        private String username;
        public String Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        private String password;
        public String Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public LoginViewModel(NavigationStore navigationStore)
        {
            LoginCommand = new RelayCommand(o =>
            {
                if (login() == true)
                {
                    navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
                }
            });
        }

        public bool login()
        {
            LoginModel model = DataBaseAccess.RetrieveLogin(Username);
            if (Username == model.username && Password == model.password && Username != null && Password != null) return true;
            return false;
        }

    }
}
