using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.LogClient;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.SimpleArgs;
using System;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.OutBreakAnalyzer
{
    internal class Starter
    {
        internal static ILogger Logger;
        internal static void Start()
        {
            var name = Args.Value(Constants.Name);
            if (string.IsNullOrWhiteSpace(name))
                name = Args.Value(Constants.Disease);
            Console.WriteLine("Disease Analyzer: " + name);

            Logger = new LoggerClient(Args.Value(Constants.LoggerEndpoint), name);
            var analyzerEndpoint = Args.Value(Constants.AnalyzerEndpoint);
            var AnalyzerServer = new AnalyzerServer(name, Logger);
            var analyzerTask = Task.Run(() => 
            {
                AnalyzerServer.Start(analyzerEndpoint);
            });
            
            var publisherEndpoint = Args.Value(Constants.PublisherEndpoint);
            var publisherServer = new OutbreakPublisherServer();
            var publisherTask = Task.Run(() =>
            {
                publisherServer.Start(publisherEndpoint, ZSocketType.PUB, publisherServer.ReceiveAction);
            });
            Task.WaitAll(analyzerTask, publisherTask);
        }
    }
}