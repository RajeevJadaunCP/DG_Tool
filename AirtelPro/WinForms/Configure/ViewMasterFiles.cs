using DG_Tool.HelperClass;
using DG_Tool.WinForms.Configure;
using DG_Tool.WinForms.Customer;
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

namespace DG_Tool.WinForms.Configure
{
    public partial class ViewMasterFiles : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public ViewMasterFiles()
        {
            InitializeComponent();
            var masterFiles = CommonClass.GetMasterFiles();

            if (masterFiles != null && masterFiles.Count > 0)
            {
                var listWithoutCol = masterFiles.Select(x => new { x.FileMasterID, x.FileName, x.FileDesc, x.FileNamingConv, x.FileIOID, x.Status }).ToList();

                dgvMasterFiles.DataSource = listWithoutCol;

                dgvMasterFiles.EnableHeadersVisualStyles = false;
                dgvMasterFiles.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                dgvMasterFiles.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvMasterFiles.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                //dgvMasterFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvMasterFiles.Columns["FileMasterId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                DataGridViewButtonColumn viewbutton = new DataGridViewButtonColumn();
                viewbutton.FlatStyle = FlatStyle.Popup;
                viewbutton.HeaderText = "View";
                viewbutton.Text = "View";
                viewbutton.UseColumnTextForButtonValue = true;
                viewbutton.Name = "view";
                viewbutton.Width = 60;

                if (dgvMasterFiles.Columns.Contains(viewbutton.Name = "View"))
                {

                }
                else
                {
                    dgvMasterFiles.Columns.Add(viewbutton);
                }
            }
        }
        private void dgvMasterFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMasterFiles.Columns[e.ColumnIndex].HeaderText == "View")
            {
                int FileMasterID = Convert.ToInt32(dgvMasterFiles.Rows[e.RowIndex].Cells["FileMasterID"].Value);
                if (IsFinalised(FileMasterID))
                {
                    MasterFileDetailedView masterFileDetailedView = new MasterFileDetailedView(FileMasterID);
                    masterFileDetailedView.ShowDialog();
                    this.Hide();
                }
                else
                {
                    AddMasterFiles addMasterFiles = new AddMasterFiles(FileMasterID);
                    addMasterFiles.ShowDialog();
                    this.Hide();
                }
            }
        }
        private bool IsFinalised(int FileMasterID)
        {
            bool flag = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT StatusID FROM FileMaster WHERE FileMasterID = @fileMasterID", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@fileMasterID", FileMasterID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["StatusID"]);
                            if (id == 10)
                                flag = true;
                            else if (id == 9)
                                return false;
                        }
                    }
                    else
                        flag = false;

                }
                return flag;
            }
        }

        private void btnAddNewFile_Click(object sender, EventArgs e)
        {
            AddMasterFiles files = new AddMasterFiles(0);
            files.ShowDialog();
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
