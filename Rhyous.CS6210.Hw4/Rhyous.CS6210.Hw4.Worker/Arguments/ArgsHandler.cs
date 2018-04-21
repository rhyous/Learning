using log4net;
using Rhyous.SimpleArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Rhyous.CS6210.Hw4
{
    // Add this line of code to Main() in Program.cs
    //   new ArgsManager<ArgsHandler>().Start(args);

    /// <summary>
    /// A class that implements IArgumentsHandler where command line
    /// arguments are defined.
    /// </summary>
    public sealed class ArgsHandler : ArgsHandlerBase
    {
        public const string Name = "Name";
        public const string PrimaryDirectory = "PrimaryDirectory";
        public const string IpAddress = "IpAddress";
        public const string Port = "Port";
        public const string Repo = "Repo";
        public static string Bucket = "Bucket";

        public override void InitializeArguments(IArgsManager argsManager)
        {
            Arguments.AddRange(new List<Argument>
            {
                new Argument
                {
                    Name = Name,
                    ShortName = "n",
                    Description = "The worker name or if no name provided, the endpoint.",
                    Example = @"{name}=Worker1",
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = PrimaryDirectory,
                    ShortName = "d",
                    Description = "The folder",
                    Example = @"{name}=c:\path\to\PrimaryDirectory",
                    IsRequired = true,
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = IpAddress,
                    ShortName = "ip",
                    Description = "This worker's Ip Address",
                    Example = @"{name}=10.1.1.2",
                    IsRequired = true,
                    CustomValidation = (value)=>
                    {
                        return Regex.IsMatch(value, Models.IpAddress.NaiveRegexPattern);
                    },
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = Port,
                    ShortName = "p",
                    Description = "This worker's Ip Address",
                    Example = @"{name}=2700",
                    DefaultValue = "2700",
                    CustomValidation = (value) => Regex.IsMatch(value, CommonAllowedValues.Digits),
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = Repo,
                    ShortName = "r",
                    Description = "The repository for hte folder structer: local filesystem or Amazon S3",
                    Example = @"{name}=Local",
                    DefaultValue = "Local",
                    AllowedValues = new ObservableCollection<string>{ "Local", "S3" },
                    Action = (value) => { Console.WriteLine(value); }
                },
                new Argument
                {
                    Name = "AccessKey",
                    ShortName = "ak",
                    Description = "The access access key provided to you by Amazon.",
                    Example = "{name}=AKA7PXD3H7MN2AWZSR9", // Fake key I made up,
                    IsRequired = true,
                    Action = (value) =>
                    {
                        Console.WriteLine(value);
                    }
                },
                new Argument
                {
                    Name = "SecretKey",
                    ShortName = "sk",
                    Description = "The secret access key provided to you by Amazon.",
                    Example = "{name}=RvOxT8JLRL8P57oEvJYfqVr0YcoQb7Xo0CN2YcBvL", // Fake key I made up
                    IsRequired = true,
                    Action = (value) =>
                    {
                        Console.WriteLine(value);
                    }
                },
                new Argument
                {
                    Name = "Bucket",
                    ShortName = "b",
                    Description = "The bucket to use.",
                    Example = "{name}=my.first.bucket",
                    DefaultValue = "my.first.bucket",
                    Action = (value) =>
                    {
                        Console.WriteLine(value);
                    }
                },
                new ConfigFileArgument(argsManager) // This is a special Argument type to allow for args in a file
            });
        }

        public override void HandleArgs(IReadArgs inArgsHandler)
        {
            base.HandleArgs(inArgsHandler);			
        }
    }
}
