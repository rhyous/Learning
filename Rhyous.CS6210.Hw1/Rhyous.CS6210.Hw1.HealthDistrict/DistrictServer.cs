using Newtonsoft.Json;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.CS6210.Hw1.Repository;
using Rhyous.SimpleArgs;
using System;
using System.Collections.Generic;
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
            var records = JsonConvert.DeserializeObject<List<Record>>(json);
            var addedRecords = Repo.Create(records);
            var responseJson = JsonConvert.SerializeObject(addedRecords);
            Send(responseJson);
        }
    }
}