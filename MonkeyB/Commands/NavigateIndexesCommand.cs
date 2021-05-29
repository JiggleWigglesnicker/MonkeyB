using System;
using MonkeyB.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Commands
{
    class NavigateIndexesCommand : BaseCommand
    {
        private NavigationStore navigationStore;

        public NavigateIndexesCommand(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            navigationStore.SelectedViewModel = new IndexViewModel(navigationStore);
        }
    }
}
