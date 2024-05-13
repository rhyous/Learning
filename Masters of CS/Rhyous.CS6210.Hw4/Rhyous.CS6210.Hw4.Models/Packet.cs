using System;

namespace Rhyous.CS6210.Hw4.Models
{
    public class Packet<T>
    {
        public T Payload { get; set; }
        public string Type { get; set; }
        public DateTime Sent { get; set; }
        public int SenderId { get; set; }
    }
}
