using Rhyous.SimpleArgs;
using System;
using System.Collections.Generic;

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
                    Name = Constants.PublisherEndpoint,
                    ShortName = "p",
                    Description = "The endpoint for the server service.",
                    Example = "{name}=tcp://127.0.0.1:5553",
                    DefaultValue = "tcp://127.0.0.1:5553",
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = Constants.DistrictServerName,
                    ShortName = "n",
                    Description = "The endpoint name",
                    DefaultValue = "EndPoint1",
                    Example = "{name}=Endpoint1"                    
                }
            });
        }

        public override void HandleArgs(IReadArgs inArgsHandler)
        {
            Starter.Start();
        }
    }
}