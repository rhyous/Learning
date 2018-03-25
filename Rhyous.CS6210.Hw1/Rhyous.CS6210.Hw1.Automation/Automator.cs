using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Rhyous.CS6210.Hw1.Automation
{
    [TestClass]
    public class Automator
    {
        [TestMethod]
        public void RunAutomation()
        {
            var nsEndpoint = "tcp://127.0.0.1:6001";
            string logServerEndpoint = null; //"tcp://127.0.0.1:5001";

            var nsTask = NameServer.Starter.StartAsync("NameServer", nsEndpoint, logServerEndpoint);
            while (NameServer.Starter.Ns == null || !NameServer.Starter.Ns.IsStarted)
            {
            }

            var mockLogger = new Mock<ILog>();
            var logMessages = new List<string>();
            mockLogger.Setup(l => l.Debug(It.IsAny<object>())).Callback((object msg) => {
                logMessages.Add(msg.ToString());
            });
            var logTask = LogServer.Starter.StartAsync("LogServer", logServerEndpoint, nsEndpoint, mockLogger.Object, true);
            while (LogServer.Starter.LS == null || !LogServer.Starter.LS.IsStarted)
            {
            }
            
            var analyzerEndpoint1 = "tcp://127.0.0.1:5101";
            var publisherEndpoint1 = "tcp://127.0.0.1:5201";
            var analyzerTask = OutBreakAnalyzer.Starter.StartAsync("AnalyzerServer1", analyzerEndpoint1, publisherEndpoint1, logServerEndpoint);
            
            var cts = new CancellationTokenSource(120000);
            Task.WaitAll(new[] { nsTask, logTask, analyzerTask }, cts.Token);
        }
    }
}