using System.Collections.Generic;
using System.Resources;
using EasySave.Command;
using EasySave.Model;
using EasySave.View.Ressources;

namespace EasySave.ViewModel.Save
{
    class DeleteBackupViewVM
    {
        // Used for navigation between views
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        // Used for traduction 
        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        // Declaration of commands that will be binded in the view (in buttons)
        public RelayCommands ReturnDelete{ get; set; }
        public RelayCommands DeleteBackup { get; set; }

        // List of backups 
        public List<SaveWork> nameList { get; set; }

        // String for traduction, that will be binded in the view
        public string title { get; set; }
        public string title2 { get; set; }
        public string delete { get; set; }
        public string ReturnButton { get; set; }

        public DeleteBackupViewVM()
        {
            nameList = new List<SaveWork>();

            // Assignment of values for traduction
            title = Resource1.DBV1;
            title2 = Resource1.DBV2;
            delete = Resource1.DBV3;
            ReturnButton = Resource1.ReturnButton;

            // Command for return button
            ReturnDelete = new RelayCommands(o =>
            {
                MenuBackupViewVM menu = new MenuBackupViewVM();
                nav.CurrentView = menu;
            });

            // Get all backups
            nameList = CommandsBackup.GetAllBackups();

            // Command for delete button
            DeleteBackup = new RelayCommands(o =>
            {
                List<SaveWork> selectedWork = new List<SaveWork>();
                foreach (SaveWork save in nameList)
                {
                    if (save.selected)
                    {
                        selectedWork.Add(save);
                    }
                }

                // Delete backup function
                CommandsBackup.DeleteBackup(selectedWork);

                // Change view when done
                MenuBackupViewVM menu = new MenuBackupViewVM();
                nav.CurrentView = menu;
            });
        }
    }
}