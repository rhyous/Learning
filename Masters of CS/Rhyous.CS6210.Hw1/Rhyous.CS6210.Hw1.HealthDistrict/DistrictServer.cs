using Newtonsoft.Json;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.CS6210.Hw1.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.HealthDistrict
{
    public class DistrictServer : SendServerWithRegistration
    {
        TimeSpan DateTimeOffset;
        public IRepository<Record> Repo = new Repository<Record>();

        public DistrictServer(string name, string endpoint, string nsEndpoint, TimeSpan dateTimeOffset, ILogger logger) 
            : base(name, endpoint, nsEndpoint, logger)
        {
            DateTimeOffset = dateTimeOffset;
        }
        
        public async Task Start(string endpoint)
        {
            await StartAsync(endpoint, ZSocketType.REP, ReceiveAction);
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
            response.VectorTimeStamp.Update(SystemRegistration.Id, DateTime.Now.Add(DateTimeOffset));
            var responseJson = JsonConvert.SerializeObject(response);
            SendAsync(responseJson).Wait();
        }
    }
}