using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
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
            var nsEndpoint = "tcp://127.0.0.1:55021";
            var mockLogger = new Mock<ILogger>();
            var name = "DSC1";
            var timeSimulator = new TimeSimulator();
            var random = new Random();
            var generator = new DiseaseRecordGenerator();
            var client = new DiseaseSimulatorClient(name, mockLogger.Object, nsEndpoint);
            var mockRequestClient = new Mock<IRequestSocket>();
            client.RegClient.Socket = mockRequestClient.Object;
            var endpoint = "tcp://127.0.0.1:55521"; // Added 1 to the end
            var reportAction = new ReportAction(client.SystemRegistration, timeSimulator, random, generator, client, endpoint);
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
