using MonkeyB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonkeyB.Commands
{
    class NavigateDashBoardCommand : BaseCommand
    {
        private NavigationStore navigationStore;

        public NavigateDashBoardCommand(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            navigationStore.SelectedViewModel = new DashBoardViewModel(navigationStore);
        }
    }
}
