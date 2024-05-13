using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Rhyous.CS6210.Hw4.Models
{
    [TypeConverter(typeof(IpAddressStringTypeConverter))]
    public partial class IpAddress
    {
        private readonly byte[] _Octets = new byte[4];

        [JsonIgnore]
        public byte Octet1 { get { return _Octets[0]; } set { _Octets[0] = value; } }
        [JsonIgnore]
        public byte Octet2 { get { return _Octets[1]; } set { _Octets[1] = value; } }
        [JsonIgnore]
        public byte Octet3 { get { return _Octets[2]; } set { _Octets[2] = value; } }
        [JsonIgnore]
        public byte Octet4 { get { return _Octets[3]; } set { _Octets[3] = value; } }
        
        public byte this[int i] { get => _Octets[i]; set => _Octets[i] = value; }

        public override string ToString() => $"{string.Join(".", _Octets)}";

        public override bool Equals(object obj)
        {
            var castedObj = obj as IpAddress;
            if (castedObj is null)
                return false;
            return this == castedObj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}