using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Threading;
using System.Activities.Statements;
using DG_Tool.Models;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using CardPrintingApplication;
using static CardPrintingApplication.EncryptionandDecryption;
using System.Activities;

namespace DG_Tool.WinForms.OutputFile
{
    public partial class DataGenProcessList : Form
    {


        StringBuilder logString = new StringBuilder();
        string Outputfilepath = Directory.GetCurrentDirectory();
        string Outfilelocation = "", headerfilepath = "";
        int fileid = 0;
        //public static DateTime foo = DateTime.Now;
        //public static long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();

        //public static string unixTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");

        string random_four = "1110";
        string random_eight = "11111110";

        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        int records = 0;
        string first_imsi = string.Empty;
        string last_imsi = string.Empty;
        string first_icicid = string.Empty;
        string last_icicid = string.Empty;
        DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();
        Random RNG32 = new Random();
        Random RNG16 = new Random();
        Random random16 = new Random();
        Random random8 = new Random();

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


        public string Random4digits()
        {
            random_four = (Int32.Parse(random_four) + 1).ToString();
            return random_four;

        }

        public string Random8digits()
        {
            //Random random = new Random();
            //char[] ch = "0123456789".ToCharArray();
            //string random_number_string = "";
            //for (int i = 0; i < 1; i++)
            //{

            //    for (int j = 0; random_number_string.Length != 8; j++)
            //    {
            //        int x = random.Next(0, ch.Length);
            //        random_number_string += ch.GetValue(x).ToString();

            //    }

            //    ////richTextBox1.Text += random_number_string;
            //    ////richTextBox1.Text += "\n";

            //}
            ////Thread.Sleep(1000);
            //return random_number_string;

            random_eight = (Int64.Parse(random_eight) + 1).ToString();
            return random_eight;


        }

        public string acc(string imisi_acc)
        {

            int decNum = 0;

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


            hexNum = hexNum.PadRight(4, '0');
            return hexNum;


        }
        public string nibble_swapped(string s)
        {
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
                    revs = "INVALID";
                }
            }
            else
                revs = "INVALID";

            return revs;
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
                    pad = "INVALID";
                }
            }
            else
                pad = "INVALID";

            return pad;
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

        public int insert_data(string id, string name, string value, string type, string file_name_inp)
        {
            using (SqlConnection con1 = new SqlConnection(connectionString))
            {

                //MessageBox.Show(file_name_inp);
                SqlDataReader reader = null;
                using (SqlCommand cmd = new SqlCommand("usp_Insert_first_record", con1))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DataGenProcessHDID", fileid);
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

        public int update(int record_id, string varname, string value)
        {
            int pass = 0;
            string value_1 = value + "test";
            string enc_tag = ConfigurationManager.AppSettings["Data_Encryption_in_DB"];
            using (SqlConnection con1 = new SqlConnection(connectionString))
            {
                string Query = "UPDATE DataGenProcessDataRecord  SET  " + @varname + "   =  @value  WHERE DataGenProcessDataRecordID = @record_id  AND  DataGenProcessHDID =  @fileid  ";
                //value = "XXX" + value + "XXXX";
                using (SqlCommand com1 = new SqlCommand(Query, con1))
                {
                    com1.Parameters.AddWithValue("@varname", varname);
                    //com1.Parameters.AddWithValue("@fileid", EncryptString(value, "thisIsASecretKey"));
                    com1.Parameters.AddWithValue("@fileid", fileid);
                    if (enc_tag == "1")
                    { com1.Parameters.AddWithValue("@value", EncryptString("3004455532FFFFFF", value)); }
                    else
                    { com1.Parameters.AddWithValue("@value", value); }
                    com1.Parameters.AddWithValue("@record_id", record_id);
                    //MessageBox.Show(record_id.ToString() + ' ' + varname + ' ' + value);
                    try
                    {
                        con1.Open();
                        com1.ExecuteNonQuery();
                        pass = 1;
                        con1.Close();
                        //MessageBox.Show("Data Saved Successfully!");
                    }
                    catch (Exception exe)
                    {
                        pass = 0;
                        MessageBox.Show(exe.Message);
                    }
                }

            }
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
        public DataGenProcessList()
        {
            InitializeComponent();
            lblCustomer.Text = OFProcessing.customer;
            lblCircle.Text = OFProcessing.circle;
            lblProfile.Text = OFProcessing.profile;
            lblInputFile.Text = "Input File : " + OFProcessing.inputFile;
            lblLicenceFile.Text = "License File : " + OFProcessing.licenceFile;
            GetGenProcessList();
            getfilenameandid();

            //foreach (DataGridViewRow row in dataGVProcessList.Rows)

            //{
            //    if (row.Cells[7].Value.Equals("S"))
            //    {
            //        row.DefaultCellStyle.BackColor = Color.Red;
            //    }
            //}

            //string searchValue = "s";
            //int rowIndex = -1;
            //foreach (DataGridViewRow row in dataGVProcessList.Rows)
            //{
            //    if (row.Cells["OutFlileStatus"].Value != null) // Need to check for null if new row is exposed
            //    {
            //        if (row.Cells["OutFlileStatus"].Value.ToString().Equals(searchValue))
            //        {
            //            rowIndex = row.Index;
            //            break;
            //        }
            //    }
            //}


        }
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
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGVProcessList.DataSource = dt;





                    if (dataGVProcessList.Columns.Contains(dgvButton.Name = "Process"))
                    {

                    }
                    else
                    {
                        dgvButton.FlatStyle = FlatStyle.System;

                        dgvButton.HeaderText = "Process";
                        dgvButton.Name = "Process";
                        dgvButton.UseColumnTextForButtonValue = true;
                        dgvButton.Text = "Process";
                        dgvButton.Visible = false;
                        dataGVProcessList.Columns.Add(dgvButton);
                    }


                }
            }
        }
        //private void GetGenProcessList_Refresh_list()
        //{
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        con.Open();
        //        using (SqlCommand cmd = new SqlCommand("usp_DataGenProcessHDFile", con))
        //        {

        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@InputfiletId", fileid);
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            DataTable dt = new DataTable();
        //            da.Fill(dt);

        //            dataGVProcessList.DataSource = dt;
        //        }
        //    }
        //}
        private void dataGVProcessList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = dataGVProcessList.CurrentCell.ColumnIndex;
            string columnName = dataGVProcessList.Columns[columnIndex].Name;
            if (columnIndex == 7 && columnName.Equals("FilePath"))
            {
                string filePath = dataGVProcessList.SelectedCells[0].Value.ToString();

                if (File.Exists(filePath))
                {
                    //Code to go to file location
                    //Process.Start("explorer.exe", "/select, " + filePath);

                    Process.Start("explorer.exe", filePath);
                }

                else
                {
                    MessageBox.Show("File not found: ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (columnName.Equals("Process"))

            {
                DataGridViewRow row = dataGVProcessList.Rows[e.RowIndex];
                string txtID = row.Cells[8].Value.ToString().Trim();

                string caseSwitch = txtID;
                switch (caseSwitch)
                {
                    case "HLR File":
                        MessageBox.Show(txtID);
                        HLR();
                        break;
                    case "HSS File":
                        MessageBox.Show(txtID);
                        HSS();
                        break;
                    case "DDMMYY File":
                        MessageBox.Show(txtID);
                        DDMMYY();
                        break;
                    case "DSA File":
                        MessageBox.Show(txtID);
                        DSA();
                        break;
                    case "CPS File":
                        MessageBox.Show(txtID);
                        CPS();
                        break;
                    case "MCA File":
                        MessageBox.Show(txtID);
                        MCA();
                        break;

                }


            }
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
                        cmd.Parameters.AddWithValue("@CustProfile_ID", OFProcessing.ProfileID);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            filename1.Add(reader["FilePath"].ToString());
                            fileid = Int32.Parse(reader["DataGenProcessHDID"].ToString());
                        }
                        filename_12 = filename1[1];

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

        private void UpdateProcessHDFile(string processFor, int fileID, string fileName, string filePath)
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
                //MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
                //                        "Error",
                //                        MessageBoxButtons.OK,
                //                        MessageBoxIcon.Error
                //                        );
            }

        }

        private void UpdateInputFileSatus(string processFor, int fileID_1)
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
                //MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
                //                        "Error",
                //                        MessageBoxButtons.OK,
                //                        MessageBoxIcon.Error
                //                        );
            }

        }

        private void dataGVProcessList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow Myrow in dataGVProcessList.Rows)
                {            //Here 2 cell is target value and 1 cell is Volume

                    if (Myrow.Cells[10].Value == null)
                    {
                        Myrow.Cells[10].Value = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(Myrow.Cells[10].Value.ToString()) && Myrow != null)
                    {
                        if (Myrow.Cells[10].Value.ToString().Equals("S"))// Or your condition 
                        {
                            Myrow.DefaultCellStyle.BackColor = Color.Red;
                        }
                        else
                        {
                            Myrow.DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void DataGenProcessList_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(Outputfilepath + "\\Output") == false)
            {
                Directory.CreateDirectory(Outputfilepath + "\\Output");

            }
            //HEADER
            if (Directory.Exists(Outputfilepath + "\\HEADER") == false)
            {
                Directory.CreateDirectory(Outputfilepath + "\\HEADER");

            }
            Outfilelocation = Outputfilepath + "\\Output";
            headerfilepath = Outputfilepath + "\\HEADER";

        }
        private void btnProcessAll_Click(object sender, EventArgs e)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            btnProcessAll.Enabled = false;

            dgvButton.Visible = true;

            //List<string> r4_data = new List<string>();
            //List<string> r8_data = new List<string>();
            string r4_data = "", r8_data = "";
            int r4_data_count = 0, r8_data_count = 0;
            List<string> r4_data_list = new List<string>();

            List<string> r8_data_list = new List<string>();

            string constr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlConnection con = new SqlConnection(constr);
            string filename_2 = getfilenameandid();
            int list_4 = 0, list_8 = 0;
            String Query0 = "SELECT * FROM InPutDataTemplate WHERE CustID = " + OFProcessing.customerID + " and ProfileID =" + OFProcessing.ProfileID;
            DataTable dt0 = new DataTable();
            DataRow workRow0;
            SqlDataAdapter adpt0 = new SqlDataAdapter(Query0, con);
            adpt0.Fill(dt0);
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = dt0;

            foreach (DataRow dv0 in dt0.Rows)
            {
                string var_name = dv0[3].ToString().Trim();
                string var_Value = dv0[4].ToString().Trim();
                string var_des = dv0[5].ToString().Trim();
                string var_type = dv0[6].ToString().Trim();

                string Var_Text = dv0[7].ToString().Trim();
                string Algo_Name = dv0[9].ToString().Trim();
                string line_sql = dv0[11].ToString().Trim();
                string File_ID = dv0[10].ToString().Trim();
                string Pos_From = dv0[13].ToString().Trim();
                string len_data = dv0[15].ToString().Trim();
                string ki_val = "";
                //int list_4 = 0,list_8=0;
                String line;
                string myData;

                try
                {
                    if (Var_Text.TrimEnd() == "FL")
                    {
                        StreamReader sr = new StreamReader(filename_2);

                        string strPath = filename_2;

                        string filename = null;
                        filename = Path.GetFileName(strPath);

                        line = sr.ReadLine();

                        int line_number = 1;

                        while (line != null)
                        {
                            if (line_number.ToString().Equals(line_sql))
                            {
                                myData = line.Substring(int.Parse(Pos_From), int.Parse(len_data));
                                if (var_des == "ICCID")
                                {
                                    first_icicid = myData;
                                }
                                else if (var_des == "IMSI")
                                {
                                    first_imsi = myData;
                                }

                                using (SqlConnection con1 = new SqlConnection(connectionString))
                                {
                                    SqlDataReader reader = null;
                                    using (SqlCommand cmd = new SqlCommand("usp_Insert_first_record", con1))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@DataGenProcessHDID", fileid);
                                        cmd.Parameters.AddWithValue("@VarID", var_name.TrimEnd());
                                        cmd.Parameters.AddWithValue("@VarName", var_des.TrimEnd());
                                        cmd.Parameters.AddWithValue("@VarValue", myData.TrimEnd());
                                        cmd.Parameters.AddWithValue("@VarType", var_type.TrimEnd());
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
                                }
                            }

                            //Read the next line
                            line = sr.ReadLine();
                            line_number++;
                        }
                        //close the file
                        sr.Close();
                        Console.ReadLine();
                    }

                    if (Var_Text.TrimEnd() == "AL")
                    {
                        string caseSwitch = Algo_Name;
                        string KI_val = "";
                        string my_data = "";
                        switch (caseSwitch)
                        {


                            case "R_4":
                                my_data = Random4digits();
                                r4_data_list.Add(my_data);
                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data.ToString(), var_type.TrimEnd(), filename_2);
                                break;

                            case "R4_PF":
                                //richTextBox2.Text += var_des;
                                my_data = padding_filler(r4_data_list[list_4]);
                                list_4 += 1;


                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data.ToString(), var_type.TrimEnd(), filename_2);
                                break;

                            case "R_8":
                                my_data = Random8digits();
                                r8_data_list.Add(my_data);
                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data.ToString(), var_type.TrimEnd(), filename_2);
                                break;

                            case "R8_P":
                                //richTextBox2.Text += var_des;
                                my_data = padding(r8_data_list[list_8]);
                                list_8 += 1;

                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data.ToString(), var_type.TrimEnd(), filename_2);
                                break;

                            case "R_16_Hex":
                                my_data = Create16DigitString();
                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data.ToString(), var_type.TrimEnd(), filename_2);
                                break;

                            case "R_32_Hex":
                                my_data = Create32DigitString();
                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data.ToString(), var_type.TrimEnd(), filename_2);
                                break;

                            case "Pad_8":
                                my_data = Padding_8();
                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data.ToString(), var_type.TrimEnd(), filename_2);
                                break;

                            case "Pad_16":
                                my_data = Padding_16();
                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data.ToString(), var_type.TrimEnd(), filename_2);
                                break;
                            //gettting null icicid
                            case "ICCID_NS":
                                my_data = nibble_swapped(first_icicid);
                                //MessageBox.Show(first_icicid+":"+ my_data);
                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data.ToString(), var_type.TrimEnd(), filename_2);
                                break;

                            case "IMSI_NS":
                                my_data = nibble_swapped("809" + first_imsi);
                                //MessageBox.Show("809" + first_imsi + ":" + my_data);
                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data.ToString(), var_type.TrimEnd(), filename_2);
                                break;

                            case "ACC_Hex":
                                my_data = acc(first_imsi);

                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data.ToString(), var_type.TrimEnd(), filename_2);
                                break;


                            case "R_32_Hex_KI":
                                KI_val = Create32DigitString();
                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), KI_val.ToString(), var_type.TrimEnd(), filename_2);
                                break;

                            case "KI_AES_128":
                                //string my_data_2 = Encrypt(ki_val, "sblw-3hn8-sqoy19");
                                string my_data_2 = Create32DigitString();

                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data_2, var_type.TrimEnd(), filename_2);
                                break;
                            case "Single_Des":
                                
                                break;

                            //case "AES_128":
                            //    //string my_data_2 = Encrypt(ki_val, "sblw-3hn8-sqoy19");
                            //    string my_data_opc = OPC_GEN.opc(textBox5.Text, ki_val_sql);

                            //    insert_data(var_name.TrimEnd(), var_des.TrimEnd(), AES_128, var_type.TrimEnd(), filename_2);
                            //    break;

                            case "3P":
                                //richTextBox2.Text += var_des;
                                my_data = padding(first_icicid);
                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data.ToString(), var_type.TrimEnd(), filename_2);
                                break;
                            case "YYYYMMDDHHMMSS":
                                my_data = DateTime.Now.ToString("yyyMMddHmmss");
                                insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data.ToString(), var_type.TrimEnd(), filename_2);
                                break;

                                

                        }
                    }
                    if (Var_Text.TrimEnd() == "TX")
                    {
                        string my_data = var_Value.TrimEnd();

                        insert_data(var_name.TrimEnd(), var_des.TrimEnd(), my_data, var_type.TrimEnd(), filename_2);
                    }
                }
                finally
                {
                    Console.WriteLine($"Executing finally block.");
                }
            }


            String Query1 = "SELECT DataGenProcessData.[DataGenProcessDataID], DataGenProcessData.[DataGenProcessHDID], DataGenProcessData.[VarID], DataGenProcessData.[VarName], DataGenProcessData.[VarValue], DataGenProcessData.[VarType], DataGenProcessData.[StatusID],[InPutDataTemplate].algoname FROM DataGenProcessData inner JOIN [InPutDataTemplate] ON [InPutDataTemplate].vardes = DataGenProcessData.varname where  DataGenProcessData.[DataGenProcessHDID] = '" + fileid + "' order by DataGenProcessData.VarID ";
            DataTable dt1 = new DataTable();
            DataRow workRow1;
            SqlCommand sqlcom1 = new SqlCommand(Query1, con);
            SqlDataAdapter adpt1 = new SqlDataAdapter(Query1, con);
            adpt1.Fill(dt1);
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = dt1;
            r4_data_list.Clear();
            r8_data_list.Clear();



            foreach (DataRow dv0 in dt1.Rows)
            {
                string var_ID = dv0[2].ToString().TrimEnd();
                string var_name = dv0[3].ToString().TrimEnd();
                string var_Value = dv0[4].ToString().TrimEnd();
                string var_algoname = dv0[7].ToString().TrimEnd();
                string var_Type = dv0[5].ToString().TrimEnd();

                if (var_name.TrimEnd() == "Quantity")
                {
                    records = Int32.Parse(var_Value.TrimEnd());


                    for (int i = 1; i <= Int32.Parse(var_Value.TrimEnd()); i++)
                    {
                        using (SqlConnection con1 = new SqlConnection(connectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = con.CreateCommand())
                            {
                                cmd.CommandText = @"insert into DataGenProcessDataRecord(DataGenProcessDataRecordID,DataGenProcessHDID) values('" + i + "','" + fileid + "')";
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }


                        }


                    }
                }
                //else 
                //MessageBox.Show(var_name.TrimEnd() + ' ' + var_Value.TrimEnd());
                if (var_name.TrimEnd() == "ICCID")

                {
                    // MessageBox.Show(records.ToString());
                    for (int i = 1; i <= records; i++)
                    {

                        if (i == 1)
                        {
                            long iccid_vl = Int64.Parse(var_Value);

                            update(i, var_ID, iccid_vl.ToString());
                            first_icicid = iccid_vl.ToString();
                        }
                        else
                        {
                            long iccid_vl = Int64.Parse(var_Value) + i;
                            //MessageBox.Show(var_ID + " " + iccid_vl.ToString());
                            update(i, var_ID, iccid_vl.ToString());
                            if (i == records)
                            {
                                last_icicid = iccid_vl.ToString();
                            }
                        }
                    }


                }

                else if (var_name.TrimEnd() == "IMSI")
                {
                    for (int i = 1; i <= records; i++)
                    {
                        if (i == 1)
                        {

                            long IMSI_vl = Int64.Parse(var_Value);

                            update(i, var_ID, IMSI_vl.ToString());
                            first_imsi = IMSI_vl.ToString();

                        }
                        else
                        {

                            long IMSI_vl = Int64.Parse(var_Value) + i;
                            update(i, var_ID, IMSI_vl.ToString());
                            if (i == records)
                            {
                                last_imsi = IMSI_vl.ToString();
                            }
                        }
                    }


                }


                else if (var_name.TrimEnd() == "AGSUI:IMSI")
                {
                    for (int i = 1; i <= records; i++)
                    {
                        update(i, var_ID, var_Value);

                    }
                }
                else if (var_name.TrimEnd() == "EKI")
                {
                    for (int i = 1; i <= records; i++)
                    {
                        update(i, var_ID, var_Value);

                    }
                }
                else if (var_name.TrimEnd() == "KIND")
                {
                    for (int i = 1; i <= records; i++)
                    {
                        update(i, var_ID, var_Value);

                    }
                }

                else if (var_name.TrimEnd() == "YYYYMMDDHHMMSS")
                {
                    for (int i = 1; i <= records; i++)
                    {
                        update(i, var_ID, var_Value);

                    }
                }
                else if (var_name.TrimEnd() == "FSETIND")
                {
                    for (int i = 1; i <= records; i++)
                    {
                        update(i, var_ID, var_Value);

                    }
                }
                else if (var_name.TrimEnd() == "A4IND")
                {
                    for (int i = 1; i <= records; i++)
                    {
                        update(i, var_ID, var_Value);

                    }
                }



                else if (var_name.TrimEnd() == "Transport_key")
                {
                    for (int i = 1; i <= records; i++)
                    {
                        update(i, var_ID, var_Value);

                    }


                }

                else if (var_name.TrimEnd() == "ENCRYPTED KI")
                {
                    for (int i = 1; i <= records; i++)
                    {
                        string var_Value_1 = Encrypt(Create32DigitString(), "sblw-3hn8-sqoy19");

                        update(i, var_ID, var_Value_1);

                    }


                }



                if (var_Type.TrimEnd() == "T")
                {
                    for (int i = 1; i <= records; i++)
                    {


                        update(i, var_ID, var_Value);

                    }

                }


                string caseSwitch = var_algoname;
                string my_data;
                r4_data = "";
                string ki_val;
                List<string> ki_val_list = new List<string>();
                switch (caseSwitch)
                {


                    case "R_4":
                        //richTextBox2.Text += var_des;
                        for (int i = 1; i <= records; i++)
                        {

                            if (i == 1)
                            {
                                my_data = var_Value;
                                r4_data_list.Add(padding_filler(my_data));

                                update(i, var_ID, my_data);
                                //richTextBox1.Text += i + " r4| " + var_ID + " | " + my_data + "|" + padding_filler(my_data) + "\n";
                            }
                            else
                            {


                                my_data = Random4digits();
                                r4_data_list.Add(padding_filler(my_data));

                                update(i, var_ID, my_data);
                                //richTextBox1.Text += i + " r4| " + var_ID + " | " + my_data + "|" + padding_filler(my_data) + "\n";
                            }

                        }
                        break;




                    case "R4_PF":
                        //richTextBox2.Text += var_des;

                        for (int i = 1; i <= records; i++)
                        {
                            if (i == 1)
                            {
                                my_data = var_Value;
                                update(i, var_ID, my_data);
                                //richTextBox1.Text += i + " r4pf| " + var_ID + " | " + my_data + "\n";
                                r4_data_count += 1; ;
                            }
                            else
                            {
                                my_data = r4_data_list[r4_data_count];
                                update(i, var_ID, my_data);
                                //richTextBox1.Text += i + " r4pf| " + var_ID + " | " + my_data + "\n";
                                r4_data_count += 1;
                            }
                        }
                        break;

                    case "R_8":

                        for (int i = 1; i <= records; i++)
                        {

                            if (i == 1)
                            {
                                my_data = var_Value;
                                update(i, var_ID, my_data);

                                r8_data_list.Add(padding(my_data));

                                //richTextBox1.Text += i + " R_8| " + var_ID + " | " + my_data + "|" + padding(my_data) + "\n";
                            }
                            else
                            {
                                my_data = Random8digits();
                                update(i, var_ID, my_data);

                                r8_data_list.Add(padding(my_data));

                                //richTextBox1.Text += i + " R_8| " + var_ID + " | " + my_data + "|" + padding(my_data) + "\n";
                            }
                        }
                        break;

                    case "R8_P":
                        //richTextBox2.Text += var_des;

                        for (int i = 1; i <= records; i++)
                        {
                            if (i == 1)
                            {
                                my_data = var_Value;
                                update(i, var_ID, my_data);
                                //richTextBox1.Text += i + " r8pf| " + var_ID + " | " + my_data + "\n";
                                r8_data_count += 1;
                            }
                            else
                            {
                                my_data = r8_data_list[r8_data_count];
                                update(i, var_ID, my_data);
                                //richTextBox1.Text += i + " r8pf| " + var_ID + " | " + my_data + "\n";
                                r8_data_count += 1;
                            }
                        }
                        break;

                    case "ACC_Hex":
                        //richTextBox2.Text += var_des;
                        for (int i = 1; i <= records; i++)
                        {

                            if (i == 1)
                            {
                                my_data = var_Value;
                                update(i, var_ID, my_data);
                                //richTextBox1.Text += i + " r4| " + var_ID + " | " + my_data + "|" + padding_filler(my_data) + "\n";
                            }
                            else
                            {


                                my_data = acc((Int64.Parse(first_imsi) + i - 1).ToString());
                                update(i, var_ID, my_data);
                                //richTextBox1.Text += i + " r4| " + var_ID + " | " + my_data + "|" + padding_filler(my_data) + "\n";
                            }

                        }
                        break;

                    case "3P":
                        //richTextBox2.Text += var_des;
                        for (int i = 1; i <= records; i++)
                        {

                            if (i == 1)
                            {
                                my_data = var_Value;
                                update(i, var_ID, my_data);
                                //richTextBox1.Text += i + " r4| " + var_ID + " | " + my_data + "|" + padding_filler(my_data) + "\n";
                            }
                            else
                            {


                                my_data = padding((Int64.Parse(first_icicid) + i - 1).ToString());
                                update(i, var_ID, my_data);
                                //richTextBox1.Text += i + " r4| " + var_ID + " | " + my_data + "|" + padding_filler(my_data) + "\n";
                            }

                        }
                        break;

                    case "R_16_Hex":

                        for (int i = 1; i <= records; i++)
                        {
                            my_data = Create16DigitString();
                            update(i, var_ID, my_data);
                        }
                        break;

                    case "R_32_Hex":

                        for (int i = 1; i <= records; i++)
                        {
                            my_data = Create32DigitString();
                            update(i, var_ID, my_data);
                        }
                        break;

                    case "Pad_8":
                        my_data = Padding_8();
                        for (int i = 1; i <= records; i++)
                        {
                            my_data = Create32DigitString();
                            update(i, var_ID, my_data);
                        }
                        break;

                    case "Pad_16":
                        my_data = Padding_16();
                        for (int i = 1; i <= records; i++)
                        {
                            my_data = Create32DigitString();
                            update(i, var_ID, my_data);
                        }
                        break;

                    case "ICCID_NS":

                        for (int i = 1; i <= records; i++)
                        {

                            string icicid_num = (Int64.Parse(first_icicid) + i - 1).ToString();

                            my_data = nibble_swapped(icicid_num);

                            update(i, var_ID, my_data);
                        }
                        break;

                    case "IMSI_NS":

                        for (int i = 1; i <= records; i++)
                        {

                            string imsi_num = "809" + (Int64.Parse(first_imsi) + i - 1).ToString();

                            my_data = nibble_swapped(imsi_num);

                            update(i, var_ID, my_data);
                        }
                        break;

                    case "R_32_Hex_KI":
                        //richTextBox2.Text += var_des;

                        for (int i = 1; i <= records; i++)
                        {
                            my_data = Create32DigitString();
                            ki_val_list.Add(my_data);
                            update(i, var_ID, my_data);
                        }
                        break;

                    case "KI_AES_128":
                        ////richTextBox1.Text += var_des;
                        //string my_data_2 = Encrypt(ki_val, "sblw-3hn8-sqoy19");
                        ////richTextBox1.Text += my_data;
                        ////richTextBox1.Text += "\n";
                        //..MessageBox.Show(my_data_2);
                        foreach (string k in ki_val_list)
                        {
                            int i = 1;

                            //my_data = Encrypt(k, "sblw-3hn8-sqoy19");
                            my_data = Create32DigitString();
                            update(i, var_ID, my_data);
                            i++;
                        }
                        break;
                    //default:
                    //    Console.WriteLine($"Happy");

                    case "Single_Des":
                        
                        break;






                }

            }


            lblMessage.Text = "Data processed successfully, Count: " + records + "\nFirst ICICID: " + first_icicid + ", \tLast ICICID: " + last_icicid + "\nFirst IMSI: " + first_imsi + ", \tLast IMSI: " + last_imsi;
            lblMessage.ForeColor = Color.Green;

            lblMessage.Visible = true;

            UpdateInputFileSatus("Input_File", fileid);
            //UpdateProcessHDFile("DDMMYY", 4, "AIR_DEL_128K_PRE_HLR5_PARTNERNAME_DDMMYY_" + unixTime + ".csv", myfile);
            GetGenProcessList();

            // Stop timing.
            stopwatch.Stop();

            // Write result.
            MessageBox.Show(stopwatch.Elapsed.ToString());

        }
        private void btnGenerateAllFiles_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT *FROM DataGenProcessDataRecord WHERE DataGenProcessHDID = @hdid", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@hdid", OFProcessing.lastInsertedId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ProcessAllFile(OFProcessing.ProfileID);
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
                    logString.Append(DateTime.Now + "\n**File processing started**");
                    using (SqlCommand cmd = new SqlCommand("  SELECT FILENAME FROM CustProfileFile where CustProfileID = @custProfileID and  FileIOID<>'I'", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("custProfileID", customerProfileID);
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                fileName = reader["FileName"].ToString();
                                switch (fileName)
                                {
                                    case "HLR":
                                        HLR();
                                        break;
                                    case "HSS":
                                        HSS();
                                        break;
                                    case "MCA":
                                        MCA();
                                        break;
                                    case "DDMMYY":
                                        DDMMYY();
                                        break;
                                    case "DSA":
                                        DSA();
                                        break;
                                    case "REP":
                                        //DSA(); //Todo work on DSA  
                                        break;
                                    case "CPS":
                                        CPS();
                                        break;
                                    default: throw new Exception();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logString.Append("\nSomething went wrong with: " + fileName + ex.Message);
            }
        }
        private void HLR()
        {
            string myfile = string.Empty;
            string unixTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            //dataGridView1.Rows.Clear();
            string constr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                String Query2 = "select DataGenProcessDataRecordID from DataGenProcessDataRecord  where DataGenProcessHDID = '" + fileid + "'";
                DataTable dt2 = new DataTable();
                DataRow workRow2;

                using (SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con))
                {
                    adpt2.Fill(dt2);
                    foreach (DataRow dv0 in dt2.Rows)
                    {

                        string record_no = dv0[0].ToString();
                        myfile = Outfilelocation + @"\AIR_DEL_POST_128K_ST_hlr_" + unixTime + ".auc";
                        if (!File.Exists(myfile))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(myfile))
                            { }

                        }

                        String Query3 = "select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = 1 and FileLineNo = 1 order by OutPutFileTemplateID,OutputTemplateLinesID";
                        DataTable dt3 = new DataTable();
                        DataRow workRow3;
                        SqlDataAdapter adpt3 = new SqlDataAdapter(Query3, con);
                        adpt3.Fill(dt3);

                        foreach (DataRow dv in dt3.Rows)
                        {
                            string str_varname = dv[0].ToString();
                            string str_VarType = dv[1].ToString();
                            if (str_VarType[0] == 'V')
                            {
                                String Query = "select * from DataGenProcessDataRecord where DataGenProcessDataRecordID =  '" + record_no + "' and  DataGenProcessHDID = '" + fileid + "' ";
                                DataTable dt = new DataTable();
                                SqlDataAdapter adpt = new SqlDataAdapter(Query, con);
                                adpt.Fill(dt);

                                DataTable dtb = new DataTable();
                                dtb.Columns.Add("VarName");
                                dtb.Columns.Add("VarValue");
                                DataRow workRow;

                                foreach (DataRow dr in dt.Rows)
                                {
                                    foreach (DataColumn dc in dt.Columns)
                                    {

                                        workRow = dtb.NewRow();
                                        workRow[0] = dc.ColumnName;
                                        workRow[1] = dr[dc.ColumnName].ToString();
                                        dtb.Rows.Add(workRow);

                                    }
                                }


                                string dta_1 = str_varname;

                                dataGridView1.ReadOnly = true;

                                dataGridView1.DataSource = dtb;

                                string expression;
                                expression = "VarName = '" + dta_1 + "'";
                                DataRow[] foundRows;

                                foundRows = dtb.Select(expression);

                                for (int i = 0; i < foundRows.Length; i++)
                                {
                                    string data = foundRows[i][1].ToString();
                                    File.AppendAllText(myfile, data.TrimEnd());

                                }
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
                                        File.AppendAllText(myfile, str);
                                    }
                                    con1.Close();
                                }
                            }
                        } 
                        File.AppendAllText(myfile, "\n");
                    }
                    //EncryptionandDecryption.EncryptFile(myfile, myfile, "HR$2pIjHR$2pIj12");

					//EncryptionandDecryption.EncryptFile(myfile, myfile+"_2", @"myKey123");

                    Pgp.EncryptFile(myfile + "_2", myfile, @"C:\Users\vishvajeet.arya\Downloads\openssl-0.9.8h-1-bin\bin\mypublickey.pem", true, true);

                    //Pgp.DecryptFile("Resources/output.txt", "Resources/privateKey.txt", "pass".ToCharArray(), "default.txt");

                }
            }
            try
            {
                UpdateProcessHDFile("HLR", 1, "AIR_DEL_POST_128K_ST_hlr_" + unixTime + ".auc", myfile);
                MessageBox.Show("HLR OutFile created successfully",
                                        "Message",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            GetGenProcessList();
        }

		private void HSS()
        {
            string myfile = string.Empty;
            string unixTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            string constr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                String Query2 = "select DataGenProcessDataRecordID from DataGenProcessDataRecord  where   DataGenProcessHDID = '" + fileid + "'";
                DataTable dt2 = new DataTable();
                DataRow workRow2;



                using (SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con))
                {

                    adpt2.Fill(dt2);
                    foreach (DataRow dv0 in dt2.Rows)
                    {

                        string record_no = dv0[0].ToString();
                        myfile = Outfilelocation + @"\AIR_DEL_POST_128K_ST_hss_" + unixTime + ".auc";
                        if (!File.Exists(myfile))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(myfile))
                            { }

                        }

                        String Query3 = "select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = 2 and FileLineNo = 1 order by OutPutFileTemplateID,OutputTemplateLinesID";
                        DataTable dt3 = new DataTable();
                        DataRow workRow3;
                        SqlDataAdapter adpt3 = new SqlDataAdapter(Query3, con);
                        adpt3.Fill(dt3);
                        //dataGridView1.ReadOnly = true;
                        //dataGridView1.DataSource = dt3;

                        foreach (DataRow dv in dt3.Rows)
                        {
                            string str_varname = dv[0].ToString();
                            string str_VarType = dv[1].ToString();
                            //str_VarType = str_VarType[0].ToString();
                            if (str_VarType[0] == 'V')
                            {
                                //MessageBox.Show(varname);
                                String Query = "select * from DataGenProcessDataRecord where DataGenProcessDataRecordID =  '" + record_no + "'  and DataGenProcessHDID = '" + fileid + "' ";
                                DataTable dt = new DataTable();
                                SqlDataAdapter adpt = new SqlDataAdapter(Query, con);
                                adpt.Fill(dt);



                                DataTable dtb = new DataTable();
                                dtb.Columns.Add("VarName");
                                dtb.Columns.Add("VarValue");
                                DataRow workRow;

                                foreach (DataRow dr in dt.Rows)
                                {
                                    foreach (DataColumn dc in dt.Columns)
                                    {

                                        workRow = dtb.NewRow();
                                        workRow[0] = dc.ColumnName;
                                        workRow[1] = dr[dc.ColumnName].ToString();
                                        dtb.Rows.Add(workRow);

                                    }
                                }


                                string dta_1 = str_varname;

                                dataGridView1.ReadOnly = true;

                                dataGridView1.DataSource = dtb;

                                string expression;
                                expression = "VarName = '" + dta_1 + "'";
                                DataRow[] foundRows;

                                // Use the Select method to find all rows matching the filter.
                                foundRows = dtb.Select(expression);

                                // Print column 0 of each returned row.
                                for (int i = 0; i < foundRows.Length; i++)
                                {
                                    string data = foundRows[i][1].ToString();
                                    File.AppendAllText(myfile, data.TrimEnd());

                                }
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

                                        File.AppendAllText(myfile, str);
                                    }
                                    con1.Close();
                                }
                            }
                        }
                        File.AppendAllText(myfile, "\n");

                    }
                    //EncryptionandDecryption.EncryptFile(myfile, myfile + "_2", @"myKey123");
                    //Pgp.EncryptFile(myfile + "_2", myfile, @"E:\ColorPlast\public.txt", true, true);
                }
            }
            //MessageBox.Show("File Built");


            try
            {
                UpdateProcessHDFile("HSS", 2, "AIR_DEL_POST_128K_ST_hss_" + unixTime + ".auc", myfile);
                MessageBox.Show("HSS OutFile created successfully",
                                        "Message",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            GetGenProcessList();

        }
        private void DSA()
        {
            string unixTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            string myfile = string.Empty;
            string constr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                String Query2 = "select DataGenProcessDataRecordID from DataGenProcessDataRecord  where   DataGenProcessHDID = '" + fileid + "'";
                DataTable dt2 = new DataTable();
                DataRow workRow2;

                string headerfile = headerfilepath + @"\AIR_DEL_POST_128K_ST_DSA_Header.AUC";
                myfile = Outfilelocation + @"\AIR_DEL_POST_128K_ST_DSA_" + unixTime + ".AUC";
                File.Copy(headerfile, myfile, true);
                File.AppendAllText(myfile, "\n");

                using (SqlCommand sqlcom2 = new SqlCommand(Query2, con))
                {
                    SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con);
                    adpt2.Fill(dt2);
                    foreach (DataRow dv0 in dt2.Rows)
                    {
                        string record_no = dv0[0].ToString();
                        if (!File.Exists(myfile))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(myfile))
                            { }

                        }

                        String Query3 = "select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = 5 and FileLineNo = 1 order by OutPutFileTemplateID,OutputTemplateLinesID";
                        DataTable dt3 = new DataTable();
                        DataRow workRow3;
                        SqlDataAdapter adpt3 = new SqlDataAdapter(Query3, con);
                        adpt3.Fill(dt3);

                        foreach (DataRow dv in dt3.Rows)
                        {
                            string str_varname = dv[0].ToString();
                            string str_VarType = dv[1].ToString();
                            if (str_VarType[0] == 'V')
                            {
                                String Query = "select * from DataGenProcessDataRecord where DataGenProcessDataRecordID =  '" + record_no + "'  and DataGenProcessHDID = '" + fileid + "'  ";
                                DataTable dt = new DataTable();
                                SqlDataAdapter adpt = new SqlDataAdapter(Query, con);
                                adpt.Fill(dt);



                                DataTable dtb = new DataTable();
                                dtb.Columns.Add("VarName");
                                dtb.Columns.Add("VarValue");
                                DataRow workRow;

                                foreach (DataRow dr in dt.Rows)
                                {
                                    foreach (DataColumn dc in dt.Columns)
                                    {

                                        workRow = dtb.NewRow();
                                        workRow[0] = dc.ColumnName;
                                        workRow[1] = dr[dc.ColumnName].ToString();
                                        dtb.Rows.Add(workRow);

                                    }
                                }


                                string dta_1 = str_varname;

                                dataGridView1.ReadOnly = true;

                                dataGridView1.DataSource = dtb;

                                string expression;
                                expression = "VarName = '" + dta_1 + "'";
                                DataRow[] foundRows;

                                // Use the Select method to find all rows matching the filter.
                                foundRows = dtb.Select(expression);

                                // Print column 0 of each returned row.
                                for (int i = 0; i < foundRows.Length; i++)
                                {
                                    string data = foundRows[i][1].ToString();
                                    File.AppendAllText(myfile, data.TrimEnd());

                                }
                            }
                            else if (str_VarType == "S")
                            {
                                using (SqlConnection con1 = new SqlConnection(connectionString))
                                {
                                    SqlCommand com1 = new SqlCommand("select Seperator from [dbo].[SeperatorMaster] where SepID ='" + str_varname + "'", con1);
                                    con1.Open();
                                    SqlDataReader sqlDataReader = com1.ExecuteReader();

                                    while (sqlDataReader.Read())
                                    {
                                        string str = sqlDataReader.GetString(0);
                                        File.AppendAllText(myfile, str);
                                    }
                                    con1.Close();
                                }

                            }

                        }
                        File.AppendAllText(myfile, "\n");
                    }
                    //EncryptionandDecryption.EncryptFile(myfile, myfile + "_2", @"myKey123");
                    //Pgp.EncryptFile(myfile + "_2", myfile, @"E:\ColorPlast\public.txt", true, true);
                }
            }
            //MessageBox.Show("File Built");



            try
            {
                UpdateProcessHDFile("DSA", 5, "AIR_DEL_POST_128K_ST_DSA.AUC", myfile);
                MessageBox.Show("DSA OutFile created successfully",
                                        "Message",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            GetGenProcessList();
        }
        private void MCA()
        {
            string myfile = string.Empty;
            string unixTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            string constr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                String Query2 = "select DataGenProcessDataRecordID from DataGenProcessDataRecord  where   DataGenProcessHDID = '" + fileid + "'";
                DataTable dt2 = new DataTable();
                DataRow workRow2;

                //string headerfile = headerfilepath + @"\AIR_DEL_POST_128K_ST_MCA.txt";
                myfile = Outfilelocation + @"\AIR_DEL_POST_128K_ST_MCA_" + unixTime + ".txt";
                //File.Copy(headerfile, myfile, true);
                File.AppendAllText(myfile, "ICCID,IMSI,ACC,DPIN1,DPIN2,DPUK1,DPUK2,ADM1,KI,OPC,KIC1,KID1,KIK1,KIC2,KID2,KIK2,KIC3,KID3,KIK3,PSK,DEK1,AICCID,ASCII_ICCID,LICENSE_KEY\n");

                using (SqlCommand sqlcom2 = new SqlCommand(Query2, con))
                {
                    SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con);
                    adpt2.Fill(dt2);
                    foreach (DataRow dv0 in dt2.Rows)
                    {
                        string record_no = dv0[0].ToString();
                        //string myfile = @"B:\DATA-FILES\DATATOOL\OUTPUT FILES\AIR_DEL_POST_128K_ST_DSA.AUC";
                        if (!File.Exists(myfile))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(myfile))
                            { }

                        }

                        String Query3 = "select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = 3 and FileLineNo = 1 order by OutPutFileTemplateID,OutputTemplateLinesID";
                        DataTable dt3 = new DataTable();
                        DataRow workRow3;
                        SqlDataAdapter adpt3 = new SqlDataAdapter(Query3, con);
                        adpt3.Fill(dt3);


                        foreach (DataRow dv in dt3.Rows)
                        {
                            string str_varname = dv[0].ToString();
                            string str_VarType = dv[1].ToString();
                            //str_VarType = str_VarType[0].ToString();
                            if (str_VarType[0] == 'V')
                            {
                                //MessageBox.Show(varname);
                                String Query = "select * from DataGenProcessDataRecord where DataGenProcessDataRecordID =  '" + record_no + "'  and DataGenProcessHDID = '" + fileid + "'  ";
                                DataTable dt = new DataTable();
                                SqlDataAdapter adpt = new SqlDataAdapter(Query, con);
                                adpt.Fill(dt);

                                DataTable dtb = new DataTable();
                                dtb.Columns.Add("VarName");
                                dtb.Columns.Add("VarValue");
                                DataRow workRow;

                                foreach (DataRow dr in dt.Rows)
                                {
                                    foreach (DataColumn dc in dt.Columns)
                                    {

                                        workRow = dtb.NewRow();
                                        workRow[0] = dc.ColumnName;
                                        workRow[1] = dr[dc.ColumnName].ToString();
                                        dtb.Rows.Add(workRow);

                                    }
                                }


                                string dta_1 = str_varname;

                                dataGridView1.ReadOnly = true;

                                dataGridView1.DataSource = dtb;

                                string expression;
                                expression = "VarName = '" + dta_1 + "'";
                                DataRow[] foundRows;

                                // Use the Select method to find all rows matching the filter.
                                foundRows = dtb.Select(expression);

                                // Print column 0 of each returned row.
                                for (int i = 0; i < foundRows.Length; i++)
                                {
                                    string data = foundRows[i][1].ToString();
                                    File.AppendAllText(myfile, data.TrimEnd());
                                }
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
                                        File.AppendAllText(myfile, str);
                                    }
                                    con1.Close();
                                }

                            }

                        }
                        File.AppendAllText(myfile, "\n");
                    }
                    //EncryptionandDecryption.EncryptFile(myfile, myfile + "_2", @"myKey123");
                    //Pgp.EncryptFile(myfile + "_2", myfile, @"E:\ColorPlast\public.txt", true, true);
                }
            }
            //MessageBox.Show("File Built");


            try
            {
                UpdateProcessHDFile("MCA", 3, "AIR_DEL_POST_128K_ST_MCA_" + unixTime + ".txt", myfile);
                MessageBox.Show("MCA OutFile created successfully",
                                        "Message",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            GetGenProcessList();
        }

        private void CPS()
        {
            string constr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            string unixTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                String Query2 = "select DataGenProcessDataRecordID from DataGenProcessDataRecord  where   DataGenProcessHDID = '" + fileid + "'  order by DataGenProcessDataRecordID";
                DataTable dt2 = new DataTable();
                DataRow workRow2;

                string headerfile = headerfilepath + @"\Sample_JAVA_Card_CPS.cps";
                string headerfile1 = headerfilepath + @"\Sample_JAVA_Card_CPS_records_wise.cps";
                string myfile = Outfilelocation + @"\Sample_JAVA_Card_CPS_" + unixTime + ".cps";
                File.Copy(headerfile, myfile, true);
                File.AppendAllText(myfile, "\n");

                using (SqlCommand sqlcom2 = new SqlCommand(Query2, con))
                {
                    SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con);
                    adpt2.Fill(dt2);
                    foreach (DataRow dv0 in dt2.Rows)
                    {
                        string record_no = dv0[0].ToString();
                        //string myfile = @"B:\DATA-FILES\DATATOOL\OUTPUT FILES\AIR_DEL_POST_128K_ST_DSA.AUC";
                        if (!File.Exists(myfile))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(myfile))
                            { }

                        }

                        ////added for header file

                        //StreamReader reading_1 = File.OpenText(headerfile);
                        //string str_1,str_12 = string.Empty;

                        //string bar_1 = string.Empty;
                        //int start_1 = 0;
                        //while ((str_1 = reading_1.ReadLine()) != null)
                        //{
                        //    start_1 = 0;
                        //    bar_1 = string.Empty;
                        //    if (str_1.Contains("#"))
                        //    {
                        //        //Console.WriteLine(str);

                        //        foreach (char c in str_1)
                        //        {
                        //            if (c == '#' && start_1 == 1)
                        //            { start_1 = 0; }
                        //            else if (c == '#')
                        //            { start_1 = 1; }

                        //            if (start_1 == 1)
                        //            { bar_1 += c; }
                        //        }
                        //    }
                        //    if (bar_1 != "")

                        //    {
                        //        bar_1 = bar_1.Replace("#", "");
                        //        using (SqlConnection con_cps_1 = new SqlConnection(constr))
                        //        {
                        //            con_cps_1.Open();
                        //            SqlCommand com_cps = new SqlCommand("SELECT " + (bar_1) + "  FROM  [DataGenProcessDataRecord]  where [DataGenProcessDataRecordID] = " + record_no + "  and DataGenProcessHDID =  '" + fileid + "'", con_cps_1);

                        //            //  SqlDataAdapter da1 = new SqlDataAdapter(com1);
                        //            SqlDataReader sqlDataReader = com_cps.ExecuteReader();
                        //            //  da1.Fill(ptDataset1);
                        //            //   dataGridView1.DataSource = ptDataset1.Tables[0];
                        //            while (sqlDataReader.Read())
                        //            {
                        //                str_12 = sqlDataReader.GetString(0).Trim();

                        //                //richTextBox1.Text += str;
                        //                //Console.WriteLine(str_3);
                        //                //using (StreamWriter sw = File.AppendText(myfile))
                        //                //{
                        //                //    sw.WriteLine(str);
                        //                //}

                        //            }

                        //            //Console.WriteLine(bar);
                        //            string bar__11 = "#" + bar_1 + "#";
                        //            //Console.WriteLine(bar_1);
                        //            str_1 = str_1.Replace(bar__11, str_12);

                        //            //Console.WriteLine(str_1);
                        //            con_cps_1.Close();
                        //        }

                        //    }

                        //    File.AppendAllText(myfile, str_1);
                        //    File.AppendAllText(myfile, "\n");



                        //}
                        ////

                        StreamReader reading = File.OpenText(headerfile1);
                        string str, str_3 = string.Empty;

                        string bar = string.Empty;
                        int start = 0;
                        while ((str = reading.ReadLine()) != null)
                        {
                            start = 0;
                            bar = string.Empty;
                            if (str.Contains("#"))
                            {
                                //Console.WriteLine(str);

                                foreach (char c in str)
                                {
                                    if (c == '#' && start == 1)
                                    { start = 0; }
                                    else if (c == '#')
                                    { start = 1; }

                                    if (start == 1)
                                    { bar += c; }
                                }
                            }
                            if (bar != "")

                            {
                                bar = bar.Replace("#", "");
                                using (SqlConnection con_cps = new SqlConnection(constr))
                                {
                                    con_cps.Open();
                                    SqlCommand com_cps = new SqlCommand("SELECT " + (bar) + "  FROM  [DataGenProcessDataRecord]  where [DataGenProcessDataRecordID] = " + record_no + "  and DataGenProcessHDID =  '" + fileid + "'", con_cps);

                                    //  SqlDataAdapter da1 = new SqlDataAdapter(com1);
                                    SqlDataReader sqlDataReader = com_cps.ExecuteReader();
                                    //  da1.Fill(ptDataset1);
                                    //   dataGridView1.DataSource = ptDataset1.Tables[0];
                                    while (sqlDataReader.Read())
                                    {
                                        str_3 = sqlDataReader.GetString(0).Trim();

                                        //richTextBox1.Text += str;
                                        //Console.WriteLine(str_3);
                                        //using (StreamWriter sw = File.AppendText(myfile))
                                        //{
                                        //    sw.WriteLine(str);
                                        //}

                                    }

                                    //Console.WriteLine(bar);
                                    string bar_123 = "#" + bar + "#";
                                    //Console.WriteLine(bar_1);
                                    str = str.Replace(bar_123, str_3);

                                    //Console.WriteLine(str_1);
                                    con_cps.Close();
                                }

                            }

                            File.AppendAllText(myfile, str);
                            File.AppendAllText(myfile, "\n");
                        }

                        File.AppendAllText(myfile, "\n");


                    }
                    //EncryptionandDecryption.EncryptFile(myfile, myfile + "_2", @"myKey123");
                    //Pgp.EncryptFile(myfile + "_2", myfile, @"E:\ColorPlast\public.txt", true, true);
                }
            }
            //MessageBox.Show("File Built");


            try
            {
                using (SqlConnection con3 = new SqlConnection(connectionString))
                {
                    con3.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_UpdateOutFileStatus", con3))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustProfileFileID", "7");
                        cmd.Parameters.AddWithValue("@FileName", "Sample_JAVA_Card_CPS_" + unixTime + ".cps");
                        cmd.Parameters.AddWithValue("@FilePath", Outfilelocation + @"\Sample_JAVA_Card_CPS_" + unixTime + ".cps");

                        cmd.ExecuteReader();

                        MessageBox.Show("CPS OutFile created successfully",
                                        "Message",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            GetGenProcessList();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DDMMYY()
        {
            string unixTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            string myfile = string.Empty;
            string constr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                String Query2 = "select DataGenProcessDataRecordID from DataGenProcessDataRecord  where DataGenProcessHDID = '" + fileid + "'";
                DataTable dt2 = new DataTable();
                DataRow workRow2;

                myfile = Outfilelocation + @"\AIR_DEL_128K_PRE_HLR5_PARTNERNAME_DDMMYY_" + unixTime + ".csv";
                File.AppendAllText(myfile, "ICCID,IMSI,PIN1,PUK1,PIN2,PUK2,ENCRYPTED KI\n");

                using (SqlCommand sqlcom2 = new SqlCommand(Query2, con))
                {
                    SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con);
                    adpt2.Fill(dt2);
                    foreach (DataRow dv0 in dt2.Rows)
                    {
                        string record_no = dv0[0].ToString();
                        if (!File.Exists(myfile))
                        {
                            using (StreamWriter sw = File.CreateText(myfile))
                            { }

                        }

                        String Query3 = "select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = 4 and FileLineNo = 1 order by OutPutFileTemplateID,OutputTemplateLinesID";
                        DataTable dt3 = new DataTable();
                        DataRow workRow3;
                        SqlDataAdapter adpt3 = new SqlDataAdapter(Query3, con);
                        adpt3.Fill(dt3);

                        foreach (DataRow dv in dt3.Rows)
                        {
                            string str_varname = dv[0].ToString();
                            string str_VarType = dv[1].ToString();
                            if (str_VarType[0] == 'V')
                            {
                                String Query = "select * from DataGenProcessDataRecord where DataGenProcessDataRecordID =  '" + record_no + "' and DataGenProcessHDID = '" + fileid + "' ";
                                DataTable dt = new DataTable();
                                SqlDataAdapter adpt = new SqlDataAdapter(Query, con);
                                adpt.Fill(dt);



                                DataTable dtb = new DataTable();
                                dtb.Columns.Add("VarName");
                                dtb.Columns.Add("VarValue");
                                DataRow workRow;

                                foreach (DataRow dr in dt.Rows)
                                {
                                    foreach (DataColumn dc in dt.Columns)
                                    {

                                        workRow = dtb.NewRow();
                                        workRow[0] = dc.ColumnName;
                                        workRow[1] = dr[dc.ColumnName].ToString();
                                        dtb.Rows.Add(workRow);

                                    }
                                }


                                string dta_1 = str_varname;

                                dataGridView1.ReadOnly = true;

                                dataGridView1.DataSource = dtb;

                                string expression;
                                expression = "VarName = '" + dta_1 + "'";
                                DataRow[] foundRows;

                                foundRows = dtb.Select(expression);

                                for (int i = 0; i < foundRows.Length; i++)
                                {
                                    string data = foundRows[i][1].ToString();
                                    File.AppendAllText(myfile, data.TrimEnd());

                                }
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
                                        File.AppendAllText(myfile, str);
                                    }
                                    con1.Close();
                                }

                            }

                        }
                        File.AppendAllText(myfile, "\n");

                    }
                    //EncryptionandDecryption.EncryptFile(myfile, myfile + "_2", @"myKey123");
                    //Pgp.EncryptFile(myfile + "_2", myfile, @"E:\ColorPlast\public.txt", true, true);
                }
            }

            //UpdateProcessHDFile("DDMMYY", 4, "AIR_DEL_128K_PRE_HLR5_PARTNERNAME_DDMMYY_" + unixTime + ".csv", myfile);
            //GetGenProcessList();
            try
            {
                //using (SqlConnection con3 = new SqlConnection(connectionString))
                //{
                //    con3.Open();
                //    using (SqlCommand cmd = new SqlCommand("usp_UpdateOutFileStatus", con3))
                //    {
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@CustProfileFileID", "7");
                //        cmd.Parameters.AddWithValue("@FileName", "Sample_JAVA_Card_CPS_" + unixTime + ".cps");
                //        cmd.Parameters.AddWithValue("@FilePath", Outfilelocation + @"\Sample_JAVA_Card_CPS_" + unixTime + ".cps");

                //        cmd.ExecuteReader();

                //        MessageBox.Show("CPS OutFile created successfully",
                //                        "Message",
                //                        MessageBoxButtons.OK,
                //                        MessageBoxIcon.Information
                //                        );


                //    }
                //}
                UpdateProcessHDFile("DDMMYY", 4, "AIR_DEL_128K_PRE_HLR5_PARTNERNAME_DDMMYY_" + unixTime + ".csv", myfile);
                MessageBox.Show("DDMMYY OutFile created successfully",
                                        "Message",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while creating profile: " + ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
            GetGenProcessList();

        }

        public static string EncryptString_1(string plainText, string key)
        {
            byte[] encryptedBytes;
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.CBC;

                // Generate a random IV (Initialization Vector)
                aes.GenerateIV();

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        // Write the IV to the beginning of the encrypted stream
                        memoryStream.Write(aes.IV, 0, aes.IV.Length);

                        // Create a CryptoStream, which encrypts the data as it is written to the stream
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                            cryptoStream.FlushFinalBlock();
                        }

                        encryptedBytes = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string DecryptString(string encryptedText, string key)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            string decryptedText;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.CBC;

                // Extract the IV from the beginning of the encrypted stream
                byte[] iv = new byte[aes.IV.Length];
                Array.Copy(encryptedBytes, iv, iv.Length);
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var memoryStream = new System.IO.MemoryStream(encryptedBytes, iv.Length, encryptedBytes.Length - iv.Length))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] decryptedBytes = new byte[encryptedBytes.Length - iv.Length];
                            int decryptedByteCount = cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);

                            decryptedText = Encoding.UTF8.GetString(decryptedBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }

            return decryptedText;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }
        public static byte[] StrToByteArray(string str)
        {
            //MessageBox.Show(str);
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);

            List<byte> hexres = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
                hexres.Add(hexindex[str.Substring(i, 2)]);

            return hexres.ToArray();
        }
        public static string Encrypt_KI(string toEncrypt, string tk_key, bool useHashing)
        {
            //MessageBox.Show(toEncrypt + " : " + tk_key);

            byte[] keyArray;
            byte[] toEncryptArray = StrToByteArray(toEncrypt);


            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file

            //string key = (string)settingsReader.GetValue("SecurityKey",typeof(String));
            string key = tk_key;

            //System.Windows.Forms.MessageBox.Show(key);
            //System.Windows.Forms.MessageBox.Show(key);

            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(StrToByteArray(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                //keyArray = StrToByteArray(key.PadRight(32, 'F'));
                keyArray = StrToByteArray(key);
            //foreach Byte in keyArray()

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
                cTransform.TransformFinalBlock(toEncryptArray, 0,
                    toEncryptArray.Length);
            //			Console.WriteLine(toEncryptArray.Length);

            //Console.WriteLine(resultArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return ByteArrayToString(resultArray).Substring(0, 32);
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
    }
}
