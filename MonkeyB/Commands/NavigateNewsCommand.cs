using MonkeyB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Commands
{
    class NavigateNewsCommand : BaseCommand
    {
        private NavigationStore navigationStore;

        public NavigateNewsCommand(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            navigationStore.SelectedViewModel = new NewsViewModel(navigationStore);
        }
    }
}
