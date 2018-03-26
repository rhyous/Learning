using Newtonsoft.Json;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using Rhyous.CS6210.Hw1.Repository;
using System;
using System.Net;
using System.Threading.Tasks;
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
        internal IpAddress Subnet { get { return UseLocalHost ? IpAddress.LocalHost : ConfiguredSubnet ?? DefaultSubnet; } }
        private IpAddress DefaultSubnet = new byte[] { 192, 168, 0, 0 };
        private IpAddress ConfiguredSubnet;
        internal int NextIp = 2;

        public DynamicNameServer(string name, ILogger logger, IpAddress subnet = null)
        {
            Logger = logger;
            SystemRegistration = new SystemRegistration(name);
            ConfiguredSubnet = subnet;
        }

        public IRepository<SystemRegistration> Repo
        {
            get { return _Repo ?? (_Repo = new Repository<SystemRegistration>()); }
            set { _Repo = value; }
        } private IRepository<SystemRegistration> _Repo;

        public async Task SelfRegister()
        {
            await RegisterSystem(SystemRegistration);
        }

        public async Task<bool> RegisterSystem(SystemRegistration registration)
        {
            return await Task.Run(() =>
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
                        registration.IpAddress = UseLocalHost 
                                               ? IpAddress.LocalHost 
                                               : (IpAddress)new byte[] { Subnet[0], Subnet[1], Subnet[2], (byte)NextIp };
                    }
                    Logger?.WriteLine($"System Registration: Id = {registration.Id}, Name = {registration.Name} ");
                    var createdregistration = Repo.Create(registration);
                    return true;
                }
            });
        }
        private object Sync = new object() { };

        public async Task StartAsync(string endpoint)
        {
            await SelfRegister();
            await StartAsync(endpoint, ZSocketType.REP, ReceiveAction);
        }

        internal void ReceiveAction(ZFrame frame)
        {
            var json = frame.ReadString();
            Logger?.WriteLine("Received: ");
            Logger?.WriteLine(json);
            if (json.StartsWith("QRY"))
                QuerySystem(json.Substring(5)).Wait(); // Must prefix JSON with "QRY: "
            if (json.StartsWith("REG"))
                RegisterSystem(json.Substring(5)).Wait(); // Must prefix JSON with "REG: "
        }

        private async Task QuerySystem(string json)
        {
            var packet = JsonConvert.DeserializeObject<Packet<NsQuery>>(json);
            var query = packet.Payload;
            var vts = packet.VectorTimeStamp;
            Logger?.WriteLine($"Revieved query request from system: {packet.SenderId} for a system with name: {query.Name}.", vts.Update(SystemRegistration.Id, DateTime.Now));
            var reg = Repo.Read(query.Name);
            var response = new Packet<SystemRegistration>
            {
                Payload = reg,
                VectorTimeStamp = vts.Update(SystemRegistration.Id, DateTime.Now)
            };
            var responseJson = JsonConvert.SerializeObject(response);
            await SendAsync(responseJson);
        }

        private async Task RegisterSystem(string json)
        {
            var packet = JsonConvert.DeserializeObject<Packet<SystemRegistration>>(json);
            var systemRegistration = packet.Payload;
            var vts = packet.VectorTimeStamp;
            Logger?.WriteLine($"Revieved system registration request from system: {systemRegistration.Name}.", vts.Update(SystemRegistration.Id, DateTime.Now));
            if (await RegisterSystem(systemRegistration))
            {
                var response = new Packet<SystemRegistration>
                {
                    Payload = systemRegistration,
                    VectorTimeStamp = packet.VectorTimeStamp.Update(SystemRegistration.Id, DateTime.Now)
                };
                var responseJson = JsonConvert.SerializeObject(response);
                await SendAsync(responseJson);
            }
        }
    }
}