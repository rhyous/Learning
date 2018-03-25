using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.CS6210.Hw1.OutBreakAnalyzer;
using Moq;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using System.Threading.Tasks;
using System.Threading;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.OutbreakAnalyzer.Tests
{
    [TestClass]
    public class OutbreakPublisherServerTests
    {
        [TestMethod]
        public void OutbreakPublisherServerStartTest()
        {
            // Arrange
            var server = new OutbreakPublisherServer();
            var mockSocket = new Mock<IReplySocket>();
            mockSocket.Setup(s => s.Bind(It.IsAny<string>()));
            server.Socket = mockSocket.Object;

            // Act
            var task = server.StartAsync("", ZSocketType.REP, null);
            Thread.Sleep(200);
            server.Stop();

            // Assert
            mockSocket.Verify(s => s.Bind(It.IsAny<string>()), Times.Once());
            mockSocket.Verify(s => s.Dispose(), Times.Once());
        }
    }
}