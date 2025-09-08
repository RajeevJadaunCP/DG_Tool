using DG_Tool.HelperClass;
using DG_Tool.Models;
using DG_Tool.WinForms.Authentication;
using DG_Tool.WinForms.Dashboard;
using DG_Tool.WinForms.OutputFile;
using CardPrintingApplication;
using CardPrintingApplication;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Pkcs;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using static CardPrintingApplication.EncryptionandDecryption;
using static Org.BouncyCastle.Math.Primes;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;
using DataTable = System.Data.DataTable;
using File = System.IO.File;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;



namespace DG_Tool.WinForms.OutputFile
{
    public partial class OFProcessing : Form
    {
        string Outfilelocation = "", headerfilepath = "";
        private int angle;
        private System.Windows.Forms.Timer timer;
        private Panel bufferingPanel;
        public static string hsm_IP = Database.sql_data_value("SELECT KeyValue FROM [DataTool_Keys] where[KeyName] = 'HSM_IP' ", "KeyValue");
        public static string file_enc_key = Database.sql_data_value("SELECT KeyValue FROM [dbo].[DataTool_Keys] where[KeyName] = 'File_Enc'", "KeyValue");
        public static string customer = string.Empty;
        public static string circle = string.Empty;
        public static string profile = string.Empty;
        public static string inputFile = string.Empty;
        public static string licenceFile = string.Empty;
        public static string log_dir = ConfigurationManager.AppSettings["LOG_DIR"];
        public static string EncryptDB = ConfigurationManager.AppSettings["Data_Encryption_in_DB"];
        public static int lastInsertedId = 0;
        public static int FileProcessingLotID = 0;
        public string unixTime = DateTime.Now.ToString("yyyyMMdd");
        public static DataTable Process_data = null;
        public static int customerID = 0;
        public static int circleID = 0;
        public static int ProfileID = 0;
        public static int total_pro_file = 0;
        public static int total_dup_file = 0;
        public static List<int> InsertedHDIDS = new List<int>();
        int fileid = 0;
        int records = 0;
        public static bool IsSingle = true;
        string first_imsi = string.Empty;
        string first_msisdn = string.Empty;
        string last_imsi = string.Empty;
        string first_icicid = string.Empty;
        string last_icicid = string.Empty;
        string Dup_First_icicid = string.Empty;
        string random_four = "1110";
        string random_eight = "11111110";
        Random RNG32 = new Random();
        Random RNG16 = new Random();
        Random random16 = new Random();
        Random random8 = new Random();
        StringBuilder logString = new StringBuilder();
        string connectionString = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public OFProcessing()
        {
            InitializeComponent();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();
            bufferingPanel = new Panel
            {
                Location = new Point(110, 12),
                Size = new System.Drawing.Size(50, 50)
            };
            bufferingPanel.Paint += BufferingPanel_Paint;
            panel1.Controls.Add(bufferingPanel);
            var customerList = CommonClass.GetCustomer();
            logString.Append($"\n********************************* Data Processing Started [{DateTime.Now}] USERNAME:{LoginPage.username} SYSTEM NAME : {Environment.MachineName} *************************************\n");
            Console.WriteLine($"\n********************************* Data Processing Started [{DateTime.Now}] USERNAME:{LoginPage.username} SYSTEM NAME : {Environment.MachineName} *************************************\n");
            if (customerList != null && customerList.Count > 0)
            {
                customerList.Insert(0, new CustomerDetails
                {
                    CustomerName = "----Select----",
                    CustomerID = 0,
                });
                cbxCustomer.DataSource = customerList;
                cbxCustomer.DisplayMember = "CustomerName";
                cbxCustomer.ValueMember = "CustomerID";


                //cbxCircle = "";
                //cbxProfile = "";

            }
        }
        private void btnLicence_Click(object sender, EventArgs e)
        {
            string filepath = string.Empty;
            string filename = string.Empty;

            if (ofdLicence.ShowDialog() == DialogResult.OK)
            {
                filepath = ofdLicence.FileName;
                filename = Path.GetFileName(filepath);
                //if (!IsDuplicateFile(filename))  **changes by sid on 22-04-2024 as not confirm to check the licence file commenting for future use.**
                //{
                //    txtLicence.Text = filepath;
                //}
                //else
                //{
                //    MessageBox.Show("File already exist: ",
                //                                "Message",
                //                                MessageBoxButtons.OK,
                //                                MessageBoxIcon.Information
                //                                );
                //}
                txtLicence.Text = filepath;

            }
            else
            {
                txtLicence.Clear();
            }

            licenceFile = filename;
        }
        private void btnInputFile_Click(object sender, EventArgs e)
        {
            string query_remove_junk_data = @"
                DELETE FROM [DataGenProcessData] 
                WHERE DataGenProcessHDID IN (SELECT [DataGenProcessHDID] FROM [dbo].[DataGenProcessHDFile] WHERE [FileName] LIKE '%mca.haes%' AND [OutFlileStatus] NOT IN (6, 17));

                DELETE FROM [dbo].[DataGenProcessDataRecord] 
                WHERE DataGenProcessHDID IN (SELECT [DataGenProcessHDID] FROM [dbo].[DataGenProcessHDFile] WHERE [FileName] LIKE '%mca.haes%' AND [OutFlileStatus] NOT IN (6, 17));

                DELETE FROM [dbo].[DataGenProcessHDFile] 
                WHERE DataGenProcessHDID IN (SELECT [DataGenProcessHDID] FROM [dbo].[DataGenProcessHDFile] WHERE [FileName] LIKE '%mca.haes%' AND [OutFlileStatus] NOT IN (6, 17));

                DELETE FROM [dbo].[DupCheck] 
                WHERE C1 IN (SELECT [DataGenProcessHDID] FROM [dbo].[DataGenProcessHDFile] WHERE [FileName] LIKE '%mca.haes%' AND [OutFlileStatus] NOT IN (6, 17));

                DELETE FROM [dbo].[FileLotMaster] 
                WHERE ID IN (SELECT [DataGenProcessHDID] FROM [dbo].[DataGenProcessHDFile] WHERE [FileName] LIKE '%mca.haes%' AND [OutFlileStatus] NOT IN (6, 17));

                DELETE FROM [dbo].[DataGenProcessHD] 
                WHERE DataGenProcessHDID IN (SELECT [DataGenProcessHDID] FROM [dbo].[DataGenProcessHDFile] WHERE [FileName] LIKE '%mca.haes%' AND [OutFlileStatus] NOT IN (6, 17));

                DELETE FROM [DataGenProcessData] 
                WHERE DataGenProcessHDID IN (select [DataGenProcessHDID] from [dbo].[DataGenProcessHD]  where (DataGenProcessStatus = 5 or isnull(DataGenProcessStatus,'')=''));

                DELETE FROM [dbo].[DataGenProcessDataRecord] 
                WHERE DataGenProcessHDID IN (select [DataGenProcessHDID] from [dbo].[DataGenProcessHD]  where (DataGenProcessStatus = 5 or isnull(DataGenProcessStatus,'')=''));

                DELETE FROM [dbo].[DataGenProcessHDFile] 
                WHERE DataGenProcessHDID IN (select [DataGenProcessHDID] from [dbo].[DataGenProcessHD]  where (DataGenProcessStatus = 5 or isnull(DataGenProcessStatus,'')=''));

                DELETE FROM [dbo].[DupCheck] 
                WHERE C1 IN (select [DataGenProcessHDID] from [dbo].[DataGenProcessHD]  where (DataGenProcessStatus = 5 or isnull(DataGenProcessStatus,'')=''));

                DELETE FROM [dbo].[FileLotMaster] 
                WHERE ID IN (select [DataGenProcessHDID] from [dbo].[DataGenProcessHD]  where (DataGenProcessStatus = 5 or isnull(DataGenProcessStatus,'')=''));

                DELETE FROM [dbo].[DataGenProcessHD] 
                WHERE DataGenProcessHDID IN (select [DataGenProcessHDID] from [dbo].[DataGenProcessHD]  where (DataGenProcessStatus = 5 or isnull(DataGenProcessStatus,'')=''));

                Truncate table [dbo].[DGPDR_Base]

            ";
            using (SqlConnection con11test_1 = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query_remove_junk_data, con11test_1);

                try
                {
                    con11test_1.Open();

                    // Begin a transaction to execute the deletions as a single unit of work
                    SqlTransaction transaction = con11test_1.BeginTransaction();
                    command.Transaction = transaction;

                    // Execute the query
                    int rowsAffected = command.ExecuteNonQuery();
                    transaction.Commit();

                    Console.WriteLine($"Data deleted successfully. Rows affected: " + rowsAffected);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: " + ex.Message);
                }
            }







            if (cbxCustomer.SelectedIndex > 0 && cbxCircle.SelectedIndex > 0 && cbxProfile.SelectedIndex > 0)
            {
                txtInputfile.Text = "";
                InsertedHDIDS.Clear();
                logString.Append($"\n1. User initiated the data processing tool and selected the following input:-\n    Customer : {cbxCustomer.Text}\n    Circle : {cbxCircle.Text}\n    Profile : {cbxProfile.Text}\n");
                Console.WriteLine($"\n1. User initiated the data processing tool and selected the following input:-\n    Customer : {cbxCustomer.Text}\n    Circle : {cbxCircle.Text}\n    Profile : {cbxProfile.Text}\n");
                string filepath = string.Empty;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "Select Multiple Files";
                openFileDialog.Filter = "All Files (*.*)|*.*";
                DialogResult result = openFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {

                    string[] fileNames = openFileDialog.FileNames;
                    logString.Append($"\n2. Selected Directory Path : [{Path.GetDirectoryName(fileNames[0])}]\n");
                    Console.WriteLine($"\n2. Selected Directory Path : [{Path.GetDirectoryName(fileNames[0])}]\n");
                    logString.Append($"\n3. {fileNames.Length} file selected.\n");
                    Console.WriteLine($"\n3. {fileNames.Length} file selected.\n");
                    total_pro_file = fileNames.Length;
                    logString.Append($"\n4. Initiating duplicate check process for selected files in the folder.\n");
                    Console.WriteLine($"\n4. Initiating duplicate check process for selected files in the folder.\n");
                    char ch = 'a';
                    foreach (string fileName in fileNames)
                    {
                        logString.Append($"\n    {ch}. File: [{Path.GetFileName(fileName)}]\n");
                        Console.WriteLine($"\n    {ch}. File: [{Path.GetFileName(fileName)}]\n");
                        if (!IsDuplicateFile(fileName))
                        {

                            logString.Append($"       - No duplicate filename found.\n");
                            //duplicacy check on iccid and imsi
                            string count = iccid_dupcheck(fileName);
                            //string count = "";
                            if (count == "")
                            {
                                if (string.IsNullOrEmpty(txtInputfile.Text))
                                {
                                    txtInputfile.Text += fileName;
                                }
                                else
                                {
                                    txtInputfile.Text = txtInputfile.Text + "," + fileName;
                                }

                                logString.Append($"       - No duplicate ICCIDs found.\n");
                            }
                            else
                            {
                                MessageBox.Show($"{count}. First Duplicate ICCID '{Dup_First_icicid}",
                                                            "Message",
                                                            MessageBoxButtons.OK,
                                                            MessageBoxIcon.Information
                                                            );
                                logString.Append($"       - {count}.First Duplicate ICCID '{Dup_First_icicid}'\n");

                                txtoutput.Text += $"{fileName} duplicate records found: \r\n";
                                total_dup_file++;

                            }

                            //logString.Append($"       - No duplicate filename found.\n");
                            //Console.WriteLine($"       - No duplicate filename found.\n");
                            //string count = iccid_dupcheck(fileName);
                            //if (count == "")
                            //{
                            //    if (string.IsNullOrEmpty(txtInputfile.Text))
                            //    {
                            //        txtInputfile.Text += fileName;
                            //    }
                            //    else
                            //    {
                            //        txtInputfile.Text = txtInputfile.Text + "," + fileName;
                            //    }

                            //    logString.Append($"       - No duplicate ICCIDs found.\n");
                            //    Console.WriteLine($"       - No duplicate ICCIDs found.\n");
                            //}
                            //else if (count == "-1")
                            //{
                            //    Console.WriteLine($"File is different from template given.");
                            //}
                            //else
                            //{
                            //    MessageBox.Show($"{fileName} duplicate records found. First Duplicate ICCID '{Dup_First_icicid}",
                            //                                "Message",
                            //                                MessageBoxButtons.OK,
                            //                                MessageBoxIcon.Information
                            //                                );
                            //    //logString.Append("\r\n" + DateTime.Now + $"**{Path.GetFileName(fileName)} file record already exist in database.**");
                            //    //Console.WriteLine($"\r\n");
                            //    logString.Append($"       - {count} duplicate ICCIDs found.First Duplicate ICCID '{Dup_First_icicid}'\n");
                            //    Console.WriteLine($"       - {count} duplicate ICCIDs found.First Duplicate ICCID '{Dup_First_icicid}'\n");

                            //    txtoutput.Text += $"{fileName} duplicate records found: \r\n";
                            //    total_dup_file++;

                            //}

                        }
                        else
                        {


                            txtoutput.Text += $"{fileName} already exist: \r\n";
                            total_dup_file++;
                        }
                        ch = (char)(ch + 1);

                    }
                    if (string.IsNullOrEmpty(txtInputfile.Text))
                    {
                        MessageBox.Show("No file to proceed.");
                        txtoutput.Text += "No file to proceed.\r\n";
                    }
                    else
                    {
                        //MessageBox.Show(txtInputfile.Text + " are ok to proceed.");
                        txtoutput.Text += txtInputfile.Text.Replace(',', '\n') + "\nOK To Proceed.\r\n";
                    }

                }
                else
                {
                    txtInputfile.Clear();
                }
                using (SqlConnection con11 = new SqlConnection(connectionString))
                {
                    con11.Open();
                    using (SqlCommand cmd1 = new SqlCommand($"SELECT COUNT(1) FROM [License_InPutTemplate] WHERE [CustID]=@cust and [ProfileID]=@profile;", con11))//check if the data matched with licence data
                    {
                        cmd1.Parameters.AddWithValue("@cust", cbxCustomer.SelectedValue);
                        cmd1.Parameters.AddWithValue("@profile", cbxProfile.SelectedValue);
                        var result1 = cmd1.ExecuteScalar();
                        Console.WriteLine(result1);
                        if (Convert.ToInt32(result1) == 0)
                        {
                            txtLicence.Text = "Selected profile has no licence file";
                            btnLicence.Enabled = false;
                        }
                        else
                        {
                            txtLicence.Text = "";
                            btnLicence.Enabled = true;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("All fields are required: ",
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information
                                            );
                logString.Append("\nAll fields are required: \n");
                Console.WriteLine($"\nAll fields are required: \n");
            }

        }
        //public string iccid_dupcheck(string filename)
        //{
        //    logString.Append($"       - Checking for duplicate records in the database.\n");
        //    Console.WriteLine($"       - Checking for duplicate records in the database.\n");
        //    string[] strs = { "ICCID", "IMSI", "MSISDN" };
        //    int count = 0;
        //    foreach (string str in strs)
        //    {
        //        string varname = "";
        //        int pos_from = 0;
        //        int len = 0;
        //        int line = 0;
        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand($"select [VarName],[PositionFrom],[Len],[LineNumber] from [InPutDataTemplate] where [CustID]= {cbxCustomer.SelectedValue} and [ProfileID]={cbxProfile.SelectedValue} and [VarDes]='{str}' ", con))
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read()) // Changed to reader.Read() instead of reader.HasRows
        //                    {
        //                        varname = reader.GetString(0); // Changed to 0 for VarName
        //                        pos_from = reader.GetInt32(1); // Changed to 1 for PositionFrom
        //                        len = reader.GetInt32(2); // Changed to 2 for Len
        //                        line = reader.GetInt32(3); // Changed to 2 for Len
        //                    }
        //                }
        //            }
        //        }
        //        if (varname != "")
        //        {
        //            StreamReader sr_10 = new StreamReader(filename);
        //            string line_all = "";
        //            int line_number_1 = 1;
        //            string my_data_1 = "";
        //            string iccid_dupname = "";
        //            List<string> iccids = new List<string>();
        //            while ((line_all = sr_10.ReadLine()) != null)
        //            {
        //                if (line_number_1 >= line)
        //                {
        //                    if (line_all.Length >= pos_from + len)
        //                    {
        //                        iccid_dupname = line_all.Substring(pos_from, len);
        //                        iccids.Add(StringToHex(iccid_dupname));
        //                    }
        //                    else if (line_all.Length > pos_from)
        //                    {
        //                        iccid_dupname = line_all.Substring(pos_from, line_all.Length - pos_from);
        //                        iccids.Add(StringToHex(iccid_dupname));
        //                    }

        //                }

        //                line_number_1++;
        //            }
        //            if (iccids.Count > 0)
        //            {
        //                using (SqlConnection con = new SqlConnection(connectionString))
        //                {
        //                    SqlDataReader reader = null;
        //                    con.Open();
        //                    using (SqlCommand cmd = new SqlCommand($"select Count(1) from [dbo].[DupCheck] where {str}  IN ('{string.Join("','", iccids)}')", con))
        //                    {
        //                        cmd.CommandTimeout = 1200;
        //                        cmd.CommandType = CommandType.Text;
        //                        count = (int)cmd.ExecuteScalar();
        //                    }
        //                }
        //            }
        //            if (count > 0)
        //            {
        //                using (SqlConnection con = new SqlConnection(connectionString))
        //                {
        //                    SqlDataReader reader = null;
        //                    con.Open();
        //                    using (SqlCommand cmd = new SqlCommand($"select Top(1) {str} from [dbo].[DupCheck] where {str}  IN ('{string.Join("','", iccids)}')", con))
        //                    {
        //                        cmd.CommandTimeout = 1200;
        //                        cmd.CommandType = CommandType.Text;
        //                        using (SqlDataReader reader1 = cmd.ExecuteReader())
        //                        {
        //                            if (reader1.Read())
        //                            {
        //                                Dup_First_icicid = HexToString(reader1[str].ToString());
        //                            }
        //                        }
        //                    }
        //                }
        //                return count.ToString();
        //            }
        //        }
        //    }
        //    return "";
        //}
        //public string iccid_dupcheck(string filename)
        //{
        //    StreamReader sr_10 = new StreamReader(filename);
        //    string line_all = "";
        //    int line_number_1 = 1;
        //    string my_data_1 = "";
        //    string iccid_dupname = "";
        //    logString.Append($"       - Checking for duplicate records in the database.\n");
        //Console.WriteLine($"       - Checking for duplicate records in the database.\n");
        //    while ((line_all = sr_10.ReadLine()) != null)
        //    {


        //        if (line_number_1 > 15)
        //        {


        //            iccid_dupname = line_all.Substring(0, 19);
        //            using (SqlConnection con = new SqlConnection(connectionString))
        //            {
        //                SqlDataReader reader = null;

        //                con.Open();
        //                //using (SqlCommand cmd = new SqlCommand("SELECT * FROM Vw_DataGenProcessList WHERE datafilename = @finename", con))
        //                using (SqlCommand cmd = new SqlCommand("  select v003 from [dbo].[DataGenProcessDataRecord] where v003  = @iccid_dupname", con))
        //                {


        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.Parameters.AddWithValue("@iccid_dupname", iccid_dupname);
        //                    reader = cmd.ExecuteReader();

        //                    if (reader.HasRows)
        //                    {
        //                        return iccid_dupname;
        //                    }
        //                }

        //            }
        //        }
        //        line_number_1++;
        //    }
        //    return "";
        //}


        //public string iccid_dupcheck(string filename)
        //{
        //    logString.Append($"       - Checking for duplicate records in the database.\n");
        //    string[] strs = { "ICCID", "IMSI", "MSISDN" };
        //    int count = 0;
        //    string msg = "";
        //    foreach (string str in strs)
        //    {
        //        string varname = "";
        //        int pos_from = 0;
        //        int len = 0;
        //        int line = 0;
        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand($"select [VarName],[PositionFrom],[Len],[LineNumber] from [InPutDataTemplate] where [CustID]= {cbxCustomer.SelectedValue} and [ProfileID]={cbxProfile.SelectedValue} and [VarDes]='{str}' ", con))
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read()) // Changed to reader.Read() instead of reader.HasRows
        //                    {
        //                        varname = reader.GetString(0); // Changed to 0 for VarName
        //                        pos_from = reader.GetInt32(1); // Changed to 1 for PositionFrom
        //                        len = reader.GetInt32(2); // Changed to 2 for Len
        //                        line = reader.GetInt32(3); // Changed to 2 for Len
        //                    }
        //                }
        //            }
        //        }
        //        if (varname != "")
        //        {
        //            StreamReader sr_10 = new StreamReader(filename);
        //            string line_all = "";
        //            int line_number_1 = 1;
        //            string my_data_1 = "";
        //            string iccid_dupname = "";
        //            List<string> iccids = new List<string>();
        //            while ((line_all = sr_10.ReadLine()) != null)
        //            {
        //                if (line_number_1 >= line)
        //                {
        //                    if (line_all.Length >= pos_from + len)
        //                    {
        //                        iccid_dupname = line_all.Substring(pos_from, len);
        //                        iccids.Add(StringToHex(iccid_dupname));
        //                    }
        //                    else if (line_all.Length > pos_from)
        //                    {
        //                        iccid_dupname = line_all.Substring(pos_from, line_all.Length - pos_from);
        //                        iccids.Add(StringToHex(iccid_dupname));
        //                    }

        //                }

        //                line_number_1++;
        //            }
        //            if (iccids.Count > 0)
        //            {

        //                using (SqlConnection con = new SqlConnection(connectionString))
        //                {
        //                    con.Open();

        //                    string query = $@"SELECT TOP 1     f.DataGenProcessHDID,      f.FileName,      f.OutFileProcessDate,      (SELECT COUNT(1) FROM DupCheck d2 WHERE d2.C1 = f.DataGenProcessHDID) AS RecordCount FROM DataGenProcessHDFile f INNER JOIN DupCheck d ON d.C1 = f.DataGenProcessHDID WHERE f.OutFlileStatus = 2 AND d.{str} IN ('{string.Join("','", iccids)}')";

        //                    using (SqlCommand cmd = new SqlCommand(query, con))
        //                    {
        //                        cmd.CommandTimeout = 1200;
        //                        cmd.CommandType = CommandType.Text;

        //                        using (SqlDataReader reader = cmd.ExecuteReader())
        //                        {
        //                            if (reader.Read())
        //                            {
        //                                int dataGenProcessHDID = reader.GetInt32(0);
        //                                string fileName = reader.GetString(1);
        //                                DateTime outFileProcessDate = reader.GetDateTime(2);
        //                                count = reader.GetInt32(3);
        //                                msg = $"Duplicate Records Found :- \nHDID:{dataGenProcessHDID}\nFilename:{fileName}\nProcessed Date:{outFileProcessDate}\nRecordCount:{count}";
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //            if (count > 0)
        //            {
        //                using (SqlConnection con = new SqlConnection(connectionString))
        //                {
        //                    SqlDataReader reader = null;
        //                    con.Open();
        //                    using (SqlCommand cmd = new SqlCommand($"select Top(1) {str} from [dbo].[DupCheck] where {str}  IN ('{string.Join("','", iccids)}')", con))
        //                    {
        //                        cmd.CommandTimeout = 1200;
        //                        cmd.CommandType = CommandType.Text;
        //                        using (SqlDataReader reader1 = cmd.ExecuteReader())
        //                        {
        //                            if (reader1.Read())
        //                            {
        //                                Dup_First_icicid = HexToString(reader1[str].ToString());
        //                            }
        //                        }
        //                    }
        //                }
        //                return msg;
        //            }
        //        }
        //    }
        //    return "";
        //}
        public string iccid_dupcheck_1(string filename)
        {
            logString.Append("       - Checking for duplicate records in the database.\n");
            string[] strs = { "QUANTITY", "ICCID", "IMSI", "MSISDN" };
            string msg = "";
            int file_qty = 0;
            foreach (string str in strs)
            {
                string varname = "", isincremental = "";
                int pos_from = 0, len = 0, line = 0;

                // Step 1: Get field positions from DB
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string templateQuery = @"SELECT [VarName], [PositionFrom], [Len], [LineNumber],tag
                                     FROM [InPutDataTemplate]
                                     WHERE [CustID]= @CustID AND [ProfileID]=@ProfileID AND [VarDes]=@VarDes  order by VarName ";

                    using (SqlCommand cmd = new SqlCommand(templateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@CustID", cbxCustomer.SelectedValue);
                        cmd.Parameters.AddWithValue("@ProfileID", cbxProfile.SelectedValue);
                        cmd.Parameters.AddWithValue("@VarDes", str);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                varname = reader.GetString(0);
                                pos_from = reader.GetInt32(1);
                                len = reader.GetInt32(2);
                                line = reader.GetInt32(3);
                                isincremental = !reader.IsDBNull(4) ? reader.GetString(4) : string.Empty;
                            }
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(varname))
                    continue;

                StreamReader sr1 = new StreamReader(filename);
                string data_val_line = sr1.ReadToEnd();

                if (str == "QUANTITY")
                {
                    try
                    {
                        file_qty = Convert.ToInt32(data_val_line.Split('\n')[line - 1].Substring(pos_from, len).Trim());

                    }
                    catch
                    {
                        string line_test = data_val_line.Split('\n')[line - 1];
                        line_test = line_test.Replace("\t", "    ");
                        int safeLen = Math.Min(len, line_test.Length - pos_from);
                        string qtyStr = line_test.Substring(pos_from, safeLen).Trim();
                        file_qty = Convert.ToInt32(qtyStr);

                    }
                    continue;


                }


                List<string> datalist = new List<string>();
                string[] lines = data_val_line.Split('\n');
                string line_all = "";

                if (!string.IsNullOrEmpty(isincremental) && isincremental.Equals("isincremental", StringComparison.OrdinalIgnoreCase))
                {
                    // Get base line for incremental case
                    line_all = lines[line - 1];
                    long data_long = 0;

                    for (int i = 0; i < file_qty; i++)
                    {
                        if (i > 0 && line_all.Length >= pos_from + len)
                            data_long = long.Parse(line_all.Substring(pos_from, len));

                        data_long++; // Always increment (first loop starts from 0+1)

                        datalist.Add(StringToHex(data_long.ToString()));
                    }
                }
                else
                {
                    for (int i = 0; i < file_qty; i++)
                    {
                        line_all = lines[line + i - 1];

                        string value = (line_all.Length >= pos_from + len)
                            ? line_all.Substring(pos_from, len)
                            : (line_all.Length > pos_from ? line_all.Substring(pos_from) : string.Empty);

                        if (!string.IsNullOrEmpty(value))
                            datalist.Add(StringToHex(value));
                    }
                }

                if (datalist.Count == 0)
                    continue;

                //// Step 2: Read file and extract ICCIDs
                //List<string> iccids = new List<string>();
                //using (StreamReader sr = new StreamReader(filename))
                //{
                //    string line_all;
                //    int line_number_1 = 1;

                //    while ((line_all = sr.ReadLine()) != null)
                //    {
                //        if (line_number_1 >= line)
                //        {
                //            if (line_all.Length >= pos_from + len)
                //                iccids.Add(StringToHex(line_all.Substring(pos_from, len)));
                //            else if (line_all.Length > pos_from)
                //                iccids.Add(StringToHex(line_all.Substring(pos_from)));
                //        }
                //        line_number_1++;
                //    }
                //}

                //if (datalist.Count == 0)
                //    continue;

                // Step 3: Prepare TVP
                DataTable iccidTable = new DataTable();
                iccidTable.Columns.Add("Value", typeof(string));
                foreach (string val in datalist.Distinct())
                    iccidTable.Rows.Add(val);

                // Step 4: Query for duplicates
                string dupQuery = $@"
            SELECT TOP 1 
                f.DataGenProcessHDID, 
                f.FileName, 
                f.OutFileProcessDate,
                COUNT_BIG(d2.C1) AS RecordCount
            FROM DataGenProcessHDFile f
            INNER JOIN DupCheck d ON d.C1 = f.DataGenProcessHDID
            LEFT JOIN DupCheck d2 ON d2.C1 = f.DataGenProcessHDID
            INNER JOIN @IccidList il ON d.{str} = il.Value
            WHERE f.OutFlileStatus = 2
            GROUP BY f.DataGenProcessHDID, f.FileName, f.OutFileProcessDate";

                long count = 0;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(dupQuery, con))
                    {
                        cmd.CommandTimeout = 1200;
                        cmd.Parameters.AddWithValue("@IccidList", iccidTable).SqlDbType = SqlDbType.Structured;
                        cmd.Parameters["@IccidList"].TypeName = "dbo.ICCID_Dup_List";

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int dataGenProcessHDID = reader.GetInt32(0);
                                string fileName = reader.GetString(1);
                                DateTime outFileProcessDate = reader.GetDateTime(2);
                                count = reader.GetInt64(3);

                                msg = $"Duplicate Records Found :- \nHDID:{dataGenProcessHDID}\nFilename:{fileName}\nProcessed Date:{outFileProcessDate}\nRecordCount:{count}";

                                reader.Close();

                                // Step 5: Get first duplicate ICCID
                                string dupValueQuery = $"SELECT TOP 1 Value FROM @IccidList";
                                using (SqlCommand cmd2 = new SqlCommand(dupValueQuery, con))
                                {
                                    cmd2.Parameters.AddWithValue("@IccidList", iccidTable).SqlDbType = SqlDbType.Structured;
                                    cmd2.Parameters["@IccidList"].TypeName = "dbo.ICCID_Dup_List";

                                    using (SqlDataReader r2 = cmd2.ExecuteReader())
                                    {
                                        if (r2.Read())
                                            Dup_First_icicid = HexToString(r2["Value"].ToString());
                                    }
                                }

                                return msg;
                            }
                        }
                    }
                }
            }

            return "";
        }

        public string iccid_dupcheck(string filename)
        {
            logString.Append("       - Checking for duplicate records in the database.\n");
            string[] strs = { "QUANTITY", "ICCID", "IMSI", "MSISDN" };
            string msg = "";
            int file_qty = 0;

            // Read the file once into memory
            string[] lines = File.ReadAllLines(filename);

            foreach (string str in strs)
            {
                string varname = "", isincremental = "";
                int pos_from = 0, len = 0, line = 0;

                // Step 1: Get field positions from DB
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(@"
            SELECT [VarName], [PositionFrom], [Len], [LineNumber], [Tag]
            FROM [InPutDataTemplate]
            WHERE [CustID] = @CustID AND [ProfileID] = @ProfileID AND [VarDes] = @VarDes order by VarName", con))
                {
                    cmd.Parameters.AddWithValue("@CustID", cbxCustomer.SelectedValue);
                    cmd.Parameters.AddWithValue("@ProfileID", cbxProfile.SelectedValue);
                    cmd.Parameters.AddWithValue("@VarDes", str);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            varname = reader.GetString(0);
                            pos_from = reader.GetInt32(1);
                            len = reader.GetInt32(2);
                            line = reader.GetInt32(3);
                            isincremental = !reader.IsDBNull(4) ? reader.GetString(4) : string.Empty;
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(varname))
                    continue;

                if (str == "QUANTITY")
                {
                    string lineText = lines[line - 1].Replace("\t", "    ");
                    int safeLen = Math.Min(len, Math.Max(0, lineText.Length - pos_from));
                    string qtyStr = safeLen > 0 ? lineText.Substring(pos_from, safeLen).Trim() : "0";
                    file_qty = int.TryParse(qtyStr, out int q) ? q : 0;
                    continue;
                }

                // Step 2: Extract values
                HashSet<string> datalist = new HashSet<string>(); // ensures distinct automatically
                if (!string.IsNullOrEmpty(isincremental) &&
                    isincremental.Equals("isincremental", StringComparison.OrdinalIgnoreCase))
                {
                    string line_all = lines[line - 1];
                    long data_long = 0;
                    for (int i = 0; i < file_qty; i++)
                    {
                        if (i > 0 && line_all.Length >= pos_from + len)
                            data_long = long.Parse(line_all.Substring(pos_from, len));
                        data_long++;
                        datalist.Add(StringToHex(data_long.ToString()));
                    }
                }
                else
                {
                    for (int i = 0; i < file_qty; i++)
                    {
                        string line_all = lines[line + i - 1];
                        if (line_all.Length <= pos_from) continue;

                        string value = (line_all.Length >= pos_from + len)
                            ? line_all.Substring(pos_from, len)
                            : line_all.Substring(pos_from);

                        if (!string.IsNullOrEmpty(value))
                            datalist.Add(StringToHex(value));
                    }
                }

                if (datalist.Count == 0)
                    continue;

                // Step 3: Prepare TVP
                DataTable iccidTable = new DataTable();
                iccidTable.Columns.Add("Value", typeof(string));
                iccidTable.BeginLoadData();
                foreach (string val in datalist)
                    iccidTable.Rows.Add(val);
                iccidTable.EndLoadData();

                // Step 4: Query for duplicates
                string dupQuery = $@"
            SELECT TOP 1 
                f.DataGenProcessHDID, 
                f.FileName, 
                f.OutFileProcessDate,
                COUNT_BIG(d2.C1) AS RecordCount,
                MIN(il.Value) AS FirstDupValue   -- << get first duplicate here
            FROM DataGenProcessHDFile f
            INNER JOIN DupCheck d ON d.C1 = f.DataGenProcessHDID
            LEFT JOIN DupCheck d2 ON d2.C1 = f.DataGenProcessHDID
            INNER JOIN @IccidList il ON d.{str} = il.Value
            WHERE f.OutFlileStatus = 2
            GROUP BY f.DataGenProcessHDID, f.FileName, f.OutFileProcessDate";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(dupQuery, con))
                {
                    cmd.CommandTimeout = 1200;
                    var p = cmd.Parameters.AddWithValue("@IccidList", iccidTable);
                    p.SqlDbType = SqlDbType.Structured;
                    p.TypeName = "dbo.ICCID_Dup_List";

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int dataGenProcessHDID = reader.GetInt32(0);
                            string fileName = reader.GetString(1);
                            DateTime outFileProcessDate = reader.GetDateTime(2);
                            long count = reader.GetInt64(3);
                            string firstDupHex = reader.IsDBNull(4) ? "" : reader.GetString(4);

                            if (!string.IsNullOrEmpty(firstDupHex))
                                Dup_First_icicid = HexToString(firstDupHex);

                            msg = $"Duplicate Records Found :- \nHDID:{dataGenProcessHDID}\nFilename:{fileName}\nProcessed Date:{outFileProcessDate}\nRecordCount:{count}";
                            return msg;
                        }
                    }
                }
            }
            return "";
        }



        public string iccid_dupcheck_working_22082025(string filename)
        {
            logString.Append("       - Checking for duplicate records in the database.\n");
            string[] strs = { "Quantity", "ICCID", "IMSI", "MSISDN" };
            string msg = "", tag_database = "";
            int file_qty = 0;
            foreach (string str in strs)
            {
                string varname = "", big_number = "";
                int pos_from = 0, len = 0, line = 0;

                // Step 1: Get field positions from DB
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string templateQuery = @"SELECT [VarName], [PositionFrom], [Len], [LineNumber],[Tag]
                                     FROM [InPutDataTemplate]
                                     WHERE [CustID]= @CustID AND [ProfileID]=@ProfileID AND [VarDes]=@VarDes  order by VarName";

                    using (SqlCommand cmd = new SqlCommand(templateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@CustID", 1023);
                        cmd.Parameters.AddWithValue("@ProfileID", 1058);
                        cmd.Parameters.AddWithValue("@VarDes", str);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                varname = reader.IsDBNull(0) ? null : reader.GetString(0);
                                pos_from = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                                len = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                                line = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                                tag_database = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            }
                        }
                    }
                }

                if (str == "Quantity")
                {
                    StreamReader sr1 = new StreamReader(filename);
                    string data_val_line = sr1.ReadToEnd();
                    try
                    {
                        file_qty = Convert.ToInt32(data_val_line.Split('\n')[line - 1].Substring(pos_from, len).Trim());

                    }
                    catch
                    {
                        string line_test = data_val_line.Split('\n')[line - 1];
                        line_test = line_test.Replace("\t", "    ");
                        int safeLen = Math.Min(len, line_test.Length - pos_from);
                        string qtyStr = line_test.Substring(pos_from, safeLen).Trim();
                        file_qty = Convert.ToInt32(qtyStr);

                    }
                    continue;
                }

                if (string.IsNullOrWhiteSpace(varname))
                    continue;



                // Step 2: Read file and extract ICCIDs
                List<string> iccids = new List<string>();
                if (tag_database.Trim() == "incremental")
                {
                    StreamReader sr1 = new StreamReader(filename);
                    string data_val_line = sr1.ReadToEnd();
                    try
                    {
                        big_number = data_val_line.Split('\n')[line - 1].Substring(pos_from, len).Trim();
                    }
                    catch
                    {
                        string line_test = data_val_line.Split('\n')[line - 1];
                        int safeLen = Math.Min(len, line_test.Length - pos_from);
                        string qtyStr = line_test.Substring(pos_from, safeLen).Trim();
                        big_number = qtyStr;
                    }

                    BigInteger start = BigInteger.Parse(big_number);
                    BigInteger end = start + file_qty - 1;

                    for (BigInteger i = start; i <= end; i++)
                    {
                        //Console.WriteLine(i);
                        iccids.Add(StringToHex(i.ToString()));
                    }


                }
                else
                {
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        string line_all;
                        int line_number_1 = 1;

                        while ((line_all = sr.ReadLine()) != null)
                        {
                            if (line_number_1 >= line)
                            {
                                if (line_all.Length >= pos_from + len)
                                    iccids.Add(StringToHex(line_all.Substring(pos_from, len)));
                                else if (line_all.Length > pos_from)
                                    iccids.Add(StringToHex(line_all.Substring(pos_from)));
                            }
                            line_number_1++;
                        }
                    }
                }
                if (iccids.Count == 0)
                    continue;

                // Step 3: Prepare TVP
                DataTable iccidTable = new DataTable();
                iccidTable.Columns.Add("Value", typeof(string));
                foreach (string val in iccids.Distinct())
                    iccidTable.Rows.Add(val);

                // Step 4: Query for duplicates
                string dupQuery = $@"SELECT TOP 1 
                            f.DataGenProcessHDID, 
                            f.FileName, 
                            f.OutFileProcessDate,
                            COUNT_BIG(d2.C1) AS RecordCount
                        FROM DataGenProcessHDFile f
                        INNER JOIN DupCheck d ON d.C1 = f.DataGenProcessHDID
                        LEFT JOIN DupCheck d2 ON d2.C1 = f.DataGenProcessHDID
                        INNER JOIN @IccidList il ON d.{str} = il.Value
                        WHERE f.OutFlileStatus = 2
                        GROUP BY f.DataGenProcessHDID, f.FileName, f.OutFileProcessDate";

                long count = 0;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(dupQuery, con))
                    {
                        cmd.CommandTimeout = 1200;
                        cmd.Parameters.AddWithValue("@IccidList", iccidTable).SqlDbType = SqlDbType.Structured;
                        cmd.Parameters["@IccidList"].TypeName = "dbo.ICCID_Dup_List";

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int dataGenProcessHDID = reader.GetInt32(0);
                                string fileName = reader.GetString(1);
                                DateTime outFileProcessDate = reader.GetDateTime(2);
                                count = reader.GetInt64(3);

                                msg = $"Duplicate Records Found :- \nHDID:{dataGenProcessHDID}\nFilename:{fileName}\nProcessed Date:{outFileProcessDate}\nRecordCount:{count}";

                                reader.Close();

                                // Step 5: Get first duplicate ICCID
                                string dupValueQuery = $"SELECT TOP 1 Value FROM @IccidList";
                                using (SqlCommand cmd2 = new SqlCommand(dupValueQuery, con))
                                {
                                    cmd2.Parameters.AddWithValue("@IccidList", iccidTable).SqlDbType = SqlDbType.Structured;
                                    cmd2.Parameters["@IccidList"].TypeName = "dbo.ICCID_Dup_List";

                                    using (SqlDataReader r2 = cmd2.ExecuteReader())
                                    {
                                        if (r2.Read())
                                            Dup_First_icicid = HexToString(r2["Value"].ToString());
                                    }
                                }

                                return msg;
                            }
                        }
                    }
                }
            }

            return "";
        }

        public bool IsDuplicateFile(string filename)
        {
            bool flag = false;
            int dataGenProcessHdId = 0;
            DateTime createdOn = DateTime.MinValue;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                logString.Append($"       - Checking for duplicate filename in the database.\n");
                Console.WriteLine($"       - Checking for duplicate filename in the database.\n");
                SqlDataReader reader = null;
                //bool flag = false;
                con.Open();
                //using (SqlCommand cmd = new SqlCommand("SELECT * FROM Vw_DataGenProcessList WHERE datafilename = @finename", con))
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Vw_InputFileDupCheck WHERE FilePath = @finename and [CustID]= {cbxCustomer.SelectedValue} and [CustProfileID]={cbxProfile.SelectedValue}", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@finename", filename);
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        flag = true;
                        while (reader.Read())
                        {
                            dataGenProcessHdId = reader.GetInt32(reader.GetOrdinal("DataGenProcessHDID"));
                            createdOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn"));

                            //Console.WriteLine($"       - Found duplicate record: DataGenProcessHDID = {dataGenProcessHdId}, CreatedOn = {createdOn}");
                            //logString.AppendLine($"       - Found duplicate record: DataGenProcessHDID = {dataGenProcessHdId}, CreatedOn = {createdOn}");
                        }
                        MessageBox.Show($"{filename} already processed \nDataGenProcessHDID = {dataGenProcessHdId} \nProcessing date = {createdOn} ",
                                                        "Message",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Information
                                                        );
                        logString.Append($"{filename} already processed \nDataGenProcessHDID = {dataGenProcessHdId} \nProcessing date = {createdOn}\n");
                        Console.WriteLine($"{filename} already processed DataGenProcessHDID = {dataGenProcessHdId}, Processing date = {createdOn}\n");
                    }
                }
                return flag;
            }
        }
        private void cbxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCustomer.SelectedIndex > 0)
            {

                // ✅ Clear dependent controls first
                cbxCircle.DataSource = null;
                cbxProfile.DataSource = null;

                txtInputfile.Clear();
                txtLicence.Clear();
                txtoutput.Clear();

                var circulList = CommonClass.GetCircle(Convert.ToInt32(cbxCustomer.SelectedValue));

                if (circulList != null && circulList.Count > 0)
                {
                    circulList.Insert(0, new Circle
                    {
                        CircleName = "----Select----",
                        CircleID = 0,
                    });
                    cbxCircle.DataSource = circulList;
                    cbxCircle.DisplayMember = "CircleName";
                    cbxCircle.ValueMember = "CircleID";

                }
                else
                {
                    // ✅ Clear dependent controls first
                    cbxCircle.DataSource = null;
                    cbxProfile.DataSource = null;

                    txtInputfile.Clear();
                    txtLicence.Clear();
                    txtoutput.Clear();
                }
            }
        }
        private void cbxCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCircle.SelectedIndex > 0)
            {
                var customerProfile = CommonClass.GetCustomerProfileList(Convert.ToInt32(cbxCustomer.SelectedValue), Convert.ToInt32(cbxCircle.SelectedValue));

                if (customerProfile != null && customerProfile.Count > 0)
                {
                    customerProfile.Insert(0, new CustomerProfile
                    {
                        ProfileID = 0,
                        ProfileName = "----Select----"
                    });
                    cbxProfile.DataSource = customerProfile;
                    cbxProfile.DisplayMember = "ProfileName";
                    cbxProfile.ValueMember = "ProfileID";
                }
                else
                {
                    cbxProfile.DataSource = null;
                }
            }
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            angle = (angle + 10) % 360;
            Invalidate(true);
        }

        private void BufferingPanel_Paint(object sender, PaintEventArgs e)
        {
            DrawBufferingCircle(e.Graphics, ((Panel)sender).ClientRectangle, angle);
        }

        private void DrawBufferingCircle(Graphics g, Rectangle bounds, int angle)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int circleRadius = Math.Min(bounds.Width, bounds.Height) / 2 - 10;
            Point center = new Point(bounds.Width / 2, bounds.Height / 2);
            int numSegments = 12;
            int segmentRadius = circleRadius / 6;

            for (int i = 0; i < numSegments; i++)
            {
                float segmentAngle = (360f / numSegments) * i + angle;
                double radians = segmentAngle * Math.PI / 180;
                Point segmentCenter = new Point(
                    center.X + (int)(Math.Cos(radians) * circleRadius),
                    center.Y + (int)(Math.Sin(radians) * circleRadius)
                );

                int alpha = (int)(255 * (i + 1) / (float)numSegments);
                using (Brush brush = new SolidBrush(Color.FromArgb(alpha, Color.Black)))
                {
                    g.FillEllipse(brush, segmentCenter.X - segmentRadius, segmentCenter.Y - segmentRadius, segmentRadius * 2, segmentRadius * 2);
                }
            }
        }

        private void StopBuffering()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= Timer_Tick;
                timer = null;
            }
            if (bufferingPanel != null)
            {
                bufferingPanel.Paint -= BufferingPanel_Paint;
                Controls.Remove(bufferingPanel);
                bufferingPanel.Dispose();
                bufferingPanel = null;
            }
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {

            customerID = Convert.ToInt32(cbxCustomer.SelectedValue);
            circleID = Convert.ToInt32(cbxCircle.SelectedValue);
            ProfileID = Convert.ToInt32(cbxProfile.SelectedValue);
            inputFile = txtInputfile.Text.Trim();
            licenceFile = txtLicence.Text.Trim();
            customer = cbxCustomer.Text;
            circle = cbxCircle.Text;
            profile = cbxProfile.Text;
            panel1.Visible = true;
            backgroundWorker1.RunWorkerAsync();

        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            startProcessing();
            this.Invoke(new MethodInvoker(delegate
            {
                panel1.Visible = false;
            }));

        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //StopBuffering();
        }
        public void startProcessing()
        {
            try
            {
                if (customerID > 0 && circleID > 0 && ProfileID > 0 && !string.IsNullOrEmpty(inputFile) && !string.IsNullOrEmpty(licenceFile))
                {
                    //customerID = Convert.ToInt32(cbxCustomer.SelectedValue);
                    //circleID = Convert.ToInt32(cbxCircle.SelectedValue);
                    //ProfileID = Convert.ToInt32(cbxProfile.SelectedValue);

                    //looging
                    logString.Append("\n5. Importing files with no duplicate filename and ICCIDs into the Database.\n");
                    Console.WriteLine($"\n5. Importing files with no duplicate filename and ICCIDs into the Database.\n");
                    int lotid = 0;
                    string[] filenames = inputFile.Split(',');
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        SqlDataReader reader = null;
                        using (SqlCommand cmd = new SqlCommand("usp_SaveFileLot", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@customerID", customerID);
                            cmd.Parameters.AddWithValue("@circleID", circleID);
                            cmd.Parameters.AddWithValue("@profileID", ProfileID);
                            cmd.Parameters.AddWithValue("@qty", filenames.Length);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                lotid = Convert.ToInt32(reader["SavedID"]);
                            }
                        }
                    }
                    if (lotid > 0)
                    {
                        int i = 0;
                        if (filenames.Length > 1)
                        {
                            IsSingle = false;
                        }
                        logString.Append("\n6. Files Uploading and Processing Started:\n");
                        Console.WriteLine($"\n6. Files Uploading and Processing Started:\n");

                        foreach (string filename in filenames)
                        {
                            logString.Append($"    - Uploading [{Path.GetFileName(filename)}].\n");
                            Console.WriteLine($"    - Uploading [{Path.GetFileName(filename)}].\n");
                            this.Invoke(new MethodInvoker(delegate
                            {
                                txtInputfile.Text = filename;
                            }));

                            using (SqlConnection con = new SqlConnection(connectionString))
                            {
                                con.Open();
                                SqlDataReader reader = null;
                                using (SqlCommand cmd = new SqlCommand("usp_SaveDataGenProcessFiles", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@custId", customerID);
                                    cmd.Parameters.AddWithValue("@circleID", circleID);
                                    cmd.Parameters.AddWithValue("@custProfileID", ProfileID);
                                    cmd.Parameters.AddWithValue("@statusID", 1);
                                    cmd.Parameters.AddWithValue("@createdBY", NewLogin.primaryId);
                                    cmd.Parameters.AddWithValue("@lot", lotid);

                                    reader = cmd.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        lastInsertedId = Convert.ToInt32(reader["ID"]);
                                    }
                                }
                                con.Close();
                            }
                            if (lastInsertedId > 0)
                            {
                                InsertedHDIDS.Add(lastInsertedId);
                                saveDataGetProcessHDfiles(lastInsertedId, lotid, filename);
                                i += btnImport_Click(lastInsertedId, lotid);
                                logString.Append($"    - [{Path.GetFileName(filename)}] Uploaded Sucessfully with FileID {lastInsertedId}.\n");
                                Console.WriteLine($"    - [{Path.GetFileName(filename)}] Uploaded Sucessfully with FileID {lastInsertedId}.\n");

                            }

                        }
                        if (i == 0)
                        {

                            MessageBox.Show($"No file Imported successfully with {lotid} as LotID ",
                                                   "Message",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Information
                                                   );
                        }
                        else if (i == filenames.Length)
                        {
                            using (SqlConnection con = new SqlConnection(connectionString))
                            {
                                con.Open();
                                using (SqlCommand cmd = new SqlCommand($"UPDATE FileLotMaster SET [DataGenProcessStatus]= 1 WHERE ID = {lotid};UPDATE DataGenProcessHD SET DataGenProcessStatus=5,DataGenProcessDate=GETDATE() WHERE lot={lotid};", con))
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            if (File.Exists(licenceFile))
                            {
                                string status = Importlicencefile(lotid);
                                if (status != "")
                                {
                                    MessageBox.Show(status);
                                }
                                else
                                {
                                    int j = 0;
                                    foreach (int hdid in InsertedHDIDS)
                                    {
                                        lastInsertedId = hdid;
                                        Console.WriteLine(lastInsertedId);
                                        int test_data_validation = btnProcessAll_Click();
                                        if (test_data_validation == 10)
                                        {
                                            throw new InvalidOperationException("HSM error");
                                        }

                                        else
                                        {
                                            j += test_data_validation;
                                        }
                                    }
                                    if (j < filenames.Length)
                                    {
                                        DialogResult result = MessageBox.Show($"{i}/{filenames.Length} File uploaded & Processed successfully.\nWant to proceed further if not the data will be deleted from database.",
                                                 "Message",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Information
                                                 );

                                        if (result == DialogResult.No)
                                        {
                                            using (SqlConnection con = new SqlConnection(connectionString))
                                            {
                                                con.Open();
                                                SqlDataReader reader = null;
                                                using (SqlCommand cmd = new SqlCommand($"Delete FROM [dbo].[DataGenProcessDataRecord] WHERE DataGenProcessHDID IN (SELECT Distinct DataGenProcessHDID FROM [DataGenProcessHDFile] WHERE FileLotID={lotid});", con))
                                                {
                                                    int rowsAffected = cmd.ExecuteNonQuery();

                                                    if (rowsAffected > 0)
                                                    {
                                                        MessageBox.Show("Data deleted successfully.");
                                                        logString.Append($"\n6. Data deleted successfully from database.\n");
                                                        Console.WriteLine($"\n6. Data deleted successfully from database.\n");
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Database Error.");
                                                        logString.Append($"\n6. Unable to delete uploaded Files with {lotid} lot.\n");
                                                        Console.WriteLine($"\n6. Unable to delete uploaded Files with {lotid} lot.\n");
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            logString.Append($"\n6. {i}/{filenames.Length} Files successfully imported & Processed.\n");
                                            Console.WriteLine($"\n6. {i}/{filenames.Length} Files successfully imported & Processed.\n");
                                            logString.Append($"\n7. Generating summary report with file lot ID:\n");
                                            Console.WriteLine($"\n7. Generating summary report with file lot ID:\n");
                                            logString.Append($"    - Lot ID: {lotid}\n");
                                            Console.WriteLine($"    - Lot ID: {lotid}\n");
                                            logString.Append($"    - Total files processed: : {total_pro_file}\n");
                                            Console.WriteLine($"    - Total files processed: : {total_pro_file}\n");
                                            logString.Append($"    - Total files imported:  {i}\n");
                                            Console.WriteLine($"    - Total files imported:  {i}\n");
                                            logString.Append($"    - Total duplicate files:  {total_dup_file}\n");
                                            Console.WriteLine($"    - Total duplicate files:  {total_dup_file}\n");
                                            logString.Append($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                            Console.WriteLine($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                            MessageBox.Show("you can procceed to processing with remaining files.");
                                            FileProcessingLotID = lotid;
                                        }

                                    }
                                    else if (j == 0)
                                    {
                                        using (SqlConnection con = new SqlConnection(connectionString))
                                        {
                                            con.Open();
                                            using (SqlCommand cmd = new SqlCommand($"UPDATE FileLotMaster SET [DataGenProcessStatus]= 16 WHERE ID = {lotid}", con))
                                            {
                                                cmd.CommandType = CommandType.Text;
                                                cmd.ExecuteNonQuery();
                                            }
                                        }

                                        MessageBox.Show($"No file Processed successfully with {lotid} as LotID ",
                                                               "Message",
                                                               MessageBoxButtons.OK,
                                                               MessageBoxIcon.Information
                                                               );
                                        logString.Append("\n6. No file Processed successfully:\n");
                                        Console.WriteLine($"\n6. No file Processed successfully:\n");
                                        logString.Append("\n7. Generating summary report with file lot ID:\n");
                                        Console.WriteLine($"\n7. Generating summary report with file lot ID:\n");
                                        logString.Append($"    - Lot ID: {lotid}\n");
                                        Console.WriteLine($"    - Lot ID: {lotid}\n");
                                        logString.Append($"    - Total files processed: : {total_pro_file}\n");
                                        Console.WriteLine($"    - Total files processed: : {total_pro_file}\n");
                                        logString.Append($"    - Total files imported:  {i}\n");
                                        Console.WriteLine($"    - Total files imported:  {i}\n");
                                        logString.Append($"    - Total duplicate files:  {total_dup_file}\n");
                                        Console.WriteLine($"    - Total duplicate files:  {total_dup_file}\n");
                                        logString.Append($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                        Console.WriteLine($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                    }
                                    else
                                    {
                                        using (SqlConnection con = new SqlConnection(connectionString))
                                        {
                                            con.Open();
                                            using (SqlCommand cmd = new SqlCommand("UPDATE FileLotMaster SET [DataGenProcessStatus]= 15 WHERE ID = (SELECT MAX(ID) FROM FileLotMaster)", con))
                                            {
                                                cmd.CommandType = CommandType.Text;
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                        this.Invoke(new MethodInvoker(delegate
                                        {
                                            txtoutput.Text += $"Files Processed successfully with {lotid} as LotID. \r\n";
                                        }));

                                        //MessageBox.Show($"Files uploaded successfully with {lotid} as LotID ",
                                        //                       "Message",
                                        //                       MessageBoxButtons.OK,
                                        //                       MessageBoxIcon.Information
                                        //                       );

                                        logString.Append("\n7. Generating summary report with file lot ID:\n");
                                        Console.WriteLine($"\n7. Generating summary report with file lot ID:\n");
                                        logString.Append($"    - Lot ID: {lotid}\n");
                                        Console.WriteLine($"    - Lot ID: {lotid}\n");
                                        logString.Append($"    - Total files processed: : {total_pro_file}\n");
                                        Console.WriteLine($"    - Total files processed: : {total_pro_file}\n");
                                        logString.Append($"    - Total files imported:  {i}\n");
                                        Console.WriteLine($"    - Total files imported:  {i}\n");
                                        logString.Append($"    - Total duplicate files:  {total_dup_file}\n");
                                        Console.WriteLine($"    - Total duplicate files:  {total_dup_file}\n");
                                        FileProcessingLotID = lotid;
                                    }
                                }
                            }
                            else
                            {
                                int j = 0;
                                foreach (int hdid in InsertedHDIDS)
                                {
                                    lastInsertedId = hdid;
                                    Console.WriteLine($"last hdid : " + lastInsertedId);

                                    int test_data_validation_1 = btnProcessAll_Click();
                                    if (test_data_validation_1 == 10)
                                    {
                                        throw new InvalidOperationException("HSM error Connectivity Lost!!!");
                                    }

                                    else
                                    {
                                        j += test_data_validation_1;
                                    }

                                    //j += btnProcessAll_Click();
                                }
                                if (j < filenames.Length)
                                {
                                    DialogResult result = MessageBox.Show($"{i}/{filenames.Length} File uploaded & Processed successfully.\nWant to proceed further if not the data will be deleted from database.",
                                             "Message",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Information
                                             );

                                    if (result == DialogResult.No)
                                    {
                                        using (SqlConnection con = new SqlConnection(connectionString))
                                        {
                                            con.Open();
                                            SqlDataReader reader = null;
                                            using (SqlCommand cmd = new SqlCommand($"Delete FROM [dbo].[DataGenProcessDataRecord] WHERE DataGenProcessHDID IN (SELECT Distinct DataGenProcessHDID FROM [DataGenProcessHDFile] WHERE FileLotID={lotid});", con))
                                            {
                                                int rowsAffected = cmd.ExecuteNonQuery();

                                                if (rowsAffected > 0)
                                                {
                                                    MessageBox.Show("Data deleted successfully.");
                                                    logString.Append($"\n6. Data deleted successfully from database.\n");
                                                    Console.WriteLine($"\n6. Data deleted successfully from database.\n");
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Database Error.");
                                                    logString.Append($"\n6. Unable to delete uploaded Files with {lotid} lot.\n");
                                                    Console.WriteLine($"\n6. Unable to delete uploaded Files with {lotid} lot.\n");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        logString.Append($"\n6. {i}/{filenames.Length} Files successfully imported & Processed.\n");
                                        Console.WriteLine($"\n6. {i}/{filenames.Length} Files successfully imported & Processed.\n");
                                        logString.Append("\n7. Generating summary report with file lot ID:\n");
                                        Console.WriteLine($"\n7. Generating summary report with file lot ID:\n");
                                        logString.Append($"    - Lot ID: {lotid}\n");
                                        Console.WriteLine($"    - Lot ID: {lotid}\n");
                                        logString.Append($"    - Total files processed: : {total_pro_file}\n");
                                        Console.WriteLine($"    - Total files processed: : {total_pro_file}\n");
                                        logString.Append($"    - Total files imported:  {i}\n");
                                        Console.WriteLine($"    - Total files imported:  {i}\n");
                                        logString.Append($"    - Total duplicate files:  {total_dup_file}\n");
                                        Console.WriteLine($"    - Total duplicate files:  {total_dup_file}\n");
                                        logString.Append($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                        Console.WriteLine($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                        MessageBox.Show("you can procceed to processing with remaining files.");
                                        FileProcessingLotID = lotid;
                                    }

                                }
                                else if (j == 0)
                                {
                                    using (SqlConnection con = new SqlConnection(connectionString))
                                    {
                                        con.Open();
                                        using (SqlCommand cmd = new SqlCommand($"UPDATE FileLotMaster SET [DataGenProcessStatus]= 16 WHERE ID = {lotid}", con))
                                        {
                                            cmd.CommandType = CommandType.Text;
                                            cmd.ExecuteNonQuery();
                                        }
                                    }

                                    MessageBox.Show($"No file Processed successfully with {lotid} as LotID ",
                                                           "Message",
                                                           MessageBoxButtons.OK,
                                                           MessageBoxIcon.Information
                                                           );
                                    logString.Append("\n6. No file Processed successfully:\n");
                                    Console.WriteLine($"\n6. No file Processed successfully:\n");
                                    logString.Append("\n7. Generating summary report with file lot ID:\n");
                                    Console.WriteLine($"\n7. Generating summary report with file lot ID:\n");
                                    logString.Append($"    - Lot ID: {lotid}\n");
                                    Console.WriteLine($"    - Lot ID: {lotid}\n");
                                    logString.Append($"    - Total files processed: : {total_pro_file}\n");
                                    Console.WriteLine($"    - Total files processed: : {total_pro_file}\n");
                                    logString.Append($"    - Total files imported:  {i}\n");
                                    Console.WriteLine($"    - Total files imported:  {i}\n");
                                    logString.Append($"    - Total duplicate files:  {total_dup_file}\n");
                                    Console.WriteLine($"    - Total duplicate files:  {total_dup_file}\n");
                                    logString.Append($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                    Console.WriteLine($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                }
                                else
                                {
                                    using (SqlConnection con = new SqlConnection(connectionString))
                                    {
                                        con.Open();
                                        using (SqlCommand cmd = new SqlCommand("UPDATE FileLotMaster SET [DataGenProcessStatus]= 15 WHERE ID = (SELECT MAX(ID) FROM FileLotMaster)", con))
                                        {
                                            cmd.CommandType = CommandType.Text;
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        txtoutput.Text += $"Files Processed successfully with {lotid} as LotID. \r\n";
                                    }));

                                    //MessageBox.Show($"Files uploaded successfully with {lotid} as LotID ",
                                    //                       "Message",
                                    //                       MessageBoxButtons.OK,
                                    //                       MessageBoxIcon.Information
                                    //                       );

                                    logString.Append("\n7. Generating summary report with file lot ID:\n");
                                    Console.WriteLine($"\n7. Generating summary report with file lot ID:\n");
                                    logString.Append($"    - Lot ID: {lotid}\n");
                                    Console.WriteLine($"    - Lot ID: {lotid}\n");
                                    logString.Append($"    - Total files processed: : {total_pro_file}\n");
                                    Console.WriteLine($"    - Total files processed: : {total_pro_file}\n");
                                    logString.Append($"    - Total files imported:  {i}\n");
                                    Console.WriteLine($"    - Total files imported:  {i}\n");
                                    logString.Append($"    - Total duplicate files:  {total_dup_file}\n");
                                    Console.WriteLine($"    - Total duplicate files:  {total_dup_file}\n");
                                    FileProcessingLotID = lotid;
                                }
                            }

                        }
                    }


                }
                else
                {
                    MessageBox.Show("All fields are required: ",
                                                "Error",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information
                                                );
                    logString.Append("\nAll fields are required: \n");
                    Console.WriteLine($"\nAll fields are required: \n");
                }
                if (FileProcessingLotID > 0)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        SqlDataReader reader = null;
                        logString.Append("8. Outfile Generation Started:\n");
                        Console.WriteLine($"8. Outfile Generation Started:\n");
                        using (SqlCommand cmd = new SqlCommand("SELECT DataGenProcessHDID FROM DataGenProcessHD WHERE lot=@lotid AND DataGenProcessStatus=2", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@lotid", FileProcessingLotID);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                lastInsertedId = Convert.ToInt32(reader.GetInt32(0));
                                if (lastInsertedId > 0)
                                {
                                    btnGenerateAllFiles_Click();
                                }
                            }
                        }
                    }
                }
                this.Invoke(new MethodInvoker(delegate
                {
                    txtInputfile.Text = "";
                    txtLicence.Text = "";
                }));
                MessageBox.Show($"File Generated for LotID : {FileProcessingLotID}");
                logString.Append($"\n**************************************[Logging Out] Data Processing is Completed [{DateTime.Now}] **************************************\n");
                Console.WriteLine($"\n**************************************[Logging Out] Data Processing is Completed [{DateTime.Now}] **************************************\n");


            }
            catch (Exception ex)
            {
                logString.Append("\nSomething went wrong during importation: " + ex.Message);
                Console.WriteLine($"\nSomething went wrong during importation: ");
                logString.Append($"\n**************************************[Logging Out] Data Processing is Failed  [{DateTime.Now}]**************************************\n");
                Console.WriteLine($"\n**************************************[Logging Out] Data Processing is Failed  [{DateTime.Now}]**************************************\n");

                Console.WriteLine($"Something went wrong during importation Exception: " + ex.Message);
                Console.WriteLine($"Something went wrong during importation Stack trace: " + ex.StackTrace);
                MessageBox.Show(ex.Message);

            }
            finally
            {
                if (!Directory.Exists(log_dir + "/Logging"))
                {
                    Directory.CreateDirectory(log_dir + "/Logging");
                }
                System.IO.File.AppendAllText(log_dir + "/Logging/" + $"{DateTime.Now.ToString("dd-MM-yyyy")}_log.txt", logString.ToString());
                upload_log();
                logString.Clear();
                logString.Append($"\n********************************* Data Processing Started [{DateTime.Now}] USERNAME:{NewLogin.username} SYSTEM NAME : {Environment.MachineName} *************************************\n");
                Console.WriteLine($"\n********************************* Data Processing Started [{DateTime.Now}] USERNAME:{NewLogin.username} SYSTEM NAME : {Environment.MachineName} *************************************\n");

            }

        }
        public void upload_log()
        {
            try
            {
                if (EncryptDB == "1")
                {
                    // Encrypt logstring data
                }
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("insert into [DataTool_log]([Date],[LogMsg],[User_name]) Values (@Date, @LogMsg, @User_name)", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        //cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("dd-MM-yyyy"));
                        cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@LogMsg", logString.ToString());
                        cmd.Parameters.AddWithValue("@User_name", LoginPage.username);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in log uploadation : " + ex.Message);
            }
        }


        public int btnImport_Click(int hdid, int lot)
        {
            string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlConnection con = new SqlConnection(constr);
            string filename_2 = getfilenameandid();
            String Query0 = "SELECT * FROM InPutDataTemplate WHERE CustID = " + OFProcessing.customerID + " and ProfileID =" + OFProcessing.ProfileID + " and  trim(VarText) = 'FL' and isnull(LineNumber,0)!=0  order by VarName ";
            System.Data.DataTable dt0 = new System.Data.DataTable();
            DataRow workRow0;
            SqlDataAdapter adpt0 = new SqlDataAdapter(Query0, con);
            adpt0.Fill(dt0);
            DataTable resultDataTable = new DataTable();
            resultDataTable.Columns.Add("ICICIDHEX", typeof(string));
            resultDataTable.Columns.Add("IMSIHEX", typeof(string));
            resultDataTable.Columns.Add("MSISDNHEX", typeof(string));
            resultDataTable.Columns.Add("ICICID", typeof(string));
            resultDataTable.Columns.Add("IMSI", typeof(string));
            resultDataTable.Columns.Add("MSISDN", typeof(string));
            resultDataTable.Columns.Add("lot", typeof(int)).DefaultValue = lot;
            resultDataTable.Columns.Add("DataGenProcessHDID", typeof(int)).DefaultValue = hdid;
            resultDataTable.Columns.Add("CustID", typeof(int)).DefaultValue = customerID;
            resultDataTable.Columns.Add("ProID", typeof(int)).DefaultValue = ProfileID;
            int imsi_line_no = 0, msisdn_line_no = 0, line_no = 0;
            int iccid_line_no = 0;
            int qty_line_no = 0;
            int qty_frm = 0;
            int qty_len = 0;
            int iccid_frm = 0;
            int iccid_len = 0;
            int imsi_from = 0;
            int imsi_len = 0;
            int msisdn_from = 0;
            int msisdn_len = 0;
            bool IsIncremental = false;
            
            string[] data_val_line = File.ReadAllLines(filename_2);
            string var_des = "";
            foreach (DataRow dv0 in dt0.Rows)
            {
                var_des = dv0[5].ToString().Trim();
                string Var_Text = dv0[7].ToString().Trim();
                string line_sql = dv0[11].ToString().Trim();
                string Pos_From = dv0[13].ToString().Trim();
                string len_data = dv0[15].ToString().Trim();
                string tag = dv0[16].ToString().Trim();
                if (Var_Text.TrimEnd() == "FL")
                {
                    if (var_des == "ICCID")
                    {
                        iccid_frm = Convert.ToInt32(Pos_From);
                        iccid_len = Convert.ToInt32(len_data);
                        iccid_line_no = Convert.ToInt32(line_sql);
                        if (!string.IsNullOrEmpty(tag))
                        {
                            IsIncremental = true;
                        }
                        else { line_no = Convert.ToInt32(line_sql); }
                    }
                    else if (var_des == "IMSI")
                    {
                        imsi_from = Convert.ToInt32(Pos_From);
                        imsi_len = Convert.ToInt32(len_data);
                        imsi_line_no = Convert.ToInt32(line_sql);
                        if (!string.IsNullOrEmpty(tag))
                        {
                            IsIncremental = true;
                        }
                    }
                    else if (var_des == "MSISDN")
                    {
                        msisdn_from = Convert.ToInt32(Pos_From);
                        msisdn_len = Convert.ToInt32(len_data);
                        msisdn_line_no = Convert.ToInt32(line_sql);
                    }
                    else if (var_des == "Quantity")
                    {
                        qty_line_no = Convert.ToInt32(line_sql);
                        qty_frm = Convert.ToInt32(Pos_From);
                        qty_len = Convert.ToInt32(len_data);
                    }
                }
            }
            if (IsIncremental)
            {
                try
                {

                    int qty = 0;
                    long iccid = 0;
                    long imsi = 0;
                    long msisdn = 0;
                    //StreamReader sr = new StreamReader(filename_2);
                    int line_number = 1;
                    string line;
                    //string data_val_line = sr.ReadToEnd();
                    //while ((line = sr.ReadLine()) != null)
                    //{
                    //    if (line_number == qty_line_no)
                    //    {
                    //        qty = Convert.ToInt32(line.Substring(qty_frm, qty_len).Trim());
                    //        break;

                    //    }
                    //    line_number++;
                    //}
                    try
                    {
                        qty = Convert.ToInt32(data_val_line[qty_line_no - 1].Substring(qty_frm, qty_len).Trim());
                    }
                    catch
                    {

                        string line_test = data_val_line[qty_line_no - 1];
                        int safeLen = Math.Min(qty_len, line_test.Length - qty_frm);
                        string qtyStr = line_test.Substring(qty_frm, safeLen).Trim();
                        qty = Convert.ToInt32(qtyStr);
                    }
                    //data_val_line = data_val_line.Split('\n')[imsi_line_no - 1];

                    try
                    {
                        iccid = Convert.ToInt64(data_val_line[iccid_line_no - 1].Substring(iccid_frm, iccid_len).Trim());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to read ICCID frim input file Linenumber:{iccid_line_no} , position from : {iccid_frm} , length : {iccid_len}");
                        throw;
                    }

                    try
                    {
                        imsi = Convert.ToInt64(data_val_line[imsi_line_no - 1].Substring(imsi_from, imsi_len).Trim());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to read ICCID frim input file Linenumber:{imsi_line_no} , position from : {imsi_from} , length : {imsi_len}");
                        throw;

                    }


                    try
                    {
                        if (msisdn_from != 0 && msisdn_len != 0)
                        {
                            msisdn = Convert.ToInt64(data_val_line[msisdn_line_no - 1].Substring(msisdn_from, msisdn_len).Trim());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to read ICCID frim input file Linenumber:{imsi_line_no} , position from : {imsi_from} , length : {imsi_len}");
                        throw;

                    }


                    for (int i = 0; i < qty; i++)
                    {
                        if (msisdn != 0)
                        {
                            resultDataTable.Rows.Add(StringToHex(iccid.ToString()).Trim(), StringToHex(imsi.ToString()).Trim(), StringToHex(msisdn.ToString()).Trim(), iccid.ToString(), imsi.ToString(), msisdn.ToString());
                            iccid += 1;
                            imsi += 1;
                            msisdn += 1;

                        }
                        else
                        {
                            resultDataTable.Rows.Add(StringToHex(iccid.ToString()).Trim(), StringToHex(imsi.ToString()).Trim(), "", iccid.ToString(), imsi.ToString(), "");
                            iccid += 1;
                            imsi += 1;
                        }

                    }

                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error During Importation : " + ex.Message);
                    string error = $"Error During Importation in file '{Path.GetFileName(filename_2)}' : " + ex.Message;
                    logString.Append($"**{error}**\n");
                    Console.WriteLine($"**{error}**\n");
                    using (SqlConnection con1 = new SqlConnection(constr))
                    {
                        con1.Open();
                        using (SqlCommand cmd1 = new SqlCommand($"DELETE FROM [DGPDR_Base] WHERE  DataGenProcessHDID={lastInsertedId};", con1))
                        {
                            int rowsAffected = cmd1.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                logString.Append($"- '{Path.GetFileName(filename_2)}' file data deleted successfully from database.\n");
                                Console.WriteLine($"- '{Path.GetFileName(filename_2)}' file data deleted successfully from database.\n");
                            }
                            else
                            {
                                logString.Append($"- Unable to delete '{Path.GetFileName(filename_2)}' file data.\n");
                                Console.WriteLine($"- Unable to delete '{Path.GetFileName(filename_2)}' file data.\n");
                            }
                        }
                    }
                    return 0;

                }
            }
            else
            {
                try
                {
                    StreamReader sr = new StreamReader(filename_2);
                    int line_number = 1;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line_number >= line_no)
                        {
                            if (msisdn_from == 0 && msisdn_len == 0)
                            {
                                resultDataTable.Rows.Add(StringToHex(line.Substring(iccid_frm, iccid_len).Trim()), StringToHex(line.Substring(imsi_from, imsi_len).Trim()), "", line.Substring(iccid_frm, iccid_len).Trim(), line.Substring(imsi_from, imsi_len).Trim(), "");
                            }
                            else
                            {
                                string msisdn_data = "";
                                try
                                {
                                    msisdn_data = line.Substring(msisdn_from, msisdn_len).Trim();

                                }
                                catch
                                {
                                    for (int i = 0; i < msisdn_len; i++)
                                    {
                                        msisdn_data += "F";
                                    }
                                }
                                resultDataTable.Rows.Add(StringToHex(line.Substring(iccid_frm, iccid_len).Trim()), StringToHex(line.Substring(imsi_from, imsi_len).Trim()), StringToHex(msisdn_data), line.Substring(iccid_frm, iccid_len).Trim(), line.Substring(imsi_from, imsi_len).Trim(), msisdn_data);


                            }

                        }
                        line_number++;
                    }
                    sr.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error During Importaion : " + ex.Message);
                    string error = $"Error During Importaion in file '{Path.GetFileName(filename_2)}' : " + ex.Message;
                    logString.Append($"**{error}**\n");
                    Console.WriteLine($"**{error}**\n");
                    using (SqlConnection con1 = new SqlConnection(constr))
                    {
                        con1.Open();
                        using (SqlCommand cmd1 = new SqlCommand($"DELETE FROM [DGPDR_Base] WHERE  DataGenProcessHDID={lastInsertedId};", con1))
                        {
                            int rowsAffected = cmd1.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                logString.Append($"- '{Path.GetFileName(filename_2)}' file data deleted successfully from database.\n");
                                Console.WriteLine($"- '{Path.GetFileName(filename_2)}' file data deleted successfully from database.\n");
                            }
                            else
                            {
                                logString.Append($"- Unable to delete '{Path.GetFileName(filename_2)}' file data.\n");
                                Console.WriteLine($"- Unable to delete '{Path.GetFileName(filename_2)}' file data.\n");
                            }
                        }
                    }
                    return 0;
                }
            }
            try
            {
                
                con.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                {
                    bulkCopy.DestinationTableName = "DGPDR_Base";
                    bulkCopy.ColumnMappings.Add("ICICID", "ICICID");
                    bulkCopy.ColumnMappings.Add("IMSI", "IMSI");
                    bulkCopy.ColumnMappings.Add("MSISDN", "MSISDN");
                    bulkCopy.ColumnMappings.Add("lot", "lot");
                    bulkCopy.ColumnMappings.Add("DataGenProcessHDID", "DataGenProcessHDID");
                    bulkCopy.WriteToServer(resultDataTable);
                }
                con.Close();
                con.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                {
                    bulkCopy.DestinationTableName = "DupCheck";
                    bulkCopy.ColumnMappings.Add("ICICIDHEX", "ICCID");
                    bulkCopy.ColumnMappings.Add("IMSIHEX", "IMSI");
                    bulkCopy.ColumnMappings.Add("MSISDNHEX", "MSISDN");
                    bulkCopy.ColumnMappings.Add("CustID", "CustID");
                    bulkCopy.ColumnMappings.Add("ProID", "CustProfileID");
                    bulkCopy.ColumnMappings.Add("DataGenProcessHDID", "C1");
                    bulkCopy.WriteToServer(resultDataTable);
                }
                con.Close();
                using (SqlConnection con11 = new SqlConnection(constr))
                {
                    con11.Open();
                    using (SqlCommand cmd1 = new SqlCommand($"UPDATE DataGenProcessHD SET StatusID=1 WHERE  DataGenProcessHDID={lastInsertedId};", con11))
                    {
                        int rowsAffected = cmd1.ExecuteNonQuery();
                    }
                }
                string r4_data = "", r8_data = "";
                int r4_data_count = 0, r8_data_count = 0, records_no=0;

                string constr1 = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                SqlConnection con1 = new SqlConnection(constr);
                int list_4 = 0, list_8 = 0;
                String Query01 = "SELECT * FROM InPutDataTemplate WHERE CustID = " + OFProcessing.customerID + " and ProfileID =" + OFProcessing.ProfileID + " order by VarName";
                System.Data.DataTable dt01 = new System.Data.DataTable();
                DataRow workRow01;
                SqlDataAdapter adpt01 = new SqlDataAdapter(Query01, con1);
                adpt01.Fill(dt01);

                DataTable Process_data = new DataTable();
                Process_data.Columns.Add("Variable", typeof(string));
                Process_data.Columns.Add("Name", typeof(string));
                Process_data.Columns.Add("Value", typeof(string));
                int frm = 0;
                int len = 0;
                line_no = 0;
                var_des = "";
                
                foreach (DataRow dv0 in dt01.Rows)
                {
                    records_no += 1;
                    string var_name = dv0[3].ToString().Trim();
                    Console.WriteLine($"inserting all data : " + var_name);

                    string var_Value = dv0[4].ToString().Trim();
                    var_des = dv0[5].ToString().Trim();
                    string var_type = dv0[6].ToString().Trim();

                    string Var_Text = dv0[7].ToString().Trim();
                    string Algo_Name = dv0[9].ToString().Trim();
                    string var_algoname = dv0[9].ToString().Trim();
                    string line_sql = dv0[11].ToString().Trim();
                    string File_ID = dv0[10].ToString().Trim();
                    string Pos_From = dv0[13].ToString().Trim();
                    string len_data = dv0[15].ToString().Trim();
                    string tag_value = dv0[16].ToString().TrimEnd();
                    String line1;
                    string myData = "";

                    try
                    {
                        if (var_des == "OutFile_Header")
                        {
                            myData = string.Join("\r\n", File.ReadLines(filename_2).Take(Convert.ToInt32(line_sql))) + "\r\n";
                            using (SqlConnection con0 = new SqlConnection(connectionString))
                            {
                                SqlDataReader reader = null;
                                using (SqlCommand cmd = new SqlCommand("usp_Insert_first_record", con0))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@DataGenProcessHDID", lastInsertedId);
                                    cmd.Parameters.AddWithValue("@VarID", var_name.TrimEnd());
                                    cmd.Parameters.AddWithValue("@VarName", var_des.TrimEnd());
                                    cmd.Parameters.AddWithValue("@VarValue", myData);
                                    cmd.Parameters.AddWithValue("@VarType", var_type.TrimEnd());
                                    cmd.Parameters.AddWithValue("@StatusID", "");
                                    try
                                    {
                                        con0.Open();
                                        reader = cmd.ExecuteReader();
                                        con0.Close();
                                    }
                                    catch (Exception exe)
                                    {
                                        MessageBox.Show(exe.Message);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Var_Text.TrimEnd() == "FL")
                            {
                                if (var_des == "LICENSE_KEY")
                                {
                                    myData = var_Value;
                                    using (SqlConnection con0 = new SqlConnection(connectionString))
                                    {
                                        SqlDataReader reader = null;
                                        using (SqlCommand cmd = new SqlCommand("usp_Insert_first_record", con0))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@DataGenProcessHDID", lastInsertedId);
                                            cmd.Parameters.AddWithValue("@VarID", var_name.TrimEnd());
                                            cmd.Parameters.AddWithValue("@VarName", var_des.TrimEnd());
                                            cmd.Parameters.AddWithValue("@VarValue", myData.TrimEnd());
                                            cmd.Parameters.AddWithValue("@VarType", var_type.TrimEnd());
                                            cmd.Parameters.AddWithValue("@StatusID", "");
                                            try
                                            {
                                                con0.Open();
                                                reader = cmd.ExecuteReader();
                                                con0.Close();
                                            }
                                            catch (Exception exe)
                                            {
                                                MessageBox.Show(exe.Message);
                                            }
                                        }
                                    }
                                }
                                StreamReader sr1 = new StreamReader(filename_2);

                                string strPath = filename_2;

                                string filename = null;
                                filename = Path.GetFileName(strPath);

                                line1 = sr1.ReadLine();

                                int line_number1 = 1;

                                while (line1 != null)
                                {

                                    if (line_number1.ToString().Equals(line_sql))
                                    {
                                        try
                                        {
                                            myData = line1.Substring(int.Parse(Pos_From), int.Parse(len_data));
                                        }
                                        catch
                                        {
                                            myData = line1.Substring(int.Parse(Pos_From), line1.Length - int.Parse(Pos_From));
                                        }
                                        if (var_des == "ICCID")
                                        {
                                            first_icicid = myData;
                                        }
                                        else if (var_des == "IMSI")
                                        {
                                            first_imsi = myData;
                                        }
                                        else if (var_des == "MSISDN")
                                        {
                                            if (string.IsNullOrEmpty(myData.Trim()))
                                            {
                                                string fd = "";
                                                for (int i = 0; i < int.Parse(len_data); i++)
                                                {
                                                    fd += "F";
                                                }
                                                myData = fd;
                                            }
                                            first_msisdn = myData;
                                        }

                                        using (SqlConnection con0 = new SqlConnection(connectionString))
                                        {
                                            SqlDataReader reader = null;
                                            using (SqlCommand cmd = new SqlCommand("usp_Insert_first_record", con0))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@DataGenProcessHDID", lastInsertedId);
                                                cmd.Parameters.AddWithValue("@VarID", var_name.TrimEnd());
                                                cmd.Parameters.AddWithValue("@VarName", var_des.TrimEnd());
                                                cmd.Parameters.AddWithValue("@VarValue", myData.TrimEnd());
                                                cmd.Parameters.AddWithValue("@VarType", var_type.TrimEnd());
                                                cmd.Parameters.AddWithValue("@StatusID", "");
                                                try
                                                {
                                                    con0.Open();
                                                    reader = cmd.ExecuteReader();
                                                    con0.Close();
                                                }
                                                catch (Exception exe)
                                                {
                                                    MessageBox.Show(exe.Message);
                                                }
                                            }
                                        }
                                    }
                                    line1 = sr1.ReadLine();
                                    line_number1++;
                                }
                                sr1.Close();
                                Console.ReadLine();
                            }
                            if (Var_Text.TrimEnd() == "AL")
                            {
                                //string tag_value_1 = tag_value.Replace('[')
                                int varCount = tag_value.Count(c => c == ',');
                                string[] parts = Array.Empty<string>();
                                string caseSwitch = Algo_Name;



                                switch (caseSwitch)
                                {

                                    case "substring":
                                        if (varCount > 1)
                                        {
                                            //MessageBox.Show($"More than one Variable found in AlgoName-{Algo_Name}  in 'Tag' Value");
                                        }
                                        else
                                        {
                                            int pos_from = Convert.ToInt32(Pos_From);
                                            len = Convert.ToInt32(len_data);
                                            string data_new_test = Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString();
                                            //Console.WriteLine(data_new_test.TrimEnd() + " " + pos_from + " " + len);

                                            myData = data_new_test.Substring(pos_from - 1, len);
                                        }
                                        break;

                                    case "concat":
                                        myData = string.Join("", tag_value.Split(',').Select(t => Process_data.Select($"Variable = '{t.Trim()}'")[0]["Value"].ToString()));
                                        break;

                                    case "identical":

                                        myData = Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString();

                                        break;

                                    case "serial":
                                        myData = records_no.ToString();
                                        break;

                                    case "R_4":
                                        myData = Random4digits();
                                        break;

                                    case "R_8_H":
                                        myData = Random8hex();
                                        break;
                                    case "R4_PF":
                                        myData = padding_filler(Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString());
                                        break;

                                    case "R_8":
                                        myData = Random8digits();
                                        break;



                                    case "ACC_Hex":


                                        myData = acc(Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString());



                                        break;

                                    case "3P":

                                        myData = padding(Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString());

                                        break;

                                    case "HEX":
                                        //myData = padding((Int64.Parse(first_icicid) + i).ToString());
                                        myData = StringToHex(Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString());

                                        break;

                                    case "MSISDN_F":

                                        myData = Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString();
                                        myData = MSISDN_F(myData);

                                        break;

                                    case "NS":

                                        myData = Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString();
                                        myData = nibble_swapped(myData);

                                        break;

                                    case "R_16_Hex":


                                        myData = Create16DigitString();
                                        break;

                                    case "R_32_Hex":


                                        myData = Create32DigitString();
                                        break;

                                    case "R_48_Hex":


                                        myData = Create48DigitString();
                                        break;

                                    case "Pad_8":

                                        myData = Pad3_F(Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString());

                                        break;

                                    case "Pad_16":

                                        myData = Pad3_F(Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString());

                                        break;

                                    case "ICCID_NS":
                                        string icicid_num = Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString();
                                        myData = nibble_swapped(icicid_num);

                                        break;

                                    case "IMSI_NS":
                                        string imsi_num = "809" + Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString();
                                        myData = nibble_swapped(imsi_num);
                                        break;

                                    case "R_32_Hex_KI":
                                        myData = Create32DigitString();
                                        break;

                                    case "ICCID_LD":
                                        myData = Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString();

                                        myData += GetLuhnCheckDigit(myData);

                                        break;

                                    case "KCV_AES":
                                        myData = CalculateKCV(Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString(), "AES");
                                        break;

                                    case "KCV_DES":
                                        myData = CalculateKCV(Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString(), "DES");
                                        break;



                                    case "KI_AES_128":
                                        parts = tag_value.Split(',');
                                        // e.g. "KeyVar,DataVar"
                                        if (parts.Length >= 2)
                                        {
                                            string key = Process_data.Select($"Variable = '{parts[0].Trim()}'")[0]["Value"].ToString();
                                            string data = Process_data.Select($"Variable = '{parts[1].Trim()}'")[0]["Value"].ToString();

                                            myData = AES_ENCYPRTION(key, data);
                                        }
                                        else
                                        {
                                            throw new Exception($"KI_AES_128 requires 2 variables, but got: {tag_value}");
                                        }
                                        //myData = AES_ENCYPRTION(data1test, data2test);
                                        break;

                                    case "Single_Des":
                                        parts = tag_value.Split(',');
                                        if (parts.Length >= 2)
                                        {
                                            string key = Process_data.Select($"Variable = '{parts[0].Trim()}'")[0]["Value"].ToString();
                                            string data = Process_data.Select($"Variable = '{parts[1].Trim()}'")[0]["Value"].ToString();

                                            myData = Encrypt_SingleDES(key, data);
                                        }
                                        else
                                        {
                                            throw new Exception($"Single_Des requires 2 variables, but got: {tag_value}");
                                        }
                                        break;

                                    ////case "KI_AES_128_2":
                                    ////    if (varCount == 1)
                                    ////    {
                                    ////        //MessageBox.Show($"More than one Variable found in Algoname-{Algo_Name}  in 'Tag' Value");
                                    ////        string[] varIDs = tag_value.Split(',');
                                    ////        Console.WriteLine($"{newRow_firstrecords[varIDs[0]].ToString()} --->  {newRow_firstrecords[varIDs[1]].ToString()}");
                                    ////        myData = AES_ENCYPRTION(newRow_firstrecords[varIDs[0]].ToString(), newRow_firstrecords[varIDs[1]].ToString());
                                    ////    }
                                    ////    //else if (varCount == 0)
                                    ////    //{
                                    ////    //    //myData = Single_Des(Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString());
                                    ////    //    myData = AES_ENCYPRTION(Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString(), "E8F8D8DCAA7DF2D372B0446C196E580C");

                                    ////    //}
                                    ////    else
                                    ////    {
                                    ////        MessageBox.Show($"{varCount + 1} variable found in Algoname-{Algo_Name}  in 'Tag' Value");
                                    ////    }
                                    ////    break;

                                    //case "AES_128_1":
                                    //    if (i == 0)
                                    //    {
                                    //        myData = var_Value;
                                    //    }
                                    //    else
                                    //    {
                                    //        if (varCount > 0)
                                    //        {
                                    //            MessageBox.Show($"More than one Variable found in Algoname-{Algo_Name}  in 'Tag' Value");
                                    //        }
                                    //        else
                                    //        {
                                    //            //myData = Aes_128(ki_val);
                                    //            //myData = Aes_128(Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString());
                                    //        }
                                    //    }
                                    //    break;
                                    case "AES_128":
                                        parts = tag_value.Split(',');
                                        // e.g. "KeyVar,DataVar"
                                        if (parts.Length >= 2)
                                        {
                                            string key = Process_data.Select($"Variable = '{parts[0].Trim()}'")[0]["Value"].ToString();
                                            string data = Process_data.Select($"Variable = '{parts[1].Trim()}'")[0]["Value"].ToString();

                                            myData = OPC_GEN.opc(key, data);
                                        }
                                        else
                                        {
                                            throw new Exception($"OPC_GEN requires 2 variables, but got: {tag_value}");
                                        }

                                        break;

                                    case "Triple_Des_CBC":
                                        parts = tag_value.Split(',');
                                        // e.g. "KeyVar,DataVar"
                                        if (parts.Length >= 2)
                                        {
                                            string key = Process_data.Select($"Variable = '{parts[0].Trim()}'")[0]["Value"].ToString();
                                            string data = Process_data.Select($"Variable = '{parts[1].Trim()}'")[0]["Value"].ToString();

                                            myData = TripleDESEncrypt_cbc(key, data);
                                        }
                                        else
                                        {
                                            throw new Exception($"Triple_Des_CBC requires 2 variables, but got: {tag_value}");
                                        }

                                        break;

                                }



                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), myData, var_type.TrimEnd(), filename_2);
                            }


                            if (Var_Text.TrimEnd() == "TX")
                            {
                                myData = var_Value.TrimEnd();
                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), myData, var_type.TrimEnd(), filename_2);
                            }
                            if (Algo_Name.TrimEnd() == "Fetch_key")
                            {
                                using (SqlConnection con10 = new SqlConnection(connectionString))
                                {
                                    con10.Open();
                                    using (SqlCommand cmd = new SqlCommand(@"
									UPDATE DataGenProcessData 
									SET VarValue = (
										SELECT DataValue 
										FROM [dbo].[FileGeneration] 
										WHERE DataType = @varValue AND IsActive = 1 AND CustomerProfileID = @profileID
									) 
									WHERE VarID = @varName", con10))
                                    {
                                        cmd.Parameters.AddWithValue("@varValue", var_Value.TrimEnd());
                                        cmd.Parameters.AddWithValue("@profileID", ProfileID);
                                        cmd.Parameters.AddWithValue("@varName", var_name);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        myData = "";
                        insert_data(var_name.TrimEnd(), var_des.TrimEnd(), myData, var_type.TrimEnd(), filename_2);

                    }



                    finally
                    {
                        Console.WriteLine($"Executing finally block." + var_name + " _ " + var_des + " _ " + myData);
                        Process_data.Rows.Add(var_name, var_des, myData);
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error During Importation : on variable {var_des} error_msg id " + ex.Message);
                string error = $"Error During Importation in file '{Path.GetFileName(filename_2)}' : " + ex.Message;
                logString.Append($"**{error}**\n");
                Console.WriteLine($"**{error}**\n");
                using (SqlConnection con1 = new SqlConnection(constr))
                {
                    con1.Open();
                    using (SqlCommand cmd1 = new SqlCommand($"DELETE FROM [DGPDR_Base] WHERE  DataGenProcessHDID={lastInsertedId};", con1))
                    {
                        int rowsAffected = cmd1.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            logString.Append($"- '{Path.GetFileName(filename_2)}' file data deleted successfully from database.\n");
                            Console.WriteLine($"- '{Path.GetFileName(filename_2)}' file data deleted successfully from database.\n");
                        }
                        else
                        {
                            logString.Append($"- Unable to delete '{Path.GetFileName(filename_2)}' file data.\n");
                            Console.WriteLine($"- Unable to delete '{Path.GetFileName(filename_2)}' file data.\n");
                        }
                    }
                }
                return 0;
            }

        }





        //        public int btnImport_Click_working(int hdid, int lot)
        //        {
        //            string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //            SqlConnection con = new SqlConnection(constr);
        //            string filename_2 = getfilenameandid();
        //            String Query0 = "SELECT * FROM InPutDataTemplate WHERE CustID = " + OFProcessing.customerID + " and ProfileID =" + OFProcessing.ProfileID + " and  trim(VarText) = 'FL' and isnull(LineNumber,0)!=0 ";
        //            System.Data.DataTable dt0 = new System.Data.DataTable();
        //            DataRow workRow0;
        //            SqlDataAdapter adpt0 = new SqlDataAdapter(Query0, con);
        //            adpt0.Fill(dt0);
        //            DataTable resultDataTable = new DataTable();
        //            resultDataTable.Columns.Add("ICICIDHEX", typeof(string));
        //            resultDataTable.Columns.Add("IMSIHEX", typeof(string));
        //            resultDataTable.Columns.Add("MSISDNHEX", typeof(string));
        //            resultDataTable.Columns.Add("ICICID", typeof(string));
        //            resultDataTable.Columns.Add("IMSI", typeof(string));
        //            resultDataTable.Columns.Add("MSISDN", typeof(string));
        //            resultDataTable.Columns.Add("lot", typeof(int)).DefaultValue = lot;
        //            resultDataTable.Columns.Add("DataGenProcessHDID", typeof(int)).DefaultValue = hdid;
        //            resultDataTable.Columns.Add("CustID", typeof(int)).DefaultValue = customerID;
        //            resultDataTable.Columns.Add("ProID", typeof(int)).DefaultValue = ProfileID;
        //            int imsi_line_no = 0, msisdn_line_no = 0, line_no = 0;
        //            int iccid_line_no = 0;
        //            int qty_line_no = 0;
        //            int qty_frm = 0;
        //            int qty_len = 0;
        //            int iccid_frm = 0;
        //            int iccid_len = 0;
        //            int imsi_from = 0;
        //            int imsi_len = 0;
        //            int msisdn_from = 0;
        //            int msisdn_len = 0;
        //            bool IsIncremental = false;
        //            foreach (DataRow dv0 in dt0.Rows)
        //            {
        //                string var_des = dv0[5].ToString().Trim();
        //                string Var_Text = dv0[7].ToString().Trim();
        //                string line_sql = dv0[11].ToString().Trim();
        //                string Pos_From = dv0[13].ToString().Trim();
        //                string len_data = dv0[15].ToString().Trim();
        //                string tag = dv0[16].ToString().Trim();
        //                if (Var_Text.TrimEnd() == "FL")
        //                {
        //                    if (var_des == "ICCID")
        //                    {
        //                        iccid_frm = Convert.ToInt32(Pos_From);
        //                        iccid_len = Convert.ToInt32(len_data);
        //                        iccid_line_no = Convert.ToInt32(line_sql);
        //                        if (!string.IsNullOrEmpty(tag))
        //                        {
        //                            IsIncremental = true;
        //                        }
        //                        else { line_no = Convert.ToInt32(line_sql); }
        //                    }

        //                    else if (var_des == "IMSI")
        //                    {
        //                        imsi_from = Convert.ToInt32(Pos_From);
        //                        imsi_len = Convert.ToInt32(len_data);
        //                        imsi_line_no = Convert.ToInt32(line_sql);
        //                        if (!string.IsNullOrEmpty(tag))
        //                        {
        //                            IsIncremental = true;
        //                        }
        //                    }
        //                    else if (var_des == "MSISDN")
        //                    {
        //                        msisdn_from = Convert.ToInt32(Pos_From);
        //                        msisdn_len = Convert.ToInt32(len_data);
        //                        msisdn_line_no = Convert.ToInt32(line_sql);
        //                    }
        //                    else if (var_des == "Quantity")
        //                    {
        //                        qty_line_no = Convert.ToInt32(line_sql);
        //                        qty_frm = Convert.ToInt32(Pos_From);
        //                        qty_len = Convert.ToInt32(len_data);
        //                    }
        //                }
        //            }
        //            if (IsIncremental)
        //            {
        //                try
        //                {

        //                    int qty = 0;
        //                    long iccid = 0;
        //                    long imsi = 0;
        //                    long msisdn = 0;
        //                    StreamReader sr = new StreamReader(filename_2);
        //                    int line_number = 1;
        //                    string line;
        //                    string data_val_line = sr.ReadToEnd();
        //                    //while ((line = sr.ReadLine()) != null)
        //                    //{
        //                    //    if (line_number == qty_line_no)
        //                    //    {
        //                    //        qty = Convert.ToInt32(line.Substring(qty_frm, qty_len).Trim());
        //                    //        break;

        //                    //    }
        //                    //    line_number++;
        //                    //}
        //                    try
        //                    {
        //                        qty = Convert.ToInt32(data_val_line.Split('\n')[qty_line_no - 1].Substring(qty_frm, qty_len).Trim());
        //                    }
        //                    catch
        //                    {

        //                        string line_test = data_val_line.Split('\n')[qty_line_no - 1];
        //                        int safeLen = Math.Min(qty_len, line_test.Length - qty_frm);
        //                        string qtyStr = line_test.Substring(qty_frm, safeLen).Trim();
        //                        qty = Convert.ToInt32(qtyStr);
        //                    }
        //                    //data_val_line = data_val_line.Split('\n')[imsi_line_no - 1];

        //                    try
        //                    {
        //                        iccid = Convert.ToInt64(data_val_line.Split('\n')[iccid_line_no - 1].Substring(iccid_frm, iccid_len).Trim());
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageBox.Show($"Unable to read ICCID frim input file Linenumber:{iccid_line_no} , position from : {iccid_frm} , length : {iccid_len}");
        //                        throw;
        //                    }

        //                    try
        //                    {
        //                        imsi = Convert.ToInt64(data_val_line.Split('\n')[imsi_line_no - 1].Substring(imsi_from, imsi_len).Trim());
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageBox.Show($"Unable to read ICCID frim input file Linenumber:{imsi_line_no} , position from : {imsi_from} , length : {imsi_len}");
        //                        throw;

        //                    }


        //                    try
        //                    {
        //                        if (msisdn_from != 0 && msisdn_len != 0)
        //                        {
        //                            msisdn = Convert.ToInt64(data_val_line.Split('\n')[msisdn_line_no - 1].Substring(msisdn_from, msisdn_len).Trim());
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageBox.Show($"Unable to read ICCID frim input file Linenumber:{imsi_line_no} , position from : {imsi_from} , length : {imsi_len}");
        //                        throw;

        //                    }


        //                    for (int i = 0; i < qty; i++)
        //                    {
        //                        if (msisdn != 0)
        //                        {
        //                            resultDataTable.Rows.Add(StringToHex(iccid.ToString()).Trim(), StringToHex(imsi.ToString()).Trim(), StringToHex(msisdn.ToString()).Trim(), iccid.ToString(), imsi.ToString(), msisdn.ToString());
        //                            iccid += 1;
        //                            imsi += 1;
        //                            msisdn += 1;

        //                        }
        //                        else
        //                        {
        //                            resultDataTable.Rows.Add(StringToHex(iccid.ToString()).Trim(), StringToHex(imsi.ToString()).Trim(), "", iccid.ToString(), imsi.ToString(), "");
        //                            iccid += 1;
        //                            imsi += 1;
        //                        }

        //                    }

        //                    sr.Close();
        //                    con.Open();
        //                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
        //                    {
        //                        bulkCopy.DestinationTableName = "DGPDR_Base";
        //                        bulkCopy.ColumnMappings.Add("ICICID", "ICICID");
        //                        bulkCopy.ColumnMappings.Add("IMSI", "IMSI");
        //                        bulkCopy.ColumnMappings.Add("MSISDN", "MSISDN");
        //                        bulkCopy.ColumnMappings.Add("lot", "lot");
        //                        bulkCopy.ColumnMappings.Add("DataGenProcessHDID", "DataGenProcessHDID");
        //                        bulkCopy.WriteToServer(resultDataTable);
        //                    }
        //                    con.Close();
        //                    con.Open();
        //                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
        //                    {
        //                        bulkCopy.DestinationTableName = "DupCheck";
        //                        bulkCopy.ColumnMappings.Add("ICICIDHEX", "ICCID");
        //                        bulkCopy.ColumnMappings.Add("IMSIHEX", "IMSI");
        //                        bulkCopy.ColumnMappings.Add("MSISDNHEX", "MSISDN");
        //                        bulkCopy.ColumnMappings.Add("CustID", "CustID");
        //                        bulkCopy.ColumnMappings.Add("ProID", "CustProfileID");
        //                        bulkCopy.ColumnMappings.Add("DataGenProcessHDID", "C1");
        //                        bulkCopy.WriteToServer(resultDataTable);
        //                    }
        //                    con.Close();
        //                    using (SqlConnection con11 = new SqlConnection(constr))
        //                    {
        //                        con11.Open();
        //                        using (SqlCommand cmd1 = new SqlCommand($"UPDATE DataGenProcessHD SET StatusID=1 WHERE  DataGenProcessHDID={lastInsertedId};", con11))
        //                        {
        //                            int rowsAffected = cmd1.ExecuteNonQuery();
        //                        }
        //                    }
        //                    string r4_data = "", r8_data = "";
        //                    int r4_data_count = 0, r8_data_count = 0;

        //                    string constr1 = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //                    SqlConnection con1 = new SqlConnection(constr);
        //                    int list_4 = 0, list_8 = 0;
        //                    String Query01 = "SELECT * FROM InPutDataTemplate WHERE CustID = " + OFProcessing.customerID + " and ProfileID =" + OFProcessing.ProfileID + "order by InPutDataTemplateID";
        //                    System.Data.DataTable dt01 = new System.Data.DataTable();
        //                    DataRow workRow01;
        //                    SqlDataAdapter adpt01 = new SqlDataAdapter(Query01, con1);
        //                    adpt01.Fill(dt01);
        //                    foreach (DataRow dv0 in dt01.Rows)
        //                    {
        //                        string var_name = dv0[3].ToString().Trim();
        //                        Console.WriteLine($"inserting all data : " + var_name);

        //                        string var_Value = dv0[4].ToString().Trim();
        //                        string var_des = dv0[5].ToString().Trim();
        //                        string var_type = dv0[6].ToString().Trim();

        //                        string Var_Text = dv0[7].ToString().Trim();
        //                        string Algo_Name = dv0[9].ToString().Trim();
        //                        string var_algoname = dv0[9].ToString().Trim();
        //                        string line_sql = dv0[11].ToString().Trim();
        //                        string File_ID = dv0[10].ToString().Trim();
        //                        string Pos_From = dv0[13].ToString().Trim();
        //                        string len_data = dv0[15].ToString().Trim();
        //                        String line1;
        //                        string myData = "";

        //                        try
        //                        {
        //                            if (var_des == "OutFile_Header")
        //                            {
        //                                myData = string.Join("\r\n", File.ReadLines(filename_2).Take(Convert.ToInt32(line_sql))) + "\r\n";
        //                                using (SqlConnection con0 = new SqlConnection(connectionString))
        //                                {
        //                                    SqlDataReader reader = null;
        //                                    using (SqlCommand cmd = new SqlCommand("usp_Insert_first_record", con0))
        //                                    {
        //                                        cmd.CommandType = CommandType.StoredProcedure;
        //                                        cmd.Parameters.AddWithValue("@DataGenProcessHDID", lastInsertedId);
        //                                        cmd.Parameters.AddWithValue("@VarID", var_name.TrimEnd());
        //                                        cmd.Parameters.AddWithValue("@VarName", var_des.TrimEnd());
        //                                        cmd.Parameters.AddWithValue("@VarValue", myData);
        //                                        cmd.Parameters.AddWithValue("@VarType", var_type.TrimEnd());
        //                                        cmd.Parameters.AddWithValue("@StatusID", "");
        //                                        try
        //                                        {
        //                                            con0.Open();
        //                                            reader = cmd.ExecuteReader();
        //                                            con0.Close();
        //                                        }
        //                                        catch (Exception exe)
        //                                        {
        //                                            MessageBox.Show(exe.Message);
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (Var_Text.TrimEnd() == "FL")
        //                                {
        //                                    if (var_des == "LICENSE_KEY")
        //                                    {
        //                                        myData = var_Value;
        //                                        using (SqlConnection con0 = new SqlConnection(connectionString))
        //                                        {
        //                                            SqlDataReader reader = null;
        //                                            using (SqlCommand cmd = new SqlCommand("usp_Insert_first_record", con0))
        //                                            {
        //                                                cmd.CommandType = CommandType.StoredProcedure;
        //                                                cmd.Parameters.AddWithValue("@DataGenProcessHDID", lastInsertedId);
        //                                                cmd.Parameters.AddWithValue("@VarID", var_name.TrimEnd());
        //                                                cmd.Parameters.AddWithValue("@VarName", var_des.TrimEnd());
        //                                                cmd.Parameters.AddWithValue("@VarValue", myData.TrimEnd());
        //                                                cmd.Parameters.AddWithValue("@VarType", var_type.TrimEnd());
        //                                                cmd.Parameters.AddWithValue("@StatusID", "");
        //                                                try
        //                                                {
        //                                                    con0.Open();
        //                                                    reader = cmd.ExecuteReader();
        //                                                    con0.Close();
        //                                                }
        //                                                catch (Exception exe)
        //                                                {
        //                                                    MessageBox.Show(exe.Message);
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                    StreamReader sr1 = new StreamReader(filename_2);

        //                                    string strPath = filename_2;

        //                                    string filename = null;
        //                                    filename = Path.GetFileName(strPath);

        //                                    line1 = sr1.ReadLine();

        //                                    int line_number1 = 1;

        //                                    while (line1 != null)
        //                                    {

        //                                        if (line_number1.ToString().Equals(line_sql))
        //                                        {
        //                                            try
        //                                            {
        //                                                myData = line1.Substring(int.Parse(Pos_From), int.Parse(len_data));
        //                                            }
        //                                            catch
        //                                            {
        //                                                myData = line1.Substring(int.Parse(Pos_From), line1.Length - int.Parse(Pos_From));
        //                                            }
        //                                            if (var_des == "ICCID")
        //                                            {
        //                                                first_icicid = myData;
        //                                            }
        //                                            else if (var_des == "IMSI")
        //                                            {
        //                                                first_imsi = myData;
        //                                            }
        //                                            else if (var_des == "MSISDN")
        //                                            {
        //                                                if (string.IsNullOrEmpty(myData.Trim()))
        //                                                {
        //                                                    string fd = "";
        //                                                    for (int i = 0; i < int.Parse(len_data); i++)
        //                                                    {
        //                                                        fd += "F";
        //                                                    }
        //                                                    myData = fd;
        //                                                }
        //                                                first_msisdn = myData;
        //                                            }

        //                                            using (SqlConnection con0 = new SqlConnection(connectionString))
        //                                            {
        //                                                SqlDataReader reader = null;
        //                                                using (SqlCommand cmd = new SqlCommand("usp_Insert_first_record", con0))
        //                                                {
        //                                                    cmd.CommandType = CommandType.StoredProcedure;
        //                                                    cmd.Parameters.AddWithValue("@DataGenProcessHDID", lastInsertedId);
        //                                                    cmd.Parameters.AddWithValue("@VarID", var_name.TrimEnd());
        //                                                    cmd.Parameters.AddWithValue("@VarName", var_des.TrimEnd());
        //                                                    cmd.Parameters.AddWithValue("@VarValue", myData.TrimEnd());
        //                                                    cmd.Parameters.AddWithValue("@VarType", var_type.TrimEnd());
        //                                                    cmd.Parameters.AddWithValue("@StatusID", "");
        //                                                    try
        //                                                    {
        //                                                        con0.Open();
        //                                                        reader = cmd.ExecuteReader();
        //                                                        con0.Close();
        //                                                    }
        //                                                    catch (Exception exe)
        //                                                    {
        //                                                        MessageBox.Show(exe.Message);
        //                                                    }
        //                                                }
        //                                            }
        //                                        }
        //                                        line1 = sr1.ReadLine();
        //                                        line_number1++;
        //                                    }
        //                                    sr1.Close();
        //                                    Console.ReadLine();
        //                                }
        //                                if (Var_Text.TrimEnd() == "AL")
        //                                {
        //                                    insert_data(var_name.TrimEnd(), var_des.TrimEnd(), "", var_type.TrimEnd(), filename_2);
        //                                }

        //                                if (Var_Text.TrimEnd() == "TX")
        //                                {
        //                                    string my_data = var_Value.TrimEnd();
        //                                    insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data, var_type.TrimEnd(), filename_2);
        //                                }
        //                                if (Algo_Name.TrimEnd() == "Fetch_key")
        //                                {
        //                                    using (SqlConnection con10 = new SqlConnection(connectionString))
        //                                    {
        //                                        con10.Open();
        //                                        using (SqlCommand cmd = new SqlCommand(@"
        //									UPDATE DataGenProcessData 
        //									SET VarValue = (
        //										SELECT DataValue 
        //										FROM [dbo].[FileGeneration] 
        //										WHERE DataType = @varValue AND IsActive = 1 AND CustomerProfileID = @profileID
        //									) 
        //									WHERE VarID = @varName", con10))
        //                                        {
        //                                            cmd.Parameters.AddWithValue("@varValue", var_Value.TrimEnd());
        //                                            cmd.Parameters.AddWithValue("@profileID", ProfileID);
        //                                            cmd.Parameters.AddWithValue("@varName", var_name);
        //                                            cmd.ExecuteNonQuery();
        //                                        }
        //                                    }
        //                                }
        //                            }

        //                        }
        //                        finally
        //                        {
        //                            Console.WriteLine($"Executing finally block.");
        //                        }
        //                    }
        //                    return 1;
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show("Error During Importation : " + ex.Message);
        //                    string error = $"Error During Importation in file '{Path.GetFileName(filename_2)}' : " + ex.Message;
        //                    logString.Append($"**{error}**\n");
        //                    Console.WriteLine($"**{error}**\n");
        //                    using (SqlConnection con1 = new SqlConnection(constr))
        //                    {
        //                        con1.Open();
        //                        using (SqlCommand cmd1 = new SqlCommand($"DELETE FROM [DGPDR_Base] WHERE  DataGenProcessHDID={lastInsertedId};", con1))
        //                        {
        //                            int rowsAffected = cmd1.ExecuteNonQuery();
        //                            if (rowsAffected > 0)
        //                            {
        //                                logString.Append($"- '{Path.GetFileName(filename_2)}' file data deleted successfully from database.\n");
        //                                Console.WriteLine($"- '{Path.GetFileName(filename_2)}' file data deleted successfully from database.\n");
        //                            }
        //                            else
        //                            {
        //                                logString.Append($"- Unable to delete '{Path.GetFileName(filename_2)}' file data.\n");
        //                                Console.WriteLine($"- Unable to delete '{Path.GetFileName(filename_2)}' file data.\n");
        //                            }
        //                        }
        //                    }
        //                    return 0;
        //                }
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    StreamReader sr = new StreamReader(filename_2);
        //                    int line_number = 1;
        //                    string line;
        //                    while ((line = sr.ReadLine()) != null)
        //                    {
        //                        if (line_number >= line_no)
        //                        {
        //                            if (msisdn_from == 0 && msisdn_len == 0)
        //                            {
        //                                resultDataTable.Rows.Add(StringToHex(line.Substring(iccid_frm, iccid_len).Trim()), StringToHex(line.Substring(imsi_from, imsi_len).Trim()), "", line.Substring(iccid_frm, iccid_len).Trim(), line.Substring(imsi_from, imsi_len).Trim(), "");
        //                            }
        //                            else
        //                            {
        //                                string msisdn_data = "";
        //                                try
        //                                {
        //                                    msisdn_data = line.Substring(msisdn_from, msisdn_len).Trim();

        //                                }
        //                                catch
        //                                {
        //                                    for (int i = 0; i < msisdn_len; i++)
        //                                    {
        //                                        msisdn_data += "F";
        //                                    }
        //                                }
        //                                resultDataTable.Rows.Add(StringToHex(line.Substring(iccid_frm, iccid_len).Trim()), StringToHex(line.Substring(imsi_from, imsi_len).Trim()), StringToHex(msisdn_data), line.Substring(iccid_frm, iccid_len).Trim(), line.Substring(imsi_from, imsi_len).Trim(), msisdn_data);


        //                            }

        //                        }
        //                        line_number++;
        //                    }
        //                    sr.Close();
        //                    con.Open();
        //                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
        //                    {
        //                        bulkCopy.DestinationTableName = "DGPDR_Base";
        //                        bulkCopy.ColumnMappings.Add("ICICID", "ICICID");
        //                        bulkCopy.ColumnMappings.Add("IMSI", "IMSI");
        //                        bulkCopy.ColumnMappings.Add("MSISDN", "MSISDN");
        //                        bulkCopy.ColumnMappings.Add("lot", "lot");
        //                        bulkCopy.ColumnMappings.Add("DataGenProcessHDID", "DataGenProcessHDID");
        //                        bulkCopy.WriteToServer(resultDataTable);
        //                    }
        //                    con.Close();
        //                    con.Open();
        //                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
        //                    {
        //                        bulkCopy.DestinationTableName = "DupCheck";
        //                        bulkCopy.ColumnMappings.Add("ICICIDHEX", "ICCID");
        //                        bulkCopy.ColumnMappings.Add("IMSIHEX", "IMSI");
        //                        bulkCopy.ColumnMappings.Add("MSISDNHEX", "MSISDN");
        //                        bulkCopy.ColumnMappings.Add("CustID", "CustID");
        //                        bulkCopy.ColumnMappings.Add("ProID", "CustProfileID");
        //                        bulkCopy.ColumnMappings.Add("DataGenProcessHDID", "C1");
        //                        bulkCopy.WriteToServer(resultDataTable);
        //                    }
        //                    con.Close();
        //                    using (SqlConnection con11 = new SqlConnection(constr))
        //                    {
        //                        con11.Open();
        //                        using (SqlCommand cmd1 = new SqlCommand($"UPDATE DataGenProcessHD SET StatusID=1 WHERE  DataGenProcessHDID={lastInsertedId};", con11))
        //                        {
        //                            int rowsAffected = cmd1.ExecuteNonQuery();
        //                        }
        //                    }
        //                    string r4_data = "", r8_data = "";
        //                    int r4_data_count = 0, r8_data_count = 0;

        //                    string constr1 = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //                    SqlConnection con1 = new SqlConnection(constr);
        //                    int list_4 = 0, list_8 = 0;
        //                    String Query01 = "SELECT * FROM InPutDataTemplate WHERE CustID = " + OFProcessing.customerID + " and ProfileID =" + OFProcessing.ProfileID + "order by InPutDataTemplateID";
        //                    System.Data.DataTable dt01 = new System.Data.DataTable();
        //                    DataRow workRow01;
        //                    SqlDataAdapter adpt01 = new SqlDataAdapter(Query01, con1);
        //                    adpt01.Fill(dt01);
        //                    foreach (DataRow dv0 in dt01.Rows)
        //                    {
        //                        string var_name = dv0[3].ToString().Trim();
        //                        string var_Value = dv0[4].ToString().Trim();
        //                        string var_des = dv0[5].ToString().Trim();
        //                        string var_type = dv0[6].ToString().Trim();

        //                        string Var_Text = dv0[7].ToString().Trim();
        //                        string Algo_Name = dv0[9].ToString().Trim();
        //                        string line_sql = dv0[11].ToString().Trim();
        //                        string File_ID = dv0[10].ToString().Trim();
        //                        string Pos_From = dv0[13].ToString().Trim();
        //                        string len_data = dv0[15].ToString().Trim();
        //                        String line1;
        //                        string myData;

        //                        try
        //                        {
        //                            if (var_des == "OutFile_Header")
        //                            {
        //                                myData = string.Join("\r\n", File.ReadLines(filename_2).Take(Convert.ToInt32(line_sql))) + "\r\n";
        //                                using (SqlConnection con0 = new SqlConnection(connectionString))
        //                                {
        //                                    SqlDataReader reader = null;
        //                                    using (SqlCommand cmd = new SqlCommand("usp_Insert_first_record", con0))
        //                                    {
        //                                        cmd.CommandType = CommandType.StoredProcedure;
        //                                        cmd.Parameters.AddWithValue("@DataGenProcessHDID", lastInsertedId);
        //                                        cmd.Parameters.AddWithValue("@VarID", var_name.TrimEnd());
        //                                        cmd.Parameters.AddWithValue("@VarName", var_des.TrimEnd());
        //                                        cmd.Parameters.AddWithValue("@VarValue", myData);
        //                                        cmd.Parameters.AddWithValue("@VarType", var_type.TrimEnd());
        //                                        cmd.Parameters.AddWithValue("@StatusID", "");
        //                                        try
        //                                        {
        //                                            con0.Open();
        //                                            reader = cmd.ExecuteReader();
        //                                            con0.Close();
        //                                        }
        //                                        catch (Exception exe)
        //                                        {
        //                                            MessageBox.Show(exe.Message);
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (Var_Text.TrimEnd() == "FL")
        //                                {
        //                                    if (var_des == "LICENSE_KEY")
        //                                    {
        //                                        myData = var_Value;
        //                                        using (SqlConnection con0 = new SqlConnection(connectionString))
        //                                        {
        //                                            SqlDataReader reader = null;
        //                                            using (SqlCommand cmd = new SqlCommand("usp_Insert_first_record", con0))
        //                                            {
        //                                                cmd.CommandType = CommandType.StoredProcedure;
        //                                                cmd.Parameters.AddWithValue("@DataGenProcessHDID", lastInsertedId);
        //                                                cmd.Parameters.AddWithValue("@VarID", var_name.TrimEnd());
        //                                                cmd.Parameters.AddWithValue("@VarName", var_des.TrimEnd());
        //                                                cmd.Parameters.AddWithValue("@VarValue", myData.TrimEnd());
        //                                                cmd.Parameters.AddWithValue("@VarType", var_type.TrimEnd());
        //                                                cmd.Parameters.AddWithValue("@StatusID", "");
        //                                                try
        //                                                {
        //                                                    con0.Open();
        //                                                    reader = cmd.ExecuteReader();
        //                                                    con0.Close();
        //                                                }
        //                                                catch (Exception exe)
        //                                                {
        //                                                    MessageBox.Show(exe.Message);
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                    StreamReader sr1 = new StreamReader(filename_2);

        //                                    string strPath = filename_2;

        //                                    string filename = null;
        //                                    filename = Path.GetFileName(strPath);

        //                                    line1 = sr1.ReadLine();

        //                                    int line_number1 = 1;

        //                                    while (line1 != null)
        //                                    {

        //                                        if (line_number1.ToString().Equals(line_sql))
        //                                        {
        //                                            try
        //                                            {
        //                                                myData = line1.Substring(int.Parse(Pos_From), int.Parse(len_data));
        //                                            }
        //                                            catch
        //                                            {
        //                                                myData = line1.Substring(int.Parse(Pos_From), line1.Length - int.Parse(Pos_From));
        //                                            }
        //                                            if (var_des == "ICCID")
        //                                            {
        //                                                first_icicid = myData;
        //                                            }
        //                                            else if (var_des == "IMSI")
        //                                            {
        //                                                first_imsi = myData;
        //                                            }
        //                                            else if (var_des == "MSISDN")
        //                                            {
        //                                                if (string.IsNullOrEmpty(myData.Trim()))
        //                                                {
        //                                                    string fd = "";
        //                                                    for (int i = 0; i < int.Parse(len_data); i++)
        //                                                    {
        //                                                        fd += "F";
        //                                                    }
        //                                                    myData = fd;
        //                                                }
        //                                                first_msisdn = myData;
        //                                            }

        //                                            using (SqlConnection con0 = new SqlConnection(connectionString))
        //                                            {
        //                                                SqlDataReader reader = null;
        //                                                using (SqlCommand cmd = new SqlCommand("usp_Insert_first_record", con0))
        //                                                {
        //                                                    cmd.CommandType = CommandType.StoredProcedure;
        //                                                    cmd.Parameters.AddWithValue("@DataGenProcessHDID", lastInsertedId);
        //                                                    cmd.Parameters.AddWithValue("@VarID", var_name.TrimEnd());
        //                                                    cmd.Parameters.AddWithValue("@VarName", var_des.TrimEnd());
        //                                                    cmd.Parameters.AddWithValue("@VarValue", myData.TrimEnd());
        //                                                    cmd.Parameters.AddWithValue("@VarType", var_type.TrimEnd());
        //                                                    cmd.Parameters.AddWithValue("@StatusID", "");
        //                                                    try
        //                                                    {
        //                                                        con0.Open();
        //                                                        reader = cmd.ExecuteReader();
        //                                                        con0.Close();
        //                                                    }
        //                                                    catch (Exception exe)
        //                                                    {
        //                                                        MessageBox.Show(exe.Message);
        //                                                    }
        //                                                }
        //                                            }
        //                                        }
        //                                        line1 = sr1.ReadLine();
        //                                        line_number1++;
        //                                    }
        //                                    sr1.Close();
        //                                    Console.ReadLine();
        //                                }
        //                                if (Var_Text.TrimEnd() == "AL")
        //                                {
        //                                    insert_data(var_name.TrimEnd(), var_des.TrimEnd(), "", var_type.TrimEnd(), filename_2);
        //                                }
        //                                if (Var_Text.TrimEnd() == "TX")
        //                                {
        //                                    string my_data = var_Value.TrimEnd();
        //                                    insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data, var_type.TrimEnd(), filename_2);
        //                                }
        //                                if (Algo_Name.TrimEnd() == "Fetch_key")
        //                                {
        //                                    using (SqlConnection con10 = new SqlConnection(connectionString))
        //                                    {
        //                                        con10.Open();
        //                                        using (SqlCommand cmd = new SqlCommand(@"
        //UPDATE DataGenProcessData 
        //SET VarValue = (
        //	SELECT DataValue 
        //	FROM [dbo].[FileGeneration] 
        //	WHERE DataType = @varValue AND IsActive = 1 AND CustomerProfileID = @profileID
        //) 
        //WHERE VarID = @varName", con10))
        //                                        {
        //                                            cmd.Parameters.AddWithValue("@varValue", var_Value.TrimEnd());
        //                                            cmd.Parameters.AddWithValue("@profileID", ProfileID);
        //                                            cmd.Parameters.AddWithValue("@varName", var_name);
        //                                            cmd.ExecuteNonQuery();
        //                                        }
        //                                    }
        //                                }
        //                            }

        //                        }
        //                        finally
        //                        {
        //                            Console.WriteLine($"Executing finally block.");
        //                        }
        //                    }
        //                    return 1;
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show("Error During Importaion : " + ex.Message);
        //                    string error = $"Error During Importaion in file '{Path.GetFileName(filename_2)}' : " + ex.Message;
        //                    logString.Append($"**{error}**\n");
        //                    Console.WriteLine($"**{error}**\n");
        //                    using (SqlConnection con1 = new SqlConnection(constr))
        //                    {
        //                        con1.Open();
        //                        using (SqlCommand cmd1 = new SqlCommand($"DELETE FROM [DGPDR_Base] WHERE  DataGenProcessHDID={lastInsertedId};", con1))
        //                        {
        //                            int rowsAffected = cmd1.ExecuteNonQuery();
        //                            if (rowsAffected > 0)
        //                            {
        //                                logString.Append($"- '{Path.GetFileName(filename_2)}' file data deleted successfully from database.\n");
        //                                Console.WriteLine($"- '{Path.GetFileName(filename_2)}' file data deleted successfully from database.\n");
        //                            }
        //                            else
        //                            {
        //                                logString.Append($"- Unable to delete '{Path.GetFileName(filename_2)}' file data.\n");
        //                                Console.WriteLine($"- Unable to delete '{Path.GetFileName(filename_2)}' file data.\n");
        //                            }
        //                        }
        //                    }
        //                    return 0;
        //                }
        //            }


        //        }
        public string Importlicencefile(int lot)
        {
            string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlConnection con = new SqlConnection(constr);
            string filename_2 = txtLicence.Text.TrimEnd();
            String Query0 = "SELECT * FROM License_InPutTemplate WHERE CustID = " + OFProcessing.customerID + " and ProfileID =" + OFProcessing.ProfileID;
            System.Data.DataTable dt0 = new System.Data.DataTable();
            DataRow workRow0;
            SqlDataAdapter adpt0 = new SqlDataAdapter(Query0, con);
            adpt0.Fill(dt0);
            DataTable resultDataTable = new DataTable();
            int line_no = 0;
            int iccid_frm = -1;
            int iccid_len = -1;
            int imsi_from = -1;
            int imsi_len = -1;
            int lic_from = -1;
            int lic_len = -1;
            string column_name = "";
            foreach (DataRow dv0 in dt0.Rows)
            {
                string var_des = dv0[5].ToString().Trim();
                string Var_Text = dv0[7].ToString().Trim();
                string line_sql = dv0[11].ToString().Trim();
                string Pos_From = dv0[13].ToString().Trim();
                string len_data = dv0[15].ToString().Trim();

                if (Var_Text.TrimEnd() == "FL")
                {
                    if (var_des == "ICCID")
                    {
                        column_name = "ICICID";
                        resultDataTable.Columns.Add("ICICID", typeof(string));
                        iccid_frm = Convert.ToInt32(Pos_From);
                        iccid_len = Convert.ToInt32(len_data);
                        line_no = Convert.ToInt32(line_sql);
                    }
                    else if (var_des == "IMSI")
                    {
                        column_name = "IMSI";
                        resultDataTable.Columns.Add("IMSI", typeof(string));
                        imsi_from = Convert.ToInt32(Pos_From);
                        imsi_len = Convert.ToInt32(len_data);
                    }
                    else if (var_des == "LICENSE_KEY")
                    {
                        resultDataTable.Columns.Add("LICENSE_KEY", typeof(string));
                        lic_from = Convert.ToInt32(Pos_From);
                        lic_len = Convert.ToInt32(len_data);
                    }
                }
            }
            try
            {
                StreamReader sr = new StreamReader(filename_2);
                int line_number = 1;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line_number >= line_no)
                    {
                        if (line_number == line_no)
                        {
                            if (line.Length >= lic_len + imsi_len + iccid_len)
                            {
                                if (iccid_frm != -1 && iccid_len != -1 && lic_from != -1 && lic_len != -1)
                                {
                                    resultDataTable.Rows.Add(line.Substring(iccid_frm, iccid_len).Trim(), line.Substring(lic_from, lic_len).Trim());
                                }
                                else if (imsi_from != -1 && imsi_len != -1 && lic_from != -1 && lic_len != -1)
                                {
                                    resultDataTable.Rows.Add(line.Substring(imsi_from, imsi_len).Trim(), line.Substring(lic_from, lic_len).Trim());
                                }
                            }
                        }
                        else
                        {
                            if (iccid_frm != -1 && iccid_len != -1 && lic_from != -1 && lic_len != -1)
                            {
                                resultDataTable.Rows.Add(line.Substring(iccid_frm, iccid_len).Trim(), line.Substring(lic_from, lic_len).Trim());
                            }
                            else if (imsi_from != -1 && imsi_len != -1 && lic_from != -1 && lic_len != -1)
                            {
                                resultDataTable.Rows.Add(line.Substring(imsi_from, imsi_len).Trim(), line.Substring(lic_from, lic_len).Trim());
                            }
                        }
                    }
                    line_number++;
                }
                sr.Close();
                con.Open();
                string createTableQuery = "CREATE TABLE TempLicence (";
                foreach (DataColumn column in resultDataTable.Columns)
                {
                    createTableQuery += $"[{column.ColumnName}] NVARCHAR(MAX),";
                }
                createTableQuery = createTableQuery.TrimEnd(',') + ")";

                using (SqlCommand createCmd = new SqlCommand(createTableQuery, con))
                {
                    createCmd.ExecuteNonQuery();
                }

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                {
                    bulkCopy.DestinationTableName = "TempLicence";

                    foreach (DataColumn column in resultDataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }

                    bulkCopy.WriteToServer(resultDataTable);
                }
                con.Close();
                var result = "";
                using (SqlConnection con11 = new SqlConnection(constr))
                {
                    con11.Open();
                    string sql = $@"
        WITH differences AS (
            SELECT {column_name} FROM TempLicence
            EXCEPT
            SELECT {column_name} FROM [DGPDR_Base] WHERE lot = {lot}
            UNION
            SELECT {column_name} FROM [DGPDR_Base] WHERE lot = {lot}
            EXCEPT
            SELECT {column_name} FROM TempLicence
        )
        SELECT CASE WHEN COUNT(*) = 0 THEN 'Data matches' ELSE 'Data does not match' END AS status 
        FROM differences;";
                    using (SqlCommand cmd1 = new SqlCommand(sql, con11))//check if the data matched with licence data
                    {
                        result = cmd1.ExecuteScalar().ToString().TrimEnd();
                    }
                }
                if (result != "Data matches")
                {
                    return "No data matched licence file data.";
                }
                else
                {
                    using (SqlConnection con11 = new SqlConnection(constr))
                    {
                        con11.Open();
                        string sql = $"UPDATE T1 SET T1.LICENSE_KEY = T2.LICENSE_KEY FROM DGPDR_Base T1 INNER JOIN TempLicence T2 ON T1.{column_name} = T2.{column_name} WHERE T1.lot = {lot}; DROP TABLE TempLicence;";
                        using (SqlCommand cmd1 = new SqlCommand(sql, con11))//check if the data matched with licence data
                        {
                            cmd1.ExecuteNonQuery();
                        }
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "Error DuringLicense File Importaion : " + ex.Message;

            }

        }
        static bool HasSpecialCharacters(string input)
        {
            //Regex specialCharPattern = new Regex(@"[^a-zA-Z0-9]");
            Regex specialCharPattern = new Regex(@"[^a-zA-Z0-9*=\\""]");
            return specialCharPattern.IsMatch(input);
        }
        static bool IsNumeric(string input)
        {
            Regex numericPattern = new Regex(@"^\d+$");
            return numericPattern.IsMatch(input);
        }

        public string getfilenameandid()
        {
            string filename_12 = string.Empty;
            try
            {
                using (SqlConnection con3 = new SqlConnection(connectionString))
                {
                    con3.Open();
                    SqlDataReader reader = null;

                    List<string> filename1 = new List<string>();

                    using (SqlCommand cmd = new SqlCommand("usp_get_input_file", con3))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustProfile_ID", ProfileID);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            filename1.Add(reader["FilePath"].ToString());
                            fileid = Int32.Parse(reader["DataGenProcessHDID"].ToString());
                        }
                        filename_12 = filename1[0];// instead of 1 enterd 0 to get first record from list which resolves the issue

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while getting filename: " + ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            return filename_12;
        }
        public string Random4digits()
        {
            //random_four = (Int32.Parse(random_four) + 1).ToString();
            random_four = RNG32.Next(1000, 9999).ToString();
            return random_four;

        }

        public string Random8hex()
        {
            //Random random = new Random();
            byte[] buffer = new byte[4];
            RNG32.NextBytes(buffer);
            return BitConverter.ToString(buffer).Replace("-", "").ToUpper();

        }



        public string random_hex_generator(String data_input)
        {
            var data_value = data_input.Split(',');
            string data_output = "";

            for (int i = 0; i < data_value.Length; i++)
            {
                byte[] buffer = new byte[Int32.Parse(data_value[i])];
                RNG32.NextBytes(buffer);
                data_output += BitConverter.ToString(buffer).Replace("-", "").ToUpper() + ',';
            }
            return data_output;
        }
        public int insert_data(string id, string name, string value, string type, string file_name_inp)
        {
            using (SqlConnection con1 = new SqlConnection(connectionString))
            {

                //MessageBox.Show(file_name_inp);
                SqlDataReader reader = null;
                using (SqlCommand cmd = new SqlCommand("usp_Insert_first_record", con1))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DataGenProcessHDID", lastInsertedId);
                    cmd.Parameters.AddWithValue("@VarID", id.TrimEnd());
                    cmd.Parameters.AddWithValue("@VarName", name.TrimEnd());
                    cmd.Parameters.AddWithValue("@VarValue", value);
                    cmd.Parameters.AddWithValue("@VarType", type.TrimEnd());
                    cmd.Parameters.AddWithValue("@StatusID", "");
                    try
                    {
                        con1.Open();
                        reader = cmd.ExecuteReader();

                        con1.Close();
                        //MessageBox.Show("Data Saved Successfully!");
                    }
                    catch (Exception exe)
                    {
                        MessageBox.Show(exe.Message);
                    }
                }
                return 1;
            }

        }
        public string padding_filler(string q)
        {
            string pad = "";

            if (q.Length > 0)
            {

                if (q.Length % 2 == 0)
                {

                    for (int i = 0; i < q.Length; i++) //String Reverse  
                    {
                        pad += '3' + q[i].ToString();
                        //Console.WriteLine(q[i ].ToString()+'3');

                    }
                    pad += "FFFF";
                }
                else
                {
                    pad = "INVALID_PADDING_" + q;
                }
            }
            else
                pad = "INVALID_PADDING_" + q;

            return pad;
        }
        public string Random8digits()
        {
            //random_eight = (Int64.Parse(random_eight) + 1).ToString();
            random_eight = RNG32.Next(10000000, 99999999).ToString();
            return random_eight;
        }
        public string padding(string q)
        {
            string t = q.Trim();
            char[] ns = t.ToCharArray();

            string padding = string.Join("3", ns);
            padding = "3" + padding;
            //MessageBox.Show(padding);
            ////richTextBox1.Text += padding;
            return padding;
        }
        public string Create32DigitString()
        {

            //var builder = new StringBuilder();
            //while (builder.Length < 32)
            //{
            //    builder.Append(RNG.Next().ToString("X"));
            //}
            //return builder.ToString();

            byte[] theBytes = new byte[16];
            RNG32.NextBytes(theBytes);
            StringBuilder buffer = new StringBuilder(32);
            for (int i = 0; i < 16; i++)
            {
                buffer.Append(theBytes[i].ToString("X").PadLeft(2, '0'));
            }
            //Thread.Sleep(1000);
            return buffer.ToString();
        }
        public string Create48DigitString()
        {
            byte[] theBytes = new byte[24];
            RNG32.NextBytes(theBytes);
            StringBuilder buffer = new StringBuilder(48);
            for (int i = 0; i < 24; i++)
            {
                buffer.Append(theBytes[i].ToString("X").PadLeft(2, '0'));
            }
            return buffer.ToString();
        }
        public string Create16DigitString()
        {


            //var builder = new StringBuilder();
            //while (builder.Length < 32)
            //{
            //    builder.Append(RNG.Next().ToString("X"));
            //}
            //return builder.ToString();

            byte[] theBytes = new byte[8];
            RNG16.NextBytes(theBytes);
            StringBuilder buffer = new StringBuilder(16);
            for (int i = 0; i < 8; i++)
            {
                buffer.Append(theBytes[i].ToString("X").PadLeft(2, '0'));
            }
            //Thread.Sleep(1000);
            return buffer.ToString();
        }
        public string Padding_16()
        {
            int datat = random16.Next(10000000, 99999999);
            string data = datat.ToString();
            string t = data.Trim();
            char[] ns = t.ToCharArray();

            string padding = string.Join("3", ns);
            padding = padding + "3";
            //MessageBox.Show(padding);
            ////richTextBox1.Text += padding;
            return padding;

        }
        //public string StringToHex(string st)
        //{
        //    string data = st;
        //    StringBuilder hex = new StringBuilder(data.Length * 2);
        //    foreach (char c in data)
        //    {
        //        hex.AppendFormat("{0:X2}", (int)c);
        //    }
        //    return hex.ToString();

        //}

        public string Pad3_F(string st)
        {
            string data = st;
            string t = data.Trim();
            char[] ns = t.ToCharArray();
            string padding = string.Join("3", ns);
            string pad_F = "";
            if (data.Length == 4)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    pad_F += "FF";
                }
            }
            padding = "3" + padding + pad_F;
            return padding;
        }
        public string Padding_8()
        {
            int datat = random8.Next(1000, 9999);
            string data = datat.ToString();
            string t = data.Trim();
            char[] ns = t.ToCharArray();

            string padding = string.Join("3", ns);
            padding = padding + "3FFFF";
            //MessageBox.Show(padding);
            ////richTextBox1.Text += padding;
            return padding;

        }
        public string nibble_swapped(string s)
        {
            s = s.Trim();
            string revs = "";
            if (s.Length > 0)
            {
                if (s.Length % 2 == 0)
                {
                    for (int i = 0; i < s.Length; i = i + 2) //String Reverse  
                    {
                        revs += s[i + 1].ToString();
                        revs += s[i].ToString();
                    }
                }
                else
                {
                    s += 'F';
                    for (int i = 0; i < s.Length; i = i + 2) //String Reverse  
                    {
                        revs += s[i + 1].ToString();
                        revs += s[i].ToString();
                    }
                }
            }
            else
                revs = "INVALID_NS_" + s;

            return revs;
        }
        public string sub_str(string s, int n)
        {
            s = s.Trim();
            string st = "";
            int i = 0;
            while (i + n < s.Length)
            {
                st += s.Substring(i, n) + ",";
                i += n;
            }
            return st + s.Substring(i, s.Length - i);
        }
        public string MSISDN_F(string s)
        {
            if (s.Length > 0)
            {
                if (s.Length % 2 != 0)
                {
                    s += 'F';
                }
            }
            else
                s = "INVALID_NS_" + s;

            return s;
        }
        public string acc(string imisi_acc)
        {

            int decNum = 0;
            string hex_acc = "";
            int i = 0;
            int rem = 0;

            string hexNum = "";


            decNum = (int)Math.Pow(2, Int32.Parse(imisi_acc.Substring(imisi_acc.Length - 1, 1)));


            while (decNum != 0)
            {
                rem = decNum % 16;
                if (rem < 10)
                    rem = rem + 48;
                else
                    rem = rem + 55;

                hexNum += Convert.ToChar(rem);
                decNum = decNum / 16;
            }






            //Console.Write("Hexa-decimal number :");
            for (i = hexNum.Length - 1; i >= 0; i--)
            { //Console.Write(hexNum[i]);
                hex_acc += hexNum[i];
            }
            //Console.Write("   ");
            //Console.Write(hex_acc);

            hex_acc = hex_acc.PadLeft(4, '0');

            //hexNum = hexNum.PadRight(4, '0');
            return hex_acc;





        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }
        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return ByteArrayToString(array);
        }
        //public int update(int record_id, string varname, string value)
        //{
        //    int pass = 0;
        //    string value_1 = value + "test";
        //    string enc_tag = ConfigurationManager.AppSettings["Data_Encryption_in_DB"];
        //    using (SqlConnection con1 = new SqlConnection(connectionString))
        //    {
        //        string Query = "UPDATE DataGenProcessDataRecord  SET  " + @varname + "   =  @value  WHERE DataGenProcessDataRecordID = @record_id  AND  DataGenProcessHDID =  @fileid  ";
        //        //value = "XXX" + value + "XXXX";
        //        using (SqlCommand com1 = new SqlCommand(Query, con1))
        //        {
        //            com1.Parameters.AddWithValue("@varname", varname);
        //            //com1.Parameters.AddWithValue("@fileid", EncryptString(value, "thisIsASecretKey"));
        //            com1.Parameters.AddWithValue("@fileid", fileid);
        //            if (enc_tag == "1")
        //            { com1.Parameters.AddWithValue("@value", EncryptString("3004455532FFFFFF", value)); }
        //            else
        //            { com1.Parameters.AddWithValue("@value", value); }
        //            com1.Parameters.AddWithValue("@record_id", record_id);
        //            //MessageBox.Show(record_id.ToString() + ' ' + varname + ' ' + value);
        //            try
        //            {
        //                con1.Open();
        //                com1.ExecuteNonQuery();
        //                pass = 1;
        //                con1.Close();
        //                //MessageBox.Show("Data Saved Successfully!");
        //            }
        //            catch (Exception exe)
        //            {
        //                pass = 0;
        //                MessageBox.Show(exe.Message);
        //            }
        //        }

        //    }
        //    return pass;
        //}
        public int update(int record_id, string varname, string value)
        {
            Console.WriteLine(record_id.ToString() + " : " + varname);
            int pass = 0;
            string enc_tag = ConfigurationManager.AppSettings["Data_Encryption_in_DB"];
            if (enc_tag == "1")
            {
                value = EncryptString("3004455532FFFFFF", value);
            }
            Process_data.AsEnumerable()
            .Where(row => Convert.ToInt32(row["DataGenProcessDataRecordID"]) == record_id)
            .ToList()
            .ForEach(row => row.SetField(varname, value));
            return pass;
        }
        public static string Encrypt(string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        string GetVarID(DataTable dt1, string valueToFind)
        {
            DataRow row = dt1.AsEnumerable().FirstOrDefault(r => r.Field<object>("VarName").Equals(valueToFind));
            return row != null ? row.Field<string>("VarID") : null;
        }
        private int btnProcessAll_Click()
        {
            int hsm_flag = 1;
            fileid = lastInsertedId;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string r4_data = "", r8_data = "";
            int r4_data_count = 0, r8_data_count = 0;
            List<string> r4_data_list = new List<string>();
            List<string> r8_data_list = new List<string>();
            string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlConnection con = new SqlConnection(constr);
            string filename_2 = getfilenameandidwithhdid();

            logString.Append($"    - [{Path.GetFileName(filename_2)}] File Processing Started.\n");
            Console.WriteLine($"    - [{Path.GetFileName(filename_2)}] File Processing Started.\n");
            String Query1 = $"SELECT DataGenProcessData.[DataGenProcessDataID], DataGenProcessData.[DataGenProcessHDID], InPutDataTemplate.[VarName] as VarID, DataGenProcessData.[VarName], DataGenProcessData.[VarValue], DataGenProcessData.[VarType], DataGenProcessData.[StatusID],[InPutDataTemplate].algoname,[InPutDataTemplate].VarText,[InPutDataTemplate].PositionFrom,[InPutDataTemplate].Len,InPutDataTemplate.tag, [InPutDataTemplate].LineNumber  FROM DataGenProcessData inner JOIN [InPutDataTemplate] ON [InPutDataTemplate].vardes = DataGenProcessData.varname where  DataGenProcessData.[DataGenProcessHDID] = '" + lastInsertedId + "' and [InPutDataTemplate].ProfileID=" + ProfileID + "  order by DataGenProcessData.VarID ";
            System.Data.DataTable dt1 = new System.Data.DataTable();
            DataRow workRow1;
            SqlCommand sqlcom1 = new SqlCommand(Query1, con);
            SqlDataAdapter adpt1 = new SqlDataAdapter(Query1, con);
            adpt1.Fill(dt1);

            r4_data_list.Clear();
            r8_data_list.Clear();
            Process_data = new DataTable();
            DataColumn idColumn = new DataColumn("DataGenProcessDataRecordID", typeof(int));
            idColumn.AutoIncrement = true;
            idColumn.AutoIncrementSeed = 1;
            Process_data.Columns.Add(idColumn);
            Process_data.Columns.Add("DataGenProcessHDID", typeof(int)).DefaultValue = lastInsertedId;
            string[] default_values = { "AGSUI:IMSI", "EKI", "KIND", "DATE_AL", "FSETIND", "A4IND", "Transport_key", "Quantity", "Index_Value" };
            foreach (DataRow dv0 in dt1.Rows)
            {

                string var_ID = dv0[2].ToString().TrimEnd();
                Console.WriteLine($"btnProcessAll_Click " + var_ID);
                string var_name = dv0[3].ToString().TrimEnd();
                string var_Value = dv0[4].ToString().TrimEnd();
                string var_Type = dv0[5].ToString().TrimEnd();
                if (var_name.TrimEnd() == "Quantity")
                {
                    records = Int32.Parse(var_Value.TrimEnd());
                }

                if (var_Type.TrimEnd() == "T")
                {
                    Process_data.Columns.Add(var_ID, typeof(string)).DefaultValue = var_Value;
                }
                else
                {
                    if (default_values.Contains(var_name.TrimEnd()))
                    {
                        Process_data.Columns.Add(var_ID, typeof(string)).DefaultValue = var_Value;
                    }
                    else
                    {
                        if (var_name.TrimEnd() == "ICCID" || var_name.TrimEnd() == "IMSI")
                        {
                            //DataColumn idColumn1 = new DataColumn(var_ID, typeof(long));
                            //idColumn1.AutoIncrement = true;
                            //idColumn1.AutoIncrementSeed = Int64.Parse(var_Value);
                            //Process_data.Columns.Add(idColumn1);
                            Process_data.Columns.Add(var_ID, typeof(string));
                        }
                        else if (var_name.TrimEnd() == "MSISDN")
                        {
                            if (var_Value.Contains('F'))
                            {
                                Process_data.Columns.Add(var_ID, typeof(string)).DefaultValue = var_Value;
                            }
                            else
                            {
                                //DataColumn idColumn1 = new DataColumn(var_ID, typeof(long));
                                //idColumn1.AutoIncrement = true;
                                //idColumn1.AutoIncrementSeed = Int64.Parse(var_Value);
                                //Process_data.Columns.Add(idColumn1);
                                Process_data.Columns.Add(var_ID, typeof(string));
                            }
                        }
                        else
                        {
                            Process_data.Columns.Add(var_ID, typeof(string));
                        }
                    }
                }
            }

            //DataRow newRow = Process_data.NewRow();
            //foreach (DataRow dv0 in dt1.Rows)
            //{
            //    string var_ID = dv0[2].ToString().TrimEnd();
            //    string var_name = dv0[3].ToString().TrimEnd();
            //    string var_Value = dv0[4].ToString().TrimEnd();
            //    string var_algoname = dv0[7].ToString().TrimEnd();
            //    string var_Type = dv0[5].ToString().TrimEnd();
            //    if (var_name.TrimEnd() == "ENCRYPTED KI")
            //    {
            //        newRow[var_ID] = Encrypt(Create32DigitString(), "DACE5E3B93AFAE0830D41C022B300597");
            //    }
            //}
            //Process_data.Rows.Add(newRow);
            logString.Append($"    - Establishing HSM Connection.\n");
            Console.WriteLine($"    - Establishing HSM Connection.\n");
            string url_1 = $"http://{hsm_IP.Trim()}/api/HSM?op_type=ip&digits=%27%27&keyname=%27%27&ip_data=%27%27";
            WebRequest request_1 = HttpWebRequest.Create(url_1);
            WebResponse response_1 = request_1.GetResponse();
            StreamReader reader_1 = new StreamReader(response_1.GetResponseStream());
            string urlText_1 = reader_1.ReadToEnd();
            if (urlText_1.Contains("CKR_DEVICE_REMOVED"))
            {
                logString.Append(" HSM CONNECTIVITY LOST Please check with Key Manager");
                Console.WriteLine($" HSM CONNECTIVITY LOST Please check with Key Manager");
                hsm_flag = 0;

            }

            if (hsm_flag == 0)
            {
                logString.AppendLine("\nFile processing stopped.");

                //break;
                return 10;
            }




            var hsm_data_1 = urlText_1.Split(',');







            //StreamReader sr1 = new StreamReader(filename_2);
            //string strPath1 = filename_2;
            //string[] lines = sr1.ReadToEnd().Split('\n');
            DataTable fl_data = new DataTable();
            using (SqlConnection connnection = new SqlConnection(connectionString))
            {
                connnection.Open();
                using (SqlCommand cmd = new SqlCommand($"SELECT IMSI,ICICID,MSISDN,LICENSE_KEY FROM [DGPDR_Base] WHERE DataGenProcessHDID={lastInsertedId} order by Sr_no ", connnection))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(fl_data);
                }
            }

            List<int> middleValues = dt1.AsEnumerable()
                            .Where(row => !string.IsNullOrEmpty(row.Field<string>("algoname")) && row.Field<string>("algoname").Contains("Hex") && row.Field<string>("algoname").Contains("R_"))
                            .Select(row =>
                            {
                                string[] parts = row.Field<string>("algoname").Split('_');
                                if (parts.Length > 1 && int.TryParse(parts[1], out int value))
                                    return value / 2;
                                else
                                    return 0; // or handle the case where conversion fails
                            })
                            .ToList();

            //List<int> middleValues = dt1.AsEnumerable()
            //                            .Where(row => !string.IsNullOrEmpty(row.Field<string>("algoname")) && row.Field<string>("algoname").Contains("Hex"))
            //                            .Select(row => Convert.ToInt32(row.Field<string>("algoname").Split('_')[1]))
            //                            .ToList();

            List<string> varIDS = dt1.AsEnumerable()
                                  .Where(row => !string.IsNullOrEmpty(row.Field<string>("algoname")) && row.Field<string>("algoname").Contains("Hex") && row.Field<string>("algoname").Contains("R_"))
                                  .Select(row => row.Field<string>("VarID").Trim())
                                  .ToList();









            for (int i = 1; i <= records; i++)
            {
                Dictionary<string, string> FetchAPI = new Dictionary<string, string>();


                if (records > 500)
                {
                    if (i % (records / 500) == 0)
                    {
                        string url = $"http://{hsm_IP.Trim()}/api/HSM?op_type=RND&digits={string.Join(",", middleValues)}&keyname=%27%27&ip_data=%27%27";
                        WebRequest request = HttpWebRequest.Create(url);
                        WebResponse response = request.GetResponse();
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string urlText_testing = reader.ReadToEnd() + "rem";
                        if (urlText_testing.Contains("CKR_DEVICE_REMOVED"))
                        {
                            logString.Append(" HSM CONNECTIVITY LOST Please check with Key Manager");
                            Console.WriteLine($" HSM CONNECTIVITY LOST Please check with Key Manager");
                            hsm_flag = 0;
                            break;
                        }
                    }
                }
                else if (records > 10)
                {
                    if (i % (records / 10) == 0)
                    {
                        string url = $"http://{hsm_IP.Trim()}/api/HSM?op_type=RND&digits={string.Join(",", middleValues)}&keyname=%27%27&ip_data=%27%27";
                        WebRequest request = HttpWebRequest.Create(url);
                        WebResponse response = request.GetResponse();
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string urlText_testing = reader.ReadToEnd() + "rem";
                        if (urlText_testing.Contains("CKR_DEVICE_REMOVED"))
                        {
                            logString.Append(" HSM CONNECTIVITY LOST Please check with Key Manager");
                            Console.WriteLine($" HSM CONNECTIVITY LOST Please check with Key Manager");
                            hsm_flag = 0;
                            break;
                        }
                    }
                }

                else
                {

                    string url = $"http://{hsm_IP.Trim()}/api/HSM?op_type=RND&digits={string.Join(",", middleValues)}&keyname=%27%27&ip_data=%27%27";
                    WebRequest request = HttpWebRequest.Create(url);
                    WebResponse response = request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string urlText_testing = reader.ReadToEnd() + "rem";
                    if (urlText_testing.Contains("CKR_DEVICE_REMOVED"))
                    {
                        logString.Append(" HSM CONNECTIVITY LOST Please check with Key Manager");
                        Console.WriteLine($" HSM CONNECTIVITY LOST Please check with Key Manager");
                        hsm_flag = 0;
                        break;
                    }

                }

                string urlText = random_hex_generator(string.Join(",", middleValues)) + "rem";


                if (urlText.Contains("CKR_DEVICE_REMOVED"))
                {
                    logString.Append(" HSM CONNECTIVITY LOST Please check with Key Manager");
                    Console.WriteLine($" HSM CONNECTIVITY LOST Please check with Key Manager");
                    hsm_flag = 0;
                    break;
                }

                urlText = urlText.Replace(",rem", "");
                var data = urlText.Split(',');
                int j = 0;
                foreach (string var in varIDS)
                {
                    FetchAPI[var] = data[j];
                    j++;
                }
                Console.WriteLine($"Processing Row : " + i);
                DataRow newRow = Process_data.NewRow();
                foreach (DataRow dv0 in dt1.Rows)
                {
                    try
                    {
                        string var_ID = dv0[2].ToString().TrimEnd();
                        Console.WriteLine(var_ID);
                        string var_name = dv0[3].ToString().TrimEnd();
                        string var_Value = dv0[4].ToString().TrimEnd();
                        string var_algoname = dv0[7].ToString().TrimEnd();
                        string var_op_type = dv0[8].ToString().TrimEnd();
                        string Pos_From = dv0[9].ToString().TrimEnd();
                        string len_data = dv0[10].ToString().TrimEnd();
                        string variableID = dv0[11].ToString().TrimEnd();
                        string lineno = dv0[12].ToString().TrimEnd();
                        int varCount = variableID.Count(c => c == ',');
                        if (var_op_type == "FL")
                        {
                            string my_data = "";

                            if (var_name.ToUpper() == "ICCID")
                            {
                                my_data = fl_data.Rows[i - 1]["ICICID"].ToString();
                            }
                            else if (var_name.ToUpper() == "MSISDN")
                            {
                                my_data = fl_data.Rows[i - 1]["MSISDN"].ToString();
                            }
                            else if (var_name.ToUpper() == "IMSI")
                            {
                                my_data = fl_data.Rows[i - 1]["IMSI"].ToString();
                            }
                            else if (var_name.ToUpper() == "LICENSE_KEY")
                            {
                                my_data = fl_data.Rows[i - 1]["LICENSE_KEY"].ToString();
                                if (string.IsNullOrEmpty(my_data))
                                {
                                    my_data = var_Value;
                                }
                            }
                            newRow[var_ID] = my_data;

                        }
                        if (var_op_type == "AL")
                        {
                            string caseSwitch = var_algoname;
                            string my_data = "";
                            r4_data = "";
                            List<string> ki_val_list = new List<string>();
                            switch (caseSwitch)
                            {

                                case "substring":
                                    if (varCount > 0)
                                    {
                                        MessageBox.Show($"More than one Variable found in AlgoName-{var_algoname}  in 'Tag' Value");
                                    }
                                    else
                                    {
                                        int pos_from = Convert.ToInt32(dv0[9].ToString().TrimEnd());
                                        int len = Convert.ToInt32(dv0[10].ToString().TrimEnd());
                                        string data_new_test = newRow[variableID].ToString();
                                        Console.WriteLine(data_new_test.TrimEnd() + " " + pos_from + " " + len);

                                        my_data = newRow[variableID].ToString().Substring(pos_from - 1, len);
                                    }
                                    break;

                                case "concat":
                                    if (varCount == 1)
                                    {
                                        //MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        string[] varIDs = variableID.Split(',');
                                        Console.WriteLine($"{newRow[varIDs[0]].ToString()} --->  {newRow[varIDs[1]].ToString()}");
                                        my_data = (newRow[varIDs[0]].ToString() + newRow[varIDs[1]].ToString());
                                    }
                                    else
                                    {
                                        MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                    }
                                    break;

                                case "identical":
                                    if (varCount > 0)
                                    {
                                        MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                    }
                                    else
                                    {
                                        my_data = newRow[variableID].ToString();
                                    }
                                    break;

                                case "serial":
                                    my_data = i.ToString();
                                    break;

                                case "R_4":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                        //r4_data_list.Add(padding_filler(my_data));
                                    }
                                    else
                                    {
                                        my_data = Random4digits();
                                        //r4_data_list.Add(padding_filler(my_data));
                                    }
                                    break;

                                case "R_8_H":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                        //r4_data_list.Add(padding_filler(my_data));
                                    }
                                    else
                                    {
                                        my_data = Random8hex();
                                        //r4_data_list.Add(padding_filler(my_data));
                                    }
                                    break;
                                case "R4_PF":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        if (varCount > 0)
                                        {
                                            MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        }
                                        else
                                        {
                                            my_data = padding_filler(newRow[variableID].ToString());
                                        }
                                    }
                                    break;

                                case "R_8":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                        r8_data_list.Add(padding(my_data));
                                    }
                                    else
                                    {
                                        my_data = Random8digits();
                                        r8_data_list.Add(padding(my_data));
                                    }
                                    break;

                                case "R8_P":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                        r8_data_count += 1;
                                    }
                                    else
                                    {
                                        my_data = r8_data_list[r8_data_count];
                                        r8_data_count += 1;
                                    }
                                    break;

                                case "ACC_Hex":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        //string varID = dv0[12].ToString().TrimEnd();// Var_From in db contains the varibles used in algo in AL case
                                        if (varCount > 0)
                                        {
                                            MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        }
                                        else
                                        {
                                            my_data = acc(newRow[variableID].ToString());
                                        }
                                        //my_data = acc((Int64.Parse(first_imsi) + i).ToString());

                                    }
                                    break;

                                case "3P":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        //my_data = padding((Int64.Parse(first_icicid) + i).ToString());
                                        my_data = padding(newRow[variableID].ToString());
                                    }
                                    break;

                                case "HEX":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        //my_data = padding((Int64.Parse(first_icicid) + i).ToString());
                                        my_data = StringToHex(newRow[variableID].ToString());
                                    }
                                    break;



                                case "R_16_Hex":

                                    //my_data = Create16DigitString();
                                    //my_data = FetchDataFromApi(16);
                                    my_data = FetchAPI[var_ID];
                                    break;

                                case "R_32_Hex":

                                    //my_data = Create32DigitString();
                                    //my_data = FetchDataFromApi(32);
                                    my_data = FetchAPI[var_ID];
                                    break;

                                case "R_48_Hex":

                                    //my_data = Create48DigitString();
                                    //my_data = FetchDataFromApi(48);
                                    my_data = FetchAPI[var_ID];
                                    break;

                                case "Pad_8":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        //my_data = padding((Int64.Parse(first_icicid) + i).ToString());
                                        my_data = Pad3_F(newRow[variableID].ToString());
                                    }
                                    //my_data = Padding_8();
                                    //my_data = Create32DigitString();
                                    break;

                                case "Pad_16":
                                    //my_data = Padding_16();
                                    //my_data = Create32DigitString();
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        //my_data = padding((Int64.Parse(first_icicid) + i).ToString());
                                        my_data = Pad3_F(newRow[variableID].ToString());
                                    }
                                    break;

                                case "ICCID_NS":
                                    if (varCount > 0)
                                    {
                                        MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                    }
                                    else
                                    {
                                        string icicid_num = newRow[variableID].ToString();
                                        my_data = nibble_swapped(icicid_num);
                                    }
                                    //string icicid_num = (Int64.Parse(first_icicid) + i).ToString();
                                    break;

                                case "IMSI_NS":
                                    if (varCount > 0)
                                    {
                                        MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                    }
                                    else
                                    {
                                        //string imsi_num = "809" + (Int64.Parse(first_imsi) + i).ToString();
                                        string imsi_num = "809" + newRow[variableID].ToString();
                                        my_data = nibble_swapped(imsi_num);
                                    }
                                    break;

                                case "R_32_Hex_KI":
                                    //my_data = Create32DigitString();
                                    //my_data = FetchDataFromApi(32);
                                    my_data = FetchAPI[var_ID];
                                    //ki_val =my_data.ToString();
                                    ki_val_list.Add(my_data);
                                    break;

                                case "ICCID_LD":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        if (varCount > 0)
                                        {
                                            MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        }
                                        else
                                        {
                                            my_data = newRow[variableID].ToString();
                                            //my_data = (Int64.Parse(first_icicid) + i).ToString();
                                            my_data += GetLuhnCheckDigit(newRow[variableID].ToString());
                                            Console.WriteLine(my_data);
                                        }
                                    }
                                    break;

                                case "KCV_AES":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        if (varCount > 0)
                                        {
                                            MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        }
                                        else
                                        {
                                            //my_data = padding((Int64.Parse(first_icicid) + i).ToString());
                                            my_data = CalculateKCV(newRow[variableID].ToString(), "AES");
                                        }
                                    }
                                    break;

                                case "KCV_DES":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        if (varCount > 0)
                                        {
                                            MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        }
                                        else
                                        {
                                            //my_data = padding((Int64.Parse(first_icicid) + i).ToString());
                                            my_data = CalculateKCV(newRow[variableID].ToString(), "DES");
                                        }
                                    }
                                    break;

                                case "MSISDN_F":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        if (varCount > 0)
                                        {
                                            MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        }
                                        else
                                        {
                                            my_data = newRow[variableID].ToString();
                                            my_data = MSISDN_F(my_data);
                                        }
                                    }
                                    break;

                                case "NS":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        if (varCount > 0)
                                        {
                                            MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        }
                                        else
                                        {
                                            my_data = newRow[variableID].ToString();
                                            my_data = nibble_swapped(my_data);
                                        }
                                    }
                                    break;

                                case "KI_AES_128":
                                    if (varCount == 1)
                                    {
                                        //MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        string[] varIDs = variableID.Split(',');
                                        Console.WriteLine($"{newRow[varIDs[0]].ToString()} --->  {newRow[varIDs[1]].ToString()}");
                                        my_data = AES_ENCYPRTION(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                    }
                                    //else if (varCount == 0)
                                    //{
                                    //    //my_data = Single_Des(newRow[variableID].ToString());
                                    //    my_data = AES_ENCYPRTION(newRow[variableID].ToString(), "db4389530b991a10b557246db732cd9a");

                                    //}
                                    else
                                    {
                                        MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                    }
                                    break;

                                case "Single_Des":
                                    if (varCount == 1)
                                    {
                                        //MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        string[] varIDs = variableID.Split(',');
                                        Console.WriteLine($"{newRow[varIDs[0]].ToString()} --->  {newRow[varIDs[1]].ToString()}");
                                        my_data = Encrypt_SingleDES(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                    }
                                    //else if (varCount == 0)
                                    //{
                                    //    //my_data = Single_Des(newRow[variableID].ToString());
                                    //    my_data = AES_ENCYPRTION(newRow[variableID].ToString(), "db4389530b991a10b557246db732cd9a");

                                    //}
                                    else
                                    {
                                        MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                    }
                                    break;

                                //case "KI_AES_128_2":
                                //    if (varCount == 1)
                                //    {
                                //        //MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                //        string[] varIDs = variableID.Split(',');
                                //        Console.WriteLine($"{newRow[varIDs[0]].ToString()} --->  {newRow[varIDs[1]].ToString()}");
                                //        my_data = AES_ENCYPRTION(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                //    }
                                //    //else if (varCount == 0)
                                //    //{
                                //    //    //my_data = Single_Des(newRow[variableID].ToString());
                                //    //    my_data = AES_ENCYPRTION(newRow[variableID].ToString(), "E8F8D8DCAA7DF2D372B0446C196E580C");

                                //    //}
                                //    else
                                //    {
                                //        MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                //    }
                                //    break;

                                case "AES_128_1":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        if (varCount > 0)
                                        {
                                            MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        }
                                        else
                                        {
                                            //my_data = Aes_128(ki_val);
                                            //my_data = Aes_128(newRow[variableID].ToString());
                                        }
                                    }
                                    break;
                                case "AES_128":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        if (varCount == 1)
                                        {
                                            //MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                            string[] varIDs = variableID.Split(',');
                                            my_data = OPC_GEN.opc(newRow[varIDs[1]].ToString(), newRow[varIDs[0]].ToString());
                                        }
                                        //else if (varCount == 0)
                                        //{
                                        //    //my_data = DESEncrypt(newRow[variableID].ToString());
                                        //    my_data = OPC_GEN.opc("436F6C6F72504C4153541220184E6F69", newRow[variableID].ToString());
                                        //    //my_data = OPC_GEN.opc("DACE5E3B93AFAE0830D41C022B300597", ki_val);

                                        //    //my_data = OPC_GEN.opc("586856051ea5e7d4122310a7093583d8", newRow[variableID].ToString());

                                        //}
                                        else
                                        {
                                            MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        }
                                    }
                                    break;

                                case "Triple_Des_CBC":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        if (varCount == 1)
                                        {
                                            //MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                            string[] varIDs = variableID.Split(',');
                                            //TripleDESEncrypt_cbc(data, key)
                                            my_data = TripleDESEncrypt_cbc(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                        }

                                        else
                                        {
                                            MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        }
                                    }
                                    break;
                            }
                            newRow[var_ID] = my_data;
                            //if (my_data != "")
                            //{

                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing var_ID: {dv0[2].ToString().TrimEnd()}, Error: {ex.Message}");
                        logString.AppendLine($"Error processing var_ID: {dv0[2].ToString().TrimEnd()}, Error: {ex.Message}");
                    }
                }



                Process_data.Rows.Add(newRow);
                Console.WriteLine($"Processed Row : " + i);


            }
            if (hsm_flag == 0)
            {
                logString.AppendLine("\nFile processing stopped.\n");

                //break;
                return 10;
            }
            else
            {
                logString.Append($"    - HSM Connection establised HSM PC IP is {hsm_data_1[0]}   ,  MAC-Address  is {hsm_data_1[1]} \n");
                Console.WriteLine($"    - HSM Connection establised HSM PC IP is {hsm_data_1[0]}   ,  MAC-Address  is {hsm_data_1[1]} \n");
                logString.Append($"    - Closing HSM Connection.\n");
                Console.WriteLine($"    - Closing HSM Connection.\n");
                try
                {
                    con.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {

                        bulkCopy.DestinationTableName = "DataGenProcessDataRecord";
                        foreach (DataColumn col in Process_data.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName.ToString(), col.ColumnName.ToString().Trim());
                        }
                        bulkCopy.BulkCopyTimeout = 1200;
                        bulkCopy.WriteToServer(Process_data);
                        UpdateInputFileSatus("Input_File", lastInsertedId);
                        Process_data.Clear();
                    }
                    con.Close();
                    stopwatch.Stop();
                    Console.WriteLine($"Filename : {Path.GetFileName(filename_2)}\nTime Taken : {stopwatch.Elapsed.ToString()}");
                    logString.Append($"    - Processed [{Path.GetFileName(filename_2)}] with FileID : {lastInsertedId} with Total no of records : {records}\n");
                    Console.WriteLine($"    - Processed [{Path.GetFileName(filename_2)}] with FileID : {lastInsertedId} with Total no of records : {records}\n");
                    return 1;
                }
                catch (Exception ex)
                {
                    logString.Append($"    - Error occurred during bulk copy.\n Column mapping failed" + ex.Message);
                    Console.WriteLine($"    - Error occurred during bulk copy.\n Column mapping failed");
                    using (SqlConnection del_con = new SqlConnection(connectionString))
                    {
                        del_con.Open();
                        SqlDataReader reader = null;
                        using (SqlCommand cmd = new SqlCommand($"Delete FROM [dbo].[DataGenProcessDataRecord] WHERE DataGenProcessHDID={lastInsertedId};)", con))
                        {
                            int rowsAffected = cmd.ExecuteNonQuery();
                        }
                        del_con.Close();
                    }
                    return 0;
                }
            }
        }
        public string AES_ENCYPRTION(string toEncrypt, string tk_key)
        {
            byte[] key = OPC_GEN.StrToByteArray(tk_key);
            byte[] data = OPC_GEN.StrToByteArray(toEncrypt);
            string result;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor();
                byte[] encryptedBytes = encryptor.TransformFinalBlock(data, 0, data.Length);
                //ICryptoTransform decryptor = aesAlg.CreateDecryptor();
                //byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                string encryptedHex = BitConverter.ToString(encryptedBytes).Replace("-", "").Substring(0, toEncrypt.Length).ToUpper();
                //string decryptedHex = BitConverter.ToString(decryptedBytes).Replace("-", "");
                result = encryptedHex;
            }
            return result;
        }
        public string Encrypt_SingleDES(string plainTextHex, string key_original)
        {
            byte[] key = HexStringToByteArray(key_original);
            if (key.Length != 8)
                throw new ArgumentException("Key must be 8 bytes (64 bits) for Single DES.");

            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = des.CreateEncryptor();
                byte[] inputBytes = HexStringToByteArray(plainTextHex);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

                return BitConverter.ToString(encryptedBytes).Replace("-", "").Substring(0, plainTextHex.Length).ToUpper();
            }
        }

        public static string TripleDESEncrypt_cbc(string hexData, string hexKey)
        {
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                byte[] data = HexStringToByteArray(hexData);
                byte[] key = HexStringToByteArray(hexKey);
                byte[] iv = new byte[8];
                tdes.Key = key;
                tdes.IV = iv;
                tdes.Mode = CipherMode.CBC;
                tdes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = tdes.CreateEncryptor())
                {
                    return BitConverter.ToString(encryptor.TransformFinalBlock(data, 0, data.Length)).Replace("-", "").Substring(0, hexData.Length);
                }
            }
        }
        public static byte[] HexStringToByteArray(string hex)
        {
            int NumberChars = hex.Length / 2;
            byte[] bytes = new byte[NumberChars];
            using (var sr = new StringReader(hex))
            {
                for (int i = 0; i < NumberChars; i++)
                    bytes[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
            }
            return bytes;
        }
        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length / 2;
            byte[] bytes = new byte[NumberChars];
            using (var sr = new StringReader(hex))
            {
                for (int i = 0; i < NumberChars; i++)
                    bytes[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
            }
            return bytes;
        }
        public string GetLuhnCheckDigit(string number)
        {
            var sum = 0;
            var alt = true;
            var digits = number.ToCharArray();

            // Iterate over the digits from right to left
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                var curDigit = (digits[i] - '0'); // Converts char to int directly

                if (alt)
                {
                    curDigit *= 2;
                    if (curDigit > 9)
                        curDigit -= 9;
                }

                sum += curDigit;
                alt = !alt;
            }

            int checkDigit = 10 - (sum % 10);

            // Check if the calculated sum modulo 10 equals 0
            if (checkDigit == 10)
                checkDigit = 0;

            // Log number and calculated check digit for debugging
            //Console.WriteLine($"Number: " + number);
            //Console.WriteLine($"Check Digit: " + checkDigit);

            // Append 'F' if the length is even, otherwise return the check digit as-is
            if (number.Length % 2 == 0)
            {
                return checkDigit.ToString() + 'F';
            }
            else
            {
                return checkDigit.ToString();
            }
        }

        static string StringToHex(string asciiString)
        {
            StringBuilder hex = new StringBuilder();

            foreach (char c in asciiString)
            {
                // Convert each character to its hexadecimal representation
                hex.Append(((int)c).ToString("X2"));
            }

            return hex.ToString();
        }
        static string HexToString(string hexString)
        {
            StringBuilder ascii = new StringBuilder();

            // Loop through the hex string, taking 2 characters (1 byte) at a time
            for (int i = 0; i < hexString.Length; i += 2)
            {
                // Convert each pair of hex characters to a byte
                string hexPair = hexString.Substring(i, 2);
                int charValue = Convert.ToInt32(hexPair, 16);

                // Append the ASCII character to the result string
                ascii.Append((char)charValue);
            }

            return ascii.ToString();
        }

        public string CalculateKCV(string key_value, string keytype_entered)
        {
            byte[] keyBytes = HexStringToByteArray(key_value);
            // Ensure the key length is either 16 bytes (2 keys) or 24 bytes (3 keys) for Triple DES
            if (keyBytes.Length != 16 && keyBytes.Length != 24)
            {
                throw new ArgumentException("Key must be 16 or 24 bytes for Triple DES.");
            }


            if (keytype_entered.ToUpper().Contains("DES"))
            {// 8-byte block of zeros for KCV calculation
                byte[] zeroBlock = new byte[8];

                using (var tripleDes = new TripleDESCryptoServiceProvider())
                {
                    tripleDes.Key = keyBytes;
                    tripleDes.Mode = CipherMode.ECB;
                    tripleDes.Padding = PaddingMode.None;

                    using (var encryptor = tripleDes.CreateEncryptor())
                    {
                        // Encrypt the zero block
                        byte[] encrypted = encryptor.TransformFinalBlock(zeroBlock, 0, zeroBlock.Length);

                        // Return the first 3 bytes of the encrypted block (KCV)
                        //return encrypted;
                        return BitConverter.ToString(encrypted).Replace("-", "").Substring(0, 6);
                    }
                }
            }
            else if (keytype_entered.ToUpper().Contains("AES"))
            { // 16-byte block of zeros for KCV calculation
                byte[] key = keyBytes;
                byte[] data = HexStringToByteArray("01010101010101010101010101010101");
                string result;
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.Mode = CipherMode.ECB;
                    aesAlg.Padding = PaddingMode.PKCS7;
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor();
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(data, 0, data.Length);
                    //ICryptoTransform decryptor = aesAlg.CreateDecryptor();
                    //byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    string encryptedHex = BitConverter.ToString(encryptedBytes).Replace("-", "").Substring(0, 32).ToUpper();
                    //string decryptedHex = BitConverter.ToString(decryptedBytes).Replace("-", "");
                    result = encryptedHex.ToUpper();
                }
                return result.Substring(0, 6);
                //byte[] zeroBlock = new byte[16];

                //using (var aes = new AesCryptoServiceProvider())
                //{
                //    aes.Key = keyBytes;
                //    aes.Mode = CipherMode.ECB;
                //    aes.Padding = PaddingMode.None;

                //    using (var encryptor = aes.CreateEncryptor())
                //    {
                //        // Encrypt the zero block
                //        byte[] encrypted = encryptor.TransformFinalBlock(zeroBlock, 0, zeroBlock.Length);

                //        // Return the first 3 bytes of the encrypted block (KCV) as a hex string
                //        return BitConverter.ToString(encrypted).Replace("-", "").Substring(0, 6);
                //    }
                //}
            }
            return "";
        }
        static string M2_padding(string hex_value)
        {
            string hex_value_padded = "";
            if (hex_value.Length % 32 == 0)
            {

                return hex_value;
            }
            else
            {

                int pad_length = hex_value.Length / 32;
                pad_length = (pad_length + 1) * 32;
                hex_value = hex_value + "8";
                hex_value_padded = hex_value.PadRight(pad_length, '0');
                return hex_value_padded;
            }
        }
        public static byte[] StrToByteArray(string str)
        {
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);

            List<byte> hexres = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
                hexres.Add(hexindex[str.Substring(i, 2)]);

            return hexres.ToArray();
        }
        static byte[] Encrypt_1(byte[] plainBytes, byte[] key, byte[] iv)
        {
            byte[] encryptedBytes = null;

            // Set up the encryption objects
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                //aes.Padding = PaddingMode.Zeros;

                // Encrypt the input plaintext using the AES algorithm
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                }
            }

            return encryptedBytes;
        }
        private void btnGenerateAllFiles_Click()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(DataGenProcessHDID) FROM DataGenProcessDataRecord WHERE DataGenProcessHDID = @hdid", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@hdid", OFProcessing.lastInsertedId);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        ProcessAllFile(ProfileID);
                    }
                    else
                        MessageBox.Show("File not processed. Please process the files first. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void ProcessAllFile(int customerProfileID)
        {
            string fileName = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlDataReader reader = null;

                    using (SqlCommand cmd = new SqlCommand("SELECT FILENAME FROM CustProfileFile where CustProfileID = @custProfileID and CustomerID=@custID and  FileIOID<>'I'", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("custProfileID", customerProfileID);
                        cmd.Parameters.AddWithValue("custID", customerID);
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                fileName = reader["FileName"].ToString();
                                generate_AllTypeOutput(fileName);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logString.Append("\nSomething went wrong with: " + fileName + ex.Message);
                Console.WriteLine($"\nSomething went wrong with: " + fileName + ex.Message);
                Console.WriteLine($"Exception: " + ex.Message);
                Console.WriteLine($"Stack trace: " + ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }
        public void generate_AllTypeOutput(string filetype)
        {
            try
            {
                string rootdir = "";
                string filenameconv = "";
                string filemasterid = "";
                string fileext = "";
                string CustProfileFileID = "";
                string last_imsi_footer = "", last_iccid_footer = "";
                int count = 0;
                string myfile = string.Empty;
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter($"SELECT FilePath,FileNamingConv,FileMasterID,FileExtn,CustProfileFileID FROM CustProfileFile where CustProfileID={ProfileID} AND CustomerID={customerID} AND FileName='{filetype}'", con))
                    {

                        sda.Fill(dt);
                        rootdir = dt.Rows[0][0].ToString().TrimEnd();
                        filenameconv = dt.Rows[0][1].ToString().TrimEnd();
                        filemasterid = dt.Rows[0][2].ToString().TrimEnd();
                        fileext = dt.Rows[0][3].ToString().TrimEnd();
                        CustProfileFileID = dt.Rows[0][4].ToString().TrimEnd();
                        Outfilelocation = rootdir + $"\\{customer}\\{profile}\\{FileProcessingLotID}_{lastInsertedId}_{unixTime}";
                        if (!Directory.Exists(Outfilelocation))
                        {
                            Directory.CreateDirectory(Outfilelocation);
                        }
                    }
                }
                string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();

                    String Query2 = $"SELECT Header FROM OutFileTemplateHD where [ProfileFileID]={filemasterid} and [ProfileID]={ProfileID}";
                    DataTable dt2 = new DataTable();
                    DataRow workRow2;
                    SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con);
                    adpt2.Fill(dt2);

                    int batch = lastInsertedId;
                    string Header = "";
                    try
                    {
                        Header = dt2.Rows[0][0].ToString();
                    }
                    catch { Header = ""; }
                    string pattern = @"\{([^{}]*)\}";
                    if (Header != "")
                    {

                        String Query1 = $"SELECT * FROM [DataGenProcessData] WHERE DataGenProcessHDID={lastInsertedId}";
                        System.Data.DataTable dt1 = new System.Data.DataTable();
                        DataRow workRow1;
                        SqlCommand sqlcom1 = new SqlCommand(Query1, con);
                        SqlDataAdapter adpt1 = new SqlDataAdapter(Query1, con);
                        adpt1.Fill(dt1);
                        //string pattern = @"\{([^{}]*)\}";
                        MatchCollection matches = Regex.Matches(Header, pattern);
                        if (matches.Count > 0)
                        {
                            foreach (Match match in matches)
                            {
                                string var = match.Groups[1].Value.ToString().Trim();
                                if (var.ToLower() == "profile")
                                {
                                    string rep_var = "{" + var + "}";
                                    string var_val = profile.Trim();
                                    Header = Header.Replace(rep_var, var_val);
                                }
                                else if (var.ToLower() == "customer")
                                {
                                    string rep_var = "{" + var + "}";
                                    string var_val = customer.Trim();
                                    Header = Header.Replace(rep_var, var_val);
                                }
                                else if (var.ToLower() == "last_imsi")
                                {
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        SqlCommand command = new SqlCommand($"select Top(1)V004 from DataGenProcessDataRecord where DataGenProcessHDID={lastInsertedId}  order by [DataGenProcessDataRecordID] desc", connection);
                                        connection.Open();
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                string id = reader.GetString(0);
                                                last_imsi_footer = id;
                                                string rep_var = "{" + var + "}";
                                                Header = Header.Replace(rep_var, id);
                                            }
                                        }
                                    }

                                }
                                else if (var.ToLower() == "last_iccid")
                                {
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        SqlCommand command = new SqlCommand($"select Top(1)V003 from DataGenProcessDataRecord where DataGenProcessHDID={lastInsertedId}  order by [DataGenProcessDataRecordID] desc", connection);
                                        connection.Open();
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                string id = reader.GetString(0);
                                                last_iccid_footer = "";
                                                string rep_var = "{" + var + "}";
                                                Header = Header.Replace(rep_var, id);
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    DataRow row = dt1.AsEnumerable()
                                               .FirstOrDefault(r => r.Field<string>("VarName").Trim() == var);
                                    string var_val = "";
                                    if (row != null)
                                    {
                                        var_val = row.Field<string>("VarValue").Trim();
                                    }
                                    string rep_var = "{" + var + "}";

                                    if (var.ToLower() == "batch")
                                    {
                                        if (string.IsNullOrEmpty(var_val))
                                        {
                                            Header = Header.Replace(rep_var, "");
                                        }
                                        else
                                        {
                                            Header = Header.Replace(rep_var, var_val);
                                            batch = Convert.ToInt32(var_val);
                                        }
                                    }
                                    else
                                    {
                                        Header = Header.Replace(rep_var, var_val);
                                    }
                                }
                            }


                        }
                    }

                    //adding footer code
                    String Query_footer = $"SELECT Footer FROM OutFileTemplateFT where [ProfileFileID]={filemasterid} and [ProfileID]={ProfileID}";
                    DataTable dt_footer = new DataTable();
                    DataRow workRow2_footer;
                    SqlDataAdapter adpt2_footer = new SqlDataAdapter(Query_footer, con);
                    adpt2_footer.Fill(dt_footer);
                    string Footer = "";
                    try
                    {
                        Footer = dt_footer.Rows[0][0].ToString();
                    }
                    catch { Footer = ""; }
                    if (Footer != "")
                    {

                        MatchCollection matches_footer = Regex.Matches(Footer, pattern);
                        if (matches_footer.Count > 0)
                        {
                            String Query1 = $"SELECT * FROM [DataGenProcessData] WHERE DataGenProcessHDID={lastInsertedId}";
                            System.Data.DataTable dt1 = new System.Data.DataTable();
                            DataRow workRow1;
                            SqlCommand sqlcom1 = new SqlCommand(Query1, con);
                            SqlDataAdapter adpt1 = new SqlDataAdapter(Query1, con);
                            adpt1.Fill(dt1);
                            foreach (Match match in matches_footer)
                            {
                                string var = match.Groups[1].Value.ToString().Trim();
                                if (var.ToLower() == "profile")
                                {
                                    string rep_var = "{" + var + "}";
                                    string var_val = profile.Trim();
                                    Footer = Footer.Replace(rep_var, var_val);
                                }
                                else if (var.ToLower() == "customer")
                                {
                                    string rep_var = "{" + var + "}";
                                    string var_val = customer.Trim();
                                    Footer = Footer.Replace(rep_var, var_val);
                                }
                                else if (var.ToLower() == "last_imsi")
                                {
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        SqlCommand command = new SqlCommand($"select Top(1)V004 from DataGenProcessDataRecord where DataGenProcessHDID={lastInsertedId}  order by [DataGenProcessDataRecordID] desc", connection);
                                        connection.Open();
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                string id = reader.GetString(0);
                                                string rep_var = "{" + var + "}";
                                                Footer = Footer.Replace(rep_var, id);
                                            }
                                        }
                                    }

                                }
                                else if (var.ToLower() == "last_iccid")
                                {
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        SqlCommand command = new SqlCommand($"select Top(1)V003 from DataGenProcessDataRecord where DataGenProcessHDID={lastInsertedId}  order by [DataGenProcessDataRecordID] desc", connection);
                                        connection.Open();
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                string id = reader.GetString(0);
                                                string rep_var = "{" + var + "}";
                                                Footer = Footer.Replace(rep_var, id);
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    DataRow row = dt1.AsEnumerable()
                                               .FirstOrDefault(r => r.Field<string>("VarName").Trim() == var);
                                    string var_val = "";
                                    if (row != null)
                                    {
                                        var_val = row.Field<string>("VarValue").Trim();
                                    }
                                    string rep_var = "{" + var + "}";

                                    if (var.ToLower() == "batch")
                                    {
                                        if (string.IsNullOrEmpty(var_val))
                                        {
                                            Footer = Footer.Replace(rep_var, "");
                                        }
                                        else
                                        {
                                            Footer = Footer.Replace(rep_var, var_val);
                                            batch = Convert.ToInt32(var_val);
                                        }
                                    }
                                    else
                                    {
                                        Footer = Footer.Replace(rep_var, var_val);
                                    }
                                }


                            }
                        }
                    }



                    if (filenameconv.Trim() == "FROMFILE")
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand($"SELECT t2.FilePath FROM  [CustProfileFile] t1 INNER JOIN [DataGenProcessHDFile] t2 on t1.CustProfileFileID=t2.CustProfileFileID WHERE FileIOID='I' and t2.DataGenProcessHDID={lastInsertedId}", connection);
                            connection.Open();
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    myfile = Outfilelocation + "\\" + Path.GetFileName(reader.GetString(0).Trim()).Split('.')[0] + fileext;
                                }
                            }
                        }
                    }
                    else
                    {

                        myfile = Outfilelocation + "\\" + filenameconv + unixTime + $"_{batch}{fileext}";
                    }

                    String Query3 = $"select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = {filemasterid} and FileLineNo = 1 and ProfileId={ProfileID} order by OutPutFileTemplateID,OutputTemplateLinesID";
                    DataTable dt3 = new DataTable();
                    DataRow workRow3;
                    SqlDataAdapter adpt3 = new SqlDataAdapter(Query3, con);
                    adpt3.Fill(dt3);
                    string qry = "";
                    foreach (DataRow dv in dt3.Rows)
                    {
                        string str_varname = dv[0].ToString().TrimEnd();
                        string str_VarType = dv[1].ToString();
                        if (str_VarType[0] == 'V')
                        {
                            qry += str_varname;
                        }

                        else if (str_VarType == "T")
                        {
                            qry += $"+'{str_varname}'+";
                        }

                        else if (str_VarType == "S")
                        {
                            using (SqlConnection con1 = new SqlConnection(connectionString))
                            {
                                SqlCommand com1 = new SqlCommand("select trim(Seperator) from [dbo].[SeperatorMaster] where SepID ='" + str_varname + "'", con1);
                                con1.Open();
                                SqlDataReader sqlDataReader = com1.ExecuteReader();
                                while (sqlDataReader.Read())
                                {
                                    string str = sqlDataReader.GetString(0);
                                    if (string.IsNullOrEmpty(str))
                                    { str = " "; }


                                    qry += "+'" + str + "'+";
                                }
                                con1.Close();
                            }

                        }

                    }
                    if (qry[qry.Length - 1] == '+')
                    {
                        qry = qry.Substring(0, qry.Length - 1);
                    }
                    String Query = $"select {qry} from DataGenProcessDataRecord where DataGenProcessHDID = '" + lastInsertedId + "'  order by [DataGenProcessDataRecordID]";
                    DataTable data = new DataTable();
                    SqlDataAdapter adpt = new SqlDataAdapter(Query, con);
                    adpt.Fill(data);
                    if (filetype == "XLSX")
                    {
                        string csvfile = myfile.Replace("xlsx", "csv");
                        using (StreamWriter writer = File.CreateText(csvfile))
                        {
                            if (Header != "")
                            {
                                writer.Write(Header + "\r\n");
                            }
                            foreach (DataRow dr in data.Rows)
                            {
                                writer.Write(dr[0].ToString() + "\r\n");
                                count++;
                            }
                            if (Footer != "")
                            {
                                writer.Write(Footer + "\r\n");
                            }

                        }
                        DataTable dataTable = ConvertCsvToDataTable(csvfile);
                        File.Delete(csvfile);
                        SaveDataTableToExcel(dataTable, myfile);
                    }
                    else
                    {
                        using (StreamWriter writer = File.CreateText(myfile))
                        {
                            if (Header != "")
                            {
                                writer.Write(Header + "\r\n");
                            }
                            foreach (DataRow dr in data.Rows)
                            {
                                writer.Write(dr[0].ToString() + "\r\n");
                                count++;
                            }
                            if (Footer != "")
                            {
                                writer.Write(Footer + "\r\n");
                            }
                        }
                    }
                    //string[] lines = File.ReadAllLines(myfile);
                    //if (fileext.ToLower().Trim() == ".mca" && IsSingle && lines.Length > 40000)
                    //{
                    //    int numFiles = (lines.Length) / 20000;
                    //    for (int i = 0; i <= numFiles; i++)
                    //    {
                    //        string outputFile = myfile.Replace(fileext, $"_Part_{i + 1}{fileext}");
                    //        using (StreamWriter writer = File.CreateText(outputFile))
                    //        {
                    //            writer.WriteLine(lines[0]);
                    //            for (int j = 1; (j <= 20000 && (j + i * 20000) <= lines.Length - 1); j++)
                    //            {
                    //                writer.WriteLine(lines[j + i * 20000]);
                    //            }
                    //        }

                    //    }
                    //    File.Delete(myfile);
                    //}
                    myfile = EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                }

                //myfile = EncryptionandDecryption.AESEncrypt_File(myfile, file_enc_key);
                logString.Append($"    - Filename : {Path.GetFileName(myfile)}\n");
                Console.WriteLine($"    - Filename : {Path.GetFileName(myfile)}\n");
                logString.Append($"    - Total no of record : {count}\n");
                Console.WriteLine($"    - Total no of record : {count}\n");
                logString.Append($"    - Input File FileID : {lastInsertedId}\n");
                Console.WriteLine($"    - Input File FileID : {lastInsertedId}\n");
                logString.Append($"    - Customer Profile FileID : {Convert.ToInt32(CustProfileFileID.Trim())}\n");
                Console.WriteLine($"    - Customer Profile FileID : {Convert.ToInt32(CustProfileFileID.Trim())}\n");

                UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(myfile)}", myfile, lastInsertedId);
                this.Invoke(new MethodInvoker(delegate
                {
                    txtoutput.Text += $"{filetype} OutFile created successfully for HDID {lastInsertedId}. \r\n";
                }));

                //MessageBox.Show($"{filetype} OutFile created successfully",
                //                        "Message",
                //                        MessageBoxButtons.OK,
                //                        MessageBoxIcon.Information
                //                        );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong while creating outfile {filetype} for HDID {lastInsertedId} error message" + ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
                logString.Append($"\nSomething went wrong while creating outfile {filetype} for HDID {lastInsertedId} error message" + ex.Message);
            }
            //GetGenProcessList();
        }


        public string getfilenameandidwithhdid()
        {
            string filename_12 = string.Empty;
            try
            {
                using (SqlConnection con3 = new SqlConnection(connectionString))
                {
                    con3.Open();
                    SqlDataReader reader = null;

                    List<string> filename1 = new List<string>();

                    using (SqlCommand cmd = new SqlCommand("usp_get_input_file_with_HDID", con3))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DataGenProcessHDID", lastInsertedId);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            filename1.Add(reader["FilePath"].ToString());
                        }
                        filename_12 = filename1[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while getting filename: " + ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            return filename_12;
        }


        //static string Single_Des(string plaintext)
        //{
        //    byte[] plaintextBytes = StringToByteArray(plaintext);
        //    byte[] keyBytes = StringToByteArray("43504C4153542018");

        //    // Ensure key is 8 bytes long (64 bits)
        //    if (keyBytes.Length != 8)
        //    {
        //        throw new ArgumentException("Key must be 8 bytes (64 bits) long in hexadecimal format");
        //    }

        //    // Create DES encryption object
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //    des.Key = keyBytes;
        //    des.Mode = CipherMode.ECB; // Electronic Codebook mode
        //    des.Padding = PaddingMode.None; // No padding

        //    // Create encryptor
        //    ICryptoTransform encryptor = des.CreateEncryptor();

        //    // Encrypt the plaintext
        //    byte[] encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

        //    // Convert encrypted bytes to hexadecimal string
        //    string encryptedHex = BitConverter.ToString(encryptedBytes).Replace("-", "");

        //    return encryptedHex;
        //}

        public static DataTable ConvertCsvToDataTable(string csvFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(csvFilePath))




            {
                string[] headers = sr.ReadLine().Split(','); // Read first row as headers
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    dt.Rows.Add(rows);

                }
            }
            return dt;
        }

        //static void SaveDataTableToExcel(DataTable dt, string excelFilePath)
        //{
        //    //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Required for EPPlus

        //    using (ExcelPackage excelPackage = new ExcelPackage())
        //    {
        //        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
        //        worksheet.Cells["A1"].LoadFromDataTable(dt, true);
        //        File.WriteAllBytes(excelFilePath, excelPackage.GetAsByteArray());
        //    }

        //}

        public static void SaveDataTableToExcel(DataTable dt, string excelFilePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(dt, "Sheet1");
                worksheet.Columns().AdjustToContents(); // Auto-fit columns
                workbook.SaveAs(excelFilePath);
            }
        }

        //private void DDMMYY()
        //{
        //    string myfile = string.Empty;
        //    string unixTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        using (SqlDataAdapter sda = new SqlDataAdapter($"SELECT FilePath FROM CustProfileFile where CustProfileID={ProfileID} AND FileName='DDMMYY'", con))
        //        {
        //            DataTable dt = new DataTable();
        //            sda.Fill(dt);

        //            Outfilelocation = dt.Rows[0][0].ToString() + unixTime + "\\Output";
        //            headerfilepath = dt.Rows[0][0].ToString() + unixTime + "\\HEADER";

        //            if (!Directory.Exists(Outfilelocation))
        //            {
        //                Directory.CreateDirectory(Outfilelocation);
        //            }
        //        }
        //    }
        //    string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        con.Open();

        //        String Query2 = "select DataGenProcessDataRecordID from DataGenProcessDataRecord  where DataGenProcessHDID = '" + fileid + "'";
        //        DataTable dt2 = new DataTable();
        //        DataRow workRow2;

        //        myfile = Outfilelocation + @"\AIR_DEL_128K_PRE_HLR5_PARTNERNAME_DDMMYY_" + unixTime + ".csv";
        //        File.AppendAllText(myfile, "ICCID,IMSI,PIN1,PUK1,PIN2,PUK2,ENCRYPTED KI\n");

        //        using (SqlCommand sqlcom2 = new SqlCommand(Query2, con))
        //        {
        //            SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con);
        //            adpt2.Fill(dt2);
        //            foreach (DataRow dv0 in dt2.Rows)
        //            {
        //                string record_no = dv0[0].ToString();
        //                if (!File.Exists(myfile))
        //                {
        //                    using (StreamWriter sw = File.CreateText(myfile))
        //                    { }

        //                }

        //                String Query3 = "select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = 4 and FileLineNo = 1 order by OutPutFileTemplateID,OutputTemplateLinesID";
        //                DataTable dt3 = new DataTable();
        //                DataRow workRow3;
        //                SqlDataAdapter adpt3 = new SqlDataAdapter(Query3, con);
        //                adpt3.Fill(dt3);

        //                foreach (DataRow dv in dt3.Rows)
        //                {
        //                    string str_varname = dv[0].ToString();
        //                    string str_VarType = dv[1].ToString();
        //                    if (str_VarType[0] == 'V')
        //                    {
        //                        String Query = "select * from DataGenProcessDataRecord where DataGenProcessDataRecordID =  '" + record_no + "' and DataGenProcessHDID = '" + fileid + "' ";
        //                        DataTable dt = new DataTable();
        //                        SqlDataAdapter adpt = new SqlDataAdapter(Query, con);
        //                        adpt.Fill(dt);



        //                        DataTable dtb = new DataTable();
        //                        dtb.Columns.Add("VarName");
        //                        dtb.Columns.Add("VarValue");
        //                        DataRow workRow;

        //                        foreach (DataRow dr in dt.Rows)
        //                        {
        //                            foreach (DataColumn dc in dt.Columns)
        //                            {

        //                                workRow = dtb.NewRow();
        //                                workRow[0] = dc.ColumnName;
        //                                workRow[1] = dr[dc.ColumnName].ToString();
        //                                dtb.Rows.Add(workRow);

        //                            }
        //                        }


        //                        string dta_1 = str_varname;

        //                        //dataGridView1.ReadOnly = true;

        //                        //dataGridView1.DataSource = dtb;

        //                        string expression;
        //                        expression = "VarName = '" + dta_1 + "'";
        //                        DataRow[] foundRows;

        //                        foundRows = dtb.Select(expression);

        //                        for (int i = 0; i < foundRows.Length; i++)
        //                        {
        //                            string data = foundRows[i][1].ToString();
        //                            File.AppendAllText(myfile, data.TrimEnd());

        //                        }
        //                    }
        //                    else if (str_VarType == "S")
        //                    {
        //                        using (SqlConnection con1 = new SqlConnection(connectionString))
        //                        {
        //                            SqlCommand com1 = new SqlCommand("select trim(Seperator) from [dbo].[SeperatorMaster] where SepID ='" + str_varname + "'", con1);
        //                            con1.Open();
        //                            SqlDataReader sqlDataReader = com1.ExecuteReader();
        //                            while (sqlDataReader.Read())
        //                            {
        //                                string str = sqlDataReader.GetString(0);
        //                                File.AppendAllText(myfile, str);
        //                            }
        //                            con1.Close();
        //                        }

        //                    }

        //                }
        //                File.AppendAllText(myfile, "\n");

        //            }
        //            //EncryptionandDecryption.EncryptFile(myfile, myfile + "_2", @"myKey123");
        //            //Pgp.EncryptFile(myfile + "_2", myfile, @"E:\ColorPlast\public.txt", true, true);
        //        }
        //    }

        //    //UpdateProcessHDFile("DDMMYY", 4, "AIR_DEL_128K_PRE_HLR5_PARTNERNAME_DDMMYY_" + unixTime + ".csv", myfile);
        //    //GetGenProcessList();
        //    try
        //    {
        //        //using (SqlConnection con3 = new SqlConnection(connectionString))
        //        //{
        //        //    con3.Open();
        //        //    using (SqlCommand cmd = new SqlCommand("usp_UpdateOutFileStatus", con3))
        //        //    {
        //        //        cmd.CommandType = CommandType.StoredProcedure;
        //        //        cmd.Parameters.AddWithValue("@CustProfileFileID", "7");
        //        //        cmd.Parameters.AddWithValue("@FileName", "Sample_JAVA_Card_CPS_" + unixTime + ".cps");
        //        //        cmd.Parameters.AddWithValue("@FilePath", Outfilelocation + @"\Sample_JAVA_Card_CPS_" + unixTime + ".cps");

        //        //        cmd.ExecuteReader();

        //        //        MessageBox.Show("CPS OutFile created successfully",
        //        //                        "Message",
        //        //                        MessageBoxButtons.OK,
        //        //                        MessageBoxIcon.Information
        //        //                        );


        //        //    }
        //        //}
        //        UpdateProcessHDFile("DDMMYY", 4, "AIR_DEL_128K_PRE_HLR5_PARTNERNAME_DDMMYY_" + unixTime + ".csv", myfile);
        //        MessageBox.Show("DDMMYY OutFile created successfully",
        //                                "Message",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information
        //                                );
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
        //                                "Error",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information
        //                                );
        //    }
        //    GetGenProcessList();

        //}
        private void UpdateProcessHDFile(string processFor, int fileID, string fileName, string filePath, int hdid)
        {
            try
            {

                using (SqlConnection con3 = new SqlConnection(connectionString))
                {
                    con3.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_UpdateOutFileStatus", con3))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustProfileFileID", fileID);
                        cmd.Parameters.AddWithValue("@FileName", fileName);
                        cmd.Parameters.AddWithValue("@FilePath", filePath);
                        cmd.Parameters.AddWithValue("@hdid", hdid);

                        cmd.ExecuteReader();

                        //MessageBox.Show("OutFile created successfully",
                        //                "Message",
                        //                MessageBoxButtons.OK,
                        //                MessageBoxIcon.Information
                        //                );


                    }
                }
            }
            catch (Exception ex)
            {
                logString.Append($"\nSomething went wrong with: {processFor}-{ex.Message}");
                Console.WriteLine($"\nSomething went wrong with: {processFor}-{ex.Message}");
                //MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
                //                        "Error",
                //                        MessageBoxButtons.OK,
                //                        MessageBoxIcon.Error
                //                        );
            }

        }
        //private void HLR()
        //{
        //    string myfile = string.Empty;
        //    string unixTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        using (SqlDataAdapter sda = new SqlDataAdapter($"SELECT FilePath FROM CustProfileFile where CustProfileID={ProfileID} AND FileName='HLR'", con))
        //        {
        //            DataTable dt = new DataTable();
        //            sda.Fill(dt);

        //            Outfilelocation = dt.Rows[0][0].ToString() + unixTime + "\\Output";

        //            if (!Directory.Exists(Outfilelocation))
        //            {
        //                Directory.CreateDirectory(Outfilelocation);
        //            }
        //        }
        //    }


        //    //dataGridView1.Rows.Clear();
        //    string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        con.Open();
        //        String Query2 = "select DataGenProcessDataRecordID from DataGenProcessDataRecord  where DataGenProcessHDID = '" + fileid + "'";
        //        DataTable dt2 = new DataTable();
        //        DataRow workRow2;

        //        using (SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con))
        //        {
        //            adpt2.Fill(dt2);
        //            foreach (DataRow dv0 in dt2.Rows)
        //            {

        //                string record_no = dv0[0].ToString();


        //                myfile = Outfilelocation + @"\AIR_DEL_POST_128K_ST_hlr_" + unixTime + ".auc";
        //                if (!File.Exists(myfile))
        //                {
        //                    // Create a file to write to.
        //                    using (StreamWriter sw = File.CreateText(myfile))
        //                    { }

        //                }

        //                String Query3 = "select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = 1 and FileLineNo = 1 order by OutPutFileTemplateID,OutputTemplateLinesID";
        //                DataTable dt3 = new DataTable();
        //                DataRow workRow3;
        //                SqlDataAdapter adpt3 = new SqlDataAdapter(Query3, con);
        //                adpt3.Fill(dt3);

        //                foreach (DataRow dv in dt3.Rows)
        //                {
        //                    string str_varname = dv[0].ToString();
        //                    string str_VarType = dv[1].ToString();
        //                    if (str_VarType[0] == 'V')
        //                    {
        //                        String Query = "select * from DataGenProcessDataRecord where DataGenProcessDataRecordID =  '" + record_no + "' and  DataGenProcessHDID = '" + fileid + "' ";
        //                        DataTable dt = new DataTable();
        //                        SqlDataAdapter adpt = new SqlDataAdapter(Query, con);
        //                        adpt.Fill(dt);

        //                        DataTable dtb = new DataTable();
        //                        dtb.Columns.Add("VarName");
        //                        dtb.Columns.Add("VarValue");
        //                        DataRow workRow;

        //                        foreach (DataRow dr in dt.Rows)
        //                        {
        //                            foreach (DataColumn dc in dt.Columns)
        //                            {

        //                                workRow = dtb.NewRow();
        //                                workRow[0] = dc.ColumnName;
        //                                workRow[1] = dr[dc.ColumnName].ToString();
        //                                dtb.Rows.Add(workRow);

        //                            }
        //                        }


        //                        string dta_1 = str_varname;

        //                        //dataGridView1.ReadOnly = true;

        //                        //dataGridView1.DataSource = dtb;

        //                        string expression;
        //                        expression = "VarName = '" + dta_1 + "'";
        //                        DataRow[] foundRows;

        //                        foundRows = dtb.Select(expression);

        //                        for (int i = 0; i < foundRows.Length; i++)
        //                        {
        //                            string data = foundRows[i][1].ToString();
        //                            File.AppendAllText(myfile, data.TrimEnd());

        //                        }
        //                    }
        //                    else if (str_VarType == "S")
        //                    {
        //                        using (SqlConnection con1 = new SqlConnection(connectionString))
        //                        {
        //                            SqlCommand com1 = new SqlCommand("select trim(Seperator) from [dbo].[SeperatorMaster] where SepID ='" + str_varname + "'", con1);
        //                            con1.Open();
        //                            SqlDataReader sqlDataReader = com1.ExecuteReader();
        //                            while (sqlDataReader.Read())
        //                            {
        //                                string str = sqlDataReader.GetString(0);
        //                                File.AppendAllText(myfile, str);
        //                            }
        //                            con1.Close();
        //                        }
        //                    }
        //                }
        //                File.AppendAllText(myfile, "\n");
        //            }
        //            //EncryptionandDecryption.EncryptFile(myfile, myfile, "HR$2pIjHR$2pIj12");

        //            //EncryptionandDecryption.EncryptFile(myfile, myfile+"_2", @"myKey123");

        //            Pgp.EncryptFile(myfile + "_2", myfile, @"C:\Users\vishvajeet.arya\Downloads\openssl-0.9.8h-1-bin\bin\mypublickey.pem", true, true);

        //            //Pgp.DecryptFile("Resources/output.txt", "Resources/privateKey.txt", "pass".ToCharArray(), "default.txt");

        //        }
        //    }
        //    try
        //    {
        //        UpdateProcessHDFile("HLR", 1, "AIR_DEL_POST_128K_ST_hlr_" + unixTime + ".auc", myfile);
        //        MessageBox.Show("HLR OutFile created successfully",
        //                                "Message",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information
        //                                );
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
        //                                "Error",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information
        //                                );
        //    }
        //    GetGenProcessList();
        //}
        //private void HSS()
        //{
        //    string myfile = string.Empty;
        //    string unixTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        using (SqlDataAdapter sda = new SqlDataAdapter($"SELECT FilePath FROM CustProfileFile where CustProfileID={ProfileID} AND FileName='HSS'", con))
        //        {
        //            DataTable dt = new DataTable();
        //            sda.Fill(dt);

        //            Outfilelocation = dt.Rows[0][0].ToString() + unixTime + "\\Output";

        //            if (!Directory.Exists(Outfilelocation))
        //            {
        //                Directory.CreateDirectory(Outfilelocation);
        //            }
        //        }
        //    }
        //    string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        con.Open();
        //        String Query2 = "select DataGenProcessDataRecordID from DataGenProcessDataRecord  where   DataGenProcessHDID = '" + fileid + "'";
        //        DataTable dt2 = new DataTable();
        //        DataRow workRow2;



        //        using (SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con))
        //        {

        //            adpt2.Fill(dt2);
        //            foreach (DataRow dv0 in dt2.Rows)
        //            {

        //                string record_no = dv0[0].ToString();
        //                myfile = Outfilelocation + @"\AIR_DEL_POST_128K_ST_hss_" + unixTime + ".auc";
        //                if (!File.Exists(myfile))
        //                {
        //                    // Create a file to write to.
        //                    using (StreamWriter sw = File.CreateText(myfile))
        //                    { }

        //                }

        //                String Query3 = "select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = 2 and FileLineNo = 1 order by OutPutFileTemplateID,OutputTemplateLinesID";
        //                DataTable dt3 = new DataTable();
        //                DataRow workRow3;
        //                SqlDataAdapter adpt3 = new SqlDataAdapter(Query3, con);
        //                adpt3.Fill(dt3);
        //                //dataGridView1.ReadOnly = true;
        //                //dataGridView1.DataSource = dt3;

        //                foreach (DataRow dv in dt3.Rows)
        //                {
        //                    string str_varname = dv[0].ToString();
        //                    string str_VarType = dv[1].ToString();
        //                    //str_VarType = str_VarType[0].ToString();
        //                    if (str_VarType[0] == 'V')
        //                    {
        //                        //MessageBox.Show(varname);
        //                        String Query = "select * from DataGenProcessDataRecord where DataGenProcessDataRecordID =  '" + record_no + "'  and DataGenProcessHDID = '" + fileid + "' ";
        //                        DataTable dt = new DataTable();
        //                        SqlDataAdapter adpt = new SqlDataAdapter(Query, con);
        //                        adpt.Fill(dt);



        //                        DataTable dtb = new DataTable();
        //                        dtb.Columns.Add("VarName");
        //                        dtb.Columns.Add("VarValue");
        //                        DataRow workRow;

        //                        foreach (DataRow dr in dt.Rows)
        //                        {
        //                            foreach (DataColumn dc in dt.Columns)
        //                            {

        //                                workRow = dtb.NewRow();
        //                                workRow[0] = dc.ColumnName;
        //                                workRow[1] = dr[dc.ColumnName].ToString();
        //                                dtb.Rows.Add(workRow);

        //                            }
        //                        }


        //                        string dta_1 = str_varname;

        //                        //dataGridView1.ReadOnly = true;

        //                        //dataGridView1.DataSource = dtb;

        //                        string expression;
        //                        expression = "VarName = '" + dta_1 + "'";
        //                        DataRow[] foundRows;

        //                        // Use the Select method to find all rows matching the filter.
        //                        foundRows = dtb.Select(expression);

        //                        // Print column 0 of each returned row.
        //                        for (int i = 0; i < foundRows.Length; i++)
        //                        {
        //                            string data = foundRows[i][1].ToString();
        //                            File.AppendAllText(myfile, data.TrimEnd());

        //                        }
        //                    }
        //                    else if (str_VarType == "S")
        //                    {
        //                        using (SqlConnection con1 = new SqlConnection(connectionString))
        //                        {
        //                            SqlCommand com1 = new SqlCommand("select trim(Seperator) from [dbo].[SeperatorMaster] where SepID ='" + str_varname + "'", con1);
        //                            con1.Open();

        //                            SqlDataReader sqlDataReader = com1.ExecuteReader();

        //                            while (sqlDataReader.Read())
        //                            {
        //                                string str = sqlDataReader.GetString(0);

        //                                File.AppendAllText(myfile, str);
        //                            }
        //                            con1.Close();
        //                        }
        //                    }
        //                }
        //                File.AppendAllText(myfile, "\n");

        //            }
        //            //EncryptionandDecryption.EncryptFile(myfile, myfile + "_2", @"myKey123");
        //            //Pgp.EncryptFile(myfile + "_2", myfile, @"E:\ColorPlast\public.txt", true, true);
        //        }
        //    }
        //    //MessageBox.Show("File Built");


        //    try
        //    {
        //        UpdateProcessHDFile("HSS", 2, "AIR_DEL_POST_128K_ST_hss_" + unixTime + ".auc", myfile);
        //        MessageBox.Show("HSS OutFile created successfully",
        //                                "Message",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information
        //                                );
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
        //                                "Error",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information
        //                                );
        //    }
        //    GetGenProcessList();

        //}
        //private void DSA()
        //{
        //    string myfile = string.Empty;
        //    string unixTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        using (SqlDataAdapter sda = new SqlDataAdapter($"SELECT FilePath FROM CustProfileFile where CustProfileID={ProfileID} AND FileName='DSA'", con))
        //        {
        //            DataTable dt = new DataTable();
        //            sda.Fill(dt);

        //            Outfilelocation = dt.Rows[0][0].ToString() + unixTime + "\\Output";
        //            headerfilepath = Directory.GetCurrentDirectory() + "\\HEADER";

        //            if (!Directory.Exists(Outfilelocation))
        //            {
        //                Directory.CreateDirectory(Outfilelocation);
        //            }
        //        }
        //    }
        //    string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        con.Open();
        //        String Query2 = "select DataGenProcessDataRecordID from DataGenProcessDataRecord  where   DataGenProcessHDID = '" + fileid + "'";
        //        DataTable dt2 = new DataTable();
        //        DataRow workRow2;

        //        string headerfile = headerfilepath + @"\AIR_DEL_POST_128K_ST_DSA_Header.AUC";
        //        myfile = Outfilelocation + @"\AIR_DEL_POST_128K_ST_DSA_" + unixTime + ".AUC";
        //        File.Copy(headerfile, myfile, true);
        //        File.AppendAllText(myfile, "\n");

        //        using (SqlCommand sqlcom2 = new SqlCommand(Query2, con))
        //        {
        //            SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con);
        //            adpt2.Fill(dt2);
        //            foreach (DataRow dv0 in dt2.Rows)
        //            {
        //                string record_no = dv0[0].ToString();
        //                if (!File.Exists(myfile))
        //                {
        //                    // Create a file to write to.
        //                    using (StreamWriter sw = File.CreateText(myfile))
        //                    { }

        //                }

        //                String Query3 = "select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = 5 and FileLineNo = 1 order by OutPutFileTemplateID,OutputTemplateLinesID";
        //                DataTable dt3 = new DataTable();
        //                DataRow workRow3;
        //                SqlDataAdapter adpt3 = new SqlDataAdapter(Query3, con);
        //                adpt3.Fill(dt3);

        //                foreach (DataRow dv in dt3.Rows)
        //                {
        //                    string str_varname = dv[0].ToString();
        //                    string str_VarType = dv[1].ToString();
        //                    if (str_VarType[0] == 'V')
        //                    {
        //                        String Query = "select * from DataGenProcessDataRecord where DataGenProcessDataRecordID =  '" + record_no + "'  and DataGenProcessHDID = '" + fileid + "'  ";
        //                        DataTable dt = new DataTable();
        //                        SqlDataAdapter adpt = new SqlDataAdapter(Query, con);
        //                        adpt.Fill(dt);



        //                        DataTable dtb = new DataTable();
        //                        dtb.Columns.Add("VarName");
        //                        dtb.Columns.Add("VarValue");
        //                        DataRow workRow;

        //                        foreach (DataRow dr in dt.Rows)
        //                        {
        //                            foreach (DataColumn dc in dt.Columns)
        //                            {

        //                                workRow = dtb.NewRow();
        //                                workRow[0] = dc.ColumnName;
        //                                workRow[1] = dr[dc.ColumnName].ToString();
        //                                dtb.Rows.Add(workRow);

        //                            }
        //                        }


        //                        string dta_1 = str_varname;

        //                        //dataGridView1.ReadOnly = true;

        //                        //dataGridView1.DataSource = dtb;

        //                        string expression;
        //                        expression = "VarName = '" + dta_1 + "'";
        //                        DataRow[] foundRows;

        //                        // Use the Select method to find all rows matching the filter.
        //                        foundRows = dtb.Select(expression);

        //                        // Print column 0 of each returned row.
        //                        for (int i = 0; i < foundRows.Length; i++)
        //                        {
        //                            string data = foundRows[i][1].ToString();
        //                            File.AppendAllText(myfile, data.TrimEnd());

        //                        }
        //                    }
        //                    else if (str_VarType == "S")
        //                    {
        //                        using (SqlConnection con1 = new SqlConnection(connectionString))
        //                        {
        //                            SqlCommand com1 = new SqlCommand("select Seperator from [dbo].[SeperatorMaster] where SepID ='" + str_varname + "'", con1);
        //                            con1.Open();
        //                            SqlDataReader sqlDataReader = com1.ExecuteReader();

        //                            while (sqlDataReader.Read())
        //                            {
        //                                string str = sqlDataReader.GetString(0);
        //                                File.AppendAllText(myfile, str);
        //                            }
        //                            con1.Close();
        //                        }

        //                    }

        //                }
        //                File.AppendAllText(myfile, "\n");
        //            }
        //            //EncryptionandDecryption.EncryptFile(myfile, myfile + "_2", @"myKey123");
        //            //Pgp.EncryptFile(myfile + "_2", myfile, @"E:\ColorPlast\public.txt", true, true);
        //        }
        //    }
        //    //MessageBox.Show("File Built");



        //    try
        //    {
        //        UpdateProcessHDFile("DSA", 5, "AIR_DEL_POST_128K_ST_DSA.AUC", myfile);
        //        MessageBox.Show("DSA OutFile created successfully",
        //                                "Message",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information
        //                                );
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
        //                                "Error",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information
        //                                );
        //    }
        //    GetGenProcessList();
        //}
        //private void MCA()
        //{
        //    string myfile = string.Empty;
        //    DataTable dt = new DataTable();
        //    string unixTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        using (SqlDataAdapter sda = new SqlDataAdapter($"SELECT FilePath,FileNamingConv FROM CustProfileFile where CustProfileID={ProfileID} AND FileName='MCA'", con))
        //        {

        //            sda.Fill(dt);

        //            Outfilelocation = dt.Rows[0][0].ToString() + unixTime + "\\Output";
        //            myfile = Outfilelocation + "\\" + dt.Rows[0][1].ToString() + unixTime + ".txt";
        //            if (!Directory.Exists(Outfilelocation))
        //            {
        //                Directory.CreateDirectory(Outfilelocation);
        //            }
        //        }
        //    }
        //    string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        con.Open();

        //        String Query2 = "select DataGenProcessDataRecordID from DataGenProcessDataRecord  where   DataGenProcessHDID = '" + fileid + "'   order by DataGenProcessDataRecordID    ";
        //        DataTable dt2 = new DataTable();
        //        DataRow workRow2;

        //        //string headerfile = headerfilepath + @"\AIR_DEL_POST_128K_ST_MCA.txt";
        //        //myfile = Outfilelocation + @"\AIR_DEL_POST_128K_ST_MCA_" + unixTime + ".txt";
        //        //File.Copy(headerfile, myfile, true);
        //        File.AppendAllText(myfile, "ICCID,IMSI,ACC,DPIN1,DPIN2,DPUK1,DPUK2,ADM1,KI,OPC,KIC1,KID1,KIK1,KIC2,KID2,KIK2,KIC3,KID3,KIK3,PSK,DEK1,AICCID,ASCII_ICCID,LICENSE_KEY\n");
        //        if (!File.Exists(myfile))
        //        {
        //            // Create a file to write to.
        //            using (StreamWriter sw = File.CreateText(myfile))
        //            { }

        //        }

        //        String Query3 = "select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = 3 and FileLineNo = 1 order by OutPutFileTemplateID,OutputTemplateLinesID";
        //        DataTable dt3 = new DataTable();
        //        DataRow workRow3;
        //        SqlDataAdapter adpt3 = new SqlDataAdapter(Query3, con);
        //        adpt3.Fill(dt3);
        //        string qry = "";
        //        foreach (DataRow dv in dt3.Rows)
        //        {
        //            string str_varname = dv[0].ToString().Trim();
        //            string str_VarType = dv[1].ToString();
        //            if (str_VarType[0] == 'V')
        //            {
        //                qry += str_varname;
        //            }
        //            else if (str_VarType == "S")
        //            {
        //                using (SqlConnection con1 = new SqlConnection(connectionString))
        //                {
        //                    SqlCommand com1 = new SqlCommand("select trim(Seperator) from [dbo].[SeperatorMaster] where SepID ='" + str_varname + "'", con1);
        //                    con1.Open();
        //                    SqlDataReader sqlDataReader = com1.ExecuteReader();
        //                    while (sqlDataReader.Read())
        //                    {
        //                        string str = sqlDataReader.GetString(0);
        //                        qry += "+'" + str.Trim() + "'+";
        //                    }
        //                    con1.Close();
        //                }

        //            }

        //        }
        //        String Query = $"select {qry} from DataGenProcessDataRecord where DataGenProcessHDID = '" + fileid + "'  ";
        //        DataTable data = new DataTable();
        //        SqlDataAdapter adpt = new SqlDataAdapter(Query, con);
        //        adpt.Fill(data);
        //        foreach (DataRow dr in data.Rows)
        //        {
        //            File.AppendAllText(myfile, dr[0].ToString() + "\n");
        //        }
        //    }
        //    try
        //    {
        //        UpdateProcessHDFile($"{dt.Rows[0][0].ToString()}", 3, $"{dt.Rows[0][1].ToString()}" + unixTime + ".txt", myfile);
        //        MessageBox.Show($"{dt.Rows[0][0].ToString()} OutFile created successfully",
        //                                "Message",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information
        //                                );
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
        //                                "Error",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information
        //                                );
        //    }
        //    GetGenProcessList();
        //}
        //private void CPS()
        //{
        //    string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //    string myfile = string.Empty;
        //    string unixTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        using (SqlDataAdapter sda = new SqlDataAdapter($"SELECT FilePath FROM CustProfileFile where CustProfileID={ProfileID} AND FileName='CPS'", con))
        //        {
        //            DataTable dt = new DataTable();
        //            sda.Fill(dt);

        //            Outfilelocation = dt.Rows[0][0].ToString() + unixTime + "\\Output";
        //            headerfilepath = Directory.GetCurrentDirectory() + unixTime + "\\HEADER";

        //            if (!Directory.Exists(Outfilelocation))
        //            {
        //                Directory.CreateDirectory(Outfilelocation);
        //            }

        //        }
        //    }
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        con.Open();
        //        String Query2 = "select DataGenProcessDataRecordID from DataGenProcessDataRecord  where   DataGenProcessHDID = '" + fileid + "'  order by DataGenProcessDataRecordID";
        //        DataTable dt2 = new DataTable();
        //        DataRow workRow2;

        //        string headerfile = headerfilepath + @"\Sample_JAVA_Card_CPS.cps";
        //        string headerfile1 = headerfilepath + @"\Sample_JAVA_Card_CPS_records_wise.cps";
        //        myfile = Outfilelocation + @"\Sample_JAVA_Card_CPS_" + unixTime + ".cps";
        //        File.Copy(headerfile, myfile, true);
        //        File.AppendAllText(myfile, "\n");

        //        using (SqlCommand sqlcom2 = new SqlCommand(Query2, con))
        //        {
        //            SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con);
        //            adpt2.Fill(dt2);
        //            foreach (DataRow dv0 in dt2.Rows)
        //            {
        //                string record_no = dv0[0].ToString();
        //                //string myfile = @"B:\DATA-FILES\DATATOOL\OUTPUT FILES\AIR_DEL_POST_128K_ST_DSA.AUC";
        //                if (!File.Exists(myfile))
        //                {
        //                    // Create a file to write to.
        //                    using (StreamWriter sw = File.CreateText(myfile))
        //                    { }

        //                }

        //                ////added for header file

        //                //StreamReader reading_1 = File.OpenText(headerfile);
        //                //string str_1,str_12 = string.Empty;

        //                //string bar_1 = string.Empty;
        //                //int start_1 = 0;
        //                //while ((str_1 = reading_1.ReadLine()) != null)
        //                //{
        //                //    start_1 = 0;
        //                //    bar_1 = string.Empty;
        //                //    if (str_1.Contains("#"))
        //                //    {
        //                //        //Console.WriteLine(str);

        //                //        foreach (char c in str_1)
        //                //        {
        //                //            if (c == '#' && start_1 == 1)
        //                //            { start_1 = 0; }
        //                //            else if (c == '#')
        //                //            { start_1 = 1; }

        //                //            if (start_1 == 1)
        //                //            { bar_1 += c; }
        //                //        }
        //                //    }
        //                //    if (bar_1 != "")

        //                //    {
        //                //        bar_1 = bar_1.Replace("#", "");
        //                //        using (SqlConnection con_cps_1 = new SqlConnection(constr))
        //                //        {
        //                //            con_cps_1.Open();
        //                //            SqlCommand com_cps = new SqlCommand("SELECT " + (bar_1) + "  FROM  [DataGenProcessDataRecord]  where [DataGenProcessDataRecordID] = " + record_no + "  and DataGenProcessHDID =  '" + fileid + "'", con_cps_1);

        //                //            //  SqlDataAdapter da1 = new SqlDataAdapter(com1);
        //                //            SqlDataReader sqlDataReader = com_cps.ExecuteReader();
        //                //            //  da1.Fill(ptDataset1);
        //                //            //   dataGridView1.DataSource = ptDataset1.Tables[0];
        //                //            while (sqlDataReader.Read())
        //                //            {
        //                //                str_12 = sqlDataReader.GetString(0).Trim();

        //                //                //richTextBox1.Text += str;
        //                //                //Console.WriteLine(str_3);
        //                //                //using (StreamWriter sw = File.AppendText(myfile))
        //                //                //{
        //                //                //    sw.WriteLine(str);
        //                //                //}

        //                //            }

        //                //            //Console.WriteLine(bar);
        //                //            string bar__11 = "#" + bar_1 + "#";
        //                //            //Console.WriteLine(bar_1);
        //                //            str_1 = str_1.Replace(bar__11, str_12);

        //                //            //Console.WriteLine(str_1);
        //                //            con_cps_1.Close();
        //                //        }

        //                //    }

        //                //    File.AppendAllText(myfile, str_1);
        //                //    File.AppendAllText(myfile, "\n");



        //                //}
        //                ////

        //                StreamReader reading = File.OpenText(headerfile1);
        //                string str, str_3 = string.Empty;

        //                string bar = string.Empty;
        //                int start = 0;
        //                while ((str = reading.ReadLine()) != null)
        //                {
        //                    start = 0;
        //                    bar = string.Empty;
        //                    if (str.Contains("#"))
        //                    {
        //                        //Console.WriteLine(str);

        //                        foreach (char c in str)
        //                        {
        //                            if (c == '#' && start == 1)
        //                            { start = 0; }
        //                            else if (c == '#')
        //                            { start = 1; }

        //                            if (start == 1)
        //                            { bar += c; }
        //                        }
        //                    }
        //                    if (bar != "")

        //                    {
        //                        bar = bar.Replace("#", "");
        //                        using (SqlConnection con_cps = new SqlConnection(constr))
        //                        {
        //                            con_cps.Open();
        //                            SqlCommand com_cps = new SqlCommand("SELECT " + (bar) + "  FROM  [DataGenProcessDataRecord]  where [DataGenProcessDataRecordID] = " + record_no + "  and DataGenProcessHDID =  '" + fileid + "'", con_cps);

        //                            //  SqlDataAdapter da1 = new SqlDataAdapter(com1);
        //                            SqlDataReader sqlDataReader = com_cps.ExecuteReader();
        //                            //  da1.Fill(ptDataset1);
        //                            //   dataGridView1.DataSource = ptDataset1.Tables[0];
        //                            while (sqlDataReader.Read())
        //                            {
        //                                str_3 = sqlDataReader.GetString(0).Trim();

        //                                //richTextBox1.Text += str;
        //                                //Console.WriteLine(str_3);
        //                                //using (StreamWriter sw = File.AppendText(myfile))
        //                                //{
        //                                //    sw.WriteLine(str);
        //                                //}

        //                            }

        //                            //Console.WriteLine(bar);
        //                            string bar_123 = "#" + bar + "#";
        //                            //Console.WriteLine(bar_1);
        //                            str = str.Replace(bar_123, str_3);

        //                            //Console.WriteLine(str_1);
        //                            con_cps.Close();
        //                        }

        //                    }

        //                    File.AppendAllText(myfile, str);
        //                    File.AppendAllText(myfile, "\n");
        //                }

        //                File.AppendAllText(myfile, "\n");


        //            }
        //            //EncryptionandDecryption.EncryptFile(myfile, myfile + "_2", @"myKey123");
        //            //Pgp.EncryptFile(myfile + "_2", myfile, @"E:\ColorPlast\public.txt", true, true);
        //        }
        //    }
        //    //MessageBox.Show("File Built");


        //    try
        //    {
        //        using (SqlConnection con3 = new SqlConnection(connectionString))
        //        {
        //            con3.Open();
        //            using (SqlCommand cmd = new SqlCommand("usp_UpdateOutFileStatus", con3))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@CustProfileFileID", "7");
        //                cmd.Parameters.AddWithValue("@FileName", "Sample_JAVA_Card_CPS_" + unixTime + ".cps");
        //                cmd.Parameters.AddWithValue("@FilePath", Outfilelocation + @"\Sample_JAVA_Card_CPS_" + unixTime + ".cps");

        //                cmd.ExecuteReader();

        //                MessageBox.Show("CPS OutFile created successfully",
        //                                "Message",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information
        //                                );
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
        //                                "Error",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information
        //                                );
        //    }
        //    GetGenProcessList();
        //}
        private void GetGenProcessList()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                //MessageBox.Show(fileid.ToString());
                using (SqlCommand cmd = new SqlCommand("usp_DataGenProcessHDFile", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InputfiletId", fileid);
                    cmd.Parameters.AddWithValue("@CustProfile_ID", OFProcessing.ProfileID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    da.Fill(dt);

                    //dataGVProcessList.DataSource = dt;


                    //if (dataGVProcessList.Columns.Contains(dgvButton.Name = "Process"))
                    //{

                    //}
                    //else
                    //{
                    //    dgvButton.FlatStyle = FlatStyle.System;

                    //    dgvButton.HeaderText = "Process";
                    //    dgvButton.Name = "Process";
                    //    dgvButton.UseColumnTextForButtonValue = true;
                    //    dgvButton.Text = "Process";
                    //    dgvButton.Visible = false;
                    //    dataGVProcessList.Columns.Add(dgvButton);
                    //}


                }
            }
        }
        public void UpdateInputFileSatus(string processFor, int fileID_1)
        {
            try
            {
                using (SqlConnection con3 = new SqlConnection(connectionString))
                {
                    con3.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_UpdateInPutFileStatus", con3))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DataGenProcessHDID", fileID_1);
                        cmd.Parameters.AddWithValue("@DataGenProcessStatus", '2');

                        cmd.ExecuteReader();

                        //MessageBox.Show("OutFile created successfully",
                        //                "Message",
                        //                MessageBoxButtons.OK,
                        //                MessageBoxIcon.Information
                        //                );


                    }
                }
            }
            catch (Exception ex)
            {
                logString.Append($"\nSomething went wrong with: {processFor}-{ex.Message}");
                Console.WriteLine($"\nSomething went wrong with: {processFor}-{ex.Message}");
                //MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
                //                        "Error",
                //                        MessageBoxButtons.OK,
                //                        MessageBoxIcon.Error
                //                        );
            }

        }
        private void saveDataGetProcessHDfiles(int dataGenProcessHDID, int lot, string filename_original)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataReader reader = null;
                using (SqlCommand cmd = new SqlCommand("Select * FROM CustProfileFile WHERE CustomerID = @customerId AND CustProfileID = @customerProfileID", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@customerId", customerID);
                    cmd.Parameters.AddWithValue("@customerProfileID", ProfileID);
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int i = 0;
                            string fileName = string.Empty;
                            string filePath = string.Empty;
                            int custProfileFileID = Convert.ToInt32(reader["CustProfileFileID"]);

                            if (reader["FileIOID"].ToString().Equals("I"))
                            {
                                if (i == 0)
                                {
                                    filePath = filename_original;
                                    fileName = Path.GetFileName(filePath);
                                }
                                else
                                {
                                    filePath = txtLicence.Text;
                                    fileName = Path.GetFileName(txtLicence.Text);
                                }
                                i++;
                            }
                            SaveDataGenProcessHDFiles(dataGenProcessHDID, custProfileFileID, fileName, filePath, lot);
                        }
                    }


                }
            }
        }
        private void SaveDataGenProcessHDFiles(int dataGenProcessHDID, int custProfileFileID, string fileName, string filePath, int lot)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlDataReader reader = null;
                using (SqlCommand cmd = new SqlCommand("usp_SaveDataGenProcessHDFiles", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dataGenProcessHDID", dataGenProcessHDID);
                    cmd.Parameters.AddWithValue("@fileName", fileName);
                    cmd.Parameters.AddWithValue("@filePath", filePath);
                    cmd.Parameters.AddWithValue("@custProfileFileID", custProfileFileID);
                    cmd.Parameters.AddWithValue("@createdBY", NewLogin.primaryId);
                    cmd.Parameters.AddWithValue("@lot", lot);
                    reader = cmd.ExecuteReader();
                }
            }

            //logging end



        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        public string FetchDataFromApi(int n)
        {
            n = n / 2;
            string url = $"http://192.168.5.110:8010/api/HSM?op_type=RND&digits={n}&keyname=%27%27&ip_data=%27%27";
            WebRequest request = HttpWebRequest.Create(url);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string urlText = reader.ReadToEnd();
            //Console.WriteLine(urlText);
            return urlText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OFProcessing_Multi processing = new OFProcessing_Multi();
            processing.Show();
            //txtPassword.Text = "";
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            logString.Append($"\n**************************************[Logging Out] Data Processing Tool is closing [{DateTime.Now}] **************************************\n");
            Console.WriteLine($"\n**************************************[Logging Out] Data Processing Tool is closing [{DateTime.Now}] **************************************\n");
            System.IO.File.AppendAllText(log_dir + "/Logging/" + $"{DateTime.Now.ToString("dd-MM-yyyy")}_log.txt", logString.ToString());
            upload_log();
            logString.Clear();
            this.Close();
        }



        public static void CreateMCABatch(string[] mcaFilePaths, int outerBatchSize, int innerBatchSize, string poNumber)
        {
            DataTable dtOuter = CreateDataTable(outerBatchSize, poNumber);
            DataTable dtInner = CreateDataTable(innerBatchSize, poNumber);

            int batchCounter = 0;
            int innersrno = 0;
            int outersrno = 0;

            foreach (var mcaFile in mcaFilePaths)
            {
                batchCounter++;
                var lines = File.ReadAllLines(mcaFile);
                if (lines.Length == 0) throw new IOException($"MCA file {mcaFile} is empty!");

                int outerIndex = 0;
                int innerIndex = 0;
                int outerBatchIndex = 0;
                int innerBatchIndex = 0;

                for (int i = 1; i < lines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;
                    if ((i - 1) % outerBatchSize == 0)
                    {
                        outersrno++;
                        outerIndex++;
                        outerBatchIndex++;
                        int endIndex = Math.Min(i + outerBatchSize - 1, lines.Length - 1);
                        AddRowToDataTable(dtOuter, lines[i], lines[endIndex], mcaFile, batchCounter, outersrno, outerBatchIndex);
                    }
                    if ((i - 1) % innerBatchSize == 0)
                    {
                        innersrno++;
                        innerIndex++;
                        innerBatchIndex++;
                        int endIndex = Math.Min(i + innerBatchSize - 1, lines.Length - 1);
                        AddRowToDataTable(dtInner, lines[i], lines[endIndex], mcaFile, batchCounter, innersrno, outerBatchIndex);
                    }
                }
            }
            SaveDataTableToExcel(dtOuter, mcaFilePaths[0], poNumber, "Outer", outerBatchSize);
            SaveDataTableToExcel(dtInner, mcaFilePaths[0], poNumber, "Inner", innerBatchSize);
        }

        private static DataTable CreateDataTable(int batchSize, string poNumber)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Serial_no", typeof(string));
            dt.Columns.Add("Input_File_Name", typeof(string));
            dt.Columns.Add("Start_ICCID", typeof(string));
            dt.Columns.Add("End_ICCID", typeof(string));
            dt.Columns.Add("Start_IMSI", typeof(string));
            dt.Columns.Add("End_IMSI", typeof(string));
            dt.Columns.Add("Quantity", typeof(int)).DefaultValue = batchSize;
            dt.Columns.Add("Circle", typeof(string)).DefaultValue = "NA";
            dt.Columns.Add("PO_Number", typeof(string)).DefaultValue = poNumber;
            dt.Columns.Add("SKU", typeof(string)).DefaultValue = "";
            dt.Columns.Add("BatchNumber", typeof(string));
            return dt;
        }

        private static void AddRowToDataTable(DataTable dt, string startLine, string endLine, string filePath, int batchNumber, int index, int batchIndex)
        {
            var startArray = startLine.Split(',');
            var endArray = endLine.Split(',');
            DataRow row = dt.NewRow();
            row["Serial_no"] = index.ToString().PadLeft(5, '0');
            row["Input_File_Name"] = Path.GetFileNameWithoutExtension(filePath) + "_" + batchIndex.ToString().PadLeft(3, '0');
            row["BatchNumber"] = batchNumber.ToString().PadLeft(4, '0');
            row["Start_ICCID"] = NibbleSwap(startArray[1]).Substring(0, NibbleSwap(startArray[1]).Length - 1);
            row["End_ICCID"] = NibbleSwap(endArray[1]).Substring(0, NibbleSwap(endArray[1]).Length - 1);
            row["Start_IMSI"] = NibbleSwap(startArray[0]).Substring(3);
            row["End_IMSI"] = NibbleSwap(endArray[0]).Substring(3);
            dt.Rows.Add(row);
        }

        private static void SaveDataTableToExcel(DataTable dt, string mcaFilePath, string poNumber, string labelType, int batchSize)
        {
            string baseDir = Path.GetDirectoryName(mcaFilePath);
            string filePrefix = Path.GetFileNameWithoutExtension(mcaFilePath).Split('_')[0];
            string filePostfix = Path.GetFileNameWithoutExtension(mcaFilePath).Split('_')[1];

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Data");
                var headerStyle = workbook.Style;
                headerStyle.Font.Bold = true;
                headerStyle.Fill.BackgroundColor = XLColor.LightGray;
                headerStyle.Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerStyle.Border.InsideBorder = XLBorderStyleValues.Thin;
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    var cell = ws.Cell(1, c + 1);
                    cell.Value = dt.Columns[c].ColumnName;
                    cell.Style = headerStyle;
                }
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        var cell = ws.Cell(r + 2, c + 1);
                        cell.Value = dt.Rows[r][c].ToString();
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    }
                }
                ws.Columns().AdjustToContents();
                string excelFilePath = Path.Combine(baseDir,
                    $"{filePrefix}_{labelType}_Label_PO_{poNumber}_{batchSize}_{filePostfix}.xlsx");

                workbook.SaveAs(excelFilePath);
            }
        }


        static string NibbleSwap(string hex)
        {
            if (hex.Length % 2 != 0) throw new ArgumentException("Hex string length must be even.");
            StringBuilder swapped = new StringBuilder(hex.Length);
            for (int i = 0; i < hex.Length; i += 2)
            {
                swapped.Append(hex[i + 1]);
                swapped.Append(hex[i]);
            }
            return swapped.ToString();
        }

        //static void Main(string[] args)
        //{
        //    string[] mcas = Directory.GetFiles("A:\\MCA", "*.mca");
        //    CreateMCABatch(mcas, 5000, 500, "CPS001102");
        //    Console.ReadLine();
        //}
    }
}
