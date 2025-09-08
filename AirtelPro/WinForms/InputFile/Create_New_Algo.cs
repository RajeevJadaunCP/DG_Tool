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

namespace DG_Tool.WinForms.InputFile
{
    public partial class Create_New_Algo : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
       
        public Create_New_Algo()
        {
            InitializeComponent();
        }

        private void Create_New_Algo_Load(object sender, EventArgs e)
        {
            getLastAlgoID();
        }

        private void getLastAlgoID()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 AlgoID FROM AlgoMaster ORDER BY AlgoID DESC", con))
                {
                    SqlDataReader reader1 = cmd.ExecuteReader();

                    if (reader1.Read())
                    {
                        int last_num = Int32.Parse(reader1.GetValue(0).ToString().Trim());
                        textBox1.Text = (last_num+1).ToString();

                    }
                }
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text!="" & textBox3.Text!="" & comboBox1.SelectedIndex!=0 & textBox5.Text!="")
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand com = new SqlCommand("usp_CreateNewAlgo", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@algoid", textBox1.Text);
                        com.Parameters.AddWithValue("@algoname",textBox3.Text);
                        com.Parameters.AddWithValue("@algotype", GetFirstChar(comboBox1.Text));
                        com.Parameters.AddWithValue("@algodesc", textBox5.Text);
                        com.Parameters.AddWithValue("@fieldname", textBox2.Text);
                        com.Parameters.AddWithValue("@createby", "1");
                        com.Parameters.AddWithValue("@createdate", DateTime.Now);
                       
                        try
                        {
                            con.Open();
                            com.ExecuteNonQuery();
                            MessageBox.Show("Data Saved Successfully!");
                            
                            textBox2.Clear();
                            textBox3.Clear();
                            textBox5.Clear();
                            comboBox1.SelectedIndex= 0;

                            getLastAlgoID();
                        }
                        catch (Exception exe)
                        {

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Fill All Information!");
            }
        }
        public string GetFirstChar(string data)
        {
            return data.Substring(0, 1);
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
