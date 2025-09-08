using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DG_Tool.WinForms.OutputFile
{
       
    class OPC_GEN
    {
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }
        public static byte[] StrToByteArray(string hex)
        {
            int NumberChars = hex.Length / 2;
            byte[] bytes = new byte[NumberChars];
            using (var sr = new StringReader(hex))
            {
                for (int i = 0; i < NumberChars; i++)
                    bytes[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
            }
            return bytes;
        }
        public static byte[] StrToByteArray_1(string str)
        {
            //MessageBox.Show(str);
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);

            List<byte> hexres = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
                hexres.Add(hexindex[str.Substring(i, 2)]);

            return hexres.ToArray();
        }
        private static byte[] Encrypt(byte[] data, byte[] Key)
        {
            byte[] buffer;
            byte[] rgbIV = new byte[Key.Length];
            using (Aes aes = Aes.Create())
            {
                aes.Padding = PaddingMode.Zeros;
                ICryptoTransform transform = aes.CreateEncryptor(Key, rgbIV);
                // disable once SuggestUseVarKeywordEvident
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                    {
                        stream2.Write(data, 0, data.Length);
                        stream2.FlushFinalBlock();
                        buffer = stream.ToArray();
                    }
                }
            }
            return buffer;
        }
        public static string setOpc(byte[] OP, byte[] key)
        {
            byte[] opc;
            //byte[] numPtr1;
            var numPtr1 = new byte[32];
            opc = Encrypt(OP, key);
            StringBuilder hex_new = new StringBuilder(50);
            string hex_1 = ByteArrayToString(opc);
            //Console.WriteLine(hex_1);
            for (byte i = 0; i < 0x10; i = (byte)(i + 1))
            {
                numPtr1[0] = (byte)opc[i];
                numPtr1[0] = (byte)(numPtr1[0] ^ OP[i]);
                hex_new.AppendFormat("{0:X2}", numPtr1[0]);
                //string hex_2 =ByteArrayToString(numPtr1);
            }
            //Console.WriteLine($"opc : "+hex_new.ToString());
            return hex_new.ToString();
        }
        public static string opc(string op_value, string ki_value)
        {
            //MessageBox.Show(op_value + " " + ki_value);
            //Console.WriteLine(op_value + " " + ki_value);
            var ki = StrToByteArray(ki_value);
            //Console.Write(op);
            var op = StrToByteArray(op_value);
            //string result_1 = System.Text.Encoding.UTF8.GetString(op);
            //string hex = ByteArrayToString(op);
            //Console.WriteLine(hex);

            string opc = setOpc(op, ki);
            //MessageBox.Show(opc);
            return opc;

        }


        public static string Encrypt_KI(string toEncrypt, string tk_key, bool useHashing)
        {
            //MessageBox.Show(toEncrypt + " : " + tk_key);

            byte[] keyArray;
            byte[] toEncryptArray = StrToByteArray(toEncrypt);


            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file

            //string key = (string)settingsReader.GetValue("SecurityKey",typeof(String));
            string key = tk_key;

            //System.Windows.Forms.MessageBox.Show(key);
            //System.Windows.Forms.MessageBox.Show(key);

            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(StrToByteArray(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                //keyArray = StrToByteArray(key.PadRight(32, 'F'));
                keyArray = StrToByteArray(key);
            //foreach Byte in keyArray()

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
                cTransform.TransformFinalBlock(toEncryptArray, 0,
                    toEncryptArray.Length);
            //			Console.WriteLine(toEncryptArray.Length);

            //Console.WriteLine(resultArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return ByteArrayToString(resultArray).Substring(0, 32);
        }
    }
}
