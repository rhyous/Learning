using Newtonsoft.Json;

namespace Rhyous.CS6210.Hw1.Models
{
    public class RegistrationClient
    {
        public string NsEndpoint { get; }
        public SystemRegistration SystemRegistration { get; set; }

        public virtual SendClient RegClient { get; }

        public bool IsConnected { get { return RegClient?.Socket?.IsConnected ?? false; } }

        public RegistrationClient(string nsEndpoint, SystemRegistration systemRegistration)
        {
            NsEndpoint = nsEndpoint;
            SystemRegistration = systemRegistration;
            RegClient = new SendClient();
        }

        public bool Register(string nsEndpoint = null)
        {
            nsEndpoint = nsEndpoint ?? NsEndpoint;
            if (string.IsNullOrWhiteSpace(nsEndpoint))
                return false;
            RegClient.Connect(nsEndpoint, ZeroMQ.ZSocketType.REQ);

            var packet = new Packet<SystemRegistration>();
            packet.Payload = SystemRegistration;
            var json = JsonConvert.SerializeObject(packet);
            return IsConnected;
        }
    }
}
