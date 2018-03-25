using Rhyous.CS6210.Hw1.Interfaces;
using System;

namespace Rhyous.CS6210.Hw1.Models
{
    public class MultiLogger : ILogger
    {
        ILogger[] Loggers;
        public MultiLogger(params ILogger[] loggers)
        {
            Loggers = loggers;
        }

        public void WriteLine(string message)
        {
            foreach (var logger in Loggers)
            {
                logger.WriteLine(message);
            }
        }

        public void WriteLine(string message, int id, DateTime? date = null)
        {
            foreach (var logger in Loggers)
            {
                logger.WriteLine(message, id, date);
            }
        }

        public void WriteLine(string message, VectorTimeStamp vts)
        {
            foreach (var logger in Loggers)
            {
                logger.WriteLine(message, vts);
            }
        }
    }
}