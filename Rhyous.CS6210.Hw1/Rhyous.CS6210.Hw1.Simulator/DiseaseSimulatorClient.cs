using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Simulator
{
    public class DiseaseSimulatorClient : RegistrationClient
    {
        public DiseaseSimulatorClient(string name, string nsEndpoint, ILogger logger) 
            : base(nsEndpoint, new SystemRegistration(name), true, logger)
        {
        }

        public async Task StartAsync()
        {
            await RegisterAsync();
        }

        public TimeSimulator TimeSimulator { get; set; }
        public IClient<ZFrame> Client { get; set; }

        internal void ReceiveAction(ZFrame frame)
        {
            Logger?.WriteLine("Received:");
            Logger?.WriteLine(frame.ReadString());
            if (TimeSimulator != null)
                TimeSimulator.IsReportingProgress = false;
        }
    }
}
