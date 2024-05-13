using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using System.Reflection;
using System.Text;

namespace Rhyous.CS6210.Hw1.InstanceLauncher
{
    class LogConfigurator
    {
        internal static void Configure()
        {
            var consoleAppender = new ConsoleAppender()
            {
                Layout = new PatternLayout("%date (%p) %message%newline")
            };
            var fileAppender = new FileAppender()
            {
                Layout = new PatternLayout("%date (%p) %message%newline"),
                File = Assembly.GetExecutingAssembly().Location + ".log",
                Encoding = Encoding.UTF8,
                AppendToFile = true,
                LockingModel = new FileAppender.MinimalLock()
            };
            fileAppender.ActivateOptions();

            BasicConfigurator.Configure(consoleAppender, fileAppender);
        }
    }
}
