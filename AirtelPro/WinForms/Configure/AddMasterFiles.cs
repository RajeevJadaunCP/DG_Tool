using DG_Tool.HelperClass;
using DG_Tool.Models;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace DG_Tool.WinForms.Configure
{
    public partial class AddMasterFiles : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        int fileMasterId = 0;
        public AddMasterFiles(int masterFileID)
        {
            InitializeComponent();
            this.fileMasterId = masterFileID;

            var fileStructireTypeList = CommonClass.GetFileStructureType();

            if (fileStructireTypeList != null && fileStructireTypeList.Count > 0)
            {
                fileStructireTypeList.Insert(0, new FileStructure
                {
                    FileStructureName = "----Select----",
                    FileStructureId = 0,
                });

                cbxFileStructure.DataSource = fileStructireTypeList;
                cbxFileStructure.ValueMember = "FileStructureId";
                cbxFileStructure.DisplayMember = "FileStructureName";
            }

            //Update
            if (masterFileID > 0)
            {
                GetMasterFileDetailsById(masterFileID);
            }
            else
            {
                cbxFileIOID.SelectedIndex = 0;
                cbxIsEncryption.SelectedIndex = 1;
            }
        }
        private void GetMasterFileDetailsById(int fileMasterID)
        {
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT *FROM FileMaster WHERE FileMasterID = @fileMasterID", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@fileMasterID", fileMasterID);

                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txtFileName.Text = reader["FileName"].ToString();
                            txtFileDescription.Text = reader["FileDesc"].ToString();
                            cbxFileIOID.Text = reader["FIleIOID"].ToString().Equals("I") ? "Input" : "Output";
                            txtFilePath.Text = reader["FilePath"].ToString();
                            txtFileNamingConvension.Text = reader["FileNamingConv"].ToString();
                            cbxFileStructure.Text = reader["FileStructure"].ToString();
                            txtFileExtension.Text = reader["FileExtn"].ToString();
                            cbxIsEncryption.Text = reader["Encrypt"].ToString();
                            txtEncryptionKey.Text = reader["EncryptKey"].ToString();
                            //lblStatus.Text = (reader["IsActive"].ToString()) == "0" ? "Inactive" : "Active";
                            btnSubmit.Text = "Finalize";
                        }
                    }

                }
            }
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = fbd.SelectedPath.ToString();
            }
            else
            {
                txtFilePath.Clear();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbxFileIOID.SelectedIndex > 0 && cbxFileStructure.SelectedIndex > 0)
                {
                    bool flag = IsValid(txtFileName.Text, txtFileDescription.Text, txtFilePath.Text, txtFileNamingConvension.Text, txtFileExtension.Text, txtEncryptionKey.Text);
                    if (flag)
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand("usp_SaveMasterFile", con))
                            {
                                string FileIOID = (cbxFileIOID.Text.Equals("Input")) ? "I" : "O";

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@fileMasterId", fileMasterId);
                                cmd.Parameters.AddWithValue("@fileName", txtFileName.Text);
                                cmd.Parameters.AddWithValue("@fileDesc", txtFileDescription.Text);
                                cmd.Parameters.AddWithValue("@fileNamingConvention", txtFileNamingConvension.Text);
                                cmd.Parameters.AddWithValue("@fileIOID", FileIOID);
                                cmd.Parameters.AddWithValue("@filePath", txtFilePath.Text);
                                cmd.Parameters.AddWithValue("@fileStructure", cbxFileStructure.SelectedValue);
                                cmd.Parameters.AddWithValue("@fileExtension", txtFileExtension.Text);
                                cmd.Parameters.AddWithValue("@encryption", cbxIsEncryption.SelectedItem);
                                cmd.Parameters.AddWithValue("@encryptionKey", txtEncryptionKey.Text);
                                
                                cmd.ExecuteReader();

                                MessageBox.Show("File saved successfully!",
                                    "Message",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information
                                    );
                                ViewMasterFiles viewMasterFiles = new ViewMasterFiles();
                                viewMasterFiles.ShowDialog();
                                this.Hide();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please fill the mandatory fields",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                    );
                    }
                }
                else
                {
                    MessageBox.Show("Please fill the mandatory fields",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while saving information",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                    );
            }
        }

        private bool IsValid(params string[] items)
        {
            bool flag = true;

            foreach (string i in items)
            {
                if (string.IsNullOrEmpty(i))
                {
                    flag = false;
                    return flag;
                }
            }
            return flag;
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFileName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        private void txtFileDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsSeparator(e.KeyChar);

        }

        private void txtFileNamingConvension_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsSeparator(e.KeyChar);

        }

        private void txtFileExtension_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsSeparator(e.KeyChar);
        }

        private void txtEncryptionKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsSeparator(e.KeyChar);
        }
    }
}
