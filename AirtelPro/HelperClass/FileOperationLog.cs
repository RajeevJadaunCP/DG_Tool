using DG_Tool.Models;
using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DG_Tool.HelperClass
{
    internal class FileOperationLog
    {
        public static string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public void Save(int HDID, string method, string operation, string msg,int flag)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Insert INTO " +
                    "FileGenerationErrorLogs(MethodName,HDID,FileName,Operation,ErrorMessage,CreatedOn,Flag) " +
                    "Values(@methodname,@HDID,@filename,@operation,@errorMessage,@createdOn,@flag)", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@methodname",method); 
                    cmd.Parameters.AddWithValue("@HDID",HDID);
                    cmd.Parameters.AddWithValue("@filename", "DataGenProcessList");
                    cmd.Parameters.AddWithValue("@operation",operation);
                    cmd.Parameters.AddWithValue("@errorMessage",msg);
                    cmd.Parameters.AddWithValue("@createdOn",DateTime.Now);
                    cmd.Parameters.AddWithValue("@flag",flag);
                    cmd.ExecuteReader();
                }

            }
        }
    }
}
