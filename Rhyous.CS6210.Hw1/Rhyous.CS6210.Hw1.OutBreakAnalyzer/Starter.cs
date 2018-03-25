using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.LogClient;
using System;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.OutBreakAnalyzer
{
    internal class Starter
    {
        internal static ILogger Logger;
        internal static async Task StartAsync(string name, string analyzerEndpoint, string publisherEndpoint, string loggerEndpoint)
        {
            Console.WriteLine("Disease Analyzer: " + name);
            Logger = new LoggerClient(loggerEndpoint, name);

            var AnalyzerServer = new AnalyzerServer(name, Logger);
            var analyzerTask = AnalyzerServer.StartAsync(analyzerEndpoint);
            
            var publisherServer = new OutbreakPublisherServer();
            var publisherTask = publisherServer.StartAsync(publisherEndpoint, ZSocketType.PUB, publisherServer.ReceiveAction);

            await Task.WhenAll(analyzerTask, publisherTask);
        }
    }
}