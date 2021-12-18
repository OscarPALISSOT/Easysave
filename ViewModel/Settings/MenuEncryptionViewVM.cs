using System.Resources;
using EasySave.Command;
using EasySave.View.Ressources;

namespace EasySave.ViewModel.Settings
{
    class MenuEncryptionViewVM
    {
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        public RelayCommands EncryptionViewCommand { get; set; }
        public RelayCommands ButtonReturn { get; set; }
        public RelayCommands EncryptionKeyView { get; set; }

        public EncryptionViewVM EncryptionViewVM { get; set; }
        public HomeVM HomeVM { get; set; }
        public EncryptionViewVM EncryptionVM { get; set; }
        public EncryptionKeyViewVM EncryptionKeyVM { get; set; }

        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        public string encryptionKey {get; set;}
        public string encryptionExtention {get; set;}
        public string ReturnButton { get; set;}

        public MenuEncryptionViewVM()
        {
            encryptionKey = Resource1.MEV1;
            encryptionExtention = Resource1.MEV2;
            ReturnButton = Resource1.ReturnButton;

            EncryptionViewCommand = new RelayCommands(o =>
            {
                EncryptionVM = new EncryptionViewVM();
                nav.CurrentView = EncryptionVM;
            });

            ButtonReturn = new RelayCommands(o =>
            {
                SettingsVM settings = new SettingsVM();
                nav.CurrentView = settings;
            });

            EncryptionKeyView = new RelayCommands(o =>
            {
                EncryptionKeyVM = new EncryptionKeyViewVM();
                nav.CurrentView = EncryptionKeyVM;
            });
        }
    }
}
