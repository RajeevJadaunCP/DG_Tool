using DG_Tool.HelperClass;
using DG_Tool.WinForms.Authentication;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DG_Tool
{
    public partial class UserProfile : Form
    {
        string connsctionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
       
        public UserProfile()
        {
            InitializeComponent();

            GetSingleUser(LoginPage.primaryId);

            CaptureUserActivity captureUserActivity = new CaptureUserActivity();
            captureUserActivity.UserActivity("ProfileView");
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
                        }
                    }

                }
            }
        }

        private void btnActivateDeactivate_Click(object sender, EventArgs e)
        {
            using(SqlConnection con = new SqlConnection(connsctionString))
            {
                con.Open();
                using(SqlCommand cmd = new SqlCommand("UPDATE UserDetails SET IsActive = CASE IsActive WHEN 1 THEN 0 ELSE 1 END WHERE Id = @id", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", NewLogin.primaryId);
                    cmd.ExecuteNonQuery();
                }
            }
            GetSingleUser(NewLogin.primaryId);
        }

		private void rsButton2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ChangePassword changePassword = new ChangePassword();
			changePassword.ShowDialog();
		}
	}
}
