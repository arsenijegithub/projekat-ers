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




    }
}
