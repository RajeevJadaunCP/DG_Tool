using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;


namespace DG_Tool.WinForms.InputFile
{
    public partial class Input_File_View : Form
    {
        string ConStr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public Input_File_View()
        {
            InitializeComponent();
        }

        private void Input_File_View_Load(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter();

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetInputTemplateDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@profileid", Convert.ToInt32(CreateInputTemplate.CustomerProfileID));
                    cmd.Parameters.AddWithValue("@customerid", Convert.ToInt32(CreateInputTemplate.CustomerID));

                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.EnableHeadersVisualStyles = false;
                    dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                    dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    for (int i = 0;i < dataGridView1.Columns.Count-1;i++)
                    {
                        dataGridView1.Columns[i].ReadOnly = true;
                    }
                    dataGridView1.Columns[dataGridView1.Columns.Count-1].ReadOnly= false;
                }
            }
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void UpdateDatabase(SqlConnection conn, int id, string value)
        {
            string updateQuery = "UPDATE InPutDataTemplate SET Tag = @value WHERE InPutDataTemplateID = @id";

            using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
            {
                cmd.Parameters.AddWithValue("@value", value);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Columns.Count > 1)
            {
                int idColumnIndex = 0;
                int valueColumnIndex = dataGridView1.Columns.Count - 1;
                using (SqlConnection conn = new SqlConnection(ConStr))
                {
                    conn.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            var idCell = row.Cells[idColumnIndex].Value;
                            var valueCell = row.Cells[valueColumnIndex].Value;

                            if (idCell != null && valueCell != null && !string.IsNullOrEmpty(valueCell.ToString()))
                            {
                                int id = Convert.ToInt32(idCell);
                                string valueToUpdate = valueCell.ToString();

                                UpdateDatabase(conn, id, valueToUpdate);
                            }
                        }
                    }

                    conn.Close();
                }
                MessageBox.Show("Varible Updated Sucessfully.");
            }
        }
    }
}
