using Rhyous.CS6210.Hw1.NameServer.Arguments;
using Rhyous.SimpleArgs;

namespace Rhyous.CS6210.Hw1.NameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new ArgsManager<ArgsHandler>().Start(args);
        }
    }
}