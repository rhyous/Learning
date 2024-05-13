using log4net;
using System;

namespace Rhyous.CS6210.Hw4
{
    public interface IWorkerLogger
    {
        void Debug(string msg);
    }

    public class MultiLogger : IWorkerLogger
    {
        private readonly IWorkerLogger[] _Loggers;
        private readonly string _Name;
        public MultiLogger(string name, params IWorkerLogger[] loggers)
        {
            _Name = name;
            _Loggers = loggers;
        }
        public void Debug(string msg)
        {
            foreach (var logger in _Loggers)
            {
                logger?.Debug($"{_Name}: {msg}");
            }
        }
    }

    public class ConsoleLogger : IWorkerLogger
    {
        public void Debug(string msg)
        {
            Console.WriteLine(msg);
        }
    }

    public class Log4NetLogger : IWorkerLogger
    {
        private readonly ILog _Logger;
        public Log4NetLogger(ILog log4netLogger) { _Logger = log4netLogger; }
        public void Debug(string msg)
        {
            _Logger.Debug(msg);
        }
    }
}
