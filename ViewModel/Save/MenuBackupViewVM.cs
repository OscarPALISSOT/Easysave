using System.Resources;
using EasySave.Command;
using EasySave.View.Ressources;

namespace EasySave.ViewModel.Save
{
    class MenuBackupViewVM
    {
        // Used for navigation between views
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        // Declaration of commands that will be binded in the view (in buttons)
        public RelayCommands ReturnCommand { get; set; }
        public RelayCommands ExecuteBackupCommand { get; set; }
        public RelayCommands CreateBackupCommand { get; set; }
        public RelayCommands ModifyBackupCommand { get; set; }
        public RelayCommands DeleteBackupCommand { get; set; }

        // String for traduction, that will be binded in the view
        public string title { get; set; }
        public string ReturnButton { get; set; }
        public string button1 { get; set; }
        public string button2 { get; set; }
        public string button3 { get; set; }
        public string button4 { get; set; }

        public MenuBackupViewVM()
        {
            // Assignment of values for traduction
            title = Resource1.MB1;
            button1 = Resource1.MB2;
            button2 = Resource1.MB3;
            button3 = Resource1.MB4;
            button4 = Resource1.MB5;
            ReturnButton = Resource1.ReturnButton;

            // Command for return button
            ReturnCommand = new RelayCommands(o =>
            {
                HomeVM home = new HomeVM();
                nav.CurrentView = home;
            });

            // Command to change view to execute backup view
            ExecuteBackupCommand = new RelayCommands(o =>
            {
                ExecuteBackupViewVM execute = new ExecuteBackupViewVM();
                nav.CurrentView = execute;
            });

            // Command to change view to create backup view
            CreateBackupCommand = new RelayCommands(o =>
            {
                CreateBackupViewVM create = new CreateBackupViewVM();
                nav.CurrentView = create;
            });

            // Command to change view to modify backup view
            ModifyBackupCommand = new RelayCommands(o =>
            {
                ModifyBackupViewVM modify = new ModifyBackupViewVM();
                nav.CurrentView = modify;
            });

            // Command to change view to delete backup view
            DeleteBackupCommand = new RelayCommands(o =>
            {
                DeleteBackupViewVM delete = new DeleteBackupViewVM();
                nav.CurrentView = delete;
            });
        }
    }
}