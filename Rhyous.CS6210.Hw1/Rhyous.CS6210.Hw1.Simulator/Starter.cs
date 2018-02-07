using Newtonsoft.Json;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.LogClient;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.SimpleArgs;
using Rhyous.StringLibrary;
using System;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Simulator
{
    internal class Starter
    {
        internal static ILogger Logger;

        internal static void Start()
        {
            var name = Args.Value(Constants.Name);
            Logger = new LoggerClient(Args.Value(Constants.LoggerEndpoint), name);
            Logger.WriteLine($"{name} has started.");

            var timeSimulator = new TimeSimulator();
            
            int year = Args.Value(Constants.Year).To<int>();
            Logger.WriteLine($"{Constants.Year}: {year}");
            int month = Args.Value(Constants.Month).To<int>();
            Logger.WriteLine($"{Constants.Month}: {month}"); 
            int day = Args.Value(Constants.Day).To<int>();
            Logger.WriteLine($"{Constants.Day}: {day}");
            int timeMultiplier = Args.Value(Constants.TimeMultiplier).To<int>();
            Logger.WriteLine($"{Constants.TimeMultiplier}: {timeMultiplier}");
            int duration = Args.Value(Constants.Duration).To<int>();
            Logger.WriteLine($"{Constants.Duration}: {duration}");

            string endpoint = Args.Value(Constants.EndPoint);
            Logger.WriteLine($"{Constants.EndPoint}: {endpoint}");

            var startDate = new DateTime(year, month, day);
            Logger.WriteLine($"{Constants.StartDate}: {startDate.ToLongDateString()}");

            var client = new DiseaseSimulatorClient { Client = new RequestClient() };
            var reportAction = new ReportAction(timeSimulator, new Random(), new DiseaseRecordGenerator(), client, endpoint);

            timeSimulator.Start(startDate, timeMultiplier, duration, reportAction.Action);
            timeSimulator.Wait();
        }
    }
}
