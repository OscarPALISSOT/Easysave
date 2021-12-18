using System.Collections.Generic;
using System.Resources;
using EasySave.Command;
using EasySave.View.Ressources;

namespace EasySave.ViewModel.Settings
{
    class EncryptionKeyViewVM
    {
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        public RelayCommands ButtonReturn { get; set; }
        public RelayCommands AddNewExtention { get; set; }
        public RelayCommands DeleteExtention { get; set; }

        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        public List<Extention> extentionList { get; set; }

        public string extention { get; set; }
        public string title { get; set; }
        public string Labelnew { get; set; }
        public string addExt { get; set; }
        public string delExt { get; set; }
        public string ReturnButton { get; set; }

        public EncryptionKeyViewVM()
        {
            title = Resource1.EKV1;
            Labelnew = Resource1.EKV2;
            addExt = Resource1.EKV3;
            delExt = Resource1.EKV4;
            ReturnButton = Resource1.ReturnButton;

            ButtonReturn = new RelayCommands(o =>
            {
                MenuEncryptionViewVM menu = new MenuEncryptionViewVM();

                nav.CurrentView = menu;
            });

            extentionList = new List<Extention>();
            foreach (var ext in Commands.GetAllExtensionToCrypt())
            {
                Extention ex = new Extention(ext);
                extentionList.Add(ex);
            }

            AddNewExtention = new RelayCommands(o =>
            {
                MenuEncryptionViewVM menu = new MenuEncryptionViewVM();

                Commands.AddExtensionToCrypt(extention);
                nav.CurrentView = menu;
            });

            DeleteExtention = new RelayCommands(o =>
            {
                List<Extention> selectedExtention = new List<Extention>();
                foreach (Extention extention in extentionList)
                {
                    if (extention.Selected)
                    {
                        selectedExtention.Add(extention);
                    }
                }
                MenuEncryptionViewVM menu = new MenuEncryptionViewVM();

                Commands.DeleteExtensionToCrypt(selectedExtention);
                nav.CurrentView = menu;
            });
        }
    }

    public class Extention
    {
        public string Ext { get; set; }

        public bool Selected { get; set; }

        public Extention(string ext)
        {
            Ext = ext; 
        }
    }
}