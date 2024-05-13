using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.LogClient;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.SimpleArgs;
using System;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw1.HealthDistrict
{
    internal class Starter
    {
        internal static ILogger Logger;
        internal static async Task StartAsync(TimeSpan offset)
        {
            var name = Args.Value(Constants.DistrictServerName);
            Logger = new MultiLogger(
                new ConsoleLogger(),
                new LoggerClient(Args.Value(Constants.LoggerEndpoint), name, Args.Value("NsEndpoint"))
            );
            Logger?.WriteLine($"{name} has started.");

            Console.WriteLine("Health District: " + name);
            var endpoint = Args.Value(Constants.DistrictServerEndPoint);
            var nsEndpoint = Args.Value(Constants.NameServerEndpoint);
            Console.WriteLine("Endpoint: " + endpoint);
            var districtServer = new DistrictServer("DS1", endpoint, nsEndpoint, offset, Logger);
            await districtServer.RegisterAsync();
            await districtServer.Start(endpoint);

            var publisherArgInput = Args.Value(Constants.PublisherEndpoints);
            Logger?.WriteLine("Publisher endpoints: " + publisherArgInput);
            var pubEndpoints =publisherArgInput.ToArray();
            foreach (var publisherEndPoint in pubEndpoints)
            {
                var subscriber = new OutbreakSubscriber(Logger);
                subscriber.Connect(publisherEndPoint);
                subscriber.Subscribe(name);
            }
        }
    }
}