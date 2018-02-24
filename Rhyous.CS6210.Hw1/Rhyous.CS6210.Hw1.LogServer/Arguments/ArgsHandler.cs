using Rhyous.CS6210.Hw1.Models;
using Rhyous.SimpleArgs;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.CS6210.Hw1.LogServer
{
    // Add this line of code to Main() in Program.cs
    //
    //   new ArgsManager<ArgsHandler>().Start(args);
    //

    /// <summary>
    /// A class that implements IArgumentsHandler where command line
    /// arguments are defined.
    /// </summary>
    public sealed class ArgsHandler : ArgsHandlerBase
    {
        public override void InitializeArguments(IArgsManager argsManager)
        {
            Arguments.AddRange(CommonArguments.Create("AwesomeLogger", "5501"));
            Arguments.AddRange(new List<Argument>
            {
                new Argument
                {
                    Name = Constants.File,
                    ShortName = "f",
                    Description = "The log file.",
                    Example = "{name}=c:\\logs\\log.txt",
                    DefaultValue = Assembly.GetExecutingAssembly().Location + ".log",
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = Constants.AlsoLogOnConsole,
                    ShortName = "c",
                    Description = "This will log to both a file and the console window.",
                    Example = "{name}=true",
                    DefaultValue = "true",
                    AllowedValues = CommonAllowedValues.TrueFalse,
                    Action = (value) => { Console.WriteLine(value); }
                },
                new ConfigFileArgument(argsManager) // This is a special Argument type to allow for args in a file 
            });
            Arguments.RemoveAt(3); // Remove common logger endpoint as this is logger
        }

        public override void HandleArgs(IReadArgs inArgsHandler)
        {
            Starter.Start();
        }
    }
}