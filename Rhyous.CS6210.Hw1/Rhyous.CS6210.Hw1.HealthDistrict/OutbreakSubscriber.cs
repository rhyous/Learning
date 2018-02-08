using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using System;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.HealthDistrict
{
    public class OutbreakSubscriber : Subsbriber
    {
        internal static ILogger Logger;

        public OutbreakSubscriber(ILogger logger) { Logger = logger; }
        internal void ReceiveAction(ZFrame frame)
        {
            var json = frame.ReadString();
            Console.WriteLine("** OUTBREAK ALLERT **");
            Console.WriteLine(json);
            Logger.WriteLine("Outbreak alert: " + json);

        }
    }
}
