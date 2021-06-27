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
        
        public ICommand RegisterCommand { get; set; }

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

        /// <summary>
        /// Sets the actions of the login button and the Register user button. 
        /// And also checks if a login is valid when the button is pressed.
        /// </summary>
        /// <param name="navigationStore"> Stores the currently selected viewmodel which is used to display a view</param>
        public LoginViewModel(NavigationStore navigationStore)
        {
            LoginCommand = new RelayCommand(o =>
            {
                if (login() == true)
                {
                    navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
                }
            });
            
            RegisterCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new RegisterViewModel(navigationStore);
            });
        }

        /// <summary>
        /// Checks if login credentails are  valid by looking it up in the database. 
        /// </summary>
        /// <returns> returns a bool which is true when login is successfull and false when not</returns>
        public bool login()
        {
            LoginModel model = DataBaseAccess.RetrieveLogin(Username);
            if (Username == model.username && Password == model.password && Username != null && Password != null) return true;
            return false;

        }

    }
}
