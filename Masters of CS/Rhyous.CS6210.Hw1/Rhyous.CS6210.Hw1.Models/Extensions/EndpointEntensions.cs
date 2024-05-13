using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.StringLibrary;

namespace Rhyous.CS6210.Hw1.Models
{
    public static class EndpointEntensions
    {
        public static Protocol GetProtocol(this string endpoint)
        {
            return endpoint.Split(':')[0].ToEnum<Protocol>() ?? Protocol.tcp;
        }

        public static IpAddress GetIp(this string endpoint)
        {
            var byteStrings = endpoint.Split(':')[1].Substring(2).Split('.');
            byte[] bytes = new byte[4];
            int i = 0;
            foreach (var byteStr in byteStrings)
            {
                bytes[i++] = byteStr.To<byte>();
            }
            return bytes;
        }

        public static ushort GetPort(this string endpoint)
        {
            return endpoint.Split(':')[2].To<ushort>();
        }
    }
}
