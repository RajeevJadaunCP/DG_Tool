using DG_Tool.WinForms.Authentication;
using System;using CardPrintingApplication;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Net;
using CardPrintingApplication;

namespace DG_Tool.HelperClass
{
    class CaptureUserActivity
    {
        string ConStr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public int UserActivity(string form_name)
        {
            int primaryId = 0;

            string hostName = Dns.GetHostName();

            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_UserActivityDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@user_id", LoginPage.primaryId);
                    cmd.Parameters.AddWithValue("@username", LoginPage.username);
                    cmd.Parameters.AddWithValue("@form_name", form_name);
                    cmd.Parameters.AddWithValue("@login_date", DateTime.Now.ToString());
                    cmd.Parameters.AddWithValue("@system_name", System.Environment.MachineName);
                    cmd.Parameters.AddWithValue("@system_ip", Dns.GetHostByName(hostName).AddressList[0].ToString());

                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        primaryId = Convert.ToInt32(reader["ID"]);
                    }

                }
            }
            return primaryId;
        }

        public int UserActivityLog(string form_name,string username,int Id)
        {
            int primaryId = 0;

            string hostName = Dns.GetHostName();

            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_UserActivityDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@user_id", Id);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@form_name", form_name);
                    cmd.Parameters.AddWithValue("@login_date", DateTime.Now.ToString());
                    cmd.Parameters.AddWithValue("@system_name", System.Environment.MachineName);
                    cmd.Parameters.AddWithValue("@system_ip", Dns.GetHostByName(hostName).AddressList[0].ToString());

                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        primaryId = Convert.ToInt32(reader["ID"]);
                    }

                }
            }
            return primaryId;
        }
    }
}

