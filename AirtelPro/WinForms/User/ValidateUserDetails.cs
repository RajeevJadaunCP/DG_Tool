using DG_Tool.HelperClass;
using DG_Tool.WinForms.Authentication;
using CardPrintingApplication;
using System;
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

namespace DG_Tool.WinForms.User
{
    public partial class ValidateUserDetails : Form
    {
        string ConStr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public ValidateUserDetails()
        {
            InitializeComponent();

            CaptureUserActivity captureUserActivity = new CaptureUserActivity();
            captureUserActivity.UserActivity("Validate User");

            GetSignUpEmpShowOrNot();

            GetEmployeeIDs();
            GetRoles();

            if (UserDetails.id != 0)
            {
                cbxEmpId.Enabled = false;
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlDataReader reader = null;
                    using (SqlCommand cmd = new SqlCommand("Select * From UserDetails WHERE Id = @Id", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Id", UserDetails.id);

                        reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                lblHeading.Text = "Validate User";
                                btnSubmit.Text = "Active";

                                if (reader["EmployeeId"].ToString().Contains("USR"))
                                {
                                    cbxEmpId.SelectedValue = reader["EmployeeId"].ToString();
                                    cbxRole.SelectedValue = reader["RoleId"].ToString();
                                    txtUsername.Text = reader["Username"].ToString();
                                    txtName.Text = reader["Name"].ToString();
                                    txtEmail.Text = reader["Email"].ToString();
                                    txtMobile.Text = reader["Contact"].ToString();
                                }
                                else
                                {
                                    cbxEmpId.SelectedValue = reader["EmployeeId"].ToString();
                                }
                            }
                        }
                    }
                }
            }


        }

        private void GetRoles()
        {
            //usp_getRoles
            SqlDataAdapter da = new SqlDataAdapter();

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_getRoles", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cbxRole.DataSource = dt;
                    cbxRole.DisplayMember = "RoleName";
                    cbxRole.ValueMember = "RoleId";
                    cbxRole.SelectedIndex = 1;      //by defaultuser
                }
            }
        }

        private void GetEmployeeIDs()
        {
            SqlDataAdapter da = new SqlDataAdapter();

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_GetEmployeeId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    //Insert the Default Item to DataTable.
                    DataRow row = dt.NewRow();
                    row[0] = " ----Select---- ";
                    dt.Rows.InsertAt(row, 0);

                    cbxEmpId.DataSource = dt;
                    cbxEmpId.DisplayMember = "EmployeeCode";
                    cbxEmpId.ValueMember = "EmployeeCode";
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (UserDetails.id != 0)
            {
                int status = 10;

                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_CreateUpdateUserDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@primaryId", UserDetails.id);

                        cmd.Parameters.AddWithValue("@employeeId", cbxEmpId.Text);
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@contact", txtMobile.Text);
                        cmd.Parameters.AddWithValue("@userName", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@password", "");
                        cmd.Parameters.AddWithValue("@roleId", cbxRole.SelectedValue);
                        cmd.Parameters.AddWithValue("@createdBy", NewLogin.primaryId);
                        cmd.Parameters.AddWithValue("@status", status);

                        cmd.ExecuteReader();

                        MessageBox.Show("User Verified successfully",
                                "Message",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                                );

                        this.Hide();

                        UserDetails userDetails = new UserDetails();
                        userDetails.ShowDialog();
                    }
                }
            }
            else
                MessageBox.Show("Please fill the mandatory fields",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                                );
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel2.ClientRectangle,
                        Color.Red, 1, ButtonBorderStyle.Dotted, // left
                        Color.Red, 1, ButtonBorderStyle.Dotted, // top
                        Color.Red, 1, ButtonBorderStyle.Dotted, // right
                        Color.Red, 1, ButtonBorderStyle.Dotted);// bottom
        }

        private void cbxEmpId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxEmpId.SelectedIndex != 0 && !string.IsNullOrEmpty(cbxEmpId.Text))
            {
                SqlDataReader reader = null;

                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sup_EmployeeDetailsFromCopal", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@empid", cbxEmpId.Text);

                        reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                txtUsername.Text = cbxEmpId.Text;
                                txtName.Text = reader["FirstName"].ToString();
                                txtEmail.Text = reader["EMailID"].ToString();
                                txtMobile.Text = reader["Mobile"].ToString();
                                txtUsername.ReadOnly = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("No record found",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                                );
                        }



                    }
                }
                if (UserDetails.id == 0)
                {
                    txtUsername.ReadOnly = true;
                    txtName.ReadOnly = true;
                    txtEmail.ReadOnly = true;
                    txtMobile.ReadOnly = true;

                }
                else
                {
                    txtUsername.ReadOnly = false;
                    txtName.ReadOnly = false;
                    txtEmail.ReadOnly = false;
                    txtMobile.ReadOnly = false;
                }


            }
            else
            {
                txtUsername.ReadOnly = true;
                txtName.ReadOnly = false;
                txtEmail.ReadOnly = false;
                txtMobile.ReadOnly = false;

                txtUsername.Clear();
                txtName.Clear();
                txtEmail.Clear();
                txtMobile.Clear();
            }

        }

        private void GetSignUpEmpShowOrNot()
        {
            using (SqlConnection con3 = new SqlConnection(ConStr))
            {
                SqlCommand com3 = new SqlCommand("select ParameterCharVal from DataToolParameters where ParameterID=2", con3);
                con3.Open();
                SqlDataReader reader3 = com3.ExecuteReader();

                if (reader3.HasRows)
                {
                    if (reader3.Read())
                    {
                        if (reader3.GetValue(0).ToString().Trim().Equals("N"))
                        {
                            lblEmpId.Hide();
                            cbxEmpId.Hide();
                            label1.Hide();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Parameter not found!");
                }
                con3.Close();

            }
        }

        private void txtMobile_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMobile.Text.Trim()))
            {
                msgMobile.Text = "Mobile Number is required";
                msgMobile.ForeColor = Color.Red;
                msgMobile.Visible = true;
                //btnSubmit.Enabled = false;
            }
            else if (txtMobile.Text.Length > 0 && txtMobile.Text.Length < 10)
            {
                msgMobile.Text = "Must be 10-digits";
                msgMobile.ForeColor = Color.Red;
                msgMobile.Visible = true;
                //btnSubmit.Enabled = false;
            }
            else
            {
                msgMobile.ForeColor = Color.Green;
                msgMobile.Visible = false;
            }

            ValidateAllFields();
        }
        private void ValidateAllFields()
        {
            if (msgMobile.ForeColor == Color.Red || msgEmailValidation.ForeColor == Color.Red)
            {
                btnSubmit.Enabled = false;
            }
            else
                btnSubmit.Enabled = true;
        }
    }
}
