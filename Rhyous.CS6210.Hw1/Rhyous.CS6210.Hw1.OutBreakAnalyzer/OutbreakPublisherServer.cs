using Newtonsoft.Json;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.CS6210.Hw1.Repository;
using System;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.OutBreakAnalyzer
{
    public class OutbreakPublisherServer : PublisherServer
    {
        public IRepository<Record> Repo = new Repository<Record>();

        internal void ReceiveAction(ZFrame frame)
        {
            var json = frame.ReadString();
            Console.WriteLine("Received: ");
            Console.WriteLine(json);
            var record = JsonConvert.DeserializeObject<Record>(json);
            Repo.Create(record);
        }
    }
}
