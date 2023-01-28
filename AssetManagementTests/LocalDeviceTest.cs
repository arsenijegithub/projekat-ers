using LocalDevice;
using LocalDevice.Wrappers;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Cryptography;
using System.Text;

namespace AssetManagementTests
{
    [TestFixture]
    public class LocalDeviceTest
    {

        [Test]
        public void HashGenerator_ReturnsHash()
        {
            //Arrange
            LocalDeviceClass localDevice = new LocalDeviceClass("1", "1", 1, "1", 1, "1");
            string actualHash;
            using (SHA256 sha256 = SHA256.Create())
            {
                string Data = localDevice.Id + localDevice.Type + localDevice.Value + localDevice.WorkTime.ToString() + localDevice.Timestamp.ToString();

                byte[] value = sha256.ComputeHash(Encoding.UTF8.GetBytes(Data));
                actualHash = Encoding.UTF8.GetString(value);
            }

            //Act

            string hash = localDevice.HashGenerator();

            //Assert
            Assert.AreEqual(actualHash, hash);

        }
    }
}

