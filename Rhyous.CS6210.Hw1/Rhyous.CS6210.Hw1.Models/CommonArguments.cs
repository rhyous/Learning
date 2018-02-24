using Rhyous.SimpleArgs;
using System.Collections.Generic;

namespace Rhyous.CS6210.Hw1.Models
{
    public class CommonArguments
    {
        public static List<Argument> Create()
        {
            return new List<Argument> {
                new Argument
                {
                    Name = Constants.Endpoint,
                    ShortName = "e",
                    Description = "The endpoint for the server service.",
                    Example = "{name}=tcp://127.0.0.1:5552",
                    DefaultValue = "tcp://127.0.0.1:5552"
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
                    Name = Constants.NameServerEndpoint,
                    ShortName = "e",
                    Description = "The endpoint for the name server service.",
                    Example = "{name}=tcp://127.0.0.1:6001",
                    DefaultValue = "tcp://127.0.0.1:6001"
                },
                new Argument
                {
                    Name = Constants.LoggerEndpoint,
                    ShortName = "L",
                    Description = "The logger server endpoint.",
                    Example = "{name}=tcp://127.0.0.1:5501",
                    DefaultValue = "tcp://127.0.0.1:5501"
                }
            };
        }
    }
}
