using Rhyous.CS6210.Hw1.Models;
using Rhyous.SimpleArgs;
using Rhyous.StringLibrary;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rhyous.CS6210.Hw1.HealthDistrict.Arguments
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
                    Name = Constants.DistrictServerEndPoint,
                    ShortName = "d",
                    Description = "The District Server endpoint.",
                    Example = "{name}=tcp://127.0.0.1:5552",
                    DefaultValue = "tcp://127.0.0.1:5552",
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = Constants.PublisherEndpoints,
                    ShortName = "p",
                    Description = "A comma separate array of endpoints for the publishers.",
                    Example = "{name}=tcp://127.0.0.1:5553,tcp://127.0.0.1:5554",
                    DefaultValue = "tcp://127.0.0.1:5553,tcp://127.0.0.1:5554",
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = Constants.DistrictServerName,
                    ShortName = "n",
                    Description = "The endpoint name",
                    DefaultValue = "Health District 1",
                    Example = "{name}=Endpoint1"
                }
            });
        }

        public override void HandleArgs(IReadArgs inArgsHandler)
        {
            var timespan = new TimeSpan(Args.Value(Constants.DateTimeOffset).To(0));
            Starter.Start(timespan);
        }
    }
}