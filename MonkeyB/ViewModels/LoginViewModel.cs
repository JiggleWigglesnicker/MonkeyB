using MonkeyB.Commands;
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

        public ICommand loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                return loginCommand;
            }
        }

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
            loginCommand = new NavigateDashBoardCommand(navigationStore);
        }

    }
}
