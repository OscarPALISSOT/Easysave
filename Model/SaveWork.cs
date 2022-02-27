using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using EasySave.Command;

namespace EasySave.Model
{
    public class SaveWork
    {
        public string Name { get; set; }
        public SaveInfo Info { get; set; }
        public SaveState State { get; set; }
        public bool Selected { get; set; }
        public bool Priority { get; set; }

        public SaveWork(string name, string fileSource, string fileTarget, bool type)
        {
            Info = new SaveInfo();
            State = new SaveState();
            Name = name;
            Info.FileSource = fileSource;
            Info.FileTarget = fileTarget;
            Info.Full = type;
            State.State = 1;
            State.Name = name;
            Info.Name = name;
            State.TotalFileToCopy = Commands.GetDirectoryTotalNbFile(Info.FileSource);
            State.TotalDirectorySize = Commands.GetDirectoryTotalSize(Info.FileSource);
            Selected = false;
            Priority = false;
        }
    }
}
