using log4net;
using Rhyous.CS6210.Hw1.Models;
using System;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.LogServer
{
    public class LoggerServer : PullServer
    {
        internal ILog Logger;
        internal string Endpoint;
        internal bool AlsoLogOnConsole;
        public LoggerServer(string name, ILog logger, string nsEndpoint) : base(name, nsEndpoint)
        {
            Logger = logger;
        }

        public void Start(string endpoint, bool alsoLogOnConsole)
        {
            Register();
            Endpoint = endpoint;
            AlsoLogOnConsole = alsoLogOnConsole;
            Start(Endpoint, ReceiveAction);
        }

        internal void ReceiveAction(ZFrame frame)
        {
            var msg = frame.ReadString();
            Logger.Debug(msg);
            if (AlsoLogOnConsole)
                Console.WriteLine(msg);
        }
    }
}