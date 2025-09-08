using DG_Tool.HelperClass;
using DG_Tool.WinForms.Dashboard;
using CardPrintingApplication;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Diagnostics;
using DG_Tool.WinForms.OutputFile;

namespace DG_Tool.WinForms.Authentication
{
	public partial class LoginPage : Form
	{
		private bool mouseDown;
		private Point lastLocation;

        public static string log_dir = ConfigurationManager.AppSettings["LOG_DIR"];
        public static int primaryId;
		public static int roleId;
		public static string username;
		public static string forgetUsername;
		public static int status;
		public static int isActive;
		public static int isDeleted;
		public static int isFirst;

		string ConStr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

		public LoginPage()
		{
			InitializeComponent();

			txtVersion.Text = "1.0.4.1";
			txtYears.Text = GetVersion.GetYears();


            //for testing
            if (Debugger.IsAttached)
            {
                txtUsername.Text = "admin";
                txtPassword.Text = "Admin@123456789";
				
            }

            txtUsername.Text = Environment.UserName;

            GetSignUpShowOrNot();
		}

		private void rsButton2_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void rsButton1_Click(object sender, EventArgs e)
		{
			if (txtUsername.Text.Length != 0 && txtPassword.Text.Length != 0 )
			{
				txtUsername.BorderColor = Color.LightGreen;
				txtPassword.BorderColor = Color.LightGreen;

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
											if (!Directory.Exists(log_dir + "/Logging"))
											{
												Directory.CreateDirectory(log_dir + "/Logging");
											}
											string log = $"\n********************************* Data Processing Tool Started *********************************\n" +
			 $"USER: {NewLogin.username} has logged in at [{DateTime.Now}]\n" +
			 $"USERNAME: {NewLogin.username}\n" +
			 $"SYSTEM NAME: {Environment.MachineName}\n" +
			 "************************************************************************************************\n";

											System.IO.File.AppendAllText(log_dir + "/Logging/" + $"{DateTime.Now.ToString("dd-MM-yyyy")}_log.txt", log);

											DashboardPage dashboard = new DashboardPage();
											dashboard.WindowState = FormWindowState.Maximized;
											dashboard.Show();
											txtPassword.Text = "";
											this.Hide();
										}

									}
								}
								else
								{
									txtPassword.Text = "";
									MessageBox.Show("Invalid credentials",
									"Error",
									MessageBoxButtons.OK,
									MessageBoxIcon.Error
									);

								}
							}
							else
							{
								txtPassword.Text = "";
								MessageBox.Show("Invalid credentials",
								"Error",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error
								);
							}

						}

					}
				}
			}
            else if (txtPassword.Text.Length < 14)
            {
				MessageBox.Show("Password must be at least 14 characters.");
            }
            else if (txtUsername.Text.Length != 0)
			{
				txtUsername.BorderColor = Color.LightGreen;
				txtPassword.BorderColor = Color.Red;
			}
			else if (txtPassword.Text.Length != 0)
			{
				txtUsername.BorderColor = Color.Red;
				txtPassword.BorderColor = Color.LightGreen;
			}
            
            else
			{
				txtUsername.BorderColor = Color.Red;
				txtPassword.BorderColor = Color.Red;
			}
		}

		private void txtPassword_RightIconClick(object sender, EventArgs e)
		{
			if (txtPassword.UseSystemPasswordChar == false)
			{
				txtPassword.RightIcon = Properties.Resources.hide;
				txtPassword.UseSystemPasswordChar = true;
			}
			else
			{
				txtPassword.RightIcon = Properties.Resources.view;
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
							label2.Hide();
							linkLabel1.Hide();
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

		private void panel2_MouseDown(object sender, MouseEventArgs e)
		{
			mouseDown = true;
			lastLocation = e.Location;
		}

		private void panel2_MouseMove(object sender, MouseEventArgs e)
		{
			if (mouseDown)
			{
				this.Location = new Point(
					(this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

				this.Update();
			}
		}

		private void panel2_MouseUp(object sender, MouseEventArgs e)
		{
			mouseDown = false;
		}

		private void panel1_MouseDown(object sender, MouseEventArgs e)
		{
			mouseDown = true;
			lastLocation = e.Location;
		}

		private void panel1_MouseMove(object sender, MouseEventArgs e)
		{
			if (mouseDown)
			{
				this.Location = new Point(
					(this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

				this.Update();
			}
		}

		private void panel1_MouseUp(object sender, MouseEventArgs e)
		{
			mouseDown = false;
		}
	}
}
