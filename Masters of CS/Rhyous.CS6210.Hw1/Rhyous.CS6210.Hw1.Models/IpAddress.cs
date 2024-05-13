using System;

namespace Rhyous.CS6210.Hw1.Models
{
    public class IpAddress
    {
        private readonly byte[] _Octets = new byte[4];
        public byte Octet1 { get { return _Octets[0]; } set { _Octets[0] = value; } }
        public byte Octet2 { get { return _Octets[1]; } set { _Octets[1] = value; } }
        public byte Octet3 { get { return _Octets[2]; } set { _Octets[2] = value; } }
        public byte Octet4 { get { return _Octets[3]; } set { _Octets[3] = value; } }

        #region Implicit operators
        public static implicit operator byte[] (IpAddress address)
        {
            return address._Octets;
        }

        public static implicit operator IpAddress(byte[] octets)
        {
            if (octets == null || octets.Length != 4)
                throw new ArgumentException("You must pass in 4 octets", "octets");
            var ip = new IpAddress();
            Array.Copy(octets, ip._Octets, 4);
            return ip;
        }
        #endregion

        #region Indexers
        public byte this[int i]
        {
            get { return _Octets[i]; }
            set { _Octets[i] = value; }
        }
        #endregion

        public static IpAddress LocalHost => new byte[] { 127, 0, 0, 1 };

        public override string ToString()
        {
            return $"{string.Join(".", _Octets)}";
        }
    }
}
