using DG_Tool.Models;
using Microsoft.Office.Interop.Excel;
using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DG_Tool.HelperClass
{
    public class CommonClass
    {
        public static string connectionString =  EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public static List<CustomerDetails> GetCustomer()
        {
            List<CustomerDetails> list = new List<CustomerDetails>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT *FROM Vw_GetCustomer order by CustomerName", con))
                {
                    SqlDataReader reader = null;
                    cmd.CommandType = System.Data.CommandType.Text;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new CustomerDetails
                        {
                            CustomerName = reader["CustomerName"].ToString(),
                            CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        });
                    }
                    //list.Insert(0,new CustomerDetails {
                    //    CustomerName = "----Select----",
                    //    CustomerID = 0,
                    //});
                }

            }
            return list;
        }
        public static List<FileStructure> GetFileStructureType()
        {
            List<FileStructure> list = new List<FileStructure>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT *FROM vw_FileStructureList", con))
                {
                    SqlDataReader reader = null;
                    cmd.CommandType = System.Data.CommandType.Text;
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new FileStructure
                        {
                            FileStructureId = Convert.ToInt32(reader["FileStructureId"]),
                            FileStructureName = reader["FileStructureName"].ToString()
                        });
                    }
                    //list.Insert(0,new CustomerDetails {
                    //    CustomerName = "----Select----",
                    //    CustomerID = 0,
                    //});
                }

            }
            return list;
        }

        public static List<Circle> GetCircle(int customerId)
        {
            List<Circle> list = new List<Circle>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_CircleList", con))
                {
                    SqlDataReader reader = null;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    reader = cmd.ExecuteReader();
                   
                    while (reader.Read())
                    {
                        list.Add(new Circle
                        {
                            CircleID = Convert.ToInt32(reader["CircleID"]),
                            CustomerName = reader["CustomerName"].ToString(),
                            CircleName = reader["CircleName"].ToString(),
                            Status = Convert.ToInt32(reader["IsActive"]) == 1 ? "Active" : "Inactive"
                        });
                    }
                    //list.Insert(0, new Circle
                    //{
                    //    CircleName = "----Select----",
                    //    CircleID = 0,
                    //});
                }

            }
            return list;
        }
        public static List<ProfileAttribute> GetProfileAttributes(int PFId)
        {
            List<ProfileAttribute> list = new List<ProfileAttribute>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_GetProfileAttribute", con))
                {
                    SqlDataReader reader = null;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pfId", PFId);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new ProfileAttribute
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            PFName = reader["PFName"].ToString(),
                            PFId = Convert.ToInt32(reader["PFId"])
                        });
                    }
                    list.Insert(0, new ProfileAttribute
                    {
                        Id = 0,
                        PFName = "-------Select-------",
                        PFId = 0
                    });
                }

            }
            return list;
        }

        //public static List<CustomerProfile> GetCustomerProfiles()
        //{
        //    List<CustomerProfile> list = new List<CustomerProfile>();
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        con.Open();
        //        using (SqlCommand cmd = new SqlCommand("SELECT *FROM vw_GetCustomerProfile", con))
        //        {
        //            SqlDataReader reader = null;
        //            cmd.CommandType = System.Data.CommandType.Text;
        //            reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                list.Add(new CustomerProfile
        //                {
        //                    Id = Convert.ToInt32(reader["ProfileID"]),
        //                    ProfileName = reader["ProfileName"].ToString()
        //                });
        //            }
        //            list.Insert(0, new CustomerProfile
        //            {
        //                Id = 0,
        //                ProfileName = "----Select----"
        //            });
        //        }

        //    }
        //    return list;
        //}
        public static List<CustomerProfile> GetCustomerProfileList(int customerID, int circleID)
        {
            List<CustomerProfile> list = new List<CustomerProfile>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_CustomerProfileList", con))
                {
                    SqlDataReader reader = null;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerID", customerID);
                    cmd.Parameters.AddWithValue("@circleID", circleID);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new CustomerProfile
                        {
                            ProfileID = Convert.ToInt32(reader["ProfileID"]),
                            CustomerName = reader["CustomerName"].ToString(),
                            CircleName = reader["CircleName"].ToString(),
                            ProfileName = reader["ProfileName"].ToString(),
                            CreatedBy = reader["CreatedBy"].ToString(),
                            CreatedOn = reader["CreatedON"].ToString(),
                            Status = reader["StatusID"].ToString()

                        });
                    }

                }

            }
            return list;
        }

        public static List<CustomerProfileFile> GetCustProfileFileById(int customerProfileId)
        {
            List<CustomerProfileFile> list = new List<CustomerProfileFile>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_GetCustProfileFileById", con))
                {
                    SqlDataReader reader = null;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerProfileId", customerProfileId);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new CustomerProfileFile
                        {
                            CustProfileFileID = Convert.ToInt32(reader["CustProfileFileID"]),
                            FileName = reader["FileName"].ToString()
                        });
                    }
                    list.Insert(0, new CustomerProfileFile
                    {
                        CustProfileFileID = 0,
                        FileName = "----Select----"
                    });
                }

            }
            return list;
        }

        public static List<CustomerProfile> GetSingleProfileByProfileID(int customerProfileId)
        {
            List<CustomerProfile> list = new List<CustomerProfile>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_SingleCustomerProfile", con))
                {
                    SqlDataReader reader = null;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerProfileID", customerProfileId);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new CustomerProfile
                        {
                            ProfileID = Convert.ToInt32(reader["ProfileID"]),
                            CustomerName = reader["CustomerName"].ToString(),
                            CircleName = reader["CircleName"].ToString(),
                            ProfileName = reader["ProfileName"].ToString(),
                            CreatedBy = reader["CreatedBy"].ToString(),
                            CreatedOn = reader["CreatedON"].ToString(),
                            Status = reader["StatusID"].ToString()

                        });
                    }

                }

            }
            return list;
        }

        public static List<FileMaster> GetMasterFiles()
        {
            List<FileMaster> files = new List<FileMaster>();
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ConfigureFileMaster", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            files.Add(new FileMaster
                            {
                                FileMasterID = reader["FileMasterID"].ToString(),
                                FileName = reader["FileName"].ToString(),
                                FileDesc = reader["FileDesc"].ToString(),
                                FileNamingConv = reader["FileNamingConv"].ToString(),
                                FileIOID = reader["FileIOID"].ToString(),
                                FilePath = reader["FilePath"].ToString(),
                                FileStructure = reader["FileStructure"].ToString(),
                                FileExtn = reader["FileExtn"].ToString(),
                                Encrypt = reader["Encrypt"].ToString(),
                                EncryptKey = reader["EncryptKey"].ToString(),
                                StatusID = Convert.ToInt32(reader["StatusID"]),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }
            }
            return files;
        }

        public static DialogResult PreClosingConfirmation()
        {
            DialogResult res = System.Windows.Forms.MessageBox.Show(" Do you want to exit?          ", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return res;
        }

    }


}
