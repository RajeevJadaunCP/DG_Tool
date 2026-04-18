using CardPrintingApplication;
using CardPrintingApplication;
using ClosedXML.Excel;
using DG_Tool.HelperClass;
using DG_Tool.Models;
using DG_Tool.WinForms.Authentication;
using DG_Tool.WinForms.Dashboard;
using DG_Tool.WinForms.OutputFile;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Bcpg;
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


namespace DG_Tool.WinForms.OutputFile
{
    public partial class OFProcessing : Form
    {
        string Outfilelocation = "", headerfilepath = "";
        private int angle;
        private System.Windows.Forms.Timer timer;
        private Panel bufferingPanel;
        public static string profilename = "";
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
        public static string batchtypename = "";
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
        static List<string> merged_outer_label_file_names = new List<string>(); 
        static List<string> merged_inner_label_file_names = new List<string>(); 
        static List<string> merged_outer_label_file_names_1 = new List<string>(); 
        static List<string> merged_batch_list = new List<string>();

        string connectionString = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);


        public void upload_log()
        {
            try
            {
                if (EncryptDB == "1")
                {
                }
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("insert into [DataTool_log]([Date],[LogMsg],[User_name]) Values (@Date, @LogMsg, @User_name)", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@LogMsg", logString.ToString());
                        cmd.Parameters.AddWithValue("@User_name", LoginPage.username);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in log uploadation : " + ex.Message+ "\n\nStack Trace:\n" + ex.StackTrace);
            }
        }

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

                        SqlTransaction transaction = con11test_1.BeginTransaction();
                        command.Transaction = transaction;

                        int rowsAffected = command.ExecuteNonQuery();
                        transaction.Commit();

                        Console.WriteLine($"Data deleted successfully. Rows affected: " + rowsAffected);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
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
                            MessageBox.Show("No file to proceed.");
                            txtoutput.Text += "No file to proceed.\r\n";
                        }
                        else
                        {
                            txtoutput.Text += txtInputfile.Text.Replace(',', '\n') + "\nOK To Proceed.\r\n";
                        }

                    }
                    else
                    {
                        MessageBox.Show("Wrong input file selected ",
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

        public string iccid_dupcheck_new_faster_all_duplciates(string filename)
        {
            logString.Append("       - Checking for duplicate records in the database.\n");
            string[] strs = { "QUANTITY", "ICCID", "IMSI", "MSISDN", "BatchNumber" };

            int file_qty = 0;
            StringBuilder resultBuilder = new StringBuilder();

            string[] lines = File.ReadAllLines(filename);

            foreach (string str in strs)
            {
                dupcheck_variable = str;
                string varname = "", isincremental = "";
                int pos_from = 0, len = 0, line = 0;

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(@"
            SELECT [VarName], [PositionFrom], [Len], [LineNumber], [Tag]
            FROM [InPutDataTemplate]
            WHERE [CustID] = @CustID 
              AND [ProfileID] = @ProfileID 
              AND [VarDes] = @VarDes 
              AND vartext = 'FL'
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


                if (!string.IsNullOrEmpty(isincremental) &&
                    isincremental.Equals("incremental", StringComparison.OrdinalIgnoreCase))
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

                DataTable iccidTable = new DataTable();
                iccidTable.Columns.Add("Value", typeof(string));
                iccidTable.BeginLoadData();
                foreach (string val in datalist)
                    iccidTable.Rows.Add(val);
                iccidTable.EndLoadData();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand createCmd = new SqlCommand(
                        "CREATE TABLE #IccidList (Value NVARCHAR(50) PRIMARY KEY);", con))
                    {
                        createCmd.ExecuteNonQuery();
                    }

                    using (SqlBulkCopy bulk = new SqlBulkCopy(con))
                    {
                        bulk.DestinationTableName = "#IccidList";
                        bulk.WriteToServer(iccidTable);
                    }

                    string dupQuery = $@"
                SELECT 
                    f.DataGenProcessHDID, 
                    f.FileName, 
                    f.OutFileProcessDate,
                    d.{str} AS DupValue,
                    COUNT_BIG(*) AS RecordCount
                FROM DataGenProcessHDFile f
                JOIN DupCheck d ON d.C1 = f.DataGenProcessHDID
                JOIN #IccidList il 
                    ON d.{str} COLLATE SQL_Latin1_General_CP1_CI_AS 
                       = il.Value COLLATE SQL_Latin1_General_CP1_CI_AS
                WHERE f.OutFlileStatus = 2
                GROUP BY f.DataGenProcessHDID, f.FileName, f.OutFileProcessDate, d.{str}
                ORDER BY RecordCount DESC;";

                    using (SqlCommand cmd = new SqlCommand(dupQuery, con))
                    {
                        try
                        {
                            cmd.CommandTimeout = 15000;

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int dataGenProcessHDID = reader.GetInt32(0);
                                    string fileName = reader.GetString(1);
                                    DateTime outFileProcessDate = reader.GetDateTime(2);
                                    string dupHex = reader.IsDBNull(3) ? "" : reader.GetString(3);
                                    long count = reader.GetInt64(4);

                                    string dupValue = !string.IsNullOrEmpty(dupHex)
                                                        ? HexToString(dupHex)
                                                        : string.Empty;

                                    if (string.IsNullOrEmpty(Dup_First_icicid) && !string.IsNullOrEmpty(dupValue))
                                        Dup_First_icicid = dupValue;

                                    resultBuilder.AppendLine($"Duplicate Type     : {str}");
                                    resultBuilder.AppendLine($"FileID             : {dataGenProcessHDID}");
                                    resultBuilder.AppendLine($"FileName           : {fileName}");
                                    resultBuilder.AppendLine($"Processed Date     : {outFileProcessDate}");
                                    resultBuilder.AppendLine($"Duplicate Value    : {dupValue}");
                                    resultBuilder.AppendLine($"Duplicate Count    : {count}");
                                    resultBuilder.AppendLine(new string('-', 70));
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Database operation timed out or the server is not responding.\n\nError Details: " + ex.Message,
                                            "SQL Timeout Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An unexpected error occurred.\n\nError Details: " + ex.Message,
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        }
                    }
                }
            }

            if (resultBuilder.Length > 0)
            {
                logString.Append("       - Duplicate records found.\n");
                return "Duplicate Records Found :- \n" + resultBuilder.ToString();
            }

            logString.Append("       - No duplicate records found.\n");
            return string.Empty;
        }


        public string iccid_dupcheck_new_faster(string filename)
        {
            logString.Append("       - Checking for duplicate records in the database.\n");
            string[] strs = { "QUANTITY", "ICCID", "IMSI", "MSISDN", "BatchNumber" };
            string msg = "";
            int file_qty = 0;

            string[] lines = File.ReadAllLines(filename);

            foreach (string str in strs)
            {
                dupcheck_variable = str;
                string varname = "", isincremental = "";
                int pos_from = 0, len = 0, line = 0;

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


                if (str == "ICCID" || str == "IMSI" || str == "MSISDN")
                {
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

                    if (checkLine.Length < pos_from + len)
                    {
                        MessageBox.Show($"{str} configuration error:\n" +
                               $"{str} is expected on Line No {line} starting at position {pos_from} " +
                               $"with length {len}, but this data is NOT present in the file."); 
                        return $"{str} configuration error:\n" +
                               $"{str} is expected on Line No {line} starting at position {pos_from} " +
                               $"with length {len}, but this data is NOT present in the file.";
                    }
                }


                if (!string.IsNullOrEmpty(isincremental) &&
                    isincremental.Equals("incremental", StringComparison.OrdinalIgnoreCase))
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

                DataTable iccidTable = new DataTable();
                iccidTable.Columns.Add("Value", typeof(string));
                iccidTable.BeginLoadData();
                foreach (string val in datalist)
                    iccidTable.Rows.Add(val);
                iccidTable.EndLoadData();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand createCmd = new SqlCommand(
                        "CREATE TABLE #IccidList (Value NVARCHAR(50) PRIMARY KEY);", con))
                    {
                        createCmd.ExecuteNonQuery();
                    }

                    using (SqlBulkCopy bulk = new SqlBulkCopy(con))
                    {
                        bulk.DestinationTableName = "#IccidList";
                        bulk.WriteToServer(iccidTable);
                    }


                    string dupQuery = $@"
                        SELECT TOP 1 
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
                            MessageBox.Show("Database operation timed out or the server is not responding.\n\nError Details: " + ex.Message,
                                            "SQL Timeout Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An unexpected error occurred.\n\nError Details: " + ex.Message,
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
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
                con.Open();
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
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Please select the batch first.",
                                                   "Message",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Information
                                                   );
                return;
            }

            if (string.IsNullOrEmpty(tb_ponum.Text.Trim()))
            {
                MessageBox.Show("Please enter the po number first.",
                                                   "Message",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Information
                                                   );
                return;
            }
            if (comboBox1.SelectedValue?.ToString() == "0")
            {
                string input = InputBox.Show("Enter batch size (leave blank for 0):", "Batch Size", "0");

                if (int.TryParse(input, out int result))
                {
                    batchsize = result;
                }
            }
            if (Circle_Label.Text.Trim() == "")
            {
                MessageBox.Show("Please enter the Label Circle first.",
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

                    if (string.IsNullOrWhiteSpace(input))
                    {
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
                        MessageBox.Show("Invalid quantity! Please enter a number greater than 0.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            this.Invoke(new MethodInvoker(delegate
            {
                panel1.Visible = false;
            }));

            

        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }
        public void startProcessing()
        {
            try
            {
                if (customerID > 0 && circleID > 0 && ProfileID > 0 && !string.IsNullOrEmpty(inputFile) && !string.IsNullOrEmpty(licenceFile))
                {

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
                                int data_file_check = btnImport_Click(lastInsertedId, lotid);
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

                if (InsertedHDIDS.Count > 1)
                {
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
                }

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

            }
            finally
            {
                upload_log();
                logString.Clear();
               
                MessageBox.Show($"File Generated for LotID : {FileProcessingLotID}");
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


                using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
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

                string filename_labels = CreateMCABatch(outputPath, batchsize, 500, Po_Num);
                string[] label_filename_parts = filename_labels.Split(',');

                logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[0])}\n");
                logString.Append($"    - Total no of record : {dummy_file_qty}\n");
                logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[1])}\n");
                logString.Append($"    - Total no of record : {dummy_file_qty}\n");
                logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[2])}\n");
                logString.Append($"    - Total no of record : {dummy_file_qty}\n");
                logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[3])}\n");
                logString.Append($"    - Total no of record : {dummy_file_qty}\n");

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

                            using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
                            {

                                using (StreamWriter writer = new StreamWriter(fs))
                                {
                                    writer.WriteLine(lines[0]);
                                    for (int j = 1; (j <= mca_batchsize && (j + i * mca_batchsize) <= lines.Length - 1); j++)
                                    {
                                        writer.WriteLine(lines[j + i * mca_batchsize]);
                                    }
                                }
                            }

                            outputFile = EncryptionandDecryption.AESEncrypt_File(outputFile, OFProcessing.file_enc_key);

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

                logString.Append($"\n**************************************[Logging Out] Data Processing is Completed [{DateTime.Now}] **************************************\n");
                upload_log();
            }
            catch (Exception ex)
            {
                logString.Append($"\n[ERROR] Exception occurred: {ex.Message}\n{ex.StackTrace}\n");
                upload_log();
                try
                {
                    if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
                    {
                        Directory.Delete(folderPath, true);
                        logString.Append($"\n[INFO] All mca files and folder deleted from productions due to error.\n");
                    }
                    string label_path = folderPath.Replace("\\Productions\\", "\\Data_Gen\\Label\\");
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
                    else if (var_des == "BatchNumber")
                    {
                        Batch_line_no = Convert.ToInt32(line_sql);
                        Batch_frm = Convert.ToInt32(Pos_From);
                        Batch_len = Convert.ToInt32(len_data);
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
                    int line_number = 1;
                    string line;

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
                    MessageBox.Show("Error During Importation : " + ex.Message +"\n\nStack Trace:\n" + ex.StackTrace);

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
                    MessageBox.Show("Error During Importaion : " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
                    string error = $"Error During Importaion in file '{Path.GetFileName(filename_2)}' : " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace;
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
                string r4_data = "", r8_data = "" ,euicc_ci_cert_data = "", euicc_eum_cert_data = "", euicc_pri_wrapped_key = "", euicc_pub_key_value="";
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
                                        MessageBox.Show(exe.Message +                "\n\nStack Trace:\n" + exe.StackTrace);
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
                                                MessageBox.Show(exe.Message +                "\n\nStack Trace:\n" + exe.StackTrace);
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
                                                    MessageBox.Show(exe.Message +                "\n\nStack Trace:\n" + exe.StackTrace);
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
                                int varCount = tag_value.Count(c => c == ',');
                                string[] parts = Array.Empty<string>();
                                string caseSwitch = Algo_Name;
                                string key_tag = "", data_tag;
                               


                                switch (caseSwitch)
                                {

                                    case "substring":
                                        if (varCount > 1)
                                        {
                                        }
                                        else
                                        {
                                            int pos_from = Convert.ToInt32(Pos_From);
                                            len = Convert.ToInt32(len_data);
                                            string data_new_test = Process_data.Select($"Variable = '{tag_value}'")[0]["Value"].ToString();
                                            if (len == 0)
                                            { len = data_new_test.Trim().Length - pos_from + 1; }
                                            

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

                                    case "Triple_Des_CBC":
                                        parts = tag_value.Split(',');
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
                                        string response_data = euicc_Data_function(parts[0].Trim(), parts[1].Trim(),1,1);
                                        response_data = response_data.Replace("\"", "");
                                        if (response_data.Contains("CKR_GENERAL_ERROR"))
                                        {
                                            throw new Exception($"hsm connectivity issue {tag_value}");
                                            logString.Append($"\nHsm connectivity issue {tag_value}");
                                        }
                                        
                                        response_data = response_data.Replace("}", "");
                                        parts = response_data.Split(',');
                                        euicc_ci_cert_data = parts[1].Replace("root_ca_hex:","").Trim();
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
                                        myData =  eid_function(1);
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
                        Process_data.Rows.Add(var_name, var_des, myData);
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error During Importation : on variable {var_des} error_msg id " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
                string error = $"Error During Importation in file '{Path.GetFileName(filename_2)}' : " + ex.Message +                "\n\nStack Trace:\n" + ex.StackTrace;
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
        static bool HasSpecialCharacters(string input)
        {
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

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while getting filename: " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            return filename_12;
        }
        public string Random4digits()
        {
            random_four = RNG32.Next(1000, 9999).ToString();
            return random_four;

        }

       

        public string Random8hex()
        {
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
                    }
                    catch (Exception exe)
                    {
                        MessageBox.Show(exe.Message +                "\n\nStack Trace:\n" + exe.StackTrace);
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

                    {
                        pad += '3' + q[i].ToString();

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
            random_eight = RNG32.Next(10000000, 99999999).ToString();
            return random_eight;
        }
        public string padding(string q)
        {
            string t = q.Trim();
            char[] ns = t.ToCharArray();

            string padding = string.Join("3", ns);
            padding = "3" + padding;
            return padding;
        }
        public string Create32DigitString()
        {


            byte[] theBytes = new byte[16];
            RNG32.NextBytes(theBytes);
            StringBuilder buffer = new StringBuilder(32);
            for (int i = 0; i < 16; i++)
            {
                buffer.Append(theBytes[i].ToString("X").PadLeft(2, '0'));
            }
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



            byte[] theBytes = new byte[8];
            RNG16.NextBytes(theBytes);
            StringBuilder buffer = new StringBuilder(16);
            for (int i = 0; i < 8; i++)
            {
                buffer.Append(theBytes[i].ToString("X").PadLeft(2, '0'));
            }
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
            return padding;

        }


        public string AES_WRAP(string key, string data)
        {
            byte[] aesKeyBytes = HexStringToByteArray(key);
            byte[] priBytes =  HexStringToByteArray(data);
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
            data = data+ (eid_db + i).ToString().PadLeft(12, '0');
            long sum = 0;

            foreach (char c in data)
            {
            }

            data = data + checksumStr;
            EID_db_last = eid_db + i;
            return data;
        }

        public string hsm_randomfunction(string cert_name, string eum_cert_name, int i, int attempt)
        {
            int maxAttempts = 10;
            using (HttpClient client = new HttpClient())
            {

                try
                {

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
                        return euicc_Data_function(cert_name, eum_cert_name, i, attempt + 1);
                    }
                    else
                    {
                        response_Data = $"ERROR: {ex.Message}";
                    }
                }

            }



            return response_Data;
        }

        public string euicc_Data_function(string cert_name,string eum_cert_name, int i, int attempt)
        {
            int maxAttempts = 10;
            using (HttpClient client = new HttpClient())
            {

                try
                {
                    
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



                    response_Data = responseBody;
                    

                    string []parts_list12345 = response_Data.Split(',');
                    string euicc_ci_cert_data = parts_list12345[1].Replace("root_ca_hex:", "").Trim();
                    euicc_ci_cert_data = parts_list12345[2].Replace("sub_ca_hex:", "").Trim();
                    euicc_ci_cert_data = parts_list12345[6].Replace("wrapped_key:", "").Trim();
                    euicc_ci_cert_data = parts_list12345[3].Replace("end_entity_hex:", "").Trim();
                }
                catch (Exception ex)
                {
                    if (attempt < maxAttempts)
                    {
                        return euicc_Data_function(cert_name, eum_cert_name, i, attempt + 1);
                    }
                    else
                    {
                        response_Data = $"ERROR: {ex.Message}";
                    }
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
            return padding;

        }


        public string mncmcc_function(string s)
        {
            s = s.Trim();
            string data = "";

            if (s.Length > 0)
            {
                if (s.Length % 2 != 0)
                {
                    s += 'F';
                }

                data = $"{s[1]}{s[0]}{s[5]}{s[2]}{s[4]}{s[3]}";
            }
            else
            {
                data = "INVALID_NS_" + s;
            }

            return data;
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






            for (i = hexNum.Length - 1; i >= 0; i--)
                hex_acc += hexNum[i];
            }

            hex_acc = hex_acc.PadLeft(4, '0');

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
            string[] default_values = { "AGSUI:IMSI", "EKI", "KIND", "DATE_AL", "FSETIND", "A4IND", "Transport_key", "Quantity", "Index_Value", "BatchNumber" };
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
                            })
                            .ToList();


            List<string> varIDS = dt1.AsEnumerable()
                                  .Where(row => !string.IsNullOrEmpty(row.Field<string>("algoname")) && row.Field<string>("algoname").Contains("Hex") && row.Field<string>("algoname").Contains("R_"))
                                  .Select(row => row.Field<string>("VarID").Trim())
                                  .ToList();







            string urlText = "";

            for (int i = 1; i <= records; i++)
            {
                Dictionary<string, string> FetchAPI = new Dictionary<string, string>();
                string euicc_ci_cert_data = "", euicc_eum_cert_data = "", euicc_pri_wrapped_key = "", euicc_pub_key_value = "";
                

                if (records > 500)
                {
                    if (i % (records / 500) == 0)
                    {
                        WebRequest request = HttpWebRequest.Create(url);
                        WebResponse response = request.GetResponse();
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        urlText = reader.ReadToEnd() + "rem";
                        if (urlText.Contains("CKR_DEVICE_REMOVED"))
                        {

                            urlText = random_hex_generator(string.Join(",", middleValues)) + "rem";

                        }
                    }
                    else
                        urlText = random_hex_generator(string.Join(",", middleValues)) + "rem";
                }
                else if (records > 10)
                {
                    if (i % (records / 10) == 0)
                    {
                        WebRequest request = HttpWebRequest.Create(url);
                        WebResponse response = request.GetResponse();
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        urlText = reader.ReadToEnd() + "rem";
                        if (urlText.Contains("CKR_DEVICE_REMOVED"))
                        {
                            urlText = random_hex_generator(string.Join(",", middleValues)) + "rem";
                        }
                    }
                    else
                        urlText = random_hex_generator(string.Join(",", middleValues)) + "rem";
                }

                else
                {

                    WebRequest request = HttpWebRequest.Create(url);
                    WebResponse response = request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    urlText = reader.ReadToEnd() + "rem";
                    if (urlText.Contains("CKR_DEVICE_REMOVED"))
                    {
                        urlText = random_hex_generator(string.Join(",", middleValues)) + "rem";
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
                    MessageBox.Show("HSM is not on. Please check with Key Manager.\n\nError Details: " + ex.Message,
                                    "HSM Connection Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
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
                                    my_data = var_Value.Trim();
                                }
                            }
                            else if (var_name.ToUpper() == "BATCHNUMBER")
                            {
                                my_data = var_Value.Trim();
                                batchnumber_file = my_data;
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
                                        }
                                        else
                                        {
                                            my_data = acc(newRow[variableID].ToString());
                                        }

                                    }
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

                                    my_data = FetchAPI[var_ID];
                                    break;

                                case "R_32_Hex":

                                    my_data = FetchAPI[var_ID];
                                    break;

                                case "R_48_Hex":

                                    my_data = FetchAPI[var_ID];
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
                                    }
                                    else
                                    {
                                        string icicid_num = newRow[variableID].ToString();
                                        my_data = nibble_swapped(icicid_num);
                                    }
                                    break;

                                case "IMSI_NS":
                                    if (varCount > 0)
                                    {
                                        MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                    }
                                    else
                                    {
                                        string imsi_num = "809" + newRow[variableID].ToString();
                                        my_data = nibble_swapped(imsi_num);
                                    }
                                    break;

                                case "R_32_Hex_KI":
                                    my_data = FetchAPI[var_ID];
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
                                        string[] varIDs = variableID.Split(',');
                                        Console.WriteLine($"{newRow[varIDs[0]].ToString()} --->  {newRow[varIDs[1]].ToString()}");
                                        my_data = AES_ENCYPRTION(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                    }

                                    else
                                    {
                                        MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                    }
                                    break;

                                case "Single_Des":
                                    if (varCount == 1)
                                    {
                                        string[] varIDs = variableID.Split(',');
                                        Console.WriteLine($"{newRow[varIDs[0]].ToString()} --->  {newRow[varIDs[1]].ToString()}");
                                        my_data = Encrypt_SingleDES(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                    }

                                    else
                                    {
                                        MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                    }
                                    break;



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
                                            string[] varIDs = variableID.Split(',');
                                            my_data = OPC_GEN.opc(newRow[varIDs[1]].ToString(), newRow[varIDs[0]].ToString());
                                        }


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
                                            string[] varIDs = variableID.Split(',');
                                            my_data = TripleDESEncrypt_cbc(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                        }

                                        else
                                        {
                                            MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
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
                                        string response_data = euicc_Data_function(varIDs[0], varIDs[1],i,1);
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
                                        }
                                        else
                                        {
                                            
                                            my_data = CHECKSUM(newRow[variableID].ToString());
                                        }


                                        
                                    }
                                    
                                    break;
                            }
                            newRow[var_ID] = my_data;

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing var_ID: {dv0[2].ToString().TrimEnd()}, Error: {ex.Message} \\n\\nStack Trace:\\n + {ex.StackTrace}");
                        logString.AppendLine($"Error processing var_ID: {dv0[2].ToString().TrimEnd()}, Error: {ex.Message}\\n\\nStack Trace:\\n + {ex.StackTrace}");
                    }
                }



                Process_data.Rows.Add(newRow);
                Console.WriteLine($"Processed Row : " + i);


            }
            if (hsm_flag == 0)
            {
                logString.AppendLine("\nFile processing stopped.\n");
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
                string encryptedHex = BitConverter.ToString(encryptedBytes).Replace("-", "").Substring(0, toEncrypt.Length).ToUpper();
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
        public string GetLuhnCheckDigit(string number)
        {
            var sum = 0;
            var alt = true;
            var digits = number.ToCharArray();

            for (int i = digits.Length - 1; i >= 0; i--)
            {

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

            if (checkDigit == 10)
                checkDigit = 0;


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
                hex.Append(((int)c).ToString("X2"));
            }

            return hex.ToString();
        }
        static string HexToString(string hexString)
        {
            StringBuilder ascii = new StringBuilder();

            for (int i = 0; i < hexString.Length; i += 2)
            {
                string hexPair = hexString.Substring(i, 2);
                int charValue = Convert.ToInt32(hexPair, 16);

                ascii.Append((char)charValue);
            }

            return ascii.ToString();
        }

        public string CalculateKCV(string key_value, string keytype_entered)
        {
            byte[] keyBytes = HexStringToByteArray(key_value);
            if (keyBytes.Length != 16 && keyBytes.Length != 24)
            {
                throw new ArgumentException("Key must be 16 or 24 bytes for Triple DES.");
            }


            if (keytype_entered.ToUpper().Contains("DES"))
                byte[] zeroBlock = new byte[8];

                using (var tripleDes = new TripleDESCryptoServiceProvider())
                {
                    tripleDes.Key = keyBytes;
                    tripleDes.Mode = CipherMode.ECB;
                    tripleDes.Padding = PaddingMode.None;

                    using (var encryptor = tripleDes.CreateEncryptor())
                    {
                        byte[] encrypted = encryptor.TransformFinalBlock(zeroBlock, 0, zeroBlock.Length);

                        return BitConverter.ToString(encrypted).Replace("-", "").Substring(0, 6);
                    }
                }
            }
            else if (keytype_entered.ToUpper().Contains("AES"))
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
                    string encryptedHex = BitConverter.ToString(encryptedBytes).Replace("-", "").Substring(0, 32).ToUpper();
                    result = encryptedHex.ToUpper();
                }
                return result.Substring(0, 6);



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

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;

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
                                int data_status  = generate_AllTypeOutput(fileName);
                                if (data_status == 0)
                                {
                                    throw new InvalidOperationException("Output generation failed.");
                                }
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
                        using (FileStream fs = new FileStream(csvfile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {

                            using (StreamWriter writer = new StreamWriter(fs))
                            {
                                if (Header != "")
                                {
                                    writer.Write(Header.TrimEnd() + "\r\n");
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
                        myfile = csvfile;
                    }
                    else
                    {
                        using (FileStream fs = new FileStream(myfile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {

                            using (StreamWriter writer = new StreamWriter(fs))
                            {
                                if ((fileext.Equals(".mca", StringComparison.OrdinalIgnoreCase)) && ((batchtypename.Equals("QUARTER", StringComparison.OrdinalIgnoreCase) ||
     batchtypename.Equals("MFF2", StringComparison.OrdinalIgnoreCase)) ) )
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

                                        bool isCP = values.Last().Trim().Equals("CP", StringComparison.OrdinalIgnoreCase);
    

                                        var firstPart = values.Take(header_count);

                                        var lastPart = isCP ? values.Skip(values.Length - 5) : values.Skip(values.Length - 4);

                                        string mergedLine = string.Join(",", firstPart.Concat(lastPart));
                                        writer.WriteLine(mergedLine);

                                        count++;
                                    }

                                    if (!string.IsNullOrWhiteSpace(Footer))
                                    {
                                        writer.WriteLine(Footer);
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(Header))
                                    {
                                        writer.WriteLine(Header.TrimEnd());
                                    }

                                    foreach (DataRow dr in data.Rows)
                                    {
                                        writer.WriteLine(dr[0].ToString());
                                        count++;
                                    }

                                    if (!string.IsNullOrWhiteSpace(Footer))
                                    {
                                        writer.WriteLine(Footer);
                                    }
                                }


                            }
                        }
                    }
                    int mca_batchsize = 0;
                    if (fileext.ToLower().Trim() == ".mca")
                    {
                        mca_batchsize = batchsize;
                        batchsize = batchsize == 0 ? 2500 : batchsize;



                        string filename_labels = CreateMCABatch(myfile, batchsize, 500, Po_Num);
                        string[] label_filename_parts = filename_labels.Split(',');
                        logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[0])}\n");
                        logString.Append($"    - Total no of record : {count}\n");
                        logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[1])}\n");
                        logString.Append($"    - Total no of record : {count}\n");
                        logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[2])}\n");
                        logString.Append($"    - Total no of record : {count}\n");
                        logString.Append($"    - Filename : {Path.GetFileName(label_filename_parts[3])}\n");
                        logString.Append($"    - Total no of record : {count}\n");

                    }
                    if (fileext.ToLower().Trim() == ".mca" && mca_batchsize > 0)
                    {
                        string mca_filename = Path.GetFullPath(myfile);
                        string[] lines = File.ReadAllLines(myfile);
                        if (IsSingle && lines.Length > mca_batchsize)
                        {
                            {

                                int numFiles = (int)Math.Ceiling((double)(lines.Length-1) / mca_batchsize);

                                logString.Append($"    - MCA file starting splitted into {numFiles} parts \n");
                                for (int i = 0; i < numFiles; i++)
                                {
                                    string outputFile = myfile.Replace(fileext, $"_{(i + 1):D4}{fileext}");
                                    using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
                                    {

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
                                    }
                                    logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                                    logString.Append($"    - No of record : {mca_batchsize}\n");
                                    
                                }
                                UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(myfile).Replace(".mca","_mca.haes")}", myfile.Replace(".mca", "_mca.haes"), lastInsertedId);
                                File.Delete(myfile);
                            }
                            else
                            {
                                string outputFile = (profilename == "EUICC") ? myfile : EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                                if (File.Exists(outputFile))
                                {
                                }
                                logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                                logString.Append($"    - No of record : {lines.Length - 1}\n");
                                UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(outputFile)}", outputFile, lastInsertedId);
                            }
                        }
                        else
                        {
                            string outputFile = (profilename == "EUICC") ? myfile : EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                            if (File.Exists(outputFile))
                            {
                            }
                            logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                            logString.Append($"    - No of record : {lines.Length - 1}\n");
                            UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(outputFile)}", outputFile, lastInsertedId);
                        }
                    }


                    else if (fileext.ToLower().Trim() == ".mca")
                    {

                        string outputFile = (profilename == "EUICC") ? myfile : EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                        if (File.Exists(outputFile))
                        {
                        }
                        logString.Append($"    - Filename : {Path.GetFileName(outputFile)}\n");
                        logString.Append($"    - No of record : {count}\n");
                        UpdateProcessHDFile($"{filetype}", Convert.ToInt32(CustProfileFileID.Trim()), $"{Path.GetFileName(outputFile)}", outputFile, lastInsertedId);
                    }
                   

                }
                if (fileext.ToLower().Trim() != ".mca")
                {
                    myfile = EncryptionandDecryption.AESEncrypt_File(myfile, OFProcessing.file_enc_key);
                    
                    
                    logString.Append($"    - Filename : {Path.GetFileName(myfile)}\n");
                    Console.WriteLine($"    - Filename : {Path.GetFileName(myfile)}\n");
                    logString.Append($"    - Total no of record : {count}\n");
                    Console.WriteLine($"    - Total no of record : {count}\n");
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
        }




























                        





                                    


                    
                


        public static string CreateMCABatch(string mcaFile, int outerBatchSize, int innerBatchSize, string poNumber)
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

            int imsiIndex = Array.FindIndex(columns, c => c.Contains("IMSI"));
            int iccidIndex = Array.FindIndex(columns, c => c.Contains("ICCID"));

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


            string outter_label_file_name = SaveDataTableToCsv(dtOuter, mcaFile, poNumber, "Outer", outerBatchSize, file_qty);

            string inner_label_file_name = SaveDataTableToCsv(dtInner, mcaFile, poNumber, "Inner", innerBatchSize, file_qty);

            string outter_label_file_name_1 = SaveDataTableToCsv(dt2000, mcaFile, poNumber, "2000", 2000, file_qty);

            string batch_list = SaveDataTableToCsv(dtOuter, mcaFile, poNumber, "Batch_List", outerBatchSize, file_qty);

            merged_outer_label_file_names.Add(outter_label_file_name );
            merged_inner_label_file_names.Add(inner_label_file_name);
            merged_outer_label_file_names_1.Add(outter_label_file_name_1);
            merged_batch_list.Add(batch_list);

            string myfile1 = outter_label_file_name + ",";
            myfile1 += inner_label_file_name + ",";
            myfile1 += outter_label_file_name_1 + ",";
            myfile1 += batch_list+ ",";
            myfile1 += Path.GetDirectoryName(outter_label_file_name);

            return ($"{myfile1}");

        }

        public static void MergeCsvsFromFolders(string[][] allFileSets)
        {
            if (allFileSets == null || allFileSets.Length == 0)
                throw new ArgumentException("No file sets provided.");

            string firstFolder = Path.GetDirectoryName(allFileSets[0][0]);

            MergeCsvType(allFileSets, "Outer", firstFolder);
            MergeCsvType(allFileSets, "Inner", firstFolder);
            MergeCsvType(allFileSets, "2000_Label", firstFolder);
            MergeCsvType(allFileSets, "Batch_List", firstFolder);
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



                foreach (var file in matchingFiles)
                {
                    string[] lines = File.ReadAllLines(file);
                    if (lines.Length == 0) continue;

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];

                        if (isFirstFile && i == 0)
                        {
                            writer.WriteLine(line);
                            continue;
                        }
                        else if (i == 0)
                        {
                        }

                        string[] columns = line.Split(',');
                        columns[0] = srNoCounter.ToString().PadLeft(5, '0');
                        srNoCounter++;

                        writer.WriteLine(string.Join(",", columns));
                    }

                    isFirstFile = false;
                }




            }

            Console.WriteLine($"Merged {typeKeyword} files saved at: {mergedFilePath}");
        }



        private static string SaveDataTableToExcel(DataTable dt, string mcaFilePath, string poNumber, string labelType, int batchSize)
        {
            string baseDir = Path.GetDirectoryName(mcaFilePath);
            string excelFilePath = "";
            string filePrefix = Path.GetFileNameWithoutExtension(mcaFilePath);

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
                excelFilePath = Path.Combine(baseDir,
                    $"{filePrefix}_{labelType}_Label_PO_{poNumber}_{batchSize}.xlsx");

                workbook.SaveAs(excelFilePath);
            }
            return excelFilePath;
        }

        private static string SaveDataTableToCsv(DataTable dt, string mcaFilePath, string poNumber, string labelType, int batchSize, int fileqty)
        {
            string baseDir = Path.GetDirectoryName(mcaFilePath);
            baseDir = baseDir.Replace("\\Productions\\", "\\Data_Gen\\Label\\");
            string csvFilePath = "";
            string filePrefix = Path.GetFileNameWithoutExtension(mcaFilePath);
            if (!Directory.Exists(baseDir))
            {
            }
            csvFilePath = Path.Combine(baseDir,
                $"{filePrefix}_{labelType}_Label_PO_{poNumber}_{batchSize}_{fileqty}.csv");

            using (var writer = new StreamWriter(csvFilePath, false, Encoding.UTF8))
            {
                var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName);
                writer.WriteLine(string.Join(",", columnNames));

                foreach (DataRow row in dt.Rows)
                {
                    var fields = row.ItemArray.Select(field =>
                    {
                        string value = field?.ToString() ?? "";
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
                MessageBox.Show("Something went wrong while getting filename: " + ex.Message,
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




        public static void SaveDataTableToExcel_1(DataTable dt, string excelFilePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(dt, "Sheet1");
                workbook.SaveAs(excelFilePath);
            }
        }

        public static void SaveDataTableToExcel(DataTable dt, string excelFilePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");

                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    worksheet.Cell(1, col + 1).Value = dt.Columns[col].ColumnName;
                }

                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        worksheet.Cell(row + 2, col + 1).SetValue(dt.Rows[row][col]?.ToString() ?? "");
                    }
                }

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



                    }
                }
            }
            catch (Exception ex)
            {
                logString.Append($"\nSomething went wrong with: {processFor}-{ex.Message}");
                Console.WriteLine($"\nSomething went wrong with: {processFor}-{ex.Message}");
            }

        }






































































































































        private void GetGenProcessList()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_DataGenProcessHDFile", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InputfiletId", fileid);
                    cmd.Parameters.AddWithValue("@CustProfile_ID", OFProcessing.ProfileID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    da.Fill(dt);







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



                    }
                }
            }
            catch (Exception ex)
            {
                logString.Append($"\nSomething went wrong with: {processFor}-{ex.Message}");
                Console.WriteLine($"\nSomething went wrong with: {processFor}-{ex.Message}");
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




        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        public string FetchDataFromApi(int n)
        {
            n = n / 2;
            WebRequest request = HttpWebRequest.Create(url);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string urlText = reader.ReadToEnd();
            return urlText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OFProcessing_Multi processing = new OFProcessing_Multi();
            processing.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            logString.Clear();
            this.Close();
        }

        






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
            row["Start_ICCID"] = NibbleSwap_F_Replace(startArray[iccidIndex]);
            row["End_ICCID"] = NibbleSwap_F_Replace(endArray[iccidIndex]);
            row["Start_IMSI"] = NibbleSwap_F_Replace(startArray[imsiIndex]).Substring(3, startArray[imsiIndex].Length - 3);
            row["End_IMSI"] = NibbleSwap_F_Replace(endArray[imsiIndex]).Substring(3, endArray[imsiIndex].Length - 3);
            row["Quantity"] = Quantity;
            dt.Rows.Add(row);
        }
        static string NibbleSwap_F_Replace(string hex)
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


        public static void deletion_errorneous_data(string processHdId)
        {

            List<string> files = Database.FetchFilespathbyhdid(Convert.ToInt32(processHdId));

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
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete folder '{folder}': {ex.Message}");
                }
            }

            try
            {
                Database.DeleteDBFile(Convert.ToInt32(processHdId));

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete the error files : " + ex.Message);
            }

        }
    }


    public class Crc32
    {
        private static readonly uint[] Table;

        static Crc32()
        {
            Table = new uint[256];
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
