using AirtelPro.HelperClass;
using AirtelPro.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirtelPro.WinForms.OutputFile
{
    public partial class FileLotMaster : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public FileLotMaster()
        {
            InitializeComponent();

            var customerList = CommonClass.GetCustomer();
            var circulList = CommonClass.GetCircle(1);

            if (customerList != null && customerList.Count > 0)
            {
                customerList.Insert(0, new CustomerDetails
                {
                    CustomerName = "----Select----",
                    CustomerID = 0,
                });

                cbxCustomer.DataSource = customerList;
                cbxCustomer.DisplayMember = "CustomerName";
                cbxCustomer.ValueMember = "CustomerID";

            }

            if (circulList != null && circulList.Count > 0)
            {
                circulList.Insert(0, new Circle
                {
                    CircleName = "----Select----",
                    CircleID = 0,
                });
                cbxCircle.DataSource = circulList;
                cbxCircle.DisplayMember = "CircleName";
                cbxCircle.ValueMember = "CircleID";

            }

            FileLotMasterList(0,0, string.Empty);

            DataGridViewButtonColumn viewbutton = new DataGridViewButtonColumn();
            viewbutton.FlatStyle = FlatStyle.Popup;
            viewbutton.HeaderText = "View";
            viewbutton.Text = "View";
            viewbutton.UseColumnTextForButtonValue = true;
            viewbutton.Name = "view";
            viewbutton.Width = 60;

            if (dgvFileLotList.Columns.Contains(viewbutton.Name = "View"))
            {

            }
            else
            {
                dgvFileLotList.Columns.Add(viewbutton);
            }
        }

        //private void FileLotMasterList()
        //{
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {

        //        con.Open();
        //        using (SqlDataAdapter sda = new SqlDataAdapter("SELECT fm.ID,cm.CustomerName,cl.CircleName,cp.ProfileName,fm.Quantity,fm.FileReceivedDate,fm.DataGenProcessDate,fm.DataGenProcessStatus,fm.OutFileProcessDate,fm.DataGenProcessStatusID from filelotmaster fm INNER Join CustomerMaster cm on  fm.CustomerID = cm.CustomerID INNER Join CircleMaster cl on fm.CircleID = cl.CircleID INNER Join CustProfile cp on fm.ProfileID = cp.ProfileID", con))
        //        {
        //            DataTable dt = new DataTable();
        //            sda.Fill(dt);

        //            dgvFileLotList.DataSource = dt;

        //            //dgvBriefList.EnableHeadersVisualStyles = false;
        //            //dgvBriefList.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
        //            //dgvBriefList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        //            //dgvBriefList.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
        //            //dgvBriefList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        //            //dgvBriefList.Columns["RolesId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //        }
        //    }
        //}
        private void FileLotMasterList(int customerId,int circleId,string dateTime)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_DataGenProcessFileLots", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerID", customerId);
                    cmd.Parameters.AddWithValue("@circleID", circleId);
                    cmd.Parameters.AddWithValue("@fileReceivedDate", dateTime);

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    dgvFileLotList.DataSource = dt;

                    dgvFileLotList.EnableHeadersVisualStyles = false;
                    dgvFileLotList.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                    dgvFileLotList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgvFileLotList.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

                    //dgvBriefList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    //dgvBriefList.Columns["RolesId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        private void dgvFileLotList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvFileLotList.Columns[e.ColumnIndex].HeaderText == "View")
            {
                dgvFileLotList.CurrentRow.Selected = true;
                int lotid = Convert.ToInt32(dgvFileLotList.Rows[e.RowIndex].Cells["ID"].Value);

                //
                OFSatusList oFSatusList = new OFSatusList(lotid);
                oFSatusList.ShowDialog();
            }
        }

        private void cbxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbxCustomer.SelectedIndex > 0 || cbxCircle.SelectedIndex > 0)
                FileLotMasterList(Convert.ToInt32(cbxCustomer.SelectedValue),Convert.ToInt32(cbxCircle.SelectedValue),txtFileReceivedDate.Text);
        }

        private void pbFromCalander_Click(object sender, EventArgs e)
        {
            if (receivedDateCalendar.Visible == true)
                receivedDateCalendar.Visible = false;
            else
                receivedDateCalendar.Visible = true;
        }


        private void receivedDateCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            txtFileReceivedDate.Text = receivedDateCalendar.SelectionRange.Start.ToString("yyyy-MM-dd");
            receivedDateCalendar.Visible = false;

            FileLotMasterList((int)cbxCustomer.SelectedValue, (int)cbxCircle.SelectedValue, txtFileReceivedDate.Text);
        }

        private void cbxCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCustomer.SelectedIndex > 0 || cbxCircle.SelectedIndex > 0)
                FileLotMasterList(Convert.ToInt32(cbxCustomer.SelectedValue), Convert.ToInt32(cbxCircle.SelectedValue), txtFileReceivedDate.Text);
        }

        private void pbReresh_Click(object sender, EventArgs e)
        {
            cbxCustomer.SelectedIndex = 0;
            cbxCircle.SelectedIndex = 0;

            txtFileReceivedDate.Clear();

            FileLotMasterList(0, 0, string.Empty);
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
