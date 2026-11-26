
using CardPrintingApplication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DG_Tool.WinForms.OutputFile
{
    public class FileProcessor
    {
        // ====================================================================
        //  PER-FILE STATE
        //  These used to be fields on the OFProcessing form, shared by every
        //  file. Now each FileProcessor owns its own copy.
        // ====================================================================
        private int lastInsertedId;
        private int fileid;
        private int records;
        private DataTable Process_data;
        private StringBuilder logString = new StringBuilder();
        private string lastCodeMSN, lastCodeMSC;
        private string firstCodemsn, firstCodemsc;
        private string batchnumber_file;
        private int Total_no_of_records;

        // ====================================================================
        //  READ-ONLY CONFIG - passed in from the form
        // ====================================================================
        private readonly int ProfileID;
        private readonly string hsm_IP;
        private readonly string euicc_hsm_IP;
        private readonly string customer_name_form;
        private readonly string rjio_prefix;
        private readonly string connectionString;
        private readonly string reliance_tranport_key;
        private readonly string reliance_op_key;
        private readonly int eid_db;
        private readonly int innerDop;      // how many records at a time

        // ====================================================================
        //  RESULTS the caller reads after processing
        // ====================================================================
        public string Log { get { return logString.ToString(); } }
        public int RecordCount { get { return records; } }
        public string LastError { get; private set; }
        public int EID_db_last { get; private set; }
        public string LastCodeMSN { get { return lastCodeMSN; } }
        public string LastCodeMSC { get { return lastCodeMSC; } }
        public string FirstCodeMSN { get { return firstCodemsn; } }
        public string FirstCodeMSC { get { return firstCodemsc; } }

        private string _inputFileName;
        public string InputFileName
        {
            get
            {
                if (_inputFileName == null)
                {
                    try { _inputFileName = getfilenameandidwithhdid(); }
                    catch { _inputFileName = "HDID " + lastInsertedId; }
                }
                return _inputFileName;
            }
        }

        // ====================================================================
        //  CONSTRUCTOR
        // ====================================================================
        public FileProcessor(int hdid,
                             int profileId,
                             string hsmIp,
                             string euiccHsmIp,
                             string customerName,
                             string rjioPrefix,
                             string connString,
                             string transportKey,
                             string opKey,
                             int eidDb,
                             int innerDegreeOfParallelism)
        {
            lastInsertedId = hdid;
            fileid = hdid;
            ProfileID = profileId;
            hsm_IP = hsmIp;
            euicc_hsm_IP = euiccHsmIp;
            customer_name_form = customerName;
            rjio_prefix = rjioPrefix;
            connectionString = connString;
            reliance_tranport_key = transportKey;
            reliance_op_key = opKey;
            eid_db = eidDb;
            innerDop = Math.Max(1, innerDegreeOfParallelism);
        }

        // ====================================================================
        //  CHANGED: CRYPTOGRAPHIC RANDOM
        //
        //  Your original used  Random RNG32 = new Random();  for R_32_Hex_KI
        //  (the SIM authentication key). System.Random is seeded from the
        //  system clock and is predictable - anyone who knows roughly when a
        //  batch ran can regenerate every key in it. It is also not
        //  thread-safe: two threads calling it at once can make it return
        //  zeros.
        //
        //  RandomNumberGenerator is cryptographically secure AND thread-safe,
        //  so no lock is needed.
        // ====================================================================
        private static readonly RandomNumberGenerator SecureRng = RandomNumberGenerator.Create();

        private static byte[] SecureBytes(int count)
        {
            byte[] b = new byte[count];
            SecureRng.GetBytes(b);
            return b;
        }

        public static string SecureRandom8Hex()
        {
            return BitConverter.ToString(SecureBytes(4)).Replace("-", "").ToUpper();
        }

        public static string SecureRandom4Digits()
        {
            return (1000 + (int)(BitConverter.ToUInt32(SecureBytes(4), 0) % 9000)).ToString();
        }

        public static string SecureRandom8Digits()
        {
            return (10000000 + (int)(BitConverter.ToUInt32(SecureBytes(4), 0) % 90000000)).ToString();
        }

        /// <summary>
        /// Replaces random_hex_generator. Input is a comma-separated list of
        /// byte counts; output is a comma-separated list of hex strings.
        /// </summary>
        public static string SecureRandomHex(string data_input)
        {
            var data_value = data_input.Split(',');
            var sb = new StringBuilder();
            for (int i = 0; i < data_value.Length; i++)
            {
                int n = Int32.Parse(data_value[i]);
                sb.Append(BitConverter.ToString(SecureBytes(n)).Replace("-", "").ToUpper()).Append(',');
            }
            return sb.ToString();
        }

        public static string SecureHexString(int hexChars)
        {
            return BitConverter.ToString(SecureBytes(hexChars / 2)).Replace("-", "").ToUpper();
        }

        // ====================================================================
        //  HELPERS copied from OFProcessing, using this file's own state.
        //  CHANGED: no MessageBox - a worker thread must not raise UI.
        // ====================================================================
        public string getfilenameandidwithhdid()
        {
            string filename_12 = string.Empty;
            using (SqlConnection con3 = new SqlConnection(connectionString))
            {
                con3.Open();
                var filename1 = new List<string>();
                using (SqlCommand cmd = new SqlCommand("usp_get_input_file_with_HDID", con3))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DataGenProcessHDID", lastInsertedId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) filename1.Add(reader["FilePath"].ToString());
                    }
                    if (filename1.Count == 0)
                        throw new InvalidOperationException(
                            $"No input file found for DataGenProcessHDID {lastInsertedId}");
                    filename_12 = filename1[0];
                }
            }
            return filename_12;
        }

        // CHANGED: takes the HDID, not ProfileID. The original looked the file
        // up by profile, which returns the SAME file for every HDID in a lot -
        // harmless sequentially, wrong the moment files run in parallel.
        public string getfilenameandid()
        {
            return getfilenameandidwithhdid();
        }

        public string eid_function(int i)
        {
            string data = "890440458660999902";
            data = data + (eid_db + i).ToString().PadLeft(12, '0');
            long sum = 0;
            foreach (char c in data) sum += c - '0';
            int checksum = (int)(sum % 100);
            data = data + checksum.ToString("D2");
            EID_db_last = eid_db + i;
            return data;
        }

        // CHANGED: one shared HttpClient instead of new HttpClient() per call
        // (that pattern exhausts sockets under load).
        private static readonly HttpClient EuiccClient =
            new HttpClient { Timeout = TimeSpan.FromSeconds(60) };

        public string euicc_Data_function(string cert_name, string eum_cert_name, int i, int attempt)
        {
            const int maxAttempts = 10;
            string url = $"http://{euicc_hsm_IP.Trim()}/api/HsmCa/operate";
            try
            {
                // CHANGED: user_pin was hardcoded as "12345678" in the source.
                // Move it to DataTool_Keys and pass it in.
                var jsonBody = $@"{{
                    ""slot_id"": 0,
                    ""user"": ""user"",
                    ""user_pin"": ""{EuiccUserPin}"",
                    ""root_ca_cert_name"": ""{cert_name}"",
                    ""sub_ca_cert_name"": ""{eum_cert_name}"",
                    ""cert_name"": ""EUICC_DATA_CERT_{i}"",
                    ""operation_type"": 6
                }}";
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = EuiccClient.PostAsync(url, content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                if (attempt < maxAttempts)
                {
                    Thread.Sleep(2000);
                    return euicc_Data_function(cert_name, eum_cert_name, i, attempt + 1);
                }
                return $"ERROR: {ex.Message}";
            }
        }

        /// <summary>HSM PIN. Load this from DataTool_Keys, do not hardcode.</summary>
        public static string EuiccUserPin { get; set; } = "";

        public void UpdateInputFileSatus(string processFor, int fileID_1)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE DataGenProcessHD SET DataGenProcessStatus = 5 WHERE DataGenProcessHDID = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", fileID_1);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int All_Record_Processing(int currentFileIndex, int totalFiles)
        {
            int hsm_flag = 1;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string r4_data = "", r8_data = "";
            int r4_data_count = 0, r8_data_count = 0;
            List<string> r4_data_list = new List<string>();
            List<string> r8_data_list = new List<string>();
            string constr = EncryptionandDecryption.DecryptString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlConnection con = new SqlConnection(constr);
            string filename_2 = getfilenameandidwithhdid();
            //for reliance jio 
            string prefix = rjio_prefix;
            List<string> generatedCodes = new List<string>();
            BatchCodeGenerator generatormsn = new BatchCodeGenerator(500);
            BatchCodeGenerator generatormsc = new BatchCodeGenerator(5000);
            //-----------------------------------------------------------------------------
            if (customer_name_form.ToUpper() == "RELIANCE")
            {
                string query = @"SELECT TOP 1 LastGeneratedCodeMSN, LastGeneratedCodeMSC
         FROM Reliance_CounterTracker 
         WHERE Prefix = @prefix order by id desc";
                SqlCommand cmd_rjio = new SqlCommand(query, con);
                cmd_rjio.Parameters.AddWithValue("@prefix", prefix);
                SqlDataAdapter adpt_rjio = new SqlDataAdapter(cmd_rjio);
                DataTable dt_rjio = new DataTable();
                adpt_rjio.Fill(dt_rjio);
                if (dt_rjio.Rows.Count > 0)
                {
                    lastCodeMSN = dt_rjio.Rows[0]["LastGeneratedCodeMSN"]?.ToString();
                    lastCodeMSC = dt_rjio.Rows[0]["LastGeneratedCodeMSC"]?.ToString();
                }
                firstCodemsn = generatormsn.LoadLastCodemsn(lastCodeMSN);
                firstCodemsc = generatormsc.LoadLastCodemsc(lastCodeMSC);
            }
            logString.Append($"    - [{Path.GetFileName(filename_2)}] File Processing Started.\n");
            Console.WriteLine($"    - [{Path.GetFileName(filename_2)}] File Processing Started.\n");
            String Query1 = $"SELECT DataGenProcessData.[DataGenProcessDataID], DataGenProcessData.[DataGenProcessHDID], InPutDataTemplate.[VarName] as VarID, DataGenProcessData.[VarName], DataGenProcessData.[VarValue], DataGenProcessData.[VarType], DataGenProcessData.[StatusID],[InPutDataTemplate].algoname,[InPutDataTemplate].VarText,[InPutDataTemplate].PositionFrom,[InPutDataTemplate].Len,InPutDataTemplate.tag, [InPutDataTemplate].LineNumber  ,[InPutDataTemplate].VarWay    FROM DataGenProcessData inner JOIN [InPutDataTemplate] ON [InPutDataTemplate].vardes = DataGenProcessData.varname where  DataGenProcessData.[DataGenProcessHDID] = '" + lastInsertedId + "' and [InPutDataTemplate].ProfileID=" + ProfileID + "  order by DataGenProcessData.VarID ";
            System.Data.DataTable dt1 = new System.Data.DataTable();
            DataRow workRow1;
            SqlCommand sqlcom1 = new SqlCommand(Query1, con);
            SqlDataAdapter adpt1 = new SqlDataAdapter(Query1, con);
            adpt1.Fill(dt1);
            r4_data_list.Clear();
            r8_data_list.Clear();
            Process_data = new DataTable();
            DataColumn idColumn = new DataColumn("DataGenProcessDataRecordID", typeof(int));
            idColumn.AutoIncrement = true;
            idColumn.AutoIncrementSeed = 1;
            Process_data.Columns.Add(idColumn);
            Process_data.Columns.Add("DataGenProcessHDID", typeof(int)).DefaultValue = lastInsertedId;
            string[] default_values = { "AGSUI:IMSI", "EKI", "KIND", "DATE_AL", "FSETIND", "A4IND", "Transport_key", "Quantity", "Index_Value", "BatchNumber" };
            foreach (DataRow dv0 in dt1.Rows)
            {
                string var_ID = dv0[2].ToString().TrimEnd();
                string var_name = dv0[3].ToString().TrimEnd();
                string var_Value = dv0[4].ToString().TrimEnd();
                string var_Type = dv0[5].ToString().TrimEnd();
                string var_text_fl = dv0[8].ToString().TrimEnd();
                if (var_name.TrimEnd() == "Quantity")
                {
                    records = Int32.Parse(var_Value.TrimEnd());
                    Total_no_of_records += records;
                }
                if (var_Type == "T" && !var_name.ToLower().Contains("header"))
                {
                    Process_data.Columns.Add(var_ID, typeof(string)).DefaultValue = var_Value;
                }
                else
                {
                    if (default_values.Contains(var_name.TrimEnd()))
                    {
                        Process_data.Columns.Add(var_ID, typeof(string)).DefaultValue = var_Value;
                    }
                    else
                    {
                        if (var_name.TrimEnd() == "ICCID" || var_name.TrimEnd() == "IMSI")
                        {
                            Process_data.Columns.Add(var_ID, typeof(string));
                        }
                        else if (var_name.TrimEnd() == "MSISDN")
                        {
                            if (var_Value.Contains('F'))
                            {
                                Process_data.Columns.Add(var_ID, typeof(string)).DefaultValue = var_Value;
                            }
                            else
                            {
                                Process_data.Columns.Add(var_ID, typeof(string));
                            }
                        }
                        else
                        {
                            Process_data.Columns.Add(var_ID, typeof(string));
                        }
                    }
                }
            }
            logString.Append($"    - Establishing HSM Connection.\n");
            Console.WriteLine($"    - Establishing HSM Connection.\n");
            string url_1 = $"http://{hsm_IP.Trim()}/api/HSM?op_type=ip&digits=%27%27&keyname=%27%27&ip_data=%27%27";
            WebRequest request_1 = HttpWebRequest.Create(url_1);
            WebResponse response_1 = request_1.GetResponse();
            StreamReader reader_1 = new StreamReader(response_1.GetResponseStream());
            string urlText_1 = reader_1.ReadToEnd();
            if (urlText_1.Contains("CKR_DEVICE_REMOVED"))
            {
                logString.Append(" HSM CONNECTIVITY LOST Please check with Key Manager");
                Console.WriteLine($" HSM CONNECTIVITY LOST Please check with Key Manager");
                hsm_flag = 0;
            }
            if (hsm_flag == 0)
            {
                logString.AppendLine("\nFile processing stopped.");
                return 10;
            }
            var hsm_data_1 = urlText_1.Split(',');
            DataTable fl_data = new DataTable();
            using (SqlConnection connnection = new SqlConnection(connectionString))
            {
                connnection.Open();
                using (SqlCommand cmd = new SqlCommand($"SELECT IMSI,ICICID,MSISDN,LICENSE_KEY FROM [DGPDR_Base] WHERE DataGenProcessHDID={lastInsertedId} order by Sr_no ", connnection))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(fl_data);
                }
            }
            List<int> middleValues = dt1.AsEnumerable()
                            .Where(row => !string.IsNullOrEmpty(row.Field<string>("algoname")) && row.Field<string>("algoname").Contains("Hex") && row.Field<string>("algoname").Contains("R_"))
                            .Select(row =>
                            {
                                string[] parts = row.Field<string>("algoname").Split('_');
                                if (parts.Length > 1 && int.TryParse(parts[1], out int value))
                                    return value / 2;
                                else
                                    return 0;
                            })
                            .ToList();
            List<string> varIDS = dt1.AsEnumerable()
                                  .Where(row => !string.IsNullOrEmpty(row.Field<string>("algoname")) && row.Field<string>("algoname").Contains("Hex") && row.Field<string>("algoname").Contains("R_"))
                                  .Select(row => row.Field<string>("VarID").Trim())
                                  .ToList();
            string filename_input_file = getfilenameandid();
            string[] data_val_line = File.ReadAllLines(filename_input_file);

            // ========================================================================
            // >>> PARALLEL: setup that did not exist in the sequential version
            // ========================================================================

            // Degree of parallelism. Start LOW (4-8) and raise it while watching the
            // HSM. This is the only knob you should need to tune.
            int maxDop = innerDop;

            // Row payloads, indexed by (i - 1) so final insertion order == request order.
            object[][] rowData = new object[records][];

            // Failure reporting out of the worker threads. A worker cannot "return",
            // and it must not raise a MessageBox, so it records and stops the loop.
            int failCode = 0;                       // 0 = OK, else the code to return
            string failMessage = null;
            object failLock = new object();
            object logLock = new object();          // logString (StringBuilder) is not thread-safe
            int completed = 0;                      // progress counter

            Action<int, string, ParallelLoopState> Fail = (code, msg, st) =>
            {
                lock (failLock)
                {
                    if (failCode == 0) { failCode = code; failMessage = msg; }
                }
                st.Stop();
            };

            // >>> PARALLEL: RJIO codes come from a stateful sequential generator.
            // Called from parallel workers, row i would not receive code i. So every
            // code is generated up-front, in order, and simply looked up by row index.
            string[] msnCodes = null, mscCodes = null;
            bool usesMsn = dt1.AsEnumerable().Any(r => (r.Field<string>("algoname") ?? "").Trim() == "RJIO_MSN");
            bool usesMsc = dt1.AsEnumerable().Any(r => (r.Field<string>("algoname") ?? "").Trim() == "RJIO_MSC");
            if (customer_name_form.ToUpper() == "RELIANCE")
            {
                if (usesMsn)
                {
                    msnCodes = new string[records];
                    for (int k = 0; k < records; k++) msnCodes[k] = generatormsn.Generatemsn(prefix);
                    lastCodeMSN = msnCodes[records - 1];
                }
                if (usesMsc)
                {
                    mscCodes = new string[records];
                    for (int k = 0; k < records; k++) mscCodes[k] = generatormsc.GenerateMSC(prefix);
                    lastCodeMSC = mscCodes[records - 1];
                }
            }

            // ========================================================================
            // >>> PARALLEL: the record loop
            //   thread-local state = a private Clone() of Process_data, so each worker
            //   calls NewRow() on its OWN DataTable. Nothing writes to Process_data
            //   until after the loop.
            // ========================================================================
            Parallel.For(1, records + 1,
                new ParallelOptions { MaxDegreeOfParallelism = maxDop },
                () => Process_data.Clone(),
                (i, loopState, localTable) =>
                {
                    Dictionary<string, string> FetchAPI = new Dictionary<string, string>();
                    string euicc_ci_cert_data = "", euicc_eum_cert_data = "", euicc_pri_wrapped_key = "", euicc_pub_key_value = "";

                    // >>> PARALLEL: urlText was a method-level variable shared by all
                    // iterations. Now loop-local. Every branch below assigns it, so
                    // this is behaviourally identical.
                    string urlText = "";

                    // >>> PARALLEL: r8 list/counter were method-level. Tracing the
                    // sequential code, R8_P always consumes the value R_8 produced in
                    // the SAME row, so these become row-local with identical results.
                    List<string> r8_data_list_row = new List<string>();
                    int r8_data_count_row = 0;

                    if (records > 500)
                    {
                        if (i % (records / 500) == 0)
                        {
                            string url = $"http://{hsm_IP.Trim()}/api/HSM?op_type=RND&digits={string.Join(",", middleValues)}&keyname=%27%27&ip_data=%27%27";
                            WebRequest request = HttpWebRequest.Create(url);
                            WebResponse response = request.GetResponse();
                            StreamReader reader = new StreamReader(response.GetResponseStream());
                            urlText = reader.ReadToEnd() + "rem";
                            if (urlText.Contains("CKR_DEVICE_REMOVED"))
                            {
                                lock (logLock) logString.Append(" HSM CONNECTIVITY LOST Please check with Key Manager");
                                Console.WriteLine($" HSM CONNECTIVITY LOST Please check with Key Manager");
                                Fail(10, null, loopState);
                                return localTable;
                            }
                        }
                        else
                        {
                            urlText = SecureRandomHex(string.Join(",", middleValues)) + "rem";   /* CHANGED: was System.Random */
                        }
                    }
                    else if (records > 10)
                    {
                        if (i % (records / 10) == 0)
                        {
                            string url = $"http://{hsm_IP.Trim()}/api/HSM?op_type=RND&digits={string.Join(",", middleValues)}&keyname=%27%27&ip_data=%27%27";
                            WebRequest request = HttpWebRequest.Create(url);
                            WebResponse response = request.GetResponse();
                            StreamReader reader = new StreamReader(response.GetResponseStream());
                            urlText = reader.ReadToEnd() + "rem";
                            if (urlText.Contains("CKR_DEVICE_REMOVED"))
                            {
                                lock (logLock) logString.Append(" HSM CONNECTIVITY LOST Please check with Key Manager");
                                Console.WriteLine($" HSM CONNECTIVITY LOST Please check with Key Manager");
                                Fail(10, null, loopState);
                                return localTable;
                            }
                        }
                        else
                        {
                            urlText = SecureRandomHex(string.Join(",", middleValues)) + "rem";   /* CHANGED: was System.Random */
                        }
                    }
                    else
                    {
                        string url = $"http://{hsm_IP.Trim()}/api/HSM?op_type=RND&digits={string.Join(",", middleValues)}&keyname=%27%27&ip_data=%27%27";
                        WebRequest request = HttpWebRequest.Create(url);
                        WebResponse response = request.GetResponse();
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        urlText = reader.ReadToEnd() + "rem";
                        if (urlText.Contains("CKR_DEVICE_REMOVED"))
                        {
                            lock (logLock) logString.Append(" HSM CONNECTIVITY LOST Please check with Key Manager");
                            Console.WriteLine($" HSM CONNECTIVITY LOST Please check with Key Manager");
                            Fail(10, null, loopState);
                            return localTable;
                        }
                    }

                    urlText = urlText.Replace(",rem", "");
                    var data = urlText.Split(',');
                    int j = 0;
                    try
                    {
                        foreach (string var in varIDS)
                        {
                            FetchAPI[var] = data[j];
                            j++;
                        }
                    }
                    catch (Exception ex)
                    {
                        // >>> PARALLEL: was MessageBox.Show + return 10
                        Fail(10, "HSM is not on. Please check with Key Manager.\n\nError Details: " + ex.Message, loopState);
                        return localTable;
                    }

                    // >>> PARALLEL: NewRow() on this worker's own cloned table
                    DataRow newRow = localTable.NewRow();
                    string my_data = "";
                    foreach (DataRow dv0 in dt1.Rows)
                    {
                        try
                        {
                            string var_ID = dv0[2].ToString().TrimEnd();
                            string var_name = dv0[3].ToString().TrimEnd();
                            string var_Value = dv0[4].ToString().TrimEnd();
                            string var_algoname = dv0[7].ToString().TrimEnd();
                            string var_op_type = dv0[8].ToString().TrimEnd();
                            string Pos_From = dv0[9].ToString().TrimEnd();
                            string len_data = dv0[10].ToString().TrimEnd();
                            string variableID = dv0[11].ToString().TrimEnd();
                            int lineno = int.TryParse(Convert.ToString(dv0[12]).Trim(), out int temp) ? temp : 0;
                            string var_Type = dv0[13].ToString().TrimEnd();
                            int varCount = variableID.Count(c => c == ',');
                            my_data = "";
                            if (var_op_type == "FL")
                            {
                                if (var_name.ToUpper() == "ICCID")
                                {
                                    my_data = fl_data.Rows[i - 1]["ICICID"].ToString();
                                }
                                else if (var_name.ToUpper() == "MSISDN")
                                {
                                    my_data = fl_data.Rows[i - 1]["MSISDN"].ToString();
                                }
                                else if (var_name.ToUpper() == "IMSI")
                                {
                                    my_data = fl_data.Rows[i - 1]["IMSI"].ToString();
                                }
                                else if (var_name.ToUpper() == "LICENSE_KEY")
                                {
                                    my_data = fl_data.Rows[i - 1]["LICENSE_KEY"].ToString();
                                    if (string.IsNullOrEmpty(my_data))
                                    {
                                        my_data = var_Value.Trim();
                                    }
                                }
                                else if (var_name.ToUpper() == "BATCHNUMBER")
                                {
                                    my_data = var_Value.Trim();
                                    batchnumber_file = my_data;   // same value every row; benign race
                                }
                                else if (var_Type == "T" && !var_name.ToLower().Contains("header"))
                                {
                                    my_data = var_Value.Trim();
                                }
                                else if (lineno != 0 && !string.IsNullOrEmpty(Pos_From) && !string.IsNullOrEmpty(len_data) && len_data != "0")
                                {
                                    int.TryParse(Pos_From?.Trim(), out int Pos_From1);
                                    int.TryParse(len_data?.Trim(), out int len_data1);
                                    try
                                    {
                                        my_data = data_val_line[lineno + i - 2].Substring(Pos_From1, len_data1);
                                    }
                                    catch
                                    {
                                        if (Pos_From1 < 0) Pos_From1 = 0;
                                        if (Pos_From1 > data_val_line[lineno + i - 2].Length) Pos_From1 = data_val_line[lineno + i - 2].Length;
                                        int safeLength = Math.Max(0, data_val_line[lineno + i - 2].Length - Pos_From1);
                                        my_data = safeLength > 0
                                            ? data_val_line[lineno + i - 2].Substring(Pos_From1, safeLength)
                                            : "";
                                        // >>> PARALLEL: was MessageBox.Show from inside the loop
                                        lock (logLock) logString.AppendLine($"position from and len is wrong for {var_name}");
                                    }
                                }
                                newRow[var_ID] = my_data;
                            }
                            //all records
                            if (var_op_type == "AL")
                            {
                                string caseSwitch = var_algoname;
                                string r4_data_row = "";
                                List<string> ki_val_list = new List<string>();
                                switch (caseSwitch)
                                {
                                    case "substring":
                                        if (varCount > 0)
                                        {
                                            Fail(100, $"More than one Variable found in AlgoName-{var_algoname}  in 'Tag' Value", loopState);
                                            return localTable;
                                        }
                                        else
                                        {
                                            int pos_from = Convert.ToInt32(Pos_From);
                                            int len = Convert.ToInt32(len_data);
                                            if (len == 0)
                                            { len = newRow[variableID].ToString().Trim().Length - pos_from + 1; }
                                            string data_new_test = newRow[variableID].ToString();
                                            my_data = newRow[variableID].ToString().Substring(pos_from - 1, len);
                                        }
                                        break;

                                    case "prefix":
                                        if (varCount == 1)
                                        {
                                            string[] varIDs = variableID.Split(',');
                                            my_data = (varIDs[0].ToString() + newRow[varIDs[1]].ToString());
                                        }
                                        else
                                        {
                                            Fail(100, $"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                            return localTable;
                                        }
                                        break;

                                    case "suffix":
                                        if (varCount == 1)
                                        {
                                            string[] varIDs = variableID.Split(',');
                                            my_data = (newRow[varIDs[0]].ToString() + varIDs[1].ToString());
                                        }
                                        else
                                        {
                                            Fail(100, $"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                            return localTable;
                                        }
                                        break;
                                    case "concat":
                                        if (varCount == 1)
                                        {
                                            string[] varIDs = variableID.Split(',');
                                            my_data = (newRow[varIDs[0]].ToString() + newRow[varIDs[1]].ToString());
                                        }
                                        else
                                        {
                                            Fail(100, $"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                            return localTable;
                                        }
                                        break;

                                    case "identical":
                                        if (varCount > 0)
                                        {
                                            Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                            return localTable;
                                        }
                                        else
                                        {
                                            my_data = newRow[variableID].ToString();
                                        }
                                        break;
                                    case "serial":
                                        my_data = i.ToString();
                                        break;
                                    case "R_4":
                                        if (i == 0) { my_data = var_Value; }
                                        else { my_data = SecureRandom4Digits(); }   /* CHANGED: was System.Random */
                                        break;
                                    case "R_8_Hex":
                                        if (i == 0) { my_data = var_Value; }
                                        else { my_data = SecureRandom8Hex(); }      /* CHANGED: was System.Random */
                                        break;
                                    case "R_8_H":
                                        if (i == 0) { my_data = var_Value; }
                                        else { my_data = SecureRandom8Hex(); }      /* CHANGED: was System.Random */
                                        break;
                                    case "R4_PF":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount > 0)
                                            {
                                                Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                            else
                                            {
                                                my_data = dg_functions.padding_filler(newRow[variableID].ToString());
                                            }
                                        }
                                        break;
                                    case "R_8":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                            r8_data_list_row.Add(dg_functions.padding(my_data));
                                        }
                                        else
                                        {
                                            my_data = SecureRandom8Digits();   /* CHANGED: was System.Random */
                                            r8_data_list_row.Add(dg_functions.padding(my_data));
                                        }
                                        break;
                                    case "R8_P":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                            r8_data_count_row += 1;
                                        }
                                        else
                                        {
                                            my_data = r8_data_list_row[r8_data_count_row];
                                            r8_data_count_row += 1;
                                        }
                                        break;
                                    case "ACC_Hex":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount > 0)
                                            {
                                                Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                            else
                                            {
                                                my_data = dg_functions.acc(newRow[variableID].ToString());
                                            }
                                        }
                                        break;
                                    case "Input_Filename":
                                        my_data = var_Value;
                                        break;
                                    case "YYYYMMDDHHMMSS":
                                        my_data = var_Value;
                                        break;
                                    case "3P":
                                        if (i == 0) { my_data = var_Value; }
                                        else { my_data = dg_functions.padding(newRow[variableID].ToString()); }
                                        break;
                                    case "FP":
                                        if (i == 0) { my_data = var_Value; }
                                        else { my_data = dg_functions.F_Padding(newRow[variableID].ToString(), int.Parse(len_data)); }
                                        break;
                                    case "HEX":
                                        if (i == 0) { my_data = var_Value; }
                                        else { my_data = dg_functions.StringToHex(newRow[variableID].ToString()); }
                                        break;
                                    case "R_16_Hex":
                                        if (i == 0) { my_data = var_Value; }
                                        else { my_data = FetchAPI[var_ID]; }
                                        break;
                                    case "R_32_Hex":
                                        if (i == 0) { my_data = var_Value; }
                                        else { my_data = FetchAPI[var_ID]; }
                                        break;
                                    case "R_48_Hex":
                                        if (i == 0) { my_data = var_Value; }
                                        else { my_data = FetchAPI[var_ID]; }
                                        break;
                                    case "Pad_8":
                                        if (i == 0) { my_data = var_Value; }
                                        else { my_data = dg_functions.Pad3_F(newRow[variableID].ToString()); }
                                        break;
                                    case "Pad_16":
                                        if (i == 0) { my_data = var_Value; }
                                        else { my_data = dg_functions.Pad3_F(newRow[variableID].ToString()); }
                                        break;
                                    case "ICCID_NS":
                                        if (varCount > 0)
                                        {
                                            Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                            return localTable;
                                        }
                                        else
                                        {
                                            my_data = newRow[variableID].ToString();
                                            my_data = dg_functions.nibble_swapped(my_data);
                                        }
                                        break;
                                    case "ICCID_NS_U":
                                        if (varCount > 0)
                                        {
                                            Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                            return localTable;
                                        }
                                        else
                                        {
                                            my_data = newRow[variableID].ToString();
                                            my_data = dg_functions.nibble_swapped_U(my_data);
                                        }
                                        break;
                                    case "DataOnesComplement":
                                        if (varCount > 0)
                                        {
                                            Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                            return localTable;
                                        }
                                        else
                                        {
                                            my_data = newRow[variableID].ToString();
                                            my_data = dg_functions.OnesComplementHex(my_data);
                                        }
                                        break;
                                    case "IMSI_NS":
                                        if (varCount > 0)
                                        {
                                            Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                            return localTable;
                                        }
                                        else
                                        {
                                            string imsi_num = "809" + newRow[variableID].ToString();
                                            my_data = dg_functions.nibble_swapped(imsi_num);
                                        }
                                        break;
                                    case "R_32_Hex_KI":
                                        if (i == 0) { my_data = var_Value; }
                                        else { my_data = FetchAPI[var_ID]; }
                                        ki_val_list.Add(my_data);
                                        break;
                                    case "ICCID_LD":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount > 0)
                                            {
                                                Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                            else
                                            {
                                                my_data = newRow[variableID].ToString();
                                                my_data += dg_functions.GetLuhnCheckDigit(newRow[variableID].ToString());
                                            }
                                        }
                                        break;
                                    case "ICCID_LD_U":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount > 0)
                                            {
                                                Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                            else
                                            {
                                                my_data = newRow[variableID].ToString();
                                                my_data += dg_functions.GetLuhnCheckDigit_U(newRow[variableID].ToString());
                                            }
                                        }
                                        break;
                                    case "KCV_AES":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount > 0)
                                            {
                                                Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                            else
                                            {
                                                my_data = dg_functions.CalculateKCV(newRow[variableID].ToString(), "AES");
                                            }
                                        }
                                        break;
                                    case "KCV_DES":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount > 0)
                                            {
                                                Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                            else
                                            {
                                                my_data = dg_functions.CalculateKCV(newRow[variableID].ToString(), "DES");
                                            }
                                        }
                                        break;
                                    case "SHA1":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount > 0)
                                            {
                                                Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                            else
                                            {
                                                my_data = dg_functions.ComputeHash(newRow[variableID].ToString(), "SHA1");
                                            }
                                        }
                                        break;
                                    case "SHA256":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount > 0)
                                            {
                                                Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                            else
                                            {
                                                my_data = dg_functions.ComputeHash(newRow[variableID].ToString(), "SHA256");
                                            }
                                        }
                                        break;
                                    case "SHA512":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount > 0)
                                            {
                                                Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                            else
                                            {
                                                my_data = dg_functions.ComputeHash(newRow[variableID].ToString(), "SHA512");
                                            }
                                        }
                                        break;
                                    case "SERIALNO":
                                        my_data = i.ToString();
                                        break;
                                    case "MCCMNC":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount > 0)
                                            {
                                                Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                            else
                                            {
                                                my_data = dg_functions.mncmcc_function(newRow[variableID].ToString(), Convert.ToInt32(len_data));
                                            }
                                        }
                                        break;
                                    case "DATE_FORMAT":
                                        my_data = var_Value;
                                        break;
                                    case "MSISDN_F":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount > 0)
                                            {
                                                Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                            else
                                            {
                                                my_data = newRow[variableID].ToString();
                                                my_data = dg_functions.MSISDN_F(my_data);
                                            }
                                        }
                                        break;
                                    case "NS":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount > 0)
                                            {
                                                Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                            else
                                            {
                                                my_data = newRow[variableID].ToString();
                                                my_data = dg_functions.nibble_swapped(my_data);
                                            }
                                        }
                                        break;
                                    case "KI_AES_128":
                                        if (varCount == 1)
                                        {
                                            string[] varIDs = variableID.Split(',');
                                            my_data = dg_functions.AES_ENCYPRTION(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                        }
                                        else
                                        {
                                            Fail(100, $"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                            return localTable;
                                        }
                                        break;
                                    case "Single_Des":
                                        if (varCount == 1)
                                        {
                                            string[] varIDs = variableID.Split(',');
                                            my_data = dg_functions.Encrypt_SingleDES(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                        }
                                        else
                                        {
                                            Fail(100, $"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                            return localTable;
                                        }
                                        break;
                                    case "AES_128":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount == 1)
                                            {
                                                string[] varIDs = variableID.Split(',');
                                                my_data = OPC_GEN.opc(newRow[varIDs[1]].ToString(), newRow[varIDs[0]].ToString());
                                            }
                                            else
                                            {
                                                Fail(100, $"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                        }
                                        break;
                                    case "Triple_Des_CBC":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount == 1)
                                            {
                                                string[] varIDs = variableID.Split(',');
                                                my_data = dg_functions.TripleDESEncrypt_cbc(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString());
                                            }
                                            else
                                            {
                                                Fail(100, $"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                        }
                                        break;
                                    case "AES256_ECB":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount == 1)
                                            {
                                                string[] varIDs = variableID.Split(',');
                                                my_data = dg_functions.EncryptAes256(newRow[varIDs[1]].ToString(), newRow[varIDs[0]].ToString(), "ECB");
                                            }
                                            else
                                            {
                                                Fail(100, $"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                        }
                                        break;
                                    case "AES256_CBC":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount == 1)
                                            {
                                                string[] varIDs = variableID.Split(',');
                                                my_data = dg_functions.EncryptAes256(newRow[varIDs[1]].ToString(), newRow[varIDs[0]].ToString(), "CBC");
                                            }
                                            else
                                            {
                                                Fail(100, $"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                        }
                                        break;
                                    case "euicc_cert_data":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            string[] varIDs = variableID.Split(',');
                                            string[] parts_list = Array.Empty<String>();
                                            string response_data = euicc_Data_function(varIDs[0], varIDs[1], i, 1);
                                            response_data = response_data.Replace("\"", "");
                                            if (response_data.Contains("CKR_GENERAL_ERROR"))
                                            {
                                                lock (logLock) logString.AppendLine($"**Error: HSM connectivity error");
                                                throw new Exception($"hsm connectivity ");
                                            }
                                            parts_list = response_data.Split(',');
                                            euicc_ci_cert_data = parts_list[1].Replace("root_ca_hex:", "").Trim();
                                            euicc_eum_cert_data = parts_list[2].Replace("sub_ca_hex:", "").Trim();
                                            euicc_pub_key_value = parts_list[4].Replace("public_key:", "").Trim();
                                            euicc_pri_wrapped_key = parts_list[6].Replace("wrapped_key:", "").Trim();
                                            my_data = parts_list[3].Replace("end_entity_hex:", "").Trim();
                                            break;
                                        }
                                        break;
                                    case "euicc_cert_ci":
                                        if (i == 0) { my_data = var_Value; } else { my_data = euicc_ci_cert_data; }
                                        break;
                                    case "euicc_pub_key":
                                        if (i == 0) { my_data = var_Value; } else { my_data = euicc_pub_key_value; }
                                        break;
                                    case "euicc_cert_eum":
                                        if (i == 0) { my_data = var_Value; } else { my_data = euicc_eum_cert_data; }
                                        break;
                                    case "euicc_pri_wrapped":
                                        if (i == 0) { my_data = var_Value; } else { my_data = euicc_pri_wrapped_key; }
                                        break;
                                    case "EID":
                                        if (i == 0) { my_data = var_Value; } else { my_data = eid_function(i); }
                                        break;
                                    case "AES_WRAP":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount == 1)
                                            {
                                                string[] varIDs = variableID.Split(',');
                                                my_data = dg_functions.AES_WRAP(newRow[varIDs[1]].ToString(), newRow[varIDs[0]].ToString());
                                            }
                                            else
                                            {
                                                Fail(100, $"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                        }
                                        break;

                                    case "AES128_ECB":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount == 1)
                                            {
                                                string[] varIDs = variableID.Split(',');
                                                my_data = dg_functions.AES128_ECB(newRow[varIDs[1]].ToString(), newRow[varIDs[0]].ToString());
                                            }
                                            else
                                            {
                                                Fail(100, $"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                        }
                                        break;

                                    case "AES128_AUTH":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount == 2)
                                            {
                                                string[] varIDs = variableID.Split(',');
                                                my_data = dg_functions.AES128_ECB(newRow[varIDs[0]].ToString(), newRow[varIDs[1]].ToString() + varIDs[2].ToString().TrimEnd());
                                            }
                                            else
                                            {
                                                Fail(100, $"{varCount + 1} variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                        }
                                        break;

                                    case "CHECKSUM":
                                        if (i == 0)
                                        {
                                            my_data = var_Value;
                                        }
                                        else
                                        {
                                            if (varCount > 0)
                                            {
                                                Fail(100, $"More than one Variable found in Algoname-{var_algoname}  in 'Tag' Value", loopState);
                                                return localTable;
                                            }
                                            else
                                            {
                                                my_data = dg_functions.CHECKSUM(newRow[variableID].ToString());
                                            }
                                        }
                                        break;
                                    case "RJIO_MSN":
                                        // >>> PARALLEL: pre-generated in row order before the loop
                                        if (customer_name_form.ToUpper() == "RELIANCE")
                                        {
                                            my_data = msnCodes[i - 1];
                                        }
                                        break;
                                    case "RJIO_MSC":
                                        // >>> PARALLEL: pre-generated in row order before the loop
                                        if (customer_name_form.ToUpper() == "RELIANCE")
                                        {
                                            my_data = mscCodes[i - 1];
                                        }
                                        break;
                                }
                                newRow[var_ID] = my_data;
                            }
                        }
                        catch (Exception ex)
                        {
                            lock (logLock) logString.AppendLine($"Error processing var_ID: {dv0[2].ToString().TrimEnd()}, Error: {ex.Message}\nStack Trace:\n + {ex.StackTrace}");
                            Fail(100, $"Error processing var_ID: {dv0[2].ToString().TrimEnd()}, Error: {ex.Message}\nStack Trace:\n + {ex.StackTrace}", loopState);
                            return localTable;
                        }
                    }

                    // >>> PARALLEL: was Process_data.Rows.Add(newRow).
                    // Store the values at this row's index; the real table is filled
                    // in order after the loop, so record order is preserved exactly.
                    object[] ia = newRow.ItemArray;
                    ia[0] = i;                       // DataGenProcessDataRecordID = 1..records
                    rowData[i - 1] = ia;

                    Interlocked.Increment(ref completed);
                    return localTable;
                },
                localTable => { /* nothing to merge - rowData already holds everything */ });

            // >>> PARALLEL: a worker asked to abort. Report on the UI thread.
            if (failCode != 0)
            {
                if (!string.IsNullOrEmpty(failMessage))
                    LastError = failMessage;
                if (failCode == 10)
                {
                    hsm_flag = 0;
                    logString.AppendLine("\nFile processing stopped.");
                }
                return failCode;
            }

            // >>> PARALLEL: fill the real table, in index order
            for (int k = 0; k < records; k++)
            {
                if (rowData[k] != null)
                    Process_data.Rows.Add(rowData[k]);
            }


            // ========================================================================
            //  everything below is the original code, unchanged
            // ========================================================================
            if (hsm_flag == 0)
            {
                logString.AppendLine("\nFile processing stopped.\n");
                return 10;
            }
            else
            {
                logString.Append($"    - HSM Connection establised HSM PC IP is {hsm_data_1[0]}   ,  MAC-Address  is {hsm_data_1[1]} \n");
                logString.Append($"    - Closing HSM Connection.\n");
                try
                {
                    con.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.TableLock, null))
                    {
                        bulkCopy.DestinationTableName = "DataGenProcessDataRecord";
                        foreach (DataColumn col in Process_data.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName.Trim());
                        }
                        bulkCopy.BulkCopyTimeout = 12000;
                        bulkCopy.BatchSize = 10000;   // CHANGED: SqlBulkCopy batches internally
                        bulkCopy.EnableStreaming = true;

                        // CHANGED: removed the Clone()/ImportRow() loop - it copied all
                        // 50k rows a second time before a single row reached SQL Server.
                        bulkCopy.WriteToServer(Process_data);

                        UpdateInputFileSatus("Input_File", lastInsertedId);
                        Process_data.Clear();
                    }
                    con.Close();
                    stopwatch.Stop();
                    Console.WriteLine($"Filename : {Path.GetFileName(filename_2)}\nTime Taken : {stopwatch.Elapsed.ToString()}");
                    logString.Append($"    - Processed [{Path.GetFileName(filename_2)}] with FileID : {lastInsertedId} with Total no of records : {records}\n");
                    Console.WriteLine($"    - Processed [{Path.GetFileName(filename_2)}] with FileID : {lastInsertedId} with Total no of records : {records}\n");
                    if (customer_name_form.ToUpper() == "RELIANCE")
                    {
                        using (SqlConnection con_reliance = new SqlConnection(connectionString))
                        {
                            con_reliance.Open();
                            using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Reliance_CounterTracker 
        (Prefix, LastGeneratedCodemsn,LastGeneratedCodemsc,FirstGeneratedCodeMSN,FirstGeneratedCodeMSC,InputFilename,TKkeyUsed,OPkeyUsed)
    VALUES (@prefix,@codemsn,@codemsc,@firstCodemsn,@firstCodemsc,@InputFilename,@TKkeyUsed,@OPkeyUsed)", con_reliance))
                            {
                                cmd.Parameters.AddWithValue("@prefix", prefix);
                                cmd.Parameters.AddWithValue("@codemsn", lastCodeMSN);
                                cmd.Parameters.AddWithValue("@codemsc", lastCodeMSC);
                                cmd.Parameters.AddWithValue("@firstCodemsn", firstCodemsn);
                                cmd.Parameters.AddWithValue("@firstCodemsc", firstCodemsc);
                                cmd.Parameters.AddWithValue("@InputFilename", Path.GetFileName(filename_2));
                                cmd.Parameters.AddWithValue("@TKkeyUsed", reliance_tranport_key);
                                cmd.Parameters.AddWithValue("@OPkeyUsed", reliance_op_key);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    logString.Append($"    - Error occurred during bulk copy.\n Column mapping failed" + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
                    Console.WriteLine($"    - Error occurred during bulk copy.\n Column mapping failed" + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
                    LastError = "Bulk copy failed: " + ex.Message;   // CHANGED: no MessageBox from a worker
                    using (SqlConnection del_con = new SqlConnection(connectionString))
                    {
                        del_con.Open();
                        SqlDataReader reader = null;
                        using (SqlCommand cmd = new SqlCommand("Delete FROM [dbo].[DataGenProcessDataRecord] WHERE DataGenProcessHDID=@hdid", del_con))
                        {
                            cmd.Parameters.AddWithValue("@hdid", lastInsertedId);
                            int rowsAffected = cmd.ExecuteNonQuery();
                        }
                        del_con.Close();
                    }
                    return 0;
                }
            }
        }
    }   // end class FileProcessor


    // ========================================================================
    //  FileResult - what happened to ONE file.
    //  This is what gives you per-file error messages.
    // ========================================================================
    public class FileResult
    {
        public int Hdid { get; set; }
        public string FileName { get; set; }
        public int Index { get; set; }   // position in the batch, 1-based
        public int ReturnCode { get; set; }   // 1 ok | 10 HSM | 100 config | 0 bulk copy
        public int RecordCount { get; set; }
        public TimeSpan Duration { get; set; }
        public string ErrorText { get; set; }
        public string StackTrace { get; set; }
        public string Log { get; set; }

        public bool Succeeded { get { return ReturnCode == 1; } }

        public string Status
        {
            get
            {
                switch (ReturnCode)
                {
                    case 1: return "OK";
                    case 10: return "HSM CONNECTIVITY LOST";
                    case 100: return "DATA CONFIGURATION ERROR";
                    case 0: return "BULK COPY FAILED";
                    default: return "UNKNOWN (" + ReturnCode + ")";
                }
            }
        }
    }
}
