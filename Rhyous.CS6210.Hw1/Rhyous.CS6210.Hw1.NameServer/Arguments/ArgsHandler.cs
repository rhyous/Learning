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
				 new Argument
                 {
                     Name = "UseLocalHost",
                     ShortName = "ulh",
                     Description = "Use localhost, 127.0.0.1 for all registrations.",
                     Example = "{name}=true",
                     DefaultValue = "false",
                     AllowedValues = CommonAllowedValues.TrueFalse
                 },
                new ConfigFileArgument(argsManager) // This is a special Argument type to allow for args in a file
            });
            Arguments.RemoveAt(2); // Remove common NameServer endpoint from comon as this is Name Server, so Enpoint is used.
        }

        public override void HandleArgs(IReadArgs inArgsHandler)
        {
            base.HandleArgs(inArgsHandler);
            var task = Starter.StartAsync(Args.Value(Constants.Name),
                                          Args.Value(Constants.Endpoint),
                                          Args.Value(Constants.LoggerServerName),
                                          Args.Value(Constants.UseLocalHost).AsBool()
                                         );
            task.Wait();
        }
    }
}
