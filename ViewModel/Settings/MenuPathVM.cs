using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using EasySave.Command;
using EasySave.View.Ressources;

namespace EasySave.ViewModel.Settings
{
    class MenuPathVM
    {
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        public RelayCommands ReturnViewCommand { get; set; }
        public RelayCommands LogFileViewCommand { get; set; }
        public RelayCommands StateFileViewCommand { get; set; }
        public RelayCommands SaveFileViewCommand { get; set; }

        public SettingsVM SettingsVM { get; set; }
        public LogFilePathVM LogViewVM { get; set; }
        public StateFilePathVM StateViewVM { get; set; }
        public SaveFilePathVM SaveViewVM { get; set; }

        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        public string title { get; set; }
        public string log { get; set; }
        public string state { get; set; }
        public string save { get; set; }
        public string ReturnButton { get; set; }

        public MenuPathVM()
        {
            title = Resource1.MPV1;
            log = Resource1.MPV2;
            state = Resource1.MPV3;
            save = Resource1.MPV4;
            ReturnButton = Resource1.ReturnButton;

            ReturnViewCommand = new RelayCommands(o =>
            {
                SettingsVM settings = new SettingsVM();
                nav.CurrentView = settings;
            });

            LogFileViewCommand = new RelayCommands(o =>
            {
                LogViewVM = new LogFilePathVM();
                nav.CurrentView = LogViewVM;
            });

            StateFileViewCommand = new RelayCommands(o =>
            {
                StateViewVM = new StateFilePathVM();
                nav.CurrentView = StateViewVM;
            });

            SaveFileViewCommand = new RelayCommands(o =>
            {
                SaveViewVM = new SaveFilePathVM();
                nav.CurrentView = SaveViewVM;
            });
        }
    }
}