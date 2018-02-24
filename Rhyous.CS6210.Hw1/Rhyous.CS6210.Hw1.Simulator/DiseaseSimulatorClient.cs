using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Simulator
{
    public class DiseaseSimulatorClient : RegistrationClient
    {
        public DiseaseSimulatorClient(string name, ILogger logger, string nsEndpoint) 
            : base(nsEndpoint, new SystemRegistration { Name = name })
        {
            Logger = logger;
            Register();
        }

        public TimeSimulator TimeSimulator { get; set; }
        public IClient<ZFrame> Client { get; set; }
        public ILogger Logger { get; }

        internal void ReceiveAction(ZFrame frame)
        {
            Logger.WriteLine("Received:");
            Logger.WriteLine(frame.ReadString());
            if (TimeSimulator != null)
                TimeSimulator.IsReportingProgress = false;
        }
    }
}
