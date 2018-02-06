using Rhyous.SimpleArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Simulator.Arguments
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
                    Name = Constants.EndPoint,
                    ShortName = "e",
                    Description = "The endpoint for the server service.",
                    Example = "{name}=tcp://127.0.0.1:5552",
                    DefaultValue = "tcp://127.0.0.1:5552",
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = Constants.SocketType,
                    ShortName = "s",
                    Description = "The SocketType",
                    Example = "{name}=1",
                    DefaultValue = "REP",
                    AllowedValues = new ObservableCollection<string>(Enum.GetNames(typeof(ZSocketType)))
                },
                new Argument
                {
                    Name = Constants.Name,
                    ShortName = "n",
                    Description = "The name of this simulator.",
                    DefaultValue = "Simulator1",
                    Example = "{name}=Simulator1"
                },
                new Argument
                {
                    Name = Constants.TimeMultiplier,
                    ShortName = "t",
                    Description = "This is how long 1 second simulates. For example: 3600 simulates 1 hour every second.",
                    DefaultValue = "3600",
                    Example = "{name}=3600",
                    CustomValidation = (val) => { return Regex.IsMatch(val, CommonAllowedValues.Digits); }
                },
                new Argument
                {
                    Name = Constants.Duration,
                    ShortName = "u",
                    Description = "The length of time in days the simulation runs. Max time simulation is 9999 days.",
                    DefaultValue = "31",
                    Example = "{name}=31",
                    CustomValidation = (val) => { return Regex.IsMatch(val, "[0-9]{4}"); }
                },
                new Argument
                {
                    Name = Constants.Year,
                    ShortName = "y",
                    Description = "The year the simulation starts.",
                    DefaultValue = "2018",
                    Example = "{name}=2018",
                    CustomValidation = (val) => { return Regex.IsMatch(val, "[0-9]{4}"); }
                },
                new Argument
                {
                    Name = Constants.Month,
                    ShortName = "m",
                    Description = "The month the simulation starts.",
                    DefaultValue = "01",
                    Example = "{name}=01",
                    CustomValidation = (val) => { return Regex.IsMatch(val, "[0]?[1-9]|[1][0-2]"); }
                },
                new Argument
                {
                    Name = Constants.Day,
                    ShortName = "d",
                    Description = "The day the simulation starts.",
                    DefaultValue = "01",
                    Example = "{name}=01",
                    CustomValidation = (val) => { return Regex.IsMatch(val, "[0]?[1-9]|[1-2][0-9]|[3][0-1]"); }
                },
                new Argument
                {
                    Name = Constants.LoggerEndpoint,
                    ShortName = "L",
                    Description = "The logger server endpoint.",
                    Example = "{name}=tcp://127.0.0.1:5501",
                    DefaultValue = "tcp://127.0.0.1:5501",
                    Action = (value) => { Console.WriteLine(value); }
                },
                new ConfigFileArgument(argsManager)
            });
        }

        public override void HandleArgs(IReadArgs inArgsHandler)
        {
            Starter.Start();
        }
    }
}
