using Newtonsoft.Json;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.CS6210.Hw1.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.HealthDistrict
{
    public class DistrictServer : SendServer
    {
        public IRepository<Record> Repo = new Repository<Record>();

        public void Start(string endpoint)
        {
            Start(endpoint, ZSocketType.REP, ReceiveAction);
        }

        internal void ReceiveAction(ZFrame frame)
        {
            var json = frame.ReadString();
            Console.WriteLine("Received: ");
            Console.WriteLine(json);
            var packet = JsonConvert.DeserializeObject<Packet<List<Record>>>(json);
            var records = packet.Payload;
            var addedRecords = Repo.Create(records);
            var response = new Packet<List<Record>>();
            response.Payload = addedRecords.ToList();
            response.VectorTimeStamp = packet.VectorTimeStamp;
            response.VectorTimeStamp.HealthDistrict++;
            var responseJson = JsonConvert.SerializeObject(response);
            Send(responseJson);
        }
    }
}