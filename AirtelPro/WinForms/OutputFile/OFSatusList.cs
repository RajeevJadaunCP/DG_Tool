using DG_Tool.HelperClass;
using DG_Tool.Models;
using DG_Tool.WinForms.Customer;
using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;


namespace DG_Tool.WinForms.OutputFile
{
    public partial class OFSatusList : Form
    {
        public string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public static int LotID = 0;
        DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();
        public OFSatusList()
        {
            InitializeComponent();
            var customerList = CommonClass.GetCustomer();
            var circulList = CommonClass.GetCircle(1);
            if (customerList != null && customerList.Count > 0)
            {
                customerList.Insert(0,new CustomerDetails
                {
                    CustomerName = "----All Customers----",
                    CustomerID = 0,
                });
                cbxCustomer.DataSource = customerList;
                cbxCustomer.DisplayMember = "CustomerName";
                cbxCustomer.ValueMember = "CustomerID";

            }

            if (circulList != null && circulList.Count > 0)
            {
                circulList.Insert(0,new Circle
                {
                    CircleID = 0,
                    CustomerName = "----All Customers----",
                    CircleName = "----All Circles----",
                    Status = "Active",
                });
                cbxCircle.DataSource = circulList;
                cbxCircle.DisplayMember = "CircleName";
                cbxCircle.ValueMember = "CircleID";

            }
            dgvBriefList.ReadOnly = true;
            DataGenProcessList(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            DataGridViewButtonColumn viewbutton = new DataGridViewButtonColumn();
            viewbutton.FlatStyle = FlatStyle.System;
            viewbutton.HeaderText = "Click to View";
            viewbutton.Text = "View";
            viewbutton.UseColumnTextForButtonValue = true;
            viewbutton.Name = "view";
            viewbutton.Width = 60;
            if (dgvBriefList.Columns.Contains(viewbutton.Name = "View"))
            {

            }
            else
            {
                dgvBriefList.Columns.Add(viewbutton);
            }
        }
        private void DataGenProcessList(string customerName, string circleName, string filePath, string fromDate, string toDate)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_DataGenProcessList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customer", customerName);
                    cmd.Parameters.AddWithValue("@circle", circleName);
                    cmd.Parameters.AddWithValue("@filePath", filePath);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    dgvBriefList.DataSource = dt;

                    dgvBriefList.EnableHeadersVisualStyles = false;
                    dgvBriefList.ColumnHeadersDefaultCellStyle.BackColor = Color.IndianRed;
                    dgvBriefList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgvBriefList.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                    //dgvBriefList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    //dgvBriefList.Columns["RolesId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }
        private void dgvBriefList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvBriefList.Columns[e.ColumnIndex].HeaderText == "Click to View")
            {
                if(dgvBriefList.Rows[e.RowIndex].Cells["Status"].Value.ToString().TrimEnd()== "File Imported")
                {
                    DialogResult result = MessageBox.Show("You need to process the file to move further.\nDo you want to continue?", "Question", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        
                        LotID = Convert.ToInt32(dgvBriefList.Rows[e.RowIndex].Cells["LotID"].Value);
                        LogMaster.addlog($"Reprocessing the file associated with lotid {LotID}");
                        BufferingForm bf=new BufferingForm(LotID);
                        bf.ShowDialog();
                        MessageBox.Show("All Files Processed Sucessfully.", "infomation");
                        this.Close();
                        OFSatusList of = new OFSatusList();
                        of.ShowDialog();



                    }
                }
                else
                {
                    dgvBriefList.CurrentRow.Selected = true;
                    LotID = Convert.ToInt32(dgvBriefList.Rows[e.RowIndex].Cells["LotID"].Value);
                    DataGenHDIDProcessList dgProcessList = new DataGenHDIDProcessList();
                    dgProcessList.Show();
                }
                
            }
        }
        public List<int> DataGenProcessHDFileByHDID(int id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand($"SELECT DataGenProcessHDID as FileID FROM [DataGenProcessHD] WHERE lot={id}", con))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                }
            }
            List<int> HDIDS = dt.AsEnumerable()
                                 .Select(row => row.Field<int>("FileID"))
                                 .ToList();
            return HDIDS;

        }

        public int btnProcessAll_Click(int lastInsertedId)
        {
            OFProcessing ofp=new OFProcessing();
            int records = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string r4_data = "", r8_data = "";
            int r4_data_count = 0, r8_data_count = 0;
            List<string> r4_data_list = new List<string>();

            List<string> r8_data_list = new List<string>();

            string constr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlConnection con = new SqlConnection(constr);
            int list_4 = 0, list_8 = 0;
            //String Query1 = $"SELECT DataGenProcessData.[DataGenProcessDataID], DataGenProcessData.[DataGenProcessHDID], InPutDataTemplate.[VarName] as VarID, DataGenProcessData.[VarName], DataGenProcessData.[VarValue], DataGenProcessData.[VarType], DataGenProcessData.[StatusID],[InPutDataTemplate].algoname,[InPutDataTemplate].VarText,[InPutDataTemplate].PositionFrom,[InPutDataTemplate].Len,InPutDataTemplate.tag,(SELECT STRING_AGG(p3.VarID, ',') FROM DataGenProcessData p3 WHERE p3.VarName IN (Select value from dbo.Get_List(TRIM(InPutDataTemplate.tag)+',')) AND p3.[DataGenProcessHDID] = {lastInsertedId} AND ISNULL(tag, '') != '') AS 'VALUE_FROM' , [InPutDataTemplate].LineNumber  FROM DataGenProcessData inner JOIN [InPutDataTemplate] ON [InPutDataTemplate].vardes = DataGenProcessData.varname where  DataGenProcessData.[DataGenProcessHDID] = '" + lastInsertedId + "' and [InPutDataTemplate].ProfileID IN (SELECT CustProfileID FROM [DataGenProcessHD] WHERE DataGenProcessHDID='" + lastInsertedId + "')  order by DataGenProcessData.VarID ";
            String Query1 = $"SELECT DataGenProcessData.[DataGenProcessDataID], DataGenProcessData.[DataGenProcessHDID], InPutDataTemplate.[VarName] as VarID, DataGenProcessData.[VarName], DataGenProcessData.[VarValue], DataGenProcessData.[VarType], DataGenProcessData.[StatusID],[InPutDataTemplate].algoname,[InPutDataTemplate].VarText,[InPutDataTemplate].PositionFrom,[InPutDataTemplate].Len,InPutDataTemplate.tag, [InPutDataTemplate].LineNumber  FROM DataGenProcessData inner JOIN [InPutDataTemplate] ON [InPutDataTemplate].vardes = DataGenProcessData.varname where  DataGenProcessData.[DataGenProcessHDID] = '" + lastInsertedId + "' and [InPutDataTemplate].ProfileID IN (SELECT CustProfileID FROM [DataGenProcessHD] WHERE DataGenProcessHDID='" + lastInsertedId + "')  order by DataGenProcessData.VarID";


            System.Data.DataTable dt1 = new System.Data.DataTable();
            DataRow workRow1;
            SqlCommand sqlcom1 = new SqlCommand(Query1, con);
            SqlDataAdapter adpt1 = new SqlDataAdapter(Query1, con);
            adpt1.Fill(dt1);

            r4_data_list.Clear();
            r8_data_list.Clear();
            DataTable Process_data = new DataTable();
            DataColumn idColumn = new DataColumn("DataGenProcessDataRecordID", typeof(int));
            idColumn.AutoIncrement = true;
            idColumn.AutoIncrementSeed = 1;
            Process_data.Columns.Add(idColumn);
            Process_data.Columns.Add("DataGenProcessHDID", typeof(int)).DefaultValue = lastInsertedId;
            string[] default_values = { "AGSUI:IMSI", "EKI", "KIND", "DATE_AL", "FSETIND", "A4IND", "Transport_key", "Quantity", "Index_Value" };
            foreach (DataRow dv0 in dt1.Rows)
            {
                string var_ID = dv0[2].ToString().TrimEnd();
                string var_name = dv0[3].ToString().TrimEnd();
                string var_Value = dv0[4].ToString().TrimEnd();
                string var_Type = dv0[5].ToString().TrimEnd();
                if (var_name.TrimEnd() == "Quantity")
                {
                    records = Int32.Parse(var_Value.TrimEnd());
                }

                if (var_Type.TrimEnd() == "T")
                {
                    Process_data.Columns.Add(var_ID, typeof(string)).DefaultValue = var_Value;
                }
                else
                {
                    if (default_values.Contains(var_name.TrimEnd()))
                    {
                        Process_data.Columns.Add(var_ID, typeof(string)).DefaultValue = var_Value;
                    }
                    else
                    {
                        if (var_name.TrimEnd() == "ICCID" || var_name.TrimEnd() == "IMSI")
                        {
                            Process_data.Columns.Add(var_ID, typeof(string));
                        }
                        else if (var_name.TrimEnd() == "MSISDN")
                        {
                            if (var_Value.Contains('F'))
                            {
                                Process_data.Columns.Add(var_ID, typeof(string)).DefaultValue = var_Value;
                            }
                            else
                            {
                                Process_data.Columns.Add(var_ID, typeof(string));
                            }
                        }
                        else
                        {
                            Process_data.Columns.Add(var_ID, typeof(string));
                        }
                    }
                }
            }
            //string[] lines = .Split('\n');
            DataTable fl_data = new DataTable();
            using (SqlConnection connnection= new SqlConnection(connectionString))
            {
                connnection.Open();
                using (SqlCommand cmd = new SqlCommand($"SELECT IMSI,ICICID,MSISDN,LICENSE_KEY FROM [DGPDR_Base] WHERE DataGenProcessHDID={lastInsertedId} order by Sr_no ", connnection))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(fl_data);
                }
            }
            records = fl_data.Rows.Count;
            for (int i  = 1; i <= records; i++)
            {

                Console.WriteLine($"Processing Row : " + i);
                DataRow newRow = Process_data.NewRow();
                foreach (DataRow dv0 in dt1.Rows)
                {
                    string var_ID = dv0[2].ToString().TrimEnd();
                    string var_name = dv0[3].ToString().TrimEnd();
                    string var_Value = dv0[4].ToString().TrimEnd();
                    string var_algoname = dv0[7].ToString().TrimEnd();
                    string var_op_type = dv0[8].ToString().TrimEnd();
                    string Pos_From = dv0[9].ToString().TrimEnd();
                    string len_data = dv0[10].ToString().TrimEnd();
                    string variableID = dv0[11].ToString().TrimEnd();
                    string lineno = dv0[12].ToString().TrimEnd();
                    int varCount = variableID.Count(c => c == ',');

                    if (var_op_type == "FL")
                    {
                        string my_data = "";
                        if (var_name == "ICCID") 
                        {
                            my_data = fl_data.Rows[i - 1]["ICICID"].ToString();
                        }
                        else if(var_name == "MSISDN" )
                        {
                            my_data = fl_data.Rows[i - 1]["MSISDN"].ToString();
                        }
                        else if (var_name == "IMSI")
                        {
                            my_data = fl_data.Rows[i - 1]["IMSI"].ToString();
                        }
                        else if (var_name == "LICENSE_KEY")
                        {
                            my_data = fl_data.Rows[i - 1]["LICENSE_KEY"].ToString();
                            if (string.IsNullOrEmpty(my_data))
                            {
                                my_data = var_Value;
                            }
                        }
                        newRow[var_ID] = my_data;
                    }
                    if (var_op_type == "AL")
                    {
                        string caseSwitch = var_algoname;
                        string my_data = "";
                        r4_data = "";
                        List<string> ki_val_list = new List<string>();
                        switch (caseSwitch)
                        {

                            case "substring":
                                if (varCount > 0)
                                {
                                    MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                }
                                else
                                {
                                    int pos_from = Convert.ToInt32(dv0[9].ToString().TrimEnd());
                                    int len = Convert.ToInt32(dv0[10].ToString().TrimEnd());
                                    my_data = newRow[variableID].ToString().Substring(pos_from - 1, len);
                                }
                                break;

                            case "identical":
                                if (varCount > 0)
                                {
                                    MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                }
                                else
                                {
                                    my_data = newRow[variableID].ToString();
                                }
                                break;

                            case "R_4":
                                my_data = ofp.Random4digits();
                                break;

                            case "serial":
                                my_data = i.ToString();
                                break;

                            case "R_8_H":
                                my_data = ofp.Random8hex();
                                break;

                            case "R4_PF":
                                if (varCount > 0)
                                {
                                    MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                }
                                else
                                {
                                    my_data = ofp.padding_filler(newRow[variableID].ToString());
                                }
                                break;

                            case "R_8":
                                my_data = ofp.Random8digits();
                                r8_data_list.Add(ofp.padding(my_data));
                                break;

                            case "R8_P":
                                my_data = r8_data_list[r8_data_count];
                                r8_data_count += 1;
                                break;

                            case "ACC_Hex":
                                if (varCount > 0)
                                {
                                    MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                }
                                else
                                {
                                    my_data = ofp.acc((Int64.Parse(newRow[variableID].ToString()) + i).ToString());
                                }
                                break;

                            case "3P":
                                my_data = ofp.padding(newRow[variableID].ToString());
                                break;

                            case "R_16_Hex":
                                my_data = ofp.FetchDataFromApi(16);
                                break;

                            case "R_32_Hex":
                                my_data = ofp.FetchDataFromApi(32);
                                break;

                            case "R_48_Hex":
                                my_data = ofp.FetchDataFromApi(48);
                                break;

                            case "Pad_8":
                                my_data = ofp.Pad3_F(newRow[variableID].ToString());
                                break;

                            case "Pad_16":
                                my_data = ofp.Pad3_F(newRow[variableID].ToString());
                                break;

                            case "ICCID_NS":
                                if (varCount > 0)
                                {
                                    MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                }
                                else
                                {
                                    string icicid_num = newRow[variableID].ToString();
                                    my_data = ofp.nibble_swapped(icicid_num);
                                }
                                break;

                            case "IMSI_NS":
                                if (varCount > 0)
                                {
                                    MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                }
                                else
                                {
                                    string imsi_num = "809" + newRow[variableID].ToString();
                                    my_data = ofp.nibble_swapped(imsi_num);
                                }
                                break;

                            case "R_32_Hex_KI":
                                my_data = ofp.FetchDataFromApi(32);
                                break;

                            case "ICCID_LD":
                                if (i == 0)
                                {
                                    my_data = var_Value;
                                }
                                else
                                {
                                    if (varCount > 0)
                                    {
                                        MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                    }
                                    else
                                    {
                                        my_data = newRow[variableID].ToString();
                                        my_data += ofp.GetLuhnCheckDigit(newRow[variableID].ToString());
                                    }
                                }
                                break;

                            case "KCV_AES":
                                if (i == 0)
                                {
                                    my_data = var_Value;
                                }
                                else
                                {
                                    if (varCount > 0)
                                    {
                                        MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                    }
                                    else
                                    {
                                        //my_data = padding((Int64.Parse(first_icicid) + i).ToString());
                                        my_data = ofp.CalculateKCV(newRow[variableID].ToString(), "AES");
                                    }
                                }
                                break;

                            case "KCV_DES":
                                if (i == 0)
                                {
                                    my_data = var_Value;
                                }
                                else
                                {
                                    if (varCount > 0)
                                    {
                                        MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                    }
                                    else
                                    {
                                        //my_data = padding((Int64.Parse(first_icicid) + i).ToString());
                                        my_data = ofp.CalculateKCV(newRow[variableID].ToString(), "DES");
                                    }
                                }
                                break;

                            case "MSISDN_F":
                                if (varCount > 0)
                                {
                                    MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                }
                                else
                                {
                                    my_data = newRow[variableID].ToString();
                                    my_data = ofp.MSISDN_F(my_data);
                                }
                                break;

                            case "NS":
                                if (varCount > 0)
                                {
                                    MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                }
                                else
                                {
                                    my_data = newRow[variableID].ToString();
                                    my_data = ofp.nibble_swapped(my_data);
                                }
                                break;

                            case "KI_AES_128":
                                if (varCount == 1)
                                {
                                    string[] varIDs = variableID.Split(',');
                                    my_data = ofp.AES_ENCYPRTION(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                }
                                //else if (varCount == 0)
                                //{
                                //    my_data = ofp.AES_ENCYPRTION(newRow[variableID].ToString(), "db4389530b991a10b557246db732cd9a");

                                //}
                                else
                                {
                                    MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                }
                                break;
                            
                            
                            case "Single_Des":
                                if (varCount == 1)
                                {
                                    string[] varIDs = variableID.Split(',');
                                    my_data = ofp.Encrypt_SingleDES(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                }
                                //else if (varCount == 0)
                                //{
                                //    my_data = ofp.Encrypt_SingleDES(newRow[variableID].ToString(), "db4389530b991a10b557246db732cd9a");

                                //}
                                else
                                {
                                    MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                }
                                break;
                            //case "KI_AES_128_2":
                            //    if (varCount == 1)
                            //    {
                            //        string[] varIDs = variableID.Split(',');
                            //        my_data = ofp.AES_ENCYPRTION(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                            //    }
                            //    else if (varCount == 0)
                            //    {
                            //        my_data = ofp.AES_ENCYPRTION(newRow[variableID].ToString(), "E8F8D8DCAA7DF2D372B0446C196E580C");

                            //    }
                            //    else
                            //    {
                            //        MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                            //    }
                            //    break;

                            case "AES_128_1":
                                if (varCount > 0)
                                {
                                    MessageBox.Show($"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                }
                                else
                                {
                                    my_data = ""; //ofp.Aes_128(newRow[variableID].ToString());
                                }
                                break;
                            case "AES_128":
                                if (varCount == 1)
                                {
                                    string[] varIDs = variableID.Split(',');
                                    my_data = OPC_GEN.opc(newRow[varIDs[1]].ToString(), newRow[varIDs[0]].ToString());
                                }
                                else if (varCount == 0)
                                {
                                    my_data = OPC_GEN.opc("436F6C6F72504C4153541220184E6F69", newRow[variableID].ToString());
                                }
                                else
                                {
                                    MessageBox.Show($"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value");
                                }
                                break;
                        }
                        newRow[var_ID] = my_data;
                    }

                }

                Process_data.Rows.Add(newRow);
                Console.WriteLine($"Processed Row : " + i);
            }
            try
            {
                con.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                {

                    bulkCopy.DestinationTableName = "DataGenProcessDataRecord";
                    foreach (DataColumn col in Process_data.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName.ToString(), col.ColumnName.ToString().Trim());
                    }
                    bulkCopy.BulkCopyTimeout = 1200;
                    bulkCopy.WriteToServer(Process_data);
                    ofp.UpdateInputFileSatus("Input_File", lastInsertedId);
                    Process_data.Clear();
                }
                con.Close();
                stopwatch.Stop();

                return 1;
            }
            catch (Exception ex)
            {
                using (SqlConnection del_con = new SqlConnection(connectionString))
                {
                    del_con.Open();
                    SqlDataReader reader = null;
                    using (SqlCommand cmd = new SqlCommand($"Delete FROM [dbo].[DataGenProcessDataRecord] WHERE DataGenProcessHDID={lastInsertedId};)", con))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                    del_con.Close();
                }
                return 0;
            }
        }

        private void cbxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbxCustomer.SelectedIndex>0)
            {
                var circulList = CommonClass.GetCircle(Convert.ToInt32(cbxCustomer.SelectedValue));
                if (circulList != null && circulList.Count > 0)
                {
                    circulList.Insert(0, new Circle
                    {
                        CircleID = 0,
                        CustomerName = "----All Customers----",
                        CircleName = "----All Circles----",
                        Status = "Active",
                    });
                    cbxCircle.DataSource = circulList;
                    cbxCircle.DisplayMember = "CircleName";
                    cbxCircle.ValueMember = "CircleID";

                }
                DataGenProcessList(cbxCustomer.Text, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            else
            {
                var circulList = CommonClass.GetCircle(1);
                if (circulList != null && circulList.Count > 0)
                {
                    circulList.Insert(0, new Circle
                    {
                        CircleID = 0,
                        CustomerName = "----All Customers----",
                        CircleName = "----All Circles----",
                        Status = "Active",
                    });
                    cbxCircle.DataSource = circulList;
                    cbxCircle.DisplayMember = "CircleName";
                    cbxCircle.ValueMember = "CircleID";

                }
                DataGenProcessList(cbxCustomer.Text, cbxCircle.Text, string.Empty, string.Empty, string.Empty);
            }
            
        }

        private void cbxCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCircle.SelectedIndex > 0 && cbxCustomer.SelectedIndex > 0)
            {
                DataGenProcessList(cbxCustomer.Text, cbxCircle.Text, string.Empty, string.Empty, string.Empty);
            }
            else if(cbxCircle.SelectedIndex < 0)
            {
                DataGenProcessList(string.Empty, cbxCircle.Text, string.Empty, string.Empty, string.Empty);
            }
        }

        private void txtFilepath_TextChanged(object sender, EventArgs e)
        {
            DataGenProcessList(string.Empty, string.Empty, txtFilepath.Text, string.Empty, string.Empty);
        }

        private void pbFromCalander_Click(object sender, EventArgs e)
        {
            if (toCalendar.Visible)
                toCalendar.Visible = false;

            if (fromCalendar.Visible)
                fromCalendar.Visible = false;
            else
                fromCalendar.Visible = true;
        }

        private void pbToCalander_Click(object sender, EventArgs e)
        {
            if (fromCalendar.Visible)
                fromCalendar.Visible = false;

            if (toCalendar.Visible)
                toCalendar.Visible = false;
            else
                toCalendar.Visible = true;
        }

        private void fromCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            txtFromDate.Text = fromCalendar.SelectionRange.Start.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(txtFromDate.Text))
                fromCalendar.Visible = false;

            //DataGenProcessList(string.Empty, string.Empty, string.Empty, txtFromDate.Text, txtToDate.Text);
        }

        private void toCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            txtToDate.Text = toCalendar.SelectionRange.Start.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(txtToDate.Text))
                toCalendar.Visible = false;

            if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
                DataGenProcessList(string.Empty, string.Empty, string.Empty, txtFromDate.Text, txtToDate.Text);
        }

        private void pbReresh_Click(object sender, EventArgs e)
        {
            var customerList = CommonClass.GetCustomer();
            var circulList = CommonClass.GetCircle(1);
            if (customerList != null && customerList.Count > 0)
            {
                customerList.Insert(0, new CustomerDetails
                {
                    CustomerName = "----All Customers----",
                    CustomerID = 0,
                });
                cbxCustomer.DataSource = customerList;
                cbxCustomer.DisplayMember = "CustomerName";
                cbxCustomer.ValueMember = "CustomerID";

            }

            if (circulList != null && circulList.Count > 0)
            {
                circulList.Insert(0, new Circle
                {
                    CircleID = 0,
                    CustomerName = "----All Customers----",
                    CircleName = "----All Circles----",
                    Status = "Active",
                });
                cbxCircle.DataSource = circulList;
                cbxCircle.DisplayMember = "CircleName";
                cbxCircle.ValueMember = "CircleID";

            }
            dgvBriefList.ReadOnly = true;
            DataGenProcessList(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            DataGridViewButtonColumn viewbutton = new DataGridViewButtonColumn();
            viewbutton.FlatStyle = FlatStyle.Popup;
            viewbutton.HeaderText = "View";
            viewbutton.Text = "View";
            viewbutton.UseColumnTextForButtonValue = true;
            viewbutton.Name = "view";
            viewbutton.Width = 60;

            if (dgvBriefList.Columns.Contains(viewbutton.Name = "View"))
            {

            }
            else
            {
                dgvBriefList.Columns.Add(viewbutton);
            }
            fromCalendar.Visible = false;
            toCalendar.Visible = false;
            txtFilepath.Clear();
            txtFromDate.Clear();
            txtToDate.Clear();
        }

        private void txtFilepath_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
