using DG_Tool.WinForms.Customer;
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
    public partial class ProfileAttributeList : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public ProfileAttributeList()
        {
            InitializeComponent();
            GetProfileAttribute();

        }

        private void GetProfileAttribute()
        {
            try
            {
                SqlDataReader reader= null;
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter("SELECT *FROM ProfileAttributeHD", con))
                    {
                        sda.Fill(dt);
                        dgvProfileAttributeList.DataSource= dt;

                        //DataGridViewButtonColumn editbutton = new DataGridViewButtonColumn();
                        //editbutton.FlatStyle = FlatStyle.Popup;
                        //editbutton.HeaderText = "Action";
                        //editbutton.Text = "Edit";
                        //editbutton.UseColumnTextForButtonValue = true;
                        //editbutton.Name = "SelectFile";
                        //editbutton.Width = 60;

                        dgvProfileAttributeList.EnableHeadersVisualStyles = false;
                        dgvProfileAttributeList.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                        dgvProfileAttributeList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                        dgvProfileAttributeList.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                        //dgvFileSetup.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


                        dgvProfileAttributeList.Columns["ProfileAttributeHDID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvProfileAttributeList.Columns["ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


                        DataGridViewButtonColumn manageButton = new DataGridViewButtonColumn();
                        manageButton.FlatStyle = FlatStyle.Popup;
                        manageButton.HeaderText = "Manage";
                        manageButton.Text = "Manage";
                        manageButton.UseColumnTextForButtonValue = true;
                        manageButton.Name = "Manage";
                        manageButton.Width = 60;

                        if (dgvProfileAttributeList.Columns.Contains(manageButton.Name = "Manage"))
                        {

                        }
                        else
                        {
                            //dgvProfileAttributeList.Columns.Add(editbutton);
                            dgvProfileAttributeList.Columns.Add(manageButton);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                    );
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtProfileAttributeHD.Text))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO ProfileAttributeHD(HeaderName) VALUES(@header);", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@header", txtProfileAttributeHD.Text);
                            cmd.ExecuteReader();

                            MessageBox.Show("Saved successfully",
                                        "Message",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
                            GetProfileAttribute();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning
                                        );
                }
            }
            else
            {
                MessageBox.Show("Fields can't be empty",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning
                                        );
            }
            
            
        }

        private void dgvProfileAttributeList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProfileAttributeList.Columns[e.ColumnIndex].HeaderText == "Manage")
            {
                int id = Convert.ToInt32(dgvProfileAttributeList.Rows[e.RowIndex].Cells["Id"].Value);
                string prfileHeader = dgvProfileAttributeList.Rows[e.RowIndex].Cells["HeaderName"].Value.ToString();

                ManageProfileAttribute manageProfileAttribute = new ManageProfileAttribute(id,prfileHeader);
                manageProfileAttribute.ShowDialog();
                this.Hide();
            }
        }

        private void txtProfileAttributeHD_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
