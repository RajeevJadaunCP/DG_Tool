using DG_Tool.HelperClass;
using DG_Tool.WinForms.User;
using CardPrintingApplication;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DG_Tool.WinForms.Dashboard;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace DG_Tool.WinForms.Authentication
{
    public partial class NewLogin : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        public static int primaryId;
        public static int roleId;
        public static string username;
        public static string forgetUsername;
        public static int status;
        public static int isActive;
        public static int isDeleted;
        public static int isFirst;

        string ConStr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public NewLogin()
        {
            InitializeComponent();

            GetSignUpShowOrNot();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                    if (!string.IsNullOrEmpty(txtUsername.Text.Trim()) && !string.IsNullOrEmpty(txtPassword.Text.Trim()))
                    {
                        //if (txtPassword.Text.Length > 11)
                        //{
                            SqlDataReader reader = null;
                            using (SqlConnection con = new SqlConnection(ConStr))
                            {
                                con.Open();
                                using (SqlCommand cmd = new SqlCommand("usp_UserLogin", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                                    cmd.Parameters.AddWithValue("@password", EncryptionandDecryption.Encrypt(txtPassword.Text));

                                    reader = cmd.ExecuteReader();

                                    while (reader.Read())
                                    {
                                        if (!string.IsNullOrEmpty(reader["Id"].ToString()) && !string.IsNullOrEmpty(reader["RoleId"].ToString()))
                                        {
                                            primaryId = Convert.ToInt32(reader["Id"]);
                                            roleId = Convert.ToInt32(reader["RoleId"]);
                                            username = reader["loggedName"].ToString();
                                            status = Convert.ToInt32(reader["Status"]);
                                            isActive = Convert.ToInt32(reader["IsActive"]);
                                            isDeleted = Convert.ToInt32(reader["IsDeleted"]);
                                            isFirst = Convert.ToInt32(reader["FirstTimeLogin"]);

                                            if (primaryId > 0)
                                            {
                                                if (status == 9)
                                                {
                                                    MessageBox.Show("Profile is in draft mode.\nPlease contact to your administrator!",
                                                    "Message",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information
                                                    );
                                                }
                                                else if (isActive == 0)
                                                {
                                                    MessageBox.Show("Profile is in-active.\nPlease contact to your administrator",
                                                    "Message",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information
                                                    );
                                                }
                                                else if (isDeleted == 1)
                                                {
                                                    MessageBox.Show("Profile is disabled.\nPlease contact to your administrator",
                                                    "Message",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information
                                                    );
                                                }
                                                else if (isFirst == 0)
                                                {
                                                    this.Hide();
                                                    FirstTimeChangePassword firstTimeChangePassword = new FirstTimeChangePassword();
                                                    firstTimeChangePassword.Show();
                                                }
                                                else
                                                {
                                                    CaptureUserActivity captureUserActivity = new CaptureUserActivity();
                                                    int a = captureUserActivity.UserActivity("Login");

                                                    if (a != 0)
                                                    {
                                                        //Dashboard dashboard = new Dashboard();
                                                        //dashboard.Show();
                                                        //txtPassword.Clear();
                                                        //this.Hide();
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                txtPassword.Clear();

                                                MessageBox.Show("Invalid credentials",
                                                "Error",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Error
                                                );

                                            }
                                        }
                                        else
                                        {
                                            txtPassword.Clear();

                                            MessageBox.Show("Invalid credentials",
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error
                                            );
                                        }

                                    }

                                }
                            }
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Password length should be atleast 12 digits",
                        //                        "Message",
                        //                        MessageBoxButtons.OK,
                        //                        MessageBoxIcon.Warning
                        //                        );
                        //}
                    }
                    else
                    {
                        MessageBox.Show("Username and Password is required",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error
                                        );
                    }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void lblForgetPassword_Clicked(object sender, EventArgs e)
        {
            //if(!string.IsNullOrEmpty(txtUsername.Text))
            //{
                forgetUsername=txtUsername.Text;
                ForgotPassword forgotPassword = new ForgotPassword();
                forgotPassword.Show();
                this.Hide();
            //}
            //else
            //{
            //    MessageBox.Show("Username is required",
            //                   "Error",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Error
            //                   );
            //}
            
        }

        private void lblSignUp_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel1.ClientRectangle,
                        Color.Red, 1, ButtonBorderStyle.Dotted, // left
                        Color.Red, 1, ButtonBorderStyle.Dotted, // top
                        Color.Red, 1, ButtonBorderStyle.Dotted, // right
                        Color.Red, 1, ButtonBorderStyle.Dotted);// bottom
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
             Application.Exit();

           // WindowState = FormWindowState.Minimized;
        }

        private void NewLogin_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void NewLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void NewLogin_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            txtUsername.BackColor= Color.LightYellow;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            txtPassword.BackColor= Color.LightYellow;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if(txtPassword.UseSystemPasswordChar==false)
            {
                pictureBox5.BringToFront();
                txtPassword.UseSystemPasswordChar= true;
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (txtPassword.UseSystemPasswordChar == true)
            {
                pictureBox4.BringToFront();
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        private void GetSignUpShowOrNot()
        {
            using (SqlConnection con3 = new SqlConnection(ConStr))
            {
                SqlCommand com3 = new SqlCommand("select ParameterCharVal from DataToolParameters where ParameterID=1", con3);
                con3.Open();
                SqlDataReader reader3 = com3.ExecuteReader();

                if (reader3.HasRows)
                {
                    if (reader3.Read())
                    {
                        if (reader3.GetValue(0).ToString().Trim().Equals("N"))
                        {
                            label5.Hide();
                            lblSignUp.Hide();
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

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsSeparator(e.KeyChar);
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogin.PerformClick();
        }
    }
}
