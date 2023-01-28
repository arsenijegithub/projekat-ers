using AMS;
using AMS.Wrappers;
using LocalController.Wrappers;
using LocalDevice;
using LocalDevice.Wrappers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementTests
{
    [TestFixture]
    public class AmsTest
    {
        [Test]
        public void StartTest()
        {
            //Arrange
            AMSClass ams = new AMSClass();


            //Act
            ams.PokreniServer();

            //Assert
            Assert.IsNotNull(ams.MyServer);
            Assert.IsNotNull(ams.MyStream);
        }

	[Test]
        public void ReceiveData_ReturnsTrue()
        {
            //Arrange
            AMSClass ams = new AMSClass();

            var myTcp = new Mock<MyTcpListener>();
            myTcp.Setup(x => x.AcceptTcpClient()).Throws(new Exception());

            ams.MyServer = myTcp.Object;


            var stream = new Mock<MyNetworkStream>();
            stream.Setup(x => x.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>())).Verifiable();
            ams.MyStream = stream.Object;


            var db = new Mock<DbWrapper>();
            db.Setup(x => x.SacuvajPodatke(It.IsAny<List<LocalDeviceClass>>())).Verifiable();

            ams.Db = new DbWrapper();
            //Act
            bool result = ams.PrimiPodatke();

            //Assert

            Assert.IsFalse(result);
        }



    }
}
