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




    }
}
