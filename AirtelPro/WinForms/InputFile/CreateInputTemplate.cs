using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DG_Tool.WinForms.InputFile
{
    public partial class CreateInputTemplate : Form
    {
        public static string CustomerID;
        public static string CustomerName;

        public static string CircleID;
        public static string CircleName;

        public static string CustomerProfileID;
        public static string CustomerProfileName;

        public static string InputFileID;
        public static string InputFileName;

        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public CreateInputTemplate()
        {
            InitializeComponent();
        }

        private void CreateInputTemplate_Load(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter("select CustomerID, rtrim(CustomerName) as CustomerName from CustomerMaster", con))
                {
                    //Fill the DataTable with records from Table.
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    //Insert the Default Item to DataTable.
                    DataRow row = dt.NewRow();
                    row[0] = 0;
                    row[1] = "--Please select--";
                    dt.Rows.InsertAt(row, 0);

                    comboBox1.DisplayMember = "CustomerName";
                    comboBox1.ValueMember = "CustomerID";

                    comboBox1.DataSource = dt;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con1 = new SqlConnection(connectionString))
            {
                con1.Open();
                using (SqlDataAdapter sda1 = new SqlDataAdapter("select CircleID, rtrim(CircleName) as CircleName from CircleMaster where CustomerID='" + comboBox1.SelectedValue + "'", con1))
                {
                    //Fill the DataTable with records from Table.
                    DataTable dt1 = new DataTable();
                    sda1.Fill(dt1);

                    //Insert the Default Item to DataTable.
                    DataRow row1 = dt1.NewRow();
                    row1[0] = 0;
                    row1[1] = "--Please select--";
                    dt1.Rows.InsertAt(row1, 0);

                    comboBox2.DisplayMember = "CircleName";
                    comboBox2.ValueMember = "CircleID";

                    comboBox2.DataSource = dt1;
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con2 = new SqlConnection(connectionString))
            {
                con2.Open();
                using (SqlDataAdapter sda2 = new SqlDataAdapter("SELECT ProfileID, rtrim(ProfileName) as ProfileName FROM CustProfile  where CircleId='" + comboBox2.SelectedValue + "'", con2))
                {
                    //Fill the DataTable with records from Table.
                    DataTable dt2 = new DataTable();
                    sda2.Fill(dt2);

                    //Insert the Default Item to DataTable.
                    DataRow row2 = dt2.NewRow();
                    row2[0] = 0;
                    row2[1] = "--Please select--";
                    dt2.Rows.InsertAt(row2, 0);

                    comboBox3.DisplayMember = "ProfileName";
                    comboBox3.ValueMember = "ProfileID";

                    comboBox3.DataSource = dt2;
                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con2 = new SqlConnection(connectionString))
            {
                con2.Open();
                using (SqlDataAdapter sda2 = new SqlDataAdapter("SELECT FileMasterID, rtrim(FileName) as FileName FROM CustProfileFile  where CustomerID="+comboBox1.SelectedValue+" and CustProfileID="+comboBox3.SelectedValue+"", con2))
                {
                    //Fill the DataTable with records from Table.
                    DataTable dt2 = new DataTable();
                    sda2.Fill(dt2);

                    //Insert the Default Item to DataTable.
                    DataRow row2 = dt2.NewRow();
                    row2[0] = 0;
                    row2[1] = "--Please select--";
                    dt2.Rows.InsertAt(row2, 0);

                    comboBox4.DisplayMember = "FileName";
                    comboBox4.ValueMember = "FileMasterID";

                    comboBox4.DataSource = dt2;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0 && comboBox2.SelectedIndex != 0 && comboBox3.SelectedIndex != 0 && comboBox4.SelectedIndex != 0)
            {
                CustomerName = comboBox1.Text;
                CustomerID = comboBox1.SelectedValue.ToString();

                CircleName = comboBox2.Text;
                CircleID = comboBox2.SelectedValue.ToString();

                CustomerProfileName = comboBox3.Text;
                CustomerProfileID = comboBox3.SelectedValue.ToString();

                InputFileName = comboBox4.Text;
                InputFileID = comboBox4.SelectedValue.ToString();
                
                Input_File_Creation ifc = new Input_File_Creation();
                ifc.TopLevel = false;
                ifc.FormBorderStyle = FormBorderStyle.None;
                ifc.Dock = DockStyle.Fill;
                form_pnl.Controls.Add(ifc);
                ifc.Show();
                form_pnl.Visible = true;
                form_pnl.BringToFront();
            }
            else
            {

                MessageBox.Show("All fields are required: ",
                                                "Error",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information
                                                );
            }
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void form_pnl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
