using CardPrintingApplication;
using ClosedXML.Excel;
using DG_Tool.HelperClass;
using DG_Tool.Models;
using DG_Tool.WinForms.Authentication;
using DocumentFormat.OpenXml.InkML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DG_Tool.WinForms.OutputFile
{
    public partial class OFProcessing_Multi : Form
    {
        public int batchsize = 0;
        public OFProcessing_Multi()
        {
            InitializeComponent();
            var customerList = CommonClass.GetCustomer_AIS140();
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
            logString.Append($"\n********************************* File Processing Started [{DateTime.Now}] USERNAME:{LoginPage.username} SYSTEM NAME : {Environment.MachineName} *************************************\n");
            //logString.Append($"\n********************************* Data Processing Started [{DateTime.Now}] USERNAME:{LoginPage.username} SYSTEM NAME : {Environment.MachineName} *************************************\n");
            //Console.WriteLine($"\n********************************* Data Processing Started [{DateTime.Now}] USERNAME:{LoginPage.username} SYSTEM NAME : {Environment.MachineName} *************************************\n");
            //if (customerList != null && customerList.Count > 0)
            //{
            //    customerList.Insert(0, new CustomerDetails
            //    {
            //        CustomerName = "----Select----",
            //        CustomerID = 0,
            //    });
            //    cbxCustomer.DataSource = customerList;
            //    cbxCustomer.DisplayMember = "CustomerName";
            //    cbxCustomer.ValueMember = "CustomerID";
            //}
            //if (customerList2 != null && customerList2.Count > 0)
            //{
            //    customerList2.Insert(0, new CustomerDetails
            //    {
            //        CustomerName = "----Select----",
            //        CustomerID = 0,
            //    });
            //    cbxCustomer2.DataSource = customerList2;
            //    cbxCustomer2.DisplayMember = "CustomerName";
            //    cbxCustomer2.ValueMember = "CustomerID";
            //}
        }
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
        public static string inputFile2 = string.Empty;
        public static string inputFile3 = string.Empty;
        public static string script_name = string.Empty;
        public static string licenceFile = string.Empty, timestamp = "";
        public static string log_dir = ConfigurationManager.AppSettings["LOG_DIR"];
        public static string Merge_File_DIR = ConfigurationManager.AppSettings["Merge_File_DIR"];
        public static string EncryptDB = ConfigurationManager.AppSettings["Data_Encryption_in_DB"];
        public static int lastInsertedId = 0;
        public static int FileProcessingLotID = 0;
        public string unixTime = DateTime.Now.ToString("yyyyMMdd");
        public static DataTable Process_data = null;
        public static int customerID = 0;
        public static int flag_merger = 0;
        public static int ProfileID = 0;
        public static int customerID2 = 0;
        public static int circleID2 = 0;
        public static int ProfileID2 = 0;
        public static int total_pro_file = 0;
        public static int total_dup_file = 0;
        public static List<int> InsertedHDIDS = new List<int>();
        int fileid = 0;
        int records = 0;
        public static bool IsSingle = true;
       
        StringBuilder logString = new StringBuilder();
        string requiredTag = "";
        List<string> expectedProviders = new List<string>();
        string connectionString = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        bool ValidateSelectedFile(string filePath, int fileIndex)
        {
            
            var parts = cbxProfile.Text.Split('_');

            // Skip AIS140 and customer name
            expectedProviders = parts.Skip(2).ToList();

            if (fileIndex >= expectedProviders.Count)
            {


                MessageBox.Show("This profile does not require this many files.");
                return false;
            }

            requiredTag = expectedProviders[fileIndex].Trim();
            string fileName = "";
            if (requiredTag == "VIL")
            {
                fileName = (filePath).ToUpper();
                requiredTag = "VODAFONE";
            }
            else if (requiredTag.ToUpper() == "AIR")
            {
                fileName = Path.GetFileName(filePath).ToUpper();
                requiredTag = "BHA";
            }
            else
            {

                fileName = Path.GetFileName(filePath).ToUpper();
            }

            if (!fileName.Contains(requiredTag))
            {
                MessageBox.Show(
                    $"Invalid file selected.\n\n" +
                    $"Expected file containing: {requiredTag}\n" +
                    $"But selected: {fileName}",
                    "File Order Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            btnInputFile2.Enabled = expectedProviders.Count >= 2;
            btnInputFile3.Enabled = expectedProviders.Count >= 3;

            return true;
        }


        private void btnInputFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false; // allow only one file
            openFileDialog.Title = "Select a File";
            openFileDialog.Filter = "Haes Files (*.haes)|*.haes";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!ValidateSelectedFile(openFileDialog.FileName, 0)) return;
                // assuming you have a TextBox named txtFile1
                txtInputfile.Text = openFileDialog.FileName;
            }
            
        }

        private void btnInputFile2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false; // allow only one file
            openFileDialog.Title = "Select a File";
            openFileDialog.Filter = "Haes Files (*.haes)|*.haes";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!ValidateSelectedFile(openFileDialog.FileName, 1)) return;
                // assuming you have a TextBox named txtFile2
                txtInputfile2.Text = openFileDialog.FileName;
            }
            
        }

        private void btnInputFile3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false; // allow only one file
            openFileDialog.Title = "Select a File";
            openFileDialog.Filter = "Haes Files (*.haes)|*.haes";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!ValidateSelectedFile(openFileDialog.FileName, 2)) return;
                // assuming you have a TextBox named txtFile2
                txtInputfile3.Text = openFileDialog.FileName;
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
                using (System.Drawing.Brush brush = new SolidBrush(Color.FromArgb(alpha, Color.Black)))
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


            customerID = Convert.ToInt32(cbxCustomer.SelectedValue);
            flag_merger = 2;
            ProfileID = Convert.ToInt32(cbxProfile.SelectedValue);

            timestamp = DateTime.UtcNow.ToString("ddMMyyyyHHmmss");
            logString.Append($"\n1. User selected input 1 :-{txtInputfile.Text}\n");
            logString.Append($"\n1. User selected input 2 :-{txtInputfile2.Text}\n");
           
            // Get inputs
            inputFile = txtInputfile.Text.Trim();
            inputFile2 = txtInputfile2.Text.Trim();
            inputFile3 = txtInputfile3.Text.Trim();
            customer = cbxCustomer.Text.Trim();
            profile = cbxProfile.Text.Trim();
            
            // ✅ Pre-check: Ensure files are not empty / missing
            if (string.IsNullOrEmpty(inputFile) ||
                string.IsNullOrEmpty(inputFile2) )
            {
                MessageBox.Show("Please select all required mca files before submitting.",
                                "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(profile) ||
                string.IsNullOrEmpty(customer) )
            {
                MessageBox.Show("Please Enter all fields before submitting.",
                                "Missing fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            inputFile = EncryptionandDecryption.AESDecrypt_file(inputFile, OFProcessing.file_enc_key);
            if (string.IsNullOrEmpty(inputFile))
            {
                return;
            }
            inputFile2 =  EncryptionandDecryption.AESDecrypt_file(inputFile2, OFProcessing.file_enc_key);
            if (string.IsNullOrEmpty(inputFile2))
            {
                return;
            }
            if (!string.IsNullOrEmpty(inputFile3))
            {
                inputFile3 = EncryptionandDecryption.AESDecrypt_file(inputFile3, OFProcessing.file_enc_key);
                logString.Append($"\n1. User selected input 3 :-{txtInputfile3.Text}\n");
                
                flag_merger = 3;
                if (string.IsNullOrEmpty(inputFile3))
                {
                    
                    return;
                    
                }
            }

            logString.Append($"\n1. User initiated the file processing tool and selected the following input:-\n    Customer : {cbxCustomer.Text}\n    Profile : {cbxProfile.Text}\n");
            panel1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        
        }



        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (flag_merger == 2)
                {
                    mergemca_2(inputFile, inputFile2);
                    upload_log();
                }
                else if (flag_merger == 3)
                {
                    mergemca_3(inputFile, inputFile2, inputFile3);
                    if (customer.ToUpper() == "TAISYS")
                    {
                        mergemca_dynamic(inputFile, inputFile2, inputFile3, "AIRTEL_IMSI,AIRTEL_ICCID,BSNL_IMSI,BSNL_ICCID,VI_IMSI,VI_ICCID,KIC1,KID1,KIK1", "OTA");
                        mergemca_dynamic(inputFile, inputFile2, inputFile3, "AIRTEL_ICCID,AIRTEL_IMSI,BSNL_ICCID,BSNL_IMSI,VI_ICCID,VI_IMSI", "txt");
                        //File.Delete(inputFile);
                        //File.Delete(inputFile2);
                        //File.Delete(inputFile3);
                    }
                    File.Delete(inputFile);
                    File.Delete(inputFile2);
                    File.Delete(inputFile3);

                    upload_log();
                }
                //this.Invoke(new MethodInvoker(delegate
                //{
                //    panel1.Visible = false;
                //}));
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }

        }
        public static string Decrypt(string fileName)
        {
            try
            {
                string lastValue = fileName.Split('_').Last();
                using (Aes aesAlg = Aes.Create())
                {


                    aesAlg.Padding = PaddingMode.PKCS7;
                    aesAlg.Key = Encoding.UTF8.GetBytes(file_enc_key);
                    using (FileStream fsInput = new FileStream(fileName, FileMode.Open))
                    {
                        // Read the IV from the beginning of the encrypted file
                        byte[] iv = new byte[16]; // IV is 16 bytes for AES
                        fsInput.Read(iv, 0, iv.Length);
                        aesAlg.IV = iv;
                        using (CryptoStream csDecrypt = new CryptoStream(fsInput, aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV), CryptoStreamMode.Read))
                        using (FileStream fsOutput = new FileStream(fileName.Replace("_" + lastValue, "." + lastValue.Substring(0, lastValue.Length - 4)), FileMode.Create))
                        {
                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            while ((bytesRead = csDecrypt.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fsOutput.Write(buffer, 0, bytesRead);
                            }

                            return fsOutput.Name;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error decrypting file: {fileName}\n\nError message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }


        public void mergemca_2(string primaryPath, string secondaryPath)
        {

            try
            {
                logString.Append($"    - Outfile Generation Started:\n");
                var primaryColumns = "ICCID,PIN1,PIN2,PUK1,PUK2,ADM1,KIC1,KID1,KIK1,PSK,DEK1,LICENSE_KEY#";
                
                var mergeColumns = "IMSI,ACC,KI,OPC";

                primaryColumns = primaryColumns + ",";
                mergeColumns = mergeColumns + ",";
                var primaryLines = File.ReadAllLines(primaryPath);

                var secondaryLines = File.ReadAllLines(secondaryPath);

                if (primaryLines.Length > 0 &&    primaryLines[0].ToUpper().Contains("ASCII_ICCID"))
                {
                    primaryColumns = "ICCID,PIN1,PIN2,PUK1,PUK2,ADM1,KIC1,KID1,KIK1,PSK,DEK1,ASCII_ICCID,LICENSE_KEY#";
                }
                //deleting all files after reading
                File.Delete(primaryPath);
                File.Delete(secondaryPath);
               
                var beforeICCID = primaryLines[0].Split(
    new string[] { "ICCID" },
    StringSplitOptions.None
)[0];
                primaryColumns = beforeICCID + primaryColumns;
                //var primaryHeader = primaryLines[0].Split(',').Select(c => c.Trim()).ToList();
                //var secondaryHeader = secondaryLines[0].Split(',').Select(c => c.Trim()).ToList();

                var primaryHeader = primaryLines[0].Split(',').Select(c => c.Trim()).ToList();
                var secondaryHeader = secondaryLines[0].Split(',').Select(c => c.Trim()).ToList();



                //var outputHeader = new List<string>();
                //foreach (var col in primaryHeader)
                //{
                //    if (primaryColumns.Contains(col))
                //        outputHeader.Add(col);
                //    else if (mergeColumns.Contains(col))
                //    {
                //        outputHeader.Add(col + "1");
                //        outputHeader.Add(col + "2");
                //        outputHeader.Add(col + "3");
                //    }
                //}
                //var outputLines = new List<string> { string.Join(",", outputHeader) };
                string header_Data = Database.sql_data_value("  select header from  OutFileTemplateHD where ProfileID = '" + ProfileID + "' ", "header");
                var outputLines = new List<string> { header_Data };



                for (int i = 1; i < primaryLines.Length; i++)
                {
                    var primaryValues = primaryLines[i].Split(',').ToList();
                    var secondaryValues = i < secondaryLines.Length ? secondaryLines[i].Split(',').ToList() : new List<string>();

                    var line = new List<string>();
                    var col = "";
                    foreach (var col_new in primaryHeader)
                    {

                        col = col_new.Trim();
                        int indexPrimary = primaryHeader.IndexOf(col);
                        int indexSecondary = secondaryHeader.IndexOf(col);
                        col += ",";
                        if (primaryColumns.Contains(col))
                        {
                            line.Add(indexPrimary < primaryValues.Count ? primaryValues[indexPrimary] : "");
                        }
                        else if (mergeColumns.Contains(col))
                        {
                            line.Add(indexPrimary < primaryValues.Count ? primaryValues[indexPrimary] : "");
                            line.Add(indexSecondary >= 0 && indexSecondary < secondaryValues.Count ? secondaryValues[indexSecondary] : "");
                        }
                    }

                    // Add extra values (unnamed columns)
                    for (int j = primaryHeader.Count; j < primaryValues.Count; j++)
                        line.Add(primaryValues[j]);

                    outputLines.Add(string.Join(",", line));
                }

            
                string cust1 = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(primaryPath))));
                string cust2 = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(secondaryPath))));
               

                var folderPath = "";
                if ((Debugger.IsAttached) || connectionString.Contains("192.168.5.22"))
                {
                    folderPath = Path.Combine("D:\\Productions\\", $"{customer}\\{profile}\\{DateTime.Now.ToString("yyyyMMdd")}");
                }
                else
                {
                    folderPath = Path.Combine("\\\\192.168.27.5\\Productions\\", $"{customer}\\{profile}\\{DateTime.Now:yyyyMMdd}");
                }




                Directory.CreateDirectory(folderPath);

                var outputPath = Path.Combine(
                    folderPath,
                    $"AIS_140_{customer}_{DateTime.Now:yyyyMMdd_HHmmss}_{primaryLines.Length - 1}.mca"
                );
                //File.WriteAllLines(outputPath, outputLines);

                if (batchsize == 0)
                {
                    File.WriteAllLines(outputPath, outputLines);
                    outputPath = EncryptionandDecryption.AESEncrypt_File(outputPath, OFProcessing.file_enc_key);
                    if ((Debugger.IsAttached) || connectionString.Contains("192.168.5.22"))
                    {
                        EncryptionandDecryption.AESDecrypt_file(outputPath, OFProcessing.file_enc_key);
                    }
                }
                else
                {
                    var fileList = SplitFileWithHeader(outputLines, batchsize, folderPath, $"AIS_140_{customer}_{DateTime.Now:yyyyMMdd_HHmmss}");

                    logString.Append($"\nFiles Split Successfully. Total Files: {fileList.Count}\n");

                    foreach (var file in fileList)
                    {
                        string encryptedFile = EncryptionandDecryption.AESEncrypt_File(file, OFProcessing.file_enc_key);

                        logString.Append($"\nEncrypted: {Path.GetFileName(encryptedFile)}");

                        if ((Debugger.IsAttached) || connectionString.Contains("192.168.5.22"))
                        {
                            EncryptionandDecryption.AESDecrypt_file(encryptedFile, OFProcessing.file_enc_key);
                        }
                    }
                }
            


                //logString.Append($"\nFile Merged Succesfully\n");
               
                //logString.Append($"\nFile encrypted Succesfully Filename : {outputPath}\n");
                logString.Append($"\n**************************************[Logging Out] File Processing Tool is closing [{DateTime.Now}] **************************************\n");


                //upload_log();

                //MessageBox.Show($"File Merged Succesfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error merging files: {ex.Message}");
                //MessageBox.Show($"Error merging files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return;
            }

            logString.Clear();
        }

        //(AllCustomers thre input file necessary)
        public void mergemca_3(string primaryPath, string secondaryPath, string thirdfilePath)
        {

            try
            {
                logString.Append($"    - Outfile Generation Started:\n");
                var primaryColumns = "ICCID,PIN1,PIN2,PUK1,PUK2,ADM1,KIC1,KID1,KIK1,PSK,DEK1,LICENSE_KEY#";
                var mergeColumns = "IMSI,ACC,KI,OPC";
                primaryColumns = primaryColumns + ",";
                mergeColumns = mergeColumns + ",";
                var primaryLines = File.ReadAllLines(primaryPath);


                if (primaryLines.Length > 0 && primaryLines[0].ToUpper().Contains("ASCII_ICCID"))
                {
                    primaryColumns = "ICCID,PIN1,PIN2,PUK1,PUK2,ADM1,KIC1,KID1,KIK1,PSK,DEK1,ASCII_ICCID,LICENSE_KEY#";
                }


                var secondaryLines = File.ReadAllLines(secondaryPath);
                var thirdfileLines = File.ReadAllLines(thirdfilePath);

                //////deleting all files qafter reading
                //if (customer.ToUpper() != "TAISYS")
                //{
                //    File.Delete(primaryPath);
                //    File.Delete(secondaryPath);
                //    File.Delete(thirdfilePath);

                //}


                var beforeICCID = primaryLines[0].Split(
new string[] { "ICCID" },
StringSplitOptions.None
)[0];
                primaryColumns = beforeICCID + primaryColumns;
                var primaryHeader = primaryLines[0].Split(',').Select(c => c.Trim()).ToList();
                var secondaryHeader = secondaryLines[0].Split(',').Select(c => c.Trim()).ToList();
                var thirdfileHeader = thirdfileLines[0].Split(',').Select(c => c.Trim()).ToList();

                //var outputHeader = new List<string>();
                //foreach (var col in primaryHeader)
                //{
                //    if (primaryColumns.Contains(col))
                //        outputHeader.Add(col);
                //    else if (mergeColumns.Contains(col))
                //    {
                //        outputHeader.Add(col + "1");
                //        outputHeader.Add(col + "2");
                //        outputHeader.Add(col + "3");
                //    }
                //}
                //var outputLines = new List<string> { string.Join(",", outputHeader) };
                string header_Data = Database.sql_data_value("  select header from  OutFileTemplateHD where ProfileID = '" + ProfileID + "' ", "header");
                var outputLines = new List<string> { header_Data };



                for (int i = 1; i < primaryLines.Length; i++)
                {
                    var primaryValues = primaryLines[i].Split(',').ToList();
                    var secondaryValues = i < secondaryLines.Length ? secondaryLines[i].Split(',').ToList() : new List<string>();
                    var thirdfileValues = i < thirdfileLines.Length ? thirdfileLines[i].Split(',').ToList() : new List<string>();

                    var line = new List<string>();
                    var col = "";
                    foreach (var col_new in primaryHeader)
                    {
                        col = col_new;
                        int indexPrimary = primaryHeader.IndexOf(col);
                        int indexSecondary = secondaryHeader.IndexOf(col);
                        int indexthirdfile = thirdfileHeader.IndexOf(col);
                        col += ",";
                        if (primaryColumns.Contains(col))
                        {
                            line.Add(indexPrimary < primaryValues.Count ? primaryValues[indexPrimary] : "");
                        }
                        else if (mergeColumns.Contains(col))
                        {
                            line.Add(indexPrimary < primaryValues.Count ? primaryValues[indexPrimary] : "");
                            line.Add(indexSecondary >= 0 && indexSecondary < secondaryValues.Count ? secondaryValues[indexSecondary] : "");
                            line.Add(indexthirdfile >= 0 && indexthirdfile < thirdfileValues.Count ? thirdfileValues[indexthirdfile] : "");
                        }
                    }

                    // Add extra values (unnamed columns)
                    for (int j = primaryHeader.Count; j < primaryValues.Count; j++)
                        line.Add(primaryValues[j]);

                    outputLines.Add(string.Join(",", line));
                }
                string cust1 = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(primaryPath))));
                string cust2 = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(secondaryPath))));
                string cust3 = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(thirdfilePath))));

                var folderPath = "";
                if ((Debugger.IsAttached) || connectionString.Contains("192.168.5.22"))
                {
                    folderPath = Path.Combine("D:\\Productions\\", $"{customer}\\{profile}\\{DateTime.Now.ToString("yyyyMMdd")}");
                    //folderPath = Path.Combine("B:\\DATA-FILES\\DATATOOL\\Telco_Customers\\Taisys Input\\Mapping files\\testing\\");
                }
                else
                {
                    folderPath = Path.Combine("\\\\192.168.27.5\\Productions\\", $"{customer}\\{profile}\\{DateTime.Now:yyyyMMdd}");
                }




                Directory.CreateDirectory(folderPath);

                var outputPath = Path.Combine(
                    folderPath,
                    $"AIS_140_{customer}_{DateTime.Now:yyyyMMdd_HHmmss}_{primaryLines.Length - 1}.mca"

                );

                if (batchsize == 0)
                {
                    File.WriteAllLines(outputPath, outputLines);
                    logString.Append($"\nFile Merged Succesfully\n");
                    outputPath = EncryptionandDecryption.AESEncrypt_File(outputPath, OFProcessing.file_enc_key);
                    if ((Debugger.IsAttached) || connectionString.Contains("192.168.5.22"))
                    {
                        EncryptionandDecryption.AESDecrypt_file(outputPath, OFProcessing.file_enc_key);
                    }

                }
                else
                {

                    var fileList = SplitFileWithHeader(outputLines, batchsize, folderPath, $"AIS_140_{customer}_{DateTime.Now:yyyyMMdd_HHmmss}");
                    logString.Append($"\nFiles Split Successfully. Total Files: {fileList.Count}\n");

                    foreach (var file in fileList)
                    {
                        string encryptedFile = EncryptionandDecryption.AESEncrypt_File(file, OFProcessing.file_enc_key);

                        logString.Append($"\nEncrypted: {Path.GetFileName(encryptedFile)}");

                        if ((Debugger.IsAttached) || connectionString.Contains("192.168.5.22"))
                        {
                            EncryptionandDecryption.AESDecrypt_file(encryptedFile, OFProcessing.file_enc_key);
                        }

                    }
                }

               
               
                logString.Append($"\nFile encrypted Succesfully Filename : {outputPath}\n");
                logString.Append($"\n**************************************[Logging Out] File Processing Tool is closing [{DateTime.Now}] **************************************\n");
               

                //upload_log();

                //MessageBox.Show($"File Merged Succesfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error merging files: {ex.Message}");
                //MessageBox.Show($"Error merging files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return;
            }

            logString.Clear();
        }


        public static List<string> SplitFileWithHeader(List<string> outputLines, int batchSize, string folderPath, string baseFileName)
        {
            List<string> generatedFiles = new List<string>();

            if (outputLines == null || outputLines.Count <= 1)
                return generatedFiles;

            string header = outputLines[0]; // First line is header
            int totalRecords = outputLines.Count - 1; // excluding header

            int fileIndex = 1;

            for (int i = 1; i <= totalRecords; i += batchSize)
            {
                List<string> batchLines = new List<string>();

                // Add header in each file
                batchLines.Add(header);

                // Add batch data
                var batchData = outputLines.Skip(i).Take(batchSize);
                batchLines.AddRange(batchData);

                // ✅ Padding file index (0001, 0002...)
                string paddedIndex = fileIndex.ToString("D4");
                // ✅ Padding file index (0001, 0002...)
                string paddedqty = (batchLines.Count - 1).ToString("D4");

                // File name with record count
                string filePath = Path.Combine(
                    folderPath,
                    $"{baseFileName}_{paddedIndex}_{paddedqty}.mca"
                );

                File.WriteAllLines(filePath, batchLines);
                generatedFiles.Add(filePath); // ✅ store file path

                fileIndex++;

            }

            return generatedFiles;
        }
        public void mergemca_dynamic(string primaryPath, string secondaryPath, string thirdfilePath, string headerTemplatePath, string extension)
        {
            try
            {
                logString.Append($"    - Outfile Generation Started:\n");

                var primaryLines = File.ReadAllLines(primaryPath);
                var secondaryLines = File.ReadAllLines(secondaryPath);
                var thirdLines = File.ReadAllLines(thirdfilePath);

                //// Header template file
                //var templateHeader = File.ReadLines(headerTemplatePath).First().Split(',').Select(x => x.Trim()).ToList();
                var templateHeader = headerTemplatePath.Split(',').Select(x => x.Trim()).ToList();

                var primaryHeader = primaryLines[0].Split(new[] { "#VN=" }, StringSplitOptions.None).Last().Split(',').Select(x => x.Trim()).ToList();
                var secondaryHeader = secondaryLines[0].Split(new[] { "#VN=" }, StringSplitOptions.None).Last().Split(',').Select(x => x.Trim()).ToList();
                var thirdHeader = thirdLines[0].Split(new[] { "#VN=" }, StringSplitOptions.None).Last().Split(',').Select(x => x.Trim()).ToList();
                //var primaryHeader = primaryLines[0].Split(',').Select(x => x.Trim()).ToList();
                //var secondaryHeader = secondaryLines[0].Split(',').Select(x => x.Trim()).ToList();
                //var thirdHeader = thirdLines[0].Split(',').Select(x => x.Trim()).ToList();

                var outputLines = new List<string>();
                if (extension == "OTA")
                {
                    outputLines.Add("Var_out: AIRTEL IMSI / AIRTEL ICCID / BSNL IMSI / BSNL ICCID / VI IMSI / Vi ICCID / KIC / KID / KIK");
                    outputLines.Add("");
                }
                else if (extension == "txt")
                { outputLines.Add("AIRTEL ICCID;AIRTEL IMSI;BSNL ICCID;BSNL IMSI;VI ICCID;VI IMSI"); }



                for (int i = 1; i < primaryLines.Length; i++)
                {
                    var p = primaryLines[i].Split(',');
                    var s = i < secondaryLines.Length ? secondaryLines[i].Split(',') : new string[0];
                    var t = i < thirdLines.Length ? thirdLines[i].Split(',') : new string[0];

                    List<string> line = new List<string>();

                    foreach (var col in templateHeader)
                    {
                        string operatorName = "";
                        string fieldName = "";

                        if (col.Contains("_"))
                        {
                            var parts = col.Split('_');
                            operatorName = parts[0].ToUpper();
                            fieldName = parts[1].ToUpper();
                        }
                        else
                        {
                            fieldName = col.ToUpper();
                        }

                        string value = "";

                        // AIRTEL → primary file
                        if (operatorName == "AIRTEL")
                        {
                            int idx = primaryHeader.IndexOf(fieldName);
                            if (idx >= 0 && p.Length > idx)
                                value = p[idx];
                        }
                        // BSNL → secondary file
                        else if (operatorName == "BSNL")
                        {
                            int idx = secondaryHeader.IndexOf(fieldName);
                            if (idx >= 0 && s.Length > idx)
                                value = s[idx];
                        }
                        // VI → third file
                        else if (operatorName == "VI")
                        {
                            int idx = thirdHeader.IndexOf(fieldName);
                            if (idx >= 0 && t.Length > idx)
                                value = t[idx];
                        }
                        else
                        {
                            // Fields like KIC,KID,KIK come from first file
                            int idx = primaryHeader.IndexOf(fieldName);
                            if (idx >= 0 && p.Length > idx)
                                value = p[idx];
                        }

                        if (fieldName == "ICCID")
                            value = NibbleSwap(value).Replace("F", "");
                        else if (fieldName == "IMSI" && value.Length > 3)
                            value = NibbleSwap(value).Substring(3);


                        line.Add(value);
                    }
                    if (extension == "OTA")
                    {
                        outputLines.Add(string.Join(" ", line));
                    }
                    else if (extension == "txt")
                    {
                        outputLines.Add(string.Join(";", line));
                    }
                }

                    var folderPath = "";

                if ((Debugger.IsAttached) || connectionString.Contains("192.168.5.22"))
                {
                    //folderPath = @"B:\DATA-FILES\DATATOOL\Telco_Customers\Taisys Input\Mapping files\testing\";
                    folderPath = Path.Combine("D:\\Data_Gen\\Output\\", $"{customer}\\{profile}\\{DateTime.Now.ToString("yyyyMMdd")}");
                }
                else
                {
                    folderPath = Path.Combine(@"\\192.168.27.5\Data_Gen\Output\", $"{customer}\\{profile}\\{DateTime.Now:yyyyMMdd}");
                }

                Directory.CreateDirectory(folderPath);

                var outputPath = Path.Combine(
                    folderPath,
                    $"AAirtel+BSNL+VI_Mapping_{timestamp}.{extension}"
                );

                //File.WriteAllLines(outputPath, outputLines);
                File.WriteAllText(outputPath, string.Join(Environment.NewLine, outputLines));




                logString.Append($"\nFile Merged Successfully\n");

                bool isDev = Debugger.IsAttached || connectionString.Contains("192.168.5.22");
                if (!isDev)
                {
                    outputPath = EncryptionandDecryption.AESEncrypt_File(outputPath, OFProcessing.file_enc_key);
                }

                logString.Append($"\nFile encrypted Successfully Filename : {outputPath}\n");

                
            }
            catch (Exception ex)
            {
                throw new Exception($"Error merging files: {ex.Message}");
            }

            logString.Clear();
        }
      

        public string NibbleSwap(string s)
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
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            panel1.Visible = false;

            if (e.Result is Exception ex)
            {
                MessageBox.Show(ex.Message,
                                "Processing Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("File processing completed successfully.",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                this.Close();
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
                throw new Exception($"Error merging files: {ex.Message}");
                //MessageBox.Show("Error in log uploadation : " + ex.Message);
            }
        }


      

  
    
     



   
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
      
        private void cbxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbxCustomer_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbxCustomer.SelectedIndex > 0)
            {

                // ✅ Clear dependent controls first

                var customerProfile = CommonClass.GetCustomerProfileList(Convert.ToInt32(cbxCustomer.SelectedValue));

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

        private void license_file_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false; // allow only one file
            openFileDialog.Title = "Select a File";
            openFileDialog.Filter = "All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!ValidateSelectedFile(openFileDialog.FileName, 2)) return;
                // assuming you have a TextBox named txtFile2
                txtInputfile3.Text = openFileDialog.FileName;
            }
        }

        private void cbxProfile_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
           
            logString.Clear();
            this.Close();
        }

    }
}
