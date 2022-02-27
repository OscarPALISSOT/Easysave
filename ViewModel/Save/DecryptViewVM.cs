using EasySave.Command;
using EasySave.Model;
using EasySave.View.Ressources;
using System.Collections.Generic;
using System.Resources;

namespace EasySave.ViewModel.Save
{
    class DecryptViewVM
    {
        // Used for navigation between views
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        // Declaration of commands that will be binded in the view (in buttons)
        public RelayCommands ReturnButton { get; set; }
        public RelayCommands Decrypt { get; set; }

        // List of backups 
        public List<SaveWork> nameList { get; set; }

        // String for traduction, that will be binded in the view
        public string returnButton { get; set; }
        public string decrypt{ get; set; }

        public DecryptViewVM()
        {
            // Assignment of values for traduction
            returnButton = Resource1.ReturnButton;
            decrypt = Resource1.Decrypt;

            // Get all backups
            nameList = CommandsBackup.GetAllBackups();

            // Command for return button
            ReturnButton = new RelayCommands(o =>
            {
                HomeVM home = new HomeVM();
                nav.CurrentView = home;
            });

            // Command for decrypting backup button
            Decrypt = new RelayCommands(o =>
            {
                List<SaveWork> selectedWork = new List<SaveWork>();
                foreach (SaveWork save in nameList)
                {
                    if (save.Selected)
                    {
                        selectedWork.Add(save);
                    }
                }
                // Decrypt backup function
                CommandsBackup.DecryptBackup(selectedWork);

                // Change view when done
                HomeVM home = new HomeVM();
                nav.CurrentView = home;
            });
        }
    }
}