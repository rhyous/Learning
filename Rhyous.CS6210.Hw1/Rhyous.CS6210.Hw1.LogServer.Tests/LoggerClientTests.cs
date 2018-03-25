using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using log4net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using ZeroMQ;
using Rhyous.CS6210.Hw1.Interfaces;

namespace Rhyous.CS6210.Hw1.LogServer.Tests
{
    [TestClass]
    public class LoggerServerTests
    {
        [TestMethod]
        public void TestLogServer()
        {
            // Arrange
            var endpoint = "tcp://127.0.0.1:55010";
            var nsEndpoint = "tcp://127.0.0.1:55020";
            var logList = new List<string>();
            var mockLog = new Mock<ILogger>();
            var loggerServer = new LoggerServer("Test Logger Server", endpoint, nsEndpoint, mockLog.Object);
            mockLog.Setup(l => l.WriteLine(It.IsAny<string>())).Callback((string msg) => {
                logList.Add(msg);
                loggerServer.Stop();
            });
        
            var task = Task.Run(() => loggerServer.StartAsync(endpoint));
            Thread.Sleep(300);
                       
            var pushContext = new ZContext();
            var pushSocket = new ZSocket(pushContext, ZSocketType.PUSH);
            pushSocket.Connect(endpoint);

            // Act
            pushSocket.Send(new ZFrame("This is a message."));

            task.Wait();
            // Assert
            Assert.AreEqual(1, logList.Count);
            Assert.AreEqual(1, logList.Count);
        }
    }
}
