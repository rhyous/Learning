using Newtonsoft.Json;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.CS6210.Hw1.Repository;
using System;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.OutBreakAnalyzer
{
    public class AnalyzerServer : SendServerWithRegistration, IName
    {
        public IRepository<Record> Repo = new Repository<Record>();
                
        public AnalyzerServer(string name, string endpoint, string nsEndpoint, ILogger logger) 
            : base(name, endpoint, nsEndpoint, logger)
        {
        }

        public async Task StartAsync(string endpoint = null)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                endpoint = Endpoint;
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                await RegisterAsync();
                endpoint = Endpoint;
            }
            var socketType = ZSocketType.PULL;
            Logger?.WriteLine($"Starting {Name} on {endpoint}.", new VectorTimeStamp().Update(SystemRegistration.Id, DateTime.Now));
            await StartAsync(endpoint, socketType, ReceiveAction);
        }

        internal void ReceiveAction(ZFrame frame)
        {
            var json = frame.ReadString();
            var record = JsonConvert.DeserializeObject<Record>(json);
            Repo.Create(record);
            // TODO: Tally them up an if threshold, Publish outbreak
        }
    }
}
