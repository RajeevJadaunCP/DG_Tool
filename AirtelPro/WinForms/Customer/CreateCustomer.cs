using DG_Tool.HelperClass;
using DG_Tool.Models;
using DG_Tool.WinForms.Authentication;
using System;using CardPrintingApplication;
using System.Collections;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DG_Tool.WinForms.Customer
{
    public partial class CreateCustomer : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();
        int varcounter = 0;
        public CreateCustomer()
        {
            InitializeComponent();
            GetAllCustomer();
            getLastCusID();
            txtCustomerCode.Text = CusCodeGenerate();

            dgvCustomerList.ClearSelection();
            dgvCustomerList.CurrentCell = null;
        }
        private void GetAllCustomer()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                DataTable dt = new DataTable();
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter("Select * from Vw_GetCustomer", con))
                {
                    sda.Fill(dt);

                    dgvCustomerList.DataSource = dt;
                    
                    dgvCustomerList.ClearSelection();


                    foreach (DataGridViewColumn col in dgvCustomerList.Columns)
                    {
                        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    dgvCustomerList.EnableHeadersVisualStyles = false;
                    dgvCustomerList.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                    dgvCustomerList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgvCustomerList.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    //dgvCustomerList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dgvCustomerList.Columns["CustomerId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dgvButton.FlatStyle = FlatStyle.System;

                    dgvButton.HeaderText = "Action";
                    dgvButton.Name = "Action";
                    dgvButton.UseColumnTextForButtonValue = true;
                    dgvButton.Text = "Change Status";



                    if (dgvCustomerList.Columns.Contains(dgvButton.Name = "Action"))
                    {

                    }
                    else
                    {
                        dgvCustomerList.Columns.Add(dgvButton);
                    }
                }
            }
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCustomerName.Text))
            {
                if(!DuplicateCheck())
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("usp_SaveCustomer", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@customerCode", txtCustomerCode.Text);
                            cmd.Parameters.AddWithValue("@customerName", txtCustomerName.Text);
                            cmd.Parameters.AddWithValue("@createdBy", LoginPage.primaryId);
                            cmd.ExecuteReader();
                            MessageBox.Show("Customer created successfully ",
                                                        "Message",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Information
                                                        );

                            GetAllCustomer();
                            txtCustomerName.Clear();
                            txtCustomerCode.Text = CusCodeGenerate();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Provided customer name already exist");
                }
            }
            else
            {
                MessageBox.Show("Please Enter Customer Name ",
                                                   "Message",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Information
                                                   );
            }
        }
        private void SetStatus(int roleID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE CustomerMaster SET IsActive = CASE IsActive WHEN 1 THEN 0 ELSE 1 END WHERE CustomerID = @id", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", roleID);
                    cmd.ExecuteNonQuery();
                }
                GetAllCustomer();
            }
        }
        private void dgvCustomerList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomerList.Columns[e.ColumnIndex].HeaderText == "Action")
            {
                if (e.RowIndex != -1)
                {
                    int roleID = Convert.ToInt32(dgvCustomerList.Rows[e.RowIndex].Cells["CustomerID"].Value);

                    SetStatus(roleID);
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
        private void getLastCusID()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 CustomerCode FROM CustomerMaster ORDER BY CustomerCode DESC", con))
                {
                    SqlDataReader reader1 = cmd.ExecuteReader();

                    if (reader1.Read())
                    {
                        string mydata = reader1.GetValue(0).ToString().Trim();

                        string res = mydata.Substring(mydata.Length - 3);

                        varcounter = Int32.Parse(res);
                    }
                }
                con.Close();
            }
        }
        public string CusCodeGenerate()
        {
            string charvar = "C";

            varcounter++;

            string sendval = charvar + varcounter.ToString("000");

            return sendval;

        }
        private void txtCustomerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar);
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }
        private bool DuplicateCheck()
        {
            DataTable dt = new DataTable();

            using(SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using(SqlDataAdapter sda = new SqlDataAdapter($"Select CustomerId from CustomerMaster WHERE CustomerName = '{txtCustomerName.Text.Trim()}' ",con))
                {
                    sda.Fill(dt);
                }
            }
            if(dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            if(txtCustomerName.Text.Length == 0)
            {
                msgCustomerName.Visible = false;
            }
            else if(txtCustomerName.Text.Length > 1)
            {
                DataTable dt = new DataTable();

                msgCustomerName.Visible =  false;
                btnSubmit.Enabled = true;

                using(SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open() ;
                    using(SqlDataAdapter adapter = new SqlDataAdapter($"Select CustomerID from CustomerMaster Where CustomerName = '{txtCustomerName.Text.Trim()}'",con))
                    {
                        adapter.Fill(dt);
                        if(dt != null && dt.Rows.Count > 0)
                        {
                            msgCustomerName.Text = "Customer name already exist";
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
                }

            }
            else
            {
                msgCustomerName.Text = "Atleast 2 character";
                msgCustomerName.ForeColor = Color.Red;
                msgCustomerName.Visible = true;
                btnSubmit.Enabled = false;
            }
        }
    }
}
