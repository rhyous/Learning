using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.EC2.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.AmazonEc2InstanceManager
{
    public class InstanceManager
    {
        public static async Task<KeyPair> CreateKeyPair(AmazonEC2Client ec2Client, string keyName, string keyOutputDirectory = null)
        {
            var keyPair = (await ec2Client.CreateKeyPairAsync(new CreateKeyPairRequest { KeyName = keyName }))?.KeyPair;
            if (!string.IsNullOrWhiteSpace(keyOutputDirectory))
                await SaveKeyPairToDisc(keyName, keyOutputDirectory, keyPair);
            return keyPair;
        }

        public static async Task SaveKeyPairToDisc(string keyName, string keyOutputDirectory, KeyPair keyPair)
        {
            await Task.Run(() =>
            {
                var path = Path.Combine(keyOutputDirectory, $"{keyName}.pem");
                File.WriteAllText(path, keyPair.KeyMaterial);
                Console.WriteLine($"They key pair was saved to: {path}");
            });
        }

        public static async Task DeleteKeyPair(AmazonEC2Client ec2Client, string keyName)
        {
            await ec2Client.DeleteKeyPairAsync(new DeleteKeyPairRequest { KeyName = keyName });
        }

        public static async Task ImportKeyPair(AmazonEC2Client ec2Client, string keyName, string keyFile)
        {
            var publicKey = File.ReadAllText(keyFile).Trim().RemoveFirstLine().RemoveLastLine();
            string publicKeyAsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(publicKey));
            await ec2Client.ImportKeyPairAsync(new ImportKeyPairRequest(keyName, publicKeyAsBase64));
        }

        public static async Task<string> CreateInstance(AmazonEC2Client ec2Client, string imageId, string KeyName, string instanceName, string userData)
        {
            var image = await ImageUtilities.FindImageAsync(ec2Client, ImageUtilities.WINDOWS_2012_BASE);
            Console.WriteLine($"Search for image id using {imageId} and found {image.ImageId}");
            var runRequest = new RunInstancesRequest
            {
                ImageId = image.ImageId,
                MinCount = 1,
                MaxCount = 1,
                KeyName = KeyName,
                UserData = userData
            };
            var response = await ec2Client.RunInstancesAsync(runRequest);
            var instanceId = response.Reservation.Instances[0].InstanceId;
            if (!string.IsNullOrWhiteSpace(instanceName))
                await NameInstance(ec2Client, instanceId, instanceName);
            Console.WriteLine($"Instance Id: {instanceId}");
            return instanceId;
        }

        public static async Task DeleteInstance(AmazonEC2Client ec2Client, string instanceId)
        {
            var request = new TerminateInstancesRequest { InstanceIds = new List<string>() { instanceId } };
            await ec2Client.TerminateInstancesAsync(request);
        }

        public static async Task NameInstance(AmazonEC2Client ec2Client, string instanceId, string instanceName)
        {
            await ec2Client.CreateTagsAsync(new CreateTagsRequest
            {
                Resources = new List<string> { instanceId },
                Tags = new List<Tag> { new Tag { Key = "Name", Value = instanceName } }
            });
            Console.WriteLine($"Instance name: {instanceName}");
        }

        public static async Task<string> GetPassword(AmazonEC2Client ec2Client, string instanceId, string keyFile)
        {
            var key = File.ReadAllText(keyFile);
            var response = await ec2Client.GetPasswordDataAsync(new GetPasswordDataRequest { InstanceId = instanceId });
            var password = response.GetDecryptedPassword(key);
            Console.WriteLine($"Instance Password: { password}");
            return password;
        }

        public static async Task StopInstance(AmazonEC2Client ec2Client, string instanceId)
        {
            var request = new StopInstancesRequest { InstanceIds = new List<string> { instanceId } };
            var response = await ec2Client.StopInstancesAsync(request);
        }

        public static async Task StartInstance(AmazonEC2Client ec2Client, string instanceId)
        {
            var request = new StartInstancesRequest { InstanceIds = new List<string> { instanceId } };
            var response = await ec2Client.StartInstancesAsync(request);
        }

        public static async Task<string> GetFqdn(AmazonEC2Client ec2Client, string instanceId)
        {
            var request = new DescribeInstancesRequest { InstanceIds = new List<string> { instanceId } };
            var response = await ec2Client.DescribeInstancesAsync(request);
            var fqdn = response?.Reservations[0].Instances[0].PublicDnsName;
            Console.WriteLine(fqdn);
            return fqdn;
        }

        public static async Task OpenInboundPort(AmazonEC2Client ec2Client, int port, string protocol, string securityGroupName)
        {
            var ipPermission = new IpPermission
            {
                FromPort = port,
                ToPort = port,
                IpProtocol = protocol,
                Ipv4Ranges = new List<IpRange> { new IpRange { CidrIp = "0.0.0.0/0" } }
            };
            var ipPermissions = new List<IpPermission> { ipPermission };
            var request = new AuthorizeSecurityGroupIngressRequest
            {
                GroupName = securityGroupName,
                IpPermissions = ipPermissions
            };
            var response = await ec2Client.AuthorizeSecurityGroupIngressAsync(request);
        }

        public static async Task CloseInboundPort(AmazonEC2Client ec2Client, int port, string protocol, string securityGroupName)
        {
            var ipPermission = new IpPermission
            {
                FromPort = port,
                ToPort = port,
                IpProtocol = protocol,
                Ipv4Ranges = new List<IpRange> { new IpRange { CidrIp = "0.0.0.0/0" } }
            };
            var ipPermissions = new List<IpPermission> { ipPermission };
            var request = new RevokeSecurityGroupIngressRequest
            {
                GroupName = securityGroupName,
                IpPermissions = ipPermissions
            };
            var response = await ec2Client.RevokeSecurityGroupIngressAsync(request);
        }


        public static async Task OpenOutboundPort(AmazonEC2Client ec2Client, int port, string securityGroupName)
        {
            var describeRequest = new DescribeSecurityGroupsRequest { GroupNames = new List<string> { securityGroupName } };
            var describeResponse = await ec2Client.DescribeSecurityGroupsAsync(describeRequest);
            var securityGroupId = describeResponse.SecurityGroups[0].GroupId;
            var ipPermissions = new List<IpPermission> { new IpPermission { ToPort = port } };
            var request = new AuthorizeSecurityGroupEgressRequest { GroupId = securityGroupId, IpPermissions = ipPermissions };
            var response = await ec2Client.AuthorizeSecurityGroupEgressAsync(request);
        }

        public static async Task TerminateInstance(AmazonEC2Client ec2Client, string instanceId)
        {
            var request = new TerminateInstancesRequest { InstanceIds = new List<string>() { instanceId } };
            await ec2Client.TerminateInstancesAsync(request);
        }
    }
}
