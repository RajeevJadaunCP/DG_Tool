using DG_Tool.HelperClass;
using DG_Tool.WinForms.Authentication;
using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DG_Tool.WinForms.InputFile
{
    public partial class Create_Algo : Form
    {
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        DataTable dt = new DataTable();
        int varcounter = 0;
        int uncheckcount = 0,checkcount=0;

        //CheckBox headerCheckBox = new CheckBox();

        public Create_Algo()
        {
            InitializeComponent();
            GetFiles();
            getLastVariable();

        }

        public void GetFiles()
        {
            List<FieldMaster> files = new List<FieldMaster>();
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select FieldID,FieldName,AlgoName,AlgoDes from [dbo].[ConstantMaster] cm inner join [dbo].[AlgoMaster] am on am.AlgoID=cm.AlgoID", con))
                {
                    cmd.CommandType = CommandType.Text;
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            files.Add(new FieldMaster
                            {
                                FieldID = reader["FieldID"].ToString(),
                                FieldName = reader["FieldName"].ToString(),
                                AlgoName = reader["AlgoName"].ToString(),
                                AlgoDes = reader["AlgoDes"].ToString(),
                            });
                        }
                    }
                    dgvFileSelection.DataSource = files;

                    dgvFileSelection.EnableHeadersVisualStyles = false;
                    dgvFileSelection.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                    dgvFileSelection.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgvFileSelection.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    dgvFileSelection.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    dgvFileSelection.Columns[0].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    dgvFileSelection.Columns[1].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    dgvFileSelection.Columns[2].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    dgvFileSelection.Columns[3].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    //headerCheckBox.Location = new Point(dgvFileSelection.Width - headerCheckBox.Width, 4);
                
                    //headerCheckBox.Location = new Point(1000, 4);
                    //headerCheckBox.Size = new Size(18, 18);

                    //Assign Click event to the Header CheckBox.
                    headerCheckBox.Click += new EventHandler(HeaderCheckBox_Clicked);



                }
            }
        }

        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            //Necessary to end the edit mode of the Cell.
            dgvFileSelection.EndEdit();

            //Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
            foreach (DataGridViewRow row in dgvFileSelection.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells["Selected"] as DataGridViewCheckBoxCell);
                checkBox.Value = headerCheckBox.Checked;

            }
        }


        private void btnSave_Click(object sender, System.EventArgs e)
        {
            int i = 0;
            List<FieldMaster> files = ((List<FieldMaster>)dgvFileSelection.DataSource)
                .Where(p => p.IsSelected).ToList();
            int[] data = new int[files.Count];

            foreach (FieldMaster file in files)
            {
                data[i] = Convert.ToInt32(file.FieldID);
                i++;
            }
            SaveProfileFile(1, data);
        }

        private void SaveProfileFile(int profileID, int[] ids)
        {
            if (profileID > 0 && ids.Length!=0)
            {
                foreach (int id in ids)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("insert into [dbo].[InPutDataTemplate]([InPutDataTemplateID], [CustID], [ProfileID], [VarName], [VarValue], [VarDes], [VarValType], [VarText], [AlgoID], [AlgoName], [FileID], [LineNumber], [VarWay], [PositionFrom], [PositionTo], [Len], [Tag]) " +
                            "select '"+varcounter+$"' as TemplateID,'{CreateInputTemplate.CustomerID}' as CustID,'{CreateInputTemplate.CustomerProfileID}' as ProfileID,'"+ VarNameGenerate()+ "' as VarName,'' as VarValue ,FieldName as VarDes,AlgoType as VarValType,'AL' as VarText,cm.AlgoID,AlgoName,'11' as FileID,'0' as LineNumber,'' as VarWay,'0' as PositionFrom,'0' as PositionTo,'0' as Len,'' as Tag from [dbo].[ConstantMaster] cm inner join [dbo].[AlgoMaster] am on am.AlgoID=cm.AlgoID where FieldId=@id", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteReader();


                        }
                    }
                }

                MessageBox.Show("Algo Saved Successfully ",
                                                    "Message",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information
                                                    );
                
            }
            else
            {
                MessageBox.Show("Error: No a valid profileId ",
                                                    "Error",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Warning
                                                    );
            }
        }

        public void getLastVariable()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand($"SELECT TOP 1 VarName FROM InPutDataTemplate WHERE CustID={CreateInputTemplate.CustomerID}and ProfileID={CreateInputTemplate.CustomerProfileID} ORDER BY InPutDataTemplateID DESC", con))
                {
                    SqlDataReader reader1 = cmd.ExecuteReader();

                    if (reader1.Read())
                    {
                        string mydata = reader1.GetValue(0).ToString().Trim();

                        string res = mydata.Substring(mydata.Length - 3);

                        varcounter = Int32.Parse(res);

                    }
                }
                con.Close();
            }
        }

        public string VarNameGenerate()
        {
            string charvar = "V";

            varcounter++;

            string sendval = charvar + varcounter.ToString("000");

            return sendval;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Create_New_Algo  new_Algo = new Create_New_Algo();
            new_Algo.ShowDialog();
            GetFiles();
            getLastVariable();

        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvFileSelection_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Check to ensure that the row CheckBox is clicked.
            if (e.RowIndex >= 0 && e.ColumnIndex == 4)
            {
                //Reference the GridView Row.
                DataGridViewRow row = dgvFileSelection.Rows[e.RowIndex];

                //Set the CheckBox selection.
                row.Cells["Selected"].Value = !Convert.ToBoolean(row.Cells["Selected"].EditedFormattedValue);

                //If CheckBox is checked, display Message Box.
                if (Convert.ToBoolean(row.Cells["Selected"].Value))
                {
                    uncheckcount++;

                   // MessageBox.Show("check count: " + checkcount + "\nuncheck count  " + uncheckcount+"  "+ row.Cells["Selected"].Value);

                }
                else
                {
                    headerCheckBox.Checked = false;
                    checkcount++;

                   // MessageBox.Show("check count: " + checkcount + "\nuncheck count  " + uncheckcount + "  " + row.Cells["Selected"].Value);
                }
            }

            if (uncheckcount == checkcount)
            {
                headerCheckBox.Checked = true;
                checkcount = 0;
                uncheckcount = 0;
            }
            else
            {
                headerCheckBox.Checked = false;
            }
        }
    }
}
