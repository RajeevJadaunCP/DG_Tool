using DG_Tool.HelperClass;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DG_Tool.WinForms.Authentication
{
    public partial class ForgotPassword : Form
    {
        string ConStr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public static int PrimaryId = 0;
        public ForgotPassword()
        {
            InitializeComponent();

            txtUsername.Text = NewLogin.forgetUsername;
            //txtUsername.ReadOnly= true;
            CaptureUserActivity captureUserActivity = new CaptureUserActivity();
            captureUserActivity.UserActivityLog("ForgetPassword","New User",0);
        }
        public bool saveOTP(int id, string otp)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    //Todo convert to stored procedure
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO OneTimmePassword(EmpId,OTP,IsActive) VALUES (@id,@otp,1)", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@otp", otp);

                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong " + ex.Message,
                "Message",
                MessageBoxButtons.OK,
                MessageBoxIcon.Stop
                );
                return false;
            }

        }

        private void btnSendOtp_Click(object sender, EventArgs e)
        {
            bool isSent = false;
            if (!string.IsNullOrEmpty(txtUsername.Text.Trim()))
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    SqlDataReader reader = null;
                    con.Open();
                    //Todo Change to Stored Procedure
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM UserDetails WHERE Email = @username OR Contact = @username OR Username = @username", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text);

                        reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                PrimaryId = Convert.ToInt32(reader["Id"]);
                                string name = reader["Name"].ToString();
                                string email = reader["Email"].ToString();
                                string mobile = reader["Contact"].ToString();
                                if (email != null)
                                {
                                    Random generator = new Random();
                                    string otp = generator.Next(0, 1000000).ToString("D6");

                                    bool IsOtpSaved = saveOTP(PrimaryId, otp);

                                    if (IsOtpSaved)
                                    {
                                        EmailService otpsend = new EmailService();
                                        isSent = otpsend.SendMailOTP(email.Trim(), otp,name);
                                                                               
                                        if (isSent)
                                        {
                                            MessageBox.Show("OTP sent to your email",
                                            "Message",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information
                                            );

                                            CaptureUserActivity captureUserActivity = new CaptureUserActivity();
                                            captureUserActivity.UserActivityLog("ForgetPassword", name, PrimaryId);

                                            ResetPassword resetPassword = new ResetPassword();
                                            resetPassword.Show();
                                            this.Close();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Something went wrong while sending OTP",
                                            "Message",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information
                                            );
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Something went wrong while saving OTP",
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
                            MessageBox.Show("Invalid Username",
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
                MessageBox.Show("Username is required",
                "Message",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
                );
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
            NewLogin newLogin=new NewLogin();
            newLogin.Show();
        }

        private void pbInfo_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.pbInfo,"OTP will be sent to your registed email");
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsSeparator(e.KeyChar);
        }
    }
}
