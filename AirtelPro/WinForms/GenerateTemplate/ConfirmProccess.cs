using DG_Tool.WinForms.GenerateTemplate;
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

namespace DG_Tool
{
    public partial class ConfirmProccess : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public ConfirmProccess()
        {
            InitializeComponent();

            label2.Text = TemplateListView.OutTempName;
        }

        private void ConfirmProccess_Load(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand("GetGridDataLines", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@outfileid", TemplateListView.OutTempID);
                com.Parameters.AddWithValue("@profileid", TemplateList.CustomerProfileID);
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(com))
                {
                    //Fill the DataTable with records from Table.
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    dataGridView1.DataSource = ConvertGrid(dt);
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                                      
                }
            }
        }

        public static DataTable ConvertGrid(DataTable dt)

        {

            DataTable dtConvert = new DataTable();

            for (int i = 0; i <= dt.Rows.Count; i++)

            {

                dtConvert.Columns.Add("Col_" + Convert.ToString(i));

            }

            for (int i = 0; i < dt.Columns.Count; i++)

            {

                dtConvert.Rows.Add();

                dtConvert.Rows[i][0] = dt.Columns[i].ColumnName;

                for (int j = 0; j < dt.Rows.Count; j++)

                {

                    dtConvert.Rows[i][j + 1] = dt.Rows[j][i];

                }

            }

            return dtConvert;

        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
