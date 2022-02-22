using EasySave.Command;
using EasySave.Model;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
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
        }
        
        // select the backup
        void SelectedBackup(object sender, SelectionChangedEventArgs args)
        {
            var item = (ListView)sender;
            var save = (SaveWork) item.SelectedItem;
            if (save.Selected)
            { 
                save.Selected = false;
            }
            else
            {
                save.Selected = true;
            }

        }
    }
}