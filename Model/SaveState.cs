using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model
{
    public class SaveState
    {
        public string Name { get; set; }
        public int State { get; set; }
        public long TotalFileToCopy { get; set; }
        public long TotalDirectorySize { get; set; }
        public long TotalRemainingSize { get; set; }
        public long FileSize { get; set; }
        public long NbFilesLeftToDo { get; set; }
        public double Progression { get; set; }

    }
}
