using System.Resources;
using EasySave.Command;
using EasySave.Model;
using EasySave.View.Ressources;

namespace EasySave.ViewModel.Settings
{
    class StateFilePathVM
    {
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();
        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        // Commands
        public RelayCommands ReturnPathState { get; set; }
        public RelayCommands ChangeStatePath { get; set; }

        // String for commands
        public string pathState { get; set; }
        public string newPathState { get; set; }

        // String resource for traduction
        public string title { get; set; }
        public string title2 { get; set; }
        public string change { get; set; }
        public string ReturnButton { get; set; }

        public StateFilePathVM()
        {
            title = Resource1.STFP1;
            title2 = Resource1.STFP2;
            change = Resource1.STFP3;
            ReturnButton = Resource1.ReturnButton;

            // Read the IniFile for the view
            IniFile myIni = new IniFile();
            pathState = myIni.Read("SaveState");

            // Return button command
            ReturnPathState = new RelayCommands(o =>
            {
                MenuPathVM menu = new MenuPathVM();
                nav.CurrentView = menu;
            });

            ChangeStatePath = new RelayCommands(o =>
            {
                Commands.ModifySaveStatePath(newPathState);

                MenuPathVM menu = new MenuPathVM();
                nav.CurrentView = menu;
            });
        }
    }
}