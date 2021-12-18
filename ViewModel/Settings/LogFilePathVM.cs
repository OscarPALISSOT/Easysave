using System.Resources;
using EasySave.Command;
using EasySave.Model;
using EasySave.View.Ressources;

namespace EasySave.ViewModel.Settings
{
    class LogFilePathVM
    {
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        public RelayCommands ReturnPathMenu { get; set; }
        public RelayCommands ChangePathLog { get; set; }

        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        public string path { get; set; }
        public string newPath { get; set; }

        public string title { get; set; }
        public string title2 { get; set; }
        public string change { get; set; }
        public string ReturnButton { get; set; }

        public LogFilePathVM()
        {
            title = Resource1.LFP1;
            title2 = Resource1.LFP2;
            change = Resource1.LFP3;
            ReturnButton = Resource1.ReturnButton;

            ReturnPathMenu = new RelayCommands(o =>
            {
                MenuPathVM menu = new MenuPathVM();
                nav.CurrentView = menu;
            });

            IniFile myIni = new IniFile();
            path = myIni.Read("LogsPath");

            ChangePathLog = new RelayCommands(o =>
            {
                Commands.ModifyLogsPath(newPath);

                MenuPathVM menu = new MenuPathVM();
                nav.CurrentView = menu;
            });
        }
    }
}