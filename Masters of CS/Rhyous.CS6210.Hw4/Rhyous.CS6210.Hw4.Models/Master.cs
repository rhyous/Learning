using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Rhyous.CS6210.Hw4.Models
{
    public enum MasterState
    {
        Starting,
        Elected,
        Resigned
    }

    public class Master : IComparable
    {
        public Master() {  }

        public WorkerConnection Connection { get; set; }
        
        public List<WorkerConnection> Workers
        {
            get { return _Slaves ?? (_Slaves = new List<WorkerConnection>()); }
            set { _Slaves = value; }
        } private List<WorkerConnection> _Slaves;

        public static bool operator >(Master left, Master right)
        {
            if (left is null)
                return false;
            if (right is null)
                return true;
            var lip = AppendToLong(left.Connection?.IpAddress ?? 0, left.Connection?.Port ?? 0);
            var rip = AppendToLong(right.Connection?.IpAddress ?? 0, right.Connection?.Port ?? 0);
            return lip > rip;
        }

        public static bool operator <(Master left, Master right)
        {
            if (left is null)
                return false;
            if (right is null)
                return true;
            var lip = AppendToLong(left.Connection?.IpAddress ?? 0, left.Connection.Port);
            var rip = AppendToLong(right.Connection?.IpAddress ?? 0, right.Connection?.Port ?? 0);
            return lip < rip;
        }

        public static bool operator ==(Master left, Master right)
        {
            if (left is null && right is null)
                return true;
            if (right is null || left is null)
                return false;
            var lip = AppendToLong(left.Connection?.IpAddress ?? 0, left.Connection.Port);
            var rip = AppendToLong(right.Connection?.IpAddress ?? 0, right.Connection?.Port ?? 0);
            return lip == rip;
        }

        public static bool operator !=(Master left, Master right)
        {
            return !(left == right);
        }

        internal static long AppendToLong(int i1, int i2)
        {
            long l = 0L;
            l = l | (uint)i1;
            l = (l << 32);
            l = l | (uint)i2;
            return l;
        }

        public override string ToString()
        {
            return Connection.ToString();
        }

        public int CompareTo(object obj)
        {
            var master = obj as Master;
            if (obj == null )
                return 1;
            if (this > master)
                return 1;
            if (this < master)
                return -1;
            if (this == master)
                return 0;
            return 1;
        }
    }
}
