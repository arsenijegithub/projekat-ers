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
        public void LocalDeviceConstructorTest()
        {
            LocalDeviceClass localDevice = new LocalDeviceClass("1", "1", 1, "1", 1, "1", 1);

            Assert.IsNotEmpty(localDevice.LocalDeviceCode);
            Assert.IsNotNull(localDevice.MyStream);
            Assert.IsNotEmpty(localDevice.Configuration);
        }

        [Test]
        public void HashGenerator_ReturnsHash()
        {
            //Arrange
            LocalDeviceClass localDevice = new LocalDeviceClass("1", "1", 1, "1", 1, "1", 1);
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


        [Test]
        public void PosaljiPodatke_ReturnsFalse()
        {
            //Arrange
            LocalDeviceClass localDevice = new LocalDeviceClass("2", "2", 2, "2", 2, "2", 2);

            var stream = new Mock<MyNetworkStream>();
            stream.Setup(x => x.Write(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());

            localDevice.MyStream = (MyNetworkStream)stream.Object;

            //Act
            bool result = localDevice.PosaljiPodatke();


            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void PosaljiPodatke_ReturnsTrue()
        {
            //Arrange
            LocalDeviceClass localDevice = new LocalDeviceClass("2", "2", 2, "2", 2, "2", 2);

            var stream = new Mock<MyNetworkStream>();
            stream.Setup(x => x.Write(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>())).Verifiable();
            stream.Setup(x => x.Close()).Verifiable();


            localDevice.MyStream = stream.Object;

            //Act
            bool result = localDevice.PosaljiPodatke();


            //Assert
            Assert.IsTrue(result);
        }



    }
}