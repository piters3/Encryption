using Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security.Cryptography;

namespace EncryptionTest
{
    [TestClass]
    public class EncryptTextToFileTests
    {
        Encryptor enc = new Encryptor();
        string inputFile = @"C:\Users\Pioter\source\repos\Encryption\Encryption\TestFile.txt";
        string text = "Tekst";
        static DES DESalg = DES.Create("DES");

        [TestMethod]
        public void EncryptedFileIsNotEmpty()
        {
            enc.EncryptTextToFile(text, inputFile, DESalg.Key, DESalg.IV);
            Assert.IsTrue(new FileInfo(inputFile).Length > 0);
        }

        [TestMethod]
        public void EncryptedTextInFileIsNotEqualSourceText()
        {
            enc.EncryptTextToFile(text, inputFile, DESalg.Key, DESalg.IV);
            string encrypted = File.ReadAllText(inputFile);

            Assert.AreNotEqual(text, encrypted);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptTextToFileThrowExceptionWithoutPathToFile()
        {
            enc.EncryptTextToFile(text, null, DESalg.Key, DESalg.IV);
        }


        [TestMethod]
        public void DecryptedTextFromFileIsEqualSourceText()
        {
            string decrypted = enc.DecryptTextFromFile(inputFile, DESalg.Key, DESalg.IV);

            Assert.AreEqual(text, decrypted);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptTextFromFileThrowExceptionWithoutPathToFile()
        {
            enc.EncryptTextToFile(text, null, DESalg.Key, DESalg.IV);
        }

    }
}
