using EasySave.View.Ressources;
using System.Resources;
using EasySave.Model;
using EasySave.Command;

namespace EasySave.ViewModel.Save
{
    class ModifyBackupChosenViewVM
    {
        // Used for navigation between views
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        // Declaration of commands that will be binded in the view (in buttons)
        public RelayCommands modifyCommand { get; set; }
        public RelayCommands returnButton { get; set; }

        // String for traduction, that will be binded in the view
        public string title1 { get; set; }
        public string title2 { get; set; }
        public string title3 { get; set; }
        public string title4 { get; set; }
        public string ReturnButton { get; set; }
        public string modify { get; set; }

        // List of backups 
        public SaveWork saveWork { get; set; }

        public ModifyBackupChosenViewVM()
        {
            // Assignment of values for traduction
            saveWork = ModifyBackupViewVM.GetThisSave();
            title1 = Resource1.CBV1;
            title2 = Resource1.CBV2;
            title3 = Resource1.CBV3;
            title4 = Resource1.CBV4;
            ReturnButton = Resource1.ReturnButton;
            modify = Resource1.MB4;

            // Command for modify button
            modifyCommand = new RelayCommands(o =>
            {
                ModifyBackupViewVM menu = new ModifyBackupViewVM();

                CommandsBackup.CreateBackup(saveWork.Name, saveWork.Info.FileSource, saveWork.Info.FileTarget, saveWork.Info.Full);
                nav.CurrentView = menu;
            });

            // Command for return button
            returnButton = new RelayCommands(o =>
            {
                ModifyBackupViewVM menu = new ModifyBackupViewVM();
                CommandsBackup.CreateBackup(saveWork.Name, saveWork.Info.FileSource, saveWork.Info.FileTarget, saveWork.Info.Full);
                nav.CurrentView = menu;
            });
        }
    }
}
