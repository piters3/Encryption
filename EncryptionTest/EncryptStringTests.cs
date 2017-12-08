using Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EncryptionTest
{
    [TestClass]
    public class EncryptStringTests
    {
        Encryptor enc = new Encryptor();
        string source = "Tekst do zaszyfrowania";
        string key = "Klucz";

        [TestMethod]
        public void EncryptedStringIsNotEqualSourceString()
        {
            string encrypted = enc.EncryptString(source, key);
            Assert.AreNotEqual(source, encrypted);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptStringThrowExceptionWithoutKey()
        {
            string encrypted = enc.EncryptString(source, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptStringThrowExceptionWithoutSource()
        {
            string encrypted = enc.EncryptString(null, key);
        }
    }
}
