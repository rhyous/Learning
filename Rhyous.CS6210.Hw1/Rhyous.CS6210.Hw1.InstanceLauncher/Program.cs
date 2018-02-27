/*******************************************************************************
* Copyright 2009-2018 Amazon.com, Inc. or its affiliates. All Rights Reserved.
* 
* Licensed under the Apache License, Version 2.0 (the "License"). You may
* not use this file except in compliance with the License. A copy of the
* License is located at
* 
* http://aws.amazon.com/apache2.0/
* 
* or in the "license" file accompanying this file. This file is
* distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied. See the License for the specific
* language governing permissions and limitations under the License.
*******************************************************************************/

using Amazon.Auth.AccessControlPolicy;
using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.EC2.Util;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using log4net;
using Rhyous.SimpleArgs;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Statement = Amazon.Auth.AccessControlPolicy.Statement;

namespace Rhyous.CS6210.Hw1.InstanceLauncher
{

    /// <summary>
    /// This sample shows how to launch an Amazon EC2 instance with a PowerShell script that is executed when the 
    /// instance becomes available and access Amazon S3.
    /// </summary>
    class Program
    {
        static ILog Log = LogManager.GetLogger(Assembly.GetExecutingAssembly().Location + ".log");


        public static void Main(string[] args)
        {
            LogConfigurator.Configure();
            Log.Debug("Program launched.");
            new ArgsManager<ArgsHandler>().Start(args);
        }

        internal static void Start()
        {
            var action = Args.Value("Action");
            var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
            MethodInfo mi = typeof(AwsActions).GetMethod(action, flags);
            switch (mi.GetParameters().Length)
            {
                case 0:
                    mi.Invoke(null, null);
                    break;
                case 1:
                    mi.Invoke(null, new[] { new AmazonEC2Client() });
                    break;
            }
        }

        public static void TerminateInstance(AmazonEC2Client ec2Client)
        {
            var bucketName = Args.Value("Bucket");
            var instanceId = Args.Value("InstanceId");
            var keyPair = new KeyPair();

            // Terminate instance
            ec2Client.TerminateInstances(new TerminateInstancesRequest
            {
                InstanceIds = new List<string>() { instanceId }
            });

            // Delete key pair created for sample.
            ec2Client.DeleteKeyPair(new DeleteKeyPairRequest { KeyName = keyPair.KeyName });
        }
    }
}
