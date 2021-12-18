using EasySave.Command;
using EasySave.View.Ressources;
using EasySave.ViewModel.Save;
using EasySave.ViewModel.Settings;
using System.Resources;

namespace EasySave.ViewModel
{
    class MainWindowsVM : PropertyChangeEvent
    {
        // Declaration of commands that will be binded in the view (in buttons)
        public RelayCommands HomeViewCommand { get; set; }
        public RelayCommands MenuBackupCommand { get; set; }
        public RelayCommands SettingsViewCommand { get; set; }
        public RelayCommands ShutDown { get; set; }
        public RelayCommands StateViewCommand { get; set; }

        // Used for navigation between views
        public static MainWindowsVM nav;

        // Used for traduction 
        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        // String for traduction, that will be binded in the view
        public string Save { get; set; }
        public string home { get; set; }
        public string backup { get; set; }
        public string state { get; set; }
        public string settings { get; set; }


        public static MainWindowsVM GetThis()
        {
            return nav;
        }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public HomeVM HomeVM { get; set; }
        public SettingsVM SettingsVM { get; set; }
        public MenuBackupViewVM MenuBackupViewVM { get; set; }
        public StateFilePathVM StateFilePathVM { get; set; }
        public ModifyBackupChosenViewVM ModifyBackupChosenVM { get; set; }
        public MainWindowsVM()
        {
            // Used for navigation
            nav = this;
            HomeVM = new HomeVM();
            MenuBackupViewVM = new MenuBackupViewVM();
            SettingsVM = new SettingsVM();
            CurrentView = HomeVM;
            Save = Resource1.BS1;

            // Assignment of values for traduction
            home = Resource1.MW1;
            backup = Resource1.MW2;
            state = Resource1.MW3;
            settings = Resource1.MW4;

            // Command to change view to home backup view
            HomeViewCommand = new RelayCommands(o =>
            {
                CurrentView = HomeVM;
            });

            // Command to change view to settings backup view
            SettingsViewCommand = new RelayCommands(o =>
            {
                CurrentView = SettingsVM;
            });

            // Command to change view to menu backup view
            MenuBackupCommand = new RelayCommands(o =>
            {
                CurrentView = MenuBackupViewVM;
            });

            // Command to change view to state backup view
            StateViewCommand = new RelayCommands(o =>
            {
                StateBackupViewVM state = new StateBackupViewVM();
                CurrentView = state;
            });

            // Command to shutdown app
            ShutDown = new RelayCommands(o =>
            {
                Commands.ShutDown();
            });
        }
    }
}
