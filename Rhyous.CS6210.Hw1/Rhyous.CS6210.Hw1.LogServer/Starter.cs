using log4net;
using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.Models;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw1.LogServer
{
    internal class Starter
    {
        internal static ILogger Logger;
        public static LoggerServer LS { get; set; }
        internal static async Task StartAsync(string name, string endpoint, string nsEndpoint, ILog log, bool alsoLogToConsole)
        {
            Logger = new Log4NetLogger(log);
            if (alsoLogToConsole)
                Logger = new MultiLogger(Logger, new ConsoleLogger());
            Logger.WriteLine($"{name} has started.");

            LS = new LoggerServer(name, endpoint, nsEndpoint, Logger);
            await LS.RegisterAsync(LS.VTS, endpoint);
            while (!LS.IsRegistered)
            { }
            await LS.StartAsync(endpoint);
        }
    }
}