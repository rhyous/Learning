using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using System;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.LogClient
{
    public class LoggerClient : ILogger
    {
        public string LogServerEndpoint { get; }
        public string ClientName { get; }
        public SendClient SendClient { get; }

        internal bool IsConnected { get; set; }

        public LoggerClient(string logServerEndpoint, string clientName) 
        {
            LogServerEndpoint = logServerEndpoint;
            ClientName = clientName;
            SendClient = new SendClient();
        }
        
        public void WriteLine(string message)
        {
            if (string.IsNullOrWhiteSpace(LogServerEndpoint))
                return; // Future - Store and query dns and then log when DNS is active
            if (!IsConnected)
            {
                SendClient.Connect(LogServerEndpoint, ZSocketType.PUSH);
                IsConnected = true;
            }
            SendClient.SendAsync($"{ClientName}: {message}");
        }
        public void WriteLine(string message, int id, DateTime? date = null)
        {
            var vts = new VectorTimeStamp().Update(id, date ?? DateTime.Now);
            WriteLine(message, vts);
        }

        public void WriteLine(string message, VectorTimeStamp vts)
        {
            WriteLine($"{ClientName}:{vts}: {message}");
        }
    }
}