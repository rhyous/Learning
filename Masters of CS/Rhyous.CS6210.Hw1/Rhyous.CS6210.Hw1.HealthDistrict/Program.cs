using Rhyous.CS6210.Hw1.HealthDistrict.Arguments;
using Rhyous.SimpleArgs;

namespace Rhyous.CS6210.Hw1.HealthDistrict
{
    class Program
    {
        static void Main(string[] args)
        {
            new ArgsManager<ArgsHandler>().Start(args);
        }
    }
}