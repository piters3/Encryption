using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Encryption;
using System.Text;
using System.Linq;

namespace EncryptionTest
{
    [TestClass]
    public class EncryptAndDecryptBytesTests
    {
        Encryptor enc = new Encryptor();
        byte[] sourceBytes = Encoding.ASCII.GetBytes("Dane");
        string key = "Klucz";


        [TestMethod]
        public void DecryptedBytesAreEqualSourceBytes()
        {
            byte[] encryptedbytes = enc.EncryptByte(sourceBytes, key);
            byte[] decryptedBytes = enc.DecryptByte(encryptedbytes, key);

            Assert.IsTrue(sourceBytes.SequenceEqual(decryptedBytes));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptBytesThrowExceptionWithoutKey()
        {
            byte[] encryptedbytes = enc.EncryptByte(sourceBytes, key);
            byte[] decryptedBytes = enc.DecryptByte(encryptedbytes, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DecryptBytesThrowExceptionWithoutSource()
        {
            byte[] encryptedbytes = enc.EncryptByte(sourceBytes, key);
            byte[] decryptedBytes = enc.DecryptByte(null, key);
        }
    }
}
