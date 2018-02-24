using Newtonsoft.Json;
using Rhyous.CS6210.Hw1.Models;
using System;
using System.Collections.Generic;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Simulator
{
    public class ReportAction
    {
        public static int SendActionCounter;
        internal SystemRegistration SystemRegistration;

        public ReportAction(SystemRegistration systemRegistration,
                            TimeSimulator timeSimulator, 
                            Random random, 
                            DiseaseRecordGenerator generator,
                            DiseaseSimulatorClient client,
                            string endpoint)
        {
            SystemRegistration = systemRegistration;
            TimeSimulator = timeSimulator;
            Random = random;
            DiseaseGenerator = generator;
            DiseaseSimulatorClient = client;
            Endpoint = endpoint;
        }

        public TimeSimulator TimeSimulator { get; set; }
        public Random Random { get; set; }
        public DiseaseRecordGenerator DiseaseGenerator { get; set; }
        public DiseaseSimulatorClient DiseaseSimulatorClient { get; set; }
        public string Endpoint { get; set; }
        public void Action(DateTime current)
        {
            var list = new List<Record>();
            int diseaseReports = Random.Next(25);
            for (int i = 0; i < diseaseReports; i++)
            {
                list.Add(DiseaseGenerator.Generate(TimeSimulator.Current, Random));
            }
            DiseaseSimulatorClient.Client.Connect(Endpoint);
            var packet = new Packet<List<Record>>
            {
                Payload = list,
                VectorTimeStamp = new VectorTimeStamp(),
                Sent = current
            };
            packet.VectorTimeStamp[SystemRegistration.Id] = new VectorCounter { Counter = ++SendActionCounter, Date = current };
            var json = JsonConvert.SerializeObject(packet);
            var frame = new ZFrame(json);
            Console.WriteLine("Sent: ");
            Console.WriteLine(json);
            DiseaseSimulatorClient.Client.Send(json, DiseaseSimulatorClient.ReceiveAction);
        }
    }
}
