using DG_Tool.WinForms.OutputFile;
using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DG_Tool
{
    public partial class Testing : Form
    {
        public Testing()
        {
            InitializeComponent();
        }
        public string triple_Des(string toEncrypt, string tk_key)
        {
            byte[] key = OPC_GEN.StrToByteArray(tk_key);
            byte[] data = OPC_GEN.StrToByteArray(toEncrypt);
            string result;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor();
                byte[] encryptedBytes = encryptor.TransformFinalBlock(data, 0, data.Length);
                ICryptoTransform decryptor = aesAlg.CreateDecryptor();
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                string encryptedHex = BitConverter.ToString(encryptedBytes).Replace("-", "").Substring(0, 32);
                string decryptedHex = BitConverter.ToString(decryptedBytes).Replace("-", "");
                result = encryptedHex;
            }
            return result;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string data = triple_Des("db4389530b991a10b557246db732cd9a", "0B1CAFBAAB398929D6C7B8C868128226");

            richTextBox1.Text = data;
        }
    }
}
