using DG_Tool.WinForms.InputFile;
using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace DG_Tool
{
    public partial class Input_File_Creation : Form
    {
        string ConStr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        int varcounter = 0;

        public Input_File_Creation()
        {
            InitializeComponent();

            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
        }

        private void Input_File_Creation_Load(object sender, EventArgs e)
        {
            getLastVariable();

            //  string inputfilepath = "X:\\Code\\Rupesh\\INPUT_FILES\\Test_Input_File_Postpaid_DL.txt";

            //  verticalTextBox1.Text = inputfilepath;

            //  string text = File.ReadAllText(inputfilepath);
            //  richTextBox1.Text = text;

            label18.Hide();
            comboBox4.Hide();
            label4.Hide();
            textBox4.Hide();

            label3.Hide();

            GetAlgorithms();

            button3.Enabled = false;

            //label13.Text = "Airtel";
            //label14.Text = "North";    
            //label16.Text = "Airtel_North_32k_Java_Sim";
            //label19.Text = "Input_File_Postpaid_DL";
            //lblProfileID.Text = CreateInputTemplate.CustomerProfileID;
            label13.Text = CreateInputTemplate.CustomerName;
            //label14.Text = CreateInputTemplate.CircleName;
            label16.Text = CreateInputTemplate.CustomerProfileName;
            label19.Text = CreateInputTemplate.InputFileName;

            comboBox2.SelectedIndex = 2;
            comboBox2.Enabled = false;

            //if (label19.Text== "Input_File_Postpaid_DL")
            //{

            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "File Browser";
            fdlg.InitialDirectory = @"d:\";
            fdlg.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                verticalTextBox1.Text = fdlg.FileName;

                string text = File.ReadAllText(fdlg.FileName);
                richTextBox1.Text = text;

            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label3.Hide();

            int firstcharindex = richTextBox1.GetFirstCharIndexOfCurrentLine();

            int currentline = richTextBox1.GetLineFromCharIndex(firstcharindex);

            string currentlinetext = richTextBox1.Lines[currentline];

            // Get the line.
            int index = richTextBox1.SelectionStart;
            int line = richTextBox1.GetLineFromCharIndex(index);

            // Get the column.
            int firstChar = richTextBox1.GetFirstCharIndexFromLine(line);
            int column = index - firstChar;

            // label1.Text = "";

            textBox4.Text = textBox2.Text;

            // label1.Text += "First" + firstcharindex + "\n";
            //  label1.Text += "Line " + (currentline + 1) + "\n";
            textBox5.Text = (currentline + 1).ToString();
            textBox6.Text = column.ToString();

            textBox7.Text = (column + textBox2.Text.Length).ToString();
            textBox8.Text = textBox2.Text.Length.ToString();
            // label1.Text += "Length " + textBox2.Text.Length;
            // label1.Text += "Length " + currentlinetext.Length;

            //string authors = textBox2.Text.ToString().Trim();
            //// Split authors separated by a comma followed by space  
            //string[] authorsList = authors.Split(':');

            //textBox3.Text = authorsList[0];
            //textBox4.Text = authorsList[1];

            //textBox8.Text = textBox4.Text.Length.ToString();

            //int txtread = textBox2.Text.LastIndexOf(":");

            //// MessageBox.Show(txtread.ToString());

            //textBox6.Text = (txtread + 1).ToString();
            //textBox7.Text = textBox2.Text.Length.ToString();

            //richTextBox1.SelectionBackColor = Color.LightGray;

            ////richTextBox1.Select(firstcharindex, currentlinetext.Length);
            //richTextBox1.Select(Int32.Parse(textBox6.Text.ToString()),Int32.Parse(textBox8.Text.ToString()));
            //richTextBox1.SelectionBackColor = Color.Yellow;

            RichTextSearchAndH(textBox2.Text);

        }

        private void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string readtext = e.Data.GetData(DataFormats.Text).ToString();
            txt.Text = readtext;
        }

        private void textBox2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int firstcharindex = richTextBox1.GetFirstCharIndexOfCurrentLine();

            int currentline = richTextBox1.GetLineFromCharIndex(firstcharindex);

            string currentlinetext = richTextBox1.Lines[currentline];

            richTextBox1.DoDragDrop(richTextBox1.SelectedText.Trim(), DragDropEffects.Copy);

            // richTextBox1.DoDragDrop(currentlinetext.Trim(), DragDropEffects.Copy);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                label3.Hide();

                if (comboBox1.Text.Equals("--Select--"))
                {
                    MessageBox.Show("Please Select VarValType!");
                }
                else if (comboBox1.Text.Equals("Text"))
                {
                    if (textBox4.Text == "")
                    {
                        MessageBox.Show("Please Enter Variable Description!");
                    }
                    else
                    {
                        button3.Enabled = true;

                        string varid = VarNameGenerate();

                        dataGridView1.ColumnCount = 10;

                        dataGridView1.Columns[0].Name = "InPutDataTemplateID";
                        //dataGridView1.Columns[1].Name = "VarName";
                        dataGridView1.Columns[1].Name = "VarCode";
                        dataGridView1.Columns[2].Name = "VarValue";
                        dataGridView1.Columns[3].Name = "VarDes";
                        dataGridView1.Columns[4].Name = "VarValType";
                        dataGridView1.Columns[5].Name = "VarText";
                        dataGridView1.Columns[6].Name = "LineNumber";
                        dataGridView1.Columns[7].Name = "PositionFrom";
                        dataGridView1.Columns[8].Name = "PositionTo";
                        dataGridView1.Columns[9].Name = "Len";

                        dataGridView1.Rows.Add(varcounter, varid, textBox2.Text, textBox4.Text, GetFirstChar(comboBox1.Text), comboBox2.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text);
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                        textBox2.Clear();
                        textBox5.Clear();
                        textBox6.Clear();
                        textBox7.Clear();
                        textBox8.Clear();
                        comboBox1.SelectedIndex = 0;

                        richTextBox1.SelectAll();
                        richTextBox1.SelectionBackColor = Color.LightGray;
                        richTextBox1.DeselectAll();

                    }
                }
                else
                {
                    if (comboBox4.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please Select Constant Variable!");
                    }
                    else
                    {
                        button3.Enabled = true;

                        string varid = VarNameGenerate();

                        dataGridView1.ColumnCount = 10;

                        dataGridView1.Columns[0].Name = "InPutDataTemplateID";
                        dataGridView1.Columns[1].Name = "VarName";
                        dataGridView1.Columns[2].Name = "VarValue";
                        dataGridView1.Columns[3].Name = "VarDes";
                        dataGridView1.Columns[4].Name = "VarValType";
                        dataGridView1.Columns[5].Name = "VarText";
                        dataGridView1.Columns[6].Name = "LineNumber";
                        dataGridView1.Columns[7].Name = "PositionFrom";
                        dataGridView1.Columns[8].Name = "PositionTo";
                        dataGridView1.Columns[9].Name = "Len";

                        dataGridView1.Rows.Add(varcounter, varid, "", comboBox4.Text, GetFirstChar(comboBox1.Text), comboBox2.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text);
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                        textBox2.Clear();
                        textBox5.Clear();
                        textBox6.Clear();
                        textBox7.Clear();
                        textBox8.Clear();
                        comboBox1.SelectedIndex = 0;

                        richTextBox1.SelectAll();
                        richTextBox1.SelectionBackColor = Color.LightGray;
                        richTextBox1.DeselectAll();
                    }

                }
            }
            else
            {
                label3.Show();
            }

        }

        public string VarNameGenerate()
        {
            string charvar = "V";

            varcounter++;

            string sendval = charvar + varcounter.ToString("000");

            return sendval;

        }

        public void RichTextSearchAndH(string dataTXT)
        {
            string[] words = dataTXT.Split(',');
            foreach (string word in words)
            {
                int startindex = 0;
                while (startindex < richTextBox1.TextLength)
                {
                    int wordstartIndex = richTextBox1.Find(word, startindex, RichTextBoxFinds.None);
                    if (wordstartIndex != -1)
                    {
                        richTextBox1.SelectionStart = wordstartIndex;
                        richTextBox1.SelectionLength = word.Length;
                        richTextBox1.SelectionBackColor = Color.Yellow;
                    }
                    else
                        break;
                    startindex += wordstartIndex + word.Length;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    using (SqlCommand com = new SqlCommand("insert into InPutDataTemplate values (@InPutDataTemplateID, @CustID, @ProfileID, @VarName, @VarValue, @VarDes, @VarValType, @VarText, @AlgoID, @AlgoName, @FileID, @LineNumber, @VarWay, @PositionFrom, @PositionTo, @Len, @Tag )", con))
                    {
                        com.Parameters.AddWithValue("@InPutDataTemplateID", row.Cells["InPutDataTemplateID"].Value);
                        com.Parameters.AddWithValue("@CustID", CreateInputTemplate.CustomerID);
                        com.Parameters.AddWithValue("@ProfileID", CreateInputTemplate.CustomerProfileID);
                        com.Parameters.AddWithValue("@VarName", row.Cells["VarName"].Value);
                        com.Parameters.AddWithValue("@VarValue", row.Cells["VarValue"].Value);
                        com.Parameters.AddWithValue("@VarDes", row.Cells["VarDes"].Value);
                        com.Parameters.AddWithValue("@VarValType", row.Cells["VarValType"].Value);
                        com.Parameters.AddWithValue("@VarText", row.Cells["VarText"].Value);
                        com.Parameters.AddWithValue("@AlgoID", "");
                        com.Parameters.AddWithValue("@AlgoName", "");
                        com.Parameters.AddWithValue("@FileID", CreateInputTemplate.InputFileID);
                        com.Parameters.AddWithValue("@LineNumber", row.Cells["LineNumber"].Value);
                        com.Parameters.AddWithValue("@VarWay", "");
                        com.Parameters.AddWithValue("@PositionFrom", row.Cells["PositionFrom"].Value);
                        com.Parameters.AddWithValue("@PositionTo", row.Cells["PositionTo"].Value);
                        com.Parameters.AddWithValue("@Len", row.Cells["Len"].Value);
                        com.Parameters.AddWithValue("@Tag", "");

                        try
                        {
                            con.Open();
                            com.ExecuteNonQuery();
                        }
                        catch (Exception exe)
                        {

                        }
                    }
                }
                // MessageBox.Show(row.Cells[5].Value.ToString());
            }
            MessageBox.Show("Data Saved Successfully!");

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            button3.Enabled = false;
        }

        private void GetAlgorithms()
        {
            //usp_getRoles
            SqlDataAdapter da = new SqlDataAdapter();

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select FieldID,FieldName from [dbo].[ConstantMaster] where FieldType='C'", con))
                {
                    // cmd.CommandType = CommandType.StoredProcedure;

                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    DataRow row = dt.NewRow();
                    row[0] = 0;
                    row[1] = " ----Select---- ";
                    dt.Rows.InsertAt(row, 0);

                    comboBox4.DataSource = dt;
                    comboBox4.DisplayMember = "FieldName";
                    comboBox4.ValueMember = "FieldID";
                    comboBox4.SelectedIndex = 0;      //by defaultuser
                }
            }
        }

        private void getLastVariable()
        {
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand($"SELECT TOP 1 VarName FROM InPutDataTemplate WHERE CustID={CreateInputTemplate.CustomerID} and ProfileID={CreateInputTemplate.CustomerProfileID} ORDER BY InPutDataTemplateID DESC", con)) 
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals("--Select--"))
            {
                label18.Hide();
                comboBox4.Hide();
                label4.Hide();
                textBox4.Hide();
            }
            else if (comboBox1.Text.Equals("Text"))
            {
                label18.Hide();
                comboBox4.Hide();
                label4.Show();
                textBox4.Show();
            }
            else
            {
                label18.Show();
                comboBox4.Show();
                label4.Hide();
                textBox4.Hide();
                comboBox4.SelectedIndex = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Input_Fix_Header input_Fix_Header = new Input_Fix_Header();
            input_Fix_Header.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Create_Algo create_Algo = new Create_Algo();
            create_Algo.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Input_File_View input_File_View = new Input_File_View();
            input_File_View.ShowDialog();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name.Equals("tabPage2"))
            {
                Input_Fix_Header input_Fix_Header = new Input_Fix_Header();
                input_Fix_Header.TopLevel = false;
                input_Fix_Header.Dock = DockStyle.Fill;
                input_Fix_Header.FormBorderStyle = FormBorderStyle.None;
                tabPage2.Controls.Add(input_Fix_Header);
                input_Fix_Header.Show();
            }
            if (tabControl1.SelectedTab.Name.Equals("tabPage3"))
            {
                Create_Algo input_Fix_Header = new Create_Algo();
                input_Fix_Header.TopLevel = false;
                input_Fix_Header.Dock = DockStyle.Fill;
                input_Fix_Header.FormBorderStyle = FormBorderStyle.None;
                tabPage3.Controls.Add(input_Fix_Header);
                input_Fix_Header.Show();
            }
            if (tabControl1.SelectedTab.Name.Equals("tabPage4"))
            {
                Input_File_View input_Fix_Header = new Input_File_View();
                input_Fix_Header.TopLevel = false;
                input_Fix_Header.Dock = DockStyle.Fill;
                input_Fix_Header.FormBorderStyle = FormBorderStyle.None;
                tabPage4.Controls.Add(input_Fix_Header);
                input_Fix_Header.Show();
            }
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font fntTab;
            Brush bshBack;
            Brush bshFore;
            if (e.Index == this.tabControl1.SelectedIndex)
            {
                fntTab = new Font(e.Font, FontStyle.Regular);
                bshBack = Brushes.BlueViolet;
                bshFore = Brushes.White;
            }
            else
            {
                fntTab = e.Font;
                bshBack = new SolidBrush(Color.IndianRed);
                bshFore = new SolidBrush(Color.White);
            }
            string tabName = this.tabControl1.TabPages[e.Index].Text;
            StringFormat sftTab = new StringFormat(StringFormatFlags.NoClip);
            sftTab.Alignment = StringAlignment.Center;
            sftTab.LineAlignment = StringAlignment.Center;
            e.Graphics.FillRectangle(bshBack, e.Bounds);
            Rectangle recTab = e.Bounds;
            recTab = new Rectangle(recTab.X, recTab.Y + 4, recTab.Width, recTab.Height - 4);
            e.Graphics.DrawString(tabName, fntTab, bshFore, recTab, sftTab);


            //Graphics g = e.Graphics;
            //Pen p = new Pen(Color.Blue, 4);
            //g.DrawRectangle(p, this.tabPage1.Bounds);
        }

        public string GetFirstChar(string data)
        {
            return data.Substring(0, 1);
        }
    }
}
