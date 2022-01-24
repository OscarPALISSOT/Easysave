using System.Resources;
using EasySave.Command;
using EasySave.View.Ressources;

namespace EasySave.ViewModel.Save
{
    class CreateBackupViewVM
    {
        // Used for navigation between views
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        // Declaration of commands that will be binded in the view (in buttons)
        public RelayCommands ReturnButtonCommand { get; set; }
        public RelayCommands CreateBackup { get; set; }

        // Variable collected when backups are created
        public string nameBackup { get; set; }
        public string fileSource { get; set; }
        public string fileTarget { get; set; }
        public bool type { get; set; }

        // String for traduction, that will be binded in the view
        public string title1 { get; set; }
        public string title2 { get; set; }
        public string title3 { get; set; }
        public string title4 { get; set; }
        public string ReturnButton { get; set; }
        public string create { get; set; }


        public CreateBackupViewVM()
        {
            // Assignment of values for traduction
            title1 = Resource1.CBV1;
            title2 = Resource1.CBV2;
            title3 = Resource1.CBV3;
            title4 = Resource1.CBV4;
            create = Resource1.CBV5;
            ReturnButton = Resource1.ReturnButton;

            // Command for return button
            ReturnButtonCommand = new RelayCommands(o =>
            {
                MenuBackupViewVM menu = new MenuBackupViewVM();
                nav.CurrentView = menu;
            });

            // Command for create backup button
            CreateBackup = new RelayCommands(o =>
            {
                // Create backup function
                CommandsBackup.CreateBackup(nameBackup, fileSource, fileTarget, type);

                // Change view when done
                MenuBackupViewVM menu = new MenuBackupViewVM();
                nav.CurrentView = menu;
            });
        }
        ~CreateBackupViewVM() {}
    }
}