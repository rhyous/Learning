using Newtonsoft.Json;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.CS6210.Hw1.Repository;
using System;
using System.Net;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.NameServer
{
    public class DynamicNameServer : SendServer
    {
        // By default Ids 0-9 reserved for names servers
        public static int DnsNextId = 0;
        public static int NextId = 10;
        public bool UseLocalHost = false;
        internal ILogger Logger;
        internal SystemRegistration SystemRegistration;
        internal CryptoRandom Random = new CryptoRandom();
        internal byte[] Subnet = new byte[] { 192, 168, 0 };
        internal int NextIp = 2;

        public DynamicNameServer(string name, ILogger logger)
        {
            Logger = logger;
            SystemRegistration = new SystemRegistration { Name = name };
        }

        public IRepository<SystemRegistration> Repo
        {
            get { return _Repo ?? (_Repo = new Repository<SystemRegistration>()); }
            set { _Repo = value; }
        }
        private IRepository<SystemRegistration> _Repo;

        public void SelfRegister()
        {
            RegisterSystem(SystemRegistration);
        }

        public bool RegisterSystem(SystemRegistration registration)
        {
            lock (Sync)
            {
                if (registration.Id > 0)
                {
                    var storedRegistration = Repo.Read(registration.Id);
                    if (storedRegistration != null)
                        registration = storedRegistration;
                }
                if (registration.Id < 1)
                {
                    registration.Id = NextId;
                    NextId++;
                }
                if (registration.Port == 0)
                {
                    registration.Port = (ushort)Random.Next(5000, 65000);
                }
                if (registration.IpAddress == null)
                {
                    registration.IpAddress = new IPAddress(new byte[] { Subnet[0], Subnet[1], Subnet[2], (byte)NextIp });
                }
                Logger.WriteLine($"System Registration: Id = {registration.Id}, Name = {registration.Name} ");
                var createdregistration = Repo.Create(registration);
                return true;
            }
        }
        private object Sync = new object() { };

        public void Start(string endpoint)
        {
            SelfRegister();
            Start(endpoint, ZSocketType.REP, ReceiveAction);
        }

        internal void ReceiveAction(ZFrame frame)
        {
            var json = frame.ReadString();
            Logger.WriteLine("Received: ");
            Logger.WriteLine(json);
            if (json.StartsWith("QRY"))
                RegisterSystem(json.Substring(5)); // Must prefix JSON with "QRY: "
            if (json.StartsWith("REG"))
                RegisterSystem(json.Substring(5)); // Must prefix JSON with "REG: "
        }
        private void QuerySystem(string json)
        {
            var packet = JsonConvert.DeserializeObject<Packet<NsQuery>>(json);
            var query = packet.Payload;
            var vts = packet.VectorTimeStamp;
            Logger.WriteLine($"Revieved query request from system: {packet.SenderId} for a system with name: {query.Name}.", vts.Update(SystemRegistration, DateTime.Now));
            var reg = Repo.Read(query.Name);
            var response = new Packet<SystemRegistration>
            {
                Payload = reg,
                VectorTimeStamp = vts.Update(SystemRegistration, DateTime.Now)
            };
            var responseJson = JsonConvert.SerializeObject(response);
            Send(responseJson);
        }

        private void RegisterSystem(string json)
        {
            var packet = JsonConvert.DeserializeObject<Packet<SystemRegistration>>(json.Substring(5));
            var systemRegistration = packet.Payload;
            var vts = packet.VectorTimeStamp;
            Logger.WriteLine($"Revieved system registration request from system: {systemRegistration.Name}.", vts.Update(SystemRegistration, DateTime.Now));
            if (RegisterSystem(systemRegistration))
            {
                var response = new Packet<SystemRegistration>
                {
                    Payload = systemRegistration,
                    VectorTimeStamp = packet.VectorTimeStamp.Update(SystemRegistration, DateTime.Now)
                };
                var responseJson = JsonConvert.SerializeObject(response);
                Send(responseJson);
            }
        }
    }
}