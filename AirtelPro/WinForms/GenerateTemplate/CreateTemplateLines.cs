using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace DG_Tool
{
    public partial class CreateTemplateLines : Form
    {
        int tbcount=0;
        int linecount=0;
        public static int profileid = Convert.ToInt32(CreateTemplate.CustomerProfileID);
        public static int custid = Convert.ToInt32(CreateTemplate.CustomerID);
        public static int fileid = Convert.ToInt32(ProcessData.OutTempID);
        public static string custname = CreateTemplate.CustomerName;
        public static string circlename = CreateTemplate.CircleName;
        public static string filename = ProcessData.OutTempName;
        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public CreateTemplateLines()
        {
            InitializeComponent();    
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            string s = (sender as Label).Text;
            lblCustomer.DoDragDrop(s, DragDropEffects.Copy);
        }

        private void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string readtext= e.Data.GetData(DataFormats.Text).ToString();

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
                a.Name = "1"+txt.Name;
                a.Location = new System.Drawing.Point(loc, loc1+25);
                groupBox2.Controls.Add(a);
                groupBox2.Controls.Remove(groupBox2.Controls.Find(a.Name , true)[0]);
                groupBox2.Controls.Add(a);
                groupBox2.Show();
               
            }
            else if(s.Equals("S"))
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

        private void GenrateTemplate_Load(object sender, EventArgs e)
        {
          //  label1.Text = CreateTemplate.CustomerName;
          //  label7.Text = CreateTemplate.CircleName;
          //  label5.Text = CreateTemplate.CustomerProfileName;
         //   label6.Text = CreateTemplate.OutputTempName;
            lblCustomer.Text = CreateTemplate.CustomerName;
            lblCircle.Text = CreateTemplate.CircleName;
            lblCustomerProfile.Text = CreateTemplate.CustomerProfileName;
            lblOutputTemplate.Text = ProcessData.OutTempName;
            label11.Text = "";
            //button2.Hide();
            //button3.Hide();
            //button4.Hide();
            //button5.Hide();
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
            //button1.Hide();
            AddLine(10, 15);
            tbcount = 10;
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;

            DisplayDataGridLines();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             linecount = 2;
            // label11.Text = "Line "+linecount;
            // comboBox1.Show();
            DisplayDataGridLines();
        }

        private void AddLine(int len,int ntextY)
        {
            int ntextX = 60;
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
                ntextX +=115;
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
                    a.BackColor= System.Drawing.Color.Coral;
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
                SqlCommand com4 = new SqlCommand("select rtrim(VarName) as VarName,rtrim(VarDes) as VarDes from InPutDataTemplate where VarText='AL'", con4);
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
                        a.Text = sqlDataReader.GetString(1)+","+ sqlDataReader.GetString(0);
                        a.Size = new System.Drawing.Size(18, 22);
                       // a.BackColor = System.Drawing.Color.BlueViolet; 
                        a.Font = new System.Drawing.Font("Arial",15,FontStyle.Bold);
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

        private void button2_Click(object sender, EventArgs e)
        {
            int exc_com=0;
            using (SqlConnection con1 = new SqlConnection(connectionString))
            {
                
                string query = "insert into OutFileTemplate values( @OutPutFileTemplateHDID, @FileLineNo, @LineType, @StatusID, @CreatedBY, @CreatedON,@ProfileFileID);DELETE FROM [OutputTemplateLines] WHERE ProfileFileID=@ProfileFileID and ProfileID=@ProfileID;";
                
                using (SqlCommand com1 = new SqlCommand(query, con1))
                {
                    //com1.Parameters.AddWithValue("@OutPutFileTmplateID", "1");
                    com1.Parameters.AddWithValue("@OutPutFileTemplateHDID", "1");
                    com1.Parameters.AddWithValue("@FileLineNo", linecount);
                    com1.Parameters.AddWithValue("@LineType", "TX");
                    com1.Parameters.AddWithValue("@StatusID", "1");
                    com1.Parameters.AddWithValue("@CreatedBY", "");
                    com1.Parameters.AddWithValue("@CreatedON", DateTime.Now.ToString());
                    com1.Parameters.AddWithValue("@ProfileFileID", fileid);
                    com1.Parameters.AddWithValue("@ProfileID", profileid);
                    try
                    {
                        con1.Open();
                        com1.ExecuteNonQuery();
                        con1.Close();
                        exc_com = 1;

                    }
                    catch (Exception exe)
                    {
                        MessageBox.Show("OutFileTemplate   " + exe.Message);
                    }
                }
            }

            if (exc_com == 1)
            {
                string[] t = new string[tbcount];
                string[] t1 = new string[tbcount];
                for (int i = 0; i < tbcount; i++)
                {
                    t[i] = ((TextBox)groupBox2.Controls["tex" + (i + 1).ToString()]).Text;

                    string s = t1[i] = ((TextBox)groupBox2.Controls["tex" + (i + 1).ToString()]).Text;
                    int n = 3;

                    if (s == "")
                    {

                    }
                    else if (s.Length < 4)
                    {
                        s = s.Remove(s.Length - n);
                    }
                    else
                    {
                        s = "F";
                    }

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand com = new SqlCommand("insert into OutputTemplateLines values(@OutPutFileTmpID, @FileLineNo, @VarID, @VarName, @VarValue, @VarType, @StatusID, @ProfileFileID,@ProfileID)", con))
                        {
                            com.Parameters.AddWithValue("@OutPutFileTmpID", "1");
                            com.Parameters.AddWithValue("@FileLineNo", "1");
                            com.Parameters.AddWithValue("@VarID", "1");
                            com.Parameters.AddWithValue("@VarName", t[i]);
                            com.Parameters.AddWithValue("@VarValue", "");
                            com.Parameters.AddWithValue("@VarType", s);
                            com.Parameters.AddWithValue("@StatusID", "1");
                            com.Parameters.AddWithValue("@ProfileFileID", fileid);
                            com.Parameters.AddWithValue("@ProfileID", profileid);
                            try
                            {
                                con.Open();
                                com.ExecuteNonQuery();
                                con.Close();

                            }
                            catch (Exception exe)
                            {
                                MessageBox.Show("OutputTemplateLines   " + exe.Message);
                            }
                        }
                    }
                }
                MessageBox.Show("Data Saved Successfully!");
            }
            //if (comboBox1.Text == "----")
            //{
            //    MessageBox.Show("Please Select Line '"+linecount+"' Type!");
            //}
            //else
            //{
            //    using (SqlConnection con1 = new SqlConnection(connectionString))
            //    {
            //        using (SqlCommand com1 = new SqlCommand("insert into OutFileTemplate values(@OutPutFileTmplateID, @OutPutFileTemplateHDID, @FileLineNo, @LineType, @StatusID, @CreatedBY, @CreatedON,@ProfileFileID)", con1))
            //        {
            //            com1.Parameters.AddWithValue("@OutPutFileTmplateID", "1");
            //            com1.Parameters.AddWithValue("@OutPutFileTemplateHDID", "1");
            //            com1.Parameters.AddWithValue("@FileLineNo", linecount);
            //            com1.Parameters.AddWithValue("@LineType", comboBox1.Text);
            //            com1.Parameters.AddWithValue("@StatusID", "1");
            //            com1.Parameters.AddWithValue("@CreatedBY", "Rupesh");
            //            com1.Parameters.AddWithValue("@CreatedON", DateTime.Now.ToString());
            //            com1.Parameters.AddWithValue("@ProfileFileID", "1");
                        
            //            try
            //            {
            //                con1.Open();
            //                com1.ExecuteNonQuery();
            //                con1.Close();
            //                exc_com = 1;

            //            }
            //            catch (Exception exe)
            //            {
            //                MessageBox.Show("OutFileTemplate   " + exe.Message);
            //            }
            //        }
            //    }

            //    if (exc_com == 1)
            //    {
            //        string[] t = new string[tbcount];
            //        string[] t1 = new string[tbcount];
            //        for (int i = 0; i < tbcount; i++)
            //        {
            //            t[i] = ((TextBox)groupBox2.Controls["tex" + (i + 1).ToString()]).Text;

            //            string s = t1[i] = ((TextBox)groupBox2.Controls["tex" + (i + 1).ToString()]).Text;
            //            int n = 3;

            //            if (s == "")
            //            {

            //            }
            //            else if (s.Length < 4)
            //            {
            //                s = s.Remove(s.Length - n);
            //            }
            //            else
            //            {
            //                s = "F";
            //            }

            //            using (SqlConnection con = new SqlConnection(connectionString))
            //            {
            //                using (SqlCommand com = new SqlCommand("insert into OutputTemplateLines values(@OutputTemplateLinesID, @OutPutFileTmpID, @FileLineNo, @VarID, @VarName, @VarValue, @VarType, @StatusID, @ProfileFileID)", con))
            //                {
            //                    com.Parameters.AddWithValue("@OutputTemplateLinesID", "1");
            //                    com.Parameters.AddWithValue("@OutPutFileTmpID", "1");
            //                    com.Parameters.AddWithValue("@FileLineNo", linecount);
            //                    com.Parameters.AddWithValue("@VarID", "1");
            //                    com.Parameters.AddWithValue("@VarName", t[i]);
            //                    com.Parameters.AddWithValue("@VarValue", "");
            //                    com.Parameters.AddWithValue("@VarType", s);
            //                    com.Parameters.AddWithValue("@StatusID", "1");
            //                    com.Parameters.AddWithValue("@ProfileFileID", "1");
            //                    try
            //                    {
            //                        con.Open();
            //                        com.ExecuteNonQuery();
            //                        con.Close();

            //                    }
            //                    catch (Exception exe)
            //                    {
            //                        MessageBox.Show("OutputTemplateLines   " + exe.Message);
            //                    }
            //                }
            //            }
            //        }
            //        MessageBox.Show("Data Saved Successfully!");
            //        //if (comboBox1.Text == "TX" || comboBox1.Text == "LP")
            //        //{
            //        //    string[] t = new string[tbcount];
            //        //    string[] t1 = new string[tbcount];
            //        //    for (int i = 0; i < tbcount; i++)
            //        //    {
            //        //        t[i] = ((TextBox)groupBox2.Controls["tex" + (i + 1).ToString()]).Text;

            //        //        string s = t1[i] = ((TextBox)groupBox2.Controls["tex" + (i + 1).ToString()]).Text;
            //        //        int n = 3;

            //        //        if (s == "")
            //        //        {

            //        //        }
            //        //        else if (s.Length < 4)
            //        //        {
            //        //            s = s.Remove(s.Length - n);
            //        //        }
            //        //        else
            //        //        {
            //        //            s = "F";
            //        //        }

            //        //        using (SqlConnection con = new SqlConnection(connectionString))
            //        //        {
            //        //            using (SqlCommand com = new SqlCommand("insert into OutputTemplateLines values(@OutputTemplateLinesID, @OutPutFileTmpID, @FileLineNo, @VarID, @VarName, @VarValue, @VarType, @StatusID, @ProfileFileID)", con))
            //        //            {
            //        //                com.Parameters.AddWithValue("@OutputTemplateLinesID", "1");
            //        //                com.Parameters.AddWithValue("@OutPutFileTmpID", "1");
            //        //                com.Parameters.AddWithValue("@FileLineNo", linecount);
            //        //                com.Parameters.AddWithValue("@VarID", "1");
            //        //                com.Parameters.AddWithValue("@VarName", t[i]);
            //        //                com.Parameters.AddWithValue("@VarValue", "");
            //        //                com.Parameters.AddWithValue("@VarType", s);
            //        //                com.Parameters.AddWithValue("@StatusID", "1");
            //        //                com.Parameters.AddWithValue("@ProfileFileID", "1");
            //        //                try
            //        //                {
            //        //                    con.Open();
            //        //                    com.ExecuteNonQuery();
            //        //                    con.Close();

            //        //                }
            //        //                catch (Exception exe)
            //        //                {
            //        //                    MessageBox.Show("OutputTemplateLines   " + exe.Message);
            //        //                }
            //        //            }
            //        //        }
            //        //    }
            //        //    MessageBox.Show("Data Saved Successfully!");
            //        //    linecount = 2;
            //        //    label11.Text = "Line " + linecount;
            //        //    //comboBox1.Text = "----";
            //        //    //button5.Hide();
            //        //    //AddLine(10,15);
            //        //    ClearAllText(groupBox2);
            //        //    DisplayDataGridLines();
            //        //}
            //        //else if(comboBox1.Text=="FL")
            //        //{
            //        //    MessageBox.Show("File saved");
            //        //}
            //    }
            //}
        }

        private string GetSepValue(string spvalue)
        {
            string str="";

            using (SqlConnection con2 = new SqlConnection(connectionString))
            {
                SqlCommand com2 = new SqlCommand("select rtrim(Seperator) as Seperator from SeperatorMaster where SepId='"+spvalue+"'", con2);
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
                SqlCommand com3 = new SqlCommand("select rtrim(VarDes) from InPutDataTemplate where VarName='" + varvalue+"'", con3);
                con3.Open();
                SqlDataReader sqlDataReader2 = com3.ExecuteReader();

                while (sqlDataReader2.Read())
                {
                    str1 = sqlDataReader2.GetString(0);
                }
                return str1;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddLine(10, 60);
            tbcount += 10;
            MessageBox.Show(tbcount.ToString());
            button4.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddLine(10, tbcount/10*45+15);
            tbcount += 10;
            //MessageBox.Show(tbcount.ToString());
        }

        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (comboBox1.Text == "FL")
        //    {
        //        // MessageBox.Show("File Line Found!");

        //        for (int i = 0; i < tbcount; i++)
        //        {
        //            groupBox2.Controls.Remove(groupBox2.Controls.Find("tex" + (i + 1), true)[0]);
        //        }

        //        AddPathLine(15);
        //        tbcount = 1;
        //        //button5.Show();
        //        button2.Hide();
        //        //button3.Hide();
        //    }
        //    else if (comboBox1.Text == "TX")
        //    {
        //        if (tbcount == 1)
        //        {
        //            for (int i = 0; i < tbcount; i++)
        //            {
        //                groupBox2.Controls.Remove(groupBox2.Controls.Find("filepath" + (i + 1), true)[0]);
        //            }
        //            //button5.Hide();
        //            AddLine(10, 15);
        //            tbcount = 10;
        //            button2.Show();
        //            //button3.Show();
        //        }
        //        else
        //        {
        //            for (int i = 0; i < tbcount; i++)
        //            {
        //                groupBox2.Controls.Remove(groupBox2.Controls.Find("tex" + (i + 1), true)[0]);
        //            }
        //            //button5.Hide();
        //            AddLine(10, 15);
        //            tbcount = 10;
        //            button2.Show();
        //            //button3.Show();
        //        }

        //    }
        //    else if (comboBox1.Text == "LP")
        //    {
        //        if (tbcount == 1)
        //        {
        //            for (int i = 0; i < tbcount; i++)
        //            {
        //                groupBox2.Controls.Remove(groupBox2.Controls.Find("filepath" + (i + 1), true)[0]);
        //            }
                   
        //            //button5.Hide();
        //            AddLine(10, 15);
        //            tbcount = 10;
        //            button2.Show();
        //            //button3.Show();
        //        }
        //        else
        //        {
        //            for (int i = 0; i < tbcount; i++)
        //            {
        //                groupBox2.Controls.Remove(groupBox2.Controls.Find("tex" + (i + 1), true)[0]);
        //            }
        //            //button5.Hide();
        //            AddLine(10, 15);
        //            tbcount = 10;
        //            button2.Show();
        //            //button3.Show();
        //        }
        //    }
        //}

        void ClearAllText(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();
                else
                    ClearAllText(c);
                c.BackColor = Color.White;
            }
        }

        private void DisplayDataGridLines()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand("GetGridDataLines", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@outfileid", ProcessData.OutTempID);
                com.Parameters.AddWithValue("@profileid", CreateTemplate.CustomerProfileID);
                con.Open();

                SqlDataReader reader1;
                reader1 = com.ExecuteReader();

                richTextBox1.Text += "Line :";
                richTextBox1.Text += " ";
                //richTextBox1.Text += comboBox1.Text;

                while (reader1.Read())
                {
                    // MessageBox.Show(reader1.GetValue(0).ToString());
                    richTextBox1.Text += " ";
                    richTextBox1.Text += reader1.GetValue(0).ToString();
                    richTextBox1.ReadOnly= true;
                }
                richTextBox1.Text += "\n";
                con.Close();
            }
        }

        public static DataTable ConvertGrid(DataTable dt)

        {

            DataTable dtConvert = new DataTable();

            for (int i = 0; i <= dt.Rows.Count; i++)

            {

                dtConvert.Columns.Add("Col_" + Convert.ToString(i));

            }

            for (int i = 0; i < dt.Columns.Count; i++)

            {

                dtConvert.Rows.Add();

                dtConvert.Rows[i][0] = dt.Columns[i].ColumnName;

                for (int j = 0; j < dt.Rows.Count; j++)

                {

                    dtConvert.Rows[i][j + 1] = dt.Rows[j][i];

                }

            }

            return dtConvert;

        }

        public static DataTable ConvertGridNew(DataTable dt)
        {
            DataTable dtConvert = new DataTable();

            for (int i = 0; i <= dt.Rows.Count; i++)

            {

                 dtConvert.Columns.Add(Convert.ToString(i));
               // dtConvert.Columns.Add();
            }

            for (int i = 0; i < dt.Columns.Count; i++)

            {

                dtConvert.Rows.Add();

                dtConvert.Rows[i][0] = dt.Columns[i].ColumnName;

                for (int j = 0; j < dt.Rows.Count; j++)

                {

                    dtConvert.Rows[i][j + 1] = dt.Rows[j][i];

                }

            }

            return dtConvert;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "File Browser";
            fdlg.InitialDirectory = @"d:\";
            fdlg.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                TextBox textBox = (TextBox)(groupBox2.Controls.Find("filepath1", true)[0]);
                textBox.Text=fdlg.FileName;
                textBox.ReadOnly = true;
                button2.Show();
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

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
