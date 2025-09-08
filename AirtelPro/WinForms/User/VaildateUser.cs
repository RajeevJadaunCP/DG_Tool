using DG_Tool.HelperClass;
using DG_Tool.WinForms.Authentication;
using CardPrintingApplication;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DG_Tool.WinForms.User
{
    public partial class VaildateUser : Form
    {
        string ConStr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public VaildateUser()
        {
            InitializeComponent();

            CaptureUserActivity captureUserActivity = new CaptureUserActivity();
            captureUserActivity.UserActivity("Validate User");

            GetSignUpEmpShowOrNot();

            GetEmployeeIDs();
            GetRoles();

            if (AddUser.pid != 0)
            {
                cbxEmpId.Enabled = false;
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlDataReader reader = null;
                    using (SqlCommand cmd = new SqlCommand("Select * From UserDetails WHERE Id = @Id", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Id", AddUser.pid);

                        reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                lblHeading.Text = "Validate User";
                                btnSubmit.Text = "Active";

                                //
                                if (reader["EmployeeId"].ToString().Contains("USR"))
                                {
                                    cbxEmpId.SelectedValue = reader["EmployeeId"].ToString();
                                    cbxRole.SelectedValue = reader["RoleId"].ToString();
                                    txtLoginId.Text = reader["Username"].ToString();
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
            if(!string.IsNullOrEmpty(txtLoginId.Text.Trim()) && !string.IsNullOrEmpty(txtName.Text.Trim()) && !string.IsNullOrEmpty(txtEmail.Text.Trim()) && !string.IsNullOrEmpty(txtMobile.Text.Trim()) )
            {
                if (AddUser.pid != 0)
                {
                    int status = 10;

                    if (!ValidateFields())
                    {
                        using (SqlConnection con = new SqlConnection(ConStr))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand("usp_CreateUpdateUserDetails", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@primaryId", AddUser.pid);

                                cmd.Parameters.AddWithValue("@employeeId", cbxEmpId.Text);
                                cmd.Parameters.AddWithValue("@name", txtName.Text);
                                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                                cmd.Parameters.AddWithValue("@contact", txtMobile.Text);
                                cmd.Parameters.AddWithValue("@userName", txtLoginId.Text);
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
                    {
                        using (SqlConnection con = new SqlConnection(ConStr))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand($"Update UserDetails SET Status = 10, IsActive=1 WHERE id = {AddUser.pid}", con))
                            {
                                cmd.CommandType = CommandType.Text;

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
                }
                else
                    MessageBox.Show("Please fill the mandatory fields",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                    );
            }
            else
                MessageBox.Show("Please fill the mandatory fields",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                                );
        }

        private bool ValidateFields()
        {
            int hasLoginId = 0;
            int hasEmail = 0;
            int hasMobile = 0;
            SqlDataReader reader = null;

            using(SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                using(SqlCommand cmd = new SqlCommand("usp_ValidateUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username",txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email",txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Mobile",txtMobile.Text.Trim());

                    reader = cmd.ExecuteReader();

                    if(reader != null & reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            hasLoginId = Convert.ToInt32(reader["LoginId"]);
                            hasEmail = Convert.ToInt32(reader["Email"]);
                            hasMobile = Convert.ToInt32(reader["Mobile"]);
                        }
                    }

                }
            }
            if(hasLoginId != 0 || hasEmail != 0 || hasMobile != 0)
            {
                if (hasLoginId > 0)
                {
                    msglblLoginId.ForeColor = Color.Red;
                    msglblLoginId.Text = "Login Id already exist";
                    msglblLoginId.Visible = true;
                }
                if (hasEmail > 0)
                {
                    msgEmailValidation.ForeColor = Color.Red;
                    msgEmailValidation.Text = "Email already exist";
                    msgEmailValidation.Visible = true;
                }
                if (hasMobile > 0)
                {
                    msgMobile.ForeColor = Color.Red;
                    msgMobile.Text = "Mobile number already exist";
                    msgMobile.Visible = true;
                }

                return true;
            }
            else
            {
                return false;
            }
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
                                txtLoginId.Text = cbxEmpId.Text;
                                txtName.Text = reader["FirstName"].ToString();
                                txtEmail.Text = reader["EMailID"].ToString();
                                txtMobile.Text = reader["Mobile"].ToString();
                                txtLoginId.ReadOnly = true;
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
                    txtLoginId.ReadOnly = true;
                    txtName.ReadOnly = true;
                    txtEmail.ReadOnly = true;
                    txtMobile.ReadOnly = true;

                }
                else
                {
                    //txtLoginId.ReadOnly = false;
                    txtName.ReadOnly = false;
                    txtEmail.ReadOnly = false;
                    txtMobile.ReadOnly = false;
                }
            }
            else
            {
                txtLoginId.ReadOnly = true;
                txtName.ReadOnly = false;
                txtEmail.ReadOnly = false;
                txtMobile.ReadOnly = false;

                txtLoginId.Clear();
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

        private void txtLoginId_TextChanged(object sender, EventArgs e)
        {
            if (txtLoginId.Text.Trim().Length > 3)
            {
                msglblLoginId.ForeColor = Color.Green;
                msglblLoginId.Visible = false;
            }
            else
            {
                msglblLoginId.Text = "Length must be greter than 3 Character";
                msglblLoginId.ForeColor = Color.Red;
                msglblLoginId.Visible = true;
            }
            ValidateAllFields();
        }
        private void ValidateAllFields()
        {
            if (msglblLoginId.ForeColor == Color.Red || msglblFullName.ForeColor == Color.Red || msgMobile.ForeColor == Color.Red || msgEmailValidation.ForeColor == Color.Red)
            {
                btnSubmit.Enabled = false;
            }
            else
                btnSubmit.Enabled = true;
        }

        private void txtMobile_TextChanged(object sender, EventArgs e)
        {
            if (txtMobile.Text.Equals(""))
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

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || txtEmail.Text.Length < 0)
            {
                msgEmailValidation.Text = "Email is required";
                msgEmailValidation.ForeColor = Color.Red;
                msgEmailValidation.Visible = true;
                //btnSubmit.Enabled = false;
            }
            else
            {
                int outValue = ValidateEmailId(txtEmail.Text);
                if (outValue == 0)
                {
                    msgEmailValidation.Text = "Not a valid email";
                    msgEmailValidation.ForeColor = Color.Red;
                    msgEmailValidation.Visible = true;
                    //btnSubmit.Enabled = false;
                }
                else if (outValue == 1)
                {
                    msgEmailValidation.Text = "Looks good";
                    msgEmailValidation.ForeColor = Color.Green;
                    msgEmailValidation.Visible = true;
                    //btnSubmit.Enabled = true;
                }
                else if (outValue == 2)
                {
                    msgEmailValidation.Text = "Must be a colorplast email";
                    msgEmailValidation.ForeColor = Color.Red;
                    msgEmailValidation.Visible = true;
                    //btnSubmit.Enabled = false;
                }

            }
            ValidateAllFields();
        }
        private int ValidateEmailId(string emailId)
        {
            /*Regular Expressions for email id*/
            Regex rEMail = new Regex(@"^[a-zA-Z][\w\.-]{0,28}[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            if (!rEMail.IsMatch(emailId))
            {
                return 0;
            }
            else
            {
                if (!emailId.Contains("colorplast"))
                {
                    return 2;
                }
                else
                {
                    return 1;
                }

            }
        }

        private void txtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back;
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        private void txtLoginId_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsSeparator(e.KeyChar);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (txtName.Text.Trim().Length > 1)
            {
                msglblFullName.ForeColor = Color.Green;
                msglblFullName.Visible = false;
            }
            else
            {
                msglblFullName.Text = "Length must be greter than 1 Character";
                msglblFullName.ForeColor = Color.Red;
                msglblFullName.Visible = true;
            }
            ValidateAllFields();
        }
    }

}
