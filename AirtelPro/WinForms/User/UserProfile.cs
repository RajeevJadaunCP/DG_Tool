using DG_Tool.HelperClass;
using DG_Tool.WinForms.Authentication;
using CardPrintingApplication;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DG_Tool.WinForms.User
{
    public partial class UserProfile : Form
    {
        string connsctionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public UserProfile()
        {
            InitializeComponent();

            CaptureUserActivity captureUserActivity = new CaptureUserActivity();
            captureUserActivity.UserActivity("User Profile");

            GetSingleUser(UserDetails.id);
        }
        private void GetSingleUser(int id)
        {
            using (SqlConnection con = new SqlConnection(connsctionString))
            {
                SqlDataReader reader = null;
                con.Open();
                //Todo change to procedure
                using(SqlCommand cmd = new SqlCommand("usp_GetSingleUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);

                    reader= cmd.ExecuteReader();

                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            lblEmployeeId.Text = reader["EmployeeCode"].ToString();
                            lblLoginId.Text = reader["Username"].ToString();
                            lblUsename.Text = reader["Name"].ToString();
                            lblRole.Text = reader["RoleName"].ToString();
                            lblEmail.Text = reader["Email"].ToString();
                            lblMobile.Text = reader["Contact"].ToString();
                            lblStatus.Text = (reader["IsActive"].ToString()) == "Yes" ? "Active" : "Inactive";
                            if (lblStatus.Text == "Active")
                            {
                                lblStatus.ForeColor = Color.White;
                                lblStatus.BackColor = Color.Green;
                            }
                            else
                            {
                                lblStatus.ForeColor = Color.White;
                                lblStatus.BackColor = Color.Red;
                            }
                            btnActivateDeactivate.Text = (reader["IsActive"].ToString())=="Yes"?"Deactivate":"Activate";

                            if (btnActivateDeactivate.Text == "Activate")
                            {
                                btnActivateDeactivate.ForeColor= Color.White;
                                btnActivateDeactivate.BackColor= Color.Green;
                            }
                            else
                            {
                                btnActivateDeactivate.ForeColor = Color.White;
                                btnActivateDeactivate.BackColor = Color.Red;
                            }

                            //UserDetails userDetails = new UserDetails();
                            //userDetails.ShowDialog();
                        }
                    }

                }
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            bool isSent = false;
            string defaultPass = "User@123";

            DialogResult res = MessageBox.Show("Are you sure " + (sender as Button).Text + "!", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (res == DialogResult.Yes)
            {
                EmailService otpsend = new EmailService();
                isSent = otpsend.SendMailOTP(lblEmail.Text.Trim(), defaultPass, lblUsename.Text);

                if (isSent)
                {
                    MessageBox.Show("Password sent to your email",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );

                    using (SqlConnection con = new SqlConnection(connsctionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("UPDATE UserDetails SET Password = @password WHERE Id = @id", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@id", UserDetails.id);
                            cmd.Parameters.AddWithValue("@password", EncryptionandDecryption.Encrypt(defaultPass));
                            cmd.ExecuteNonQuery();
                        }
                    }
                    GetSingleUser(UserDetails.id);
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
            if (res == DialogResult.No)
            {

            }

        }

        private void btnActivateDeactivate_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure "+ (sender as Button).Text + " account!", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
           
            if (res == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connsctionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE UserDetails SET IsActive = CASE IsActive WHEN 1 THEN 0 ELSE 1 END WHERE Id = @id", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@id", UserDetails.id);
                        cmd.ExecuteNonQuery();
                    }
                }
                GetSingleUser(UserDetails.id);

                this.Close();

            }
            if (res == DialogResult.No)
            {
                
            }           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
