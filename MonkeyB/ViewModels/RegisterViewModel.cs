using System.Windows;
using System.Windows.Input;
using MonkeyB.Commands;
using MonkeyB.Database;

namespace MonkeyB.ViewModels
{
    internal class RegisterViewModel : BaseViewModel
    {
        private string password;

        private string username;

        /// <summary>
        ///     Constructor for the RegisterViewModel
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

        public ICommand ToLoginCommand { get; set; }

        public ICommand RegisterCommand { get; set; }

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
    }
}