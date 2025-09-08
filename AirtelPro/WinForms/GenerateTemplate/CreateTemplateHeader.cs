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
    public partial class CreateTemplateHeader : Form
    {

        int tbcount = 0;
        int linecount = 0;

        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public CreateTemplateHeader()
        {
            InitializeComponent();
        }

        private void CreateTemplateHeader_Load(object sender, EventArgs e)
        {  
            lblCustomer.Text = CreateTemplate.CustomerName;
            lblCircle.Text = CreateTemplate.CircleName;
            lblCustomerProfile.Text = CreateTemplate.CustomerProfileName;
            lblOutputTemplate.Text = ProcessData.OutTempName;


            label11.Text = "";
            button2.Hide();
            button3.Hide();
            button4.Hide();
            button5.Hide();
            //  comboBox1.Hide();
            //  dataGridView1.Hide();
            AddVariableFileLabel();
            AddFileLabel();
            AddAlgorithmLabel();
            AddSperetor();
            AddDataFileLabel();
            AddSperetorAllTab(tabPage1);
            AddSperetorAllTab(tabPage2);
            AddSperetorAllTab(tabPage3);
            AddSperetorAllTab(tabPage5);
            linecount = 1;
            label11.Text = "Line " + linecount;
            button1.Hide();
            AddLine(10, 15);
            tbcount = 10;
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;

            //DisplayDataGridLines();


            //comboBox1.SelectedIndex = 0;
            //comboBox1.Enabled = false;
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            string s = (sender as Label).Text;
            lblCustomer.DoDragDrop(s, DragDropEffects.Copy);
        }

        private void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string readtext = e.Data.GetData(DataFormats.Text).ToString();

            string res = readtext.Substring(readtext.Length - 4);

            // int txtread = readtext.LastIndexOf(",");

            //  MessageBox.Show(res);

            // txt.Text = readtext.Substring(0,txtread);
            txt.Text = res;
        }

        private void text_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            txt.BackColor = Color.LightYellow;

            //int myArray= txt.Location.X;
            //int myArray1 = txt.Location.Y;
            //MessageBox.Show(myArray.ToString() +myArray1.ToString());

            int loc = txt.Location.X;
            int loc1 = txt.Location.Y;

            string s = txt.Text;
            int n = 3;
            if (s == "")
            {

            }
            else
            {
                s = s.Remove(s.Length - n);
            }

            // s = s.Remove(s.Length - n);
            // Console.WriteLine(s);

            if (s.Equals("V"))
            {
                Label a = new Label();
                a.Text = GetVarValue(txt.Text);
                //a.BringToFront();
                // a.Size = new System.Drawing.Size(60, 20);
                a.AutoSize = true;
                a.Name = "1" + txt.Name;
                a.Location = new System.Drawing.Point(loc, loc1 + 25);
                groupBox2.Controls.Add(a);
                groupBox2.Controls.Remove(groupBox2.Controls.Find(a.Name, true)[0]);
                groupBox2.Controls.Add(a);
                groupBox2.Show();

            }
            else if (s.Equals("S"))
            {
                Label a = new Label();
                a.Text = GetSepValue(txt.Text);
                //a.BringToFront();
                // a.Size = new System.Drawing.Size(60, 20);
                a.AutoSize = true;
                a.UseMnemonic = false;
                a.Name = "1" + txt.Name;
                a.Location = new System.Drawing.Point(loc, loc1 + 25);
                groupBox2.Controls.Add(a);
                groupBox2.Controls.Remove(groupBox2.Controls.Find(a.Name, true)[0]);
                groupBox2.Controls.Add(a);
                groupBox2.Show();

            }

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

        private void AddLine(int len, int ntextY)
        {
            int ntextX = 100;
            for (int i = 0; i < len; i++)
            {
                TextBox sp = new TextBox();
                // sp.Text = "SP00" + (i + 1).ToString();
                sp.Size = new System.Drawing.Size(110, 30);
                sp.Name = "tex" + (i + 1).ToString();
                sp.AllowDrop = true;
                sp.Location = new System.Drawing.Point(ntextX, ntextY);
                groupBox2.Controls.Add(sp);
                sp.TextChanged += new System.EventHandler(text_TextChanged);
                sp.DragDrop += new System.Windows.Forms.DragEventHandler(textBox2_DragDrop);
                sp.DragEnter += new System.Windows.Forms.DragEventHandler(textBox2_DragEnter);
                groupBox2.Controls.Remove(sp);
                groupBox2.Controls.Add(sp);
                groupBox2.Show();
                //  ntextY += 50;
                ntextX += 115;
            }
        }

        private void AddPathLine(int ntextY)
        {
            int ntextX = 100;

            TextBox sp = new TextBox();
            // sp.Text = "SP00" + (i + 1).ToString();
            sp.Size = new System.Drawing.Size(500, 30);
            sp.Name = "filepath1";
            sp.AllowDrop = true;
            sp.Location = new System.Drawing.Point(ntextX, ntextY);
            groupBox2.Controls.Add(sp);
            groupBox2.Controls.Remove(sp);
            //sp.TextChanged += new System.EventHandler(text_TextChanged);
            //sp.DragDrop += new System.Windows.Forms.DragEventHandler(textBox2_DragDrop);
            //sp.DragEnter += new System.Windows.Forms.DragEventHandler(textBox2_DragEnter);
            groupBox2.Controls.Add(sp);
            groupBox2.Show();
            //  ntextY += 50;
            // ntextX += 115;
        }

        private void AddVariableFileLabel()
        {
            int pointX = 20, pointY = 15;

            using (SqlConnection con4 = new SqlConnection(connectionString))
            {
                SqlCommand com4 = new SqlCommand("select rtrim(VarName) as VarName,rtrim(VarDes) as VarDes from InPutDataTemplate where VarText='FL' and VarValType in ('N' ,'V')", con4);
                con4.Open();
                SqlDataReader sqlDataReader4 = com4.ExecuteReader();

                int mynum = 1;

                while (sqlDataReader4.Read())
                {
                    Label a = new Label();
                    a.Text = sqlDataReader4.GetString(1) + "\n," + sqlDataReader4.GetString(0);
                    a.Size = new System.Drawing.Size(185, 28);
                    a.BackColor = System.Drawing.Color.Coral;
                    a.Padding = new Padding(3);
                    a.TextAlign = ContentAlignment.MiddleCenter;
                    // a.AutoSize = true;
                    // a.Name = "label" + (j + 1);
                    a.Location = new System.Drawing.Point(pointX, pointY);
                    tabPage1.Controls.Add(a);
                    a.MouseDown += new System.Windows.Forms.MouseEventHandler(label1_MouseDown);
                    tabPage1.Show();
                    // pointY += 50;
                    pointX += 190;
                    //  count++;
                    //  MessageBox.Show(count.ToString());

                    if (mynum == 6)
                    {
                        pointY += 30;
                        pointX = 20;
                        mynum = 0;

                        // Label b = new Label();
                        // b.Text = "Hello new line data";
                        // a.Size = new System.Drawing.Size(49, 20);
                        //  b.AutoSize = true;
                        //  a.Name = "label" + (j + 1);
                        //a.Location = new System.Drawing.Point(npointX, npointY);
                        //tabPage1.Controls.Add(a);
                        //a.MouseDown += new System.Windows.Forms.MouseEventHandler(label1_MouseDown);
                        //tabPage1.Show();
                        //// pointY += 50;
                        //npointX += 110;
                        ////  count++;


                    }

                    mynum++;
                }
            }
        }

        private void AddFileLabel()
        {
            int pointX = 20, pointY = 15;

            using (SqlConnection con4 = new SqlConnection(connectionString))
            {
                SqlCommand com4 = new SqlCommand("select rtrim(VarName) as VarName,rtrim(VarDes) as VarDes from InPutDataTemplate where VarText='TX' and VarValType='T'", con4);
                con4.Open();
                SqlDataReader sqlDataReader4 = com4.ExecuteReader();

                int mynum = 1;

                while (sqlDataReader4.Read())
                {
                    Label a = new Label();
                    a.Text = sqlDataReader4.GetString(1) + "\n," + sqlDataReader4.GetString(0);
                    a.Size = new System.Drawing.Size(185, 28);
                    a.BackColor = System.Drawing.Color.Coral;
                    a.Padding = new Padding(3);
                    a.TextAlign = ContentAlignment.MiddleCenter;
                    // a.Size = new System.Drawing.Size(60, 20);
                    //a.AutoSize = true;
                    // a.Name = "label" + (j + 1);
                    a.Location = new System.Drawing.Point(pointX, pointY);
                    tabPage5.Controls.Add(a);
                    a.MouseDown += new System.Windows.Forms.MouseEventHandler(label1_MouseDown);
                    tabPage5.Show();
                    // pointY += 50;
                    pointX += 190;
                    //  count++;
                    //  MessageBox.Show(count.ToString());

                    if (mynum == 6)
                    {
                        pointY += 30;
                        pointX = 20;
                        mynum = 0;

                        // Label b = new Label();
                        // b.Text = "Hello new line data";
                        // a.Size = new System.Drawing.Size(49, 20);
                        //  b.AutoSize = true;
                        //  a.Name = "label" + (j + 1);
                        //a.Location = new System.Drawing.Point(npointX, npointY);
                        //tabPage1.Controls.Add(a);
                        //a.MouseDown += new System.Windows.Forms.MouseEventHandler(label1_MouseDown);
                        //tabPage1.Show();
                        //// pointY += 50;
                        //npointX += 110;
                        ////  count++;


                    }

                    mynum++;
                }
            }
        }

        private void AddDataFileLabel()
        {
            int pointX = 20, pointY = 15;

            using (SqlConnection con4 = new SqlConnection(connectionString))
            {
                SqlCommand com4 = new SqlCommand("select rtrim(VarName) as VarName,rtrim(VarDes) as VarDes from InPutDataTemplate where VarText in('TX','FL') and VarValType='T'", con4);
                con4.Open();
                SqlDataReader sqlDataReader4 = com4.ExecuteReader();

                int mynum = 1;

                while (sqlDataReader4.Read())
                {
                    Label a = new Label();
                    a.Text = sqlDataReader4.GetString(1) + "\n," + sqlDataReader4.GetString(0);
                    a.Size = new System.Drawing.Size(185, 28);
                    a.BackColor = System.Drawing.Color.Coral;
                    a.Padding = new Padding(3);
                    a.TextAlign = ContentAlignment.MiddleCenter;
                    // a.Size = new System.Drawing.Size(60, 20);
                    //a.AutoSize = true;
                    // a.Name = "label" + (j + 1);
                    a.Location = new System.Drawing.Point(pointX, pointY);
                    tabPage2.Controls.Add(a);
                    a.MouseDown += new System.Windows.Forms.MouseEventHandler(label1_MouseDown);
                    tabPage2.Show();
                    // pointY += 50;
                    pointX += 190;
                    //  count++;
                    //  MessageBox.Show(count.ToString());

                    if (mynum == 6)
                    {
                        pointY += 30;
                        pointX = 20;
                        mynum = 0;

                        // Label b = new Label();
                        // b.Text = "Hello new line data";
                        // a.Size = new System.Drawing.Size(49, 20);
                        //  b.AutoSize = true;
                        //  a.Name = "label" + (j + 1);
                        //a.Location = new System.Drawing.Point(npointX, npointY);
                        //tabPage1.Controls.Add(a);
                        //a.MouseDown += new System.Windows.Forms.MouseEventHandler(label1_MouseDown);
                        //tabPage1.Show();
                        //// pointY += 50;
                        //npointX += 110;
                        ////  count++;


                    }

                    mynum++;
                }
            }
        }

        private void AddAlgorithmLabel()
        {
            int pointX = 20, pointY = 15;

            using (SqlConnection con4 = new SqlConnection(connectionString))
            {
               // SqlCommand com4 = new SqlCommand("select rtrim(VarName) as VarName,rtrim(AlgoName) as VarDes from InPutDataTemplate where VarText='AL'", con4);
                SqlCommand com4 = new SqlCommand("select rtrim(VarName) as VarName,rtrim(InputTemp.AlgoName) as VarDes,rtrim(InputTemp.VarDes) as VarDes,rtrim(AlgoDes) as VarTooltip from InPutDataTemplate InputTemp inner join [dbo].[AlgoMaster] am on InputTemp.AlgoID=am.AlgoID where InputTemp.VarText='AL'", con4);
                con4.Open();
                SqlDataReader sqlDataReader4 = com4.ExecuteReader();

                int mynum = 1;

                ToolTip toolTip = new ToolTip();
                toolTip.ToolTipIcon = ToolTipIcon.Info;
                toolTip.ToolTipTitle = "Important";
                toolTip.IsBalloon = true;

                while (sqlDataReader4.Read())
                {
                    Label a = new Label();
                    a.Text = sqlDataReader4.GetString(2) + "\n," + sqlDataReader4.GetString(0);
                    toolTip.SetToolTip(a, sqlDataReader4.GetString(3));
                    a.Size = new System.Drawing.Size(185, 28);
                    a.BackColor = System.Drawing.Color.Coral;
                    a.Padding = new Padding(3);
                    a.TextAlign = ContentAlignment.MiddleCenter;
                    // a.Size = new System.Drawing.Size(60, 20);
                    // a.AutoSize = true;
                    // a.Name = "label" + (j + 1);
                    a.Location = new System.Drawing.Point(pointX, pointY);
                    tabPage3.Controls.Add(a);
                    a.MouseDown += new System.Windows.Forms.MouseEventHandler(label1_MouseDown);
                    tabPage3.Show();
                    // pointY += 50;
                    pointX += 190;
                    //  count++;
                    //  MessageBox.Show(count.ToString());

                    if (mynum == 6)
                    {
                        pointY += 30;
                        pointX = 20;
                        mynum = 0;

                        // Label b = new Label();
                        // b.Text = "Hello new line data";
                        // a.Size = new System.Drawing.Size(49, 20);
                        //  b.AutoSize = true;
                        //  a.Name = "label" + (j + 1);
                        //a.Location = new System.Drawing.Point(npointX, npointY);
                        //tabPage1.Controls.Add(a);
                        //a.MouseDown += new System.Windows.Forms.MouseEventHandler(label1_MouseDown);
                        //tabPage1.Show();
                        //// pointY += 50;
                        //npointX += 110;
                        ////  count++;


                    }

                    mynum++;
                }
            }
        }

        private void AddSperetor()
        {
            int pointX = 20, pointY = 15;

            using (SqlConnection con1 = new SqlConnection(connectionString))
            {
                SqlCommand com1 = new SqlCommand("select SepID, rtrim(Seperator) as Seperator from SeperatorMaster", con1);
                con1.Open();
                SqlDataReader sqlDataReader = com1.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    // string str = sqlDataReader.GetString(0);

                    //  MessageBox.Show(str);

                    Label a = new Label();
                    a.Text = sqlDataReader.GetString(1) + "," + sqlDataReader.GetString(0);
                    a.Size = new System.Drawing.Size(18, 22);
                    // a.BackColor = System.Drawing.Color.BlueViolet; 
                    a.Font = new System.Drawing.Font("Arial", 15, FontStyle.Bold);
                    a.UseMnemonic = false;
                    // a.Name = sqlDataReader.GetString(0);
                    a.Location = new System.Drawing.Point(pointX, pointY);
                    tabPage3.Controls.Add(a);
                    a.MouseDown += new System.Windows.Forms.MouseEventHandler(label1_MouseDown);
                    tabPage3.Show();
                    pointX += 50;

                }
            }
        }

        private void AddSperetorAllTab(Control control)
        {
            int pointX = 1180, pointY = 45;

            using (SqlConnection con1 = new SqlConnection(connectionString))
            {
                SqlCommand com1 = new SqlCommand("select SepID, rtrim(Seperator) as Seperator from SeperatorMaster", con1);
                con1.Open();
                SqlDataReader sqlDataReader = com1.ExecuteReader();

                int mynumtext = 1;

                while (sqlDataReader.Read())
                {
                    // string str = sqlDataReader.GetString(0);

                    //  MessageBox.Show(str);

                    Label a = new Label();
                    a.Text = sqlDataReader.GetString(1) + "\n," + sqlDataReader.GetString(0);
                    a.Size = new System.Drawing.Size(25, 30);
                    a.BackColor = System.Drawing.Color.RosyBrown;
                    a.ForeColor = System.Drawing.Color.White;
                    a.Padding = new Padding(3);
                    a.Font = new System.Drawing.Font("Arial", 15, FontStyle.Bold);
                    a.UseMnemonic = false;
                    // a.Name = sqlDataReader.GetString(0);
                    a.Location = new System.Drawing.Point(pointX, pointY);
                    control.Controls.Add(a);
                    a.MouseDown += new System.Windows.Forms.MouseEventHandler(label1_MouseDown);
                    control.Show();
                    //pointX += 50;
                    pointY += 50;

                    if (mynumtext == 3)
                    {
                        pointX += 40;
                        pointY = 45;
                        mynumtext = 0;
                    }
                    mynumtext++;
                }

            }
        }
        private string GetSepValue(string spvalue)
        {
            string str = "";

            using (SqlConnection con2 = new SqlConnection(connectionString))
            {
                SqlCommand com2 = new SqlCommand("select rtrim(Seperator) as Seperator from SeperatorMaster where SepId='" + spvalue + "'", con2);
                con2.Open();
                SqlDataReader sqlDataReader1 = com2.ExecuteReader();

                while (sqlDataReader1.Read())
                {
                    str = sqlDataReader1.GetString(0);
                }
                return str;
            }
        }

        private string GetVarValue(string varvalue)
        {
            string str1 = "";

            using (SqlConnection con3 = new SqlConnection(connectionString))
            {
                SqlCommand com3 = new SqlCommand("select rtrim(VarDes) from InPutDataTemplate where VarName='" + varvalue + "'", con3);
                con3.Open();
                SqlDataReader sqlDataReader2 = com3.ExecuteReader();

                while (sqlDataReader2.Read())
                {
                    str1 = sqlDataReader2.GetString(0);
                }
                return str1;
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
                bshBack = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.LightSkyBlue, Color.LightGreen, System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);
                bshFore = Brushes.Blue;
            }
            else
            {
                fntTab = e.Font;
                bshBack = new SolidBrush(Color.White);
                bshFore = new SolidBrush(Color.Black);
            }
            string tabName = this.tabControl1.TabPages[e.Index].Text;
            StringFormat sftTab = new StringFormat(StringFormatFlags.NoClip);
            sftTab.Alignment = StringAlignment.Center;
            sftTab.LineAlignment = StringAlignment.Center;
            e.Graphics.FillRectangle(bshBack, e.Bounds);
            Rectangle recTab = e.Bounds;
            recTab = new Rectangle(recTab.X, recTab.Y + 4, recTab.Width, recTab.Height - 4);
            e.Graphics.DrawString(tabName, fntTab, bshFore, recTab, sftTab);


            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Blue, 4);
            g.DrawRectangle(p, this.tabPage1.Bounds);
        }
    }
}
