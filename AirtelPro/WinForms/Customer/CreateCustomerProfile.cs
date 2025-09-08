using DG_Tool.HelperClass;
using DG_Tool.Models;
using DG_Tool.WinForms.Authentication;
using DG_Tool.WinForms.Customer;
using DG_Tool.WinForms.OutputFile;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DG_Tool
{
    public partial class CreateCustomerProfile : Form
    {
        public static int profileID = 0;
        //public static string customerName = string.Empty;
        //public static string circleName = string.Empty;
        //public static string customerProfile = string.Empty;

        string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        List<Label> labels = new List<Label>();
        List<System.Windows.Forms.ComboBox> comboBoxes = new List<System.Windows.Forms.ComboBox>();

        int lblMount = 1;
        int cbxMount = 1;
        public CreateCustomerProfile()
        {
            InitializeComponent();

            CustomerProfileList.customerName = string.Empty;
            CustomerProfileList.circleName = string.Empty;
            CustomerProfileList.customerProfile = string.Empty;

            AddLables();
            AddComboBox();
            var customerList = CommonClass.GetCustomer();
            //var pfOneList = CommonClass.GetProfileAttributes(1);
            //var pfTwoList = CommonClass.GetProfileAttributes(2);
            //var pfThreeList = CommonClass.GetProfileAttributes(3);
            //var pfFourList = CommonClass.GetProfileAttributes(4);

            if (customerList != null && customerList.Count > 0)
            {
                customerList.Insert(0, new CustomerDetails
                {
                    CustomerName = "----Select----",
                    CustomerID = 0,
                });
                cbxCustomer.DataSource = customerList;
                cbxCustomer.DisplayMember = "CustomerName";
                cbxCustomer.ValueMember = "CustomerID";

            }
            var customerList1= CommonClass.GetCustomer();
            //var pfOneList = CommonClass.GetProfileAttributes(1);
            //var pfTwoList = CommonClass.GetProfileAttributes(2);
            //var pfThreeList = CommonClass.GetProfileAttributes(3);
            //var pfFourList = CommonClass.GetProfileAttributes(4);

            if (customerList1 != null && customerList1.Count > 0)
            {
                customerList1.Insert(0, new CustomerDetails
                {
                    CustomerName = "----Select----",
                    CustomerID = 0,
                });
                comboBox1.DataSource = customerList1;
                comboBox1.DisplayMember = "CustomerName";
                comboBox1.ValueMember = "CustomerID";

            }
            var customerList2 = CommonClass.GetCustomer();
            //var pfOneList = CommonClass.GetProfileAttributes(1);
            //var pfTwoList = CommonClass.GetProfileAttributes(2);
            //var pfThreeList = CommonClass.GetProfileAttributes(3);
            //var pfFourList = CommonClass.GetProfileAttributes(4);

            if (customerList2 != null && customerList2.Count > 0)
            {
                customerList2.Insert(0, new CustomerDetails
                {
                    CustomerName = "----Select----",
                    CustomerID = 0,
                });
                comboBox4.DataSource = customerList2;
                comboBox4.DisplayMember = "CustomerName";
                comboBox4.ValueMember = "CustomerID";

            }


            //if (pfOneList != null && pfOneList.Count > 0)
            //{
            //    cbxPostPaid.DataSource = pfOneList;
            //    cbxPostPaid.DisplayMember = "PFName";
            //    cbxPostPaid.ValueMember = "Id";
            //}
            //if (pfTwoList != null && pfTwoList.Count > 0)
            //{
            //    cbxChipSize.DataSource = pfTwoList;
            //    cbxChipSize.DisplayMember = "PFName";
            //    cbxChipSize.ValueMember = "Id";
            //}
            //if (pfThreeList != null && pfThreeList.Count > 0)
            //{
            //    cbxOS.DataSource = pfThreeList;
            //    cbxOS.DisplayMember = "PFName";
            //    cbxOS.ValueMember = "Id";
            //}
            //if (pfFourList != null && pfFourList.Count > 0)
            //{
            //    cbxPF4.DataSource = pfFourList;
            //    cbxPF4.DisplayMember = "PFName";
            //    cbxPF4.ValueMember = "Id";
            //}
        }
        private void AddLables()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT  *FROM ProfileAttributeHD", con))
                    {
                        SqlDataReader reader = null;
                        cmd.CommandType = CommandType.Text;

                        reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Label lbl = new Label();
                                lbl.Location = new Point(25, 85 + (lblMount * 35));
                                //lbl.Left = 200;
                                lbl.Text = reader["HeaderName"].ToString();
                                lbl.Name = reader["Id"].ToString();
                                lbl.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                                panel2.Controls.Add(lbl);
                                labels.Add(lbl);
                                lblMount++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message,
                                        "Message",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
        }
        private void AddComboBox()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Select pahd.Id,pahd.ProfileAttributeHDID,pahd.HeaderName,paa.ProfileName FROM ProfileAttributeHD pahd LEFT JOIN (SELECT pa.PFId,STRING_AGG(pa.PFName, ',') As ProfileName FROM ProfileAttribute pa  Group by pa.PFId) AS paa ON paa.PFId = pahd.Id", con))
                    {
                        SqlDataReader reader = null;
                        cmd.CommandType = CommandType.Text;

                        reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                System.Windows.Forms.ComboBox combo = new System.Windows.Forms.ComboBox();

                                combo.Location = new Point(125, 85 + (cbxMount * 35));
                                combo.DataSource = ("----Select----,"+reader["ProfileName"].ToString()).Split(',').ToList();
                                combo.Name = "cbx" + reader["Id"].ToString();
                                combo.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                                combo.Size = new Size(150, 20);

                                combo.DropDownStyle = ComboBoxStyle.DropDownList;

                                //Adding control even to comboBox
                                combo.SelectedIndexChanged += new EventHandler(combo_SelectedIndexChanged);

                                comboBoxes.Add(combo);
                                panel2.Controls.Add(combo);
                                cbxMount++;
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message,
                                        "Message",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                        );
            }
        }
        public void combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetProfileName();
        }
        private void GetProfileName()
        {
            int profileID = 0;
            try
            {
                if (cbxCustomer.SelectedIndex > 0 && cbxCircle.SelectedIndex > 0)
                {
                    txtProfileName.Text = string.Empty;
                    StringBuilder profileName = new StringBuilder();

                    for (int i = 0; i < comboBoxes.Count; i++)
                    {
                        profileName.Append(comboBoxes[i].Text);
                        if (i < comboBoxes.Count - 1)
                        {
                            profileName.Append("_");
                        }
                    }
                    txtProfileName.Text = cbxCustomer.Text + '_' + cbxCircle.Text + '_' + profileName.ToString();

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        SqlDataReader reader = null;
                        con.Open();
                        //Todo change to procedure, also check for customer and circle
                        using (SqlCommand cmd = new SqlCommand("SELECT ProfileID FROM CustProfile WHERE  ProfileName = @pfName", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@pfName", txtProfileName.Text);
                            reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    profileID = Convert.ToInt32(reader["ProfileID"]);
                                }
                            }
                        }
                    }
                    if (profileID > 0)
                    {
                        lblmsgProfilename.Text = "Profile already exist";
                        lblmsgProfilename.ForeColor = Color.Red;
                        lblmsgProfilename.Visible = true;
                        btnSubmit.Enabled = false;
                    }
                    else
                    {
                        lblmsgProfilename.Visible = false;
                        btnSubmit.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Please select Customer and Circle from dropdown: ",
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
        private bool CheckSelectedIndex()
        {
            bool flag = true;
            for (int i = 0; i < comboBoxes.Count; i++)
            {
                if(comboBoxes[i].SelectedIndex == 0)
                {
                    flag = false;
                }
            }
            return flag;
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            GetProfileName();

            

            if (cbxCustomer.SelectedIndex > 0 && cbxCircle.SelectedIndex > 0 && !string.IsNullOrEmpty(txtProfileName.Text.Trim()))
            {
                if(CheckSelectedIndex())
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            SqlDataReader reader = null;
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand("usp_SaveCustomerProfile", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@custid", cbxCustomer.SelectedValue);
                                cmd.Parameters.AddWithValue("@circleId", cbxCircle.SelectedValue);
                                cmd.Parameters.AddWithValue("@profileName", txtProfileName.Text);
                                cmd.Parameters.AddWithValue("@createdBy", NewLogin.primaryId);
                                reader = cmd.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        profileID = Convert.ToInt32(reader["ProfileID"]);
                                        SaveProfileAttribute(profileID);

                                    }
                                }

                                MessageBox.Show("Profile created successfully",
                                                "Message",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information
                                                );
                                ConfigureFile allCustomers = new ConfigureFile(cbxCustomer.Text, cbxCircle.Text, txtProfileName.Text);
                                allCustomers.ShowDialog();
                                this.Hide();
                            }
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
                else
                {
                    MessageBox.Show("Please select all the attributes ",
                                                "Error",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning
                                                );
                }
                
            }
            else
            {
                MessageBox.Show("All fields are required: ",
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information
                                            );
            }

        }
        private void SaveProfileAttribute(int profileID)
        {
            int[] attrubutrFileHDID = new int[labels.Count];
            int[] attrubutrFileID = new int[comboBoxes.Count];

            for (int i = 0; i < comboBoxes.Count; i++)
            {
                attrubutrFileHDID[i] = Convert.ToInt32(labels[i].Name.ToString());

                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        SqlDataReader reader = null;
                        con.Open();
                        //Todo
                        using (SqlCommand cmd = new SqlCommand("SELECT Id FROM ProfileAttribute WHERE PFId = @attrubutrFileHDID AND PFName = @pfName", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@pfName", comboBoxes[i].SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@attrubutrFileHDID", attrubutrFileHDID[i]);
                            reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    attrubutrFileID[i] = Convert.ToInt32(reader["Id"]);
                                }
                            }
                        }
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
            for (int i = 0; i < labels.Count; i++)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        SqlDataReader reader = null;
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("Insert INTO " +
                            "CustomerProfileAttribute(CustomerID,CircleID,CustomerProfileID,ProfileAttributeHDID,ProfieAttributeID) " +
                            "VALUES(@customerID,@circleID,@customerProfileID,@profileAttributeHDID,@profileAttributeID)", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@customerID", cbxCustomer.SelectedValue);
                            cmd.Parameters.AddWithValue("@circleID", cbxCircle.SelectedValue);
                            cmd.Parameters.AddWithValue("@customerProfileID", profileID);
                            cmd.Parameters.AddWithValue("@profileAttributeHDID", attrubutrFileHDID[i]);
                            cmd.Parameters.AddWithValue("@profileAttributeID", attrubutrFileID[i]);
                            cmd.ExecuteReader();
                        }
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


        }
        private void cbxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCustomer.SelectedIndex > 0)
            {

                var circulList = CommonClass.GetCircle(Convert.ToInt32(cbxCustomer.SelectedValue));
                if (circulList != null && circulList.Count > 0)
                {
                    circulList.Insert(0, new Circle
                    {
                        CircleName = "----Select----",
                        CircleID = 0,
                    });

                    cbxCircle.DataSource = circulList;
                    cbxCircle.DisplayMember = "CircleName";
                    cbxCircle.ValueMember = "CircleID";

                }
                else
                {
                    cbxCircle.DataSource = null;
                }
            }
            else if (cbxCustomer.SelectedIndex > 0 && cbxCircle.SelectedIndex > 0)
            {
                GetProfileName();
            }
        }
        private void cbxCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cbxCustomer.SelectedIndex > 0 && cbxCircle.SelectedIndex > 0)
            //{
            //    GetProfileName();
            //}
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (CommonClass.PreClosingConfirmation() == DialogResult.Yes)
            {
                this.Close();
            }
        }
        private void pbInfo_MouseHover(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.pbInfo, "Autogenerated as comination of Customer, Circle, and other Attributes");
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click_2(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel2.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int custID=Convert.ToInt32(comboBox1.SelectedValue.ToString());
            int newcustID=Convert.ToInt32(comboBox4.SelectedValue.ToString());
            int circleID = Convert.ToInt32(comboBox2.SelectedValue.ToString());
            int newcircleID = Convert.ToInt32(comboBox5.SelectedValue.ToString());
            int profileID = Convert.ToInt32(comboBox3.SelectedValue.ToString());
            string profilename = textBox1.Text.Trim();
            if (custID>0 && profileID>0 && circleID>0 && !string.IsNullOrEmpty(profilename) && newcustID > 0 && newcircleID > 0)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("CreateNewProfileAndCopyData", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@OldCustID", custID);
                            cmd.Parameters.AddWithValue("@newCustID", newcustID);
                            cmd.Parameters.AddWithValue("@OldCircleID", circleID);
                            cmd.Parameters.AddWithValue("@newCircleID", newcircleID);
                            cmd.Parameters.AddWithValue("@OldProfileID", profileID);
                            cmd.Parameters.AddWithValue("@NewProfileName", profilename);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                MessageBox.Show("Profile Created Succesfully.");
            }
            else
            {
                MessageBox.Show("Please Enter All Details First.");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > 0)
            {

                var circulList = CommonClass.GetCircle(Convert.ToInt32(comboBox1.SelectedValue));
                if (circulList != null && circulList.Count > 0)
                {
                    circulList.Insert(0, new Circle
                    {
                        CircleName = "----Select----",
                        CircleID = 0,
                    });

                    comboBox2.DataSource = circulList;
                    comboBox2.DisplayMember = "CircleName";
                    comboBox2.ValueMember = "CircleID";

                }
                else
                {
                    comboBox2.DataSource = null;
                }
            }
        }
        
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex > 0)
            {
                List<CustomerProfile> circulList = new List<CustomerProfile>();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM CustProfile ", con))
                        {
                            //cmd.Parameters.AddWithValue("@ProfileName", comboBox2.SelectedValue.ToString());
                            cmd.CommandType = CommandType.Text;

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    circulList.Add(new CustomerProfile
                                    {
                                        ProfileID = reader.GetInt32(reader.GetOrdinal("ProfileID")),
                                        CustomerName = "",
                                        CircleName = "",
                                        ProfileName = reader["ProfileName"].ToString(),
                                        CreatedBy = reader["CreatedBy"].ToString(),
                                        CreatedOn = reader["CreatedON"].ToString(),
                                        Status = reader["StatusID"].ToString()
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
                if (circulList != null && circulList.Count > 0)
                {
                    circulList.Insert(0, new CustomerProfile
                    {
                        ProfileName = "----Select----",
                        ProfileID = 0,
                    });
                    comboBox3.DataSource = circulList;
                    comboBox3.DisplayMember = "ProfileName";
                    comboBox3.ValueMember = "ProfileID";
                }
                else
                {
                    comboBox3.DataSource = null;
                }
            }
        }

        private void CreateCustomerProfile_Load(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex > 0)
            {

                var circulList = CommonClass.GetCircle(Convert.ToInt32(comboBox4.SelectedValue));
                if (circulList != null && circulList.Count > 0)
                {
                    circulList.Insert(0, new Circle
                    {
                        CircleName = "----Select----",
                        CircleID = 0,
                    });
                    comboBox5.DataSource = circulList;
                    comboBox5.DisplayMember = "CircleName";
                    comboBox5.ValueMember = "CircleID";

                }
                else
                {
                    comboBox5.DataSource = null;
                }
            }
        }

        //private void CreateCustomerProfile_Load(object sender, EventArgs e)
        //{
        //    int  width = Screen.PrimaryScreen.Bounds.Width;
        //    int height = Screen.PrimaryScreen.Bounds.Height;

        //    this.Location = new Point(0, 0);
        //    this.Size = new Size(width, height);
        //}
    }
}