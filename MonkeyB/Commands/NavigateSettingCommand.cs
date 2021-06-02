using MonkeyB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Commands
{
    class NavigateSettingCommand : BaseCommand
    {
        private NavigationStore navigationStore;

        public NavigateSettingCommand(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            navigationStore.SelectedViewModel = new SettingViewModel(navigationStore);
        }
    }
}
