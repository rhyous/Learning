using Rhyous.CS6210.Hw1.Models;
using Rhyous.SimpleArgs;
using System;
using System.Collections.Generic;

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
            Arguments.AddRange(CommonArguments.Create());
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
        }

        public override void HandleArgs(IReadArgs inArgsHandler)
        {
            base.HandleArgs(inArgsHandler);
            Console.WriteLine("I handled the args!!!");
        }
    }
}
