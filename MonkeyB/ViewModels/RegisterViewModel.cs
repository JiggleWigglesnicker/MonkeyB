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

        public RegisterViewModel(NavigationStore navigationStore)
        {
            ToLoginCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new LoginViewModel(navigationStore);
            });
            RegisterCommand = new RelayCommand(o =>
            {
                if (DataBaseAccess.RegisterUser(username, password))
                {
                    MessageBox.Show("User created!");
                    navigationStore.SelectedViewModel = new LoginViewModel(navigationStore);
                    return;
                }
                MessageBox.Show("Invalid input, try again!");
            });

        }

    }
}