using MonkeyB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyB.Commands
{
    class NavigateAchievementCommand : BaseCommand
    {
        private NavigationStore navigationStore;

        public NavigateAchievementCommand(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            navigationStore.SelectedViewModel = new AchievementViewModel(navigationStore);
        }
    }
}
