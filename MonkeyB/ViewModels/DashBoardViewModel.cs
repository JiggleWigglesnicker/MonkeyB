using MonkeyB.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonkeyB.ViewModels
{
    class DashBoardViewModel : BaseViewModel
    {
        public ICommand IndexCommand { get; set; }
        public ICommand BuySellCommand { get; set; }
        public ICommand NewsCommand { get; set; }

        public DashBoardViewModel(NavigationStore navigationStore)
        {
            IndexCommand = new NavigateIndexesCommand(navigationStore);
            BuySellCommand = new NavigateBuySellCommand(navigationStore);
            NewsCommand = new NavigateNewsCommand(navigationStore);
        }

    }
}
