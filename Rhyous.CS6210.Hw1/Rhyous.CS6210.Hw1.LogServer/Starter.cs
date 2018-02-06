using Rhyous.SimpleArgs;

namespace Rhyous.CS6210.Hw1.LogServer
{
    internal class Starter
    {
        internal static void Start()
        {
            LogConfigurator.Configure(Args.Value(Constants.File));
            var loggerEndpoint = Args.Value(Constants.LoggerEndpoint);
            var logServer = new LoggerServer(Args.Value(Constants.Name), LogConfigurator.Log);
            logServer.Start(loggerEndpoint, Args.Value(Constants.AlsoLogOnConsole).AsBool());
        }
    }
}