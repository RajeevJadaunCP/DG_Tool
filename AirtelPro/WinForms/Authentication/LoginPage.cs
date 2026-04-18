using CardPrintingApplication;
using CardPrintingApplication;
using DG_Tool.HelperClass;
using DG_Tool.Models;
using DG_Tool.WinForms.Dashboard;
using DG_Tool.WinForms.OutputFile;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
using System.Windows.Forms;

/*
 * 
 * 
1.0.0.4.0  : 30032026 :1.Reliance DG incorporation
1.0.0.3.13  : 28022026 :1.remove this from messagebox
1.0.0.3.12  : 25022026 :1."Duplicity check within lot and also remove msisdn from aftel
1.0.0.3.11  : 25022026 :1."Bugs Fixing for DG TOOL iccid and  imsi validation
1.0.0.3.10  : 05022026 :1."Bugs Fixing for DG TOOL
1. mca vs label name mistamch for smaple files or small qty data
2. Batch list name nomenclature wrong
3. Total qty in dg tool log is not capturing in case of multi file processing 
4. Iccid validation on vodafone
5. csv file importation instead of excel 
6. BSNL graphical update"
1.0.0.3.9  : 13012026 :imsi vs iccid validation from last 6 digit for vodafone only for incremental records(generic profile)
1.0.0.3.8  : 13012026 : "Enhancement as per data team
1.  Licensing update
2. Circle name validation."
1.0.0.3.7  : 07012026 Automatic  input file selection
1.0.0.3.6  : 26122025 :1.input Input file validation as per name for multifile processing (sequnece match on matching  input files)
1.0.0.3.5  : 05122025 :1. need to chnage the label creation funciton as imsi and iccid index chnges for aftel files
					   2. also chnage the formula for calculatiing mca file qty beacuse for header
1.0.0.3.4  : 27112025 :1. chnage the formula for calculatiing mca file qty because for non divisible file qty with mca bacthsize
*/

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
		public static DateTime? isLastPasswordChangeDate;

        public static string ConStr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

		public LoginPage()
		{
			InitializeComponent();

			txtVersion.Text = "1.0.0.4.2";
			txtYears.Text = GetVersion.GetYears();


            //for testing
            if ((Debugger.IsAttached)|| ConStr.Contains("192.168.5.22"))
            {
                txtUsername.Text = Environment.UserName;
                txtPassword.Text = "Admin@123456789";
				
            }
			else {
                txtUsername.Text = Environment.UserName;
                txtUsername.Enabled = false;
            }

			//label7.Text = "DATA GEN TOOL WIHTOUT IMSI DUPLICIITY";
            GetSignUpShowOrNot();
		}

		private void rsButton2_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void rsButton1_Click(object sender, EventArgs e)
		{
			//string encryptPassword = EncryptionandDecryption.Encrypt(txtPassword.Text);

			//         if (string.IsNullOrEmpty(txtUsername.Text.Trim()))
			//{
			//             MessageBox.Show("Please Enter Username.", "Warning");
			//         }
			//else
			//{
			//             if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
			//             {
			//                 MessageBox.Show("Please Enter Password.", "Warning");
			//             }
			//	else
			//	{
			//		try
			//		{
			//                     LoginMaster user = GetUser(txtUsername.Text);
			//                     if ((user.Username!= null))
			//                     {
			//                         if(user.Status == 9)
			//				{
			//                             MessageBox.Show("Profile is in draft mode.\nPlease contact to your administrator!",
			//					"Message",
			//					MessageBoxButtons.OK,
			//					MessageBoxIcon.Information
			//					);
			//				}
			//                         else if (user.IsDeleted == 1)
			//				{
			//					MessageBox.Show("Profile is disabled.\nPlease contact to your administrator",
			//					"Message",
			//					MessageBoxButtons.OK,
			//					MessageBoxIcon.Information
			//					);
			//				}
			//				else if(user.IsActive == 0)
			//				{
			//                             MessageBox.Show("Profile is in-active.\nPlease contact to your administrator",
			//					"Message",
			//					MessageBoxButtons.OK,
			//					MessageBoxIcon.Information
			//					);
			//				}
			//                     }
			//                     else
			//			{
			//                         MessageBox.Show("Invalid Username.", "Warning");
			//                     }
			//                 }
			//		catch(Exception ex)
			//		{

			//		}
			//	}
			//         }
			if (txtUsername.Text.Length != 0 && txtPassword.Text.Length != 0 && txtPassword.Text.Length >= 14) 
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



        public static LoginMaster GetUser(string username)
        {
            LoginMaster master = new LoginMaster();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                string proname = $"Select * From [UserDetails] Where [Username]='{username}'";
                con.Open();
                using (SqlCommand cmd = new SqlCommand(proname, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            master.Id = Convert.ToInt32(reader["ID"]);
                            master.Name = reader["Name"].ToString().Trim();
                            master.Username = reader["Username"].ToString().Trim();
                            master.Password = reader["Password"].ToString().Trim();
                            master.RoleId = Convert.ToInt32(reader["RoleId"]);
                            master.IsActive = Convert.ToInt32(reader["IsActive"]);
                            master.FirstLogin = Convert.ToInt32(reader["FirstLogin"]);
							master.IsDeleted = Convert.ToInt32(reader["IsDeleted"]);
                            master.Status = Convert.ToInt32(reader["Status"]);
                            master.LastPasswordChangeDate = reader["LastPasswordChangeDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["LastPasswordChangeDate"]);
                        }
                    }
                }
            }
            return master;
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

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
