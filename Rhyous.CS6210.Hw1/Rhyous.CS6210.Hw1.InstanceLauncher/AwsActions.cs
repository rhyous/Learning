using Amazon.Auth.AccessControlPolicy;
using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.EC2.Util;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using log4net;
using Rhyous.SimpleArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Statement = Amazon.Auth.AccessControlPolicy.Statement;

namespace Rhyous.CS6210.Hw1.InstanceLauncher
{
    public class AwsActions
    {
        static ILog Log = LogManager.GetLogger(Assembly.GetExecutingAssembly().Location + ".log");
        public static void Default()
        {
            Log.Debug($"I don't do anything by default.");
        }

        #region Buckets
        public static void UploadFile()
        {
            var bucketName = Args.Value("Bucket");
            var file = Args.Value("File");
            TransferUtility utility = new TransferUtility();
            utility.Upload(file, bucketName);
            ListFiles();
        }

        public static void ListFiles()
        {
            var bucketName = Args.Value("Bucket");
            var client = new AmazonS3Client();
            var listResponse = client.ListObjects(new ListObjectsRequest { BucketName = bucketName });
            if (listResponse.S3Objects.Count > 0)
            {
                Log.Debug($"Listing items in S3 bucket: {bucketName}");
                foreach (var obj in listResponse.S3Objects)
                {
                    Console.WriteLine(obj.Key);
                }
            }
        }

        public static void CreateBucket()
        {
            var bucketName = Args.Value("Bucket");
            var client = new AmazonS3Client();
            client.PutBucket(bucketName);
            Log.Debug($"Created S3 bucket: {bucketName}");
        }

        public static void DeleteBucket()
        {
            var bucketName = Args.Value("Bucket");
            var client = new AmazonS3Client();
            client.PutBucket(bucketName);
            AmazonS3Util.DeleteS3BucketWithObjects(client, bucketName);
            Log.Debug($"Deleted S3 bucket: {bucketName}");
        }
        #endregion

        #region KeyPair
        public static KeyPair CreateKeyPair(AmazonEC2Client ec2Client)
        {
            var keyName = Args.Value("KeyName");
            var keyPair = ec2Client.CreateKeyPair(new CreateKeyPairRequest { KeyName = keyName }).KeyPair;
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{keyName}.pem");
            File.WriteAllText(path, keyPair.KeyMaterial);
            Log.Debug($"They key pair was save to your desktop: {path}");
            return keyPair;
        }

        public static void DeleteKeyPair(AmazonEC2Client ec2Client)
        {
            var keyName = Args.Value("KeyName");
            ec2Client.DeleteKeyPair(new DeleteKeyPairRequest { KeyName = keyName });
        }
        #endregion

        #region Role
        public static void CreateRole()
        {
            var roleName = Args.Value("Role");
            var client = new AmazonIdentityManagementServiceClient();
            client.CreateRole(new CreateRoleRequest
            {
                RoleName = roleName,
                AssumeRolePolicyDocument = @"{""Statement"":[{""Principal"":{""Service"":[""ec2.amazonaws.com""]},""Effect"":""Allow"",""Action"":[""sts:AssumeRole""]}]}"
            });
        }

        public static void CreateRolePolicy()
        {
            var roleName = Args.Value("Role");
            var client = new AmazonIdentityManagementServiceClient();
            var statement = new Statement(Statement.StatementEffect.Allow);
            statement.Actions.Add(S3ActionIdentifiers.AllS3Actions);
            statement.Resources.Add(new Resource("*"));
            var policy = new Policy();
            policy.Statements.Add(statement);
            client.PutRolePolicy(new PutRolePolicyRequest
            {
                RoleName = roleName,
                PolicyName = "S3Access",
                PolicyDocument = policy.ToJson()
            });
        }

        public static void DeleteRolePolicy()
        {
            var roleName = Args.Value("Role");
            var client = new AmazonIdentityManagementServiceClient();
            client.DeleteRolePolicy(new DeleteRolePolicyRequest
            {
                RoleName = roleName,
                PolicyName = "S3Access"
            });
        }

        public static void DeleteRole()
        {
            var roleName = Args.Value("Role");
            var client = new AmazonIdentityManagementServiceClient();
            client.DeleteRole(new DeleteRoleRequest
            {
                RoleName = roleName
            });
        }
        #endregion

        #region Instance

        public static void RunInstance(AmazonEC2Client ec2Client)
        {
            var instanceProfileArn = Args.Value("InstanceProfile");
            Log.Debug($"Created Instance Profile: {instanceProfileArn}");
            Thread.Sleep(15000);
            var keyPair = CreateKeyPair(ec2Client);
            var imageKey = Args.Value("Image");
            Log.Debug($"Finding Image by key: {imageKey}");
            var imageId = ImageUtilities.FindImage(ec2Client, imageKey).ImageId;
            Log.Debug($"Found Image ID: {imageId}");
            var runRequest = new RunInstancesRequest
            {
                ImageId = imageId,
                MinCount = 1,
                MaxCount = 1,
                KeyName = keyPair.KeyName,
                IamInstanceProfile = new IamInstanceProfileSpecification { Arn = instanceProfileArn },

                // Add the region for the S3 bucket and the name of the bucket to create
                //UserData = EncodeToBase64(string.Format(USER_DATA_SCRIPT, RegionEndpoint.USWest2.SystemName, bucketName))
            };
            var instanceId = ec2Client.RunInstances(runRequest).Reservation.Instances[0].InstanceId;
            Log.Debug($"Launch Instance {instanceId}");
        }

        public static void CreateInstance(AmazonEC2Client ec2Client)
        {
            var instanceId = Args.Value("InstanceId");
            var instanceProfileArn = CreateInstanceProfile();           

            // Create the name tag
            ec2Client.CreateTags(new CreateTagsRequest
            {
                Resources = new List<string> { instanceId },
                Tags = new List<Amazon.EC2.Model.Tag> { new Amazon.EC2.Model.Tag { Key = "Name", Value = "Processor" } }
            });
            Log.Debug("Adding Name Tag to instance");


        }

        public static void GetInstancePassword(AmazonEC2Client ec2Client)
        {
            var instanceId = Args.Value("InstanceId");
            var keyPairMaterial = File.ReadAllText(Args.Value("KeyPairFile"));
            var passwordResponse = ec2Client.GetPasswordData(new GetPasswordDataRequest
            {
                InstanceId = instanceId
            });
            // Make sure we actually got a password
            if (passwordResponse.PasswordData != null)
            {
                var password = passwordResponse.GetDecryptedPassword(keyPairMaterial);
                Log.Debug($"The Windows Administrator password is: {password}");
            }
        }
        #endregion

        #region Profile
        /// <summary>
        /// Create the instance profile that will give permission for the EC2 instance to make request to Amazon S3.
        /// </summary>
        /// <returns></returns>
        static string CreateInstanceProfile()
        {
            var client = new AmazonIdentityManagementServiceClient();
            var roleName = Args.Value("Role");
            CreateRole();
            CreateRolePolicy();
            var response = client.CreateInstanceProfile(new CreateInstanceProfileRequest
            {
                InstanceProfileName = roleName
            });

            return response.InstanceProfile.Arn;
        }

        public void AssignRoleToInstance()
        {
            var roleName = Args.Value("Role");
            var client = new AmazonIdentityManagementServiceClient();
            client.AddRoleToInstanceProfile(new AddRoleToInstanceProfileRequest
            {
                InstanceProfileName = roleName,
                RoleName = roleName
            });
        }

        public static void RemoveRoleFromInstance()
        {
            var roleName = Args.Value("Role");
            var client = new AmazonIdentityManagementServiceClient();
            client.RemoveRoleFromInstanceProfile(new RemoveRoleFromInstanceProfileRequest
            {
                InstanceProfileName = roleName,
                RoleName = roleName
            });

        }

        /// <summary>
        /// Delete the instance profile created for the sample.
        /// </summary>
        public static void DeleteInstance()
        {
            var roleName = Args.Value("Role");
            var client = new AmazonIdentityManagementServiceClient();
            DeleteRolePolicy();
            RemoveRoleFromInstance();
            DeleteRole();
            client.DeleteInstanceProfile(new DeleteInstanceProfileRequest
            {
                InstanceProfileName = roleName
            });
        }
        #endregion
    }
}
