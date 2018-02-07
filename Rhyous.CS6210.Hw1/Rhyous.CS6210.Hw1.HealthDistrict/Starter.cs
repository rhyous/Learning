using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.LogClient;
using Rhyous.SimpleArgs;
using System;

namespace Rhyous.CS6210.Hw1.HealthDistrict
{
    internal class Starter
    {
        internal static ILogger Logger;
        internal static void Start()
        {
            var name = Args.Value(Constants.DistrictServerName);
            Logger = new LoggerClient(Args.Value(Constants.LoggerEndpoint), name);
            Logger.WriteLine($"{name} has started.");

            Console.WriteLine("Health District: " + Args.Value(Constants.DistrictServerName));
            var endpoint = Args.Value(Constants.DistrictServerEndPoint);
            Console.WriteLine("Endpoint: " + endpoint);
            var districtServer = new DistrictServer();
            districtServer.Start(endpoint);


        }
    }
}