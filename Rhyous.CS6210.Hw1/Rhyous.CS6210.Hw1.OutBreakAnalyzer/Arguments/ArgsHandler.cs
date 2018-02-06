using Rhyous.SimpleArgs;
using System;
using System.Collections.Generic;

namespace Rhyous.CS6210.Hw1.OutBreakAnalyzer.Arguments
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
            Arguments.AddRange(new List<Argument>
            {
                new Argument
                {
                    Name = Constants.AnalyzerEndpoint,
                    ShortName = "a",
                    Description = "The endpoint for the server service.",
                    Example = "{name}=tcp://127.0.0.1:5555",
                    DefaultValue = "tcp://127.0.0.1:5555",
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = Constants.PublisherEndpoint,
                    ShortName = "p",
                    Description = "The endpoint for the server service.",
                    Example = "{name}=tcp://127.0.0.1:5556",
                    DefaultValue = "tcp://127.0.0.1:5556",
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = Constants.Name,
                    ShortName = "n",
                    Description = "The disease outbreak analyzer server name",
                    Example = "{name}=Measels Outbreak Analyzer"
                },
                new Argument
                {
                    Name = Constants.Disease,
                    ShortName = "d",
                    Description = "The disease this service analyzes.",
                    DefaultValue = "Influenza",
                    Example = "{name}=Influenza"
                },
                new Argument
                {
                    Name = Constants.Threshold,
                    ShortName = "t",
                    Description = "The number of diseases per day that indicate an outbreak.",
                    DefaultValue = "200",
                    Example = "{name}=200"
                },
                new Argument
                {
                    Name = Constants.LoggerEndpoint,
                    ShortName = "L",
                    Description = "The logger server endpoint.",
                    Example = "{name}=tcp://127.0.0.1:5501",
                    DefaultValue = "tcp://127.0.0.1:5501",
                    Action = (value) => { Console.WriteLine(value); }
                }
            });
        }

        public override void HandleArgs(IReadArgs inArgsHandler)
        {
            Starter.Start();            
        }
    }
}