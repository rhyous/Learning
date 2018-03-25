using Rhyous.CS6210.Hw1.Interfaces;
using System;

namespace Rhyous.CS6210.Hw1.Models
{
    public class ConsoleLogger : ILogger
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteLine(string message, int id, DateTime? date = null)
        {
            var vts = new VectorTimeStamp().Update(id, date ?? DateTime.Now);
            Console.WriteLine(message, vts);
        }

        public void WriteLine(string message, VectorTimeStamp vts)
        {
            Console.WriteLine(message, vts);
        }
    }
}
