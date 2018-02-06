using Rhyous.CS6210.Hw1.Interfaces;
using System;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Simulator
{
    public class DiseaseSimulatorClient
    {
        public TimeSimulator TimeSimulator { get; set; }
        public IClient<ZFrame> Client { get; set; }

        internal void ReceiveAction(ZFrame frame)
        {
            Console.WriteLine("Received:");
            Console.WriteLine(frame.ReadString());
            if (TimeSimulator != null)
                TimeSimulator.IsReportingProgress = false;
        }
    }
}
