using Rhyous.CS6210.Hw1.Models;
using Rhyous.SimpleArgs;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            Arguments.AddRange(CommonArguments.Create());
            Arguments.AddRange(new List<Argument>
            {
                new Argument
                {
                    Name = Constants.TimeMultiplier,
                    ShortName = "t",
                    Description = "This is how long 1 second simulates. For example: 3600 simulates 1 hour every second.",
                    DefaultValue = "3600",
                    Example = "{name}=3600",
                    CustomValidation = (val) => 
                    {
                        return Regex.IsMatch(val, CommonAllowedValues.Digits);
                    }
                },
                new Argument
                {
                    Name = Constants.Duration,
                    ShortName = "u",
                    Description = "The length of time in days the simulation runs. Max time simulation is 9999 days.",
                    DefaultValue = "31",
                    Example = "{name}=31",
                    CustomValidation = (val) => 
                    {
                        return Regex.IsMatch(val, "[1-9][0-9]{0,4}");
                    }
                },
                new Argument
                {
                    Name = Constants.Year,
                    ShortName = "y",
                    Description = "The year the simulation starts.",
                    DefaultValue = "2018",
                    Example = "{name}=2018",
                    CustomValidation = (val) => 
                    {
                        return Regex.IsMatch(val, "[1-9][0-9]{3}");
                    }
                },
                new Argument
                {
                    Name = Constants.Month,
                    ShortName = "m",
                    Description = "The month the simulation starts.",
                    DefaultValue = "01",
                    Example = "{name}=01",
                    CustomValidation = (val) => 
                    {
                        return Regex.IsMatch(val, "[0]?[1-9]|[1][0-2]");
                    }
                },
                new Argument
                {
                    Name = Constants.Day,
                    ShortName = "d",
                    Description = "The day the simulation starts.",
                    DefaultValue = "01",
                    Example = "{name}=01",
                    CustomValidation = (val) => 
                    {
                        return Regex.IsMatch(val, "[0]?[1-9]|[1-2][0-9]|[3][0-1]");
                    }
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
