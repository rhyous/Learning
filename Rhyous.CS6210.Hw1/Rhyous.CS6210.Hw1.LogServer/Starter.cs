using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.LogClient;
using Rhyous.SimpleArgs;

namespace Rhyous.CS6210.Hw1.LogServer
{
    internal class Starter
    {
        internal static ILogger Logger;
        internal static void Start()
        {
            var name = Args.Value(Constants.Name);
            Logger = new LoggerClient(Args.Value(Constants.LoggerEndpoint), name);
            Logger.WriteLine($"{name} has started.");

            LogConfigurator.Configure(Args.Value(Constants.File));
            var loggerEndpoint = Args.Value(Constants.LoggerEndpoint);
            var logServer = new LoggerServer(Args.Value(Constants.Name), LogConfigurator.Log);
            logServer.Start(loggerEndpoint, Args.Value(Constants.AlsoLogOnConsole).AsBool());
        }
    }
}