using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DG_Tool.Models
{
    public class Circul
    {
        public int CustomerCirculID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerCirculName { get; set; }
        public string Status { get; set; }
    }
}
