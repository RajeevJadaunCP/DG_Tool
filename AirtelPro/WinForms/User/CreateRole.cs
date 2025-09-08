using DG_Tool.HelperClass;
using DG_Tool.WinForms.Authentication;
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

namespace DG_Tool.WinForms.User
{
    public partial class CreateRole : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();
        int roleID = 0;

        public CreateRole()
        {
            InitializeComponent();
            //GetStatus();
            GetRoles();
        }
        private void GetRoles()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from vw_RoleList", con))
                {
                    DataTable dt = new DataTable();
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                    sda.Fill(dt);

                    dgvRoleList.DataSource = dt;

                    dgvRoleList.EnableHeadersVisualStyles = false;
                    dgvRoleList.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                    dgvRoleList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgvRoleList.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    //dgvRoleList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dgvRoleList.Columns["RolesId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dgvButton.FlatStyle = FlatStyle.System;

                    dgvButton.HeaderText = "Action";
                    dgvButton.Name = "Action";
                    dgvButton.UseColumnTextForButtonValue = true;
                    dgvButton.Text = "Change Status";

                    if (dgvRoleList.Columns.Contains(dgvButton.Name = "Action"))
                    {

                    }
                    else
                    {
                        dgvRoleList.Columns.Add(dgvButton);
                    }
                }
            }
        }
        private void SetStatus(int roleID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Roles SET IsActive = CASE IsActive WHEN 1 THEN 0 ELSE 1 END WHERE RoleID = @id", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", roleID);
                    cmd.ExecuteNonQuery();
                }
                GetRoles();
            }
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("usp_CreateRoles", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@role", txtRole.Text);
                            cmd.Parameters.AddWithValue("@createdBy", NewLogin.primaryId); //Todo
                            cmd.Parameters.AddWithValue("@isActive", 1);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Role created successfully ",
                                                "Message",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information
                                                );
                            GetRoles();
                            txtRole.Clear();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please fill all the mandatory fields: ",
                                                "Error",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information
                                                );
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
        private bool IsValidInput()
        {
            if (!string.IsNullOrEmpty(txtRole.Text))
            {
                return true;
            }
            else
                return false;
        }
        private void dgvRoleList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvRoleList.Columns[e.ColumnIndex].HeaderText == "Action")
            {
                if (e.RowIndex != -1)
                {
                    dgvRoleList.CurrentRow.Selected = true;
                    roleID = Convert.ToInt32(dgvRoleList.Rows[e.RowIndex].Cells["RolesID"].Value);

                    SetStatus(roleID);

                }
            }

        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (CommonClass.PreClosingConfirmation() == DialogResult.Yes)
            {
                this.Close();
            }
        }
        private void txtRole_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }
        private void txtRole_TextChanged(object sender, EventArgs e)
        {
            if(txtRole.Text.Trim().Length > 3)
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand($"SELECT RoleName FROM Roles  WHERE RoleName = '{txtRole.Text.Trim()}'", con))
                    {
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(dt);

                        if(dt != null && dt.Rows.Count > 0)
                        {
                            msgRoleName.Text = "Role already exist";
                            msgRoleName.ForeColor = Color.Red;
                            msgRoleName.Visible = true;
                            btnSubmit.Enabled = false;
                        }
                        else
                        {
                            msgRoleName.ForeColor = Color.Green;
                            msgRoleName.Visible = false;
                            btnSubmit.Enabled = true;
                        }
                    }
                }
            }
            else
            {
                msgRoleName.Text = "Must be greater than 3 character";
                msgRoleName.ForeColor = Color.Red;
                msgRoleName.Visible = true;
                btnSubmit.Enabled = false;
            }

        }
    }
}
