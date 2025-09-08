using DG_Tool.Models;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using File=System.IO.File;
using DataTable = System.Data.DataTable;
using Font = System.Drawing.Font;
using DG_Tool.HelperClass;

namespace DG_Tool.WinForms.OutputFile
{
    public partial class DataGenHDIDProcessList : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();
        public static int HDID = 0;
        public string unixTime = DateTime.Now.ToString("yyyyMMdd");
        public static List<int> HDIDS = new List<int>();
        public DataGenHDIDProcessList()
        {
            InitializeComponent();
            dgvDetailedList.ReadOnly = true;
            DataGenProcessHDFileByHDID(OFSatusList.LotID);
            DataGridViewButtonColumn viewbutton = new DataGridViewButtonColumn();
            viewbutton.FlatStyle = FlatStyle.System;
            viewbutton.HeaderText = "Click to View";
            viewbutton.Text = "View";
            viewbutton.UseColumnTextForButtonValue = true;
            viewbutton.Name = "view";
            viewbutton.Width = 60;

            if (dgvDetailedList.Columns.Contains(viewbutton.Name = "View"))
            {

            }
            else
            {
                dgvDetailedList.Columns.Add(viewbutton);
            }
        }
        private void DataGenProcessHDFileByHDID(int id)
        {
            dgvDetailedList.DataSource = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_DataGenProcessHDFileByLotID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LotID", id);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    HDIDS = dt.AsEnumerable()
                                 .Select(row => row.Field<int>("FileID"))
                                 .ToList();
                    dgvDetailedList.DataSource = dt;

                    dgvDetailedList.EnableHeadersVisualStyles = false;
                    dgvDetailedList.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                    dgvDetailedList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgvDetailedList.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                   // dgvDetailedList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    
                }
            }

        }
        private void dgvDetailedList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            

        }

        private void dgvDetailedList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = dgvDetailedList.CurrentCell.ColumnIndex;
            string columnName = dgvDetailedList.Columns[columnIndex].Name;
            if (columnName.Equals("FileDetails"))
            {
                string filePath = dgvDetailedList.SelectedCells[0].Value.ToString();

                if (File.Exists(filePath))
                {

                    Process.Start("explorer.exe", filePath);
                }

                else
                {
                    MessageBox.Show("File not found: ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (dgvDetailedList.Columns[e.ColumnIndex].HeaderText == "Click to View")
            {
                dgvDetailedList.CurrentRow.Selected = true;
                HDID = Convert.ToInt32(dgvDetailedList.Rows[e.RowIndex].Cells["FileID"].Value);
                DetailedProcessList dgProcessList = new DetailedProcessList();
                dgProcessList.Show();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                LogMaster.addlog($"Regenerating Files for {OFSatusList.LotID} LOTID.");
                foreach (var hdid in HDIDS)
                {
                    LogMaster.addlog($"Generating File for {hdid} hdid.");
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        SqlDataReader reader = null;
                        using (SqlCommand cmd = new SqlCommand($"SELECT T2.FileName FROM  [DataGenProcessHDFile] T1 INNER JOIN [CustProfileFile] T2 ON T2.[CustProfileFileID]=T1.CustProfileFileID INNER JOIN [CustomerMaster] T4 ON T4.[CustomerID]=T2.CustomerID INNER JOIN [CustProfile] T5 ON T5.[ProfileID]=T2.CustProfileID WHERE [DataGenProcessHDID] = {hdid} and T2.FileIOID!='I'", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string filetype = reader["FileName"].ToString();
                                    generate_AllTypeOutput(hdid, filetype);
                                    LogMaster.addlog($"File Generated Succesfully.");
                                }
                            }
                        }
                    }
                }
                LogMaster.addlog($"Regenerated Files for {OFSatusList.LotID} LOTID.");
                MessageBox.Show("File Generated Successfully.");
            }
            catch(Exception ex)
            {

                LogMaster.addlog(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        public void generate_AllTypeOutput(int lastInsertedId, string filetype)
        {
            string profilename = "";
            int ProfileID = 0;
            int custID = 0;
            string Customername = "";
            DataTable dt0 = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter($"SELECT T5.ProfileName\r\n   ,T4.[CustomerName]\r\n   ,T2.CustomerID\r\n   ,T2.CustProfileID\r\n\t  \r\n  FROM  [DataGenProcessHDFile] T1\r\n  INNER JOIN [CustProfileFile] T2 ON T2.[CustProfileFileID]=T1.CustProfileFileID\r\n  INNER JOIN [CustomerMaster] T4 ON T4.[CustomerID]=T2.CustomerID\r\n  INNER JOIN [CustProfile] T5 ON T5.[ProfileID]=T2.CustProfileID\r\n  WHERE T2.FileName='{filetype}' and [DataGenProcessHDID]={lastInsertedId}", con))
                {

                    sda.Fill(dt0);
                    profilename = dt0.Rows[0][0].ToString();
                    Customername = dt0.Rows[0][1].ToString();
                    custID = Convert.ToInt32(dt0.Rows[0][2].ToString());
                    ProfileID = Convert.ToInt32(dt0.Rows[0][3].ToString());
                }
            }

            string rootdir = "";
            string filenameconv = "";
            string filemasterid = "";
            string fileext = "";
            string CustProfileFileID = "";
            string Outfilelocation = "";
            int count = 0;
            string myfile = string.Empty;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter($"SELECT FilePath,FileNamingConv,FileMasterID,FileExtn,CustProfileFileID FROM CustProfileFile where CustProfileID={ProfileID} AND CustomerID={custID} AND FileName='{filetype}'", con))
                {

                    sda.Fill(dt);
                    rootdir = dt.Rows[0][0].ToString();
                    filenameconv = dt.Rows[0][1].ToString();
                    filemasterid = dt.Rows[0][2].ToString();
                    fileext = dt.Rows[0][3].ToString();
                    CustProfileFileID = dt.Rows[0][4].ToString();
                    Outfilelocation = rootdir + $"\\{Customername}\\{profilename}\\{OFSatusList.LotID}_{lastInsertedId}_{unixTime}";
                    if (!Directory.Exists(Outfilelocation))
                    {
                        Directory.CreateDirectory(Outfilelocation);
                    }
                }
            }
            string constr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                String Query2 = $"SELECT Header FROM OutFileTemplateHD where [ProfileFileID]={filemasterid} and [ProfileID]={ProfileID}";
                DataTable dt2 = new DataTable();
                DataRow workRow2;
                SqlDataAdapter adpt2 = new SqlDataAdapter(Query2, con);
                adpt2.Fill(dt2);

                int batch = lastInsertedId;
                string Header = dt2.Rows[0][0].ToString();
                String Query1 = $"SELECT * FROM [DataGenProcessData] WHERE DataGenProcessHDID={lastInsertedId}";
                System.Data.DataTable dt1 = new System.Data.DataTable();
                DataRow workRow1;
                SqlCommand sqlcom1 = new SqlCommand(Query1, con);
                SqlDataAdapter adpt1 = new SqlDataAdapter(Query1, con);
                adpt1.Fill(dt1);
                string pattern = @"\{([^{}]*)\}";
                MatchCollection matches = Regex.Matches(Header, pattern);
                foreach (Match match in matches)
                {
                    string var = match.Groups[1].Value.ToString().Trim();
                    if (var.ToLower() == "profile")
                    {
                        string rep_var = "{" + var + "}";
                        //string var_val = cbxProfile.Text.Trim();
                        string var_val = profilename;
                        Header = Header.Replace(rep_var, var_val);
                    }
                    else if (var.ToLower() == "customer")
                    {
                        string rep_var = "{" + var + "}";
                        string var_val = Customername;
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
                //if (!File.Exists(myfile))
                //{
                //    // Create a file to write to.
                //    using (StreamWriter sw = File.CreateText(myfile))
                //    { }

                //}
                //File.AppendAllText(myfile, Header + "\r\n");
                String Query3 = $"select Varname , vartype from OutputTemplateLines where OutPutFileTemplateID = {filemasterid} and FileLineNo = 1 and ProfileId={ProfileID} order by OutPutFileTemplateID,OutputTemplateLinesID";
                DataTable dt3 = new DataTable();
                DataRow workRow3;
                SqlDataAdapter adpt3 = new SqlDataAdapter(Query3, con);
                adpt3.Fill(dt3);
                string qry = "";
                foreach (DataRow dv in dt3.Rows)
                {
                    string str_varname = dv[0].ToString().Trim();
                    string str_VarType = dv[1].ToString();
                    if (str_VarType[0] == 'V')
                    {
                        qry += str_varname;
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
                using (StreamWriter writer = File.CreateText(myfile))
                {
                    writer.Write(Header + "\r\n");
                    foreach (DataRow dr in data.Rows)
                    {
                        writer.Write(dr[0].ToString() + "\r\n");
                        count++;
                    }
                }
                string[] lines = File.ReadAllLines(myfile);
                if (fileext.ToLower().Trim() == ".mca" && lines.Length > 40000)
                {
                    int numFiles = (lines.Length) / 20000;
                    for (int i = 0; i <= numFiles; i++)
                    {
                        string outputFile = myfile.Replace(fileext, $"_Part_{i + 1}{fileext}");
                        using (StreamWriter writer = File.CreateText(outputFile))
                        {
                            writer.WriteLine(lines[0]);
                            for (int j = 1; (j <= 20000 && (j + i * 20000) <= lines.Length - 1); j++)
                            {
                                writer.WriteLine(lines[j + i * 20000]);
                            }
                        }

                    }
                    File.Delete(myfile);
                }
                using (SqlConnection con3 = new SqlConnection(connectionString))
                {
                    con3.Open();
                    using (SqlCommand cmd = new SqlCommand($"UPDATE [dbo].[DataGenProcessHDFile] SET  OutFlileStatus = 17 WHERE  CustProfileFileID = {CustProfileFileID} and DataGenProcessHDID={lastInsertedId}", con3))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteReader();
                    }
                }

            }
        }
    }
}
