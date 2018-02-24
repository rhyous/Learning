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
        internal SystemRegistration SystemRegistration = new SystemRegistration();
        TimeSpan DateTimeOffset;
        public IRepository<Record> Repo = new Repository<Record>();

        public DistrictServer(string name, TimeSpan dateTimeOffset)
        {
            SystemRegistration.Name = name;
            DateTimeOffset = dateTimeOffset;
        }
        
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
            var response = new Packet<List<Record>>
            {
                Payload = addedRecords.ToList(),
                VectorTimeStamp = packet.VectorTimeStamp
            };
            response.VectorTimeStamp.Update(SystemRegistration, DateTime.Now.Add(DateTimeOffset));
            var responseJson = JsonConvert.SerializeObject(response);
            Send(responseJson);
        }
    }
}