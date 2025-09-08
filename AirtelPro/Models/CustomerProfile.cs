using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DG_Tool.Models
{
    public class CustomerProfile
    {
        public int ProfileID { get; set; }
        public string CustomerName { get; set; }
        public string CircleName { get; set; }
        public string ProfileName { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
    }
    public class CustomerProfileFile
    {
        public int CustProfileFileID { get; set; }
        public string FileName { get; set; }
    }
}
