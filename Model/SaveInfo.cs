using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model
{
    public class SaveInfo
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public bool Full { get; set; }

    }
}
