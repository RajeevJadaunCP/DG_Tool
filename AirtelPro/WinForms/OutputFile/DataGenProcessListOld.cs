using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirtelPro.WinForms.OutputFile
{
    public partial class DataGenProcessList : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public DataGenProcessList()
        {
            InitializeComponent();
            lblCustomer.Text = OFProcessing.customer;
            lblCircle.Text = OFProcessing.circle;
            lblProfile.Text = OFProcessing.profile;
            GetGenProcessList();
        }

        private void GetGenProcessList()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                
                using (SqlCommand cmd = new SqlCommand("usp_DataGenProcessHDFile", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGVProcessList.DataSource = dt;

                    DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();

                    dgvButton.FlatStyle = FlatStyle.System;

                    dgvButton.HeaderText = "Process";
                    dgvButton.Name = "Process";
                    dgvButton.UseColumnTextForButtonValue = true;
                    dgvButton.Text = "Process";

                    dataGVProcessList.Columns.Add(dgvButton);


                }
            }
        }

        private void dataGVProcessList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = dataGVProcessList.CurrentCell.ColumnIndex;
            string columnName = dataGVProcessList.Columns[columnIndex].Name;

            if (columnIndex == 7  && columnName.Equals("FilePath"))
            {
                string filePath = dataGVProcessList.SelectedCells[0].Value.ToString();

                if (File.Exists(filePath))
                {
                    //Code to go to file location
                    //Process.Start("explorer.exe", "/select, " + filePath);

                    Process.Start("explorer.exe", filePath);
                }
                else
                {
                    MessageBox.Show("File not found: ",
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information
                                            );
                }




            }
            
        }

    }
}
