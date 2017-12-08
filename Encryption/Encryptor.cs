using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Encryption
{
    public class Encryptor
    {
        private static readonly byte[] SALT = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };

        public void EncryptTextToFile(string Data, string FileName, byte[] Key, byte[] IV)
        {
            try
            {
                FileStream fStream = File.Open(FileName, FileMode.Truncate);
                DES DESalg = DES.Create();
                CryptoStream cStream = new CryptoStream(fStream, DESalg.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
                StreamWriter sWriter = new StreamWriter(cStream);

                sWriter.WriteLine(Data);

                sWriter.Close();
                cStream.Close();
                fStream.Close();
            }
            catch (CryptographicException ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public string DecryptTextFromFile(string FileName, byte[] Key, byte[] IV)
        {
            try
            {
                FileStream fStream = File.Open(FileName, FileMode.OpenOrCreate);
                DES DESalg = DES.Create();
                CryptoStream cStream = new CryptoStream(fStream, DESalg.CreateDecryptor(Key, IV), CryptoStreamMode.Read);
                StreamReader sReader = new StreamReader(cStream);

                string val = sReader.ReadLine();

                sReader.Close();
                cStream.Close();
                fStream.Close();

                return val;
            }
            catch (CryptographicException ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }


        public void EncryptFile(string inputFile, string outputFile, string skey)
        {
            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    byte[] key = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(skey));
                    byte[] IV = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(skey)).Take(16).ToArray();

                    using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
                    {
                        using (ICryptoTransform encryptor = aes.CreateEncryptor(key, IV))
                        {
                            using (CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                                {
                                    int data;
                                    while ((data = fsIn.ReadByte()) != -1)
                                    {
                                        cs.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DecryptFile(string inputFile, string outputFile, string skey)
        {
            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    byte[] key = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(skey));
                    byte[] IV = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(skey)).Take(16).ToArray();

                    using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
                    {
                        using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                        {
                            using (ICryptoTransform decryptor = aes.CreateDecryptor(key, IV))
                            {
                                using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
                                {
                                    int data;
                                    while ((data = cs.ReadByte()) != -1)
                                    {
                                        fsOut.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public string EncryptString(string source, string key)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

            byte[] byteHash;
            byte[] byteBuff;

            byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB;
            byteBuff = Encoding.UTF8.GetBytes(source);

            string encoded = Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            return encoded;
        }


        public string DecryptString(string encodedText, string key)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

            byte[] byteHash;
            byte[] byteBuff;

            byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB;
            byteBuff = Convert.FromBase64String(encodedText);

            string plaintext = Encoding.UTF8.GetString(desCryptoProvider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            return plaintext;
        }


        public byte[] EncryptByte(byte[] clearData, string key)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();

            alg.Key = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(key));
            alg.IV = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(key)).Take(16).ToArray();

            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearData, 0, clearData.Length);
            cs.FlushFinalBlock();
            cs.Close();
            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }


        public byte[] DecryptByte(byte[] cipherData, string key)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();

            alg.Key = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(key));
            alg.IV = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(key)).Take(16).ToArray();

            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.FlushFinalBlock();
            cs.Close();
            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }
    }
}
