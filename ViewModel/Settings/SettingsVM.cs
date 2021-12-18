using EasySave.Command;
using EasySave.View.Ressources;
using System.Resources;

namespace EasySave.ViewModel.Settings
{
    class SettingsVM : PropertyChangeEvent
    {
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();
        public RelayCommands MenuPathViewCommand { get; set; }
        public RelayCommands ReturnViewCommand { get; set; }
        public RelayCommands EncryptionViewCommand { get; set; }
        public RelayCommands LanguageViewCommand { get; set; }
        public RelayCommands LogExtentionCommand { get; set; }
        public RelayCommands BusinessSoftwareCommand { get; set; }
        public RelayCommands PriorityFilesCommand { get; set; }
        
        public MenuPathVM MenuVM { get; set; }
        public HomeVM ReturnVM { get; set; }
        public MenuEncryptionViewVM MenuEncryptionViewVM { get; set; }
        public LogExtentionViewVM LogExtentionViewVM { get; set; }
        public LanguageViewVM LanguageVM { get; set; }
        public BusinessSoftwareVM BusinessSoftwareVM { get; set; }

        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        public string path { get; set; }
        public string encryption { get; set; }
        public string logExtention { get; set; }
        public string businessSoftware { get; set; }
        public string language { get; set; }
        public string title { get; set; }
        public string priorityFiles { get; set; }
        public string ReturnButton { get; set; }

        public SettingsVM()
        {
            path = Resource1.SV3;
            encryption = Resource1.SV2;
            logExtention = Resource1.SV4;
            businessSoftware = Resource1.SV5;
            language = Resource1.SV6;
            title = Resource1.SV1;
            ReturnButton = Resource1.ReturnButton;
            priorityFiles = Resource1.SV7;

            MenuPathViewCommand = new RelayCommands(o =>
            {
                MenuPathVM menu = new MenuPathVM();
                nav.CurrentView = menu;
            });
            
            ReturnViewCommand = new RelayCommands(o =>
            {
                HomeVM home = new HomeVM();
                nav.CurrentView = home;
            });

            EncryptionViewCommand = new RelayCommands(o =>
            {
                MenuEncryptionViewVM encryption = new MenuEncryptionViewVM();
                nav.CurrentView = encryption;
            });

            LogExtentionCommand = new RelayCommands(o =>
            {
                LogExtentionViewVM extention = new LogExtentionViewVM();
                nav.CurrentView = extention;
            });

            LanguageViewCommand = new RelayCommands(o =>
            {
                LanguageViewVM language = new LanguageViewVM();
                nav.CurrentView = language;
            });

            BusinessSoftwareCommand = new RelayCommands(o =>
            {
                BusinessSoftwareVM business = new BusinessSoftwareVM();
                nav.CurrentView = business;
            });

            PriorityFilesCommand = new RelayCommands(o =>
            {
                PriorityFilesVM priorityFiles = new PriorityFilesVM();
                nav.CurrentView = priorityFiles;
            });
        }
    }
}