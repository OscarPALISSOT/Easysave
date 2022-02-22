using DevExpress.Data.Browsing;
using EasySave.Command;
using EasySave.Model;
using EasySave.View.Ressources;
using System.Collections.Generic;
using System.Windows.Threading;

namespace EasySave.ViewModel.Save
{
    class StateBackupViewVM
    {
            private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

            public RelayCommands ReturnCommand { get; set; }
            public RelayCommands PauseCommand { get; set; }

            public List<SaveWork> NameList { get; set; }
            public string ReturnButton { get; set; }

            public StateBackupViewVM()
            {
                ReturnButton = Resource1.ReturnButton;

                
                NameList = CommandsBackup.GetAllBackups();
                
                List<SaveWork> backups = CommandsBackup.GetAllBackups();
                ReturnCommand = new RelayCommands(o =>
                {
                    HomeVM home = new HomeVM();
                    nav.CurrentView = home;
                });

                PauseCommand = new RelayCommands(o =>
                {
                    List<SaveWork> selectedWorks = new List<SaveWork>();
                    foreach (SaveWork save in NameList)
                    {
                        if (save.Selected)
                        {
                            selectedWorks.Add(save);
                        }
                    }
                });
            }
    }
}