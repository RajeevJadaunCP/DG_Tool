using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DG_Tool.HelperClass
{
    internal class LogMaster
    {

        public static string log_dir = ConfigurationManager.AppSettings["LOG_DIR"];
        StringBuilder logString = new StringBuilder();

        public static void addlog(string msg) 
        {
            if (!Directory.Exists(log_dir + "/Logging"))
            {
                Directory.CreateDirectory(log_dir + "/Logging");
            }
            System.IO.File.AppendAllText(log_dir + "/Logging/" + $"{DateTime.Now.ToString("dd-MM-yyyy")}_log.txt", msg);
        }
    }
}
