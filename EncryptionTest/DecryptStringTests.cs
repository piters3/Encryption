using Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EncryptionTest
{
    [TestClass]
    class DecryptStringTests
    {
        Encryptor enc = new Encryptor();
        string source = "Tekst do zaszyfrowania";
        string key = "Klucz";

       
        [TestMethod]
        public void DecryptedStringAreEqualSourceString()
        {
            string encrypted = enc.EncryptString(source, key);
            string decrypted = enc.DecryptString(encrypted, key);

            Assert.AreEqual(source, decrypted);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptStringThrowExceptionWithoutKey()
        {
            string encrypted = enc.EncryptString(source, key);
            string decrypted = enc.DecryptString(encrypted, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptStringThrowExceptionWithoutSource()
        {
            string encrypted = enc.EncryptString(source, key);
            string decrypted = enc.DecryptString(null, key);
        }
    }
}
