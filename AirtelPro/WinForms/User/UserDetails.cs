using DG_Tool.HelperClass;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DG_Tool.Properties;
using System.IO;
using System.Reflection;

namespace DG_Tool.WinForms.User
{
    public partial class UserDetails : Form
    {
        string connsctionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public static int id = 0;

        public UserDetails()
        {
            InitializeComponent();
            GetAllEmployeeRecord();

            CaptureUserActivity captureUserActivity = new CaptureUserActivity();
            //captureUserActivity.UserActivity("User Details List");
        }
        private void GetAllEmployeeRecord()
        {
            SqlDataAdapter da = new SqlDataAdapter();

            using (SqlConnection con = new SqlConnection(connsctionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_UserList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridViewEmpDetails.DataSource = dt;

                    //dataGridViewEmpDetails.Columns[1].Width = 108;

                    dataGridViewEmpDetails.EnableHeadersVisualStyles = false;
                    dataGridViewEmpDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                    dataGridViewEmpDetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dataGridViewEmpDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    dataGridViewEmpDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridViewEmpDetails.Columns["UserId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dataGridViewEmpDetails.CellContentClick += new DataGridViewCellEventHandler(dataGridViewEmpDetails_CellContentClick);

                    DataGridViewImageColumn img = new DataGridViewImageColumn();
                    img.Image = DG_Tool.Properties.Resources.Preview_2_icon;
                    img.Width = 10;
                    img.ImageLayout = DataGridViewImageCellLayout.Normal;
                    // dataGridView1.Columns.Add(img);
                    img.HeaderText = "View";
                    img.Name = "img";
                    img.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    dataGridViewEmpDetails.Columns.Insert(0, img);

                    DataGridViewButtonColumn View = new DataGridViewButtonColumn();
                    {
                        View.Name = "btnView";
                        View.HeaderText = "View";
                        View.Text = "View";
                        //View.FlatStyle= FlatStyle.Popup;
                        //View.CellTemplate.Style.BackColor = Color.IndianRed;
                        View.UseColumnTextForButtonValue = true; //dont forget this line
                                                                 // this.dataGridViewEmpDetails.Columns.Add(View);

                        // dataGridViewEmpDetails.Columns.Insert(0,View);
                    }

                }
            }
        }
        private void dataGridViewEmpDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewEmpDetails.Columns[e.ColumnIndex].HeaderText == "View")
            {
                if (e.RowIndex != -1)
                {
                    dataGridViewEmpDetails.CurrentRow.Selected = true;
                    id = Convert.ToInt32(dataGridViewEmpDetails.Rows[e.RowIndex].Cells["UserId"].Value);

                    int statusId = GetStatus(id);

                    if (statusId == 9)
                    {
                        AddUser.pid = id;
                        this.Hide();
                        VaildateUser register = new VaildateUser();
                        register.ShowDialog();
                    }
                    else if (statusId == 10)
                    {
                        // this.Hide();
                        UserProfile profile = new UserProfile();
                        profile.ShowDialog();
                    }
                }
            }

        }
        private int GetStatus(int id)
        {
            using (SqlConnection con = new SqlConnection(connsctionString))
            {
                DataTable dt = new DataTable();
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM UserDetails WHERE id = @id", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                    sda.Fill(dt);

                    return Convert.ToInt32(dt.Rows[0]["Status"]);
                }
            }
        }
        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            UserDetails.id = 0;
            AddUser register = new AddUser();
            register.ShowDialog();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (CommonClass.PreClosingConfirmation() == DialogResult.Yes)
            {
                this.Close();
            }
        }
        private void UserDetails_Load(object sender, EventArgs e)
        {
            //int width = Screen.PrimaryScreen.Bounds.Width;
            //int height = Screen.PrimaryScreen.Bounds.Height;

            //this.Location = new Point(0, 200);
            //this.Size = new Size(width, height - 100);
        }
    }
}
