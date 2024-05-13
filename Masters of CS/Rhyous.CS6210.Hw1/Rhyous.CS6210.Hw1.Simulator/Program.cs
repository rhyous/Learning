using Rhyous.CS6210.Hw1.Interfaces;
using Rhyous.CS6210.Hw1.LogClient;
using Rhyous.CS6210.Hw1.Simulator.Arguments;
using Rhyous.SimpleArgs;

namespace Rhyous.CS6210.Hw1.Simulator
{
    static partial class Program
    {
        static void Main(string[] args)
        {
            new ArgsManager<ArgsHandler>().Start(args);
        }        
    }
}