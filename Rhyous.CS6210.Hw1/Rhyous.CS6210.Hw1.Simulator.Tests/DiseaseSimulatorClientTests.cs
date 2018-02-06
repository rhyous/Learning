using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.CS6210.Hw1.Interfaces;
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
            var timeSimulator = new TimeSimulator() { IsReportingProgress = true };
            var client = new DiseaseSimulatorClient() { TimeSimulator = timeSimulator };
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
