using MonkeyB.Commands;
using System.Windows.Input;

namespace MonkeyB.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        public ICommand ToLoginCommand { get; set; }

        public RegisterViewModel(NavigationStore navigationStore)
        {
            ToLoginCommand = new RelayCommand(o =>
            {
                navigationStore.SelectedViewModel = new LoginViewModel(navigationStore);
            });


        }

    }
}