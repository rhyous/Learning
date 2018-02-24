using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.LogClient;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.SimpleArgs;
using System;

namespace Rhyous.CS6210.Hw1.NameServer
{
    internal class Starter
    {
        internal static ILogger Logger;
        internal static void Start()
        {
            var name = Args.Value(Constants.Name);
            Console.WriteLine($"{name}:{typeof(DynamicNameServer).Name}");
            var endpoint = Args.Value(Constants.Endpoint);
            Console.WriteLine(endpoint);
            Logger = new LoggerClient(Args.Value(Constants.LoggerEndpoint), name);
            var nameServer = new DynamicNameServer(name, Logger);
            nameServer.Start(endpoint);
        }
    }
}