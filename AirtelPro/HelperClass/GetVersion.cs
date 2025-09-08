using System;using CardPrintingApplication;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DG_Tool.HelperClass
{
	public  static class GetVersion
	{
		static string ConStr =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

		public static string SetVersion() 
		{
			string version = string.Empty;

			using (SqlConnection con3 = new SqlConnection(ConStr))
			{
				SqlCommand com3 = new SqlCommand("select Version from [dbo].[AppVersionControl] order by id desc", con3);
				con3.Open();
				SqlDataReader reader3 = com3.ExecuteReader();

				if (reader3.HasRows)
				{
					if (reader3.Read())
					{
						version = reader3.GetValue(0).ToString().Trim();
					}
				}
				else
				{
					MessageBox.Show("Parameter not found!");
				}
				con3.Close();
			}

			return "Version : " + version + "";
		}

		public static string GetYears()
		{
			return "Copyright © " + DateTime.Now.ToString("yyyy") + " Colorplast Systems Private Limited";
		}

	}
}
