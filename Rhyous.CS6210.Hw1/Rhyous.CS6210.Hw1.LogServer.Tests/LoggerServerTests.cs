using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using log4net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using ZeroMQ;
using Rhyous.CS6210.Hw1.LogClient;
using Rhyous.CS6210.Hw1.Models;
using System;

namespace Rhyous.CS6210.Hw1.LogServer.Tests
{
    [TestClass]
    public class LoggerClientTests
    {
        [TestMethod]
        public void TestLogServerFromClient()
        {
            // Arrange
            var endpoint = "tcp://127.0.0.1:55011"; 
            var nsEndpoint = "tcp://127.0.0.1:55021"; 
            var msgPushSocket = "This is a message from a PUSH zsocket.";
            var msgLogger = "This is a test message from the LoggerClient.";
            var clientName = "Test Client";
            var systemRegistration = new SystemRegistration { Id = 1, Name = clientName };
            var logList = new List<string>();
            var mockLog = new Mock<ILog>();
            var loggerServer = new LoggerServer("Test Logger Server", mockLog.Object, nsEndpoint);
            mockLog.Setup(l => l.Debug(It.IsAny<object>())).Callback((object msg) => {
                logList.Add(msg.ToString());
                if (logList.Count > 1)
                    loggerServer.Stop();
            });
        
            var task = Task.Run(() => loggerServer.Start(endpoint, true));
            Thread.Sleep(300);
                       
            var pushContext = new ZContext();
            var pushSocket = new ZSocket(pushContext, ZSocketType.PUSH);
            pushSocket.Connect(endpoint);

            var loggerClient = new LoggerClient(endpoint, clientName);
            var vts = new VectorTimeStamp();
            vts.Update(systemRegistration, DateTime.Now);

            // Act
            loggerClient.WriteLine(msgLogger, vts);
            Thread.Sleep(25);
            pushSocket.Send(new ZFrame(msgPushSocket));
            task.Wait();

            // Assert
            Assert.AreEqual(2, logList.Count);
            Assert.AreEqual($"{clientName}:{vts}: {msgLogger}", logList[0]);
            Assert.AreEqual(msgPushSocket, logList[1]);
        }
    }
}
