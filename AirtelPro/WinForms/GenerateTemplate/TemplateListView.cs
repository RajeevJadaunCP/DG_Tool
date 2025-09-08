using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DG_Tool.WinForms.GenerateTemplate
{
    public partial class TemplateListView : Form
    {
        public static string OutTempID;
        public static string OutTempName;

        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);


        public TemplateListView()
        {
            InitializeComponent();
        }

        private void TemplateListView_Load(object sender, EventArgs e)
        {
            label2.Text = TemplateList.CustomerName;
            label3.Text = TemplateList.CircleName;
            label5.Text = TemplateList.CustomerProfileName;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand("GetGridData", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@custid", Convert.ToInt32(TemplateList.CustomerID));
                com.Parameters.AddWithValue("@profileid", Convert.ToInt32(TemplateList.CustomerProfileID));

                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(com))
                {
                    //Fill the DataTable with records from Table.
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    dataGridView1.DataSource = dt;

                    //dataGridView1.Columns["HeaderCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    //dataGridView1.Columns["FileCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    // dataGridView1.Columns["HeaderStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    //  dataGridView1.Columns["LinesStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    //  dataGridView1.Columns["FileID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    //   dataGridView1.Columns["LinesCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();

                    dgvButton.FlatStyle = FlatStyle.System;

                    dgvButton.HeaderText = "Action";
                    dgvButton.Name = "Action";
                    dgvButton.UseColumnTextForButtonValue = true;
                    dgvButton.Text = "View";

                    dataGridView1.Columns.Add(dgvButton);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Action"].Index)
            {
                OutTempID = dataGridView1.Rows[e.RowIndex].Cells["FileID"].Value.ToString();
                OutTempName = dataGridView1.Rows[e.RowIndex].Cells["FileType"].Value.ToString();

                ConfirmProccess genrateTemplate = new ConfirmProccess();
                genrateTemplate.ShowDialog();

                //  MessageBox.Show("Lines Button Clicked " + (e.RowIndex + 1).ToString());
            }
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
