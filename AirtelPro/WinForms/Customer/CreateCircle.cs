using DG_Tool.HelperClass;
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
    public partial class CreateCircle : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public static int circleID = 0;
        DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();

        public CreateCircle()
        {
            InitializeComponent();
            cbxCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            //GetStatus();
            var customerList = CommonClass.GetCustomer();
            if (customerList != null && customerList.Count > 0)
            {
                cbxCustomer.DataSource = customerList;
                cbxCustomer.DisplayMember = "CustomerName";
                cbxCustomer.ValueMember = "CustomerID";
            }

            GetCircleList(0);
        }
        //private void GetStatus()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.AddRange(new DataColumn[] { new DataColumn("Id", typeof(int)), new DataColumn("Status", typeof(string)) });
        //    dt.Rows.Add(1, "Active");
        //    dt.Rows.Add(0, "Inactive");

        //    //Insert the Default Item to DataTable.
        //    DataRow row = dt.NewRow();
        //    row[0] = 0;
        //    row[1] = "Please select";
        //    dt.Rows.InsertAt(row, 0);

        //    //Assign DataTable as DataSource.
        //    cbxStatus.DataSource = dt;
        //    cbxStatus.DisplayMember = "Status";
        //    cbxStatus.ValueMember = "Id";
        //}

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtCircle.Text.Length >= 3)
                {
                    if (IsValidInput())
                    {
                        if (!IsDuplicateCircle())
                        {
                            using (SqlConnection con = new SqlConnection(connectionString))
                            {
                                con.Open();
                                using (SqlCommand cmd = new SqlCommand("usp_CreateCircle", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@customerID", cbxCustomer.SelectedValue);
                                    cmd.Parameters.AddWithValue("@circleName", txtCircle.Text);
                                    cmd.Parameters.AddWithValue("@isActive", 1);
                                    cmd.ExecuteNonQuery();
                                    cbxCustomer.SelectedIndex = 0;
                                    txtCircle.Clear();
                                    txtCircle.Text = "";
                                    MessageBox.Show("Circle created successfully ",
                                                        "Message",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Information
                                                        );
                                    GetCircleList(0);
                                    
                                }
                            }
                        }
                        else
                            MessageBox.Show("Provided circle already exist");

                    }
                    else
                    {
                        MessageBox.Show("Please fill all the mandatory fields: ",
                                                    "Error",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information
                                                    );
                    }
                }
                else
                {
                    MessageBox.Show("Circle name should be greater than 3 character");
                }
                
            }
            catch (Exception ex)
            {
                
            }
        }

        private bool IsValidInput()
        {
            if (cbxCustomer.SelectedIndex >= 0 & !string.IsNullOrEmpty(txtCircle.Text))
            {
                return true;
            }
            else
                return false;
        }

        private void GetCircleList(int count)
        {
            var circulList = CommonClass.GetCircle(0);

            if (circulList != null && circulList.Count > 0)
            {
                dgvCircleList.DataSource = circulList;
                dgvCircleList.ClearSelection();

                foreach (DataGridViewColumn col in dgvCircleList.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                dgvCircleList.EnableHeadersVisualStyles = false;
                dgvCircleList.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                dgvCircleList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvCircleList.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

                dgvCircleList.Columns["CircelId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                //dgvCircleList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                if (count == 0)
                {
                    dgvButton.FlatStyle = FlatStyle.System;

                    dgvButton.HeaderText = "Action";
                    dgvButton.Name = "Action";
                    dgvButton.UseColumnTextForButtonValue = true;
                    dgvButton.Text = "Change Status";

                    dgvCircleList.Columns.Add(dgvButton);
                }
            }
        }
        private void SetStatus(int circleID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE CircleMaster SET IsActive = CASE IsActive WHEN 1 THEN 0 ELSE 1 END WHERE CircleID = @id", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", circleID);
                    cmd.ExecuteNonQuery();
                }
                GetCircleList(1);
            }
        }

         private void dgvCircleList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCircleList.Columns[e.ColumnIndex].HeaderText == "Action")
            {
                if (e.RowIndex != -1)
                {
                    circleID = Convert.ToInt32(dgvCircleList.Rows[e.RowIndex].Cells["CircelID"].Value);

                    SetStatus(circleID);
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

        private void txtCircle_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar);
        }
        
        private bool IsDuplicateCircle()
        {
            DataTable dt = new DataTable();
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using(SqlDataAdapter sda = new SqlDataAdapter($"SELECT CircleID FROM CircleMaster Where CustomerID = {circleID} and CircleName = '{txtCircle.Text.Trim()}'", con))
                {
                    sda.Fill(dt);
                }
            }
            if(dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        private void txtCircle_TextChanged(object sender, EventArgs e)
        {
            if(txtCircle.Text.Length == 0)
            {
                msgCustomerName.Visible = false;
            }
            else if (txtCircle.Text.Length > 2)
            {
                if(CircleAlreadyExist())
                {
                    msgCustomerName.Text = "Already exist";
                    msgCustomerName.ForeColor = Color.Red;
                    msgCustomerName.Visible = true;
                    btnSubmit.Enabled = false;
                }
                else
                {
                    msgCustomerName.Visible = false;
                    btnSubmit.Enabled = true;
                }
                
            }
            else
            {
                msgCustomerName.Text = "Must be greater than 2 character";
                msgCustomerName.ForeColor = Color.Red;
                msgCustomerName.Visible = true;
                btnSubmit.Enabled = false;
            }
        }

        private bool CircleAlreadyExist()
        {
            DataTable dt = new DataTable();
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using(SqlDataAdapter adapter = new SqlDataAdapter($"SELECT *FROM CircleMaster WHERE CustomerID= '{cbxCustomer.SelectedValue}' and CircleName= '{txtCircle.Text.Trim()}'", con))
                {
                    adapter.Fill(dt);

                    if(dt.Rows.Count > 0)
                        return true;
                    else
                        return false;

                }
            }
        }

        private void cbxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCircle.Clear();
        }
    }
}
