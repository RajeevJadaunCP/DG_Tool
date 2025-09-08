using DG_Tool.HelperClass;
using DG_Tool.WinForms.Authentication;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace DG_Tool.WinForms.Dashboard
{
	public partial class DashboardPage : Form
	{
		private bool mouseDown;
		private Point lastLocation;
		private FormWindowState previousState = FormWindowState.Normal;
		private bool isMaximized = false;

		SqlConnection conn;
		MenuStrip MnuStrip;
		ToolStripMenuItem MnuStripItem;

		static DashboardPage _obj;

		string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

		public static DashboardPage Instance
		{
			get
			{
				if (_obj == null)
				{
					_obj = new DashboardPage();
				}
				return _obj;
			}
		}

		public Panel PnlContainer
		{
			get
			{
				return panel3;

			}
			set
			{
				panel3 = value;
			}
		}

		public DashboardPage()
		{
			InitializeComponent();

			_obj = this;

			CaptureUserActivity captureUserActivity = new CaptureUserActivity();
			captureUserActivity.UserActivity("Dashboard");

			CreateMenu();

			LoadDashboardSummary();

			label1.Text = LoginPage.username;
			label2.Text = GetRoleName(LoginPage.roleId);
		}

		private void rsButton2_Click(object sender, EventArgs e)
		{
			CaptureUserActivity captureUserActivity = new CaptureUserActivity();
			captureUserActivity.UserActivity("Logout");

			Application.Exit();
		}

		private void rsButton1_Click(object sender, EventArgs e)
		{
			if (isMaximized)
			{
				previousState = FormWindowState.Normal;
				this.WindowState = FormWindowState.Normal;
				isMaximized = false;
			}
			else
			{
				this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
				previousState = FormWindowState.Maximized;
				this.WindowState = FormWindowState.Maximized;
				isMaximized = true;
			}
		}

		private void rsButton3_Click(object sender, EventArgs e)
		{
			this.WindowState=FormWindowState.Minimized;
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

		public void CreateMenu()
		{
			MnuStrip = new MenuStrip();
			MnuStrip.BackColor = Color.LightSalmon;
			MnuStrip.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			MnuStrip.ImageScalingSize=new Size(20,20);
	
			panel2.Controls.Add(MnuStrip);

			conn = new SqlConnection(connectionString);

			String Sequel = "select MenuId, MenuName,ImgName from MenuMaster where role='" + LoginPage.roleId + "' order by MenuId";

			SqlDataAdapter da = new SqlDataAdapter(Sequel, conn);

			DataTable dt = new DataTable();

			conn.Open();

			da.Fill(dt);

			foreach (DataRow dr in dt.Rows)
			{
				MnuStripItem = new ToolStripMenuItem(dr["MenuName"].ToString(), null, new EventHandler(MenuClick));
				MnuStripItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));	
				MnuStripItem.Image = imageList1.Images[dr["ImgName"].ToString()];
				SubMenu(MnuStripItem, dr["MenuId"].ToString());

				MnuStripItem.Padding = new System.Windows.Forms.Padding(8);
				MnuStrip.Padding = new System.Windows.Forms.Padding(5);
				MnuStripItem.Margin = new System.Windows.Forms.Padding(2);

				MnuStrip.Items.Add(MnuStripItem);

			}

			this.MainMenuStrip = MnuStrip;
			conn.Close();
		}

		public void SubMenu(ToolStripMenuItem mnu, string submenu)
		{
			String Seqchild = "SELECT MenuName,ImgName FROM SubMenuMaster WHERE MenuId='" + submenu + "' and Role='" + LoginPage.roleId + "'order by MenuId";

			SqlDataAdapter dachildmnu = new SqlDataAdapter(Seqchild, conn);

			DataTable dtchild = new DataTable();

			dachildmnu.Fill(dtchild);

			foreach (DataRow dr in dtchild.Rows)
			{
				ToolStripMenuItem SSMenu = new ToolStripMenuItem(dr["MenuName"].ToString(), null, new EventHandler(SubMenuClick));
				SSMenu.Image = imageList1.Images[dr["ImgName"].ToString()];
				SSMenu.ImageTransparentColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
				mnu.DropDownItems.Add(SSMenu);
			}
		}

		private void MenuClick(object sender, EventArgs e)
		{
			String Seqtx = "SELECT FormName FROM MenuMaster WHERE MenuName='" + sender.ToString() + "'";

			SqlDataAdapter datransaction = new SqlDataAdapter(Seqtx, conn);

			DataTable dtransaction = new DataTable();

			datransaction.Fill(dtransaction);

			Assembly frmAssembly = Assembly.LoadFile(Application.ExecutablePath);

			foreach (Type type in frmAssembly.GetTypes())
			{
				if (type.BaseType == typeof(Form))
				{
					if (type.Name == dtransaction.Rows[0][0].ToString())
					{
						Form frmShow = (Form)frmAssembly.CreateInstance(type.ToString());

						frmShow.Dock = DockStyle.Fill;
						frmShow.TopLevel = false;
						frmShow.Show();
						PnlContainer.Controls.Clear();
						PnlContainer.Controls.Add(frmShow);
					}
				}
			}
		}

		private void SubMenuClick(object sender, EventArgs e)
		{
			String Seqtx = "SELECT FormName FROM SubMenuMaster WHERE MenuName='" + sender.ToString() + "'";

			SqlDataAdapter datransaction = new SqlDataAdapter(Seqtx, conn);

			DataTable dtransaction = new DataTable();

			datransaction.Fill(dtransaction);

			Assembly frmAssembly = Assembly.LoadFile(Application.ExecutablePath);

			foreach (Type type in frmAssembly.GetTypes())
			{
				if (type.BaseType == typeof(Form))
				{
					if (type.Name == dtransaction.Rows[0][0].ToString())
					{
						Form frmShow = (Form)frmAssembly.CreateInstance(type.ToString());

						frmShow.Dock = DockStyle.Fill;
						frmShow.TopLevel = false;
						frmShow.Show();
						PnlContainer.Controls.Clear();
						PnlContainer.Controls.Add(frmShow);
					}
				}
			}
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			contextMenuStrip1.Show(pictureBox2, 0, pictureBox2.Height);
		}

		private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CaptureUserActivity captureUserActivity = new CaptureUserActivity();
			captureUserActivity.UserActivity("Logout");

			this.Close();
			LoginPage loginPage = new LoginPage();
			loginPage.Show();
		}

		private void profileViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UserProfile userProfile = new UserProfile();
			userProfile.ShowDialog();
		}

		private void LoadDashboardSummary()
		{
			DashboardSummary un = new DashboardSummary();
			un.Dock = DockStyle.Fill;
			un.TopLevel = false;
			un.Show();
			PnlContainer.Controls.Clear();
			PnlContainer.Controls.Add(un);
			PnlContainer.Controls["DashboardSummary"].BringToFront();
		}

		private string GetRoleName(int roleid)
		{
			string rolname = "";

			using (SqlConnection con3 = new SqlConnection(connectionString))
			{
				SqlCommand com3 = new SqlCommand("select RoleName from Roles where RoleId='" + roleid + "'", con3);

				con3.Open();
				SqlDataReader reader3 = com3.ExecuteReader();

				if (reader3.HasRows)
				{
					if (reader3.Read())
					{
						rolname = reader3.GetValue(0).ToString();
					}
				}
				else
				{

				}
				con3.Close();

			}

			return rolname;
		}
	}
}
