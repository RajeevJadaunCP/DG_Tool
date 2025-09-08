using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DG_Tool.HelperClass
{
    public class ShortMasterFile
    {
        public string FileMasterID { get; set; }
        public string FileName { get; set; }

        public string FileDesc { get; set; }
        public string FileNamingConv { get; set; }
        public string FileIOID { get; set; }
        public bool IsSelected { get; set; }

    }
    public class FileMaster
    {
        public string FileMasterID { get; set; }
        public string FileName { get; set; }

        public string FileDesc { get; set; }
        public string FileNamingConv { get; set; }
        public string FileIOID { get; set; }
        public string FilePath { get; set; }
        public string FileStructure { get; set; }
        public string FileExtn { get; set; }
        public string Encrypt { get; set; }
        public string EncryptKey { get; set; }
        public int StatusID { get; set; }

        public string Status { get; set; }
        public bool IsSelected { get; set; }
    }
}
