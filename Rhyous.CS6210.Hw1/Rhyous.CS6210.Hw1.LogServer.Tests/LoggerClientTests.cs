using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using log4net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using ZeroMQ;

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
            var mockLog = new Mock<ILog>();
            var loggerServer = new LoggerServer("Test Logger Server", mockLog.Object, nsEndpoint);
            mockLog.Setup(l => l.Debug(It.IsAny<object>())).Callback((object msg) => {
                logList.Add(msg.ToString());
                loggerServer.Stop();
            });
        
            var task = Task.Run(() => loggerServer.Start(endpoint, true));
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
