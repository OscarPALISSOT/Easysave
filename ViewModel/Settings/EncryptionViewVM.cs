using EasySave.Command;
using EasySave.View.Ressources;
using System.Resources;

namespace EasySave.ViewModel.Settings
{
    class EncryptionViewVM
    {
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        public RelayCommands ReturnEncryptionCommand { get; set; }
        public RelayCommands ModifyEncryptionKey { get; set; }
        public RelayCommands GenerateRandomKey { get; set; }

        public MenuEncryptionViewVM MenuEncryptionViewVM { get; set; }

        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        public string title { get; set; }
        public string title2 { get; set; }
        public string generate { get; set; }
        public string change { get; set; }

        public string key { get; set; }
        public string newkey { get; set; }
        public string ReturnButton { get; set; }

        public EncryptionViewVM()
        {
            title = Resource1.EV1;
            title2 = Resource1.EV2;
            generate = Resource1.EV3;
            change = Resource1.EV4;
            ReturnButton = Resource1.ReturnButton;

            key = Commands.GetEncryptionKey();

            ReturnEncryptionCommand = new RelayCommands(o =>
            {
                MenuEncryptionViewVM menu = new MenuEncryptionViewVM();

                nav.CurrentView = menu;
            });
            
            GenerateRandomKey = new RelayCommands(o =>
            {
                MenuEncryptionViewVM menu = new MenuEncryptionViewVM();

                Commands.Generate64BitKey();
                nav.CurrentView = menu;
            });
            
            ModifyEncryptionKey = new RelayCommands(o =>
            {
                MenuEncryptionViewVM menu = new MenuEncryptionViewVM();

                if (newkey != "")
                {
                    Commands.ModifyEncryptionKey(newkey);
                }
                nav.CurrentView = menu;
            });

            
        }
    }
}