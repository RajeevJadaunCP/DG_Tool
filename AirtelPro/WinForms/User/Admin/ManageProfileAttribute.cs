using DG_Tool.WinForms.Authentication;
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

namespace DG_Tool.WinForms.User.Admin
{
    public partial class ManageProfileAttribute : Form
    {
        string connedctionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private int id = 0;
        public ManageProfileAttribute(int id,string profileAttribute)
        {
            InitializeComponent();
            this.id = id;
            lblAttribute.Text = profileAttribute;
            ProfileAttributeItems(id);
        }
        private void ProfileAttributeItems(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connedctionString))
                {
                    con.Open();
                    DataTable dt = new DataTable();
                    SqlDataReader reader = null;
                    using (SqlCommand cmd = new SqlCommand("SELECT *FROM ProfileAttribute WHERE PFId = @pfid", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@pfid", id);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(dt);
                        dgvAttributeItems.DataSource = dt;

                        dgvAttributeItems.EnableHeadersVisualStyles = false;
                        dgvAttributeItems.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                        dgvAttributeItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                        dgvAttributeItems.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    }
                }
            }
            catch(Exception ex)
            {

            }
            
        }

        private void btnAddNewItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProfileName.Text))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connedctionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO ProfileAttribute(PFId,PFName,CreatedOn,CreatedBy,Status) Values(@pfid,@pfname,@date,@createdBy,@status)", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@pfid", id);
                            cmd.Parameters.AddWithValue("@pfname", txtProfileName.Text);
                            cmd.Parameters.AddWithValue("@date", DateTime.Now);
                            cmd.Parameters.AddWithValue("@createdBy", NewLogin.primaryId);
                            cmd.Parameters.AddWithValue("@status", 1);

                            cmd.ExecuteReader();
                            MessageBox.Show("Saved successfully",
                                    "Message",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information
                                    );

                        }
                    }
                    txtProfileName.Text = string.Empty;
                }
                catch (Exception ex)
                {

                }
                ProfileAttributeItems(id);
            }
            else
            {
                MessageBox.Show("Field required",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                    );
            }
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
