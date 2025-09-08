using DG_Tool.WinForms.Authentication;
using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DG_Tool.WinForms.GenerateTemplate
{
    public partial class CreateOutfileHeader : Form
    {
        public static int profileid = Convert.ToInt32(CreateTemplate.CustomerProfileID);
        public static int custid = Convert.ToInt32(CreateTemplate.CustomerID);
        public static int fileid = Convert.ToInt32(ProcessData.OutTempID);
        public static string custname = CreateTemplate.CustomerName;
        public static string circlename = CreateTemplate.CircleName;
        public static string filename = ProcessData.OutTempName;
        string ConStr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public CreateOutfileHeader()
        {
            InitializeComponent();
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton2.Checked)
                {
                    if (comboBox1.SelectedIndex > 0)
                    {
                        string header = "{" + comboBox1.SelectedItem + "}\r\n";
                        using (SqlConnection conn = new SqlConnection(ConStr))
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand("CreateOutfileHeader", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@ProfileID", profileid);
                                cmd.Parameters.AddWithValue("@ProfileFileID", fileid);
                                cmd.Parameters.AddWithValue("@User", LoginPage.username);
                                cmd.Parameters.AddWithValue("@Header", header);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Header Added Sucessfully.");
                    }
                    else
                    {
                        MessageBox.Show("Please Select Header Variable.");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(richTextBox1.Text.Trim()))
                    {
                        MessageBox.Show("Please Enter the Header details to proceed.");
                    }
                    else
                    {
                        string header = richTextBox1.Text + "\r\n";
                        using (SqlConnection conn = new SqlConnection(ConStr))
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand("CreateOutfileHeader", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure; 
                                cmd.Parameters.AddWithValue("@ProfileID", profileid);
                                cmd.Parameters.AddWithValue("@ProfileFileID", fileid);
                                cmd.Parameters.AddWithValue("@User", LoginPage.username);
                                cmd.Parameters.AddWithValue("@Header", header);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Header Added Sucessfully.");
                        richTextBox1.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateOutfileHeader_Load(object sender, EventArgs e)
        {
            try
            {
                custname = CreateTemplate.CustomerName;
                circlename = CreateTemplate.CircleName;
                filename = ProcessData.OutTempName;
                label1.Text = filename;
                label3.Text = circlename;
                label8.Text = custname;
                using (SqlConnection conn = new SqlConnection(ConStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand($"SELECT VarDes FROM InPutDataTemplate WHERE ProfileID={profileid}", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        comboBox1.Items.Add("--Select Header Variable--");
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["VarDes"].ToString());
                        }
                        comboBox1.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Visible = true;
            richTextBox1.ReadOnly= true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.ReadOnly = false;
            comboBox1.Visible = false;
        }
    }
}
