using Rhyous.SimpleArgs;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.CS6210.Hw1.LogServer
{
    // Add this line of code to Main() in Program.cs
    //
    //   ArgsManager.Instance.Start(new ArgsHandler(), args);
    //

    /// <summary>
    /// A class that implements IArgumentsHandler where command line
    /// arguments are defined.
    /// </summary>
    public sealed class ArgsHandler : ArgsHandlerBase
    {
        public ArgsHandler()
        {
            Arguments = new List<Argument>
            {
                new Argument
                {
                    Name = Constants.LoggerEndpoint,
                    ShortName = "e",
                    Description = "The logger endpoint.",
                    Example = "{name}=tcp://127.0.0.1:5501",
                    DefaultValue = "tcp://127.0.0.1:5501",
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = Constants.File,
                    ShortName = "f",
                    Description = "The log file.",
                    Example = "{name}=tcp://127.0.0.1:5501",
                    DefaultValue = Assembly.GetExecutingAssembly().Location + ".log",
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = Constants.Name,
                    ShortName = "n",
                    Description = "The logger name.",
                    Example = "{name}=AwesomeLogger",
                    DefaultValue = "LogServer",
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
            };
        }

        public override void HandleArgs(IReadArgs inArgsHandler)
        {
            Starter.Start();
        }
    }
}