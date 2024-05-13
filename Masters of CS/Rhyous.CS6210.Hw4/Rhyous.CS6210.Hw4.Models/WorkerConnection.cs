using Newtonsoft.Json;

namespace Rhyous.CS6210.Hw4.Models
{
    public class WorkerConnection
    {
        public WorkerConnection() { }
        
        public WorkerConnection(IpAddress ipAddress, int port)
        {
            IpAddress = ipAddress;
            Port = port;
        }
        public string Name { get; set; }

        [JsonConverter(typeof(ToStringJsonConverter<IpAddress>))]
        public IpAddress IpAddress { get; set; }

        public int Port { get; set; }

        public override string ToString()
        {
            return $"{IpAddress}.{Port}";
        }

        public override bool Equals(object obj)
        {
            var castedObj = obj as WorkerConnection;
            if (castedObj is null)
                return false;
            return this == castedObj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(WorkerConnection left, WorkerConnection right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            return left.IpAddress == right.IpAddress 
                && left.Port == right.Port
                && left.Name == right.Name;
        }
        public static bool operator !=(WorkerConnection left, WorkerConnection right)
        {
            if (left is null && right is null)
                return false;
            if (left is null || right is null)
                return true;
            return left.IpAddress != right.IpAddress
                || left.Port != right.Port
                || left.Name != right.Name;
        }
    }
}
