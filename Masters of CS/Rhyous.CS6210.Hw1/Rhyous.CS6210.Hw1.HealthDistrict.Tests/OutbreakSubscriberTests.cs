using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using ZeroMQ;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rhyous.CS6210.Hw1.HealthDistrict.Tests
{
    [TestClass]
    public class OutbreakSubscriberTests
    {
        [TestMethod]
        public void OutbreakSubscriberConnect()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var subscriber = new OutbreakSubscriber(mockLogger.Object);
            var mockSocket = new Mock<ISubscribeSocket>();
            mockSocket.Setup(s => s.Connect(It.IsAny<string>()));
            mockSocket.Setup(s => s.Subscribe(It.IsAny<string>()));
            subscriber.Socket = mockSocket.Object;
            var record = new Record { Id = 1, Disease = 0, };

            // Act
            var task = Task.Run(() => { subscriber.Connect(""); });
            Thread.Sleep(300);

            // Assert
            mockSocket.Verify(s => s.Connect(It.IsAny<string>()), Times.Once());
        }
    }
}