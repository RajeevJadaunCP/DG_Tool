
using CardPrintingApplication;
using ClosedXML.Excel;
using DG_Tool.HelperClass;
using DG_Tool.Models;
using DG_Tool.WinForms.Authentication;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DG_Tool.WinForms.OutputFile
{
    public partial class Euicc_Data_Verification : Form
    {
        BackgroundWorker bgWorker = new BackgroundWorker();
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
        public static string licenceFile = string.Empty;
        public static string log_dir = ConfigurationManager.AppSettings["LOG_DIR"];
        public static string Merge_File_DIR = ConfigurationManager.AppSettings["Merge_File_DIR"];
        public static string EncryptDB = ConfigurationManager.AppSettings["Data_Encryption_in_DB"];
        public static int lastInsertedId = 0;
        public static int FileProcessingLotID = 0;
        public string unixTime = DateTime.Now.ToString("yyyyMMdd");
        public static DataTable Process_data = null;
        public static int customerID = 0;
        public static int circleID = 0;
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


        public Euicc_Data_Verification()
        {
            InitializeComponent();
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
            var customerList = CommonClass.GetCustomer();
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

        private void btnInputFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false; // allow only one file
            openFileDialog.Title = "Select a File";
            openFileDialog.Filter = "Haes Files (*.haes)|*.haes";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // assuming you have a TextBox named txtFile1
                txtInputfile.Text = openFileDialog.FileName;
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

            logString.Append($"\n1. User selected input1 :-{txtInputfile.Text}\n");
           
            logString.Append($"\n1. User initiated the file validation tool and selected the following input:-{txtInputfile.Text} \n");
            // Get inputs
            inputFile = txtInputfile.Text.Trim();
           
            
            // ✅ Pre-check: Ensure files are not empty / missing
            if (string.IsNullOrEmpty(inputFile)) 
            {
                MessageBox.Show("Please select EIS file before submitting.",
                                "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            

            
            inputFile = EncryptionandDecryption.AESDecrypt_file(inputFile, OFProcessing.file_enc_key);
            if (string.IsNullOrEmpty(inputFile))
            {
                return;
            }
            if (!bgWorker.IsBusy)
            {
                panel1.Visible = true; // show loading panel
                bgWorker.RunWorkerAsync(inputFile); // pass file path
            }

        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string primaryPath = e.Argument.ToString();

            try
            {
                logString.Append($"\n1.Merging of files started\n");

                var primaryLines = File.ReadAllLines(primaryPath);
                File.Delete(primaryPath);

                for (int i = 1; i < primaryLines.Length; i++)
                {
                    var primaryValues = primaryLines[i].Split(',').ToList();
                    string rootCaHex = primaryValues[0];
                    string subCaHex = primaryValues[1];
                    string euiic_cert_hex = primaryValues[2];

                    byte[] subCaBytes = HexToBytes(subCaHex);
                    byte[] rootCaBytes = HexToBytes(rootCaHex);
                    byte[] euiccCaBytes = HexToBytes(euiic_cert_hex);

                    var subCaCert = new X509Certificate2(subCaBytes);
                    var rootCaCert = new X509Certificate2(rootCaBytes);
                    var euiccCaCert = new X509Certificate2(euiccCaBytes);

                    this.Invoke((MethodInvoker)delegate
                    {
                        PrintCertificateDetails(euiccCaCert);
                    });

                    bool isValid = false;

                    Console.WriteLine("\n***************Validation 3 of EUICC CA Cert*****************");
                    this.Invoke((MethodInvoker)delegate
                    {

                        VerifyKeyIdentifiers(subCaCert, euiccCaCert);
                    });

                    Console.WriteLine("\n***************Validation 4 of EUICC CA Cert*****************");
                    isValid = VerifyCertificate(euiccCaCert, subCaCert);
                    if (isValid)
                        e.Result = "EUICC is valid and signed by SUB CA.";
                    else
                        e.Result = "EUICC is NOT signed by SUB CA.";

                    logString.Append($"\nEuicc Data validated Successfully Filename : {primaryPath}\n");
                }

                logString.Append($"\n**************************************[Logging Out] File Processing Tool is closing [{DateTime.Now}] **************************************\n");
            }
            catch (Exception ex)
            {
                e.Result = $"Error merging files: {ex.Message}";
                File.Delete(primaryPath);
            }
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            panel1.Visible = false;
            if (e.Result != null && e.Result.ToString().StartsWith("Error"))
            {
                MessageBox.Show(e.Result.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(e.Result?.ToString() ?? "Validation Done.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            logString.Clear();
        }


        //private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    mergemca(inputFile, inputFile2, inputFile3);
        //    this.Invoke(new MethodInvoker(delegate
        //    {
        //        panel1.Visible = false;
        //    }));

        //}

        public static byte[] HexToBytes(string hex)
        {
            if (hex.Length % 2 != 0)
                throw new ArgumentException("Invalid hex string length.");

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }
        //public void Validator(string primaryPath)
        //{

        //    try
        //    {
        //        logString.Append($"\n1.Merging of files started\n");
                

        //        var primaryLines = File.ReadAllLines(primaryPath);
        //        File.Delete(primaryPath);

        //        for (int i = 1; i < primaryLines.Length; i++)
        //        {
        //            var primaryValues = primaryLines[i].Split(',').ToList();
        //            string rootCaHex = primaryValues[0];
        //            string subCaHex = primaryValues[1];  // your cert hex
        //            string euiic_cert_hex = primaryValues[2];
        //            // Convert hex → byte[]
        //            byte[] subCaBytes = HexToBytes(subCaHex);
        //            byte[] rootCaBytes = HexToBytes(rootCaHex);
        //            byte[] euiccCaBytes = HexToBytes(euiic_cert_hex);

        //            // Create X509Certificate2 from byte[]
        //            var subCaCert = new X509Certificate2(subCaBytes);
        //            var rootCaCert = new X509Certificate2(rootCaBytes);
        //            var euiccCaCert = new X509Certificate2(euiccCaBytes);


        //            //var subCaCert = new X509Certificate2(@"C:\Temp\HsmOutput\CERT_EUM_ECDSA_Cert.pem");
        //            //var rootCaCert = new X509Certificate2(@"C:\Temp\HsmOutput\CERT_CI_ECDSA_RootCA.pem");




        //            //PrintCertDetails("Sub CA", subCaCert);
        //            //PrintCertDetails("Root CA", rootCaCert);
        //            Console.WriteLine("\n***************Details of EUICC CA Cert*****************");
        //            //txtoutput.Text += (euiccCaCert);
        //            PrintCertificateDetails(euiccCaCert);
        //            //Console.WriteLine("\n***************Details of SUB CA Cert*****************");
        //            //txtoutput.Text += (subCaCert);
        //            //Console.WriteLine("\n\n***************Details of Root CA Cert*****************");
        //            //txtoutput.Text += (rootCaCert);
        //            ////PrintCertificateDetails(subCaCert);

        //            //VerifySubCaCertificate(subCaCert, rootCaCert)
        //            //VerifySubCaWithRoot(rootCaCert, subCaCert);

        //            bool isValid = false;

        //            ////Console.WriteLine("\n***************Validation 3 of SUB CA Cert*****************");
        //            //VerifyKeyIdentifiers(rootCaCert, subCaCert);

        //            ////Console.WriteLine("\n***************Validation 4 of SUB CA Cert*****************");
        //            //isValid = VerifyCertificate(subCaCert, rootCaCert);
        //            //if (isValid)
        //            //    txtoutput.Text += (" SubCA is valid and signed by Root CA.");
        //            //else
        //            //    txtoutput.Text += (" SubCA is NOT signed by Root CA.");






        //            Console.WriteLine("\n***************Validation 3 of EUICC CA Cert*****************");
        //            VerifyKeyIdentifiers(subCaCert, euiccCaCert);

        //            Console.WriteLine("\n***************Validation 4 of EUICC CA Cert*****************");
        //            isValid = VerifyCertificate(euiccCaCert, subCaCert);
        //            if (isValid)
        //                txtoutput.Text += (" , EUICC is valid and signed by SUB CA.");
        //            else
        //                txtoutput.Text += (" , EUICC is NOT signed by SUB CA.");
        //            logString.Append($"\nEuicc Data validated Succesfully Filename : {primaryPath}\n");
        //        }
        //        logString.Append($"\n**************************************[Logging Out] File Processing Tool is closing [{DateTime.Now}] **************************************\n");

        //        panel1.Visible = false;
        //        //upload_log();
        //        MessageBox.Show($"Euicc Data Validated Succesfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {

        //        panel1.Visible = false;
        //        MessageBox.Show($"Error merging files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        File.Delete(primaryPath);
                
        //        return;
        //    }

        //    logString.Clear();
        //}

        private void PrintCertificateDetails(X509Certificate2 cert)
        {
            txtoutput.Text +=($"\n  Subject: {cert.Subject}");
            txtoutput.Text +=($"  Issuer: {cert.Issuer}");
            //txtoutput.Text +=($"  Serial Number: {cert.SerialNumber}");
            //txtoutput.Text +=($"  Valid From: {cert.NotBefore}");
            //txtoutput.Text +=($"  Valid Until: {cert.NotAfter}");
            txtoutput.Text +=($"  Signature Algorithm: {cert.SignatureAlgorithm.FriendlyName}");
            //txtoutput.Text +=($"  Public Key Algorithm: {cert.PublicKey.Oid.FriendlyName}");
            txtoutput.Text +=($"  Fingerprint (SHA256): {cert.GetCertHashString(HashAlgorithmName.SHA256)}");
            txtoutput.Text += ($"  Fingerprint (SHA1): {cert.Thumbprint}");

            // Use Bouncy Castle to get extension details.
            //var bcCert = DotNetUtilities.FromX509Certificate(cert);
            //PrintExtensions(bcCert);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //StopBuffering();
        }
        private void VerifyKeyIdentifiers(X509Certificate2 rootCaCert, X509Certificate2 subCaCert)
        {
            var bcRootCert = DotNetUtilities.FromX509Certificate(rootCaCert);
            var bcSubCert = DotNetUtilities.FromX509Certificate(subCaCert);

            // CORRECTED: Use GetInstance() to correctly parse the generic ASN.1 object.
            // Assuming bcRootCert and bcSubCert are BouncyCastle.X509.X509Certificate

            // Root SKID
            var rootSkidExt = SubjectKeyIdentifier.GetInstance(
                X509ExtensionUtilities.FromExtensionValue(
                    bcRootCert.GetExtensionValue(X509Extensions.SubjectKeyIdentifier)));

            // Sub AKID
            var subAkidExt = AuthorityKeyIdentifier.GetInstance(
                X509ExtensionUtilities.FromExtensionValue(
                    bcSubCert.GetExtensionValue(X509Extensions.AuthorityKeyIdentifier)));

            if (rootSkidExt != null && subAkidExt != null)
            {
                var rootSkid = rootSkidExt.GetKeyIdentifier();
                var subAkid = subAkidExt.GetKeyIdentifier();

                if (rootSkid.SequenceEqual(subAkid))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    txtoutput.Text +=("SUCCESS: The Authority Key Identifier (AKID) of the EUICC DATA CERT MATCHES the Subject Key Identifier (SKID) of the EUM CERT.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    txtoutput.Text += ("FAILURE: The AKID of the EUICC DATA CERT does NOT match the SKID of the EUM CERT.");
                }
            }
            else
            {
                Console.WriteLine("Could not find key identifier extensions on one or both certificates.");
            }
            Console.ResetColor();
        }

        static bool VerifyCertificate(X509Certificate2 childCert, X509Certificate2 parentCert)
        {
            try
            {
                var bcChild = DotNetUtilities.FromX509Certificate(childCert);
                var bcParent = DotNetUtilities.FromX509Certificate(parentCert);

                // Use parent's public key to verify child's signature
                bcChild.Verify(bcParent.GetPublicKey());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Signature verification failed: {ex.Message}");
                return false;
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









        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
            logString.Clear();
            this.Close();
        }

    }
}
