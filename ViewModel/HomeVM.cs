using EasySave.Command;
using EasySave.View.Ressources;
using EasySave.ViewModel.Save;
using System.Resources;

namespace EasySave.ViewModel
{
    class HomeVM : PropertyChangeEvent
    {
        // Used for navigation between views
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        // Used for traduction 
        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        // Declaration of commands that will be binded in the view (in buttons)
        public RelayCommands DecryptViewCommand { get; set; }
        public RelayCommands ExecuteBackupViewCommand { get; set; }
        public RelayCommands CreateBackupViewCommand { get; set; }

        // String for traduction, that will be binded in the view
        public string title { get; set; }
        public string shortcuts { get; set; }
        public string button1 { get; set; }
        public string button2 { get; set; }
        public string button3 { get; set; }

        public HomeVM()
        {
            // Assignment of values for traduction
            title = Resource1.HV1;
            shortcuts = Resource1.shortcuts;
            button1 = Resource1.MB2;
            button2 = Resource1.Decrypt;
            button3 = Resource1.MB3;

            // Command to change view to decrypt backup view
            DecryptViewCommand = new RelayCommands(o =>
            {
                DecryptViewVM decrypt = new DecryptViewVM();
                nav.CurrentView = decrypt;
            });

            // Command to change view to execute backup view
            ExecuteBackupViewCommand = new RelayCommands(o =>
            {
                ExecuteBackupViewVM execute = new ExecuteBackupViewVM();
                nav.CurrentView = execute;
            });

            // Command to change view to create backup view
            CreateBackupViewCommand = new RelayCommands(o =>
            {
                CreateBackupViewVM create = new CreateBackupViewVM();
                nav.CurrentView = create;
            });
        }
    }
}