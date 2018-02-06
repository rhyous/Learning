using Rhyous.CS6210.Hw1.OutBreakAnalyzer.Arguments;
using Rhyous.SimpleArgs;
using System;

namespace Rhyous.CS6210.Hw1.OutBreakAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            ArgsManager.Instance.Start(new ArgsHandler(), args);
        }
    }
}