using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{
    [Serializable]
    public class configData
    {
        public string AppName = string.Empty;

        public string SourceFTP = string.Empty;

        public string SourcePath = string.Empty;

        public string BackupPath = string.Empty;

        public string TrashPath = string.Empty;

        public string BackupFTP = string.Empty;

        public string UnZipperLoc = string.Empty;

        public string ReduceDecimal = string.Empty;
    }
}