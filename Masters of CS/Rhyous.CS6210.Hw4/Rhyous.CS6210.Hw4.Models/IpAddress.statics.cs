using Rhyous.StringLibrary;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rhyous.CS6210.Hw4.Models
{
    public partial class IpAddress
    {
        #region Implicit operators
        public static implicit operator int(IpAddress address) => BitConverter.ToInt32(address._Octets, 0);

        public static implicit operator byte[] (IpAddress address) => address._Octets;

        public static implicit operator IpAddress(byte[] octets)
        {
            if (octets == null || octets.Length != 4)
                throw new ArgumentException("You must pass in 4 octets", "octets");
            var ip = new IpAddress();
            Array.Copy(octets, ip._Octets, 4);
            return ip;
        }

        public static implicit operator IpAddress(string ip)
        {
            if (!Regex.IsMatch(ip, NaiveRegexPattern))
                return null;
            var octetStrings = ip.Split('.');
            return octetStrings.Select(o => o.To<byte>()).ToArray();
        }
        #endregion

        #region Operator Overloads
        public static bool operator >(IpAddress left, IpAddress right)
        {
            if (left is null)
                return false;
            if (right is null)
                return true;
            return ((int)left) > (int)right;
        }
        public static bool operator <(IpAddress left, IpAddress right)
        {
            if (left is null)
                return false;
            if (right is null)
                return true;
            return (int)left < (int)right;
        }
        public static bool operator ==(IpAddress left, IpAddress right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            return ((int)left) == (int)right;
        }
        public static bool operator !=(IpAddress left, IpAddress right)
        {
            if (left is null && right is null)
                return false;
            if (left is null || right is null)
                return true;
            return (int)left != (int)right;
        }
        #endregion

        public static IpAddress LocalHost => new byte[] { 127, 0, 0, 1 };   
        
        public const string NaiveRegexPattern = @"^([0-9]{1,3}\.){3}[0-9]{1,3}$";
    }
}