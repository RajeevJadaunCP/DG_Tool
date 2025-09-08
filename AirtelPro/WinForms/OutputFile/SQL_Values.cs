using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DG_Tool.WinForms.OutputFile
{
    class SQL_Values
    {
        static string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public static string sql_data_value(string query_sql, string data_name)
        {
            string sql_data_val = "";
            //string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlConnection con1 = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query_sql, con1))
                {
                    try
                    {
                        con1.Open();
                        SqlDataReader myReader = cmd.ExecuteReader();
                        if (myReader.HasRows)
                        {
                            while (myReader.Read())
                                sql_data_val = myReader[data_name].ToString().Trim();
                        }
                        else
                        { sql_data_val = "0"; }

                        //MessageBox.Show(sql_data_val);
                        con1.Close();

                    }
                    catch (Exception exe)
                    {
                        MessageBox.Show(exe.Message);
                    }
                }



            }
            return sql_data_val;
        }

        public static DataTable sql_data_value_extra(string query_sql)
        {
            string sql_data_val = "";
            //string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlConnection con1 = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query_sql, con1))
                {
                    try
                    {
                        con1.Open();
                        SqlDataReader myReader = cmd.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(myReader);
                        con1.Close();
                        return dt;



                    }
                    catch (Exception exe)
                    {
                        MessageBox.Show(exe.Message);
                        return null;
                    }
                }



            }
        }

        public static int insert_data_DataGenProcessHD(int id, string lot_no, string first_imsi, string first_msisdn, string qty)
        {


            using (SqlConnection con1 = new SqlConnection(connectionString))
            {

                //MessageBox.Show(file_name_inp);
                SqlDataReader reader = null;
                using (SqlCommand cmd = new SqlCommand("insert into DataGenProcessHD  (DataGenProcessHDID, Lot_NO, First_IMSI, First_MSISDN, DataGenProcessDate,DataGenProcessStatus ,Qty ) values( @DataGenProcessHDID ,@lot_no, @first_imsi, @first_msisdn, @date, @Status,@qty)", con1))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@DataGenProcessHDID", id);
                    cmd.Parameters.AddWithValue("@lot_no", lot_no);
                    cmd.Parameters.AddWithValue("@first_imsi", first_imsi);
                    cmd.Parameters.AddWithValue("@first_msisdn", first_msisdn);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Status", "I");
                    cmd.Parameters.AddWithValue("@qty", Int32.Parse(qty));
                    try
                    {
                        con1.Open();
                        reader = cmd.ExecuteReader();

                        con1.Close();
                        //MessageBox.Show("Data Saved Successfully!");
                    }
                    catch (Exception exe)
                    {
                        MessageBox.Show(exe.Message);
                    }
                }
                return 1;
            }

        }

    }
}
