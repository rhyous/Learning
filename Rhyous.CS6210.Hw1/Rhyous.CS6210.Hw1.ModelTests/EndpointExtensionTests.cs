using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.CS6210.Hw1.Models;

namespace Rhyous.CS6210.Hw1.ModelTests
{
    [TestClass]
    public class EndpointExtensionTests
    {
        [TestMethod]
        public void GetIpTest()
        {
            // Arrange
            var endpoint = "tcp://127.0.0.1:5000";
            var expected = "127.0.0.1";

            // Act
            var ip = endpoint.GetIp();

            // Assert
            Assert.AreEqual(expected, ip.ToString());
        }
    }
}
