using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using System;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.LogClient
{
    public class LoggerClient : ILogger
    {
        public string LogServerName;
        public string LogServerEndpoint { get; set; }
        public SendClient SendClient { get; }
        public string ClientName;
        public RegistrationClient NsClient;
        private bool NsQueryInProgress;

        internal bool IsConnected { get; set; }

        public LoggerClient(string serverName, string clientName, string nsEndpoint) 
        {
            LogServerName = serverName;
            ClientName = clientName;
            SendClient = new SendClient(false, false);
            NsClient = new RegistrationClient(nsEndpoint, new SystemRegistration($"{ClientName}_LogClient"), false, null);
            var task = GetLogServerEndpointAsync(serverName);
        }
        
        public void WriteLine(string message)
        {
            if (string.IsNullOrWhiteSpace(LogServerEndpoint))
            {
                GetLogServerEndpointAsync(LogServerName);
                return;
            }
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

        public async Task<string> GetLogServerEndpointAsync(string logServerName)
        {
            if (NsQueryInProgress)
                return null;
            NsQueryInProgress = true;
            try
            {
                var systemRegistration = await NsClient.QueryAsync(logServerName, new VectorTimeStamp());
                LogServerEndpoint = systemRegistration?.EndPoint;
            }
            catch (Exception)
            {
                NsQueryInProgress = false;
                return null;
            }
            NsQueryInProgress = false;
            return LogServerEndpoint;
        }
    }
}