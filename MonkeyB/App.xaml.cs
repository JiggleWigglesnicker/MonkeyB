using System.Globalization;
using System.Threading;
using System.Windows;
using MonkeyB.Database;
using MonkeyB.ViewModels;

namespace MonkeyB
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static int UserID { get; set; }

        /// <summary>
        ///     Sets the culture, navigationstore, database initialization and startview of the application at startup
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

            NavigationStore navigationStore = new NavigationStore();

            navigationStore.SelectedViewModel = new LoginViewModel(navigationStore);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };

            MainWindow.Show();

            base.OnStartup(e);

            DataBaseAccess.InitializeDatabase();
        }
    }
}