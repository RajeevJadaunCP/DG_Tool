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
    public partial class Script_TemplateListView : Form
    {
        public static string OutTempID;
        public static string OutTempName;

        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        private string GetOriginalScriptName(string id)
        {
            string scriptName = "";

            string query = @"
        SELECT 
            SUBSTRING(
                header,
                CHARINDEX('scripts', header) + 8,
                CHARINDEX('#', header) -
                (CHARINDEX('scripts', header) + 8)
            )
        FROM OutFileTemplateHD
        WHERE OutPutFileTemplateHdID = @ID";

            using (SqlConnection conn =
                new SqlConnection(connectionString))
            using (SqlCommand cmd =
                new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(id));

                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    scriptName = result.ToString();
                }
            }

            return scriptName;
        }

        private bool UpdateScriptName(string id,string newScriptName)
        {
            string updateQuery = @"
        UPDATE OutFileTemplateHD
        SET header = STUFF(
            header,
            CHARINDEX('scripts', header) + LEN('scripts\'),
            CHARINDEX('#', header) - (CHARINDEX('scripts', header) + LEN('scripts\')),
            @ScriptName
        )
        WHERE OutPutFileTemplateHdID = @ID";

            using (SqlConnection conn =
                new SqlConnection(connectionString))
            using (SqlCommand cmd =
                new SqlCommand(updateQuery, conn))
            {
                cmd.Parameters.Add("@ScriptName", SqlDbType.VarChar)
                              .Value = newScriptName;

                cmd.Parameters.Add("@ID", SqlDbType.Int)
                              .Value = Convert.ToInt32(id);

                conn.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        public Script_TemplateListView()
        {
            InitializeComponent();
        }

        private void Script_TemplateListView_Load(object sender, EventArgs e)
        {
            label2.Text = Script_TemplateList.CustomerName;
            label3.Text = Script_TemplateList.CircleName;
            label5.Text = Script_TemplateList.CustomerProfileName;

            LoadTemplateData();
        }
        private void LoadTemplateData()
        {
            // Clear any previous columns before rebinding
            dataGridView1.Columns.Clear();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Included header column in the SELECT query
                string query = @"
            SELECT 
                OutPutFileTemplateHdID AS ID,
                SUBSTRING(
                    header,
                    CHARINDEX('scripts\', header) + 8,
                    CHARINDEX('#', header) - (CHARINDEX('scripts\', header) + 8)
                ) AS ScriptName,
                header AS Header
            FROM OutFileTemplateHD
            WHERE ProfileFileID = 3
              AND ProfileID = @profileid
              AND header LIKE '%scripts\%'";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@profileid", SqlDbType.Int).Value =
                        Convert.ToInt32(Script_TemplateList.CustomerProfileID);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
            }

            // Grid layout & read-only restrictions
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.Columns["ID"].ReadOnly = true;
            dataGridView1.Columns["Header"].ReadOnly = true;
            dataGridView1.Columns["ScriptName"].ReadOnly = true;

            // Add Action button column
            DataGridViewButtonColumn actionColumn = new DataGridViewButtonColumn
            {
                Name = "Action",
                HeaderText = "Action",
                Text = "Edit",
                UseColumnTextForButtonValue = true, // Sets "Edit" for all rows automatically
                FlatStyle = FlatStyle.Standard
            };
            dataGridView1.Columns.Add(actionColumn);
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridView1.Columns[e.ColumnIndex].Name != "Action")
                return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            string action = row.Cells["Action"].Value?.ToString() ?? "Edit";

            // ==========================================
            // 1. SWITCH TO EDIT MODE
            // ==========================================
            if (action == "Edit")
            {
                // Unbind the uniform button text for this cell so it can toggle independently
                DataGridViewButtonCell btnCell = (DataGridViewButtonCell)row.Cells["Action"];
                btnCell.UseColumnTextForButtonValue = false;
                btnCell.Value = "Update";

                // Make ONLY the selected row's ScriptName editable
                row.Cells["ScriptName"].ReadOnly = false;

                // Set active cell for direct editing
                dataGridView1.CurrentCell = row.Cells["ScriptName"];
                dataGridView1.BeginEdit(true);
            }
            // ==========================================
            // 2. PROCESS UPDATE
            // ==========================================
            else if (action == "Update")
            {
                dataGridView1.EndEdit();

                string newScriptName = row.Cells["ScriptName"].Value?.ToString().Trim() ?? "";
                string id = row.Cells["ID"].Value?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(newScriptName))
                {
                    MessageBox.Show("Script Name cannot be empty.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string oldScriptName = GetOriginalScriptName(id);

                if (oldScriptName.Equals(newScriptName, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("No change has been made to the Script Name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    row.Cells["ScriptName"].ReadOnly = true;
                    row.Cells["Action"].Value = "Edit";
                    return;
                }

                // Confirmation Dialog
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to change the Script Name?\n\nOld Script Name: {oldScriptName}\nNew Script Name: {newScriptName}",
                    "Confirm Script Name Change",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return; // Leave in Update mode so the user can continue editing or cancel
                }

                // Run Update Query
                if (UpdateScriptName(id, newScriptName))
                {
                    // Lock cell back to non-editable
                    row.Cells["ScriptName"].ReadOnly = true;

                    // Switch button text back to Edit
                    row.Cells["Action"].Value = "Edit";

                    // Update the in-memory Header display without re-querying everything
                    string currentHeader = row.Cells["Header"].Value?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(currentHeader))
                    {
                        row.Cells["Header"].Value = currentHeader.Replace(oldScriptName, newScriptName);
                    }

                    MessageBox.Show("Script Name updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update Script Name in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //private void Script_TemplateListView_Load(object sender, EventArgs e)
        //{
        //    label2.Text = Script_TemplateList.CustomerName;
        //    label3.Text = Script_TemplateList.CircleName;
        //    label5.Text = Script_TemplateList.CustomerProfileName;

        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        SqlCommand com = new SqlCommand("select OutPutFileTemplateHdID as ID, SUBSTRING(   header,    CHARINDEX('scripts', header) + 8, CHARINDEX('#', header) - (CHARINDEX('scripts', header)+8)) AS ScriptName, header  from OutFileTemplateHD where ProfileFileID =3 and ProfileID =@profileid and header like '%script%' ", con);
        //        com.CommandType = CommandType.Text;
        //        //com.Parameters.AddWithValue("@custid", Convert.ToInt32(TemplateList.CustomerID));
        //        com.Parameters.AddWithValue("@profileid", Convert.ToInt32(Script_TemplateList.CustomerProfileID));

        //        con.Open();
        //        using (SqlDataAdapter sda = new SqlDataAdapter(com))
        //        {
        //            //Fill the DataTable with records from Table.
        //            DataTable dt = new DataTable();
        //            sda.Fill(dt);

        //            dataGridView1.DataSource = dt;

        //            //dataGridView1.Columns["HeaderCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //            //dataGridView1.Columns["FileCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        //            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //            // dataGridView1.Columns["HeaderStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //            //  dataGridView1.Columns["LinesStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //            //  dataGridView1.Columns["FileID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //            //   dataGridView1.Columns["LinesCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        //            DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();

        //            dgvButton.FlatStyle = FlatStyle.System;

        //            dgvButton.HeaderText = "Action";
        //            dgvButton.Name = "Action";
        //            dgvButton.UseColumnTextForButtonValue = true;
        //            dgvButton.Text = "View";

        //            dataGridView1.Columns.Add(dgvButton);
        //        }
        //    }
        //}

        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    //if (e.ColumnIndex == dataGridView1.Columns["Action"].Index)
        //    //{
        //    //    OutTempID = dataGridView1.Rows[e.RowIndex].Cells["FileID"].Value.ToString();
        //    //    OutTempName = dataGridView1.Rows[e.RowIndex].Cells["FileType"].Value.ToString();

        //    //    ConfirmProccess genrateTemplate = new ConfirmProccess();
        //    //    genrateTemplate.ShowDialog();

        //    //    //  MessageBox.Show("Lines Button Clicked " + (e.RowIndex + 1).ToString());
        //    //}
        //    if (e.RowIndex < 0 || e.ColumnIndex < 0)
        //        return;

        //    var clickedColumn = dataGridView1.Columns[e.ColumnIndex].Name;


        //    // Edit/Update button logic
        //    if (clickedColumn == "Action")
        //    {
        //        DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
        //        string currentAction = row.Cells["Action"].Value?.ToString();

        //        if (currentAction == "Edit")
        //        {
        //            dataGridView1.ReadOnly = false;
        //            row.ReadOnly = false;

        //            ((DataGridViewButtonCell)row.Cells["Action"]).Value = "Update";


        //            row.Cells["Action"].Value = "Update";
        //        }
        //        else if (currentAction == "Update")
        //        {
        //            string ScriptName = row.Cells["ScriptName"].Value?.ToString() ?? "";
        //            //string password = row.Cells["Password"].Value?.ToString() ?? "";
        //            //string role = row.Cells["role"].Value?.ToString() ?? "";
        //            //string isActive = row.Cells["isactive"].Value?.ToString() ?? "";
        //            string id = row.Cells["ID"].Value?.ToString() ?? "";
        //            //if (password.Contains(" "))
        //            //{
        //            //    MessageBox.Show("Password should not contain spaces.");
        //            //    return;
        //            //}

        //            //if (!int.TryParse(role, out _) || !int.TryParse(isActive, out _))
        //            //{
        //            //    MessageBox.Show("Role and IsActive must be integers.");
        //            //    return;
        //            //}

        //            // TODO: UPDATE TO DATABASE HERE
        //            string updateQuery = $"UPDATE OutFileTemplateHD SET header =  STUFF(        header,     CHARINDEX('scripts', header) + LEN('scripts\\'),        CHARINDEX('#', header) - (CHARINDEX('scripts', header) + LEN('scripts\')),         @ScriptName ) WHERE ProfileID = @profileid and ProfileFileID = 3; ";
        //            using (SqlConnection conn = new SqlConnection(connectionString))
        //            using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@ScriptName", ScriptName);
        //                cmd.Parameters.AddWithValue("@profileid", Convert.ToInt32(Script_TemplateList.CustomerProfileID));


        //                conn.Open();
        //                cmd.ExecuteNonQuery();
        //            }
        //            row.ReadOnly = true;
        //            ((DataGridViewButtonCell)row.Cells["Action"]).Value = "Edit";

        //            //// Mask the password again
        //            //actualPasswords[e.RowIndex] = password;
        //            //row.Cells["Password"].Value = new string('*', password.Length);
        //        }
        //    }
        //}

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
