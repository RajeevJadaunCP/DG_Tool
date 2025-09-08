using DG_Tool.WinForms.Authentication;
using DG_Tool.WinForms.InputFile;
using System;using CardPrintingApplication;
using System.IO;
using System.Windows.Forms;
using DG_Tool.WinForms.OutputFile;

namespace DG_Tool
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginPage());
            //Application.Run(new Testing());
            //Application.Run(new Create_Algo());
            if (!Directory.Exists(LoginPage.log_dir + "/Logging"))
            {
                Directory.CreateDirectory(LoginPage.log_dir + "/Logging");
            }
            string log = $"\n*******************************************************************************************\n" +
$"USER: {NewLogin.username} has logged out at [{DateTime.Now}]\n" +
$"USERNAME: {NewLogin.username}\n" +
$"SYSTEM NAME: {Environment.MachineName}\n" +
"*************************************** Data Processing Tool Closed *******************************************\n";

            //System.IO.File.AppendAllText(LoginPage.log_dir + "/Logging/" + $"{DateTime.Now.ToString("dd-MM-yyyy")}_log.txt", log);

        }
    }
}
