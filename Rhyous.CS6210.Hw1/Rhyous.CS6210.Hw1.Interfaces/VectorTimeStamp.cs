using System;
using System.Collections.Generic;

namespace Rhyous.CS6210.Hw1.Models
{
    public class VectorTimeStamp : Dictionary<int, VectorCounter>
    {
        public override string ToString()
        {
            var str = "";
            foreach (var kvp in this)
            {
                if (!string.IsNullOrWhiteSpace(str))
                    str += ",";
               str += $"[{kvp.Key}:{kvp.Value}";
            }
            return str + "]";
        }

        public VectorTimeStamp Update(SystemRegistration systemRegistration, DateTime dateTime)
        {
            VectorCounter counter;
            if (TryGetValue(systemRegistration.Id, out counter))
            {
                counter.Counter++;
                return this;
            }
            this[systemRegistration.Id] = new VectorCounter { Counter = 1, Date = dateTime };
            return this;
        }
    }
}