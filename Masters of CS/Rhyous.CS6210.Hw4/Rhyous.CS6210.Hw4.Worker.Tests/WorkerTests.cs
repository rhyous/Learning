using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.CS6210.Hw4.Models;

namespace Rhyous.CS6210.Hw4.Tests
{
    [TestClass]
    public class WorkerTests
    {
        [TestMethod]
        public void WorkerConnection_ToString()
        {
            // Arrange
            var ip = "192.168.0.1";
            var port = 2700;
            var connection = new WorkerConnection(ip, port);

            // Act
            var actual = connection.ToString();

            // Assert
            Assert.AreEqual($"{ip}.{port}", actual);

        }
    }
}
