using Amazon.EC2.Util;
using log4net;
using Rhyous.SimpleArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Rhyous.CS6210.Hw1.InstanceLauncher
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
        static ILog Log = LogManager.GetLogger(Assembly.GetExecutingAssembly().Location + ".log");

        public override void InitializeArguments(IArgsManager argsManager)
        {
            Arguments.AddRange(new List<Argument>
            {
                new Argument
                {
                    Name = "AccessKey",
                    ShortName = "ak",
                    Description = "The access access key provided to you by Amazon.",
                    Example = "{name}=AKA7PXD3H7MN2AWZSR9", // Fake key I made up
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "SecretKey",
                    ShortName = "sk",
                    Description = "The secret access key provided to you by Amazon.",
                    Example = "{name}=RvOxT8JLRL8P57oEvJYfqVr0YcoQb7Xo0CN2YcBvL", // Fake key I made up
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "ImageId",
                    ShortName = "i",
                    Description = "The operating system image to use. From the ImageUtilities class.",
                    Example = "{name}=WINDOWS_2012_BASE",
                    DefaultValue = "WINDOWS_2012_BASE",
                    AllowedValues = new ObservableCollection<string>(ImageUtilities.ImageKeys),
                    Action = (value) =>
                    {
                        Log.Debug(value);
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
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "InstanceId",
                    ShortName = "id",
                    Description = "The instance id to use.",
                    Example = "{name}=1",
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "InstanceName",
                    ShortName = "n",
                    Description = "The instance name to use.",
                    Example = "{name}=MyServer1",
                    DefaultValue = "",
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "Role",
                    ShortName = "R",
                    Description = "The instance role.",
                    Example = "{name}=MyNewRole",
                    DefaultValue = "MyNewRole",
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "File",
                    ShortName = "F",
                    Description = "A file.",
                    Example = "{name}=C:\\some\\file.txt",
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "KeyName",
                    ShortName = "k",
                    Description = "A key pair name.",
                    Example = "{name}=MyKeyPair",
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "KeyPairFile",
                    ShortName = "pem",
                    Description = "A key pair file name.",
                    Example = "{name}=MyKeyPair.pem",
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "InstanceProfile",
                    ShortName = "P",
                    Description = "The name of the instane profile.",
                    Example = "{name}=Profile1",
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "LocalDirectory",
                    ShortName = "ld",
                    Description = "The local directory to copy to S3.",
                    Example = "{name}=C:\\TestDir",
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "RemoteDirectory",
                    ShortName = "rd",
                    Description = "The remote directory on the S3 Bucket.",
                    Example = "{name}=/My/Remote/Directory",
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "InstanceDirectory",
                    ShortName = "idir",
                    Description = "The directory local to the instance server.",
                    Example = "{name}=C:\\TestDir",
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "LaunchScript",
                    ShortName = "script",
                    Description = "A powershell script to run at instance launch.The local directory to copy to S3.",
                    Example = "{name}=C:\\MyDir\\MyScript.ps1",
                    CustomValidation = (value) => File.Exists(value),
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "Region",
                    Description = "The region",
                    Example = "{name}=us-west-2",
                    DefaultValue = ConfigurationManager.AppSettings["AWSRegion"],
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                },
                new Argument
                {
                    Name = "SkipSteps",
                    ShortName = "skip",
                    Description = "Steps to skip in a comma separated list.",
                    Example = "{name}=UploadFiles,StartInstance",
                    Action = (value) =>
                    {
                        Log.Debug(value);
                    }
                }
            });
        }

        public override void HandleArgs(IReadArgs inArgsHandler)
        {
            base.HandleArgs(inArgsHandler);
            var task = Program.Start();
            task.Wait();
        }
    }
}