using Rhyous.SimpleArgs;

namespace Rhyous.CS6210.Hw1.LogServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new ArgsManager<ArgsHandler>().Start(args);
        }
    }
}