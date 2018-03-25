using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.LogClient;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.SimpleArgs;
using Rhyous.StringLibrary;
using System;

namespace Rhyous.CS6210.Hw1.Simulator
{
    internal class Starter
    {
        internal static ILogger Logger;

        internal static void Start()
        {
            var name = Args.Value(Constants.Name);
            var nsEndpoint = Args.Value(Constants.NameServerEndpoint);
            var timeSimulator = new TimeSimulator();

            int year = Args.Value(Constants.Year).To<int>();
            int month = Args.Value(Constants.Month).To<int>();
            int day = Args.Value(Constants.Day).To<int>();
            int timeMultiplier = Args.Value(Constants.TimeMultiplier).To<int>();
            int duration = Args.Value(Constants.Duration).To<int>();
            
            Logger = new LoggerClient(Args.Value(Constants.LoggerEndpoint), name);
            var client = new DiseaseSimulatorClient(name, nsEndpoint, Logger) { Client = new RequestClient() };

            Logger.WriteLine($"{Constants.Year}: {year}", client.SystemRegistration.Id);
            Logger.WriteLine($"{Constants.Month}: {month}", client.SystemRegistration.Id);
            Logger.WriteLine($"{Constants.Day}: {day}", client.SystemRegistration.Id);
            Logger.WriteLine($"{Constants.TimeMultiplier}: {timeMultiplier}", client.SystemRegistration.Id);
            Logger.WriteLine($"{Constants.Duration}: {duration}", client.SystemRegistration.Id);

            Logger.WriteLine($"{name} has started.");
            Console.WriteLine("Simulator: " + name);


            string endpoint = Args.Value(Constants.Endpoint);
            Logger.WriteLine($"{Constants.Endpoint}: {endpoint}");

            var startDate = new DateTime(year, month, day);
            Logger.WriteLine($"{Constants.StartDate}: {startDate.ToLongDateString()}");
            
            var reportAction = new ReportAction(client.SystemRegistration, timeSimulator, new Random(), new DiseaseRecordGenerator(), client, endpoint);

            timeSimulator.Start(startDate, timeMultiplier, duration, reportAction.Action);
            timeSimulator.Wait();
        }
    }
}
