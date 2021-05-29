using System;
using MonkeyB.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Commands 
{
    class NavigateBuySellCommand : BaseCommand
    {
        private NavigationStore navigationStore;

        public NavigateBuySellCommand(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            navigationStore.SelectedViewModel = new BuySellViewModel(navigationStore);
        }
    }
}
