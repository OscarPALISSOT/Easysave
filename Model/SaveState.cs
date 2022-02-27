using System;
using System.Collections.Generic;
using System.Text;
using EasySave.ViewModel;

namespace EasySave.Model
{
    public class SaveState: PropertyChangeEvent
    {
        public string Name { get; set; }
        public int State { get; set; }
        
        public long TotalFileToCopy { get; set; }
        public long TotalDirectorySize { get; set; }
        public long TotalRemainingSize { get; set; }
        public long FileSize { get; set; }
        public long NbFilesLeftToDo { get; set; }
        
        public int Progression { get; set; }
        
        public int progression
        {
            get
            {
                return Progression;
            }
            set
            {
                if(Progression != value)
                {
                    Progression = value;
                    OnPropertyChanged("progression");
                }
            }
           
        }

    }
}
