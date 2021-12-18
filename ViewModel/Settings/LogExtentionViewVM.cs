using EasySave.Command;
using EasySave.Model;
using EasySave.View.Ressources;
using System.Resources;

namespace EasySave.ViewModel.Settings
{
    class LogExtentionViewVM
    {
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        public RelayCommands ButtonReturn { get; set; }
        public RelayCommands ChangeLogExtention { get; set; }

        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        public bool selectedJSON { get; set; }
        public bool selectedXML { get; set; }
        public string format { get; set; }

        public string title { get; set; }
        public string title2 { get; set; }
        public string change { get; set; }
        public string ReturnButton { get; set; }

        public LogExtentionViewVM()
        {
            title = Resource1.LE1;
            title2 = Resource1.LE2;
            change = Resource1.LE3;
            ReturnButton = Resource1.ReturnButton;

            ButtonReturn = new RelayCommands(o =>
            {
                SettingsVM settings = new SettingsVM();

                nav.CurrentView = settings;
            });

            IniFile myIni = new IniFile();
            format = myIni.Read("LogFormat");

            ChangeLogExtention = new RelayCommands(o =>
             {
                 SettingsVM settings = new SettingsVM();

                 int input = 0;
                 if (selectedJSON)
                 {
                     input = 1;
                 }
                 else if (selectedXML)
                 {
                     input = 2;
                 }
                 else
                 {
                     nav.CurrentView = settings;
                 }
                 Commands.ChangeLogFormat(input);

                 nav.CurrentView = settings;
             });
        }
    };
}