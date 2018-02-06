using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.HealthDistrict.Tests
{
    [TestClass]
    public class DistrictServerTests
    {
        #region start tests
        [TestMethod]
        public void DistrictServerStart()
        {
            // Arrange
            var district = new DistrictServer();
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

        [TestMethod]
        public void ReceiveActionCallsRepoCreate()
        {
            // Arrange
            var district = new DistrictServer();
            var records = new List<Record> { new Record { Id = 1, Disease = 0 } };

            var mockRepo = new Mock<IRepository<Record>>();
            district.Repo = mockRepo.Object;
            mockRepo.Setup(r => r.Create(It.IsAny<IEnumerable<Record>>()))
                    .Returns((IEnumerable<Record> values) => { return values; });

            var mockSocket = new Mock<IReplySocket>();
            district.Socket = mockSocket.Object;
            mockSocket.Setup(s => s.Send(It.IsAny<string>()));

            // Act
            district.ReceiveAction(new ZFrame(JsonConvert.SerializeObject(records)));

            // Assert
            mockRepo.Verify(x => x.Create(It.IsAny<IEnumerable<Record>>()), Times.Once());
            mockSocket.Verify(x => x.Send(It.IsAny<string>()), Times.Once());
        }

        #endregion

        #region
        [TestMethod]
        public void DistrictServerBindTest()
        {
            // Arrange
            var district = new DistrictServer();
            var mockRepo = new Mock<IRepository<Record>>();
            var list = new List<Record>();
            district.Repo = mockRepo.Object;
            var records = new List<Record> { new Record { Id = 1, Disease = 0, } };
            mockRepo.Setup(r => r.Create(It.IsAny<IEnumerable<Record>>()))
                .Returns((IEnumerable<Record> values) => 
                {
                    list.AddRange(values);
                    return values;
                });
            var endpoint = "tcp://127.0.0.1:5553";
            var task = Task.Run(() => district.Start(endpoint));
            Thread.Sleep(200);

            var context = new ZContext();
            var socket = new ZSocket(ZSocketType.REQ);
            socket.Connect(endpoint);

            var json = JsonConvert.SerializeObject(records);

            // Act
            socket.Send(new ZFrame(json));
            bool receivedResponse = false;
            while (!receivedResponse)
            {
                using (var frame = socket.ReceiveFrame())
                {
                    receivedResponse = true;
                }
            }

            // Assert
            mockRepo.Verify(x => x.Create(It.IsAny<IEnumerable<Record>>()), Times.Once());
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(new CompareLogic().Compare(records[0], list[0]).AreEqual);
        }
        #endregion
    }
}