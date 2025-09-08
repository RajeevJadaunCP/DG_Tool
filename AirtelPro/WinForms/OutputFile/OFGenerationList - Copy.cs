using System;
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

namespace AirtelPro.WinForms.OutputFile
{
    public partial class OFGenerationList : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public OFGenerationList()
        {
            InitializeComponent();
        }
        public void GetInputFiles()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_GetDataGenerationHeaderFiles", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //Todo: Set parameter Data dynamically
                        cmd.Parameters.AddWithValue("@dataGenProcessHDID",1);

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGVInputFiles.DataSource = dt;
                        }

                        MessageBox.Show("Profile created successfully",
                                        "Message",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went worng while creating profile: " + ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
