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

        public ICommand LoginCommand { get; set; }

        public LoginViewModel(NavigationStore navigationStore)
        {
            LoginCommand = new NavigateDashBoardCommand(navigationStore);
        }

    }
}
