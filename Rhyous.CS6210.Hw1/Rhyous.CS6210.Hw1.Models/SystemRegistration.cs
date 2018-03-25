using Rhyous.CS6210.Hw1.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Rhyous.StringLibrary;

namespace Rhyous.CS6210.Hw1.Models
{
    public class SystemRegistration : IEntity
    {
        public SystemRegistration() { }

        public SystemRegistration(string name) => Name = name;

        public SystemRegistration(string name, IpAddress address, Protocol protocol, ushort port)
        {
            Name = name;
            IpAddress = address;
            Protocol = Protocol;
            Port = port;
        }
        public SystemRegistration(string name, string endpoint) : this(name)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                return;
            IpAddress = endpoint.GetIp();
            Protocol = endpoint.GetProtocol();
            Port = endpoint.GetPort();
        }

        [Key]
        public int Id { get; set; } = -1;

        public string Name { get; set; }

        [NotMapped]
        public Protocol Protocol { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [Column("Protocol")]
        public string ProtocolString
        {
            get { return Protocol.ToString(); }
            set { Protocol = value.ToEnum<Protocol>() ?? Protocol.tcp; }
        }

        [NotMapped]
        public IpAddress IpAddress { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [MinLength(4), MaxLength(4)]
        public byte[] IPAddressBytes
        {
            get { return IpAddress; }
            set { IpAddress = value; }
        }

        public ushort Port { get; set; }

        [NotMapped]
        public string EndPoint { get { return $"{Protocol}://{IpAddress}:{Port}"; } }
    }
}