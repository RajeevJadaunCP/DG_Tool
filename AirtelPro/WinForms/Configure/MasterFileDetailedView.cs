using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DG_Tool.WinForms.Configure
{
    public partial class MasterFileDetailedView : Form
    {
        int fileId = 0;
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public MasterFileDetailedView(int fileMasterID)
        {
            InitializeComponent();

            this.fileId = fileMasterID;
            GetMasterFileDetailsById(fileMasterID);
        }
        private void GetMasterFileDetailsById(int fileMasterID)
        {
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT *FROM vw_ConfigureFileMaster WHERE FileMasterID = @fileMasterID", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@fileMasterID", fileMasterID);

                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            lblFileName.Text = reader["FileName"].ToString();
                            lblfileDescription.Text = reader["FileDesc"].ToString();
                            lblFileIOID.Text = reader["FIleIOID"].ToString().Equals("O") ? "Output" : "Input";
                            lblFilePath.Text = reader["FilePath"].ToString();
                            lblNamingConvernsion.Text = reader["FileNamingConv"].ToString();
                            lblFileStructure.Text = reader["FileStructure"].ToString();
                            lblFIleExtension.Text = reader["FileExtn"].ToString();
                            lblEncryption.Text = reader["Encrypt"].ToString();
                            lblEncryptionKey.Text = reader["EncryptKey"].ToString();
                            lblStatus.Text = (reader["IsActive"].ToString()) == "0" ? "Inactive" : "Active";
                            btnSubmit.Text = (lblStatus.Text == "Inactive") ? "Activate" : "Deactivate";
                        }
                    }

                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("Are you sure " + (sender as Button).Text + " account!", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (res == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("UPDATE FileMaster SET IsActive = CASE IsActive WHEN 1 THEN 0 ELSE 1 END WHERE FileMasterID = @id", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@id", fileId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    GetMasterFileDetailsById(fileId);

                    MessageBox.Show("Status succesfully updated",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong:" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );

            }

        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
