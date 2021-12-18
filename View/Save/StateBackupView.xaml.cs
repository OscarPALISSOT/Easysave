using EasySave.Command;
using EasySave.Model;
using EasySave.View.Ressources;
using EasySave.ViewModel;
using System.Collections.Generic;
using System.Resources;
using System.Threading;
using System.Windows.Controls;

namespace EasySave.View.Save
{
    /// <summary>
    /// Logique d'interaction pour StateBackupView.xaml
    /// </summary>
    public partial class StateBackupView : UserControl
    {
        private readonly MainWindowsVM nav = MainWindowsVM.GetThis();

        public RelayCommands ReturnCommand { get; set; }
        public RelayCommands PauseCommand { get; set; }

        public List<SaveWork> nameList { get; set; }
        public List<DataList> dataList { get; set; }

        public ResourceManager manager = new ResourceManager(typeof(Resource1));

        public string ReturnButton { get; set; }
        
        public StateBackupView()
        {
            InitializeComponent();

            DataContext = this;
            ReturnButton = Resource1.ReturnButton;

            nameList = CommandsBackup.GetAllBackups();
            dataList = new List<DataList>();

            dataList = new List<DataList>();
            List<SaveWork> backups = CommandsBackup.GetAllBackups();
            foreach (var backup in backups)
            {
                DataList data = new DataList(backup);
                dataList.Add(data);
            }
            ReturnCommand = new RelayCommands(o =>
            {
                HomeVM home = new HomeVM();
                nav.CurrentView = home;
            });

            PauseCommand = new RelayCommands(o =>
            {
                List<SaveWork> selectedWorks = new List<SaveWork>();
                foreach (SaveWork save in nameList)
                {
                    if (save.selected)
                    {
                        selectedWorks.Add(save);
                    }
                }
                foreach (var selectedWork in selectedWorks)
                {
                    if (selectedWork.Play)
                    {
                        selectedWork.Play = false;
                    }
                    else
                    {
                        selectedWork.Play = true;
                    }
                }
            });

            //Thread Trefresh = new Thread(new ThreadStart(
            //    () =>
            //    {
            //        while (true)
            //        {
            //            this.Dispatcher.Invoke(() =>
            //            {
            //                dataList = new List<DataList>();
            //                List<SaveWork> backups = CommandsBackup.GetAllBackups();
            //                foreach (var backup in backups)
            //                {
            //                    DataList data = new DataList(backup);
            //                    dataList.Add(data);
            //                    dataGrid.ItemsSource = null;
            //                    dataGrid.ItemsSource = dataList;
            //                }
            //                Thread.Sleep(5000);
            //            });
                        
            //        }
            //    }
            //    )
            //);
            //Trefresh.Start();
        }

        public class DataList
        {
            public string Name { get; set; }
            public bool Selected { get; set; }
            public int State { get; set; }
            public double Progression { get; set; }
            public string FileTarget { get; set; }
            public string FileSource { get; set; }
            public bool Full { get; set; }

            public DataList(SaveWork saveWork)
            {
                Name = saveWork.Name;
                Selected = saveWork.selected;
                State = saveWork.State.State;
                Progression = saveWork.State.Progression;
                FileSource = saveWork.Info.FileSource;
                FileTarget = saveWork.Info.FileTarget;
                Full = saveWork.Info.Full;
            }
        }

    }
}
