using System;
using System.Collections.Generic;
using System.Text;
using EasySave.Command;
using EasySave.View.Ressources;
using System.Resources;

namespace EasySave.ViewModel.Settings
{
    class PriorityFilesVM
    {
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        public RelayCommands ButtonReturn { get; set; }
        public RelayCommands AddFile { get; set; }
        public RelayCommands DeleteFile { get; set; }

        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        public List<PriorityFile> fileList { get; set; }

        public string newFile { get; set; }

        public string ReturnButton { get; set; }
        public string Title { get; set; }
        public string Add { get; set; }
        public string Delete { get; set; }
        public string New { get; set; }


        public PriorityFilesVM()
        {
            ReturnButton = Resource1.ReturnButton;
            Title = Resource1.PF1;
            Add = Resource1.PF2;
            Delete = Resource1.PF3;
            New = Resource1.EKV2;

            ButtonReturn = new RelayCommands(o =>
            {
                SettingsVM settings = new SettingsVM();

                nav.CurrentView = settings;
            });

            fileList = new List<PriorityFile>();
            foreach (var file in Commands.GetAllPriorityFile())
            {
                PriorityFile fi = new PriorityFile(file);
                fileList.Add(fi);
            }

            AddFile = new RelayCommands(o =>
            {
                SettingsVM settings = new SettingsVM();

                Commands.AddPriorityFile(newFile);
                nav.CurrentView = settings;
            });

            DeleteFile = new RelayCommands(o =>
            {
                List<PriorityFile> selectedFile = new List<PriorityFile>();
                foreach (PriorityFile file in fileList)
                {
                    if (file.Selected)
                    {
                        selectedFile.Add(file);
                    }
                }
                SettingsVM settings = new SettingsVM();

                Commands.DeletePriorityFile(selectedFile);

                nav.CurrentView = settings;
            });
        }
    }

    public class PriorityFile
    {
        public string File { get; set; }

        public bool Selected { get; set; }

        public PriorityFile(string soft)
        {
            File = soft;
        }
    }
}
