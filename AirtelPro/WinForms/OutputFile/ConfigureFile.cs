using DG_Tool.HelperClass;
using DG_Tool.WinForms.Customer;
using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;

using System.Windows.Forms;

namespace DG_Tool.WinForms.OutputFile
{
    public partial class ConfigureFile : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        DataTable dt = new DataTable();
        public ConfigureFile(string customerName, string circleName, string customerProfile)
        {
            InitializeComponent();
            if (CreateCustomerProfile.profileID == 0)
                lblProfileId.Text = CustomerProfileList.customerProfileID.ToString();
            else
                lblProfileId.Text = CreateCustomerProfile.profileID.ToString();
            
            lblCustomer.Text = customerName;
            lblCircle.Text = circleName;
            lblProfile.Text = customerProfile;

            GetFiles();
        }
        private void GetFiles()
        {
            List<ShortMasterFile> files = new List<ShortMasterFile>();
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT *FROM vw_ConfigureFileMaster", con))
                {
                    cmd.CommandType = CommandType.Text;
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            files.Add(new ShortMasterFile
                            {
                                FileMasterID = reader["FileMasterID"].ToString(),
                                FileName = reader["FileName"].ToString(),
                                FileDesc = reader["FileDesc"].ToString(),
                                FileNamingConv = reader["FileNamingConv"].ToString(),
                                FileIOID = reader["FileIOID"].ToString(),
                                //FilePath = reader["FilePath"].ToString(),
                                //FileStructure = reader["FileStructure"].ToString(),
                                //FileExtn = reader["FileExtn"].ToString(),
                                //Encrypt = reader["Encrypt"].ToString(),
                                //EncryptKey = reader["EncryptKey"].ToString(),
                                //StatusID = reader["StatusID"].ToString()
                            });
                        }
                    }
                    dgvFileSelection.DataSource = files;

                    dgvFileSelection.EnableHeadersVisualStyles = false;
                    dgvFileSelection.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                    dgvFileSelection.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgvFileSelection.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    //dgvFileSelection.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dgvFileSelection.Columns["FileMasterID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            List<ShortMasterFile> files = ((List<ShortMasterFile>)dgvFileSelection.DataSource)
                .Where(p => p.IsSelected).ToList();
            int[] data = new int[files.Count];

            foreach (ShortMasterFile file in files)
            {
                data[i] = Convert.ToInt32(file.FileMasterID);
                i++;
            }
            if (CreateCustomerProfile.profileID == 0)
                SaveProfileFile(CustomerProfileList.customerProfileID, data);
            else
                SaveProfileFile(CreateCustomerProfile.profileID, data);
        }

        private void SaveProfileFile(int profileID, int[] masterFileIDs)
        {
            if (profileID > 0)
            {
                foreach (int id in masterFileIDs)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("ups_SaveProfileFile", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@profileID", profileID);
                            //Todo chnage customer ID dynamically
                            cmd.Parameters.AddWithValue("@customerID", 1);
                            cmd.Parameters.AddWithValue("@fileID", id);
                            cmd.ExecuteReader();


                        }
                    }
                }

                MessageBox.Show("successfully ",
                                                    "Message",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information
                                                    );
                setupConfigureFile setupConfigureFile = new setupConfigureFile(profileID,lblCustomer.Text, lblCircle.Text, lblProfile.Text);
                setupConfigureFile.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Error: No a valid profileId ",
                                                    "Error",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Warning
                                                    );
            }
        }

        private void dgvFileSelection_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvFileSelection.Columns[e.ColumnIndex].HeaderText == "Select")
            {
                if (e.RowIndex != -1)
                {
                    int fileID = Convert.ToInt32(dgvFileSelection.Rows[e.RowIndex].Cells["FileMasterID"].Value);
                    int profileId = (CustomerProfileList.customerProfileID == 0) ? CreateCustomerProfile.profileID : CustomerProfileList.customerProfileID;

                    if (DoesFileAlreadyExist(profileId, fileID))
                    {

                        dgvFileSelection.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;

                        MessageBox.Show("File already exist",
                                                        "Error",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Warning
                                                        );

                    }
                    else
                    {
                        if (dgvFileSelection.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Green)
                            dgvFileSelection.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        else
                            dgvFileSelection.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;
                    }
                }

            }
        }
        private bool DoesFileAlreadyExist(int customerProfileID, int masterFileID)
        {

            int id = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select CustProfileFileID from CustProfileFile Where CustProfileID = @customerProfileID and FileMasterID = @fileMasterID", con))
                {
                    SqlDataReader reader = null;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@customerProfileID", customerProfileID);
                    cmd.Parameters.AddWithValue("@fileMasterID", masterFileID);

                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["CustProfileFileID"].ToString());
                        }
                    }

                    if (id > 0)
                        return true;
                    else
                        return false;
                }
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
