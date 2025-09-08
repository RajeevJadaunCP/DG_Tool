using DG_Tool.HelperClass;
using DG_Tool.Models;
using DG_Tool.WinForms.OutputFile;
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
    public partial class setupConfigureFile : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public setupConfigureFile(int profileId,string customerName, string circleName, string customerProfile)
        {
            InitializeComponent();
            GetFileStructureList();
            //GetAllFileType();

            pbxAddMoreFiles.Location = new Point(1053, 104);
            label4.Location = new Point(1093, 106);


            dgvFileSetup.Width = 1229;
            dgvFileSetup.Height = 457;

            groupBox1.Visible = false;

            lblProfileId.Text = profileId.ToString();
            lblCustomer.Text = customerName;
            lblCircle.Text = circleName;
            lblProfile.Text = customerProfile;
            
            lblCustomer1.Text = customerName;
            lblCircle1.Text = circleName;
            lblProfile1.Text = customerProfile;

            if (CreateCustomerProfile.profileID == 0)
                GetFileData(CustomerProfileList.customerProfileID);
            else
                GetFileData(CreateCustomerProfile.profileID);

        }
        private void GetFileStructureList()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT *FROM vw_FileStructureList", con))
                {
                    DataTable dt = new DataTable();
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    cbxFileStructure.DataSource = dt;
                    cbxFileStructure.ValueMember = "FileStructureId";
                    cbxFileStructure.DisplayMember = "FileStructureName";
                }
            }
        }

        private void GetFileData(int profileID)
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
                    dgvFileSetup.DataSource = dt;

                    dgvFileSetup.EnableHeadersVisualStyles = false;
                    dgvFileSetup.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                    dgvFileSetup.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgvFileSetup.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    //dgvFileSetup.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    DataGridViewButtonColumn editbutton = new DataGridViewButtonColumn();

                    editbutton.FlatStyle = FlatStyle.Popup;

                    editbutton.HeaderText = "Edit";
                    editbutton.Text = "Edit";
                    editbutton.UseColumnTextForButtonValue = true;
                    editbutton.Name = "Edit";

                    editbutton.Width = 60;

                    if (dgvFileSetup.Columns.Contains(editbutton.Name = "Edit"))
                    {

                    }
                    else
                    {
                        dgvFileSetup.Columns.Add(editbutton);
                    }
                }
            }
        }

        private void dgvFileSetup_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvFileSetup.Columns[e.ColumnIndex].HeaderText == "Edit")
            {
                pbxAddMoreFiles.Location = new Point(803, 104);
                label4.Location = new Point(839, 106);

                dgvFileSetup.Width = 978;
                dgvFileSetup.Height = 457;

                groupBox1.Visible = true;

                lblFileName.Text = dgvFileSetup.Rows[e.RowIndex].Cells["FileName"].Value.ToString();
                lblFileIOID.Text = dgvFileSetup.Rows[e.RowIndex].Cells["FileIOID"].Value.ToString();
                lblExtension.Text = dgvFileSetup.Rows[e.RowIndex].Cells["FileExtension"].Value.ToString();

                txtCustomerProfileFileId.Text = dgvFileSetup.Rows[e.RowIndex].Cells["CustomerProfileFileId"].Value.ToString();

                lblFieldId.Text = dgvFileSetup.Rows[e.RowIndex].Cells["CustomerProfileFileId"].Value.ToString();
                //txtCustProfileID.Text = dgvFileSetup.Rows[e.RowIndex].Cells["ProfileName"].Value.ToString();
                txtFileNamingConv.Text = dgvFileSetup.Rows[e.RowIndex].Cells["NamingConvention"].Value.ToString();
                txtFolderPath.Text = dgvFileSetup.Rows[e.RowIndex].Cells["FilePath"].Value.ToString();
                cbxFileStructure.SelectedText = dgvFileSetup.Rows[e.RowIndex].Cells["FileStructure"].Value.ToString();
                cbxIsEncryption.SelectedItem = (dgvFileSetup.Rows[e.RowIndex].Cells["Encrypt"].Value.ToString().ToLower() == "yes") ? "YES" : "NO";
                txtEncryptKey.Text = dgvFileSetup.Rows[e.RowIndex].Cells["EncryptKey"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCustomerProfileFileId.Text))
                {
                    DataTable dt = new DataTable();
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        //Todo
                        using (SqlCommand cmd = new SqlCommand("UPDATE CustProfileFile " +
                            "SET FileNamingConv = @fileNamingConv,FilePath = @filePath,FileStructure = @fileStructure,Encrypt = @encrypt,EncryptKey = @encryptKey " +
                            " WHERE CustProfileFileID = @custProfileFileID", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@fileNamingConv", txtFileNamingConv.Text);
                            cmd.Parameters.AddWithValue("@filePath", txtFolderPath.Text);
                            //cmd.Parameters.AddWithValue("@fileTypeID", cbxFileType.SelectedValue);
                            cmd.Parameters.AddWithValue("@fileStructure", cbxFileStructure.SelectedValue);
                            cmd.Parameters.AddWithValue("@encrypt", cbxIsEncryption.SelectedItem);
                            cmd.Parameters.AddWithValue("@encryptKey", txtEncryptKey.Text);
                            cmd.Parameters.AddWithValue("@custProfileFileID", txtCustomerProfileFileId.Text);

                            cmd.ExecuteReader();
                            if (CustomerProfileList.customerProfileID == 0)
                            {
                                GetFileData(CreateCustomerProfile.profileID);
                            }
                            else if (CreateCustomerProfile.profileID == 0)
                            {
                                GetFileData(CustomerProfileList.customerProfileID);
                            }

                            MessageBox.Show("Updated successfully",
                                                "Message",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information
                                                );
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Nothing to update",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation
                                        );
                }
            }
            catch(Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
            

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtFolderPath.Text = fbd.SelectedPath.ToString();
            }
        }

        private void pbxAddMoreFiles_Click(object sender, EventArgs e)
        {
            ConfigureFile configureFile = new ConfigureFile(lblCustomer.Text, lblCircle.Text, lblProfile.Text);
            configureFile.ShowDialog();
            this.Hide();
        }

        private void setupConfigureFile_Load(object sender, EventArgs e)
        {
            
        }

        private void txtEncryptKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar); 
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (CommonClass.PreClosingConfirmation() == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void lblCloseEdit_Click(object sender, EventArgs e)
        {
            pbxAddMoreFiles.Location = new Point(1053, 104);
            label4.Location = new Point(1093, 106);

            dgvFileSetup.Width = 1229;
            dgvFileSetup.Height = 457;

            groupBox1.Visible = false;
        }
    }
}
