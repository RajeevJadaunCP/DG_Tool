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

namespace DG_Tool
{
    public partial class AllCustomers : Form
    {
        public static int id = 0;
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public AllCustomers()
        {
            InitializeComponent();
            GetAllCustomerProfile();
            var customerList = CommonClass.GetCustomer();
            var circulList = CommonClass.GetCircle(1);
            if (customerList != null && customerList.Count > 0)
            {
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
        }
        private void GetAllCustomerProfile()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_SearchCustomerProfile", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerID", 0);
                    cmd.Parameters.AddWithValue("@circleID", 0);
                    cmd.Parameters.AddWithValue("@other", txtSearch.Text);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        dataGridViewCustomer.DataSource = dt;
                    }

                }
            }
        }
        private void dataGridViewCustomerDetails_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridViewCustomer.CurrentRow.Index != -1)
            {

                DataGridView _dgvCurrentRow = new DataGridView();
                int id = Convert.ToInt32(_dgvCurrentRow.SelectedCells[0].Value);
            }
        }
        private void dataGridViewCustomerDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                dataGridViewCustomer.CurrentRow.Selected = true;
                id = Convert.ToInt32(dataGridViewCustomer.Rows[e.RowIndex].Cells["ProfileID"].Value);

                CreateCustomerProfile register = new CreateCustomerProfile();
                register.Show();
                this.Hide();
            }

        }
        private void cbxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCustomer.SelectedIndex > 0)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_SearchCustomerProfile", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@customerID", cbxCustomer.SelectedValue);
                        cmd.Parameters.AddWithValue("@circleID", 0);
                        cmd.Parameters.AddWithValue("@other", string.Empty);

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGridViewCustomer.DataSource = dt;
                        }
                    }
                }
            }

        }
        private void cbxCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCircle.SelectedIndex > 0)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_SearchCustomerProfile", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@customerID", 0);
                        cmd.Parameters.AddWithValue("@circleID", cbxCircle.SelectedValue);
                        cmd.Parameters.AddWithValue("@other", string.Empty);

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGridViewCustomer.DataSource = dt;
                        }
                    }
                }
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_SearchCustomerProfile", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerID", 0);
                    cmd.Parameters.AddWithValue("@circleID", 0);
                    cmd.Parameters.AddWithValue("@other", txtSearch.Text);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        dataGridViewCustomer.DataSource = dt;
                    }

                }
            }
        }
        private void btnAddCustomerProfile_Click(object sender, EventArgs e)
        {
            CreateCustomerProfile createCustomerProfile = new CreateCustomerProfile();
            createCustomerProfile.Show();
            this.Hide();
        }

    }
}
