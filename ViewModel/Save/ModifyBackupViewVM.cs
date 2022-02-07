using System.Resources;
using EasySave.Command;
using EasySave.View.Ressources;
using EasySave.Model;
using System.Collections.Generic;

namespace EasySave.ViewModel.Save
{
    class ModifyBackupViewVM
    {
        // Used for navigation between views
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        // Declaration of commands that will be binded in the view (in buttons)
        public RelayCommands ReturnModify { get; set; }
        public RelayCommands modifyCommand { get; set; }

        public ModifyBackupChosenViewVM ModifyBackupChosenVM { get; set; }

        // List of backups 
        public List<SaveWork> nameList { get; set; }

        // Get save
        public static SaveWork GetThisSave()
        {
            return saveWork;
        }

        public static SaveWork saveWork { get; set; }

        // String for traduction, that will be binded in the view
        public string title { get; set; }
        public string title2 { get; set; }
        public string ReturnButton { get; set; }
        public string modify{ get; set; }


        public ModifyBackupViewVM()
        {            
            nameList = new List<SaveWork>();

            // Get all backups
            nameList = CommandsBackup.GetAllBackups();

            // Assignment of values for traduction
            title = Resource1.MBV1;
            title2 = Resource1.MBV2;
            ReturnButton = Resource1.ReturnButton;
            modify = Resource1.MBV3;

            // Command for return button
            ReturnModify = new RelayCommands(o =>
            {
                MenuBackupViewVM menu = new MenuBackupViewVM();
                nav.CurrentView = menu;
            });

            // Command for modify backup button
            modifyCommand = new RelayCommands(o =>
            {
                ModifyBackupChosenVM = new ModifyBackupChosenViewVM();
                foreach (SaveWork save in nameList)
                {
                    if (save.Selected)
                    {
                        saveWork = save;
                    }
                }
                CommandsBackup.DeleteBackup(saveWork.Name);

                // Change view when done
                ModifyBackupChosenViewVM modify = new ModifyBackupChosenViewVM();
                nav.CurrentView = modify;
            });
        }
    }
}