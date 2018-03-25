using Rhyous.CS6210.Hw1.Models;
using Rhyous.SimpleArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw1.NameServer.Arguments
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
            Arguments.AddRange(CommonArguments.Create("NameServer", "6001"));
            Arguments.AddRange(new List<Argument>
            {
                // Add more args here
				// new Argument
                // {
                //     Name = "NextArg",
                //     ShortName = "N",
                //     Description = "This is the next arg you are going to add.",
                //     Example = "{name}=true",
                //     DefaultValue = "false"
                //     AllowedValues = CommonAllowedValues.TrueFalse
                // },
                new ConfigFileArgument(argsManager) // This is a special Argument type to allow for args in a file
            });
            Arguments.RemoveAt(2); // Remove common logger endpoint as this is logger
        }

        public override void HandleArgs(IReadArgs inArgsHandler)
        {
            base.HandleArgs(inArgsHandler);
            var task = Starter.StartAsync(Args.Value(Constants.Name),
                                          Args.Value(Constants.Endpoint),
                                          Args.Value(Constants.LoggerEndpoint)
                                         );
            task.Wait();
        }
    }
}
