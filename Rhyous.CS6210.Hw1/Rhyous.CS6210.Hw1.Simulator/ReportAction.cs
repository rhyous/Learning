using Newtonsoft.Json;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.SimpleArgs;
using System;
using System.Collections.Generic;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Simulator
{
    public class ReportAction
    {
        public ReportAction(TimeSimulator timeSimulator, 
                            Random random, 
                            DiseaseRecordGenerator generator,
                            DiseaseSimulatorClient client)
        {
            TimeSimulator = timeSimulator;
            Random = random;
            DiseaseGenerator = generator;
            DiseaseSimulatorClient = client;
        }

        public TimeSimulator TimeSimulator { get; set; }
        public Random Random { get; set; }
        public DiseaseRecordGenerator DiseaseGenerator { get; set; }
        public DiseaseSimulatorClient DiseaseSimulatorClient { get; set; }
        public void Action(DateTime current)
        {
            var list = new List<Record>();
            int diseaseReports = Random.Next(25);
            for (int i = 0; i < diseaseReports; i++)
            {
                list.Add(DiseaseGenerator.Generate(TimeSimulator.Current, Random));
            }
            var endpoint = Args.Value(Constants.EndPoint);
            DiseaseSimulatorClient.Client.Connect(endpoint);
            var json = JsonConvert.SerializeObject(list);
            var frame = new ZFrame(json);
            Console.WriteLine("Sent:");
            Console.WriteLine(json);
            DiseaseSimulatorClient.Client.Send(json, DiseaseSimulatorClient.ReceiveAction);
        }
    }
}
