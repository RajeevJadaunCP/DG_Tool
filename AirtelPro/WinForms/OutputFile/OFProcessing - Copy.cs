using AirtelPro.HelperClass;
using AirtelPro.Models;
using AirtelPro.WinForms.Authentication;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace AirtelPro.WinForms.OutputFile
{
    public partial class OFProcessing : Form
    {
        public static string customer = string.Empty;
        public static string circle = string.Empty;
        public static string profile = string.Empty;
        public static string inputFile = string.Empty;
        public static string licenceFile = string.Empty;
        public static int lastInsertedId = 0;

        public static int customerID = 0;
        public static int circleID = 0;
        public static int ProfileID = 0;

        StringBuilder logString = new StringBuilder();

        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public OFProcessing()
        {
            InitializeComponent();
            var customerList = CommonClass.GetCustomer();

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
        }
        private void btnLicence_Click(object sender, EventArgs e)
        {
            string filepath = string.Empty;
            string filename = string.Empty;

            if(ofdLicence.ShowDialog() == DialogResult.OK)
            {
                filepath = ofdLicence.FileName;
                filename = Path.GetFileName(filepath);
                if (!IsDuplicateFile(filename))
                {
                    txtLicence.Text = filepath;
                }
                else
                {
                    MessageBox.Show("File already exist: ",
                                                "Message",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information
                                                );
                }

            }
            else
            {
                txtLicence.Clear();
            }

            licenceFile = filename;
        }

        private void btnInputFile_Click(object sender, EventArgs e)
        {
            string filepath = string.Empty;
            string filename = string.Empty;

            if (ofdInputFile.ShowDialog() == DialogResult.OK)
            {
                filepath = ofdInputFile.FileName;
                filename = Path.GetFileName(filepath);

                if (!IsDuplicateFile(filename))
                {
                    txtInputfile.Text = filepath;
                }
                else
                {
                    MessageBox.Show("File already exist: ",
                                                "Message",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information
                                                );
                }




            }
            else
            {
                txtInputfile.Clear();
            }
            inputFile = filename;


            //OpenFileDialog openFileDialog = new OpenFileDialog();

            //// Allow multiple file selection
            //openFileDialog.Multiselect = true;

            //// Set the title of the dialog box
            //openFileDialog.Title = "Select Multiple Files";

            //// Set the filter for the types of files you want to select
            //openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            //// Show the dialog
            //DialogResult result = openFileDialog.ShowDialog();

            //// Check if the user clicked OK
            //if (result == DialogResult.OK)
            //{
            //    // Get the selected file paths
            //    string[] fileNames = openFileDialog.FileNames;

            //    // Display the selected file paths
            //    foreach (string fileName in fileNames)
            //    {
            //        if (!IsDuplicateFile(fileName))
            //        {
            //            txtInputfile.Text += fileName;
            //        }
            //        else
            //        {
            //            MessageBox.Show("File already exist: ",
            //                                        "Message",
            //                                        MessageBoxButtons.OK,
            //                                        MessageBoxIcon.Information
            //                                        );
            //        }

            //    }
            //    MessageBox.Show(string.Join(",", fileNames));
            //}
            //else
            //{
            //    txtInputfile.Clear();
            //}
            ////inputFile = filename;
            ////MessageBox.Show(string.Join(",", fileNames));

        }

        public bool IsDuplicateFile(string filename)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataReader reader = null;
                bool flag = false;
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT *FROM Vw_DataGenProcessList WHERE datafilename = @finename", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@finename", filename);
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        flag = true;
                    }
                }
                return flag;
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
        }

        private void cbxCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCircle.SelectedIndex > 0)
            {
                var customerProfile = CommonClass.GetCustomerProfileList(Convert.ToInt32(cbxCustomer.SelectedValue), Convert.ToInt32(cbxCircle.SelectedValue));

                if (customerProfile != null && customerProfile.Count > 0)
                {
                    customerProfile.Insert(0, new CustomerProfile
                    {
                        ProfileID = 0,
                        ProfileName = "----Select----"
                    });
                    cbxProfile.DataSource = customerProfile;
                    cbxProfile.DisplayMember = "ProfileName";
                    cbxProfile.ValueMember = "ProfileID";
                }
                else
                {
                    cbxProfile.DataSource = null;
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbxCustomer.SelectedIndex > 0 && cbxCircle.SelectedIndex > 0 && cbxProfile.SelectedIndex > 0 && !string.IsNullOrEmpty(txtInputfile.Text.Trim()) && !string.IsNullOrEmpty(txtLicence.Text.Trim()))
                {
                    customerID = Convert.ToInt32(cbxCustomer.SelectedValue);
                    circleID = Convert.ToInt32(cbxCircle.SelectedValue);
                    ProfileID = Convert.ToInt32(cbxProfile.SelectedValue);

                    //looging
                    logString.Append(DateTime.Now + "\n**File upload process started**");

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {


                        con.Open();
                        SqlDataReader reader = null;
                        using (SqlCommand cmd = new SqlCommand("usp_SaveDataGenProcessFiles", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@custId", cbxCustomer.SelectedValue);
                            cmd.Parameters.AddWithValue("@circleID", cbxCircle.SelectedValue);
                            cmd.Parameters.AddWithValue("@custProfileID", cbxProfile.SelectedValue);
                            cmd.Parameters.AddWithValue("@statusID", 1);
                            cmd.Parameters.AddWithValue("@createdBY", NewLogin.primaryId);

                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                lastInsertedId = Convert.ToInt32(reader["ID"]);
                            }
                            if (lastInsertedId > 0)
                            {
                                saveDataGetProcessHDfiles(lastInsertedId);
                                MessageBox.Show("File uploaded successfully: ",
                                            "Message",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information
                                            );

                                DataGenProcessList dataGenProcessList = new DataGenProcessList();
                                dataGenProcessList.ShowDialog();
                                this.Hide();
                            }


                            //logging
                            logString.Append("**File uploading completed**");
                        }
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
            catch (Exception ex)
            {
                logString.Append("\nSomething went wrong: " + ex.Message);
            }
            finally
            {
                File.AppendAllText("Logging/" + "log.txt", logString.ToString());
                logString.Clear();
            }

        }

        private void saveDataGetProcessHDfiles(int dataGenProcessHDID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataReader reader = null;
                using (SqlCommand cmd = new SqlCommand("Select *FROM CustProfileFile WHERE CustomerID = @customerId AND CustProfileID = @customerProfileID", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@customerId", cbxCustomer.SelectedValue);
                    cmd.Parameters.AddWithValue("@customerProfileID", cbxProfile.SelectedValue);
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int i = 0;
                            string fileName = string.Empty;
                            string filePath = string.Empty;
                            int custProfileFileID = Convert.ToInt32(reader["CustProfileFileID"]);

                            if (reader["FileIOID"].ToString().Equals("I"))
                            {
                                if (i == 0)
                                {
                                    filePath = txtInputfile.Text;
                                    fileName = Path.GetFileName(txtInputfile.Text);
                                }
                                else
                                {
                                    filePath = txtLicence.Text;
                                    fileName = Path.GetFileName(txtLicence.Text);
                                }
                                i++;
                            }
                            SaveDataGenProcessHDFiles(dataGenProcessHDID, custProfileFileID, fileName, filePath);
                        }
                    }


                }
            }
        }
        private void SaveDataGenProcessHDFiles(int dataGenProcessHDID, int custProfileFileID, string fileName, string filePath)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlDataReader reader = null;
                using (SqlCommand cmd = new SqlCommand("usp_SaveDataGenProcessHDFiles", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dataGenProcessHDID", dataGenProcessHDID);
                    cmd.Parameters.AddWithValue("@fileName", fileName);
                    cmd.Parameters.AddWithValue("@filePath", filePath);
                    cmd.Parameters.AddWithValue("@custProfileFileID", custProfileFileID);
                    cmd.Parameters.AddWithValue("@createdBY", NewLogin.primaryId);

                    reader = cmd.ExecuteReader();
                }
            }

            //logging end

            customer = cbxCustomer.Text;
            circle = cbxCircle.Text;
            profile = cbxProfile.Text;

        }

        private void pbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
