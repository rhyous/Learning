using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Simulator.Tests
{
    [TestClass]
    public class DiseaseSimulatorClientTests
    {
        [TestMethod]
        public void DiseaseSimulatorClient_ReceiveAction()
        {
            // Arrange
            var nsEndpoint = "tcp://127.0.0.1:55021";
            var mockLogger = new Mock<ILogger>();
            var name = "DSC1";
            var timeSimulator = new TimeSimulator() { IsReportingProgress = true };
            var client = new DiseaseSimulatorClient(name, nsEndpoint, mockLogger.Object) { TimeSimulator = timeSimulator };
            var mockRequestClient = new Mock<IRequestSocket>();
            client.RegClient.Socket = mockRequestClient.Object;
            var date = new DateTime(2018, 1, 1);
            var mockClient = new Mock<IClient<ZFrame>>();
            client.Client = mockClient.Object;

            // Act
            client.ReceiveAction(new ZFrame());

            // Assert
            Assert.IsFalse(timeSimulator.IsReportingProgress);
        }
    }
}
