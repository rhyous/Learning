using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.CS6210.Hw4.Models.Tests
{
    [TestClass]
    public class IpAddressTests
    {
        [TestMethod]
        public void IpAddress_ImplicitCast_FromString()
        {
            // Arrange
            var ipStr = "10.1.2.3";

            // Act
            IpAddress ip = ipStr;

            // Assert
            Assert.AreEqual(ip.Octet1, 10);
            Assert.AreEqual(ip.Octet2, 1);
            Assert.AreEqual(ip.Octet3, 2);
            Assert.AreEqual(ip.Octet4, 3);

        }
    }
}