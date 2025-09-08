using System;using CardPrintingApplication;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DG_Tool.WinForms.Dashboard
{
	public partial class DashboardSummary : Form
	{
		string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

		public DashboardSummary()
		{
			InitializeComponent();

			GetCounter();

			fillChart1();
			fillChart2();
		}

		private void GetCounter()
		{
			SqlDataReader reader = null;
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();
				using (SqlCommand cmd = new SqlCommand("usp_dashboard_counter", con))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					reader = cmd.ExecuteReader();

					while (reader.Read())
					{
						totalUsersCount.Text = reader.GetValue(0).ToString();
						activeUsersCount.Text = reader.GetValue(1).ToString();
						InactiveUsersCount.Text = reader.GetValue(2).ToString();
						totalCustomersCount.Text = reader.GetValue(3).ToString();
						customerProfileCount.Text = reader.GetValue(4).ToString();
						TotalInputFilesCount.Text = reader.GetValue(5).ToString();
						TotalOutputFilesCount.Text = reader.GetValue(6).ToString();
						totalCirclesCount.Text = reader.GetValue(7).ToString();
					}
				}
			}
		}

		private void rsButton2_Click(object sender, EventArgs e)
		{
			GetCounter();
		}

		private void fillChart1()
		{
			this.chart1.Series["Series1"].Points.AddXY("Noida", new object[]
			{
				"70"
			});
			this.chart1.Series["Series1"].Points.AddXY("Delhi", new object[]
			{
				"60"
			});
			this.chart1.Series["Series1"].Points.AddXY("Gurugram", new object[]
			{
				"50"
			});
			this.chart1.Series["Series1"].Points.AddXY("Greater Noida", new object[]
			{
				"40"
			});
			this.chart1.Series["Series1"].Points.AddXY("Haryana", new object[]
			{
				"20"
			});
		}

		private void fillChart2()
		{
			this.chart2.Series["Series1"].Points.AddXY("Airtel", new object[]
			{
				"50"
			});
			this.chart2.Series["Series1"].Points.AddXY("Vodafone", new object[]
			{
				"80"
			});
			this.chart2.Series["Series1"].Points.AddXY("Jio", new object[]
			{
				"70"
			});
		}
	}
}
