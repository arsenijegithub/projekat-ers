using LocalController;
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
    public class LocalControllerTest
    {

        [Test]
        public void LocalControllerConstructor()
        {
            LocalControllerClass localController = new LocalControllerClass();

            Assert.IsNotNull(localController.XmlReader);
            Assert.IsNotNull(localController.XmlWriter);
            Assert.IsNotNull(localController.MyAMSStream);
        }

        [Test]
        public void PosaljiPodatke_RetursnTrue()
        {
            //Arrange
            LocalControllerClass localController = new LocalControllerClass();

            var xml = new Mock<MyXmlReader>();
            xml.Setup(x => x.UcitajXml(It.IsAny<string>())).Returns(new List<LocalDeviceClass>());

            var stream = new Mock<MyNetworkStream>();
            stream.Setup(x => x.Write(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>())).Verifiable();
            stream.Setup(x => x.Close()).Verifiable();

            localController.XmlReader = xml.Object;
            localController.MyAMSStream = stream.Object;



            //Act
            bool resutl = localController.PosaljiPodatke();


            //Assert

            Assert.IsTrue(resutl);
        }


        [Test]
        public void PosaljiPodatke_RetursnFalse()
        {
            //Arrange
            LocalControllerClass localController = new LocalControllerClass();

            var xml = new Mock<MyXmlReader>();
            xml.Setup(x => x.UcitajXml(It.IsAny<string>())).Returns(new List<LocalDeviceClass>());

            var stream = new Mock<MyNetworkStream>();
            stream.Setup(x => x.Write(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());
            stream.Setup(x => x.Close()).Verifiable();

            localController.XmlReader = xml.Object;
            localController.MyAMSStream = stream.Object;



            //Act
            bool resutl = localController.PosaljiPodatke();


            //Assert

            Assert.IsFalse(resutl);
        }


        [Test]
        public void PrimiPodatke_ReturnsFalse()
        {
            //Arrange
            LocalControllerClass localController = new LocalControllerClass();



            var t = new Mock<MyTcpClient>();
            t.Setup(x => x.GetStream()).Verifiable();


            localController.MyClient = t.Object;
            var xml = new Mock<MyXmlWriter>();
            xml.Setup(x => x.PisiUXml(It.IsAny<LocalDeviceClass>(), It.IsAny<string>())).Verifiable();

            localController.XmlWriter = xml.Object;

            var myTcp = new Mock<MyTcpListener>();
            myTcp.Setup(x => x.AcceptTcpClient()).Throws(new Exception());

            localController.MyServer = myTcp.Object;


            var stream = new Mock<MyNetworkStream>();
            stream.Setup(x => x.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>())).Verifiable();


            localController.MyStream = stream.Object;
            //Act
            bool result = localController.PrimiPodatke();

            //Assert
            Assert.IsFalse(result);
        }
    }
}
