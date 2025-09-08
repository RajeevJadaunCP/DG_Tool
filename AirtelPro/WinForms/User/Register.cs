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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DG_Tool.WinForms.User
{
    public partial class Register : Form
    {
        //int ID = 0;
        string ConStr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public Register()
        {
            InitializeComponent();

            CaptureUserActivity captureUserActivity = new CaptureUserActivity();
            captureUserActivity.UserActivityLog("SignUp", "New User", 0);

            GetSignUpEmpShowOrNot();

            GetEmployeeIDs();
            GetRoles();

        }

        //Todo place this in common class
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

        private void GetRoles()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("RoleId", typeof(int)), new DataColumn("RoleName", typeof(string)) });
            dt.Rows.Add(2, "User");

            cbxRole.DataSource = dt;
            cbxRole.DisplayMember = "RoleName";
            cbxRole.ValueMember = "RoleId";
            cbxRole.SelectedIndex = 0;      //by defaultuser
        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text == txtConfirmPassword.Text)
            {
                lblIsPasswordMatched.Text = "Matched";
                lblIsPasswordMatched.ForeColor = Color.Green;
                lblIsPasswordMatched.Visible = true;
                btnSubmit.Enabled = true;
            }
            else
            {
                lblIsPasswordMatched.Text = "Password mis-match";
                lblIsPasswordMatched.ForeColor = Color.Red;
                lblIsPasswordMatched.Visible = true;
                btnSubmit.Enabled = false;
            }
            ValidateAllFields();
        }

        private static Regex email_validation()
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(pattern, RegexOptions.IgnoreCase);
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            int status = 9;
            string empid = string.Empty;
            //if(UserDetails.id !=0)
            //    status = 10;



            if (IsValid(txtName.Text, txtEmail.Text, txtMobile.Text, txtLoginId.Text.Trim().Replace(" ", string.Empty), txtPassword.Text, txtConfirmPassword.Text, cbxRole.Text))
            {
                Regex validate_emailaddress = email_validation();

                if (validate_emailaddress.IsMatch(txtEmail.Text) != true)
                {
                    MessageBox.Show("Invalid Email Address!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtEmail.Focus();
                    return;
                }
                else
                {
                    if (cbxEmpId.SelectedIndex == 0)
                    {
                        DataTable dt = new DataTable();
                        bool b = true;

                        while (b == true)
                        {
                            Random generator = new Random();
                            empid = "USR" + generator.Next(0, 10000).ToString("D4");

                            using (SqlConnection con = new SqlConnection(ConStr))
                            {
                                con.Open();
                                using (SqlDataAdapter sda = new SqlDataAdapter($"SELECT EmployeeId FROM UserDetails WHERE EmployeeId = '{empid}'", con))
                                {
                                    sda.Fill(dt);
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        b = true;
                                    }
                                    else
                                        b = false;
                                }
                            }
                        }
                    }
                    else
                        empid = cbxEmpId.Text;

                    using (SqlConnection con = new SqlConnection(ConStr))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("usp_CreateUpdateUserDetails", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@primaryId", UserDetails.id);

                            cmd.Parameters.AddWithValue("@employeeId", empid);
                            cmd.Parameters.AddWithValue("@name", txtName.Text);
                            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                            cmd.Parameters.AddWithValue("@contact", txtMobile.Text);
                            cmd.Parameters.AddWithValue("@userName", txtLoginId.Text);
                            cmd.Parameters.AddWithValue("@password", EncryptionandDecryption.Encrypt(txtPassword.Text));
                            cmd.Parameters.AddWithValue("@roleId", cbxRole.SelectedValue);
                            cmd.Parameters.AddWithValue("@createdBy", NewLogin.primaryId);
                            cmd.Parameters.AddWithValue("@status", status);

                            cmd.ExecuteReader();

                            MessageBox.Show("User created successfully",
                                    "Message",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information
                                    );

                            CaptureUserActivity captureUserActivity = new CaptureUserActivity();
                            captureUserActivity.UserActivityLog("SignUp", txtName.Text, 0);

                            this.Hide();
                            NewLogin newLogin = new NewLogin();
                            newLogin.Show();
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

        private bool IsValid(params string[] fields)
        {
            bool isValid = true;
            foreach (string i in fields)
            {
                if (!string.IsNullOrEmpty(i))
                {
                    continue;
                }
                else
                    isValid = false;

            }
            return isValid;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtEmail.Text) || txtEmail.Text.Length < 0)
            {
                msgEmailValidation.Text = "Email is required";
                msgEmailValidation.ForeColor = Color.Red;
                msgEmailValidation.Visible = true;
                btnSubmit.Enabled = false;
            }
            else
            {
                int outValue = ValidateEmailId(txtEmail.Text);
                if (outValue == 0)
                {
                    msgEmailValidation.Text = "Not a valid email";
                    msgEmailValidation.ForeColor = Color.Red;
                    msgEmailValidation.Visible = true;
                    btnSubmit.Enabled = false;
                }
                else if (outValue == 1)
                {
                    msgEmailValidation.Text = "Looks good";
                    msgEmailValidation.ForeColor = Color.Green;
                    msgEmailValidation.Visible = true;
                    btnSubmit.Enabled = true;
                }
                else if (outValue == 2)
                {
                    msgEmailValidation.Text = "Must be a colorplast email";
                    msgEmailValidation.ForeColor = Color.Red;
                    msgEmailValidation.Visible = true;
                    btnSubmit.Enabled = false;
                }
                else if (outValue == 3)
                {
                    msgEmailValidation.Text = "Email already exist";
                    msgEmailValidation.ForeColor = Color.Red;
                    msgEmailValidation.Visible = true;
                    btnSubmit.Enabled = false;
                }
            }
            ValidateAllFields();
        }

        // 0 for not a valid email
        // 2 Must be a colorplast email
        // 1 Looks good
        // 3 Email already exist

        private int ValidateEmailId(string emailId)
        {
            /*Regular Expressions for email id*/
            Regex rEMail = new Regex(@"^[a-zA-Z][\w\.-]{2,28}[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
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
                    SqlDataReader reader = null;
                    using (SqlConnection con = new SqlConnection(ConStr))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("Select * From UserDetails Where Email = @email", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@email", emailId);
                            reader = cmd.ExecuteReader();

                            if (reader.HasRows == true)
                            {
                                return 3;
                            }
                            else
                            {
                                return 1;
                            }

                        }
                    }
                }

            }
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
            //else if (txtMobile.Text.Length > 10)
            //{
            //    msgMobile.Text = "Must be greater than 10-digits & less than 15-digits";
            //    msgMobile.ForeColor = Color.Red;
            //    msgMobile.Visible = true;
            //    //btnSubmit.Enabled = false;
            //}
            else
            {
                SqlDataReader reader = null;
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Select * From UserDetails Where Contact = @email", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@email", txtMobile.Text);
                        reader = cmd.ExecuteReader();

                        if (reader.HasRows == true)
                        {
                            msgMobile.Text = "Mobile number already exist";
                            msgMobile.ForeColor = Color.Red;
                            msgMobile.Visible = true;
                            //btnSubmit.Enabled = false;
                        }
                        else
                        {
                            msgMobile.Text = "Looks good";
                            msgMobile.ForeColor = Color.Green;
                            msgMobile.Visible = true;
                            //btnSubmit.Enabled = true;
                        }

                    }
                }
            }
            ValidateAllFields();
        }
        private void cbxEmpId_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbxEmpId.SelectedIndex != 0 && !string.IsNullOrEmpty(cbxEmpId.Text))
            {
                txtPassword.Clear();
                txtConfirmPassword.Clear();
                if (!IsEmpIdExist())
                {
                    lblEmpCode.ForeColor = Color.Green;
                    lblIsPasswordMatched.Visible = false;

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
                                    txtName.ReadOnly = true;
                                    //txtEmail.ReadOnly = true;
                                    //txtMobile.ReadOnly = true;
                                    
                                    txtEmail.ReadOnly = false;
                                    txtMobile.ReadOnly = false;
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
                }
                else
                {
                    lblEmpCode.Text = "Employee code already exist";
                    lblEmpCode.ForeColor = Color.Red;
                    lblEmpCode.Visible = true;

                }


            }
            else
            {
                txtLoginId.ReadOnly = false;
                txtName.ReadOnly = false;
                txtEmail.ReadOnly = false;
                txtMobile.ReadOnly = false;

                txtLoginId.Clear();
                txtName.Clear();
                txtEmail.Clear();
                txtMobile.Clear();
            }
            ValidateAllFields();
        }
        private bool IsEmpIdExist()
        {
            SqlDataReader reader = null;

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select *from UserDetails WHERE EmployeeId= @empid", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@empid", cbxEmpId.Text);

                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text.Length <= 11)
            {
                msgPassword.Text = "Password must be greater than 11-digits";
                msgPassword.ForeColor = Color.Red;
                msgPassword.Visible = true;
                //btnSubmit.Enabled = false;
            }
            else
            {
                msgPassword.ForeColor = Color.Green;
                msgPassword.Visible = false;
                //btnSubmit.Enabled = true;
            }
            ValidateAllFields();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (CommonClass.PreClosingConfirmation() == DialogResult.Yes)
            {
                this.Close();
            }
            NewLogin newLogin = new NewLogin();
            newLogin.Show();
            // Application.Exit();
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel2.ClientRectangle,
                        Color.Red, 1, ButtonBorderStyle.Dotted, // left
                        Color.Red, 1, ButtonBorderStyle.Dotted, // top
                        Color.Red, 1, ButtonBorderStyle.Dotted, // right
                        Color.Red, 1, ButtonBorderStyle.Dotted);// bottom
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
        private void txtLoginId_KeyPress(object sender, KeyPressEventArgs e)
        {
            //var regex = new Regex(@"[^a-zA-Z0-9\s]");
            //if (regex.IsMatch(e.KeyChar.ToString()))
            //{
            //    e.Handled = true;
            //}

            //e.Handled = !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar); // backspace not allowed

            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }
        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }
        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsSeparator(e.KeyChar);
        }
        private void txtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back;
        }
        private void txtLoginId_TextChanged(object sender, EventArgs e)
        {
            if (txtLoginId.Text.Trim().Length > 3)
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter($"SELECT *FROM UserDetails WHERE Username = '{txtLoginId.Text}'", con))
                    {
                        sda.Fill(dt);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            msglblLoginId.Text = "Login Id already exist";
                            msglblLoginId.ForeColor = Color.Red;
                            msglblLoginId.Visible = true;
                            //btnSubmit.Enabled = false;
                        }
                        else
                        {
                            msglblLoginId.ForeColor = Color.Green;
                            msglblLoginId.Visible = false;
                            //btnSubmit.Enabled = true;
                        }
                    }
                }
            }
            else
            {
                msglblLoginId.Text = "Length must be greter than 3 Character";
                msglblLoginId.ForeColor = Color.Red;
                msglblLoginId.Visible = true;
            }


            ValidateAllFields();
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (txtName.Text.Trim().Length > 1)
            {
                msglblFullname.ForeColor = Color.Green;
                msglblFullname.Visible = false;
            }
            else
            {
                msglblFullname.Text = "Length must be greter than 1 Character";
                msglblFullname.ForeColor = Color.Red;
                msglblFullname.Visible = true;
            }
            ValidateAllFields();
        }
        private void ValidateAllFields()
        {
            if (lblEmpCode.ForeColor == Color.Red || msglblLoginId.ForeColor == Color.Red || msgPassword.ForeColor == Color.Red || msgMobile.ForeColor == Color.Red || msgEmailValidation.ForeColor == Color.Red || lblIsPasswordMatched.ForeColor == Color.Red || msglblFullname.ForeColor == Color.Red)
            {
                btnSubmit.Enabled = false;
            }
            else
                btnSubmit.Enabled = true;
        }
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsSeparator(e.KeyChar);
        }
        private void txtConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsSeparator(e.KeyChar);
        }
    }
}
