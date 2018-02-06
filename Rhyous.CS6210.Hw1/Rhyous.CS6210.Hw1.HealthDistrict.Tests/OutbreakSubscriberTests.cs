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
        public void OutbreakSubscriberStart()
        {
            // Arrange
            var district = new OutbreakSubsriber();
            var mockSocket = new Mock<IReplySocket>();
            mockSocket.Setup(s => s.Bind(It.IsAny<string>()));
            district.Socket = mockSocket.Object;
            var record = new Record { Id = 1, Disease = 0, };

            // Act
            var task = Task.Run(() => { district.Start("", ZSocketType.REP, null); });
            Thread.Sleep(200);
            district.Stop();

            // Assert
            mockSocket.Verify(s => s.Bind(It.IsAny<string>()), Times.Once());
            mockSocket.Verify(s => s.Dispose(), Times.Once());
        }
    }
}