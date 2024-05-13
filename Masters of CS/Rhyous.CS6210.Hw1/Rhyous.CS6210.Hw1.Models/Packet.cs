using System;

namespace Rhyous.CS6210.Hw1.Models
{
    public class Packet<T>
    {
        public T Payload { get; set; }
        public VectorTimeStamp VectorTimeStamp { get; set; }
        public DateTime Sent { get; set; }
        public int SenderId { get; set; }
    }
}
