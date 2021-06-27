using System.Windows;
using MonkeyB.Commands;
using System.Windows.Input;
using MonkeyB.Database;

namespace MonkeyB.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        public ICommand ToLoginCommand { get; set; }
        
        public ICommand RegisterCommand { get; set; }
        
        private string username;
        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        /// <summary>
        /// Constructor for the RegisterViewModel
        /// </summary>
        /// <param name="navigationStore">Stores the currently selected viewmodel which is used to display a view</param>
        public RegisterViewModel(NavigationStore navigationStore)
        {
            ToLoginCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new LoginViewModel(navigationStore);
            });
            RegisterCommand = new RelayCommand(o =>
            {
                //register user, if complete give message
                if (DataBaseAccess.RegisterUser(username, password))
                {
                    MessageBox.Show("User created!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                    navigationStore.SelectedViewModel = new LoginViewModel(navigationStore);
                    return;
                }
                //Give error when invalid input
                MessageBox.Show("Invalid input, try again!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            });

        }

    }
}