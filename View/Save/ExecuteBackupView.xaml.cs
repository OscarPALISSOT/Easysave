using EasySave.Command;
using EasySave.Model;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;

namespace EasySave.View.Save
{
    /// <summary>
    /// Logique d'interaction pour ExecuteBackupView.xaml
    /// </summary>
    public partial class ExecuteBackupView : UserControl
    {
        //public List<SaveState> BackupsState { get; set; }

        public ExecuteBackupView()
        {
            InitializeComponent();

            Thread myThread;

            myThread = new Thread(new ThreadStart(ThreadLoop));

            myThread.Start();
        }

        public static void ThreadLoop()
        {
            // Tant que le thread n'est pas tué, on travaille
            while (Thread.CurrentThread.IsAlive)
            {
                List<SaveState> BackupsState = new List<SaveState>();

                BackupsState = CommandsBackup.GetBackUpsState();
            }
        }
    }
}