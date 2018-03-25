using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using System;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.LogServer
{
    public class LoggerServer : PullServer
    {
        internal bool AlsoLogOnConsole;
        internal VectorTimeStamp VTS = new VectorTimeStamp();
        public LoggerServer(string name, string nsEndpoint, ILogger logger) 
            : base(name, nsEndpoint, logger)
        {
        }

        public LoggerServer(string name, string endpoint, string nsEndpoint, ILogger logger)
            : base(name, endpoint, nsEndpoint, logger)
        {
        }

        public async Task StartAsync(string endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                endpoint = Endpoint;
            if (string.IsNullOrWhiteSpace(endpoint) && !IsRegistered)
                await RegisterAsync(VTS);
            endpoint = Endpoint;
            await StartAsync(Endpoint, ReceiveAction);
        }

        internal void ReceiveAction(ZFrame frame)
        {
            var msg = frame.ReadString();
            Logger.WriteLine(msg);
        }
    }
}