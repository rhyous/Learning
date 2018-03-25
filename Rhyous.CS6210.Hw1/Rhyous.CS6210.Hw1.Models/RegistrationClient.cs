using Newtonsoft.Json;
using Rhyous.CS6210.Hw1.Interfaces;
using System.Diagnostics;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class RegistrationClient
    {
        protected internal ILogger Logger;
        public bool IsRegistered { get; set; } 
        public string Endpoint { get; set; }
        public string NsEndpoint { get; }
        public SystemRegistration SystemRegistration { get; set; }

        public virtual SendClient RegClient { get; }

        public bool IsConnected { get { return RegClient?.Socket?.IsConnected ?? false; } }

        public RegistrationClient(string nsEndpoint, SystemRegistration systemRegistration, ILogger logger)
        {
            Logger = logger;
            NsEndpoint = nsEndpoint;
            SystemRegistration = systemRegistration;
            RegClient = new SendClient();
        }

        public async Task<bool> RegisterAsync(VectorTimeStamp vts = null, string nsEndpoint = null)
        {
            nsEndpoint = nsEndpoint ?? NsEndpoint;
            if (string.IsNullOrWhiteSpace(nsEndpoint))
                return false;
            RegClient.Connect(nsEndpoint, ZeroMQ.ZSocketType.REQ);
            Logger.WriteLine($"Connected to NameServer at {nsEndpoint}");
            var packet = new Packet<SystemRegistration> { Payload = SystemRegistration,  VectorTimeStamp = vts ?? new VectorTimeStamp()};
            var json = JsonConvert.SerializeObject(packet);
            await RegClient.SendAsync($"REG: {json}", (ZFrame frame) => {
                var responseJson = frame.ToString();
                var responsePacket = JsonConvert.DeserializeObject<Packet<SystemRegistration>>(responseJson);
                SystemRegistration = responsePacket.Payload;
                Endpoint = SystemRegistration.EndPoint;
                IsRegistered = true;
            });
            Logger.WriteLine($"Sent system registration to NameServer. Name: {SystemRegistration.Name}, Endpoint: {SystemRegistration.EndPoint}");
            return IsConnected;
        }

        public async Task<string> QueryAsync(string name, VectorTimeStamp vts, string nsEndpoint = null)
        {
            nsEndpoint = nsEndpoint ?? NsEndpoint;
            if (string.IsNullOrWhiteSpace(nsEndpoint))
                return null;
            RegClient.Connect(nsEndpoint, ZeroMQ.ZSocketType.REQ);
            Logger.WriteLine($"Connected to NameServer at {nsEndpoint}");
            var packet = new Packet<string> { Payload = name, VectorTimeStamp = vts };
            var json = JsonConvert.SerializeObject(packet);
            string result = null;
            await RegClient.SendAsync($"QRY: {json}", (ZFrame frame) => {
                var responseJson = frame.ToString();
                var responsePacket = JsonConvert.DeserializeObject<Packet<string>>(responseJson);
                result = responsePacket.Payload;
            });
            Logger.WriteLine($"Sent system registration to NameServer. Name: {SystemRegistration.Name}, Endpoint: {SystemRegistration.EndPoint}");

            await Awaiter.AwaitTrueAsync(() => { return result != null; }, 5000);
            return result;
            
        }
    }
}
