using System.Globalization;
using System.Resources;
using System.Threading;
using EasySave.Command;
using EasySave.View.Ressources;

namespace EasySave.ViewModel.Settings
{
    class LanguageViewVM : PropertyChangeEvent
    {
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        public RelayCommands ReturnLanguageCommand { get; set; }
        public RelayCommands Language_FR { get; set; }
        public RelayCommands Language_US { get; set; }

        public SettingsVM SettingsVM { get; set; }

        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        public string title { get; set; }
        public string ReturnButton { get; set; }

        public LanguageViewVM()
        {
            title = Resource1.SAL;
            ReturnButton = Resource1.ReturnButton;

            ReturnLanguageCommand = new RelayCommands(o =>
            {
                SettingsVM settings = new SettingsVM();

                nav.CurrentView = settings;
            });
            
            Language_FR = new RelayCommands(o =>
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");

                SettingsVM settings = new SettingsVM();
                nav.CurrentView = settings;
            });
            
            Language_US = new RelayCommands(o =>
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("us-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("us-US");

                SettingsVM settings = new SettingsVM();
                nav.CurrentView = settings;
            });
        }
    }
}