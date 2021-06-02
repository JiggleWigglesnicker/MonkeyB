using MonkeyB.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonkeyB.ViewModels
{
    class SettingViewModel : BaseViewModel
    {
        public ICommand DashBoardCommand { get; set; }

        public SettingViewModel(NavigationStore navigationStore)
        {
            DashBoardCommand = new NavigateDashBoardCommand(navigationStore);
        }
    }
}
