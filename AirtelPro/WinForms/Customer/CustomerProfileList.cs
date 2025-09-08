using DG_Tool.HelperClass;
using DG_Tool.Models;
using DG_Tool.WinForms.Customer;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DG_Tool
{
    public partial class CustomerProfileList : Form
    {
        //public static int id = 0;
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public static int customerProfileID = 0;
        public static string customerName = string.Empty;
        public static string circleName = string.Empty;
        public static string customerProfile = string.Empty;
        public CustomerProfileList()
        {
            InitializeComponent();
            GetAllCustomerProfile();
            var customerList = CommonClass.GetCustomer();
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

                        dataGridViewCustomer.EnableHeadersVisualStyles = false;
                        dataGridViewCustomer.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                        dataGridViewCustomer.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                        dataGridViewCustomer.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

                        dataGridViewCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        dataGridViewCustomer.Columns["ProfileID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                        dataGridViewCustomer.ClearSelection();

                        DataGridViewButtonColumn editbutton = new DataGridViewButtonColumn();
                        editbutton.FlatStyle = FlatStyle.Popup;
                        editbutton.HeaderText = "Edit Button";
                        editbutton.Text = "Edit";
                        editbutton.UseColumnTextForButtonValue = true;
                        editbutton.Name = "SelectFile";
                        editbutton.Width = 60;


                        DataGridViewButtonColumn viewbutton = new DataGridViewButtonColumn();
                        viewbutton.FlatStyle = FlatStyle.Popup;
                        viewbutton.HeaderText = "View Button";
                        viewbutton.Text = "View";
                        viewbutton.UseColumnTextForButtonValue = true;
                        viewbutton.Name = "view";
                        viewbutton.Width = 60;

                        if (dataGridViewCustomer.Columns.Contains(editbutton.Name = "SelectFile") && dataGridViewCustomer.Columns.Contains(viewbutton.Name = "View"))
                        {

                        }
                        else
                        {
                            dataGridViewCustomer.Columns.Add(editbutton);
                            dataGridViewCustomer.Columns.Add(viewbutton);
                        }

                        dataGridViewCustomer.Columns["InputFileCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridViewCustomer.Columns["OutFileCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridViewCustomer.Columns["InputFileProcessed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }

                }
            }
        }
        private void cbxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCustomer.SelectedIndex > 0)
            {
                var circulList = CommonClass.GetCircle(Convert.ToInt32(cbxCustomer.SelectedValue));

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

                            dataGridViewCustomer.EnableHeadersVisualStyles = false;
                            dataGridViewCustomer.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                            dataGridViewCustomer.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                            dataGridViewCustomer.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                            dataGridViewCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

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

                            dataGridViewCustomer.EnableHeadersVisualStyles = false;
                            dataGridViewCustomer.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                            dataGridViewCustomer.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                            dataGridViewCustomer.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                            dataGridViewCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

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

                        dataGridViewCustomer.EnableHeadersVisualStyles = false;
                        dataGridViewCustomer.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                        dataGridViewCustomer.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                        dataGridViewCustomer.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                        dataGridViewCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    }

                }
            }
        }
        private void btnAddCustomerProfile_Click(object sender, EventArgs e)
        {
            this.Hide();
            CreateCustomerProfile createCustomerProfile = new CreateCustomerProfile();
            createCustomerProfile.ShowDialog();
            createCustomerProfile.TopMost= true;
          
        }
        private void dataGridViewCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCustomer.Columns[e.ColumnIndex].HeaderText == "Edit Button")
            {
                if (e.RowIndex != -1)
                {
                    dataGridViewCustomer.CurrentRow.Selected = true;
                    customerProfileID = Convert.ToInt32(dataGridViewCustomer.Rows[e.RowIndex].Cells["ProfileID"].Value);
                    customerName = dataGridViewCustomer.Rows[e.RowIndex].Cells["CustName"].Value.ToString();
                    circleName = dataGridViewCustomer.Rows[e.RowIndex].Cells["CirclName"].Value.ToString();
                    customerProfile = dataGridViewCustomer.Rows[e.RowIndex].Cells["ProfileName"].Value.ToString();

                    setupConfigureFile setupConfigureFile = new setupConfigureFile(customerProfileID, customerName, circleName, customerProfile);
                    setupConfigureFile.ShowDialog();
                    this.Hide();
                }
            }
            else if (dataGridViewCustomer.Columns[e.ColumnIndex].HeaderText == "View Button")
            {
                if (e.RowIndex != -1)
                {
                    int customerProfileID = Convert.ToInt32(dataGridViewCustomer.Rows[e.RowIndex].Cells["ProfileID"].Value);

                    customerName = dataGridViewCustomer.Rows[e.RowIndex].Cells["CustName"].Value.ToString();
                    circleName = dataGridViewCustomer.Rows[e.RowIndex].Cells["CirclName"].Value.ToString();
                    customerProfile = dataGridViewCustomer.Rows[e.RowIndex].Cells["ProfileName"].Value.ToString();

                    ViewCustomerProfile viewCustomerProfile = new ViewCustomerProfile(customerProfileID);
                    viewCustomerProfile.ShowDialog();
                    this.Hide();
                }
            }
        }
        private void CustomerProfileList_Load(object sender, EventArgs e)
        {
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;

            this.Location = new Point(0, 0);
            this.Size = new Size(width, height - 35);
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (CommonClass.PreClosingConfirmation() == DialogResult.Yes)
            {
                this.Close();
            }
        }
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }
    }
}
