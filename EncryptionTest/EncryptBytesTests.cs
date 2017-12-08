using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Encryption;
using System.Text;

namespace EncryptionTest
{
    [TestClass]
    public class EncryptBytesTests
    {
        Encryptor enc = new Encryptor();
        byte[] sourceBytes = Encoding.ASCII.GetBytes("Dane");
        string key = "Klucz";

        [TestMethod]
        public void EncryptedBytesAreNotEqualSourceBytes()
        {
            byte[] encryptedbytes = enc.EncryptByte(sourceBytes, key);
            Assert.AreNotEqual(sourceBytes, encryptedbytes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptBytesThrowExceptionWithoutKey()
        {
            byte[] encryptedbytes = enc.EncryptByte(sourceBytes, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void EncryptBytesThrowExceptionWithoutSource()
        {
            byte[] encryptedbytes = enc.EncryptByte(null, key);
        }
    }
}
