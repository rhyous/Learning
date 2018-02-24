using Rhyous.CS6210.Hw1.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Rhyous.CS6210.Hw1.Models
{
    public class SystemRegistration : IEntity
    {
        [Key]
        public int Id { get; set; } = -1;

        public string Name { get; set; }
        [NotMapped]

        public Protocol Protocol { get; set; }
        [Column("Protocol")]
        public string ProtocolString { get; set; }

        [NotMapped]
        public IPAddress IpAddress { get; set; }
        [MinLength(4), MaxLength(4)]
        public byte[] IPAddressBytes
        {
            get { return IpAddress?.GetAddressBytes(); }
            set { IpAddress = new IPAddress(value); }
        }

        public ushort Port { get; set; }

        [NotMapped]
        public string EndPoint { get { return $"{Protocol}://{IpAddress}:{Port}"; } }
    }
}