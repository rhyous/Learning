using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.LogClient;
using Rhyous.CS6210.Hw1.Models;
using System;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.OutBreakAnalyzer
{
    internal class Starter
    {
        internal static ILogger Logger1;
        internal static ILogger Logger2;
        internal static async Task StartAsync(string analyzerName, string analyzerEndpoint, string publisherName, string publisherEndpoint, string nsEndpoint, string logServerName)
        {
            Console.WriteLine("Disease Analyzer: " + analyzerName);
            var loggerClient1 = new LoggerClient(logServerName, analyzerName, nsEndpoint);
            var consoleLogger = new ConsoleLogger();
            await loggerClient1.GetLogServerEndpointAsync(logServerName);

            Logger1 = new MultiLogger(consoleLogger, loggerClient1);
            Logger2 = new MultiLogger(consoleLogger, new LoggerClient(logServerName, analyzerName, nsEndpoint) { LogServerEndpoint = loggerClient1.LogServerEndpoint});

            var analyzerServer = new AnalyzerServer(analyzerName, analyzerEndpoint, nsEndpoint, Logger1);
            var publisherServer = new OutbreakPublisherServer(publisherName, publisherEndpoint, nsEndpoint, Logger2);

            var analyzerServerRegTask = analyzerServer.RegisterAsync();           
            var publisherServerRegTask = publisherServer.RegisterAsync();
            await Task.WhenAll(analyzerServerRegTask, publisherServerRegTask);

            var analyzerTask = analyzerServer.StartAsync(analyzerEndpoint);            
            var publisherTask = publisherServer.StartAsync(publisherEndpoint, ZSocketType.PUB, publisherServer.ReceiveAction);
            await Task.WhenAll(analyzerTask, publisherTask);
        }
    }
}