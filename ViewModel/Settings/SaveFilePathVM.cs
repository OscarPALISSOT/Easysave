using System.Resources;
using EasySave.Command;
using EasySave.Model;
using EasySave.View.Ressources;

namespace EasySave.ViewModel.Settings
{
    class SaveFilePathVM : PropertyChangeEvent
    {
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        public RelayCommands ReturnPathSave { get; set; }
        public RelayCommands ChangeSavePath { get; set; }

        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        public string pathSave { get; set; }
        public string newPathSave { get; set; }

        public string title { get; set; }
        public string title2 { get; set; }
        public string change { get; set; }
        public string ReturnButton { get; set; }

        public SaveFilePathVM()
        {
            title = Resource1.SFP1;
            title2 = Resource1.SFP2;
            change = Resource1.SFP3;
            ReturnButton = Resource1.ReturnButton;

            ReturnPathSave = new RelayCommands(o =>
            {
                MenuPathVM menu = new MenuPathVM();
                nav.CurrentView = menu;
            });

            IniFile myIni = new IniFile();
            pathSave = myIni.Read("SavePath");

            ChangeSavePath = new RelayCommands(o =>
            {
                Commands.ModifySaveInfoPath(newPathSave);

                MenuPathVM menu = new MenuPathVM();
                nav.CurrentView = menu;
            });
        }
    }
}