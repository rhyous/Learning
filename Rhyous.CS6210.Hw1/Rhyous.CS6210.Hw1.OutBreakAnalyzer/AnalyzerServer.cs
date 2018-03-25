using Newtonsoft.Json;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.CS6210.Hw1.Repository;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.OutBreakAnalyzer
{
    public class AnalyzerServer : SendServer, IName
    {
        internal ILogger Logger;

        public IRepository<Record> Repo = new Repository<Record>();
        internal SystemRegistration SystemRegistration;

        public AnalyzerServer(string name, ILogger logger)
        {
            SystemRegistration = new SystemRegistration(name);
            Name = name;
            Logger = logger;

        }

        public string Name
        {
            get { return SystemRegistration.Name; }
            internal set { SystemRegistration.Name = value; }
        }

        public async Task StartAsync(string endpoint)
        {
            var socketType = ZSocketType.PULL;
            Logger.WriteLine($"Starting {Name} on {endpoint}.", new VectorTimeStamp());
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
