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

namespace DG_Tool.WinForms.Configure
{
    public partial class CreateDataToolParameter : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public CreateDataToolParameter()
        {
            InitializeComponent();
            GetParameterList();
            btnSubmit.Enabled = false;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtParameterName.Text) && !string.IsNullOrEmpty(txtParameterDescription.Text))
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_SaveDataToolParameters", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@parameterName", txtParameterName.Text);
                        cmd.Parameters.AddWithValue("@parameterDetails", txtParameterDescription.Text);
                        cmd.Parameters.AddWithValue("@parameterCharVal", 'Y');
                        cmd.Parameters.AddWithValue("@parameterIntVal", 0);
                        cmd.ExecuteReader();
                    }
                    MessageBox.Show("Saved successfully: ",
                                                "Message",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information
                                                );
                }
            }
            else
            {
                MessageBox.Show("Please Fill All Informations! ",
                                               "Message",
                                               MessageBoxButtons.OK,
                                               MessageBoxIcon.Error
                                               );
            }
            
        }

        private void GetParameterList()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select * FROM DataToolParameters", con))
                {
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();

                    dgvParameterList.DataSource = dt;

                    dgvParameterList.EnableHeadersVisualStyles = false;
                    dgvParameterList.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                    dgvParameterList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgvParameterList.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    dgvParameterList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dgvParameterList.Columns["ParameterID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dgvButton.FlatStyle = FlatStyle.System;

                    dgvButton.HeaderText = "Action";
                    dgvButton.Name = "Action";
                    dgvButton.UseColumnTextForButtonValue = true;
                    dgvButton.Text = "Change Status";

                    if (dgvParameterList.Columns.Contains(dgvButton.Name = "Action"))
                    {

                    }
                    else
                    {
                        dgvParameterList.Columns.Add(dgvButton);
                    }

                    dgvParameterList.Columns["ParameterIntVal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                }
            }
        }

        private void SetStatus(int parameterID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE DataToolParameters SET ParameterCharVal = CASE ParameterCharVal WHEN 'Y' THEN 'N' ELSE 'Y' END WHERE ParameterID = @id", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", parameterID);
                    cmd.ExecuteNonQuery();
                }
                GetParameterList();
            }
        }

        private void dgvParameterList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvParameterList.Columns[e.ColumnIndex].HeaderText == "Action")
            {
                dgvParameterList.CurrentRow.Selected = true;
                int parameterID = Convert.ToInt32(dgvParameterList.Rows[e.RowIndex].Cells["ParameterID"].Value);

                SetStatus(parameterID);
            }
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtParameterName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        private void txtParameterDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsSeparator(e.KeyChar);

        }
    }
}
