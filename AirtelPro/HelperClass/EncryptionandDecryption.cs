using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Security;
using System;using CardPrintingApplication;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CardPrintingApplication
{
    internal class EncryptionandDecryption
    {
        public static string DecryptString(string cipherText)
        {
            string EncryptionKey = "XCgMNAvzSA3q+OkIEDf+8Q==";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public static string Encrypt(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";  //we can change the code converstion key as per our requirement    
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";  //we can change the code converstion key as per our requirement, but the decryption key should be same as encryption key    
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
		public static void EncryptFile(string inputFile, string outputFile,string password)
		{

			try
			{
				UnicodeEncoding UE = new UnicodeEncoding();
				byte[] key = UE.GetBytes(password);

				string cryptFile = outputFile;
				FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

				RijndaelManaged RMCrypto = new RijndaelManaged();

				CryptoStream cs = new CryptoStream(fsCrypt,
					RMCrypto.CreateEncryptor(key, key),
					CryptoStreamMode.Write);

				FileStream fsIn = new FileStream(inputFile, FileMode.Open);

				int data;
				while ((data = fsIn.ReadByte()) != -1)
					cs.WriteByte((byte)data);


				fsIn.Close();
				cs.Close();
				fsCrypt.Close();
			}
			catch
			{
				Console.WriteLine($"Error");
			}
		}
		///<summary>
		/// Steve Lydford - 12/05/2008.
		///
		/// Decrypts a file using Rijndael algorithm.
		///</summary>
		///<param name="inputFile"></param>
		///<param name="outputFile"></param>
		public static void DecryptFile(string inputFile, string outputFile,string password)
		{
			{
				UnicodeEncoding UE = new UnicodeEncoding();
				byte[] key = UE.GetBytes(password);

				FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

				RijndaelManaged RMCrypto = new RijndaelManaged();

				CryptoStream cs = new CryptoStream(fsCrypt,
					RMCrypto.CreateDecryptor(key, key),
					CryptoStreamMode.Read);

				FileStream fsOut = new FileStream(outputFile, FileMode.Create);

				int data;
				while ((data = cs.ReadByte()) != -1)
					fsOut.WriteByte((byte)data);

				fsOut.Close();
				cs.Close();
				fsCrypt.Close();

			}
		}

        public static string AESEncrypt_File(string inputFile, string key)
        {
            try
            {
                string out_file_name = "";
                FileInfo fi = new FileInfo(inputFile);

                // Get File Name
                string justFileName = fi.Name;
                //MessageBox.Show(justFileName);

                // Get file name with full path
                string fullFileName = fi.FullName;
                // MessageBox.Show(fullFileName);

                // Get file extension
                string extn = fi.Extension;
                // MessageBox.Show(extn);

                // Get directory name
                string directoryName = fi.DirectoryName;
                //MessageBox.Show(directoryName);
                //foreach (string inputFile in inputFiles)
                //{
                string encryptionKey = key;
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Padding = PaddingMode.PKCS7;
                    aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
                    aesAlg.GenerateIV();
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))

                    using (FileStream fsEncrypted = new FileStream(inputFile.Replace(extn, $"_{extn.Substring(1, extn.Length - 1)}.haes"), FileMode.Create))
                    using (CryptoStream csEncrypt = new CryptoStream(fsEncrypted, encryptor, CryptoStreamMode.Write))
                    {
                        // Write the IV to the beginning of the encrypted file
                        fsEncrypted.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        while ((bytesRead = fsInput.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            csEncrypt.Write(buffer, 0, bytesRead);
                        }
                    }

                }
                out_file_name = inputFile.Replace(extn, $"_{extn.Substring(1, extn.Length - 1)}.haes");
                File.Delete(inputFile);
                return out_file_name;
            }
            catch (Exception ex) { return "Error.file"; }
            //}
        }

        //Implemented PGP Encryption and Decryption
        public class Pgp
        {
            public static void DecryptFile(
                string inputFileName,
                string keyFileName,
                char[] passwd,
                string defaultFileName)
            {
                using (Stream input = File.OpenRead(inputFileName),
                       keyIn = File.OpenRead(keyFileName))
                {
                    DecryptFile(input, keyIn, passwd, defaultFileName);
                }
            }

            /**
             * decrypt the passed in message stream
             */
            private static void DecryptFile(
                Stream inputStream,
                Stream keyIn,
                char[] passwd,
                string defaultFileName)
            {
                inputStream = PgpUtilities.GetDecoderStream(inputStream);

                try
                {
                    PgpObjectFactory pgpF = new PgpObjectFactory(inputStream);
                    PgpEncryptedDataList enc;

                    PgpObject o = pgpF.NextPgpObject();
                    //
                    // the first object might be a PGP marker packet.
                    //
                    if (o is PgpEncryptedDataList)
                    {
                        enc = (PgpEncryptedDataList)o;
                    }
                    else
                    {
                        enc = (PgpEncryptedDataList)pgpF.NextPgpObject();
                    }

                    //
                    // find the secret key
                    //
                    PgpPrivateKey sKey = null;
                    PgpPublicKeyEncryptedData pbe = null;
                    PgpSecretKeyRingBundle pgpSec = new PgpSecretKeyRingBundle(
                        PgpUtilities.GetDecoderStream(keyIn));

                    foreach (PgpPublicKeyEncryptedData pked in enc.GetEncryptedDataObjects())
                    {
                        sKey = PgpExampleUtilities.FindSecretKey(pgpSec, pked.KeyId, passwd);

                        if (sKey != null)
                        {
                            pbe = pked;
                            break;
                        }
                    }

                    if (sKey == null)
                    {
                        throw new ArgumentException("secret key for message not found.");
                    }

                    Stream clear = pbe.GetDataStream(sKey);

                    PgpObjectFactory plainFact = new PgpObjectFactory(clear);

                    PgpObject message = plainFact.NextPgpObject();

                    if (message is PgpCompressedData)
                    {
                        PgpCompressedData cData = (PgpCompressedData)message;
                        PgpObjectFactory pgpFact = new PgpObjectFactory(cData.GetDataStream());

                        message = pgpFact.NextPgpObject();
                    }

                    if (message is PgpLiteralData)
                    {
                        PgpLiteralData ld = (PgpLiteralData)message;

                        string outFileName = ld.FileName;
                        if (outFileName.Length == 0)
                        {
                            outFileName = defaultFileName;
                        }

                        Stream fOut = File.Create(outFileName);
                        Stream unc = ld.GetInputStream();
                        Streams.PipeAll(unc, fOut);
                        fOut.Close();
                    }
                    else if (message is PgpOnePassSignatureList)
                    {
                        throw new PgpException("encrypted message contains a signed message - not literal data.");
                    }
                    else
                    {
                        throw new PgpException("message is not a simple encrypted file - type unknown.");
                    }

                    if (pbe.IsIntegrityProtected())
                    {
                        if (!pbe.Verify())
                        {
                            Console.Error.WriteLine("message failed integrity check");
                        }
                        else
                        {
                            Console.Error.WriteLine("message integrity check passed");
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("no message integrity check");
                    }
                }
                catch (PgpException e)
                {
                    Console.Error.WriteLine(e);

                    Exception underlyingException = e.InnerException;
                    if (underlyingException != null)
                    {
                        Console.Error.WriteLine(underlyingException.Message);
                        Console.Error.WriteLine(underlyingException.StackTrace);
                    }
                }
            }

            

            public static void EncryptFile(
                string outputFileName,
                string inputFileName,
                string encKeyFileName,
                bool armor,
                bool withIntegrityCheck)
            {
                PgpPublicKey encKey = PgpExampleUtilities.ReadPublicKey(encKeyFileName);

                using (Stream output = File.Create(outputFileName))
                {
                    EncryptFile(output, inputFileName, encKey, armor, withIntegrityCheck);
                }
            }

            private static void EncryptFile(
                Stream outputStream,
                string fileName,
                PgpPublicKey encKey,
                bool armor,
                bool withIntegrityCheck)
            {
                if (armor)
                {
                    outputStream = new ArmoredOutputStream(outputStream);
                }

                try
                {
                    byte[] bytes = PgpExampleUtilities.CompressFile(fileName, CompressionAlgorithmTag.Zip);

                    PgpEncryptedDataGenerator encGen = new PgpEncryptedDataGenerator(
                        SymmetricKeyAlgorithmTag.Cast5, withIntegrityCheck, new SecureRandom());
                    encGen.AddMethod(encKey);

                    Stream cOut = encGen.Open(outputStream, bytes.Length);

                    cOut.Write(bytes, 0, bytes.Length);
                    cOut.Close();

                    if (armor)
                    {
                        outputStream.Close();
                    }
                }
                catch (PgpException e)
                {
                    Console.Error.WriteLine(e);

                    Exception underlyingException = e.InnerException;
                    if (underlyingException != null)
                    {
                        Console.Error.WriteLine(underlyingException.Message);
                        Console.Error.WriteLine(underlyingException.StackTrace);
                    }
                }
            }
        }
        public class PgpExampleUtilities
        {
            /**
             * Search a secret key ring collection for a secret key corresponding to keyID if it
             * exists.
             * 
             * @param pgpSec a secret key ring collection.
             * @param keyID keyID we want.
             * @param pass passphrase to decrypt secret key with.
             * @return
             * @throws PGPException
             * @throws NoSuchProviderException
             */
            internal static PgpPrivateKey FindSecretKey(PgpSecretKeyRingBundle pgpSec, long keyID, char[] pass)
            {
                PgpSecretKey pgpSecKey = pgpSec.GetSecretKey(keyID);

                if (pgpSecKey == null)
                {
                    return null;
                }

                return pgpSecKey.ExtractPrivateKey(pass);
            }

            internal static PgpPublicKey ReadPublicKey(string fileName)
            {
                using (Stream keyIn = File.OpenRead(fileName))
                {
                    return ReadPublicKey(keyIn);
                }
            }

            internal static PgpPublicKey ReadPublicKey(Stream input)
            {
                PgpPublicKeyRingBundle pgpPub = new PgpPublicKeyRingBundle(
                    PgpUtilities.GetDecoderStream(input));

                //
                // we just loop through the collection till we find a key suitable for encryption, in the real
                // world you would probably want to be a bit smarter about this.
                //

                foreach (PgpPublicKeyRing keyRing in pgpPub.GetKeyRings())
                {
                    foreach (PgpPublicKey key in keyRing.GetPublicKeys())
                    {
                        if (key.IsEncryptionKey)
                        {
                            return key;
                        }
                    }
                }

                throw new ArgumentException("Can't find encryption key in key ring.");
            }


            internal static byte[] CompressFile(string fileName, CompressionAlgorithmTag algorithm)
            {
                MemoryStream bOut = new MemoryStream();
                PgpCompressedDataGenerator comData = new PgpCompressedDataGenerator(algorithm);
                PgpUtilities.WriteFileToLiteralData(comData.Open(bOut), PgpLiteralData.Binary,
                    new FileInfo(fileName));
                comData.Close();
                return bOut.ToArray();
            }
        }
        public class Streams
        {
            private const int BufferSize = 512;

            public static void PipeAll(Stream inStr, Stream outStr)
            {
                byte[] bs = new byte[BufferSize];
                int numRead;
                while ((numRead = inStr.Read(bs, 0, bs.Length)) > 0)
                {
                    outStr.Write(bs, 0, numRead);
                }
            }
        }
    }
}
