using CardPrintingApplication;
using ClosedXML.Excel;
using DG_Tool.HelperClass;
using DG_Tool.Models;
using DG_Tool.WinForms.Authentication;

using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using static CardPrintingApplication.EncryptionandDecryption;
using static ClosedXML.Excel.XLPredefinedFormat;
using static Org.BouncyCastle.Math.Primes;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using Brush = System.Drawing.Brush;
using Button = System.Windows.Forms.Button;
using Color = System.Drawing.Color;
using Control = System.Windows.Forms.Control;
using DataTable = System.Data.DataTable;
using DateTime = System.DateTime;
using File = System.IO.File;
using Label = System.Windows.Forms.Label;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using TextBox = System.Windows.Forms.TextBox;


//070426: made changes in code merge code for if (File.Exists(licenceFile)) in start processing function

namespace DG_Tool.WinForms.OutputFile
{
    public partial class OFProcessing : Form
    {
        // 👇 GLOBAL (accessible everywhere)
        private ConcurrentDictionary<string, ConcurrentDictionary<string, (string FileName, int LineNumber)>> tracker_bulk
            = new ConcurrentDictionary<string, ConcurrentDictionary<string, (string, int)>>();

        public static string inputLogs = "";

        ConcurrentBag<string> outputLogs = new ConcurrentBag<string>();
        int custId = 0;
        int profileId = 0;
        private object logLock = new object();

        string Outfilelocation = "", headerfilepath = "";
        private int angle;
        private System.Windows.Forms.Timer timer;
        private Panel bufferingPanel;
        public static string profilename = "";
        public static string customer_name_form = "";
        public static string hsm_IP = Database.sql_data_value("SELECT KeyValue FROM [DataTool_Keys] where[KeyName] = 'HSM_IP' ", "KeyValue");
        public static string euicc_hsm_IP = Database.sql_data_value("SELECT KeyValue FROM [DataTool_Keys] where[KeyName] = 'EUICC_HSM_IP' ", "KeyValue");
        public static string euicc_data_IP = Database.sql_data_value("SELECT KeyValue FROM [DataTool_Keys] where[KeyName] = 'EUICC_DATA_IP' ", "KeyValue");
        public static string file_enc_key = Database.sql_data_value("SELECT KeyValue FROM [dbo].[DataTool_Keys] where[KeyName] = 'File_Enc'", "KeyValue");
        public static int eid_db = Int32.Parse(Database.sql_data_value("SELECT KeyValue FROM [dbo].[DataTool_Keys] where[KeyName] = 'EID'", "KeyValue")), EID_db_last = 0;
        public static string product_type = "";
        public static string customer = string.Empty;
        public static string circle = string.Empty;
        public static string profile = string.Empty;
        public static string inputFile = string.Empty;
        public static string licenceFile = string.Empty;
        public static string product_type_customer_profile = string.Empty;
        public static string log_dir = ConfigurationManager.AppSettings["LOG_DIR"];
        public static string EncryptDB = ConfigurationManager.AppSettings["Data_Encryption_in_DB"];
        public static int lastInsertedId = 0;
        public static int FileProcessingLotID = 0;
        public string unixTime = DateTime.Now.ToString("yyyyMMdd");
        public static DataTable Process_data = null;
        public static string Po_Num = "", dupcheck_variable = "", label_circle_data="", batchnumber_file = "";
        public static int batchsize = 0;
        public static int customerID = 0;
        public static int circleID = 0;
        public static int ProfileID = 0;
        public static int total_pro_file = 0;
        public static int total_dup_file = 0, dummy_file_qty = 0;
        public static List<int> InsertedHDIDS = new List<int>();
        public static string batchtypename = "",generic_batch_no = "";
        public static string timestamp = "", date_format = "", lastCodeMSN = "", lastCodeMSC = "";
        int fileid = 0;
        int records = 0;
        static int Total_no_of_records = 0, Total_no_of_files = 0;
        public static bool IsSingle = true;
        string first_imsi = string.Empty;
        string rjio_prefix   = string.Empty;
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
        static List<string> merged_outer_label_file_names = new List<string>(); 
        static List<string> merged_inner_label_file_names = new List<string>(); 
        static List<string> merged_outer_label_file_names_1 = new List<string>(); 
        static List<string> merged_batch_list = new List<string>();
        //HashSet<string> total_imsi = new HashSet<string>();
        //HashSet<string> total_iccid = new HashSet<string>();
        //HashSet<string> total_msisdn = new HashSet<string>();
        //Dictionary<string, string> iccidTracker = new Dictionary<string, string>();
        //Dictionary<string, string> imsiTracker = new Dictionary<string, string>();
        //Dictionary<string, string> msisdnTracker = new Dictionary<string, string>();

        Dictionary<string, Dictionary<string, (string FileName, int LineNumber)>> tracker
    = new Dictionary<string, Dictionary<string, (string, int)>>(StringComparer.OrdinalIgnoreCase)
{
    { "ICCID", new Dictionary<string, (string, int)>() },
    { "IMSI", new Dictionary<string, (string, int)>() },
    { "MSISDN", new Dictionary<string, (string, int)>() }
};

        string connectionString = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        private T ExecuteDb<T>(Func<SqlConnection, T> action)
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            return action(con);
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
                MessageBox.Show($"Error in log uploadation : " + ex.Message+ "\n\nStack Trace:\n" + ex.StackTrace);
            }
        }


        

        public OFProcessing()
        {
            InitializeComponent();

            //btnSubmit.Visible = false;

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

            //if (ofdLicence.ShowDialog() == DialogResult.OK)
            //{
            //    filepath = ofdLicence.FileName;
            //    filename = Path.GetFileName(filepath);
            //    //if (!IsDuplicateFile(filename))  **changes by sid on 22-04-2024 as not confirm to check the licence file commenting for future use.**
            //    //{
            //    //    txtLicence.Text = filepath;
            //    //}
            //    //else
            //    //{
            //    //    MessageBox.Show($"File already exist: ",
            //    //                                "Message",
            //    //                                MessageBoxButtons.OK,
            //    //                                MessageBoxIcon.Information
            //    //                                );
            //    //}
            //    txtLicence.Text = filepath;

            //}
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "Select Multiple Files",
                Filter = "All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get all selected file paths
                string[] filePaths = openFileDialog.FileNames;

                // Extract only file names
                var fileNames = filePaths
                    .Select(f => Path.GetFileName(f))
                    .ToArray();

                // Show in textbox (full path OR only names as per need)
                txtLicence.Text = string.Join(", ", filePaths);   // full path
                                                                  // txtLicence.Text = string.Join(", ", fileNames); // only names (optional)

                // If you want to store last file or all
                licenceFile = string.Join(",", fileNames); // store all names
            }
            else
            {
                txtLicence.Clear();
            }

            licenceFile = filename;
        }

        public Dictionary<string, (int pos, int len, int line, string tag)> LoadTemplateConfig(int custId, int profileId)
        {
            var dict = new Dictionary<string, (int, int, int, string)>();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT VarDes, PositionFrom, Len, LineNumber, Tag
        FROM InPutDataTemplate
        WHERE CustID = @CustID AND ProfileID = @ProfileID AND vartext = 'FL' and isnull([LineNumber],0)!=0 ", con))
            {
                cmd.Parameters.AddWithValue("@CustID", custId);
                cmd.Parameters.AddWithValue("@ProfileID", profileId);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dict[reader["VarDes"].ToString().ToUpper()] =
                        (
                            Convert.ToInt32(reader["PositionFrom"]),
                            Convert.ToInt32(reader["Len"]),
                            Convert.ToInt32(reader["LineNumber"]),
                            reader["Tag"]?.ToString()
                        );
                    }
                }
            }

            return dict;
        }

       
        private void btnInputFile_Click(object sender, EventArgs e)
        {
            //total_imsi.Clear();
            //total_iccid.Clear();
            //total_msisdn.Clear();
            //uniqueTracker.Clear();
            ///tracker.Clear();
            CleanupDatabase();
            custId = Convert.ToInt32(cbxCustomer.SelectedValue);
            profileId = Convert.ToInt32(cbxProfile.SelectedValue);

            //MessageBox.Show($"Tool will proceeed  without IMSI duplicity \nAre you ok to proceed",
            //                                                   "Message",
            //                                                   MessageBoxButtons.OK,
            //                                                   MessageBoxIcon.Information
            //                                                   );


            customer_name_form = cbxCustomer.Text;

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
                    //hsm_IP =  (profilename == "EUICC") ?  euicc_data_IP:hsm_IP;
                    //if (Path.GetDirectoryName(fileNames[0]).ToString().ToLower().Contains(cbxCustomer.Text.ToLower())   && Path.GetDirectoryName(fileNames[0]).ToString().ToLower().Contains(cbxProfile.Text.ToLower())) 
                    if (Path.GetDirectoryName(fileNames[0]).ToString().ToLower().Contains(cbxCustomer.Text.ToLower()))
                    {

                        total_pro_file = fileNames.Length;
                        logString.Append($"\n4. Initiating duplicate check process for selected files in the folder.\n");
                        Console.WriteLine($"\n4. Initiating duplicate check process for selected files in the folder.\n");
                        char ch = 'a';
                        
                        Stopwatch sw = new Stopwatch();
                        sw.Start();



                        //var configCache = LoadTemplateConfig(custId, profileId);


                        foreach (string fileName in fileNames)
                        {
                            logString.Append($"\n    {ch}. File: [{Path.GetFileName(fileName)}]\n");
                            Console.WriteLine($"\n    {ch}. File: [{Path.GetFileName(fileName)}]\n");
                            if ((!IsDuplicateFile(fileName)))
                            {

                                logString.Append($"       - No duplicate filename found.\n");
                                //duplicacy check on iccid and imsi

                                string count = iccid_dupcheck_new_faster(fileName);
                                
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
                                    MessageBox.Show($"{count}.\nFirst Duplicate {dupcheck_variable} '{Dup_First_icicid}",
                                                                "Message",
                                                                MessageBoxButtons.OK,
                                                                MessageBoxIcon.Information
                                                                );
                                    logString.Append($"       - {count}.First Duplicate {dupcheck_variable} '{Dup_First_icicid}'\n");

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
                            sw.Stop();
                            MessageBox.Show($"No file to proceed.");
                            txtoutput.Text += "No file to proceed.\r\n";
                        }
                        else
                        {
                            string[] files = txtInputfile.Text.Split(',');

                            var fileNamesfordg = files
                            .Select(f => Path.GetFileName(f.Trim()))
                            .Where(f => !string.IsNullOrEmpty(f));

                            string resultfordg = string.Join("\n", fileNamesfordg);
                            sw.Stop();
                            TimeSpan ts = sw.Elapsed;
                            string timeTaken = $"Total Time: {ts.Hours}h {ts.Minutes}m {ts.Seconds}s {ts.Milliseconds}ms";


                            MessageBox.Show(resultfordg + $"\nInput Parsing Completed.");
                            txtoutput.Text += timeTaken+ $"for {resultfordg.Split('\n').Length} files \n" + resultfordg + "\nInput Parsing Completed.\r\n";


                            //MessageBox.Show(txtInputfile.Text.Replace(',', '\n') + "Input Parsing Completed.");
                            //txtoutput.Text += txtInputfile.Text.Replace(',', '\n') + "\n Input Parsing Completed.\r\n";
                        }

                    }
                    else
                    {
                        MessageBox.Show($"Wrong input file selected ",
                                                    "Error",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information
                                                    );
                        logString.Append("\nWrong input file selected \n");
                        Console.WriteLine($"\nWrong input file selected \n");
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
                    MessageBox.Show($"All fields are required: ",
                                                "Error",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information
                                                );
                    logString.Append("\nAll fields are required: \n");
                    Console.WriteLine($"\nAll fields are required: \n");
                }
            
            
        }


        public string iccid_dupcheck_new_faster_ALL_RECORDS(string filename)
       {
            return "";
//            ////for testing
//            //if ((Debugger.IsAttached))
//            //{
//            //    return "";
//            //}

//            int iccid_len_1 = 0;
//            logString.Append("       - Checking for duplicate records in the database.\n");

//            string[] strs;

//            if (customer_name_form.Equals("AFTEL", StringComparison.OrdinalIgnoreCase))
//            {
//                strs = new string[] { "QUANTITY", "ICCID", "IMSI" };
//            }
//            else
//            {
//                strs = new string[] { "QUANTITY", "ICCID", "IMSI", "MSISDN" };
//            }

//            //if (customer_name_form.Equals("AFTEL", StringComparison.OrdinalIgnoreCase))
//            //{
//            //    strs = new string[] { "QUANTITY", "ICCID"};
//            //}
//            //else
//            //{
//            //    strs = new string[] { "QUANTITY", "ICCID","MSISDN" };
//            //}





//            //string[] strs = { "QUANTITY", "ICCID",  "MSISDN" };
//            string msg = "", firsticcid = "", firstimsi = "";
//            int file_qty = 0;
//            // Step 2: Extract values
//            HashSet<string> datalist = new HashSet<string>(); // ensures distinct automatically
//            // Read the file once into memory
//            string[] lines = File.ReadAllLines(filename);

//            foreach (string str in strs)
//            {
//                dupcheck_variable = str;
//                string varname = "", isincremental = "";
//                int pos_from = 0, len = 0, line = 0;

//                // Step 1: Get field positions from DB
//                using (SqlConnection con = new SqlConnection(connectionString))
//                using (SqlCommand cmd = new SqlCommand(@"
//            SELECT [VarName], [PositionFrom], [Len], [LineNumber], [Tag]
//            FROM [InPutDataTemplate]
//            WHERE [CustID] = @CustID AND [ProfileID] = @ProfileID 
//              AND [VarDes] = @VarDes and vartext = 'FL'
//            ORDER BY VarName", con))
//                {
//                    cmd.Parameters.AddWithValue("@CustID", cbxCustomer.SelectedValue);
//                    cmd.Parameters.AddWithValue("@ProfileID", cbxProfile.SelectedValue);
//                    cmd.Parameters.AddWithValue("@VarDes", str);

//                    con.Open();
//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            varname = reader.GetString(0);
//                            pos_from = reader.GetInt32(1);
//                            len = reader.GetInt32(2);
//                            line = reader.GetInt32(3);
//                            isincremental = !reader.IsDBNull(4) ? reader.GetString(4) : string.Empty;
//                        }
//                    }
//                }

//                if (string.IsNullOrWhiteSpace(varname))
//                    continue;

//                if (str == "QUANTITY")
//                {
//                    string lineText = lines[line - 1].Replace("\t", "    ");
//                    int safeLen = Math.Min(len, Math.Max(0, lineText.Length - pos_from));
//                    string qtyStr = safeLen > 0 ? lineText.Substring(pos_from, safeLen).Trim() : "0";
//                    file_qty = int.TryParse(qtyStr, out int q) ? q : 0;
//                    continue;
//                }


//                if (str == "BatchNumber")
//                {
//                    string lineText = lines[line - 1].Replace("\t", "    ");
//                    int safeLen = Math.Min(len, Math.Max(0, lineText.Length - pos_from));
//                    string qtyStr = safeLen > 0 ? lineText.Substring(pos_from, safeLen).Trim() : "0";
//                    batchnumber_file = qtyStr.Trim();
//                    continue;
//                }


//                // ===== VALIDATION : Field must exist in file =====
//                if (str == "ICCID" || str == "IMSI" || str == "MSISDN")
//                {
//                    // Line number validation
//                    if (line <= 0 || line > lines.Length)
//                    {
//                        MessageBox.Show($"{str} configuration error:\n" +
//                               $"{str} is expected on Line No {line} with Length {len}, " +
//                               $"but the file has only {lines.Length} lines.");

//                        return $"{str} configuration error:\n" +
//                               $"{str} is expected on Line No {line} with Length {len}, " +
//                               $"but the file has only {lines.Length} lines.";
//                    }

//                    string checkLine = lines[line - 1];

//                    // Position + Length validation
//                    if (checkLine.Length < pos_from + len)
//                    {
//                        MessageBox.Show($"{str} configuration error:\n" +
//                               $"{str} is expected on Line No {line} starting at position {pos_from} " +
//                               $"with length {len}, but this data is NOT present in the file.");
//                        return $"{str} configuration error:\n" +
//                               $"{str} is expected on Line No {line} starting at position {pos_from} " +
//                               $"with length {len}, but this data is NOT present in the file.";
//                    }
//                }

//                datalist.Clear();


//                if (!string.IsNullOrEmpty(isincremental) &&
//                    isincremental.Equals("incremental", StringComparison.OrdinalIgnoreCase))
//                {
//                    string line_all = lines[line - 1];
//                    long data_long = 0;
//                    for (int i = 0; i < file_qty; i++)
//                    {

//                        if (i == 0 && line_all.Length >= pos_from + len)
//                            data_long = long.Parse(line_all.Substring(pos_from, len));

//                        if (i == 0 && str == "ICCID")
//                        {
//                            iccid_len_1 = len;
//                            firsticcid = data_long.ToString();
//                        }
//                        else if (i == 0 && str == "IMSI")
//                        {
//                            firstimsi = data_long.ToString();
//                        }
//                        data_long++;
//                        datalist.Add(StringToHex(data_long.ToString()));

//                        string currentFileName = Path.GetFileName(filename);
//                        int currentLineNumber = line;
//                        string str1 = str?.Trim();
//                        data_long.ToString()?.Trim();

//                        if (tracker.TryGetValue(str1, out var typeDictionary))
//                        {
//                            if (typeDictionary.ContainsKey(data_long.ToString()?.Trim()))
//                            {
//                                var firstOccurrence = typeDictionary[data_long.ToString()?.Trim()];

//                                return $"Duplicate {str1}: {data_long.ToString()?.Trim()}\n" +
//                                       $"First Found In File: {firstOccurrence.FileName} (Line {firstOccurrence.LineNumber})\n" +
//                                       $"Duplicate Found In File: {currentFileName} (Line {currentLineNumber})";
//                            }
//                            else
//                            {
//                                typeDictionary[data_long.ToString()?.Trim()] = (currentFileName, currentLineNumber);
//                            }
//                        }



//                        //if (iccidTracker.ContainsKey(data_long.ToString()))
//                        //{
//                        //    string firstFile = iccidTracker[data_long.ToString()];

//                        //    return $"Duplicate ICCID: {data_long.ToString()}\n" +
//                        //           $"First Found In: {firstFile}\n" +
//                        //           $"Duplicate Found In: {Path.GetFileName(filename)}";
//                        //}
//                        //else
//                        //{
//                        //    iccidTracker[data_long.ToString()] = Path.GetFileName(filename);
//                        //}


//                    }
//                }
//                else
//                {
//                    for (int i = 0; i < file_qty; i++)
//                    {
//                        string line_all = lines[line + i - 1];
//                        if (line_all.Length <= pos_from) continue;

//                        string value = (line_all.Length >= pos_from + len)
//                            ? line_all.Substring(pos_from, len)
//                            : line_all.Substring(pos_from);

//                        if (!string.IsNullOrEmpty(value))
//                            datalist.Add(StringToHex(value));






//                        string currentFileName = Path.GetFileName(filename);
//                        int currentLineNumber = line;
//                        string str1 = str?.Trim();
//                        value = value?.Trim();
//                        if (tracker.TryGetValue(str1, out var typeDictionary))
//                        {
//                            if (typeDictionary.ContainsKey(value))
//                            {
//                                var firstOccurrence = typeDictionary[value];

//                                return $"Duplicate {str1}: {value}\n" +
//                                       $"First Found In File: {firstOccurrence.FileName} (Line {firstOccurrence.LineNumber})\n" +
//                                       $"Duplicate Found In File: {currentFileName} (Line {currentLineNumber})";
//                            }
//                            else
//                            {
//                                typeDictionary[value] = (currentFileName, currentLineNumber);
//                            }
//                        }


//                        //if (iccidTracker.ContainsKey(value))
//                        //{
//                        //    string firstFile = iccidTracker[value];

//                        //    return $"Duplicate ICCID: {value}\n" +
//                        //           $"First Found In: {firstFile}\n" +
//                        //           $"Duplicate Found In: {Path.GetFileName(filename)}";
//                        //}
//                        //else
//                        //{
//                        //    iccidTracker[value] = Path.GetFileName(filename);
//                        //}
//                    }
//                }




//                if (datalist.Count == 0)
//                    continue;

//                // Step 3: Load values into DataTable
//                DataTable iccidTable = new DataTable();
//                iccidTable.Columns.Add("Value", typeof(string));
//                iccidTable.BeginLoadData();
//                foreach (string val in datalist)
//                    iccidTable.Rows.Add(val);
//                iccidTable.EndLoadData();

//                using (SqlConnection con = new SqlConnection(connectionString))
//                {
//                    con.Open();

//                    // Create temp table
//                    using (SqlCommand createCmd = new SqlCommand(
//                        "CREATE TABLE #IccidList (Value NVARCHAR(50) PRIMARY KEY);", con))
//                    {
//                        createCmd.ExecuteNonQuery();
//                    }

//                    // Bulk insert into temp table
//                    using (SqlBulkCopy bulk = new SqlBulkCopy(con))
//                    {
//                        bulk.DestinationTableName = "#IccidList";
//                        bulk.WriteToServer(iccidTable);
//                    }

//                    // Step 4: QUERY FOR DUPLICITY
//                    string dupQuery = $@"
//SELECT
//    il.Value AS DuplicateValue,
//    f.DataGenProcessHDID,
//    f.FileName,
//    f.OutFileProcessDate
//FROM DataGenProcessHDFile f
//JOIN DupCheck d
//    ON d.C1 = f.DataGenProcessHDID
//JOIN #IccidList il 
//    ON d.{str} COLLATE SQL_Latin1_General_CP1_CI_AS 
//     = il.Value COLLATE SQL_Latin1_General_CP1_CI_AS
//WHERE f.OutFlileStatus = 2
//ORDER BY f.DataGenProcessHDID,il.Value";

//                    string csvPath = @$"D:\{str}_DuplicateRecords_{Path.GetFileName(filename)}.csv";

//                    using (SqlCommand cmd = new SqlCommand(dupQuery, con))
//                    {
//                        try
//                        {
//                            cmd.CommandTimeout = 15000;

//                            using (SqlDataReader reader = cmd.ExecuteReader())
//                            {
//                                List<string> csvLines = new List<string>();

//                                // CSV Header
//                                csvLines.Add("DuplicateValue,Old_FileName");

//                                while (reader.Read())
//                                {
//                                    string dupValue = reader.IsDBNull(0) ? "" : reader.GetString(0);
//                                    int dataGenProcessHDID = reader.GetInt32(1);
//                                    string fileName = reader.GetString(2);
//                                    DateTime outFileProcessDate = reader.GetDateTime(3);

//                                    if (!string.IsNullOrEmpty(dupValue))
//                                        Dup_First_icicid = HexToString(dupValue);

//                                    csvLines.Add($"{Dup_First_icicid},{fileName}");

//                                    msg = $"Duplicate Found\nFileID:{dataGenProcessHDID}\nFileName:{fileName}\nProcessed Date:{outFileProcessDate}";
//                                }

//                                if (csvLines.Count > 1)
//                                {
//                                    File.WriteAllLines(csvPath, csvLines);
//                                    MessageBox.Show($"Duplicate records exported to:\n{csvPath}");
//                                }
//                            }
//                        }
//                        catch (SqlException ex)
//                        {
//                            MessageBox.Show($"Database timeout or server not responding.\n\nError: {ex.Message}",
//                                            "SQL Error",
//                                            MessageBoxButtons.OK,
//                                            MessageBoxIcon.Error);
//                        }
//                        catch (Exception ex)
//                        {
//                            MessageBox.Show($"Unexpected error.\n\nError: {ex.Message}",
//                                            "Error",
//                                            MessageBoxButtons.OK,
//                                            MessageBoxIcon.Error);
//                        }
//                    }

                  

//                }
//            }
//            if (cbxCustomer.Text.ToUpper() == "VODAFONE" && firsticcid != "" && firstimsi != "" && iccid_len_1 == 18)
//            {
//                string error = ValidateIccid(firsticcid, firstimsi, true);

//                if (!string.IsNullOrEmpty(error))
//                {
//                    MessageBox.Show(error);
//                    return "Wrong iccid"; // stop processing
//                }
//            }
//            return "";
        }
        public string iccid_dupcheck_new_faster(string filename)
        {
            ////for testing
            //if ((Debugger.IsAttached))
            //{
            //    return "";
            //}

            int iccid_len_1 = 0;
            logString.Append("       - Checking for duplicate records in the database.\n");

            string[] strs;

            if (customer_name_form.Equals("AFTEL", StringComparison.OrdinalIgnoreCase))
            {
                strs = new string[] { "QUANTITY", "ICCID", "IMSI" };
            }
            else
            {
                strs = new string[] { "QUANTITY", "ICCID", "IMSI", "MSISDN" };
            }

            //if (customer_name_form.Equals("AFTEL", StringComparison.OrdinalIgnoreCase))
            //{
            //    strs = new string[] { "QUANTITY", "ICCID"};
            //}
            //else
            //{
            //    strs = new string[] { "QUANTITY", "ICCID","MSISDN" };
            //}





            //string[] strs = { "QUANTITY", "ICCID",  "MSISDN" };
            string msg = "", firsticcid = "", firstimsi = "";
            int file_qty = 0;
            // Step 2: Extract values
            HashSet<string> datalist = new HashSet<string>(); // ensures distinct automatically
            // Read the file once into memory
            string[] lines = File.ReadAllLines(filename);

            foreach (string str in strs)
            {
                dupcheck_variable = str;
                string varname = "", isincremental = "";
                int pos_from = 0, len = 0, line = 0;

                // Step 1: Get field positions from DB
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(@"
            SELECT [VarName], [PositionFrom], [Len], [LineNumber], [Tag]
            FROM [InPutDataTemplate]
            WHERE [CustID] = @CustID AND [ProfileID] = @ProfileID 
              AND [VarDes] = @VarDes and vartext = 'FL'
            ORDER BY VarName", con))
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


                if (str == "BatchNumber")
                {
                    string lineText = lines[line - 1].Replace("\t", "    ");
                    int safeLen = Math.Min(len, Math.Max(0, lineText.Length - pos_from));
                    string qtyStr = safeLen > 0 ? lineText.Substring(pos_from, safeLen).Trim() : "0";
                    batchnumber_file = qtyStr.Trim();
                    continue;
                }


                // ===== VALIDATION : Field must exist in file =====
                if (str == "ICCID" || str == "IMSI" || str == "MSISDN")
                {
                    // Line number validation
                    if (line <= 0 || line > lines.Length)
                    {
                        MessageBox.Show($"{str} configuration error:\n" +
                               $"{str} is expected on Line No {line} with Length {len}, " +
                               $"but the file has only {lines.Length} lines.");

                        return $"{str} configuration error:\n" +
                               $"{str} is expected on Line No {line} with Length {len}, " +
                               $"but the file has only {lines.Length} lines.";
                    }

                    string checkLine = lines[line - 1];

                    // Position + Length validation
                    if (checkLine.Replace("\t","   ").Length < pos_from + len)
                    {
                        MessageBox.Show($"{str} configuration error:\n" +
                               $"{str} is expected on Line No {line} starting at position {pos_from} " +
                               $"with length {len}, but this data is NOT present in the file.");
                        return $"{str} configuration error:\n" +
                               $"{str} is expected on Line No {line} starting at position {pos_from} " +
                               $"with length {len}, but this data is NOT present in the file.";
                    }
                }

                datalist.Clear();


                if (!string.IsNullOrEmpty(isincremental) &&
                    isincremental.Equals("incremental", StringComparison.OrdinalIgnoreCase))
                {
                    string line_all = lines[line - 1];
                    long data_long = 0;
                    for (int i = 0; i < file_qty; i++)
                    {
                        
                        if (i == 0 && line_all.Length >= pos_from + len)
                            data_long = long.Parse(line_all.Substring(pos_from, len));
                       
                        if (i == 0 && str == "ICCID")
                        {
                            iccid_len_1 = len;
                            firsticcid = data_long.ToString();
                        }
                        else if (i == 0 && str == "IMSI")
                        {
                            firstimsi = data_long.ToString();
                        }
                        data_long++;
                        datalist.Add(StringToHex(data_long.ToString()));

                        string currentFileName = Path.GetFileName(filename);
                        int currentLineNumber = line;
                        string str1 = str?.Trim();
                        data_long.ToString()?.Trim();
                        
                        if (tracker.TryGetValue(str1, out var typeDictionary))
                        {
                            if (typeDictionary.ContainsKey(data_long.ToString()?.Trim()))
                            {
                                var firstOccurrence = typeDictionary[data_long.ToString()?.Trim()];

                                return $"Duplicate {str1}: {data_long.ToString()?.Trim()}\n" +
                                       $"First Found In File: {firstOccurrence.FileName} (Line {firstOccurrence.LineNumber})\n" +
                                       $"Duplicate Found In File: {currentFileName} (Line {currentLineNumber})";
                            }
                            else
                            {
                                typeDictionary[data_long.ToString()?.Trim()] = (currentFileName, currentLineNumber);
                            }
                        }



                        //if (iccidTracker.ContainsKey(data_long.ToString()))
                        //{
                        //    string firstFile = iccidTracker[data_long.ToString()];

                        //    return $"Duplicate ICCID: {data_long.ToString()}\n" +
                        //           $"First Found In: {firstFile}\n" +
                        //           $"Duplicate Found In: {Path.GetFileName(filename)}";
                        //}
                        //else
                        //{
                        //    iccidTracker[data_long.ToString()] = Path.GetFileName(filename);
                        //}


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






                        string currentFileName = Path.GetFileName(filename);
                        int currentLineNumber = line;
                        string str1 = str?.Trim();
                        value = value?.Trim();
                        if (tracker.TryGetValue(str1, out var typeDictionary))
                        {
                            if (typeDictionary.ContainsKey(value))
                            {
                                var firstOccurrence = typeDictionary[value];

                                return $"Duplicate {str1}: {value}\n" +
                                       $"First Found In File: {firstOccurrence.FileName} (Line {firstOccurrence.LineNumber})\n" +
                                       $"Duplicate Found In File: {currentFileName} (Line {currentLineNumber})";
                            }
                            else
                            {
                                typeDictionary[value] = (currentFileName, currentLineNumber);
                            }
                        }


                        //if (iccidTracker.ContainsKey(value))
                        //{
                        //    string firstFile = iccidTracker[value];

                        //    return $"Duplicate ICCID: {value}\n" +
                        //           $"First Found In: {firstFile}\n" +
                        //           $"Duplicate Found In: {Path.GetFileName(filename)}";
                        //}
                        //else
                        //{
                        //    iccidTracker[value] = Path.GetFileName(filename);
                        //}
                    }
                }




                if (datalist.Count == 0)
                    continue;

                // Step 3: Load values into DataTable
                DataTable iccidTable = new DataTable();
                iccidTable.Columns.Add("Value", typeof(string));
                iccidTable.BeginLoadData();
                foreach (string val in datalist)
                    iccidTable.Rows.Add(val);
                iccidTable.EndLoadData();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Create temp table
                    using (SqlCommand createCmd = new SqlCommand(
                        "CREATE TABLE #IccidList (Value NVARCHAR(50) PRIMARY KEY);", con))
                    {
                        createCmd.ExecuteNonQuery();
                    }

                    // Bulk insert into temp table
                    using (SqlBulkCopy bulk = new SqlBulkCopy(con))
                    {
                        bulk.DestinationTableName = "#IccidList";
                        bulk.WriteToServer(iccidTable);
                    }





                    // Step 4: QUERY FOR DUPLICITY
                    string dupQuery = $@"
                        SELECT TOP 100
                            f.DataGenProcessHDID, 
                            f.FileName, 
                            f.OutFileProcessDate,
                            COUNT_BIG(*) AS RecordCount,
                            MIN(il.Value) AS FirstDupValue
                        FROM DataGenProcessHDFile f
                        JOIN DupCheck d ON d.C1 = f.DataGenProcessHDID
                        JOIN #IccidList il 
                            ON d.{str} COLLATE SQL_Latin1_General_CP1_CI_AS = il.Value COLLATE SQL_Latin1_General_CP1_CI_AS
                        WHERE f.OutFlileStatus = 2
                        GROUP BY f.DataGenProcessHDID, f.FileName, f.OutFileProcessDate
                        ORDER BY RecordCount DESC;";



                    using (SqlCommand cmd = new SqlCommand(dupQuery, con))
                        try
                        {
                            cmd.CommandTimeout = 15000; // Increase timeout to 5 minutes (adjust as needed)

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

                                    msg = $"Duplicate Records Found :- \nPreviously processed FileID:{dataGenProcessHDID}\nPreviously processed FileName:{fileName}\nProcessed Date:{outFileProcessDate}\nRecordCount:{count}";
                                    return msg;
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show($"Database operation timed out or the server is not responding.\n\nError Details: " + ex.Message,
                                            "SQL Timeout Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An unexpected error occurred.\n\nError Details: " + ex.Message,
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        }

                }
            }
            if (cbxCustomer.Text.ToUpper() == "VODAFONE" && firsticcid != "" && firstimsi != "" && iccid_len_1 == 18)
            {
                string error = ValidateIccid(firsticcid, firstimsi, true);

                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show(error);
                    return "Wrong iccid"; // stop processing
                }
            }
            return "";
        }
        public static string ValidateIccid(string iccid, string imsi, bool isIncrementalTag)
        {
            if (string.IsNullOrEmpty(iccid))
                return "ICCID is empty";

            // Rule 1: ICCID length = 19 and last digit is Luhn digit
            if (iccid.Length == 19)
            {
                if (IsValidLuhn(iccid))
                {
                    return "Wrong ICCID length check"; // ❌ error
                }
            }

            // Rule 2: If tag is incremental, last 6 digits must match IMSI
            if (isIncrementalTag)
            {
                if (string.IsNullOrEmpty(imsi) || imsi.Length < 6)
                    return "Invalid IMSI";

                string iccidLast6 = iccid.Substring(iccid.Length - 6);
                string imsiLast6 = imsi.Substring(imsi.Length - 6);

                if (iccidLast6 != imsiLast6)
                {
                    return "Last 6 of iccid and imsi mistmach"; // ❌ error
                }
            }

            return ""; // ✅ valid ICCID
        }
        public static bool IsValidLuhn(string number)
        {
            int sum = 0;
            bool alternate = false;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(number[i].ToString());

                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                        n -= 9;
                }

                sum += n;
                alternate = !alternate;
            }

            return (sum % 10 == 0);
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
                MessageBox.Show($"Something went wrong while getting filename: " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace,
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
                        //MessageBox.Show($"Data Saved Successfully!");
                    }
                    catch (Exception exe)
                    {
                        MessageBox.Show(exe.Message + "\n\nStack Trace:\n" + exe.StackTrace);
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
                    pad += "FFFFFFFF";
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


        public string AES_WRAP(string key, string data)
        {
            byte[] aesKeyBytes = HexStringToByteArray(key);
            byte[] priBytes = HexStringToByteArray(data);
            using var aes = Aes.Create();
            aes.Key = aesKeyBytes;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            byte[] wrappedKey;
            using (var enc = aes.CreateEncryptor())
                wrappedKey = enc.TransformFinalBlock(priBytes, 0, priBytes.Length);

            string wrappedHex = BitConverter.ToString(wrappedKey).Replace("-", "");
            return wrappedHex;
        }



        public string CHECKSUM(string data)
        {
            byte[] data_hex = Encoding.ASCII.GetBytes(data);
            uint crc32 = Crc32.ComputeChecksum(data_hex);
            string checksumHex = crc32.ToString("X8");
            return checksumHex;
        }
        public string eid_function(int i)
        {
            string data = "890440458660999902";
            data = data + (eid_db + i).ToString().PadLeft(12, '0');
            long sum = 0;

            foreach (char c in data)
            {
                sum += c - '0'; // add digit value
            }

            int checksum = (int)(sum % 100); // force to 2 digits
            string checksumStr = checksum.ToString("D2"); // always 2-di
            data = data + checksumStr;
            EID_db_last = eid_db + i;
            return data;
        }

        public string hsm_randomfunction(string cert_name, string eum_cert_name, int i, int attempt)
        {
            int maxAttempts = 10;
            string url = $"http://{euicc_hsm_IP.Trim()}/api/HsmCa/operate", response_Data = "";
            // string url = "http://localhost:7087/api/HsmCa/operate", response_Data = "";
            using (HttpClient client = new HttpClient())
            {

                try
                {
                    //var jsonBody = @"{
                    //""slot_id"": 0,
                    //""user"": ""user"",
                    //""user_pin"": ""12345678"",
                    //""root_ca_cert_name"": ""CERT_test_cp_ci"",
                    //""sub_ca_cert_name"": ""CERT_test_cp_eum"",
                    //""cert_name"": ""CERT_test_1"",
                    //""operation_type"": 6
                    //         }";
                    // ✅ Delay to prevent too many rapid requests

                    var jsonBody = $@"{{
                        ""slot_id"": 0,
                        ""user"": ""user"",
                        ""user_pin"": ""12345678"",
                        ""root_ca_cert_name"": ""{cert_name}"",
                        ""sub_ca_cert_name"": ""{eum_cert_name}"",
                        ""cert_name"": ""EUICC_DATA_CERT_{i}"",
                        ""operation_type"": 6
                    }}";
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = client.PostAsync(url, content).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;


                    //var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    //    HttpResponseMessage response = client.PostAsync(url, content).Result;
                    //    string responseBody = response.Content.ReadAsStringAsync().Result;
                    response_Data = responseBody;


                    string[] parts_list12345 = response_Data.Split(',');
                    string euicc_ci_cert_data = parts_list12345[1].Replace("root_ca_hex:", "").Trim();
                    euicc_ci_cert_data = parts_list12345[2].Replace("sub_ca_hex:", "").Trim();
                    euicc_ci_cert_data = parts_list12345[6].Replace("wrapped_key:", "").Trim();
                    euicc_ci_cert_data = parts_list12345[3].Replace("end_entity_hex:", "").Trim();
                }
                catch (Exception ex)
                {
                    if (attempt < maxAttempts)
                    {
                        //logString.Append($"\nAttempt {attempt} failed: {ex.Message}, retrying...");
                        System.Threading.Thread.Sleep(2000); // wait 2 sec before retry
                        return euicc_Data_function(cert_name, eum_cert_name, i, attempt + 1);
                    }
                    else
                    {
                        response_Data = $"ERROR: {ex.Message}";
                    }
                    //Consoleresponse_DataWriteLine(errorLine);
                    //File.AppendAllText(outputFile, errorLine + Environment.NewLine);
                }

            }



            return response_Data;
        }

        public string euicc_Data_function(string cert_name, string eum_cert_name, int i, int attempt)
        {
            int maxAttempts = 10;
            string url = $"http://{euicc_hsm_IP.Trim()}/api/HsmCa/operate", response_Data = "";
            // string url = "http://localhost:7087/api/HsmCa/operate", response_Data = "";
            using (HttpClient client = new HttpClient())
            {

                try
                {
                    //var jsonBody = @"{
                    //""slot_id"": 0,
                    //""user"": ""user"",
                    //""user_pin"": ""12345678"",
                    //""root_ca_cert_name"": ""CERT_test_cp_ci"",
                    //""sub_ca_cert_name"": ""CERT_test_cp_eum"",
                    //""cert_name"": ""CERT_test_1"",
                    //""operation_type"": 6
                    //         }";
                    // ✅ Delay to prevent too many rapid requests

                    var jsonBody = $@"{{
                        ""slot_id"": 0,
                        ""user"": ""user"",
                        ""user_pin"": ""12345678"",
                        ""root_ca_cert_name"": ""{cert_name}"",
                        ""sub_ca_cert_name"": ""{eum_cert_name}"",
                        ""cert_name"": ""EUICC_DATA_CERT_{i}"",
                        ""operation_type"": 6
                    }}";
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = client.PostAsync(url, content).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;


                    //var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    //    HttpResponseMessage response = client.PostAsync(url, content).Result;
                    //    string responseBody = response.Content.ReadAsStringAsync().Result;
                    response_Data = responseBody;


                    string[] parts_list12345 = response_Data.Split(',');
                    string euicc_ci_cert_data = parts_list12345[1].Replace("root_ca_hex:", "").Trim();
                    euicc_ci_cert_data = parts_list12345[2].Replace("sub_ca_hex:", "").Trim();
                    euicc_ci_cert_data = parts_list12345[6].Replace("wrapped_key:", "").Trim();
                    euicc_ci_cert_data = parts_list12345[3].Replace("end_entity_hex:", "").Trim();
                }
                catch (Exception ex)
                {
                    if (attempt < maxAttempts)
                    {
                        //logString.Append($"\nAttempt {attempt} failed: {ex.Message}, retrying...");
                        System.Threading.Thread.Sleep(2000); // wait 2 sec before retry
                        return euicc_Data_function(cert_name, eum_cert_name, i, attempt + 1);
                    }
                    else
                    {
                        response_Data = $"ERROR: {ex.Message}";
                    }
                    //Consoleresponse_DataWriteLine(errorLine);
                    //File.AppendAllText(outputFile, errorLine + Environment.NewLine);
                }

            }



            return response_Data;
        }

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

        public static string GetFormattedDate(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return DateTime.Now.ToString("yyyyMMdd");

            format = format.Replace("YYYY", "yyyy")
                           .Replace("YY", "yy")
                           .Replace("DD", "dd")
                           .Replace("mm", "MM")
                           .Replace("HH", "HH")
                           .Replace("MI", "mm")
                           .Replace("SS", "ss");

            return DateTime.Now.ToString(format);
        }
        public string mncmcc_function(string s, int len)
        {
            s = s.Trim();
            string data = "";
            s = s.Substring(0, len);
            if (s.Length > 0)
            {
                if (s.Length % 2 != 0)
                {
                    s += 'F';
                }

                // Use string interpolation or concatenation, not '+' on chars
                data = $"{s[1]}{s[0]}{s[5]}{s[2]}{s[4]}{s[3]}";
            }
            else
            {
                data = "INVALID_NS_" + s;
            }

            return data;
        }

        public string nibble_swapped_U(string s)
        {
            s = s.Trim();
            string revs = "";
            if (s.Length > 0)
            {
                if (s.Length % 2 != 0)
                {
                    s += 'U';
                }

                for (int i = 0; i < s.Length; i = i + 2) //String Reverse  
                {
                    revs += s[i + 1].ToString();
                    revs += s[i].ToString();
                }
            }
            else
                revs = "INVALID_NS_" + s;

            return revs;
        }

        public string nibble_swapped(string s)
        {
            s = s.Trim();
            string revs = "";
            if (s.Length > 0)
            {
                if (s.Length % 2 != 0)
                {
                    s += 'F';
                }

                for (int i = 0; i < s.Length; i = i + 2) //String Reverse  
                {
                    revs += s[i + 1].ToString();
                    revs += s[i].ToString();
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

        public string Input_Filename_sep(string filename, char sep, int sep_count)
        {
            string[] data = filename.Split(sep);
            sep_count = sep_count - 1;

            if (sep_count < 0 || sep_count >= data.Length)
                throw new ArgumentOutOfRangeException(nameof(sep_count), "Separator index is out of range.");

            return data[sep_count];
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


        public static string EncryptAes256(
            string keyHex,
            string dataHex,
            string mode,
            string ivHex = null)
        {
            byte[] key = HexStringToByteArray(keyHex);
            byte[] data = HexStringToByteArray(dataHex);

            int data_Length = dataHex.Length;
            if (key.Length != 32)
                throw new Exception("Key must be 32 bytes (256 bit)");

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.Key = key;
                aes.Padding = PaddingMode.PKCS7;

                if (mode.ToUpper() == "ECB")
                {
                    aes.Mode = CipherMode.ECB;
                }
                else if (mode.ToUpper() == "CBC")
                {

                    string zeros = new string('0', data_Length);

                    aes.Mode = CipherMode.CBC;
                    aes.IV = HexStringToByteArray(zeros);
                    //aes.IV = HexStringToByteArray(ivHex);

                    if (aes.IV.Length != 16)
                        throw new Exception("IV must be 16 bytes");
                }
                else
                {
                    throw new Exception("Invalid mode");
                }

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] encrypted = encryptor.TransformFinalBlock(data, 0, data.Length);
                    return BytesToHex(encrypted).Substring(0, data_Length);
                }
            }
        }
        private static string BytesToHex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
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
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Vw_InputFileDupCheck WHERE FilePath = @finename and [CustID]= {custId} and [CustProfileID]={profileId}", con))
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
            Total_no_of_records = 0;
            Total_no_of_files = 0;
            customer_name_form = cbxCustomer.Text;
            timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            date_format = "";
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show($"Please select the batch first.",
                                                   "Message",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Information
                                                   );
                return;
            }

            if (string.IsNullOrEmpty(tb_ponum.Text.Trim()))
            {
                MessageBox.Show($"Please enter the po number first.",
                                                   "Message",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Information
                                                   );
                return;
            }
            if (comboBox1.SelectedValue?.ToString() == "0")
            {
                string input = InputBox.Show("Enter batch size (leave blank for 0):", "Batch Size", "0");

                batchsize = 0; // default = 0
                if (int.TryParse(input, out int result))
                {
                    batchsize = result;
                }
                else
                {
                    MessageBox.Show($"Please enter the batchsize in numeric.",
                                                  "Message",
                                                  MessageBoxButtons.OK,
                                                  MessageBoxIcon.Information
                                                  );

                    return;
                }
            }
            if (Circle_Label.Text.Trim() == "")
            {
                MessageBox.Show($"Please enter the Label Circle first.",
                                                  "Message",
                                                  MessageBoxButtons.OK,
                                                  MessageBoxIcon.Information
                                                  );
                return;

            }

            if (txtInputfile.Text.Trim() == "")
            {
                MessageBox.Show($"Please select input file.",
                                                  "Message",
                                                  MessageBoxButtons.OK,
                                                  MessageBoxIcon.Information
                                                  );
                return;

            }
            else
            {

                batchsize = Convert.ToInt32(comboBox1.SelectedValue);
                label_circle_data = Circle_Label.Text.Trim();
            }
            //initionalising all values with initials
            lastInsertedId = 0;
            FileProcessingLotID = 0;
            merged_outer_label_file_names = new List<string>();
            merged_inner_label_file_names = new List<string>();
            merged_outer_label_file_names_1 = new List<string>();
            merged_batch_list = new List<string>();

            batchtypename =  comboBox1.Text.Trim();
            customerID = Convert.ToInt32(cbxCustomer.SelectedValue);
            circleID = Convert.ToInt32(cbxCircle.SelectedValue);
            ProfileID = Convert.ToInt32(cbxProfile.SelectedValue);
            Po_Num = tb_ponum.Text.Trim();
            inputFile = txtInputfile.Text.Trim();
            licenceFile = txtLicence.Text.Trim();
            customer = cbxCustomer.Text;
            circle = cbxCircle.Text;
            profile = cbxProfile.Text;
            panel1.Visible = true;
            profilename = cbxProfile.Text.ToUpper();
            hsm_IP = (profilename == "EUICC") ? euicc_data_IP : hsm_IP;
            product_type_customer_profile = Database.sql_data_value("  select product_type from custprofile where ProfileID  = " + ProfileID + " ", "product_type");
            if (product_type_customer_profile == "DUMMY")
            {
                dummy_file_qty = 0;
                bool valid = false;

                while (!valid)
                {
                    string input = Interaction.InputBox(
                        "Enter Data Quantity (must be greater than 0):",
                        "Data Quantity",
                        "1"); // default value

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        // User cancelled
                        return;
                    }

                    if (int.TryParse(input, out dummy_file_qty) && dummy_file_qty > 0)
                    {
                        valid = true;
                        MessageBox.Show($"You entered: {dummy_file_qty}", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Invalid quantity! Please enter a number greater than 0.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

                backgroundWorker1.RunWorkerAsync();
            
        }


        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (product_type_customer_profile != "DUMMY")
            { startProcessing(); }
            else
            {
                startProcessing_dummy();
            }
            ////this.Close();
            //this.Invoke(new MethodInvoker(delegate
            //{
            //    panel1.Visible = false;
            //}));



        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //StopBuffering();
            panel1.Visible = false;
            this.Close();
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
                        //if (filenames.Length > 1)
                        //{
                        //    IsSingle = false;
                        //}
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
                                int data_file_check = First_Record_Processing(lastInsertedId, lotid);
                                i += data_file_check;
                                if (data_file_check == 0)
                                {

                                    MessageBox.Show($"Error During Importation in file '{Path.GetFileName(filename)}' Please check logs ",
                                                           "Message",
                                                           MessageBoxButtons.OK,
                                                           MessageBoxIcon.Information
                                                           );
                                    return;
                                }
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
                            if (!File.Exists(licenceFile))
                            {
                                //if (File.Exists(licenceFile))
                                //{
                                //    string status = Importlicencefile(lotid);
                                //    if (status != "")
                                //    {
                                //        MessageBox.Show(status);
                                //    }
                                //    else
                                //    {
                                //        int j = 0;
                                //        foreach (int hdid in InsertedHDIDS)
                                //        {
                                //            lastInsertedId = hdid;
                                //            Console.WriteLine(lastInsertedId);
                                //            int test_data_validation = All_Record_Processing();
                                //            if (test_data_validation == 10)
                                //            {
                                //                throw new InvalidOperationException("HSM error");
                                //            }
                                //            if (test_data_validation == 100)
                                //            {
                                //                throw new InvalidOperationException("Data Configuration error");
                                //            }

                                //            else
                                //            {
                                //                j += test_data_validation;
                                //            }
                                //        }
                                //        if (j < filenames.Length)
                                //        {
                                //            DialogResult result = MessageBox.Show($"{i}/{filenames.Length} File uploaded & Processed successfully.\nWant to proceed further if not the data will be deleted from database.",
                                //                     "Message",
                                //                     MessageBoxButtons.YesNo,
                                //                     MessageBoxIcon.Information
                                //                     );

                                //            if (result == DialogResult.No)
                                //            {
                                //                using (SqlConnection con = new SqlConnection(connectionString))
                                //                {
                                //                    con.Open();
                                //                    SqlDataReader reader = null;
                                //                    using (SqlCommand cmd = new SqlCommand($"Delete FROM [dbo].[DataGenProcessDataRecord] WHERE DataGenProcessHDID IN (SELECT Distinct DataGenProcessHDID FROM [DataGenProcessHDFile] WHERE FileLotID={lotid});", con))
                                //                    {
                                //                        int rowsAffected = cmd.ExecuteNonQuery();

                                //                        if (rowsAffected > 0)
                                //                        {
                                //                            MessageBox.Show($"Data deleted successfully.");
                                //                            logString.Append($"\n6. Data deleted successfully from database.\n");
                                //                            Console.WriteLine($"\n6. Data deleted successfully from database.\n");
                                //                        }
                                //                        else
                                //                        {
                                //                            MessageBox.Show($"Database Error.");
                                //                            logString.Append($"\n6. Unable to delete uploaded Files with {lotid} lot.\n");
                                //                            Console.WriteLine($"\n6. Unable to delete uploaded Files with {lotid} lot.\n");
                                //                        }
                                //                    }
                                //                }
                                //            }
                                //            else
                                //            {
                                //                logString.Append($"\n6. {i}/{filenames.Length} Files successfully imported & Processed.\n");
                                //                Console.WriteLine($"\n6. {i}/{filenames.Length} Files successfully imported & Processed.\n");
                                //                logString.Append($"\n7. Generating summary report with file lot ID:\n");
                                //                Console.WriteLine($"\n7. Generating summary report with file lot ID:\n");
                                //                logString.Append($"    - Lot ID: {lotid}\n");
                                //                Console.WriteLine($"    - Lot ID: {lotid}\n");
                                //                logString.Append($"    - Total files processed: : {total_pro_file}\n");
                                //                Console.WriteLine($"    - Total files processed: : {total_pro_file}\n");
                                //                logString.Append($"    - Total files imported:  {i}\n");
                                //                Console.WriteLine($"    - Total files imported:  {i}\n");
                                //                logString.Append($"    - Total duplicate files:  {total_dup_file}\n");
                                //                Console.WriteLine($"    - Total duplicate files:  {total_dup_file}\n");
                                //                logString.Append($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                //                Console.WriteLine($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                //                logString.Append($"    - Total records processed: : {Total_no_of_records}\n");
                                //                Console.WriteLine($"    - Total records processed: : {Total_no_of_records}\n");
                                //                MessageBox.Show($"you can procceed to processing with remaining files.");
                                //                FileProcessingLotID = lotid;
                                //            }

                                //        }
                                //        else if (j == 0)
                                //        {
                                //            using (SqlConnection con = new SqlConnection(connectionString))
                                //            {
                                //                con.Open();
                                //                using (SqlCommand cmd = new SqlCommand($"UPDATE FileLotMaster SET [DataGenProcessStatus]= 16 WHERE ID = {lotid}", con))
                                //                {
                                //                    cmd.CommandType = CommandType.Text;
                                //                    cmd.ExecuteNonQuery();
                                //                }
                                //            }

                                //            MessageBox.Show($"No file Processed successfully with {lotid} as LotID ",
                                //                                   "Message",
                                //                                   MessageBoxButtons.OK,
                                //                                   MessageBoxIcon.Information
                                //                                   );
                                //            logString.Append("\n6. No file Processed successfully:\n");
                                //            Console.WriteLine($"\n6. No file Processed successfully:\n");
                                //            logString.Append("\n7. Generating summary report with file lot ID:\n");
                                //            Console.WriteLine($"\n7. Generating summary report with file lot ID:\n");
                                //            logString.Append($"    - Lot ID: {lotid}\n");
                                //            Console.WriteLine($"    - Lot ID: {lotid}\n");
                                //            logString.Append($"    - Total files processed: : {total_pro_file}\n");
                                //            Console.WriteLine($"    - Total files processed: : {total_pro_file}\n");
                                //            logString.Append($"    - Total files imported:  {i}\n");
                                //            Console.WriteLine($"    - Total files imported:  {i}\n");
                                //            logString.Append($"    - Total duplicate files:  {total_dup_file}\n");
                                //            Console.WriteLine($"    - Total duplicate files:  {total_dup_file}\n");
                                //            logString.Append($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                //            Console.WriteLine($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                //            logString.Append($"    - Total records processed: : {Total_no_of_records}\n");
                                //            Console.WriteLine($"    - Total records processed: : {Total_no_of_records}\n");
                                //        }
                                //        else
                                //        {
                                //            using (SqlConnection con = new SqlConnection(connectionString))
                                //            {
                                //                con.Open();
                                //                using (SqlCommand cmd = new SqlCommand("UPDATE FileLotMaster SET [DataGenProcessStatus]= 15 WHERE ID = (SELECT MAX(ID) FROM FileLotMaster)", con))
                                //                {
                                //                    cmd.CommandType = CommandType.Text;
                                //                    cmd.ExecuteNonQuery();
                                //                }
                                //            }
                                //            this.Invoke(new MethodInvoker(delegate
                                //            {
                                //                txtoutput.Text += $"Files Processed successfully with {lotid} as LotID. \r\n";
                                //            }));

                                //            //MessageBox.Show($"Files uploaded successfully with {lotid} as LotID ",
                                //            //                       "Message",
                                //            //                       MessageBoxButtons.OK,
                                //            //                       MessageBoxIcon.Information
                                //            //                       );

                                //            logString.Append("\n7. Generating summary report with file lot ID:\n");
                                //            Console.WriteLine($"\n7. Generating summary report with file lot ID:\n");
                                //            logString.Append($"    - Lot ID: {lotid}\n");
                                //            Console.WriteLine($"    - Lot ID: {lotid}\n");
                                //            logString.Append($"    - Total files processed: : {total_pro_file}\n");
                                //            Console.WriteLine($"    - Total files processed: : {total_pro_file}\n");
                                //            logString.Append($"    - Total files imported:  {i}\n");
                                //            Console.WriteLine($"    - Total files imported:  {i}\n");
                                //            logString.Append($"    - Total duplicate files:  {total_dup_file}\n");
                                //            Console.WriteLine($"    - Total duplicate files:  {total_dup_file}\n");
                                //            FileProcessingLotID = lotid;
                                //        }
                                //    }
                                //}
                                //else
                                //{
                                //    int j = 0;
                                //    foreach (int hdid in InsertedHDIDS)
                                //    {
                                //        lastInsertedId = hdid;
                                //        Console.WriteLine($"last hdid : " + lastInsertedId);

                                //        int test_data_validation_1 = All_Record_Processing();
                                //        if (test_data_validation_1 == 10)
                                //        {
                                //            throw new InvalidOperationException("HSM error Connectivity Lost!!!");
                                //        }
                                //        if (test_data_validation_1 == 100)
                                //        {
                                //            throw new InvalidOperationException("Data Configuration error");
                                //        }

                                //        else
                                //        {
                                //            j += test_data_validation_1;
                                //        }

                                //        //j += All_Record_Processing();
                                //    }
                                //    if (j < filenames.Length)
                                //    {
                                //        DialogResult result = MessageBox.Show($"{i}/{filenames.Length} File uploaded & Processed successfully.\nWant to proceed further if not the data will be deleted from database.",
                                //                 "Message",
                                //                 MessageBoxButtons.YesNo,
                                //                 MessageBoxIcon.Information
                                //                 );

                                //        if (result == DialogResult.No)
                                //        {
                                //            using (SqlConnection con = new SqlConnection(connectionString))
                                //            {
                                //                con.Open();
                                //                SqlDataReader reader = null;
                                //                using (SqlCommand cmd = new SqlCommand($"Delete FROM [dbo].[DataGenProcessDataRecord] WHERE DataGenProcessHDID IN (SELECT Distinct DataGenProcessHDID FROM [DataGenProcessHDFile] WHERE FileLotID={lotid});", con))
                                //                {
                                //                    int rowsAffected = cmd.ExecuteNonQuery();

                                //                    if (rowsAffected > 0)
                                //                    {
                                //                        MessageBox.Show($"Data deleted successfully.");
                                //                        logString.Append($"\n6. Data deleted successfully from database.\n");
                                //                        Console.WriteLine($"\n6. Data deleted successfully from database.\n");
                                //                    }
                                //                    else
                                //                    {
                                //                        MessageBox.Show($"Database Error.");
                                //                        logString.Append($"\n6. Unable to delete uploaded Files with {lotid} lot.\n");
                                //                        Console.WriteLine($"\n6. Unable to delete uploaded Files with {lotid} lot.\n");
                                //                    }
                                //                }
                                //            }
                                //        }
                                //        else
                                //        {
                                //            logString.Append($"\n6. {i}/{filenames.Length} Files successfully imported & Processed.\n");
                                //            Console.WriteLine($"\n6. {i}/{filenames.Length} Files successfully imported & Processed.\n");
                                //            logString.Append("\n7. Generating summary report with file lot ID:\n");
                                //            Console.WriteLine($"\n7. Generating summary report with file lot ID:\n");
                                //            logString.Append($"    - Lot ID: {lotid}\n");
                                //            Console.WriteLine($"    - Lot ID: {lotid}\n");
                                //            logString.Append($"    - Total files processed: : {total_pro_file}\n");
                                //            Console.WriteLine($"    - Total files processed: : {total_pro_file}\n");
                                //            logString.Append($"    - Total files imported:  {i}\n");
                                //            Console.WriteLine($"    - Total files imported:  {i}\n");
                                //            logString.Append($"    - Total duplicate files:  {total_dup_file}\n");
                                //            Console.WriteLine($"    - Total duplicate files:  {total_dup_file}\n");
                                //            logString.Append($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                //            Console.WriteLine($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                //            MessageBox.Show($"you can procceed to processing with remaining files.");
                                //            FileProcessingLotID = lotid;
                                //        }

                                //    }
                                //    else if (j == 0)
                                //    {
                                //        using (SqlConnection con = new SqlConnection(connectionString))
                                //        {
                                //            con.Open();
                                //            using (SqlCommand cmd = new SqlCommand($"UPDATE FileLotMaster SET [DataGenProcessStatus]= 16 WHERE ID = {lotid}", con))
                                //            {
                                //                cmd.CommandType = CommandType.Text;
                                //                cmd.ExecuteNonQuery();
                                //            }
                                //        }

                                //        MessageBox.Show($"No file Processed successfully with {lotid} as LotID ",
                                //                               "Message",
                                //                               MessageBoxButtons.OK,
                                //                               MessageBoxIcon.Information
                                //                               );
                                //        logString.Append("\n6. No file Processed successfully:\n");
                                //        Console.WriteLine($"\n6. No file Processed successfully:\n");
                                //        logString.Append("\n7. Generating summary report with file lot ID:\n");
                                //        Console.WriteLine($"\n7. Generating summary report with file lot ID:\n");
                                //        logString.Append($"    - Lot ID: {lotid}\n");
                                //        Console.WriteLine($"    - Lot ID: {lotid}\n");
                                //        logString.Append($"    - Total files processed: : {total_pro_file}\n");
                                //        Console.WriteLine($"    - Total files processed: : {total_pro_file}\n");
                                //        logString.Append($"    - Total files imported:  {i}\n");
                                //        Console.WriteLine($"    - Total files imported:  {i}\n");
                                //        logString.Append($"    - Total duplicate files:  {total_dup_file}\n");
                                //        Console.WriteLine($"    - Total duplicate files:  {total_dup_file}\n");
                                //        logString.Append($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                //        Console.WriteLine($"    - Total error during importation files:  {total_pro_file - total_dup_file - i}\n");
                                //    }
                                //    else
                                //    {
                                //        using (SqlConnection con = new SqlConnection(connectionString))
                                //        {
                                //            con.Open();
                                //            using (SqlCommand cmd = new SqlCommand("UPDATE FileLotMaster SET [DataGenProcessStatus]= 15 WHERE ID = (SELECT MAX(ID) FROM FileLotMaster)", con))
                                //            {
                                //                cmd.CommandType = CommandType.Text;
                                //                cmd.ExecuteNonQuery();
                                //            }
                                //        }
                                //        this.Invoke(new MethodInvoker(delegate
                                //        {
                                //            txtoutput.Text += $"Files Processed successfully with {lotid} as LotID. \r\n";
                                //        }));

                                //        //MessageBox.Show($"Files uploaded successfully with {lotid} as LotID ",
                                //        //                       "Message",
                                //        //                       MessageBoxButtons.OK,
                                //        //                       MessageBoxIcon.Information
                                //        //                       );

                                //        logString.Append("\n7. Generating summary report with file lot ID:\n");
                                //        Console.WriteLine($"\n7. Generating summary report with file lot ID:\n");
                                //        logString.Append($"    - Lot ID: {lotid}\n");
                                //        Console.WriteLine($"    - Lot ID: {lotid}\n");
                                //        logString.Append($"    - Total files processed: : {total_pro_file}\n");
                                //        Console.WriteLine($"    - Total files processed: : {total_pro_file}\n");
                                //        logString.Append($"    - Total files imported:  {i}\n");
                                //        Console.WriteLine($"    - Total files imported:  {i}\n");
                                //        logString.Append($"    - Total duplicate files:  {total_dup_file}\n");
                                //        Console.WriteLine($"    - Total duplicate files:  {total_dup_file}\n");
                                //        FileProcessingLotID = lotid;
                                //    }
                                //}
                            }
                            if (licenceFile!= "Selected profile has no licence file")
                            {
                                string status = Importlicencefile(lotid);
                                if (status != "")
                                {
                                    MessageBox.Show(status);
                                }
                            }

                            int j = 0;
                            foreach (int hdid in InsertedHDIDS)
                            {
                                lastInsertedId = hdid;
                                Console.WriteLine(lastInsertedId);
                                int test_data_validation = All_Record_Processing();
                                if (test_data_validation == 10)
                                {
                                    throw new InvalidOperationException("HSM error Connectivity Lost!!!");
                                }
                                if (test_data_validation == 100)
                                {
                                    throw new InvalidOperationException("Data Configuration error");
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
                                                MessageBox.Show($"Data deleted successfully.");
                                                logString.Append($"\n6. Data deleted successfully from database.\n");
                                                Console.WriteLine($"\n6. Data deleted successfully from database.\n");
                                            }
                                            else
                                            {
                                                MessageBox.Show($"Database Error.");
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
                                    logString.Append($"    - Total records processed: : {Total_no_of_records}\n");
                                    Console.WriteLine($"    - Total records processed: : {Total_no_of_records}\n");
                                    MessageBox.Show($"you can procceed to processing with remaining files.");
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
                                logString.Append($"    - Total records processed: : {Total_no_of_records}\n");
                                Console.WriteLine($"    - Total records processed: : {Total_no_of_records}\n");
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
                else
                {
                    MessageBox.Show($"All fields are required: ",
                                                "Error",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information
                                                );
                    logString.Append("\nAll fields are required: \n");
                    Console.WriteLine($"\nAll fields are required: \n");
                    return;
                }
                if (FileProcessingLotID > 0)
                {
                    List<int> ids = new List<int>();
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
                                ids.Add(reader.GetInt32(0));
                                //lastInsertedId = Convert.ToInt32(reader.GetInt32(0));
                                //if (lastInsertedId > 0)
                                //{
                                //    btnGenerateAllFiles_Click();
                                //}
                            }


                        }
                    }
                    // ✅ Now safe to call your method
                    foreach (var id in ids)
                    {
                        lastInsertedId = id;
                        if (lastInsertedId > 0)
                        {
                            btnGenerateAllFiles_Click();
                        }
                    }
                }

                

                this.Invoke(new MethodInvoker(delegate
                {
                    txtInputfile.Text = "";
                    txtLicence.Text = "";
                }));

                if (InsertedHDIDS.Count > 1)
                {
                    //..merged_batch_list code of merging all label files
                    string[] merged_outer_label_file_names_array = merged_outer_label_file_names.ToArray();
                    string[] merged_inner_label_file_names_array = merged_inner_label_file_names.ToArray();
                    string[] merged_outer_label_file_names_1_array = merged_outer_label_file_names_1.ToArray();
                    string[] merged_batch_list_array = merged_batch_list.ToArray();
                    string[][] allFileSets = new string[merged_outer_label_file_names_array.Length][];

                    for (int i = 0; i < merged_outer_label_file_names_array.Length; i++)
                    {
                        allFileSets[i] = new string[]
                        {
                        merged_outer_label_file_names_array[i],
                        merged_inner_label_file_names_array[i],
                        merged_outer_label_file_names_1_array[i],
                        merged_batch_list_array[i]
                        };
                    }
                    MergeCsvsFromFolders(allFileSets);
                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                }

                logString.Append($"\nTotal files processed: {Total_no_of_files}. \nTotal records processed: {Total_no_of_records}.\n");
                logString.Append($"\n**************************************[Logging Out] Data Processing is Completed [{DateTime.Now}] **************************************\n");
                Console.WriteLine($"\n**************************************[Logging Out] Data Processing is Completed [{DateTime.Now}] **************************************\n");
                Database.sql_data_update("update [dbo].[DataTool_Keys] set KeyValue  = '" + EID_db_last + "' where[KeyName] = 'EID'");
                


            }
            catch (Exception ex)
            {
                logString.Append("\nSomething went wrong during importation: " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
                Console.WriteLine($"\nSomething went wrong during importation: "+ "\n\nStack Trace:\n" + ex.StackTrace);
                logString.Append($"\n**************************************[Logging Out] Data Processing is Failed  [{DateTime.Now}]**************************************\n");
                Console.WriteLine($"\n**************************************[Logging Out] Data Processing is Failed  [{DateTime.Now}]**************************************\n");

                Console.WriteLine($"Something went wrong during importation Exception: " + ex.Message);
                Console.WriteLine($"Something went wrong during importation Stack trace: " + ex.StackTrace);
                MessageBox.Show(ex.Message);
                return;

            }
            finally
            {
                //if (!Directory.Exists(log_dir + "/Logging"))
                //{
                //    Directory.CreateDirectory(log_dir + "/Logging");
                //}
                //System.IO.File.AppendAllText(log_dir + "/Logging/" + $"{DateTime.Now.ToString("dd-MM-yyyy")}_log.txt", logString.ToString());
                upload_log();
                logString.Clear();
               
                MessageBox.Show($"File Generated for LotID : {FileProcessingLotID}");
                //this.Close();
                
            }

        }

        public void startProcessing_dummy()
        {
            string folderPath = "";
            string outputPath = "";

            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    logString.Append($"\n1. User initiated the data processing tool and selected the following input:-\n    Customer : {cbxCustomer.Text}\n    Circle : {cbxCircle.Text}\n    Profile : {cbxProfile.Text}\n");
                }));

                logString.Append($"\n1. User Entered File QTY = : {dummy_file_qty}\n");

                if ((Debugger.IsAttached) || connectionString.Contains("192.168.5.22"))
                {
                    folderPath = Path.Combine("D:\\Productions\\", $"{customer}\\{profile}\\{DateTime.Now:yyyyMMdd}");
                }
                else
                {
                    folderPath = Path.Combine("\\\\192.168.27.5\\Productions\\", $"{customer}\\{profile}\\{DateTime.Now:yyyyMMdd}");
                }

                Directory.CreateDirectory(folderPath);

                outputPath = Path.Combine(
                    folderPath,
                    $"{customer}_{dummy_file_qty}_{DateTime.Now:yyyyMMdd_HHmmss}.mca"
                );

                string header_Data = Database.sql_data_value("select header from OutFileTemplateHD where profilefileid = 1 and ProfileID = '" + ProfileID + "'", "header");
                string dummy_Data = Database.sql_data_value("select header from OutFileTemplateHD where profilefileid = 3 and ProfileID = '" + ProfileID + "'", "header");

                var outputLines = new List<string> { header_Data };
                for (int i = 1; i <= dummy_file_qty; i++)
                    outputLines.Add(dummy_Data);

                //File.WriteAllLines(outputPath, outputLines);

                // Create hidden file for writing
                using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    // Mark as hidden immediately
                    File.SetAttributes(outputPath, FileAttributes.Hidden);

                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        foreach (string line in outputLines)
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
                logString.Append($"    - Outfile Generation Started:\n");
                int mca_batchsize = batchsize;

                batchsize = batchsize == 0 ? 2500 : batchsize;

                string filename_labels = CreateMCABatch(outputPath, batchsize, 500, Po_Num , customer_name_form);
                string[] label_filename_parts = filename_labels.Split(',');

                logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[0])}\n");
                logString.Append($"    - No of record : {dummy_file_qty}\n");
                logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[1])}\n");
                logString.Append($"    - No of record : {dummy_file_qty}\n");
                logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[2])}\n");
                logString.Append($"    - No of record : {dummy_file_qty}\n");
                logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[3])}\n");
                logString.Append($"    - No of record : {dummy_file_qty}\n");

                if (mca_batchsize > 0)
                {
                    string[] lines = File.ReadAllLines(outputPath);
                    if (IsSingle && lines.Length > mca_batchsize)
                    {
                        int numFiles = (int)Math.Ceiling((double)(lines.Length-1) / mca_batchsize);

                        logString.Append($"    - MCA file starting splitted into {numFiles} parts \n");

                        for (int i = 0; i < numFiles; i++)
                        {
                            string outputFile = outputPath.Replace(".mca", $"_{(i + 1):D4}.mca");

                            // Create hidden file for writing
                            using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                File.SetAttributes(outputFile, FileAttributes.Hidden); // mark hidden immediately

                                using (StreamWriter writer = new StreamWriter(fs))
                                {
                                    writer.WriteLine(lines[0]);
                                    for (int j = 1; (j <= mca_batchsize && (j + i * mca_batchsize) <= lines.Length - 1); j++)
                                    {
                                        writer.WriteLine(lines[j + i * mca_batchsize]);
                                    }
                                }
                            }

                            // Encrypt file
                            outputFile = EncryptionandDecryption.AESEncrypt_File(outputFile, OFProcessing.file_enc_key);

                            // Make encrypted file visible
                            if (File.Exists(outputFile))
                            {
                                File.SetAttributes(outputFile, FileAttributes.Normal);
                            }

                            logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                            logString.Append($"    - No of record : {mca_batchsize}\n");
                        }


                        File.Delete(outputPath);
                    }
                    else
                    {
                        string outputFile = EncryptionandDecryption.AESEncrypt_File(outputPath, OFProcessing.file_enc_key);
                        // Make encrypted file visible
                        if (File.Exists(outputFile))
                        {
                            File.SetAttributes(outputFile, FileAttributes.Normal);
                        }
                        logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                        logString.Append($"    - No of record : {lines.Length - 1}\n");
                    }
                }
                

                    logString.Append($"Files Processed successfully with data count : {dummy_file_qty}\r\n");
                this.Invoke(new MethodInvoker(delegate
                {
                    txtoutput.Text += $"Files Processed successfully with data count : {dummy_file_qty} \r\n";
                }));

                this.Invoke(new MethodInvoker(delegate
                {
                    txtInputfile.Text = "";
                    txtLicence.Text = "";
                }));

                //logString.Append($"Files Processed successfully with data count : {dummy_file_qty}\r\n");
                logString.Append($"\n**************************************[Logging Out] Data Processing is Completed [{DateTime.Now}] **************************************\n");
                upload_log();
            }
            catch (Exception ex)
            {
                logString.Append($"\n[ERROR] Exception occurred: {ex.Message}\n{ex.StackTrace}\n");
                upload_log();
                try
                {
                    // Delete all files and folder if exists
                    if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
                    {
                        Directory.Delete(folderPath, true);
                        logString.Append($"\n[INFO] All mca files and folder deleted from productions due to error.\n");
                    }
                    string label_path = folderPath.Replace("\\Productions\\", "\\Data_Gen\\Label\\");
                    // Delete all files and folder if exists
                    if (!string.IsNullOrEmpty(label_path) && Directory.Exists(label_path))
                    {
                        Directory.Delete(label_path, true);
                        logString.Append($"\n[INFO] All label files and folder deleted from label due to error.\n");
                    }
                }
                catch (Exception delEx)
                {
                    logString.Append($"\n[ERROR] Failed to delete files/folder: {delEx.Message}\n");
                }

                this.Invoke(new MethodInvoker(delegate
                {
                    txtoutput.Text += $"❌ Processing failed. Error: {ex.Message}\r\n";
                }));
            }
        }

        public int First_Record_Processing(int hdid, int lot)
        {
            string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlConnection con = new SqlConnection(constr);
            string filename_2 = getfilenameandid();
            String Query0 = "SELECT * FROM InPutDataTemplate WHERE CustID = " + OFProcessing.customerID + " and ProfileID =" + OFProcessing.ProfileID + " and  trim(VarText) = 'FL' and isnull(LineNumber,0)!=0 and isnull(Len,0)!=0  order by VarName ";
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
            int Batch_line_no = 0;
            int Batch_frm = 0;
            int Batch_len = 0;
            int iccid_frm = 0;
            int iccid_len = 0;
            int imsi_from = 0;
            int imsi_len = 0;
            int msisdn_from = 0;
            int msisdn_len = 0;
            bool IsIncremental = false;
            int error_frm = 0;
            int error_len = 0;
            int error_line_no = 0;

            int qty = 0;
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

                    error_frm = Convert.ToInt32(Pos_From);
                    error_len = Convert.ToInt32(len_data);
                    error_line_no = Convert.ToInt32(line_sql);



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
                    }
                    else if (var_des == "BatchNumber")
                    {
                        Batch_line_no = Convert.ToInt32(line_sql);
                        Batch_frm = Convert.ToInt32(Pos_From);
                        Batch_len = Convert.ToInt32(len_data);

                        //for batchnumber
                        try
                        {
                            batchnumber_file = data_val_line[Batch_line_no - 1].Substring(Batch_frm, Batch_len).Trim();
                        }
                        catch
                        {

                            try
                            {
                                string line_test = data_val_line[Batch_line_no - 1];
                                int safeLen = Math.Min(Batch_len, line_test.Length - Batch_frm);
                                batchnumber_file = line_test.Substring(Batch_frm, safeLen).Trim();
                            }
                            catch
                            {
                                batchnumber_file = "";
                            }


                        }
                    }

                }
            }
            if (IsIncremental)
            {
                try
                {


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
                    //for qty



                    //data_val_line = data_val_line.Split('\n')[imsi_line_no - 1];

                    try
                    {
                        iccid = Convert.ToInt64(data_val_line[iccid_line_no - 1].Substring(iccid_frm, iccid_len).Trim());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to read ICCID from input file Linenumber:{iccid_line_no} , position from : {iccid_frm} , length : {iccid_len}");
                        throw;
                    }

                    try
                    {
                        imsi = Convert.ToInt64(data_val_line[imsi_line_no - 1].Substring(imsi_from, imsi_len).Trim());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to read ICCID from input file Linenumber:{imsi_line_no} , position from : {imsi_from} , length : {imsi_len}");
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
                        MessageBox.Show($"Unable to read ICCID from input file Linenumber:{imsi_line_no} , position from : {imsi_from} , length : {imsi_len}");
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
                    MessageBox.Show($"Error occurred during import of incremental records.\n\n" + $"Variable: {var_des}\n" + $"Required Line Number: {error_line_no}\n" + $"Position From: {error_frm}\n" + $"Length: {error_len}\n\n" + $"Error Message: {ex.Message}\n\n" + $"Stack Trace:\n{ex.StackTrace}");
                    string error = $"Error During Importation for incremental records in file '{Path.GetFileName(filename_2)}' : on variable {var_des} error_msg id " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace;

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
                        if (line_number >= line_no && line_number <= iccid_line_no + qty - 1)
                        {

                            if (msisdn_from == 0 && msisdn_len == 0)
                            {
                                resultDataTable.Rows.Add(StringToHex(line.Substring(iccid_frm, iccid_len).Trim()), StringToHex(line.Substring(imsi_from, imsi_len).Trim()), "", line.Substring(iccid_frm, iccid_len).Trim(), line.Substring(imsi_from, imsi_len).Trim(), "");

                                //try
                                //{
                                //    string iccid_raw = "";
                                //    string imsi_raw = "";
                                //    string iccid_hex = "";
                                //    string imsi_hex = "";

                                //    try
                                //    {
                                //        iccid_raw = line.Substring(iccid_frm, iccid_len).Trim();
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        throw new Exception($"Error extracting ICCID | Start: {iccid_frm}, Length: {iccid_len}, LineLength: {line.Length}", ex);
                                //    }

                                //    try
                                //    {
                                //        imsi_raw = line.Substring(imsi_from, imsi_len).Trim();
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        throw new Exception($"Error extracting IMSI | Start: {imsi_from}, Length: {imsi_len}, LineLength: {line.Length}", ex);
                                //    }

                                //    try
                                //    {
                                //        iccid_hex = StringToHex(iccid_raw);
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        throw new Exception($"Error converting ICCID to HEX | Value: {iccid_raw}", ex);
                                //    }

                                //    try
                                //    {
                                //        imsi_hex = StringToHex(imsi_raw);
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        throw new Exception($"Error converting IMSI to HEX | Value: {imsi_raw}", ex);
                                //    }

                                //    resultDataTable.Rows.Add(
                                //        iccid_hex,
                                //        imsi_hex,
                                //        "",
                                //        iccid_raw,
                                //        imsi_raw,
                                //        ""
                                //    );
                                //}
                                //catch (Exception ex)
                                //{
                                //    // Log full error
                                //    Console.WriteLine("ERROR: " + ex.Message);

                                //    // Optional: write to file
                                //    // File.AppendAllText("error_log.txt", ex.ToString());

                                //    throw; // or remove if you don't want crash
                                //}
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
                    MessageBox.Show($"Error occurred during import.\n\n" + $"Variable: {var_des}\n" + $"Required Line Number: {error_line_no}\n" + $"Position From: {error_frm}\n" + $"Length: {error_len}\n\n" + $"Error Message: {ex.Message}\n\n" + $"Stack Trace:\n{ex.StackTrace}");
                    string error = $"Error During Importation in file '{Path.GetFileName(filename_2)}' : on variable {var_des} error_msg id " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace;
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
                string r4_data = "", r8_data = "", euicc_ci_cert_data = "", euicc_eum_cert_data = "", euicc_pri_wrapped_key = "", euicc_pub_key_value = "",rjio_po = "",rjio_sku = "";
                int r4_data_count = 0, r8_data_count = 0, records_no = 0;

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
                    //Console.WriteLine($"inserting all data : " + var_name);

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
                                        MessageBox.Show(exe.Message + "\n\nStack Trace:\n" + exe.StackTrace);
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
                                                MessageBox.Show(exe.Message + "\n\nStack Trace:\n" + exe.StackTrace);
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
                                        else if (var_des == "PO")
                                        {
                                            rjio_po =   myData;
                                        }
                                        else if (var_des == "SKU")
                                        {
                                            rjio_sku = myData;
                                        }
                                        else if (var_des == "BATCH_NO")
                                        {
                                            generic_batch_no = myData;
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
                                                    MessageBox.Show(exe.Message + "\n\nStack Trace:\n" + exe.StackTrace);
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
                            //single record
                            if (Var_Text.TrimEnd() == "AL")
                            {
                                //string tag_value_1 = tag_value.Replace('[')
                                int varCount = tag_value.Count(c => c == ',');
                                string[] parts = Array.Empty<string>();
                                string caseSwitch = Algo_Name;
                                string key_tag = "", data_tag;



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
                                            if (len == 0)
                                            { len = data_new_test.Trim().Length - pos_from + 1; }

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

                                    case "Input_Filename":

                                        string fileNameOnly = Path.GetFileNameWithoutExtension(filename_2);
                                        // Check if both are null/empty
                                        if (string.IsNullOrWhiteSpace(tag_value) && string.IsNullOrWhiteSpace(len_data))
                                        {
                                            myData = fileNameOnly;
                                        }
                                        else
                                        {
                                            char separator = !string.IsNullOrEmpty(tag_value) ? tag_value[0] : '\0';

                                            int index = int.TryParse(len_data, out int temp) ? temp : 0;

                                            myData = Input_Filename_sep(fileNameOnly, separator, index);
                                        }
                                        //char separator = tag_value[0];
                                        //int index = int.Parse(len_data);
                                        //myData = Input_Filename_sep(fileNameOnly, separator, index);
                                        break;



                                    case "YYYYMMDDHHMMSS":
                                        myData = timestamp;
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

                                    case "ICCID_NS_U":
                                        myData = Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString();
                                        myData = nibble_swapped_U(myData);

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

                                    case "MCCMNC":
                                        myData = mncmcc_function(Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString(), Convert.ToInt32(len_data));
                                        break;
                                    
                                    case "SERIALNO":
                                        myData = "1";
                                        break;

                                    case "DATE_FORMAT":
                                        myData = GetFormattedDate($"{tag_value}");
                                        break;

                                    case "KI_AES_128":
                                        parts = tag_value.Split(',');
                                        // e.g. "KeyVar,DataVar"
                                        if (parts.Length >= 2)
                                        {
                                            key_tag = Process_data.Select($"Variable = '{parts[0].Trim()}'")[0]["Value"].ToString();
                                            data_tag = Process_data.Select($"Variable = '{parts[1].Trim()}'")[0]["Value"].ToString();

                                            myData = AES_ENCYPRTION(key_tag, data_tag);
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
                                            key_tag = Process_data.Select($"Variable = '{parts[0].Trim()}'")[0]["Value"].ToString();
                                            data_tag = Process_data.Select($"Variable = '{parts[1].Trim()}'")[0]["Value"].ToString();

                                            myData = Encrypt_SingleDES(key_tag, data_tag);
                                        }
                                        else
                                        {
                                            throw new Exception($"Single_Des requires 2 variables, but got: {tag_value}");
                                        }
                                        break;


                                    case "AES_128":
                                        parts = tag_value.Split(',');
                                        // e.g. "KeyVar,DataVar"
                                        if (parts.Length >= 2)
                                        {
                                            key_tag = Process_data.Select($"Variable = '{parts[0].Trim()}'")[0]["Value"].ToString();
                                            data_tag = Process_data.Select($"Variable = '{parts[1].Trim()}'")[0]["Value"].ToString();

                                            myData = OPC_GEN.opc(key_tag, data_tag);
                                        }
                                        else
                                        {
                                            throw new Exception($"OPC_GEN requires 2 variables, but got: {tag_value}");
                                        }

                                        break;


                                    case "AES256_ECB":
                                        parts = tag_value.Split(',');
                                        // e.g. "KeyVar,DataVar"
                                        if (parts.Length >= 2)
                                        {
                                            data_tag = Process_data.Select($"Variable = '{parts[0].Trim()}'")[0]["Value"].ToString();
                                            key_tag = Process_data.Select($"Variable = '{parts[1].Trim()}'")[0]["Value"].ToString();

                                            myData = EncryptAes256(key_tag, data_tag, "ECB");
                                        }
                                        else
                                        {
                                            throw new Exception($"OPC_GEN requires 2 variables, but got: {tag_value}");
                                        }

                                        break;


                                    case "AES256_CBC":
                                        parts = tag_value.Split(',');
                                        // e.g. "KeyVar,DataVar"
                                        if (parts.Length >= 2)
                                        {
                                            data_tag = Process_data.Select($"Variable = '{parts[0].Trim()}'")[0]["Value"].ToString();
                                            key_tag = Process_data.Select($"Variable = '{parts[1].Trim()}'")[0]["Value"].ToString();

                                            myData = EncryptAes256(key_tag, data_tag, "CBC");
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
                                            key_tag = Process_data.Select($"Variable = '{parts[0].Trim()}'")[0]["Value"].ToString();
                                            data_tag = Process_data.Select($"Variable = '{parts[1].Trim()}'")[0]["Value"].ToString();

                                            myData = TripleDESEncrypt_cbc(key_tag, data_tag);
                                        }
                                        else
                                        {
                                            throw new Exception($"Triple_Des_CBC requires 2 variables, but got: {tag_value}");
                                        }

                                        break;

                                    case "euicc_cert_data":
                                        parts = tag_value.Split(',');
                                        string response_data = euicc_Data_function(parts[0].Trim(), parts[1].Trim(), 1, 1);
                                        response_data = response_data.Replace("\"", "");
                                        if (response_data.Contains("CKR_GENERAL_ERROR"))
                                        {
                                            throw new Exception($"hsm connectivity issue {tag_value}");
                                            logString.Append($"\nHsm connectivity issue {tag_value}");
                                        }

                                        response_data = response_data.Replace("}", "");
                                        parts = response_data.Split(',');
                                        euicc_ci_cert_data = parts[1].Replace("root_ca_hex:", "").Trim();
                                        euicc_eum_cert_data = parts[2].Replace("sub_ca_hex:", "").Trim();
                                        euicc_pub_key_value = parts[4].Replace("public_key:", "").Trim();
                                        euicc_pri_wrapped_key = parts[6].Replace("wrapped_key:", "").Trim();
                                        myData = parts[3].Replace("end_entity_hex:", "").Trim();
                                        break;


                                    case "euicc_cert_ci":
                                        myData = euicc_ci_cert_data;
                                        if (myData == "")
                                        {
                                            throw new Exception($"hsm connectivity issue {tag_value}");
                                        }
                                        break;

                                    case "euicc_pub_key":
                                        myData = euicc_pub_key_value;
                                        if (myData == "")
                                        {
                                            throw new Exception($"hsm connectivity issue {tag_value}");
                                        }
                                        break;

                                    case "euicc_cert_eum":
                                        myData = euicc_eum_cert_data;
                                        break;

                                    case "euicc_pri_wrapped":
                                        myData = euicc_pri_wrapped_key;
                                        break;


                                    case "EID":
                                        myData = eid_function(1);
                                        break;

                                    case "AES_WRAP":
                                        string key = Process_data.Select($"Variable = '{parts[1].Trim()}'")[0]["Value"].ToString();
                                        string data = Process_data.Select($"Variable = '{parts[0].Trim()}'")[0]["Value"].ToString();
                                        myData = AES_WRAP(key, data);
                                        break;

                                    case "CHECKSUM":

                                        myData = CHECKSUM(Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString());
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
									) OUTPUT inserted.VarValue
									WHERE VarID = @varName", con10))
                                    {
                                        cmd.Parameters.AddWithValue("@varValue", var_Value.TrimEnd());
                                        cmd.Parameters.AddWithValue("@profileID", ProfileID);
                                        cmd.Parameters.AddWithValue("@varName", var_name);
                                        object result = cmd.ExecuteScalar();

                                        myData = result?.ToString();
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
                        //Console.WriteLine($"Executing finally block." + var_name + " _ " + var_des + " _ " + myData);
                        Process_data.Rows.Add(var_name, var_des, myData);
                    }
                }
                
                if (customer_name_form.ToUpper() == "RELIANCE")
                {
                    rjio_prefix = "UCP" + rjio_sku.Substring(rjio_sku.Length - 2, 2) + rjio_po;
                }
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error During Importation : on variable {var_des} error_msg id " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
                string error = $"Error During Importation in file '{Path.GetFileName(filename_2)}' : " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace;
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

                //deletion_errorneous_data(lastInsertedId.ToString());
                return 0;
            }

        }


        private int All_Record_Processing()
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
            //for reliance jio 
            string prefix = rjio_prefix;
            
            List<string> generatedCodes = new List<string>();
            BatchCodeGenerator generatormsn = new BatchCodeGenerator(500);
            BatchCodeGenerator generatormsc = new BatchCodeGenerator(5000);
            //-----------------------------------------------------------------------------
            if (customer_name_form.ToUpper() == "RELIANCE")
            {
                string query = @"SELECT TOP 1 LastGeneratedCodeMSN, LastGeneratedCodeMSC
                     FROM CodeTracker 
                     WHERE Prefix = @prefix";

                SqlCommand cmd_rjio = new SqlCommand(query, con);
                cmd_rjio.Parameters.AddWithValue("@prefix", prefix);

                SqlDataAdapter adpt_rjio = new SqlDataAdapter(cmd_rjio);
                DataTable dt_rjio= new DataTable();
                adpt_rjio.Fill(dt_rjio);

                if (dt_rjio.Rows.Count > 0)
                {
                    lastCodeMSN = dt_rjio.Rows[0]["LastGeneratedCodeMSN"]?.ToString();
                    lastCodeMSC = dt_rjio.Rows[0]["LastGeneratedCodeMSC"]?.ToString();
                }
            }
            logString.Append($"    - [{Path.GetFileName(filename_2)}] File Processing Started.\n");
            Console.WriteLine($"    - [{Path.GetFileName(filename_2)}] File Processing Started.\n");
            String Query1 = $"SELECT DataGenProcessData.[DataGenProcessDataID], DataGenProcessData.[DataGenProcessHDID], InPutDataTemplate.[VarName] as VarID, DataGenProcessData.[VarName], DataGenProcessData.[VarValue], DataGenProcessData.[VarType], DataGenProcessData.[StatusID],[InPutDataTemplate].algoname,[InPutDataTemplate].VarText,[InPutDataTemplate].PositionFrom,[InPutDataTemplate].Len,InPutDataTemplate.tag, [InPutDataTemplate].LineNumber  ,[InPutDataTemplate].VarWay    FROM DataGenProcessData inner JOIN [InPutDataTemplate] ON [InPutDataTemplate].vardes = DataGenProcessData.varname where  DataGenProcessData.[DataGenProcessHDID] = '" + lastInsertedId + "' and [InPutDataTemplate].ProfileID=" + ProfileID + "  order by DataGenProcessData.VarID ";
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
            string[] default_values = { "AGSUI:IMSI", "EKI", "KIND", "DATE_AL", "FSETIND", "A4IND", "Transport_key", "Quantity", "Index_Value", "BatchNumber" };
            foreach (DataRow dv0 in dt1.Rows)
            {

                string var_ID = dv0[2].ToString().TrimEnd();
                Console.WriteLine($"All_Record_Processing " + var_ID);
                string var_name = dv0[3].ToString().TrimEnd();
                string var_Value = dv0[4].ToString().TrimEnd();
                string var_Type = dv0[5].ToString().TrimEnd();
                string var_text_fl = dv0[8].ToString().TrimEnd();
                //string var_way = dv0[13].ToString().TrimEnd();
                if (var_name.TrimEnd() == "Quantity")
                {
                    records = Int32.Parse(var_Value.TrimEnd());
                    Total_no_of_records += records; 
                    //Process_data.Columns.Add(var_ID, typeof(string)).DefaultValue = var_Value;
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


            List<string> varIDS = dt1.AsEnumerable()
                                  .Where(row => !string.IsNullOrEmpty(row.Field<string>("algoname")) && row.Field<string>("algoname").Contains("Hex") && row.Field<string>("algoname").Contains("R_"))
                                  .Select(row => row.Field<string>("VarID").Trim())
                                  .ToList();


            string urlText = "";
            string filename_input_file = getfilenameandid();

            string[] data_val_line = File.ReadAllLines(filename_input_file);
            for (int i = 1; i <= records; i++)
            {
                //System.Threading.Thread.Sleep(700); // 1000 ms = 1 second
                Dictionary<string, string> FetchAPI = new Dictionary<string, string>();
                string euicc_ci_cert_data = "", euicc_eum_cert_data = "", euicc_pri_wrapped_key = "", euicc_pub_key_value = "";


                if (records > 500)
                {
                    if (i % (records / 500) == 0)
                    {
                        string url = $"http://{hsm_IP.Trim()}/api/HSM?op_type=RND&digits={string.Join(",", middleValues)}&keyname=%27%27&ip_data=%27%27";
                        WebRequest request = HttpWebRequest.Create(url);
                        WebResponse response = request.GetResponse();
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        urlText = reader.ReadToEnd() + "rem";
                        if (urlText.Contains("CKR_DEVICE_REMOVED"))
                        {

                            //urlText = random_hex_generator(string.Join(",", middleValues)) + "rem";

                            logString.Append(" HSM CONNECTIVITY LOST Please check with Key Manager");
                            Console.WriteLine($" HSM CONNECTIVITY LOST Please check with Key Manager");
                            hsm_flag = 0;
                            logString.AppendLine("\nFile processing stopped.");
                            return 10;
                        }
                    }
                    else
                        urlText = random_hex_generator(string.Join(",", middleValues)) + "rem";
                }
                else if (records > 10)
                {
                    if (i % (records / 10) == 0)
                    {
                        string url = $"http://{hsm_IP.Trim()}/api/HSM?op_type=RND&digits={string.Join(",", middleValues)}&keyname=%27%27&ip_data=%27%27";
                        WebRequest request = HttpWebRequest.Create(url);
                        WebResponse response = request.GetResponse();
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        urlText = reader.ReadToEnd() + "rem";
                        if (urlText.Contains("CKR_DEVICE_REMOVED"))
                        {
                            logString.Append(" HSM CONNECTIVITY LOST Please check with Key Manager");
                            Console.WriteLine($" HSM CONNECTIVITY LOST Please check with Key Manager");
                            hsm_flag = 0;
                            logString.AppendLine("\nFile processing stopped.");
                            return 10;
                            //urlText = random_hex_generator(string.Join(",", middleValues)) + "rem";
                        }
                    }
                    else
                        urlText = random_hex_generator(string.Join(",", middleValues)) + "rem";
                }

                else
                {

                    string url = $"http://{hsm_IP.Trim()}/api/HSM?op_type=RND&digits={string.Join(",", middleValues)}&keyname=%27%27&ip_data=%27%27";
                    WebRequest request = HttpWebRequest.Create(url);
                    WebResponse response = request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    urlText = reader.ReadToEnd() + "rem";
                    if (urlText.Contains("CKR_DEVICE_REMOVED"))
                    {
                        logString.Append(" HSM CONNECTIVITY LOST Please check with Key Manager");
                        Console.WriteLine($" HSM CONNECTIVITY LOST Please check with Key Manager");
                        hsm_flag = 0;
                        logString.AppendLine("\nFile processing stopped.");
                        return 10;
                        //urlText = random_hex_generator(string.Join(",", middleValues)) + "rem";
                    }

                }






                urlText = urlText.Replace(",rem", "");
                var data = urlText.Split(',');
                int j = 0;
                try
                {
                    foreach (string var in varIDS)
                    {
                        FetchAPI[var] = data[j];
                        j++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"HSM is not on. Please check with Key Manager.\n\nError Details: " + ex.Message,
                                    "HSM Connection Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return 10;
                }

                Console.WriteLine($"Processing Row : " + i);
                DataRow newRow = Process_data.NewRow();
                string my_data = "";
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
                        int lineno = int.TryParse(Convert.ToString(dv0[12]).Trim(), out int temp) ? temp : 0;
                        string var_Type = dv0[13].ToString().TrimEnd();
                        int varCount = variableID.Count(c => c == ',');
                        my_data = "";
                        if (var_op_type == "FL")
                        {
                           

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
                                    my_data = var_Value.Trim();
                                }
                            }
                            else if (var_name.ToUpper() == "BATCHNUMBER")
                            {
                                my_data = var_Value.Trim();
                                batchnumber_file = my_data;
                            }
                            else if (var_Type == "T")
                            {
                                my_data = var_Value.Trim();
                                
                            }
                            else if (lineno != 0)
                            {
                                int.TryParse(Pos_From?.Trim(), out int Pos_From1);
                                int.TryParse(len_data?.Trim(), out int len_data1);

                                //my_data = data_val_line[lineno+i-2].Substring(Pos_From1,len_data1);

                               

                                try
                                {
                                    // try normal
                                    my_data = data_val_line[lineno + i - 2].Substring(Pos_From1, len_data1);
                                }
                                catch
                                {
                                    // fallback safe logic
                                    if (Pos_From1 < 0) Pos_From1 = 0;
                                    if (Pos_From1 > data_val_line[lineno + i - 2].Length) Pos_From1 = data_val_line[lineno + i - 2].Length;

                                    int safeLength = Math.Max(0, data_val_line[lineno + i - 2].Length - Pos_From1);

                                    my_data = safeLength > 0
                                        ? data_val_line[lineno + i - 2].Substring(Pos_From1, safeLength)
                                        : "";
                                    MessageBox.Show($"position from and  len is wrong for {var_name} ");
                                }

                            }

                            newRow[var_ID] = my_data;

                            
                        }
                        
                        //all records
                        if (var_op_type == "AL")
                        {
                            string caseSwitch = var_algoname;
                            
                            r4_data = "";
                            List<string> ki_val_list = new List<string>();
                            switch (caseSwitch)
                            {

                                case "substring":
                                    if (varCount > 0)
                                    {
                                        MessageBox.Show($"More than one Variable found in AlgoName-{var_algoname}  in 'Tag' Value");
                                        return 100;
                                    }
                                    else
                                    {
                                        int pos_from = Convert.ToInt32(Pos_From);
                                        int len = Convert.ToInt32(len_data);
                                        if (len == 0)
                                        { len = newRow[variableID].ToString().Trim().Length - pos_from + 1; }
                                        string data_new_test = newRow[variableID].ToString();
                                        Console.WriteLine(data_new_test.TrimEnd() + " " + pos_from + " " + len);

                                        my_data = newRow[variableID].ToString().Substring(pos_from - 1, len);
                                    }
                                    break;

                                case "concat":
                                    if (varCount == 1)
                                    {
                                        string[] varIDs = variableID.Split(',');
                                        Console.WriteLine($"{newRow[varIDs[0]].ToString()} --->  {newRow[varIDs[1]].ToString()}");
                                        my_data = (newRow[varIDs[0]].ToString() + newRow[varIDs[1]].ToString());
                                    }
                                    else
                                    {
                                        MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        return 100;
                                    }
                                    break;

                                case "identical":
                                    if (varCount > 0)
                                    {
                                        MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        return 100;
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

                                    }
                                    else
                                    {
                                        my_data = Random4digits();

                                    }
                                    break;

                                case "R_8_H":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;

                                    }
                                    else
                                    {
                                        my_data = Random8hex();

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
                                            return 100;
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
                                        if (varCount > 0)
                                        {
                                            MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                            return 100;
                                        }
                                        else
                                        {
                                            my_data = acc(newRow[variableID].ToString());
                                        }

                                    }
                                    break;

                                case "Input_Filename":

                                    my_data = var_Value;
                                    break;

                                case "YYYYMMDDHHMMSS":
                                    my_data = var_Value;

                                    break;

                                case "3P":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
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
                                        my_data = StringToHex(newRow[variableID].ToString());
                                    }
                                    break;



                                case "R_16_Hex":

                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        my_data = FetchAPI[var_ID];
                                    }

                                    break;

                                case "R_32_Hex":

                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        my_data = FetchAPI[var_ID];
                                    }
                                    break;

                                case "R_48_Hex":

                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        my_data = FetchAPI[var_ID];
                                    }
                                    break;

                                case "Pad_8":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        my_data = Pad3_F(newRow[variableID].ToString());
                                    }
                                    break;

                                case "Pad_16":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        my_data = Pad3_F(newRow[variableID].ToString());
                                    }
                                    break;

                                case "ICCID_NS":
                                    if (varCount > 0)
                                    {
                                        MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        return 100;
                                    }
                                    else
                                    {
                                        string icicid_num = newRow[variableID].ToString();
                                        my_data = nibble_swapped(icicid_num);
                                    }
                                    //string icicid_num = (Int64.Parse(first_icicid) + i).ToString();
                                    break;

                                case "ICCID_NS_U":
                                    if (varCount > 0)
                                    {
                                        MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        return 100;
                                    }
                                    else
                                    {
                                        string icicid_num = newRow[variableID].ToString();
                                        my_data = nibble_swapped_U(icicid_num);
                                    }
                                    //string icicid_num = (Int64.Parse(first_icicid) + i).ToString();
                                    break;

                                   



                                case "IMSI_NS":
                                    if (varCount > 0)
                                    {
                                        MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        return 100;
                                    }
                                    else
                                    {
                                        //string imsi_num = "809" + (Int64.Parse(first_imsi) + i).ToString();
                                        string imsi_num = "809" + newRow[variableID].ToString();
                                        my_data = nibble_swapped(imsi_num);
                                    }
                                    break;

                                case "R_32_Hex_KI":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        my_data = FetchAPI[var_ID];

                                    }
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
                                            return 100;
                                        }
                                        else
                                        {
                                            my_data = newRow[variableID].ToString();
                                            my_data += GetLuhnCheckDigit(newRow[variableID].ToString());

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
                                            return 100;
                                        }
                                        else
                                        {
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
                                            return 100;
                                        }
                                        else
                                        {
                                            my_data = CalculateKCV(newRow[variableID].ToString(), "DES");
                                        }
                                    }
                                    break;

                                case "SERIALNO":
                                    my_data = i.ToString();
                                    break;



                                case "MCCMNC":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        if (varCount > 0)
                                        {
                                            MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                            return 100;
                                        }
                                        else
                                        {
                                            my_data = mncmcc_function(newRow[variableID].ToString(), Convert.ToInt32(len_data));
                                        }
                                    }
                                    break;



                                case "DATE_FORMAT":
                                    my_data = var_Value;
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
                                            return 100;
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
                                            return 100;
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
                                        string[] varIDs = variableID.Split(',');
                                        my_data = AES_ENCYPRTION(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                    }
                                    else
                                    {
                                        MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        return 100;
                                    }
                                    break;

                                case "Single_Des":
                                    if (varCount == 1)
                                    {
                                        string[] varIDs = variableID.Split(',');
                                        my_data = Encrypt_SingleDES(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                    }
                                    else
                                    {
                                        MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                        return 100;
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
                                            string[] varIDs = variableID.Split(',');
                                            my_data = OPC_GEN.opc(newRow[varIDs[1]].ToString(), newRow[varIDs[0]].ToString());
                                        }
                                        else
                                        {
                                            MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                            return 100;
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
                                            string[] varIDs = variableID.Split(',');
                                            my_data = TripleDESEncrypt_cbc(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                        }

                                        else
                                        {
                                            MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                            return 100;
                                        }
                                    }
                                    break;

                                case "AES256_ECB":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        if (varCount == 1)
                                        {
                                            string[] varIDs = variableID.Split(',');
                                            my_data = EncryptAes256(newRow[varIDs[1]].ToString(), newRow[varIDs[0]].ToString(), "ECB");
                                        }

                                        else
                                        {
                                            MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                            return 100;
                                        }
                                    }
                                    break;

                                case "AES256_CBC":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        if (varCount == 1)
                                        {
                                            string[] varIDs = variableID.Split(',');
                                            my_data = EncryptAes256(newRow[varIDs[1]].ToString(), newRow[varIDs[0]].ToString(), "CBC");
                                        }

                                        else
                                        {
                                            MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                            return 100;
                                        }
                                    }
                                    break;

                                case "euicc_cert_data":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        string[] varIDs = variableID.Split(',');
                                        string[] parts_list = Array.Empty<String>();
                                        string response_data = euicc_Data_function(varIDs[0], varIDs[1], i, 1);
                                        response_data = response_data.Replace("\"", "");
                                        if (response_data.Contains("CKR_GENERAL_ERROR"))
                                        {
                                            throw new Exception($"hsm connectivity ");
                                            logString.Append($"\nHsm connectivity ");
                                        }

                                        parts_list = response_data.Split(',');
                                        euicc_ci_cert_data = parts_list[1].Replace("root_ca_hex:", "").Trim();
                                        euicc_eum_cert_data = parts_list[2].Replace("sub_ca_hex:", "").Trim();
                                        euicc_pub_key_value = parts_list[4].Replace("public_key:", "").Trim();
                                        euicc_pri_wrapped_key = parts_list[6].Replace("wrapped_key:", "").Trim();
                                        my_data = parts_list[3].Replace("end_entity_hex:", "").Trim();
                                        break;
                                    }
                                    break;

                                case "euicc_cert_ci":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        my_data = euicc_ci_cert_data;

                                    }
                                    break;

                                case "euicc_pub_key":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        my_data = euicc_pub_key_value;

                                    }
                                    break;

                                case "euicc_cert_eum":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        my_data = euicc_eum_cert_data;
                                    }
                                    break;

                                case "euicc_pri_wrapped":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        my_data = euicc_pri_wrapped_key;
                                    }
                                    break;


                                case "EID":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        my_data = eid_function(i);
                                    }
                                    break;


                                case "AES_WRAP":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {
                                        if (varCount == 1)
                                        {
                                            string[] varIDs = variableID.Split(',');
                                            my_data = AES_WRAP(newRow[varIDs[1]].ToString(), newRow[varIDs[0]].ToString());
                                        }

                                        else
                                        {
                                            MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                            return 100;
                                        }
                                    }
                                    break;


                                case "CHECKSUM":
                                    if (i == 0)
                                    {
                                        my_data = var_Value;
                                    }
                                    else
                                    {

                                        if (varCount > 0)
                                        {
                                            MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                            return 100;
                                        }
                                        else
                                        {

                                            my_data = CHECKSUM(newRow[variableID].ToString());
                                        }



                                    }

                                    break;


                                case "RJIO_MSN":
                                    if (customer_name_form.ToUpper() == "RELIANCE")
                                    {
                                        my_data = generatormsn.Generatemsn(prefix);
                                        //updateData.Add(i, code);
                                        lastCodeMSN = my_data;
                                    }

                                    break;

                                case "RJIO_MSC":
                                    if (customer_name_form.ToUpper() == "RELIANCE")
                                    {
                                        my_data = generatormsc.GenerateMSC(prefix);
                                        //updateData.Add(i, code);
                                        lastCodeMSC = my_data;
                                    }

                                    break;
                            }
                            newRow[var_ID] = my_data;
                          
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing var_ID: {dv0[2].ToString().TrimEnd()}, Error: {ex.Message} \nStack Trace:\n + {ex.StackTrace}");
                        logString.AppendLine($"Error processing var_ID: {dv0[2].ToString().TrimEnd()}, Error: {ex.Message}\nStack Trace:\n + {ex.StackTrace}");
                        MessageBox.Show($"Error processing var_ID: {dv0[2].ToString().TrimEnd()}, Error: {ex.Message}\nStack Trace:\n + {ex.StackTrace}");
                        //deletion_errorneous_data(lastInsertedId.ToString());
                        return 100;
                    }
                }



                Process_data.Rows.Add(newRow);
                Console.WriteLine($"Processed Row : " + i);


            }
            if (hsm_flag == 0)
            {
                logString.AppendLine("\nFile processing stopped.\n");
                //deletion_errorneous_data(lastInsertedId.ToString());
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
                    //deletion_errorneous_data(lastInsertedId.ToString());
                    logString.Append($"    - Error occurred during bulk copy.\n Column mapping failed" + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
                    Console.WriteLine($"    - Error occurred during bulk copy.\n Column mapping failed" + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
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



       





        
        public string Importlicencefile(int lot)
        {
            //mkimik

            string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlConnection con = new SqlConnection(constr);
            string[] filename_all = licenceFile.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(f => f.Trim()).ToArray();
            //if (Path.GetFileNameWithoutExtension(filename_2) == Path.GetFileNameWithoutExtension())
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
                foreach (string filename_2 in filename_all)
                   
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
                }
                con.Open();

                //string createTableQuery = "CREATE TABLE TempLicence (";
                string createTableQuery = "IF OBJECT_ID('TempLicence', 'U') IS NOT NULL DROP TABLE TempLicence; ";
                createTableQuery += "CREATE TABLE TempLicence (";
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
         SELECT STRING_AGG(ICICID, ',') AS ICCIDs FROM differences";
        //SELECT CASE WHEN COUNT(*) = 0 THEN 'Data matches' ELSE 'Data does not match' END AS status 
        //FROM differences;";
                    using (SqlCommand cmd1 = new SqlCommand(sql, con11))//check if the data matched with licence data
                    {
                        result = cmd1.ExecuteScalar().ToString().TrimEnd();
                    }
                }
                if (!string.IsNullOrEmpty(result))
                {
                    return "Below Iccid are not in license files\n" + result;
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
                return "Error During License File Importaion : " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace;

            }

        }
      
        private void btnGenerateAllFiles_Click()
        {
            int count_btnGenerateAllFiles_Click = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(DataGenProcessHDID) FROM DataGenProcessDataRecord WHERE DataGenProcessHDID = @hdid", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@hdid", OFProcessing.lastInsertedId);
                    count_btnGenerateAllFiles_Click = (int)cmd.ExecuteScalar();
                    
                }
            }
            if (count_btnGenerateAllFiles_Click > 0)
            {
                ProcessAllFile(ProfileID);
            }
            else
            {
                MessageBox.Show(
                    "File not processed. Please process the files first.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        private void ProcessAllFile(int customerProfileID)
        {
            string fileName_ProcessAllFile = string.Empty;
            List<string> fileNames = new List<string>();
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
                                fileNames.Add(reader["FileName"].ToString());
                                //fileName_ProcessAllFile = reader["FileName"].ToString();
                                //int data_status  = generate_AllTypeOutput(fileName_ProcessAllFile);
                                //if (data_status == 0)
                                //{
                                //    throw new InvalidOperationException("Output generation failed.");
                                //}
                            }
                        }
                    }
                }
                // ✅ Now safe to process
                foreach (var fileName in fileNames)
                {
                    fileName_ProcessAllFile = fileName;
                    int data_status = generate_AllTypeOutput(fileName_ProcessAllFile);
                    if (data_status == 0)
                    {
                        throw new InvalidOperationException("Output generation failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                logString.Append("\nSomething went wrong with: " + fileName_ProcessAllFile + "\n" + ex.Message);
                Console.WriteLine($"\nSomething went wrong with: " + fileName_ProcessAllFile + ex.Message);
                Console.WriteLine($"Exception: " + ex.Message);
                Console.WriteLine($"Stack trace: " + ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }

        public int generate_AllTypeOutput(string filetype)
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
                    using (SqlDataAdapter sda = new SqlDataAdapter($"SELECT FilePath,FileNamingConv,FileMasterID,FileExtn,CustProfileFileID FROM CustProfileFile where CustProfileID={ProfileID} AND CustomerID={customerID} AND FileName='{filetype}' and   FileIOID<>'I'", con))
                    {

                        sda.Fill(dt);
                        rootdir = dt.Rows[0][0].ToString().TrimEnd();
                        filenameconv = dt.Rows[0][1].ToString().TrimEnd();
                        filemasterid = dt.Rows[0][2].ToString().TrimEnd();
                        fileext = dt.Rows[0][3].ToString().TrimEnd();
                        CustProfileFileID = dt.Rows[0][4].ToString().TrimEnd();

                        if (customer_name_form.ToUpper() == "RELIANCE")
                        {
                            Outfilelocation = rootdir + $"\\{customer}\\{profile}\\{generic_batch_no}_{FileProcessingLotID}_{lastInsertedId}_{unixTime}";
                        }
                        else {
                            Outfilelocation = rootdir + $"\\{customer}\\{profile}\\{FileProcessingLotID}_{lastInsertedId}_{unixTime}";
                        }
                        
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
                    //DataRow workRow2;
                    //SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con);
                    //adpt2.Fill(dt2);

                    using (SqlCommand cmd = new SqlCommand(Query2, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt2.Load(reader);
                    }

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
                        //DataRow workRow1;
                        //SqlCommand sqlcom1 = new SqlCommand(Query1, con);
                        //SqlDataAdapter adpt1 = new SqlDataAdapter(Query1, con);
                        //adpt1.Fill(dt1);
                        using (SqlCommand cmd = new SqlCommand(Query1, con))
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt1.Load(reader);
                        }


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
                    //DataRow workRow2_footer;
                    //SqlDataAdapter adpt2_footer = new SqlDataAdapter(Query_footer, con);
                    //adpt2_footer.Fill(dt_footer);

                    using (SqlCommand cmd = new SqlCommand(Query_footer, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt_footer.Load(reader);
                    }


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
                            using (SqlCommand cmd = new SqlCommand(Query1, con))
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                dt1.Load(reader);
                            }
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
                    else if (filenameconv.Trim().Contains("FROMFILE_"))
                    {
                        string filename_data = string.Empty;

                        // safer split
                        string[] datafilename_data = string.IsNullOrWhiteSpace(filenameconv)? Array.Empty<string>(): filenameconv.Trim().Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                        // fetch value using ExecuteScalar (faster than reader for single value)
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        using (SqlCommand command = new SqlCommand(@"SELECT TOP 1 t2.FilePath FROM CustProfileFile t1 INNER JOIN DataGenProcessHDFile t2 ON t1.CustProfileFileID = t2.CustProfileFileID WHERE t1.FileIOID = 'I' AND t2.DataGenProcessHDID = @id", connection))
                        {
                            command.Parameters.AddWithValue("@id", lastInsertedId);

                            connection.Open();

                            var result = command.ExecuteScalar();

                            if (result != null)
                            {
                                filename_data = Path.GetFileNameWithoutExtension(result.ToString().Trim());
                            }
                        }

                        // safe index usagevalues.Skip(1)
                        string replacement = datafilename_data.Length > 1 ? string.Join("_", datafilename_data.Skip(1)) : "";

                        myfile = Path.Combine(Outfilelocation,filename_data.Replace("IN_", replacement+"_") + fileext);

                        
                    }
                    else
                    {

                        myfile = Outfilelocation + "\\" + filenameconv + unixTime + $"_{batch}{fileext}";
                    }
                    //String Query3 = $"select Varname , vartype from OutputTemplateLines where ProfileFileID = {filemasterid} and ProfileId={ProfileID} order by FileLineNo";
                    String Query3 = $"select Varname , vartype from OutputTemplateLines where ProfileFileID = {filemasterid} and ProfileId={ProfileID} order by OutputTemplateLinesID";
                    //String Query3 = $"select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = {filemasterid} and FileLineNo = 1 and ProfileId={ProfileID} order by OutPutFileTemplateID,OutputTemplateLinesID";
                    DataTable dt3 = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(Query3, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt3.Load(reader);
                    }
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
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        data.Load(reader);
                    }
                    if (filetype == "XLSX")
                    {
                        string csvfile = myfile.Replace("xlsx", "csv");
                        //using (StreamWriter writer = File.CreateText(csvfile))
                        using (FileStream fs = new FileStream(csvfile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            File.SetAttributes(csvfile, FileAttributes.Hidden); // hide immediately

                            using (StreamWriter writer = new StreamWriter(fs))
                            {
                                if (Header != "")
                                {
                                    //writer.Write(Header.TrimEnd() + "\r\n");
                                    writer.Write(Header + "\r\n");
                                }
                                foreach (DataRow dr in data.Rows)
                                {
                                    writer.Write(dr[0].ToString() + "\r\n");
                                    count++;
                                }
                                if (Footer != "")
                                {
                                    // writer.Write(Footer + "\r\n");
                                    if (!Footer.Equals("NO", StringComparison.OrdinalIgnoreCase))
                                    {
                                        writer.Write(Footer);
                                    }
                                    else
                                    {
                                        writer.Flush();
                                        writer.BaseStream.SetLength(writer.BaseStream.Length - Environment.NewLine.Length);
                                    }
                                }

                            }
                        }
                        myfile = csvfile;
                        //DataTable dataTable = ConvertCsvToDataTable(csvfile);
                        //File.Delete(csvfile);
                        //SaveDataTableToExcel(dataTable, myfile);
                    }
                    else if (filetype == "CPD")
                    {
                        using (FileStream fs = new FileStream(myfile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            File.SetAttributes(myfile, FileAttributes.Hidden); // hide immediately

                            using (StreamWriter writer = new StreamWriter(fs))
                            {

                                int header_count = 0;

                                if (!string.IsNullOrWhiteSpace(Header))
                                {
                                    writer.WriteLine(Header.TrimEnd());

                                    header_count = Header.TrimEnd().Split(',').Length;
                                }
                                int countCCPD = 0;

                                string blockValue11 = "";
                                string blockValue12 = "";
                                foreach (DataRow dr in data.Rows)
                                {
                                    string[] values = dr[0].ToString().Split(';');
                                    // -------------------------------
                                    // FIELD 11 LOGIC (1,501,1001...)
                                    // -------------------------------
                                    if (countCCPD % 500 == 0) // start of block
                                    {
                                        blockValue11 = values.Length > 11 ? values[11] : "";
                                    }
                                    else
                                    {
                                        if (values.Length > 11 && values[11] != blockValue11)
                                        {
                                            values[11] = "FFFFFFFFFFFFFFFFFFFF";
                                            values[16] = "FFFFFFFFFFFFFFFFFFFF";
                                        }
                                    }

                                    // --------------------------------
                                    // FIELD 12 LOGIC (500,1000,1500...)
                                    // --------------------------------
                                    if ((countCCPD + 1) % 500 == 0) // end of block
                                    {
                                        blockValue12 = values.Length > 12 ? values[12] : "";
                                    }
                                    else
                                    {
                                        if (values.Length > 12 && values[12] != blockValue12)
                                        {
                                            values[12] = "FFFFFFFFFFFFFFFFFFFF";
                                        }
                                    }

                                    // Rebuild line
                                    string mergedLine = string.Join(";", values);

                                    writer.WriteLine(mergedLine);

                                    countCCPD++;
                                }

                                if (!string.IsNullOrWhiteSpace(Footer))
                                {
                                    //  writer.WriteLine(Footer);
                                    if (!Footer.Equals("NO", StringComparison.OrdinalIgnoreCase))
                                    {
                                        writer.Write(Footer);
                                    }
                                    else
                                    {
                                        writer.Flush();
                                        writer.BaseStream.SetLength(writer.BaseStream.Length - Environment.NewLine.Length);
                                    }
                                }
                            }




                        }
                    }

                    else if (filetype == "TXT"  && customer_name_form.ToUpper() == "RELIANCE")
                    {
                        using (FileStream fs = new FileStream(myfile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            File.SetAttributes(myfile, FileAttributes.Hidden); // hide immediately

                            using (StreamWriter writer = new StreamWriter(fs))
                            {

                                int header_count = 0;

                                if (!string.IsNullOrWhiteSpace(Header))
                                {
                                    writer.WriteLine(Header.TrimEnd());

                                    header_count = Header.TrimEnd().Split(',').Length;
                                }
                                
                                foreach (DataRow dr in data.Rows)
                                {
                                    string mergedLine = dr[0].ToString();
                                    

                                    writer.WriteLine(mergedLine.Replace("IN_", "CNUM_")+".txt");
                                    writer.WriteLine(mergedLine.Replace("IN_", "SCM_") + ".txt");
                                    writer.WriteLine(mergedLine.Replace("IN_", "SIMODA_") + ".cps");


                                }

                                if (!string.IsNullOrWhiteSpace(Footer))
                                {
                                    //  writer.WriteLine(Footer);
                                    if (!Footer.Equals("NO", StringComparison.OrdinalIgnoreCase))
                                    {
                                        writer.Write(Footer);
                                    }
                                    else
                                    {
                                        writer.Flush();
                                        writer.BaseStream.SetLength(writer.BaseStream.Length - Environment.NewLine.Length);
                                    }
                                }
                            }




                        }
                    }
                    else
                    {
                        //using (StreamWriter writer = File.CreateText(myfile))
                        using (FileStream fs = new FileStream(myfile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            File.SetAttributes(myfile, FileAttributes.Hidden); // hide immediately

                            using (StreamWriter writer = new StreamWriter(fs))
                            {
                                if ((fileext.Equals(".mca", StringComparison.OrdinalIgnoreCase)) && ((batchtypename.Equals("QUARTER", StringComparison.OrdinalIgnoreCase) ||
     batchtypename.Equals("MFF2", StringComparison.OrdinalIgnoreCase))))
                                {
                                    int header_count = 0;

                                    if (!string.IsNullOrWhiteSpace(Header))
                                    {
                                        writer.WriteLine(Header.TrimEnd());

                                        header_count = Header.TrimEnd().Split(',').Length;
                                    }

                                    foreach (DataRow dr in data.Rows)
                                    {
                                        string[] values = dr[0].ToString().Split(',');

                                        // ✅ Check only the last column
                                        bool isCP = values.Last().Trim().Equals("CP", StringComparison.OrdinalIgnoreCase);


                                        // Take first header_count values
                                        var firstPart = values.Take(header_count);

                                        // Append last 5 if CP, else last 4
                                        var lastPart = isCP ? values.Skip(values.Length - 5) : values.Skip(values.Length - 4);

                                        // Combine both parts
                                        string mergedLine = string.Join(",", firstPart.Concat(lastPart));
                                        writer.WriteLine(mergedLine);

                                        count++;
                                    }

                                    if (!string.IsNullOrWhiteSpace(Footer))
                                    {
                                        //  writer.WriteLine(Footer);
                                        if (!Footer.Equals("NO", StringComparison.OrdinalIgnoreCase))
                                        {
                                            writer.Write(Footer);
                                        }
                                        else
                                        {
                                            writer.Flush();
                                            writer.BaseStream.SetLength(writer.BaseStream.Length - Environment.NewLine.Length);
                                        }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(Header))
                                    {
                                        writer.WriteLine(Header);
                                        //writer.WriteLine(Header.TrimEnd());
                                    }

                                    foreach (DataRow dr in data.Rows)
                                    {
                                        writer.WriteLine(dr[0].ToString());
                                        count++;
                                    }

                                    if (!string.IsNullOrWhiteSpace(Footer))
                                    {
                                        // writer.WriteLine(Footer);
                                        if (!Footer.Equals("NO", StringComparison.OrdinalIgnoreCase))
                                        {
                                            writer.Write(Footer);
                                        }
                                        else
                                        {
                                            writer.Flush();
                                            writer.BaseStream.SetLength(writer.BaseStream.Length - Environment.NewLine.Length);
                                        }
                                    }
                                }


                                ////original method
                                //if (Header != "")
                                //{
                                //    writer.Write(Header.TrimEnd() + "\r\n");
                                //}
                                //foreach (DataRow dr in data.Rows)
                                //{
                                //    writer.Write(dr[0].ToString() + "\r\n");
                                //    count++;
                                //}
                                //if (Footer != "")
                                //{
                                //    writer.Write(Footer + "\r\n");
                                //}
                            }
                        }
                    }
                    int mca_batchsize = 0;
                    //if (fileext.ToLower().Trim() == ".mca" && batchsize == 0)
                    if (fileext.ToLower().Trim() == ".mca")
                    {
                        mca_batchsize = batchsize;
                        batchsize = batchsize == 0 ? 2500 : batchsize;



                        string filename_labels = CreateMCABatch(myfile, batchsize, 500, Po_Num, customer_name_form);
                        string[] label_filename_parts = filename_labels.Split(',');
                        //myfile = EncryptionandDecryption.AESEncrypt_File(myfile, file_enc_key);
                        logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[0])}\n");
                        logString.Append($"    - No of record : {count}\n");
                        logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[1])}\n");
                        logString.Append($"    - No of record : {count}\n");
                        logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[2])}\n");
                        logString.Append($"    - No of record : {count}\n");
                        logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[3])}\n");
                        logString.Append($"    - No of record : {count}\n");
                        //UpdateProcessHDFile($"Label", Convert.ToInt32(CustProfileFileID.Trim()), $"{label_filename_parts[0]}",label_filename_parts[4]+"\\"+label_filename_parts[0], lastInsertedId);
                        //UpdateProcessHDFile($"Label", Convert.ToInt32(CustProfileFileID.Trim()), $"{label_filename_parts[1]}",label_filename_parts[4]+"\\"+label_filename_parts[1], lastInsertedId);
                        //UpdateProcessHDFile($"Label", Convert.ToInt32(CustProfileFileID.Trim()), $"{label_filename_parts[2]}",label_filename_parts[4]+"\\"+label_filename_parts[2], lastInsertedId);

                    }
                    //if (fileext.ToLower().Trim() == ".mca" && mca_batchsize > 0)
                    if (fileext.ToLower().Trim() == ".mca" )
                    {
                        //Total_no_of_records += count;
                        Total_no_of_files += 1;
                        string mca_filename = Path.GetFullPath(myfile);
                        string[] lines = File.ReadAllLines(myfile);
                        if (IsSingle && lines.Length > mca_batchsize && mca_batchsize > 0)
                        {
                            if (lines.Length > mca_batchsize) // split only if more than batchsize
                            {

                                int numFiles = (int)Math.Ceiling((double)(lines.Length-1) / mca_batchsize);

                                logString.Append($"    - MCA file starting splitted into {numFiles} parts \n");
                                for (int i = 0; i < numFiles; i++)
                                {
                                    string outputFile = myfile.Replace(fileext, $"_{(i + 1):D4}{fileext}");
                                    //using (StreamWriter writer = File.CreateText(outputFile))
                                    using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
                                    {
                                        File.SetAttributes(outputFile, FileAttributes.Hidden); // hide immediately

                                        using (StreamWriter writer = new StreamWriter(fs))
                                        {
                                            writer.WriteLine(lines[0]);
                                            for (int j = 1; (j <= mca_batchsize && (j + i * mca_batchsize) <= lines.Length - 1); j++)
                                            {
                                                writer.WriteLine(lines[j + i * mca_batchsize]);
                                            }
                                        }
                                    }
                                    outputFile = (profilename == "EUICC") ? outputFile : EncryptionandDecryption.AESEncrypt_File(outputFile, OFProcessing.file_enc_key); ;
                                    if (File.Exists(outputFile))
                                    {
                                        File.SetAttributes(outputFile, FileAttributes.Normal); // make encrypted file visible
                                    }
                                    logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                                    logString.Append($"    - No of record : {mca_batchsize}\n");
                                    
                                }
                                UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(myfile).Replace(".mca","_mca.haes")}", myfile.Replace(".mca", "_mca.haes"), lastInsertedId);
                                File.Delete(myfile);
                            }
                            else
                            {
                                //for renameing the original file with _0001 pading
                                myfile = mca_filename.Replace(fileext, "_0001" + fileext);

                                if (File.Exists(myfile))
                                {
                                    File.Delete(myfile);
                                }

                                File.Move(mca_filename, myfile);
                                mca_filename = Path.GetFullPath(myfile);

                                // No need to split, just encrypt original file
                                string outputFile = (profilename == "EUICC") ? myfile : EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                                if (File.Exists(outputFile))
                                {
                                    File.SetAttributes(outputFile, FileAttributes.Normal); // make encrypted file visible
                                }
                                //string outputFile = EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                                logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                                logString.Append($"    - No of record : {lines.Length - 1}\n");
                                UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(outputFile)}", outputFile, lastInsertedId);
                            }
                        }
                        else if (mca_batchsize == 0)
                        {
                            //for renameing the original file with _0001 pading
                            myfile = mca_filename.Replace(fileext, "_0001" + fileext);

                            if (File.Exists(myfile))
                            {
                                File.Delete(myfile);
                            }

                            File.Move(mca_filename, myfile);
                            mca_filename = Path.GetFullPath(myfile);


                            // No need to split, just encrypt original file
                            string outputFile = (profilename == "EUICC") ? myfile : EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                            if (File.Exists(outputFile))
                            {
                                File.SetAttributes(outputFile, FileAttributes.Normal); // make encrypted file visible
                            }
                            //string outputFile = EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                            logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                            logString.Append($"    - No of record : {lines.Length - 1}\n");
                            UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(outputFile)}", outputFile, lastInsertedId);
                        }

                        else
                        {
                            //for renameing the original file with _0001 pading
                            myfile = mca_filename.Replace(fileext, "_0001" + fileext);

                            if (File.Exists(myfile))
                            {
                                File.Delete(myfile);
                            }

                            File.Move(mca_filename, myfile);
                            mca_filename = Path.GetFullPath(myfile);


                            // No need to split, just encrypt original file
                            string outputFile = (profilename == "EUICC") ? myfile : EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                            if (File.Exists(outputFile))
                            {
                                File.SetAttributes(outputFile, FileAttributes.Normal); // make encrypted file visible
                            }
                            //string outputFile = EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                            logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                            logString.Append($"    - No of record : {lines.Length - 1}\n");
                            UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(outputFile)}", outputFile, lastInsertedId);
                        }
                        //myfile = EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                    }

                    
                    else if (fileext.ToLower().Trim() == ".mca")
                    {

                        string outputFile = (profilename == "EUICC") ? myfile : EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                        if (File.Exists(outputFile))
                        {
                            File.SetAttributes(outputFile, FileAttributes.Normal); // make encrypted file visible
                        }
                        logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                        logString.Append($"    - No of record : {count}\n");
                        
                        UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(outputFile)}", outputFile, lastInsertedId);
                    }
                   

                }
                if (fileext.ToLower().Trim() != ".mca")
                {
                    myfile = EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                    
                    
                    //myfile = EncryptionandDecryption.AESEncrypt_File(myfile, file_enc_key);
                    logString.Append($"    - Filename : {Path.GetFileName(myfile)}\n");
                    Console.WriteLine($"    - Filename : {Path.GetFileName(myfile)}\n");
                    logString.Append($"    - No of record : {count}\n");
                    Console.WriteLine($"    - No of record : {count}\n");
                    logString.Append($"    - Input File FileID : {lastInsertedId}\n");
                    Console.WriteLine($"    - Input File FileID : {lastInsertedId}\n");
                    logString.Append($"    - Customer Profile FileID : {Convert.ToInt32(CustProfileFileID.Trim())}\n");
                    Console.WriteLine($"    - Customer Profile FileID : {Convert.ToInt32(CustProfileFileID.Trim())}\n");
                    UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(myfile)}", myfile, lastInsertedId);
                }

                this.Invoke(new MethodInvoker(delegate
                {
                    txtoutput.Text += $"{filetype} OutFile created successfully for HDID {lastInsertedId}. \r\n";
                }));

                return 1;
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
                deletion_errorneous_data(lastInsertedId.ToString());
                logString.Append($"\nSomething went wrong while creating outfile {filetype} for HDID {lastInsertedId} error message" + ex.Message);
                return 0;
            }
            //GetGenProcessList();
        }
  
        public static string CreateMCABatch(string mcaFile, int outerBatchSize, int innerBatchSize, string poNumber, string customerName)
        {
            DataTable dtOuter = CreateDataTable(outerBatchSize, poNumber);
            DataTable dtInner = CreateDataTable(innerBatchSize, poNumber);
            DataTable dt2000 = CreateDataTable(2000, poNumber);

            int batchCounter = 0;
            int innersrno = 0;
            int outersrno = 0;
            int twoksrno = 0;


            batchCounter++;
            var lines = File.ReadAllLines(mcaFile);
            if (lines.Length == 0) throw new IOException($"MCA file {mcaFile} is empty!");

            int outerIndex = 0;
            int innerIndex = 0;
            int twokIndex = 0;

            int outerBatchIndex = 0;
            int innerBatchIndex = 0;
            int twokBatchIndex = 0;
            int batchQty = 0;
            string mca_header = lines[0];

            string columnsPart = mca_header.Split('#')
                                           .FirstOrDefault(x => x.StartsWith("VN="))
                                           ?.Substring(3) ?? "";

            string[] columns = columnsPart.Split(',');

            // Find indexes
            int imsiIndex = Array.FindIndex(columns, c => c.Contains("IMSI"));
            int iccidIndex ;


            if (customerName.Equals("SKYFI", StringComparison.OrdinalIgnoreCase))
            {
                iccidIndex = columns.Length + 2;
            }
            else
            {
                iccidIndex = Array.FindIndex(columns, c => c.Contains("ICCID"));
            }


            Console.WriteLine("IMSI Index (0-based): " + imsiIndex);
            Console.WriteLine("ICCID Index (0-based): " + iccidIndex);

            int file_qty = lines.Length-1;
            for (int i = 1; i < lines.Length; i++)
            {
               
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                if ((i - 1) % outerBatchSize == 0)
                {
                    outersrno++;
                    outerIndex++;
                    outerBatchIndex++;
                    int endIndex = Math.Min(i + outerBatchSize - 1, lines.Length - 1);
                    int remaining = lines.Length - i;
                    batchQty = Math.Min(outerBatchSize, remaining);
                    AddRowToDataTableCreatemcaBatch(dtOuter, lines[i], lines[endIndex], mcaFile, batchCounter, outersrno, outerBatchIndex, batchQty, imsiIndex, iccidIndex);
                }
                if ((i - 1) % 2000 == 0)
                {
                    innersrno++;
                    twokIndex++;
                    twokBatchIndex++;
                    int endIndex = Math.Min(i + 2000 - 1, lines.Length - 1);
                    int remaining = lines.Length - i;
                    batchQty = Math.Min(2000, remaining);
                    AddRowToDataTableCreatemcaBatch(dt2000, lines[i], lines[endIndex], mcaFile, batchCounter, innersrno, outerBatchIndex, batchQty, imsiIndex, iccidIndex);
                }
                if ((i - 1) % innerBatchSize == 0)
                {
                    twoksrno++;
                    innerIndex++;
                    innerBatchIndex++;
                    int endIndex = Math.Min(i + innerBatchSize - 1, lines.Length - 1);
                    int remaining = lines.Length - i;
                    batchQty = Math.Min(innerBatchSize, remaining);
                    AddRowToDataTableCreatemcaBatch(dtInner, lines[i], lines[endIndex], mcaFile, batchCounter, innersrno, outerBatchIndex, batchQty, imsiIndex, iccidIndex);
                }
                
            }

            //string outter_label_file_name = SaveDataTableToExcel(dtOuter, mcaFile, poNumber, "Outer", outerBatchSize);
            //string outter_label_file_name_1 = SaveDataTableToExcel(dtOuter, mcaFile, poNumber, "2000", 2000);
            //string inner_label_file_name = SaveDataTableToExcel(dtInner, mcaFile, poNumber, "Inner", innerBatchSize);

            string outter_label_file_name = SaveDataTableToCsv(dtOuter, mcaFile, poNumber, "Outer", outerBatchSize, file_qty);

            string inner_label_file_name = SaveDataTableToCsv(dtInner, mcaFile, poNumber, "Inner", innerBatchSize, file_qty);

            string outter_label_file_name_1 = SaveDataTableToCsv(dt2000, mcaFile, poNumber, "2000", 2000, file_qty);

            string batch_list = SaveDataTableToCsv(dtOuter, mcaFile, poNumber, "Batch_List", outerBatchSize, file_qty);

            merged_outer_label_file_names.Add(outter_label_file_name );
            merged_inner_label_file_names.Add(inner_label_file_name);
            merged_outer_label_file_names_1.Add(outter_label_file_name_1);
            merged_batch_list.Add(batch_list);
            //merged_label_directory = Path.GetDirectoryName(outter_label_file_name);

            string myfile1 = outter_label_file_name + ",";
            myfile1 += inner_label_file_name + ",";
            myfile1 += outter_label_file_name_1 + ",";
            myfile1 += batch_list+ ",";
            myfile1 += Path.GetDirectoryName(outter_label_file_name);

            //string myfile1 = EncryptionandDecryption.AESEncrypt_File(outter_label_file_name, OFProcessing.file_enc_key) + ",";
            //myfile1 += EncryptionandDecryption.AESEncrypt_File(inner_label_file_name, OFProcessing.file_enc_key) + ",";
            //myfile1 += EncryptionandDecryption.AESEncrypt_File(outter_label_file_name_1, OFProcessing.file_enc_key) + ",";
            //myfile1 += EncryptionandDecryption.AESEncrypt_File(batch_list, OFProcessing.file_enc_key) + ",";
            //myfile1 += Path.GetDirectoryName(outter_label_file_name);
            return ($"{myfile1}");

        }

        public static void MergeCsvsFromFolders(string[][] allFileSets)
        {
            if (allFileSets == null || allFileSets.Length == 0)
                throw new ArgumentException("No file sets provided.");

            // Take the first folder from the first file path
            string firstFolder = Path.GetDirectoryName(allFileSets[0][0]);

            // Merge each type of file (Outer, Inner, 2000, Batch_List)
            MergeCsvType(allFileSets, "Outer", firstFolder);
            MergeCsvType(allFileSets, "Inner", firstFolder);
            MergeCsvType(allFileSets, "2000_Label", firstFolder);
            MergeCsvType(allFileSets, "Batch List", firstFolder);
        }

        private static void MergeCsvType(string[][] allFileSets, string typeKeyword, string outputFolder)
        {
            List<string> matchingFiles = new List<string>();

            foreach (var fileSet in allFileSets)
            {
                string match = fileSet.FirstOrDefault(f =>
                    f.IndexOf(typeKeyword, StringComparison.OrdinalIgnoreCase) >= 0);

                if (!string.IsNullOrEmpty(match) && File.Exists(match))
                    matchingFiles.Add(match);
            }

            if (matchingFiles.Count == 0) return;

            string firstFile = matchingFiles[0];
            string mergedFileName = "Merged_"+Path.GetFileNameWithoutExtension(firstFile) + ".csv";
            string mergedFilePath = Path.Combine(outputFolder, mergedFileName);

            using (var writer = new StreamWriter(mergedFilePath, false))
            {
                bool isFirstFile = true;
                int srNoCounter = 1; // to keep SrNo in continuation



                foreach (var file in matchingFiles)
                {
                    string[] lines = File.ReadAllLines(file);
                    if (lines.Length == 0) continue;

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];

                        // For header line, write only once
                        if (isFirstFile && i == 0)
                        {
                            writer.WriteLine(line);
                            continue;
                        }
                        else if (i == 0)
                        {
                            continue; // skip header for subsequent files
                        }

                        // Replace SrNo (first column) with continuous number
                        string[] columns = line.Split(',');
                        columns[0] = srNoCounter.ToString().PadLeft(5, '0');
                        srNoCounter++;

                        writer.WriteLine(string.Join(",", columns));
                    }

                    isFirstFile = false;
                }



                //..merged file with origianl sr nor from all file not in continuation
                //foreach (var file in matchingFiles)
                //{
                //    string[] lines = File.ReadAllLines(file);
                //    if (lines.Length == 0) continue;

                //    if (isFirstFile)
                //    {
                //        foreach (var line in lines)
                //            writer.WriteLine(line);
                //        isFirstFile = false;
                //    }
                //    else
                //    {
                //        for (int i = 1; i < lines.Length; i++)
                //            writer.WriteLine(lines[i]);
                //    }
                //}
            }

            Console.WriteLine($"Merged {typeKeyword} files saved at: {mergedFilePath}");
        }


      

        private static string SaveDataTableToCsv(DataTable dt, string mcaFilePath, string poNumber, string labelType, int batchSize, int fileqty)
        {
            string baseDir = Path.GetDirectoryName(mcaFilePath);
            baseDir = baseDir.Replace("\\Productions\\", "\\Data_Gen\\Label\\");
            string csvFilePath = "";
            string filePrefix = Path.GetFileNameWithoutExtension(mcaFilePath);
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);  // creates MyFolder, TEST1, TEST2, TEST3 if missing
            }

            if (labelType == "Batch_List")
            {
                csvFilePath = Path.Combine(baseDir,$"{customer}_Batch List_PO_{poNumber}_{label_circle_data}_{Total_no_of_records}.csv");
            }
            else
            {
                csvFilePath = Path.Combine(baseDir,$"{filePrefix}_{labelType}_Label_PO_{poNumber}_{batchSize}_{fileqty}.csv");
            }
            using (var writer = new StreamWriter(csvFilePath, false, Encoding.UTF8))
            {
                // Write header
                var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName);
                writer.WriteLine(string.Join(",", columnNames));

                // Write rows
                foreach (DataRow row in dt.Rows)
                {
                    var fields = row.ItemArray.Select(field =>
                    {
                        string value = field?.ToString() ?? "";
                        // Escape quotes and commas for CSV compliance
                        if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
                        {
                            value = "\"" + value.Replace("\"", "\"\"") + "\"";
                        }
                        return value;
                    });
                    writer.WriteLine(string.Join(",", fields));
                }
            }

            return csvFilePath;
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
                MessageBox.Show($"Something went wrong while getting filename: " + ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            return filename_12;
        }


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

       

        public static void SaveDataTableToExcel(DataTable dt, string excelFilePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");

                // Add headers
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    worksheet.Cell(1, col + 1).Value = dt.Columns[col].ColumnName;
                }

                // Add rows
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        worksheet.Cell(row + 2, col + 1).SetValue(dt.Rows[row][col]?.ToString() ?? "");
                    }
                }

                worksheet.Columns().AdjustToContents(); // Auto-fit columns
                workbook.SaveAs(excelFilePath);
            }
        }


        
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

                        //MessageBox.Show($"OutFile created successfully",
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
                //MessageBox.Show($"Something went wrong while creating profile: " + ex.Message,
                //                        "Error",
                //                        MessageBoxButtons.OK,
                //                        MessageBoxIcon.Error
                //                        );
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

                        //MessageBox.Show($"OutFile created successfully",
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
                //MessageBox.Show($"Something went wrong while creating profile: " + ex.Message,
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
            //logString.Append($"\n**************************************[Logging Out] Data Processing Tool is closing [{DateTime.Now}] **************************************\n");
            //Console.WriteLine($"\n**************************************[Logging Out] Data Processing Tool is closing [{DateTime.Now}] **************************************\n");
            //System.IO.File.AppendAllText(log_dir + "/Logging/" + $"{DateTime.Now.ToString("dd-MM-yyyy")}_log.txt", logString.ToString());
            //upload_log();
            logString.Clear();
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //tracker.Clear();
            //MessageBox.Show($"Tool will proceeed  without IMSI duplicity \nAre you ok to proceed",
            //                                                   "Message",
            //                                                   MessageBoxButtons.OK,
            //                                                   MessageBoxIcon.Information
            //                                                   );
            custId = Convert.ToInt32(cbxCustomer.SelectedValue);
            profileId = Convert.ToInt32(cbxProfile.SelectedValue);

            string input_filepath = excel_selector();
            //string response_data = euicc_Data_function();
            CleanupDatabase();
            customer_name_form = cbxCustomer.Text;

            if (string.IsNullOrWhiteSpace(input_filepath))
                return;

            if (!ValidateUserSelection())
                return;


            if (cbxCustomer.SelectedIndex > 0 && cbxCircle.SelectedIndex > 0 && cbxProfile.SelectedIndex > 0)
            {
                txtInputfile.Text = "";
                InsertedHDIDS.Clear();
                logString.Append($"\n1. User initiated the data processing tool and selected the following input:-\n    Customer : {cbxCustomer.Text}\n    Circle : {cbxCircle.Text}\n    Profile : {cbxProfile.Text}\n");
                Console.WriteLine($"\n1. User initiated the data processing tool and selected the following input:-\n    Customer : {cbxCustomer.Text}\n    Circle : {cbxCircle.Text}\n    Profile : {cbxProfile.Text}\n");
                string filepath = string.Empty;
               
                string[] fileNames = Directory.GetFiles(Path.GetDirectoryName(input_filepath))
    .Where(f => !Path.GetFileName(f).Equals(Path.GetFileName(input_filepath), StringComparison.OrdinalIgnoreCase))
    .ToArray();
                logString.Append($"\n2. Selected Directory Path : [{Path.GetDirectoryName(fileNames[0])}]\n");
                Console.WriteLine($"\n2. Selected Directory Path : [{Path.GetDirectoryName(fileNames[0])}]\n");
                logString.Append($"\n3. {fileNames.Length} file selected.\n");
                Console.WriteLine($"\n3. {fileNames.Length} file selected.\n");
                //hsm_IP =  (profilename == "EUICC") ?  euicc_data_IP:hsm_IP;
                //if (Path.GetDirectoryName(fileNames[0]).ToString().ToLower().Contains(cbxCustomer.Text.ToLower())   && Path.GetDirectoryName(fileNames[0]).ToString().ToLower().Contains(cbxProfile.Text.ToLower())) 
                if (Path.GetDirectoryName(fileNames[0]).ToString().ToLower().Contains(cbxCustomer.Text.ToLower()))
                {

                    total_pro_file = fileNames.Length;
                    logString.Append($"\n4. Initiating duplicate check process for selected files in the folder.\n");
                    Console.WriteLine($"\n4. Initiating duplicate check process for selected files in the folder.\n");
                    char ch = 'a';




                    foreach (string fileName in fileNames)
                    {
                        logString.Append($"\n    {ch}. File: [{Path.GetFileName(fileName)}]\n");
                        Console.WriteLine($"\n    {ch}. File: [{Path.GetFileName(fileName)}]\n");
                        if ((!IsDuplicateFile(fileName)))
                        {

                            logString.Append($"       - No duplicate filename found.\n");
                            string count = iccid_dupcheck_new_faster(fileName);
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
                                MessageBox.Show($"{count}.\nFirst Duplicate {dupcheck_variable} '{Dup_First_icicid}",
                                                            "Message",
                                                            MessageBoxButtons.OK,
                                                            MessageBoxIcon.Information
                                                            );
                                logString.Append($"       - {count}.First Duplicate {dupcheck_variable} '{Dup_First_icicid}'\n");

                                txtoutput.Text += $"{fileName} duplicate records found: \r\n";
                                total_dup_file++;

                            }



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
                        MessageBox.Show($"No file to proceed.");
                        txtoutput.Text += "No file to proceed.\r\n";
                        upload_log();
                    }
                    else
                    {
                        string[] files = txtInputfile.Text.Split(',');

                        var fileNamesfordg = files
                            .Select(f => Path.GetFileName(f.Trim()))
                            .Where(f => !string.IsNullOrEmpty(f));

                        string resultfordg = string.Join("\n", fileNamesfordg);

                        MessageBox.Show(resultfordg + "\nInput Parsing Completed.");
                        txtoutput.Text += resultfordg + "\nInput Parsing Completed.\r\n";
                        //txtoutput.Text += txtInputfile.Text.Replace(',', '\n') + "\n Input Parsing Completed.\r\n";
                        ////MessageBox.Show(txtInputfile.Text + " are ok to proceed.");
                        //txtoutput.Text += txtInputfile.Text.Replace(',', '\n') + "\nOK To Proceed.\r\n";
                    }

                }
                else
                {
                    MessageBox.Show($"Wrong input file selected ",
                                                "Error",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information
                                                );
                    logString.Append("\nWrong input file selected \n");
                    Console.WriteLine($"\nWrong input file selected \n");
                    return;
                }
                using (SqlConnection con11 = new SqlConnection(connectionString))
                {
                    con11.Open();
                    using (SqlCommand cmd1 = new SqlCommand($"SELECT COUNT(1) FROM [License_InPutTemplate] WHERE [CustID]=@cust and [ProfileID]=@profile;", con11))//check if the data matched with licence data
                    {
                        cmd1.Parameters.AddWithValue("@cust", custId);
                        cmd1.Parameters.AddWithValue("@profile", profileId);
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
                MessageBox.Show($"All fields are required: ",
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information
                                            );
                logString.Append("\nAll fields are required: \n");
                Console.WriteLine($"\nAll fields are required: \n");
                return;
            }


        }

     
        public string iccid_dupcheck_bulk(string filename)
        {
            try
            {
                string[] lines = File.ReadAllLines(filename);

                string[] strs;
                if (customer_name_form.Equals("AFTEL", StringComparison.OrdinalIgnoreCase))
                    strs = new string[] { "QUANTITY", "ICCID", "IMSI" };
                else
                    strs = new string[] { "QUANTITY", "ICCID", "IMSI", "MSISDN" };

                int file_qty = 0;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    foreach (string str in strs)
                    {
                        string varname = "", tag = "";
                        int pos_from = 0, len = 0, line = 0;

                        // ✅ STEP 1: GET POSITION FROM DB (YOUR LOGIC)
                        using (SqlCommand cmd = new SqlCommand(@"
                    SELECT [VarName], [PositionFrom], [Len], [LineNumber], [Tag]
                    FROM [InPutDataTemplate]
                    WHERE CustID = @CustID 
                      AND ProfileID = @ProfileID
                      AND VarDes = @VarDes
                      AND vartext = 'FL'", con))
                        {
                            cmd.Parameters.AddWithValue("@CustID", custId);
                            cmd.Parameters.AddWithValue("@ProfileID", profileId);
                            cmd.Parameters.AddWithValue("@VarDes", str);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    varname = reader.GetString(0);
                                    pos_from = reader.GetInt32(1);
                                    len = reader.GetInt32(2);
                                    line = reader.GetInt32(3);
                                    tag = reader.IsDBNull(4) ? "" : reader.GetString(4);
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(varname))
                            continue;

                        // ✅ STEP 2: GET QUANTITY
                        if (str == "QUANTITY")
                        {
                            string lineText = lines[line - 1];
                            file_qty = int.Parse(lineText.Substring(pos_from, len).Trim());
                            continue;
                        }

                        // ✅ STEP 3: EXTRACT VALUES BASED ON CONFIG
                        HashSet<string> datalist = new HashSet<string>();

                        if (!string.IsNullOrEmpty(tag) && tag.Equals("incremental", StringComparison.OrdinalIgnoreCase))
                        {
                            string lineText = lines[line - 1];
                            long value = long.Parse(lineText.Substring(pos_from, len));

                            for (int i = 0; i < file_qty; i++)
                            {
                                datalist.Add(StringToHex(value.ToString()));
                                value++;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < file_qty; i++)
                            {
                                string lineText = lines[line + i - 1];

                                if (lineText.Length < pos_from)
                                    continue;

                                string value = (lineText.Length >= pos_from + len)
                                    ? lineText.Substring(pos_from, len).Trim()
                                    : lineText.Substring(pos_from).Trim();

                                if (!string.IsNullOrEmpty(value))
                                    datalist.Add(StringToHex(value));
                            }
                        }

                        if (datalist.Count == 0)
                            continue;

                        // ✅ STEP 4: BULK COPY
                        DataTable table = new DataTable();
                        table.Columns.Add("Value", typeof(string));

                        foreach (var val in datalist)
                            table.Rows.Add(val);

                        using (SqlCommand cmd = new SqlCommand(
                            "CREATE TABLE #TempList (Value NVARCHAR(50) PRIMARY KEY);", con))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        using (SqlBulkCopy bulk = new SqlBulkCopy(con))
                        {
                            bulk.DestinationTableName = "#TempList";
                            bulk.BatchSize = 50000;
                            bulk.BulkCopyTimeout = 600;

                            bulk.WriteToServer(table);
                        }

                        // ✅ STEP 5: DUPLICATE CHECK
                        string query = $@"
                    SELECT TOP 1
                        COUNT_BIG(*) AS RecordCount,
                        MIN(t.Value) AS FirstDup
                    FROM DupCheck d
                    INNER JOIN #TempList t ON d.{str} = t.Value";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandTimeout = 600;

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    long count = reader.GetInt64(0);

                                    if (count > 0)
                                    {
                                        string firstDup = reader.IsDBNull(1) ? "" : reader.GetString(1);

                                        return $"Duplicate {str} Found\nCount: {count}\nFirst: {HexToString(firstDup)}";
                                    }
                                }
                            }
                        }

                        // cleanup temp table
                        using (SqlCommand cmd = new SqlCommand("DROP TABLE #TempList", con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }

            return "";
        }
        private bool ValidateUserSelection()
        {
            if (cbxCustomer.SelectedIndex <= 0 ||
                cbxCircle.SelectedIndex <= 0 ||
                cbxProfile.SelectedIndex <= 0)
            {
                MessageBox.Show($"All fields are required");
                logString.Append($"All fields are required");
                return false;
            }

            logString.Append($"1. User Input:\n Customer: {cbxCustomer.Text}\n Circle: {cbxCircle.Text}\n Profile: {cbxProfile.Text}");
            return true;
        }

        private void CleanupDatabase()
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("sp_DeleteJunkData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Optional: Increase timeout if large delete
                    cmd.CommandTimeout = 0; // no timeout

                    cmd.ExecuteNonQuery();
                }
            }

            //string query_remove_junk_data = @"
            //    DELETE FROM [DataGenProcessData] 
            //    WHERE DataGenProcessHDID IN (SELECT [DataGenProcessHDID] FROM [dbo].[DataGenProcessHDFile] WHERE [FileName] LIKE '%mca.haes%' AND [OutFlileStatus] NOT IN (6, 17));

            //    DELETE FROM [dbo].[DataGenProcessDataRecord] 
            //    WHERE DataGenProcessHDID IN (SELECT [DataGenProcessHDID] FROM [dbo].[DataGenProcessHDFile] WHERE [FileName] LIKE '%mca.haes%' AND [OutFlileStatus] NOT IN (6, 17));

            //    DELETE FROM [dbo].[DupCheck] 
            //    WHERE C1 IN (SELECT [DataGenProcessHDID] FROM [dbo].[DataGenProcessHDFile] WHERE [FileName] LIKE '%mca.haes%' AND [OutFlileStatus] NOT IN (6, 17));

            //    DELETE FROM [dbo].[DataGenProcessHD] 
            //    WHERE DataGenProcessHDID IN (SELECT [DataGenProcessHDID] FROM [dbo].[DataGenProcessHDFile] WHERE [FileName] LIKE '%mca.haes%' AND [OutFlileStatus] NOT IN (6, 17));

            //    DELETE FROM [dbo].[DataGenProcessHDFile] 
            //    WHERE DataGenProcessHDID IN (SELECT [DataGenProcessHDID] FROM [dbo].[DataGenProcessHDFile] WHERE [FileName] LIKE '%mca.haes%' AND [OutFlileStatus] NOT IN (6, 17));






            //    DELETE FROM [DataGenProcessData] 
            //    WHERE DataGenProcessHDID IN (select [DataGenProcessHDID] from [dbo].[DataGenProcessHD]  where (DataGenProcessStatus = 5 or isnull(DataGenProcessStatus,'')=''));

            //    DELETE FROM [dbo].[DataGenProcessDataRecord] 
            //    WHERE DataGenProcessHDID IN (select [DataGenProcessHDID] from [dbo].[DataGenProcessHD]  where (DataGenProcessStatus = 5 or isnull(DataGenProcessStatus,'')=''));

            //    DELETE FROM [dbo].[DataGenProcessHDFile] 
            //    WHERE DataGenProcessHDID IN (select [DataGenProcessHDID] from [dbo].[DataGenProcessHD]  where (DataGenProcessStatus = 5 or isnull(DataGenProcessStatus,'')=''));

            //    DELETE FROM [dbo].[DupCheck] 
            //    WHERE C1 IN (select [DataGenProcessHDID] from [dbo].[DataGenProcessHD]  where (DataGenProcessStatus = 5 or isnull(DataGenProcessStatus,'')=''));


            //    DELETE FROM [dbo].[DataGenProcessHD] 
            //    WHERE DataGenProcessHDID IN (select [DataGenProcessHDID] from [dbo].[DataGenProcessHD]  where (DataGenProcessStatus = 5 or isnull(DataGenProcessStatus,'')=''));

            //    Truncate table [dbo].[DGPDR_Base]

            //";
            //using (SqlConnection con11test_1 = new SqlConnection(connectionString))
            //{
            //    SqlCommand command = new SqlCommand(query_remove_junk_data, con11test_1);

            //    try
            //    {
            //        con11test_1.Open();

            //        // Begin a transaction to execute the deletions as a single unit of work
            //        SqlTransaction transaction = con11test_1.BeginTransaction();
            //        command.Transaction = transaction;

            //        // Execute the query
            //        int rowsAffected = command.ExecuteNonQuery();
            //        transaction.Commit();

            //        Console.WriteLine($"Data deleted successfully. Rows affected: " + rowsAffected);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error: " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
            //    }
            //}



        }


        public string excel_selector()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xlsx";

            if (ofd.ShowDialog() != DialogResult.OK)
                return "";

            FileInfo file = new FileInfo(ofd.FileName);

            // EPPlus 8+ License Fix
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets[0];

                Dictionary<string, int> col = new Dictionary<string, int>();
                for (int c = 1; c <= ws.Dimension.End.Column; c++)
                {
                    col[ws.Cells[1, c].Text.Trim()] = c;
                }

                int r = 2;

                cbxCustomer.Text = ws.Cells[r, col["Customer_Name"]].Text;
                cbxCircle.Text = ws.Cells[r, col["Circle_DB"]].Text;
                cbxProfile.Text = ws.Cells[r, col["Profile_Name"]].Text;
                comboBox1.Text = ws.Cells[r, col["Card_Format"]].Text;
                Circle_Label.Text = ws.Cells[r, col["Circle_FILE"]].Text;
                tb_ponum.Text = ws.Cells[r, col["PO_Number"]].Text;
            }
            return Path.GetFullPath(ofd.FileName);
        }

        //public static void CreateMCABatch(string[] mcaFilePaths, int outerBatchSize, int innerBatchSize, string poNumber)
        //{
        //    DataTable dtOuter = CreateDataTable(outerBatchSize, poNumber);
        //    DataTable dtInner = CreateDataTable(innerBatchSize, poNumber);

        //    int batchCounter = 0;
        //    int innersrno = 0;
        //    int outersrno = 0;

        //    foreach (var mcaFile in mcaFilePaths)
        //    {
        //        batchCounter++;
        //        var lines = File.ReadAllLines(mcaFile);
        //        if (lines.Length == 0) throw new IOException($"MCA file {mcaFile} is empty!");

        //        int outerIndex = 0;
        //        int innerIndex = 0;
        //        int outerBatchIndex = 0;
        //        int innerBatchIndex = 0;

        //        for (int i = 1; i < lines.Length; i++)
        //        {
        //            if (string.IsNullOrWhiteSpace(lines[i])) continue;
        //            if ((i - 1) % outerBatchSize == 0)
        //            {
        //                outersrno++;
        //                outerIndex++;
        //                outerBatchIndex++;
        //                int endIndex = Math.Min(i + outerBatchSize - 1, lines.Length - 1);
        //                AddRowToDataTable(dtOuter, lines[i], lines[endIndex], mcaFile, batchCounter, outersrno, outerBatchIndex);
        //            }
        //            if ((i - 1) % innerBatchSize == 0)
        //            {
        //                innersrno++;
        //                innerIndex++;
        //                innerBatchIndex++;
        //                int endIndex = Math.Min(i + innerBatchSize - 1, lines.Length - 1);
        //                AddRowToDataTable(dtInner, lines[i], lines[endIndex], mcaFile, batchCounter, innersrno, outerBatchIndex);
        //            }
        //        }
        //    }
        //    SaveDataTableToExcel(dtOuter, mcaFilePaths[0], poNumber, "Outer", outerBatchSize);
        //    SaveDataTableToExcel(dtInner, mcaFilePaths[0], poNumber, "Inner", innerBatchSize);
        //}

        private static DataTable CreateDataTable(int batchSize, string poNumber)
        {

            batchnumber_file = Database.sql_data_value("select trim(VarValue) as VarValue from DataGenProcessData where DataGenProcessHDID = '" + lastInsertedId + "' and varname	 = 'Batchnumber' ", "VarValue");

            DataTable dt = new DataTable();
            dt.Columns.Add("Serial_no", typeof(string));
            dt.Columns.Add("Input_File_Name", typeof(string));
            dt.Columns.Add("Start_ICCID", typeof(string));
            dt.Columns.Add("End_ICCID", typeof(string));
            dt.Columns.Add("Start_IMSI", typeof(string));
            dt.Columns.Add("End_IMSI", typeof(string));
            //dt.Columns.Add("Quantity", typeof(int)).DefaultValue = batchSize;
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("Circle", typeof(string)).DefaultValue = label_circle_data;
            dt.Columns.Add("PO_Number", typeof(string)).DefaultValue = poNumber;
            dt.Columns.Add("SKU", typeof(string)).DefaultValue = "";
            dt.Columns.Add("BatchNumber", typeof(string)).DefaultValue = batchnumber_file;
            return dt;
        }

       


        private static void AddRowToDataTableCreatemcaBatch(DataTable dt, string startLine, string endLine, string filePath, int batchNumber, int index, int batchIndex,int Quantity, int imsiIndex, int iccidIndex)
        {
            var startArray = startLine.Split(',');
            var endArray = endLine.Split(',');
            DataRow row = dt.NewRow();
            row["Serial_no"] = index.ToString().PadLeft(5, '0');
            row["Input_File_Name"] = Path.GetFileNameWithoutExtension(filePath) + $"_{Int32.Parse(batchIndex.ToString()):D4}";  //"_" + batchIndex.ToString().PadLeft(3, '0');
            //row["BatchNumber"] = batchNumber.ToString().PadLeft(4, '0');
            row["Start_ICCID"] = NibbleSwap_F_Replace(startArray[iccidIndex]);
            //Console.WriteLine(row);
            row["End_ICCID"] = NibbleSwap_F_Replace(endArray[iccidIndex]);
            //Console.WriteLine(NibbleSwap_F_Replace(endArray[1]).Substring(0, NibbleSwap_F_Replace(endArray[1]).Length - 1));
            row["Start_IMSI"] = NibbleSwap_F_Replace(startArray[imsiIndex]).Substring(3, startArray[imsiIndex].Length - 3);
            //Console.WriteLine(NibbleSwap_F_Replace(startArray[0]).Substring(3));
            row["End_IMSI"] = NibbleSwap_F_Replace(endArray[imsiIndex]).Substring(3, endArray[imsiIndex].Length - 3);
            row["Quantity"] = Quantity;
            //row["BatchNumber"] = batchIndex.ToString().PadLeft(4, '0');
            //Console.WriteLine(NibbleSwap_F_Replace(endArray[0]).Substring(3));
            dt.Rows.Add(row);
        }
        static string NibbleSwap_F_Replace(string hex)
        {
            if (!customer_name_form.Equals("SKYFI", StringComparison.OrdinalIgnoreCase))
            {
                if (hex.Length % 2 != 0) throw new ArgumentException("Hex string length must be even.");
                StringBuilder swapped = new StringBuilder(hex.Length);
                for (int i = 0; i < hex.Length; i += 2)
                {
                    swapped.Append(hex[i + 1]);
                    swapped.Append(hex[i]);
                }
                return swapped.ToString().Replace("F", "");
            }
            else
            {
                return hex;
            }
            
        }


        //private static void SaveDataTableToExcel(DataTable dt, string mcaFilePath, string poNumber, string labelType, int batchSize)
        //{
        //    string baseDir = Path.GetDirectoryName(mcaFilePath);
        //    string filePrefix = Path.GetFileNameWithoutExtension(mcaFilePath).Split('_')[0];
        //    string filePostfix = Path.GetFileNameWithoutExtension(mcaFilePath).Split('_')[1];

        //    using (var workbook = new XLWorkbook())
        //    {
        //        var ws = workbook.Worksheets.Add("Data");
        //        var headerStyle = workbook.Style;
        //        headerStyle.Font.Bold = true;
        //        headerStyle.Fill.BackgroundColor = XLColor.LightGray;
        //        headerStyle.Border.OutsideBorder = XLBorderStyleValues.Thin;
        //        headerStyle.Border.InsideBorder = XLBorderStyleValues.Thin;
        //        for (int c = 0; c < dt.Columns.Count; c++)
        //        {
        //            var cell = ws.Cell(1, c + 1);
        //            cell.Value = dt.Columns[c].ColumnName;
        //            cell.Style = headerStyle;
        //        }
        //        for (int r = 0; r < dt.Rows.Count; r++)
        //        {
        //            for (int c = 0; c < dt.Columns.Count; c++)
        //            {
        //                var cell = ws.Cell(r + 2, c + 1);
        //                cell.Value = dt.Rows[r][c].ToString();
        //                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        //                cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        //            }
        //        }
        //        ws.Columns().AdjustToContents();
        //        string excelFilePath = Path.Combine(baseDir,
        //            $"{filePrefix}_{labelType}_Label_PO_{poNumber}_{batchSize}_{filePostfix}.xlsx");

        //        workbook.SaveAs(excelFilePath);
        //    }
        //}

        private void cbxProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxProfile.SelectedIndex > 0)
            {
                var batch = CommonClass.GetBatchList();

                if (batch != null && batch.Count > 0)
                {
                    batch.Insert(0, new BatchTypes
                    {
                        BatchSize = 0,
                        BatchType = "----Select----"
                    });
                    comboBox1.DataSource = batch;
                    comboBox1.DisplayMember = "BatchType";
                    comboBox1.ValueMember = "BatchSize";
                    ProfileID = Convert.ToInt32(cbxProfile.SelectedValue);
                    product_type_customer_profile = Database.sql_data_value("  select product_type from custprofile where ProfileID  = " + ProfileID + " ", "product_type");
                    if (product_type_customer_profile == "DUMMY")
                    {
                        txtInputfile.Text = "Selected profile has no Input file";
                        btnInputFile.Enabled = false;


                        txtLicence.Text = "Selected profile has no Licence file";
                        btnLicence.Enabled = false;
                    }


                }
                else
                {
                    comboBox1.DataSource = null;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
        public static void deletion_errorneous_data(string processHdId)
        {

            List<string> files = Database.FetchFilespathbyhdid(Convert.ToInt32(processHdId));

            //List<string> files = database.FetchFilespathbyhdid(Convert.ToInt32(processHdId));
            string nonexistfiles = "";
            string labelfolder = "";
            

            

            foreach (string file in files)
            {
                if (file.Contains("mca"))
                {
                    labelfolder = file.Replace("\\Productions\\", "\\Data_Gen\\Label\\");
                }

                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }

            var folders = files.Select(f => Path.GetDirectoryName(f)).Distinct().ToList();
            // Add labelfolder if it's not already in the list
            if (!folders.Contains(labelfolder))
            {
                folders.Add(Path.GetDirectoryName(labelfolder));
            }

            foreach (string folder in folders)
            {
                try
                {
                    if (Directory.Exists(folder))
                    {
                        Directory.Delete(folder, true); // deletes folder and all files/subfolders
                    }
                    ////if (Directory.Exists(folder) && !Directory.EnumerateFileSystemEntries(folder).Any())
                    ////{
                    ////	Directory.Delete(folder);
                    ////}
                }
                catch (Exception ex)
                {
                    // Optional: log error or show message if a folder couldn't be deleted
                    Console.WriteLine($"Failed to delete folder '{folder}': {ex.Message}");
                }
            }

            try
            {
                Database.DeleteDBFile(Convert.ToInt32(processHdId));
                //MessageBox.Show($"All Files deleted successfully.");

                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete the error files : " + ex.Message);
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

        public int generate_AllTypeOutput_original(string filetype)
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
                    using (SqlDataAdapter sda = new SqlDataAdapter($"SELECT FilePath,FileNamingConv,FileMasterID,FileExtn,CustProfileFileID FROM CustProfileFile where CustProfileID={ProfileID} AND CustomerID={customerID} AND FileName='{filetype}' and   FileIOID<>'I'", con))
                    {

                        sda.Fill(dt);
                        rootdir = dt.Rows[0][0].ToString().TrimEnd();
                        filenameconv = dt.Rows[0][1].ToString().TrimEnd();
                        filemasterid = dt.Rows[0][2].ToString().TrimEnd();
                        fileext = dt.Rows[0][3].ToString().TrimEnd();
                        CustProfileFileID = dt.Rows[0][4].ToString().TrimEnd();

                        if (customer_name_form.ToUpper() == "RELIANCE")
                        {
                            Outfilelocation = rootdir + $"\\{customer}\\{profile}\\{generic_batch_no}_{FileProcessingLotID}_{lastInsertedId}_{unixTime}";
                        }
                        else
                        {
                            Outfilelocation = rootdir + $"\\{customer}\\{profile}\\{FileProcessingLotID}_{lastInsertedId}_{unixTime}";
                        }

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
                    else if (filenameconv.Trim().Contains("FROMFILE_"))
                    {
                        string filename_data = string.Empty;

                        // safer split
                        string[] datafilename_data = string.IsNullOrWhiteSpace(filenameconv) ? Array.Empty<string>() : filenameconv.Trim().Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                        // fetch value using ExecuteScalar (faster than reader for single value)
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        using (SqlCommand command = new SqlCommand(@"SELECT TOP 1 t2.FilePath FROM CustProfileFile t1 INNER JOIN DataGenProcessHDFile t2 ON t1.CustProfileFileID = t2.CustProfileFileID WHERE t1.FileIOID = 'I' AND t2.DataGenProcessHDID = @id", connection))
                        {
                            command.Parameters.AddWithValue("@id", lastInsertedId);

                            connection.Open();

                            var result = command.ExecuteScalar();

                            if (result != null)
                            {
                                filename_data = Path.GetFileNameWithoutExtension(result.ToString().Trim());
                            }
                        }

                        // safe index usagevalues.Skip(1)
                        string replacement = datafilename_data.Length > 1 ? string.Join("_", datafilename_data.Skip(1)) : "";

                        myfile = Path.Combine(Outfilelocation, filename_data.Replace("IN_", replacement + "_") + fileext);


                    }
                    else
                    {

                        myfile = Outfilelocation + "\\" + filenameconv + unixTime + $"_{batch}{fileext}";
                    }
                    //String Query3 = $"select Varname , vartype from OutputTemplateLines where ProfileFileID = {filemasterid} and ProfileId={ProfileID} order by FileLineNo";
                    String Query3 = $"select Varname , vartype from OutputTemplateLines where ProfileFileID = {filemasterid} and ProfileId={ProfileID} order by OutputTemplateLinesID";
                    //String Query3 = $"select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = {filemasterid} and FileLineNo = 1 and ProfileId={ProfileID} order by OutPutFileTemplateID,OutputTemplateLinesID";
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
                        //using (StreamWriter writer = File.CreateText(csvfile))
                        using (FileStream fs = new FileStream(csvfile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            File.SetAttributes(csvfile, FileAttributes.Hidden); // hide immediately

                            using (StreamWriter writer = new StreamWriter(fs))
                            {
                                if (Header != "")
                                {
                                    //writer.Write(Header.TrimEnd() + "\r\n");
                                    writer.Write(Header + "\r\n");
                                }
                                foreach (DataRow dr in data.Rows)
                                {
                                    writer.Write(dr[0].ToString() + "\r\n");
                                    count++;
                                }
                                if (Footer != "")
                                {
                                    // writer.Write(Footer + "\r\n");
                                    if (!Footer.Equals("NO", StringComparison.OrdinalIgnoreCase))
                                    {
                                        writer.Write(Footer);
                                    }
                                    else
                                    {
                                        writer.Flush();
                                        writer.BaseStream.SetLength(writer.BaseStream.Length - Environment.NewLine.Length);
                                    }
                                }

                            }
                        }
                        myfile = csvfile;
                        //DataTable dataTable = ConvertCsvToDataTable(csvfile);
                        //File.Delete(csvfile);
                        //SaveDataTableToExcel(dataTable, myfile);
                    }
                    else if (filetype == "CPD")
                    {
                        using (FileStream fs = new FileStream(myfile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            File.SetAttributes(myfile, FileAttributes.Hidden); // hide immediately

                            using (StreamWriter writer = new StreamWriter(fs))
                            {

                                int header_count = 0;

                                if (!string.IsNullOrWhiteSpace(Header))
                                {
                                    writer.WriteLine(Header.TrimEnd());

                                    header_count = Header.TrimEnd().Split(',').Length;
                                }
                                int countCCPD = 0;

                                string blockValue11 = "";
                                string blockValue12 = "";
                                foreach (DataRow dr in data.Rows)
                                {
                                    string[] values = dr[0].ToString().Split(';');
                                    // -------------------------------
                                    // FIELD 11 LOGIC (1,501,1001...)
                                    // -------------------------------
                                    if (countCCPD % 500 == 0) // start of block
                                    {
                                        blockValue11 = values.Length > 11 ? values[11] : "";
                                    }
                                    else
                                    {
                                        if (values.Length > 11 && values[11] != blockValue11)
                                        {
                                            values[11] = "FFFFFFFFFFFFFFFFFFFF";
                                            values[16] = "FFFFFFFFFFFFFFFFFFFF";
                                        }
                                    }

                                    // --------------------------------
                                    // FIELD 12 LOGIC (500,1000,1500...)
                                    // --------------------------------
                                    if ((countCCPD + 1) % 500 == 0) // end of block
                                    {
                                        blockValue12 = values.Length > 12 ? values[12] : "";
                                    }
                                    else
                                    {
                                        if (values.Length > 12 && values[12] != blockValue12)
                                        {
                                            values[12] = "FFFFFFFFFFFFFFFFFFFF";
                                        }
                                    }

                                    // Rebuild line
                                    string mergedLine = string.Join(";", values);

                                    writer.WriteLine(mergedLine);

                                    countCCPD++;
                                }

                                if (!string.IsNullOrWhiteSpace(Footer))
                                {
                                    //  writer.WriteLine(Footer);
                                    if (!Footer.Equals("NO", StringComparison.OrdinalIgnoreCase))
                                    {
                                        writer.Write(Footer);
                                    }
                                    else
                                    {
                                        writer.Flush();
                                        writer.BaseStream.SetLength(writer.BaseStream.Length - Environment.NewLine.Length);
                                    }
                                }
                            }




                        }
                    }

                    else if (filetype == "TXT" && customer_name_form.ToUpper() == "RELIANCE")
                    {
                        using (FileStream fs = new FileStream(myfile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            File.SetAttributes(myfile, FileAttributes.Hidden); // hide immediately

                            using (StreamWriter writer = new StreamWriter(fs))
                            {

                                int header_count = 0;

                                if (!string.IsNullOrWhiteSpace(Header))
                                {
                                    writer.WriteLine(Header.TrimEnd());

                                    header_count = Header.TrimEnd().Split(',').Length;
                                }

                                foreach (DataRow dr in data.Rows)
                                {
                                    string mergedLine = dr[0].ToString();


                                    writer.WriteLine(mergedLine.Replace("IN_", "CNUM_") + ".txt");
                                    writer.WriteLine(mergedLine.Replace("IN_", "SCM_") + ".txt");
                                    writer.WriteLine(mergedLine.Replace("IN_", "SIMODA_") + ".cps");


                                }

                                if (!string.IsNullOrWhiteSpace(Footer))
                                {
                                    //  writer.WriteLine(Footer);
                                    if (!Footer.Equals("NO", StringComparison.OrdinalIgnoreCase))
                                    {
                                        writer.Write(Footer);
                                    }
                                    else
                                    {
                                        writer.Flush();
                                        writer.BaseStream.SetLength(writer.BaseStream.Length - Environment.NewLine.Length);
                                    }
                                }
                            }




                        }
                    }
                    else
                    {
                        //using (StreamWriter writer = File.CreateText(myfile))
                        using (FileStream fs = new FileStream(myfile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            File.SetAttributes(myfile, FileAttributes.Hidden); // hide immediately

                            using (StreamWriter writer = new StreamWriter(fs))
                            {
                                if ((fileext.Equals(".mca", StringComparison.OrdinalIgnoreCase)) && ((batchtypename.Equals("QUARTER", StringComparison.OrdinalIgnoreCase) ||
     batchtypename.Equals("MFF2", StringComparison.OrdinalIgnoreCase))))
                                {
                                    int header_count = 0;

                                    if (!string.IsNullOrWhiteSpace(Header))
                                    {
                                        writer.WriteLine(Header.TrimEnd());

                                        header_count = Header.TrimEnd().Split(',').Length;
                                    }

                                    foreach (DataRow dr in data.Rows)
                                    {
                                        string[] values = dr[0].ToString().Split(',');

                                        // ✅ Check only the last column
                                        bool isCP = values.Last().Trim().Equals("CP", StringComparison.OrdinalIgnoreCase);


                                        // Take first header_count values
                                        var firstPart = values.Take(header_count);

                                        // Append last 5 if CP, else last 4
                                        var lastPart = isCP ? values.Skip(values.Length - 5) : values.Skip(values.Length - 4);

                                        // Combine both parts
                                        string mergedLine = string.Join(",", firstPart.Concat(lastPart));
                                        writer.WriteLine(mergedLine);

                                        count++;
                                    }

                                    if (!string.IsNullOrWhiteSpace(Footer))
                                    {
                                        //  writer.WriteLine(Footer);
                                        if (!Footer.Equals("NO", StringComparison.OrdinalIgnoreCase))
                                        {
                                            writer.Write(Footer);
                                        }
                                        else
                                        {
                                            writer.Flush();
                                            writer.BaseStream.SetLength(writer.BaseStream.Length - Environment.NewLine.Length);
                                        }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(Header))
                                    {
                                        writer.WriteLine(Header);
                                        //writer.WriteLine(Header.TrimEnd());
                                    }

                                    foreach (DataRow dr in data.Rows)
                                    {
                                        writer.WriteLine(dr[0].ToString());
                                        count++;
                                    }

                                    if (!string.IsNullOrWhiteSpace(Footer))
                                    {
                                        // writer.WriteLine(Footer);
                                        if (!Footer.Equals("NO", StringComparison.OrdinalIgnoreCase))
                                        {
                                            writer.Write(Footer);
                                        }
                                        else
                                        {
                                            writer.Flush();
                                            writer.BaseStream.SetLength(writer.BaseStream.Length - Environment.NewLine.Length);
                                        }
                                    }
                                }


                                ////original method
                                //if (Header != "")
                                //{
                                //    writer.Write(Header.TrimEnd() + "\r\n");
                                //}
                                //foreach (DataRow dr in data.Rows)
                                //{
                                //    writer.Write(dr[0].ToString() + "\r\n");
                                //    count++;
                                //}
                                //if (Footer != "")
                                //{
                                //    writer.Write(Footer + "\r\n");
                                //}
                            }
                        }
                    }
                    int mca_batchsize = 0;
                    //if (fileext.ToLower().Trim() == ".mca" && batchsize == 0)
                    if (fileext.ToLower().Trim() == ".mca")
                    {
                        mca_batchsize = batchsize;
                        batchsize = batchsize == 0 ? 2500 : batchsize;



                        string filename_labels = CreateMCABatch(myfile, batchsize, 500, Po_Num, customer_name_form);
                        string[] label_filename_parts = filename_labels.Split(',');
                        //myfile = EncryptionandDecryption.AESEncrypt_File(myfile, file_enc_key);
                        logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[0])}\n");
                        logString.Append($"    - No of record : {count}\n");
                        logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[1])}\n");
                        logString.Append($"    - No of record : {count}\n");
                        logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[2])}\n");
                        logString.Append($"    - No of record : {count}\n");
                        logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[3])}\n");
                        logString.Append($"    - No of record : {count}\n");
                        //UpdateProcessHDFile($"Label", Convert.ToInt32(CustProfileFileID.Trim()), $"{label_filename_parts[0]}",label_filename_parts[4]+"\\"+label_filename_parts[0], lastInsertedId);
                        //UpdateProcessHDFile($"Label", Convert.ToInt32(CustProfileFileID.Trim()), $"{label_filename_parts[1]}",label_filename_parts[4]+"\\"+label_filename_parts[1], lastInsertedId);
                        //UpdateProcessHDFile($"Label", Convert.ToInt32(CustProfileFileID.Trim()), $"{label_filename_parts[2]}",label_filename_parts[4]+"\\"+label_filename_parts[2], lastInsertedId);

                    }
                    //if (fileext.ToLower().Trim() == ".mca" && mca_batchsize > 0)
                    if (fileext.ToLower().Trim() == ".mca")
                    {
                        //Total_no_of_records += count;
                        Total_no_of_files += 1;
                        string mca_filename = Path.GetFullPath(myfile);
                        string[] lines = File.ReadAllLines(myfile);
                        if (IsSingle && lines.Length > mca_batchsize && mca_batchsize > 0)
                        {
                            if (lines.Length > mca_batchsize) // split only if more than batchsize
                            {

                                int numFiles = (int)Math.Ceiling((double)(lines.Length - 1) / mca_batchsize);

                                logString.Append($"    - MCA file starting splitted into {numFiles} parts \n");
                                for (int i = 0; i < numFiles; i++)
                                {
                                    string outputFile = myfile.Replace(fileext, $"_{(i + 1):D4}{fileext}");
                                    //using (StreamWriter writer = File.CreateText(outputFile))
                                    using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
                                    {
                                        File.SetAttributes(outputFile, FileAttributes.Hidden); // hide immediately

                                        using (StreamWriter writer = new StreamWriter(fs))
                                        {
                                            writer.WriteLine(lines[0]);
                                            for (int j = 1; (j <= mca_batchsize && (j + i * mca_batchsize) <= lines.Length - 1); j++)
                                            {
                                                writer.WriteLine(lines[j + i * mca_batchsize]);
                                            }
                                        }
                                    }
                                    outputFile = (profilename == "EUICC") ? outputFile : EncryptionandDecryption.AESEncrypt_File(outputFile, OFProcessing.file_enc_key); ;
                                    if (File.Exists(outputFile))
                                    {
                                        File.SetAttributes(outputFile, FileAttributes.Normal); // make encrypted file visible
                                    }
                                    logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                                    logString.Append($"    - No of record : {mca_batchsize}\n");

                                }
                                UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(myfile).Replace(".mca", "_mca.haes")}", myfile.Replace(".mca", "_mca.haes"), lastInsertedId);
                                File.Delete(myfile);
                            }
                            else
                            {
                                //for renameing the original file with _0001 pading
                                myfile = mca_filename.Replace(fileext, "_0001" + fileext);

                                if (File.Exists(myfile))
                                {
                                    File.Delete(myfile);
                                }

                                File.Move(mca_filename, myfile);
                                mca_filename = Path.GetFullPath(myfile);

                                // No need to split, just encrypt original file
                                string outputFile = (profilename == "EUICC") ? myfile : EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                                if (File.Exists(outputFile))
                                {
                                    File.SetAttributes(outputFile, FileAttributes.Normal); // make encrypted file visible
                                }
                                //string outputFile = EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                                logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                                logString.Append($"    - No of record : {lines.Length - 1}\n");
                                UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(outputFile)}", outputFile, lastInsertedId);
                            }
                        }
                        else if (mca_batchsize == 0)
                        {
                            //for renameing the original file with _0001 pading
                            myfile = mca_filename.Replace(fileext, "_0001" + fileext);

                            if (File.Exists(myfile))
                            {
                                File.Delete(myfile);
                            }

                            File.Move(mca_filename, myfile);
                            mca_filename = Path.GetFullPath(myfile);


                            // No need to split, just encrypt original file
                            string outputFile = (profilename == "EUICC") ? myfile : EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                            if (File.Exists(outputFile))
                            {
                                File.SetAttributes(outputFile, FileAttributes.Normal); // make encrypted file visible
                            }
                            //string outputFile = EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                            logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                            logString.Append($"    - No of record : {lines.Length - 1}\n");
                            UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(outputFile)}", outputFile, lastInsertedId);
                        }

                        else
                        {
                            //for renameing the original file with _0001 pading
                            myfile = mca_filename.Replace(fileext, "_0001" + fileext);

                            if (File.Exists(myfile))
                            {
                                File.Delete(myfile);
                            }

                            File.Move(mca_filename, myfile);
                            mca_filename = Path.GetFullPath(myfile);


                            // No need to split, just encrypt original file
                            string outputFile = (profilename == "EUICC") ? myfile : EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                            if (File.Exists(outputFile))
                            {
                                File.SetAttributes(outputFile, FileAttributes.Normal); // make encrypted file visible
                            }
                            //string outputFile = EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                            logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                            logString.Append($"    - No of record : {lines.Length - 1}\n");
                            UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(outputFile)}", outputFile, lastInsertedId);
                        }
                        //myfile = EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                    }


                    else if (fileext.ToLower().Trim() == ".mca")
                    {

                        string outputFile = (profilename == "EUICC") ? myfile : EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                        if (File.Exists(outputFile))
                        {
                            File.SetAttributes(outputFile, FileAttributes.Normal); // make encrypted file visible
                        }
                        logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                        logString.Append($"    - No of record : {count}\n");

                        UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(outputFile)}", outputFile, lastInsertedId);
                    }


                }
                if (fileext.ToLower().Trim() != ".mca")
                {
                    myfile = EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);


                    //myfile = EncryptionandDecryption.AESEncrypt_File(myfile, file_enc_key);
                    logString.Append($"    - Filename : {Path.GetFileName(myfile)}\n");
                    Console.WriteLine($"    - Filename : {Path.GetFileName(myfile)}\n");
                    logString.Append($"    - No of record : {count}\n");
                    Console.WriteLine($"    - No of record : {count}\n");
                    logString.Append($"    - Input File FileID : {lastInsertedId}\n");
                    Console.WriteLine($"    - Input File FileID : {lastInsertedId}\n");
                    logString.Append($"    - Customer Profile FileID : {Convert.ToInt32(CustProfileFileID.Trim())}\n");
                    Console.WriteLine($"    - Customer Profile FileID : {Convert.ToInt32(CustProfileFileID.Trim())}\n");
                    UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(myfile)}", myfile, lastInsertedId);
                }

                this.Invoke(new MethodInvoker(delegate
                {
                    txtoutput.Text += $"{filetype} OutFile created successfully for HDID {lastInsertedId}. \r\n";
                }));

                return 1;
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
                deletion_errorneous_data(lastInsertedId.ToString());
                logString.Append($"\nSomething went wrong while creating outfile {filetype} for HDID {lastInsertedId} error message" + ex.Message);
                return 0;
            }
            //GetGenProcessList();
        }
    }
    public class BatchCodeGenerator
    {
        private string _lastPrefix = "";
        private char _letter = 'A';
        private int _number = 0;
        private int _batchCounter = 0;
        private int _batchSize;

        public BatchCodeGenerator(int batchSize = 500)
        {
            _batchSize = batchSize;
        }

        public void LoadlastCodeMSC(string lastCode)
        {
            if (string.IsNullOrEmpty(lastCode))
                return;

            _lastPrefix = lastCode.Substring(0, lastCode.Length - 4);
            _letter = lastCode[lastCode.Length - 4];
            _number = int.Parse(lastCode.Substring(lastCode.Length - 3)) + 1;
        }
        public void LoadLastCodemsc(string lastCode)
        {
            if (string.IsNullOrEmpty(lastCode))
                return;

            _lastPrefix = lastCode.Substring(0, lastCode.Length - 4);
            _letter = lastCode[lastCode.Length - 3];
            _number = int.Parse(lastCode.Substring(lastCode.Length - 2)) + 1;
        }

        public string Generatemsn(string prefix)
        {
            if (prefix != _lastPrefix)
            {
                _letter = 'A';
                _number = 1;
                _batchCounter = 0;
                _lastPrefix = prefix;
            }

            if (_batchCounter % _batchSize == 0 && _batchCounter != 0)
            {
                _number++;

                if (_number > 999)
                {
                    _letter++;
                    _number = 1;
                }
            }

            _batchCounter++;

            return $"{prefix}{_letter}{_number:D3}";
        }

        public string GenerateMSC(string prefix)
        {
            if (prefix != _lastPrefix)
            {
                _letter = 'C';
                _number = 1;
                _batchCounter = 0;
                _lastPrefix = prefix;
            }

            if (_batchCounter % _batchSize == 0 && _batchCounter != 0)
            {
                _number++;

                if (_number > 99)
                {
                    _letter++;
                    _number = 1;
                }
            }

            _batchCounter++;

            return $"{prefix}M{_letter}{_number:D2}";
        }



    }

    public class Crc32
    {
        private static readonly uint[] Table;

        static Crc32()
        {
            Table = new uint[256];
            const uint poly = 0xEDB88320; // reversed 0x04C11DB7
            for (uint i = 0; i < Table.Length; i++)
            {
                uint crc = i;
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 1) != 0)
                        crc = (crc >> 1) ^ poly;
                    else
                        crc >>= 1;
                }
                Table[i] = crc;
            }
        }

        public static uint ComputeChecksum(byte[] bytes)
        {
            uint crc = 0xFFFFFFFF;
            foreach (byte b in bytes)
            {
                byte index = (byte)((crc & 0xFF) ^ b);
                crc = (crc >> 8) ^ Table[index];
            }
            return crc ^ 0xFFFFFFFF;
        }
    }

    public static class InputBox
    {
        public static string Show(string prompt, string title, string defaultValue = "0")
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = prompt;
            textBox.Text = defaultValue;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(10, 10, 380, 20);
            textBox.SetBounds(10, 35, 380, 20);
            buttonOk.SetBounds(220, 70, 75, 25);
            buttonCancel.SetBounds(305, 70, 75, 25);

            form.ClientSize = new System.Drawing.Size(400, 110);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            return form.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
