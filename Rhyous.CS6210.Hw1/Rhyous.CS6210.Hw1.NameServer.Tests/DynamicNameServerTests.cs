using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;

namespace Rhyous.CS6210.Hw1.NameServer.Tests
{
    [TestClass]
    public class DynamicNameServerTests
    {
        [TestMethod]
        public void DynamicNameServerTest()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockRepo = new Mock<IRepository<SystemRegistration>>();
            var server = new DynamicNameServer("DNS System", mockLogger.Object)
            {
                UseLocalHost = true,
                Logger = mockLogger.Object,
                Repo = mockRepo.Object
            };
            var systemRegistration = new SystemRegistration { Name = "S1" };

            // Act
            server.RegisterSystem(systemRegistration);

            // Assert
            Assert.AreEqual(10, systemRegistration.Id);
            Assert.AreEqual(new IPAddress(new byte[] { 192, 168, 0, 2 }), systemRegistration.IpAddress);
            Assert.IsTrue(systemRegistration.Port >= 5000 && systemRegistration.Port <= 65000);
            Assert.AreEqual(10, systemRegistration.Id);
            mockRepo.Verify(r => r.Create(It.IsAny<SystemRegistration>()), Times.Once);
            mockRepo.Verify(r => r.Create(It.IsAny<SystemRegistration>()), Times.Once);
        }
        public void DynamicNameServerSelRegisterTest()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockRepo = new Mock<IRepository<SystemRegistration>>();
            var server = new DynamicNameServer("DNS System", mockLogger.Object)
            {
                UseLocalHost = true,
                Logger = mockLogger.Object,
                Repo = mockRepo.Object
            };

            // Act
            server.SelfRegister();

            // Assert
            var systemRegistration = server.SystemRegistration;
            Assert.AreEqual(10, systemRegistration.Id);
            Assert.AreEqual(new IPAddress(new byte[] { 192, 168, 0, 2 }), systemRegistration.IpAddress);
            Assert.IsTrue(systemRegistration.Port >= 5000 && systemRegistration.Port <= 65000);
            Assert.AreEqual(10, systemRegistration.Id);
            mockRepo.Verify(r => r.Create(It.IsAny<SystemRegistration>()), Times.Once);
            mockRepo.Verify(r => r.Create(It.IsAny<SystemRegistration>()), Times.Once);
        }
    }
}
