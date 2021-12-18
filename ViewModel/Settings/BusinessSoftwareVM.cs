using System.Collections.Generic;
using System.Resources;
using EasySave.Command;
using EasySave.View.Ressources;

namespace EasySave.ViewModel.Settings
{
    class BusinessSoftwareVM
    {
        // Used for navigation between views
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        // Declaration of commands that will be binded in the view (in buttons)
        public RelayCommands ButtonReturn { get; set; }
        public RelayCommands AddSoft { get; set; }
        public RelayCommands DeleteSoft { get; set; }

        // Used for traduction 
        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        // List of software 
        public List<BusinessSoftware> softwareList { get; set; }

        public string newSoft { get; set; }

        // String for traduction, that will be binded in the view
        public string titleP1 { get; set; }
        public string titleP2 { get; set; }
        public string newSoftware { get; set; }
        public string add { get; set; }
        public string delete { get; set; }
        public string ReturnButton { get; set; }


        public BusinessSoftwareVM()
        {
            // Assignment of values for traduction
            titleP1 = Resource1.BS1;
            titleP2 = Resource1.BS2;
            newSoftware = Resource1.BS3;
            add = Resource1.BS4;
            delete = Resource1.BS5;
            ReturnButton = Resource1.ReturnButton;

            // Command for return button
            ButtonReturn = new RelayCommands(o =>
            {
                SettingsVM settings =  new SettingsVM();

                nav.CurrentView = settings;
            });

            softwareList = new List<BusinessSoftware>();
            foreach (var soft in Commands.GetAllBusinessSoftware())
            {
                BusinessSoftware so = new BusinessSoftware(soft);
                softwareList.Add(so);
            }

            // Command to add software button
            AddSoft = new RelayCommands(o =>
            {
                SettingsVM settings = new SettingsVM();

                Commands.AddBusinessSoftware(newSoft);
                nav.CurrentView = settings;
            });

            // Command for delete button
            DeleteSoft = new RelayCommands(o =>
            {
                List<BusinessSoftware> selectedSoft = new List<BusinessSoftware>();
                foreach (BusinessSoftware soft in softwareList)
                {
                    if (soft.Selected)
                    {
                        selectedSoft.Add(soft);
                    }
                }
                SettingsVM settings = new SettingsVM();

                // Delete software function
                Commands.DeleteBusinessSoftware(selectedSoft);

                // Change view when done
                nav.CurrentView = settings;
            });
        }
    }

    public class BusinessSoftware
    {
        public string Soft { get; set; }

        public bool Selected { get; set; }

        public BusinessSoftware(string soft)
        {
            Soft = soft;
        }
    }
}