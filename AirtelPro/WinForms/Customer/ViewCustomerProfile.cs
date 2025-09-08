using DG_Tool.HelperClass;
using DG_Tool.Models;
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

namespace DG_Tool.WinForms.Customer
{
    public partial class ViewCustomerProfile : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private int customerProfileID = 0;
        public ViewCustomerProfile(int customerProfileID)
        {
            InitializeComponent();

            lblProfileId.Text = customerProfileID.ToString();

            lblCustomer1.Text = CustomerProfileList.customerName;
            lblCircle1.Text = CustomerProfileList.circleName;
            lblProfile1.Text = CustomerProfileList.customerProfile;

            this.customerProfileID = customerProfileID;
            var data = CommonClass.GetSingleProfileByProfileID(customerProfileID);
            GetFileData(customerProfileID);
            foreach (var i in data)
            {
                string status = (Convert.ToInt32(i.Status) == 1) ? "Active" : "Inactive";

                //if (status.Equals("Active"))
                //    lblStatus.BackColor = Color.Green;
                //else
                //    lblStatus.BackColor = Color.Red;

                lblCustomer.Text = i.CustomerName;
                lblCircle.Text = i.CircleName;
                lblProfile.Text = i.ProfileName;
                lblCreatedBy.Text =  i.CreatedBy;
                lblStatus.Text = status;
            }
        }

        private void GetAttributeById(int id)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                DataTable dt = new DataTable();
                using(SqlCommand cmd = new SqlCommand("SELECT pahd.HeaderName,pa.PFName FROM CustomerProfileAttribute cpa INNER JOIN ProfileAttributeHD pahd ON pahd.Id = cpa.ProfileAttributeHDID INNER JOIN ProfileAttribute pa ON pa.id = cpa.ProfieAttributeID WHERE CustomerProfileID = @id", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    dgvProfileAttribute.DataSource = dt;

                    dgvProfileAttribute.EnableHeadersVisualStyles = false;
                    dgvProfileAttribute.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                    dgvProfileAttribute.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgvProfileAttribute.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    dgvProfileAttribute.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
            }
        }
        private void GetFileData(int profileID)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    //Todo
                    using (SqlCommand cmd = new SqlCommand("uspGetCustomerProfile", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@customerProfileID", profileID);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(dt);
                        dgvCustomerProfileFile.DataSource = dt;

                        dgvProfileAttribute.EnableHeadersVisualStyles = false;
                        dgvProfileAttribute.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                        dgvProfileAttribute.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                        dgvProfileAttribute.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                        dgvProfileAttribute.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


                        //DataGridViewButtonColumn viewbutton = new DataGridViewButtonColumn();
                        //viewbutton.FlatStyle = FlatStyle.Popup;
                        //viewbutton.HeaderText = "Remove";
                        //viewbutton.Text = "Remove";
                        //viewbutton.UseColumnTextForButtonValue = true;
                        //viewbutton.Name = "Remove";
                        //viewbutton.Width = 60;

                        //if (dgvCustomerProfileFile.Columns.Contains(viewbutton.Name = "Remove"))
                        //{

                        //}
                        //else
                        //{
                        //    dgvCustomerProfileFile.Columns.Add(viewbutton);
                        //}
                    }
                }
                GetAttributeById(profileID);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void ViewCustomerProfile_Load(object sender, EventArgs e)
        {
            //int width = Screen.PrimaryScreen.Bounds.Width;
            //int height = Screen.PrimaryScreen.Bounds.Height;

            //this.Location = new Point(0, 0);
            //this.Size = new Size(width, height - 35);
        }

        private void dgvCustomerProfileFile_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvCustomerProfileFile.Columns[e.ColumnIndex].HeaderText == "Remove")
                {
                    int id = Convert.ToInt32(dgvCustomerProfileFile.Rows[e.RowIndex].Cells["CustProfileFileID"].Value);

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("UPDATE CustProfileFile SET IsActive = 0, IsDeleted = 1 WHERE CustProfileFileID = @customerProfileFileId", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@customerProfileFileId", id);
                            cmd.ExecuteNonQuery();

                        }
                    }
                    GetFileData(customerProfileID);
                }
            }
            catch(Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (CommonClass.PreClosingConfirmation() == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
