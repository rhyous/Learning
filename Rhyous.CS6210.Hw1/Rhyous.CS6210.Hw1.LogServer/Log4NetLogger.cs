using log4net;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using System;

namespace Rhyous.CS6210.Hw1.LogServer
{
    public class Log4NetLogger : ILogger
    {
        private ILog Log;
        public Log4NetLogger(ILog log4netLog)
        {
            Log = log4netLog;
        }

        public void WriteLine(string message)
        {
            Log.Debug(message);
        }

        public void WriteLine(string message, int id, DateTime? date = null)
        {
            var vts = new VectorTimeStamp().Update(id, date ?? DateTime.Now);
            Log.Debug($"{vts} - {message}");
        }

        public void WriteLine(string message, VectorTimeStamp vts)
        {
            Log.Debug($"{vts} - {message}");
        }
    }
}
