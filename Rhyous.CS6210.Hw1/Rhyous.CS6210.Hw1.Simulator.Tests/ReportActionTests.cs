using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.CS6210.Hw1.Interfaces;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Simulator.Tests
{
    [TestClass]
    public class ReportActionTests
    {
        [TestMethod]
        public void ReportActionConnectsAndSendsTest()
        {
            // Arrange
            var timeSimulator = new TimeSimulator();
            var random = new Random();
            var generator = new DiseaseRecordGenerator();
            var client = new DiseaseSimulatorClient();
            var endpoint = "tcp://127.0.0.1:55521"; // Added 1 to the end
            var reportAction = new ReportAction(timeSimulator, random, generator, client, endpoint);
            var date = new DateTime(2018, 1, 1);
            var mockClient = new Mock<IClient<ZFrame>>();
            client.Client = mockClient.Object;

            // Act
            reportAction.Action(date);

            // Assert
            mockClient.Verify(x => x.Connect(It.IsAny<string>()), Times.Once);
            mockClient.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<Action<ZFrame>>()), Times.Once);
        }
    }
}
