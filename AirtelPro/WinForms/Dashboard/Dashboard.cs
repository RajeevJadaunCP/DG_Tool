using DG_Tool.HelperClass;
using DG_Tool.WinForms.Authentication;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;



namespace DG_Tool
{
    public partial class Dashboard : Form
    {
        public static int mycount;

        SqlConnection conn;
        MenuStrip MnuStrip;
        ToolStripMenuItem MnuStripItem;

        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public Dashboard()
        {
            InitializeComponent();

            timer1.Start();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            label3.Text=NewLogin.username;
           
            label22.Text = GetRoleName(NewLogin.roleId);

            CaptureUserActivity captureUserActivity = new CaptureUserActivity();
            captureUserActivity.UserActivity("Dashboard");

            //Creating object of MenuStrip class

            //this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            //this.WindowState = FormWindowState.Maximized;

            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;

            MnuStrip = new MenuStrip();
            MnuStrip.BackColor = Color.LightSalmon;
            MnuStrip.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			//Placing the control to the Form

			

			// this.Controls.Add(MnuStrip);
			panel1.Controls.Add(MnuStrip);

            conn = new SqlConnection(connectionString);

            String Sequel = "select MenuId, MenuName from MenuMaster where role='"+NewLogin.roleId+"'";

            SqlDataAdapter da = new SqlDataAdapter(Sequel, conn);

            DataTable dt = new DataTable();

            conn.Open();

            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)

            {

                MnuStripItem = new ToolStripMenuItem(dr["MenuName"].ToString(), null, new EventHandler(MenuClick));
                // MnuStripItem.BackColor = Color.DarkSeaGreen;
                MnuStripItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
                // MnuStripItem.Size= new System.Drawing.Size(50, 50);
                MnuStripItem.Image = Properties.Resources.customer;
                SubMenu(MnuStripItem, dr["MenuId"].ToString());

                MnuStripItem.Padding = new System.Windows.Forms.Padding(8);
                MnuStrip.Padding = new System.Windows.Forms.Padding(5);
                MnuStripItem.Margin = new System.Windows.Forms.Padding(2);

                MnuStrip.Items.Add(MnuStripItem);

            }

            // The Form.MainMenuStrip property determines the merge target.

            this.MainMenuStrip = MnuStrip;
            conn.Close();
        }

		public void SubMenu(ToolStripMenuItem mnu, string submenu)

        {

            String Seqchild = "SELECT MenuName FROM SubMenuMaster WHERE MenuId='" + submenu + "' and Role='"+NewLogin.roleId+"'";

            SqlDataAdapter dachildmnu = new SqlDataAdapter(Seqchild, conn);

            DataTable dtchild = new DataTable();

            dachildmnu.Fill(dtchild);



            foreach (DataRow dr in dtchild.Rows)

            {

                ToolStripMenuItem SSMenu = new ToolStripMenuItem(dr["MenuName"].ToString(), null, new EventHandler(SubMenuClick));
               SSMenu.Image = Properties.Resources.customer;
                mnu.DropDownItems.Add(SSMenu);

            }

        }

        private void MenuClick(object sender, EventArgs e)

        {

            //  MessageBox.Show(string.Concat("You have Clicked ", sender.ToString(), " Menu"), "Menu Items Event",MessageBoxButtons.OK, MessageBoxIcon.Information);



            String Seqtx = "SELECT FormName FROM MenuMaster WHERE MenuName='" + sender.ToString() + "'";

            SqlDataAdapter datransaction = new SqlDataAdapter(Seqtx, conn);

            DataTable dtransaction = new DataTable();

            datransaction.Fill(dtransaction);



            Assembly frmAssembly = Assembly.LoadFile(Application.ExecutablePath);

            foreach (Type type in frmAssembly.GetTypes())
            {
                //MessageBox.Show(type.Name);

                if (type.BaseType == typeof(Form))
                {
                    if (type.Name == dtransaction.Rows[0][0].ToString())
                    {
                        Form frmShow = (Form)frmAssembly.CreateInstance(type.ToString());

                        frmShow.ShowDialog();
                        frmShow.TopMost = true;
                    }
                    
                }

            }

        }

        private void SubMenuClick(object sender, EventArgs e)

        {

           //  MessageBox.Show(string.Concat("You have Clicked ", sender.ToString(), " Menu"), "Menu Items Event",MessageBoxButtons.OK, MessageBoxIcon.Information);



            String Seqtx = "SELECT FormName FROM SubMenuMaster WHERE MenuName='" + sender.ToString() + "'";

            SqlDataAdapter datransaction = new SqlDataAdapter(Seqtx, conn);

            DataTable dtransaction = new DataTable();

            datransaction.Fill(dtransaction);



            Assembly frmAssembly = Assembly.LoadFile(Application.ExecutablePath);

            foreach (Type type in frmAssembly.GetTypes())

            {

                //MessageBox.Show(type.Name);

                if (type.BaseType == typeof(Form))

                {

                    if (type.Name == dtransaction.Rows[0][0].ToString())

                    {

                        Form frmShow = (Form)frmAssembly.CreateInstance(type.ToString());

                        // frmShow.MdiParent = this;

                        // frmShow.WindowState = FormWindowState.Maximized;

                        frmShow.ShowDialog();
                        frmShow.TopMost = true;

                    }

                }

            }

        }

        private void Dashboard_FormClosed(object sender, FormClosingEventArgs e)
        {
            CaptureUserActivity captureUserActivity = new CaptureUserActivity();
            captureUserActivity.UserActivity("Logout");

            Application.Exit();
        }

        private void changePasswordToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChangePassword changePassword = new ChangePassword();
            changePassword.ShowDialog();
        }

        private string GetRoleName(int roleid)
        {
            string rolname = "";

            using (SqlConnection con3 = new SqlConnection(connectionString))
            {
                SqlCommand com3 = new SqlCommand("select RoleName from Roles where RoleId='"+roleid+"'", con3);
                
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

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserProfile userProfile=new UserProfile();
            userProfile.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToString();
            GetCounter();
        }

        private void GetCounter()
        {
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_dashboard_counter", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        totalUsersCount.Text = reader.GetValue(0).ToString();
                        activeUsersCount.Text = reader.GetValue(1).ToString();
                        InactiveUsersCount.Text=reader.GetValue(2).ToString();
                        totalCustomersCount.Text = reader.GetValue(3).ToString();
                        customerProfileCount.Text = reader.GetValue(4).ToString();
                        TotalInputFilesCount.Text = reader.GetValue(5).ToString();
                        TotalOutputFilesCount.Text=reader.GetValue(6).ToString();
                        totalCirclesCount.Text = reader.GetValue(7).ToString();
                    }
                }
            }
        }

        private void pbLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            NewLogin login = new NewLogin();
            login.ShowDialog();
        }

        private void pbHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard dashboard = new Dashboard();
            dashboard.ShowDialog();
        }

       
    }
}