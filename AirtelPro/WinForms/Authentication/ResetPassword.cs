using DG_Tool.HelperClass;
using CardPrintingApplication;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace DG_Tool.WinForms.Authentication
{
    public partial class ResetPassword : Form
    {
        string ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public ResetPassword()
        {
            InitializeComponent();

            CaptureUserActivity captureUserActivity = new CaptureUserActivity();
            captureUserActivity.UserActivityLog("ResetPassword", "New User", 0);
        }

        //int primaryId = 0;
        //public ResetPassword(int Id)
        //{
        //    InitializeComponent();
        //    primaryId = Id;
        //}

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOtp.Text.Trim()) && !string.IsNullOrEmpty(txtNewPassword.Text.Trim()) && !string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {

                if(!IsOldPassword())
                {
                    using (SqlConnection con = new SqlConnection(ConStr))
                    {
                        SqlDataReader reader = null;
                        con.Open();
                        //Todo convert to stored procedure
                        using (SqlCommand cmd = new SqlCommand(@"SELECT
	                                                            *
                                                            FROM
	                                                            OneTimmePassword
                                                            WHERE
	                                                            EmpId = @id AND
	                                                            OTP = @otp AND
	                                                            DATEDIFF(MINUTE, CreatedOn , GETDATE()) < 10 AND
	                                                            IsActive = 1 ", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@id", ForgotPassword.PrimaryId);
                            cmd.Parameters.AddWithValue("@otp", txtOtp.Text);

                            reader = cmd.ExecuteReader();

                            if (reader.HasRows)
                            {
                                changePassword(ForgotPassword.PrimaryId, txtOtp.Text, txtNewPassword.Text.Trim());
                            }
                            else
                            {
                                MessageBox.Show("Invalid OTP",
                                                "Message",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Stop
                                                );
                            }

                        }
                    }
                }
                else
                {
                    MessageBox.Show("New password can not be your old password",
                                            "Message",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning
                                            );
                }
                
            }
            else
            {
                MessageBox.Show("All fields are required",
                                            "Message",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning
                                            );
            }
        }

        private bool IsOldPassword()
        {
            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT *FROM UserDetails WHERE ID = @id And password = newPassword", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@id", ForgotPassword.PrimaryId);
                        cmd.Parameters.AddWithValue("@newPassword", EncryptionandDecryption.Encrypt(txtNewPassword.Text.Trim()));

                        reader = cmd.ExecuteReader();

                        if(reader.HasRows)
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
            catch (Exception ex)
            {
                MessageBox.Show("Error"+ ex.Message,
                                "Message",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                                );
                return false;
            }
        }
        private void isPasswordMatched(string newPassword, string confirmPassword)
        {
            if (!string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(confirmPassword))
            {
                if (newPassword.Equals(confirmPassword))
                {
                    btnSubmit.Enabled = true;
                    lblConfirmPassword.Text = "Matched";
                    lblConfirmPassword.ForeColor = Color.Green;
                    lblConfirmPassword.Visible = true;
                }
                else
                {
                    btnSubmit.Enabled = false;
                    lblConfirmPassword.Text = "Password mis-match";
                    lblConfirmPassword.ForeColor = Color.Red;
                    lblConfirmPassword.Visible = true;
                }
            }
            else
            {
                MessageBox.Show("All fields are required",
                                "Message",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                                );
            }


        }
        private void txtNewPassword_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                if (txtNewPassword.Text.Length <= 11)
                {

                    msgPassword.Text = "Password must be greater than 11-digits";
                    msgPassword.ForeColor = Color.Red;
                    msgPassword.Visible = true;
                    btnSubmit.Enabled = false;

                }
                else
                {
                    msgPassword.ForeColor = Color.Green;
                    msgPassword.Visible = false;
                    btnSubmit.Enabled = true;
                }
            }
            else
            {
                lblConfirmPassword.Visible = false;
                btnSubmit.Enabled = false;
            }

            

            
        }
        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewPassword.Text.Trim()) && !string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                isPasswordMatched(txtNewPassword.Text, txtConfirmPassword.Text);
            }
            else
            {
                lblConfirmPassword.Visible = false;
                btnSubmit.Enabled = true;
            }
        }

        private void changePassword(int id, string otp, string newPassword)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_ResetPassword", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@otp", otp);
                        cmd.Parameters.AddWithValue("@newPassword", EncryptionandDecryption.Encrypt(newPassword));

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Password changes successfully. Please login",
                                            "Message",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information
                                            );

                        CaptureUserActivity captureUserActivity = new CaptureUserActivity();
                        captureUserActivity.UserActivityLog("RestPassword", GetSingleUser(id), id);

                        NewLogin login = new NewLogin();
                        login.Show();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message,
                                            "Message",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Stop
                                            );
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private string GetSingleUser(int id)
        {
            string uname = "";

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                SqlDataReader reader = null;
                con.Open();
                //Todo change to procedure
                using (SqlCommand cmd = new SqlCommand("usp_GetSingleUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);

                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            uname = reader["Name"].ToString();  
                        }
                    }

                }
            }
            return uname;
        }

        private void txtNewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsSeparator(e.KeyChar);
        }

        private void txtConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsSeparator(e.KeyChar);
        }
    }
}
