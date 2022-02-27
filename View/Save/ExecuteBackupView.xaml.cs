using EasySave.Command;
using EasySave.Model;
using System.Collections.Generic;
using System.Windows.Controls;
using EasySave.View.Ressources;
using EasySave.ViewModel;
using EasySave.ViewModel.Save;
using SelectionChangedEventArgs = DevExpress.Data.SelectionChangedEventArgs;

namespace EasySave.View.Save
{
    /// <summary>
    /// Logique d'interaction pour ExecuteBackupView.xaml
    /// </summary>
    public partial class ExecuteBackupView : UserControl
    {
        
        public ExecuteBackupView()
        {
            InitializeComponent();
        }
        
        // select the backup
        private void SelectedBackup(object sender, System.Windows.Controls.SelectionChangedEventArgs selectionChangedEventArgs)
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