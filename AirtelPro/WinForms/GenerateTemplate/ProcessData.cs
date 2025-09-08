using DG_Tool.WinForms.GenerateTemplate;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;


namespace DG_Tool
{
    public partial class ProcessData : Form
    {
        public static string OutTempID;
        public static string OutTempName;

        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public ProcessData()
        {
            InitializeComponent();
        }

        private void ProcessData_Load(object sender, EventArgs e)
        {
            label2.Text = CreateTemplate.CustomerName;
            label3.Text = CreateTemplate.CircleName;
            label5.Text = CreateTemplate.CustomerProfileName;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand("GetGridData", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@custid", Convert.ToInt32(CreateTemplate.CustomerID));
                com.Parameters.AddWithValue("@profileid", Convert.ToInt32(CreateTemplate.CustomerProfileID));
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(com))
                {
                    //Fill the DataTable with records from Table.
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                   // dataGridView1.Columns["HeaderStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                  //  dataGridView1.Columns["LinesStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                  //  dataGridView1.Columns["FileID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                 //   dataGridView1.Columns["LinesCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();

                    dgvButton.FlatStyle = FlatStyle.System;

                    dgvButton.HeaderText = "Header";
                    dgvButton.Name = "Header";
                    dgvButton.UseColumnTextForButtonValue = true;
                    dgvButton.Text = "Create";

                    dataGridView1.Columns.Add(dgvButton);

                    DataGridViewButtonColumn btnColumn1 = new DataGridViewButtonColumn();
                    btnColumn1.Name = "Lines";
                    btnColumn1.HeaderText = "Lines";
                    btnColumn1.Text = "Create";
                    btnColumn1.UseColumnTextForButtonValue = true;
                    btnColumn1.CellTemplate.Style.BackColor = Color.GreenYellow;

                    dataGridView1.Columns.Add(btnColumn1);
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                   
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Lines"].Index)
            {
                OutTempID = dataGridView1.Rows[e.RowIndex].Cells["FileID"].Value.ToString();
                OutTempName = dataGridView1.Rows[e.RowIndex].Cells["FileType"].Value.ToString();

                //this.Hide();
                CreateTemplateLines genrateTemplate=new CreateTemplateLines();
                genrateTemplate.WindowState = FormWindowState.Maximized;
                genrateTemplate.ShowDialog();

              //  MessageBox.Show("Lines Button Clicked " + (e.RowIndex + 1).ToString());
            }
            else if (e.ColumnIndex == dataGridView1.Columns["Header"].Index)
            {
                OutTempID = dataGridView1.Rows[e.RowIndex].Cells["FileID"].Value.ToString();
                OutTempName = dataGridView1.Rows[e.RowIndex].Cells["FileType"].Value.ToString();

                //this.Hide();
                CreateOutfileHeader genrateTemplate = new CreateOutfileHeader();
                genrateTemplate.ShowDialog();

                // MessageBox.Show("Header Button Clicked " + (e.RowIndex + 1).ToString());
            }
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
