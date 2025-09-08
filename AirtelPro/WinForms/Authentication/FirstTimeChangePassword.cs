using DG_Tool.HelperClass;
using CardPrintingApplication;
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

namespace DG_Tool.WinForms.Authentication
{
    public partial class FirstTimeChangePassword : Form
    {
        string ConStr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public FirstTimeChangePassword()
        {
            InitializeComponent();

            CaptureUserActivity captureUserActivity = new CaptureUserActivity();
            captureUserActivity.UserActivity("FirstTimeUpdatePassword");
        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewPassword.Text.Trim()) && !string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                isPasswordMatched(txtNewPassword.Text, txtConfirmPassword.Text);
            }
        }

        private void isPasswordMatched(string newPassword, string confirmPassword)
        {
            if(confirmPassword.Length > 0)
            {
                if (!string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(confirmPassword))
                {
                    if (newPassword.Equals(confirmPassword))
                    {
                        //btnSubmit.Enabled = true;
                        //btnSubmit.BackColor = Color.Green;
                        lblConfirmPassword.Text = "Matched";
                        lblConfirmPassword.ForeColor = Color.Green;
                        lblConfirmPassword.Visible = true;
                    }
                    else
                    {
                        //btnSubmit.Enabled = false;
                        //btnSubmit.BackColor = Color.Red;
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
            else
            {
                lblConfirmPassword.Visible = false;
            }
            ValidateFields();
        }

        private void txtNewPassword_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewPassword.Text.Trim()) && !string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                isPasswordMatched(txtNewPassword.Text, txtConfirmPassword.Text);
            }

            if (txtNewPassword.Text.Length > 0 && txtNewPassword.Text.Length <= 11)
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
            ValidateFields();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewPassword.Text.Trim()) && !string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    SqlDataReader reader = null;
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_UpdatePassword", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", NewLogin.primaryId);
                        cmd.Parameters.AddWithValue("@newPassword", EncryptionandDecryption.Encrypt(txtNewPassword.Text));
                        reader= cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int returnValue = Convert.ToInt32(reader["Result"]);
                            if (returnValue == 1)
                            {
                                MessageBox.Show("Password changed successfully.",
                                "Message",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                                );
                                this.Close();

                                NewLogin newLogin=new NewLogin();
                                newLogin.Show();
                            }
                            else
                            {
                                MessageBox.Show("User details not found!",
                                "Message",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                                );
                            }
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("All fields are required",
                                "Message",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Stop
                                );
            }
        }

        private void txtNewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsSeparator(e.KeyChar);
        }

        private void txtConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsSeparator(e.KeyChar);
        }

        private void ValidateFields()
        {
            if(msgPassword.ForeColor == Color.Red || lblConfirmPassword.ForeColor == Color.Red)
            {
                btnSubmit.Enabled = false;
            }
            else
            {
                btnSubmit.Enabled = true;
            }
        }
    }
}
