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
        internal TimeSpan DateTimeOffset;
        internal CryptoRandom Random;
        internal byte[] Subnet = new byte[] { 192, 168, 0 };
        internal int NextIp = 2;

        public DynamicNameServer(string name, CryptoRandom random, TimeSpan dateTimeOffset, ILogger logger)
        {
            Random = random;
            DateTimeOffset = dateTimeOffset;
            Logger = logger;
            SystemRegistration = new SystemRegistration { Name = name };
        }

        public IRepository<SystemRegistration> Repo
        {
            get { return _Repo ?? (_Repo = new Repository<SystemRegistration>()); }
            set { _Repo = value; }
        } private IRepository<SystemRegistration> _Repo;

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
        } private object Sync = new object() { };
        
        public void Start(string endpoint)
        {
            Start(endpoint, ZSocketType.REP, ReceiveAction);
        }

        internal void ReceiveAction(ZFrame frame)
        {
            var json = frame.ReadString();
            var vts = new VectorTimeStamp();
            Logger.WriteLine("Received: ");
            Logger.WriteLine(json);
            var packet = JsonConvert.DeserializeObject<Packet<SystemRegistration>>(json);
            var systemRegistration = packet.Payload;
            Logger.WriteLine($"Revieved system registration request from system: {systemRegistration.Name}.", vts);
            if (RegisterSystem(systemRegistration))
            {
                var response = new Packet<SystemRegistration>();
                response.Payload = systemRegistration;
                response.VectorTimeStamp = packet.VectorTimeStamp;
                response.VectorTimeStamp.Update(SystemRegistration, DateTime.Now.Add(DateTimeOffset));
                var responseJson = JsonConvert.SerializeObject(response);
                Send(responseJson);
            }
        }

        public byte GetIp()
        {
            return (byte)Random.Next(2, 254);
        }
    }
}