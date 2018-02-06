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
            Logger = new LoggerClient(Args.Value(Constants.LoggerEndpoint), Args.Value(Constants.Name));
            Logger.WriteLine($"{Args.Value(Constants.Name)} has started.");

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

            var startDate = new DateTime(year, month, day);
            Logger.WriteLine($"{Constants.StartDate}: {startDate.ToLongDateString()}");

            var client = new DiseaseSimulatorClient { Client = new RequestClient() };
            var reportAction = new ReportAction(timeSimulator, new Random(), new DiseaseRecordGenerator(), client);

            timeSimulator.Start(startDate, timeMultiplier, duration, reportAction.Action);
            timeSimulator.Wait();
        }
    }
}
