using Rhyous.SimpleArgs;

namespace Rhyous.CS6210.Hw1.LogServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ArgsManager.Instance.Start(new ArgsHandler(), args);
        }
    }
}