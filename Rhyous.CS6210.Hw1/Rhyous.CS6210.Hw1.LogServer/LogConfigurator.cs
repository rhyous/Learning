using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using System.Reflection;
using System.Text;

namespace Rhyous.CS6210.Hw1.LogServer
{
    class LogConfigurator
    {
        internal static ILog Log;

        public static void Configure(string logPath = null)
        {
            logPath = logPath ?? Assembly.GetExecutingAssembly().Location + ".log";
            var appender = new FileAppender()
            {
                Layout = new PatternLayout("%date (%p) %message%newline"),
                File = Assembly.GetExecutingAssembly().Location + ".log",
                Encoding = Encoding.UTF8,
                AppendToFile = true,
                LockingModel = new FileAppender.MinimalLock()
            };
            appender.ActivateOptions();

            // Step 4 - Configure log4Net to use the FileAppender
            BasicConfigurator.Configure(appender);
            Log = LogManager.GetLogger(logPath);
        }
    }
}
