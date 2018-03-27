using Amazon;
using Amazon.EC2;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using log4net;
using Rhyous.AmazonEc2InstanceManager;
using Rhyous.AmazonS3BucketManager;
using Rhyous.SimpleArgs;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw1.InstanceLauncher
{
    class Program
    {
        static ILog Log = LogManager.GetLogger(Assembly.GetExecutingAssembly().Location + ".log");

        public static void Main(string[] args)
        {
            LogConfigurator.Configure();
            Log.Debug("Program launched.");
            new ArgsManager<ArgsHandler>().Start(args);
        }

        internal static async Task Start()
        {
            var region = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["AWSRegion"]);
            var e2client = new AmazonEC2Client(Args.Value("AccessKey"), Args.Value("SecretKey"), region);
            var s3client = new AmazonS3Client(Args.Value("AccessKey"), Args.Value("SecretKey"), region);
            var transferUtility = new TransferUtility(Args.Value("AccessKey"), Args.Value("SecretKey"), region);

            // Sync files
            var bucket = Args.Value("bucket");

            // Create bucket
            var bucketExists = (await s3client.ListBucketsAsync()).Buckets.Any(b => b.BucketName == bucket);
            if (!bucketExists)
                await BucketManager.CreateBucketAsync(s3client, bucket);

            // Upload files to bucket
            var skipSections = Args.Value("Skip")?.Split(',');
            if (skipSections != null && skipSections.Any() && skipSections.Any(v=>v=="UploadFiles"))
                await BucketManager.UploadFilesAsync(transferUtility, bucket, Args.Value("LocalDirectory"));

            // Get launch script
            var scriptText = File.ReadAllText(Args.Value("LaunchScript"));
            var matches = Regex.Matches(scriptText, "\"{[^}]+}\"");
            foreach (Match match in matches)
            {
                scriptText = scriptText.Replace(match.Value.Trim(new[] { '"'}), Args.Value(match.Value.Trim(new[] { '{', '}', '"' })));
            }
            scriptText = $"<powershell>\n{scriptText}\n</powershell>";

            var scriptAsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(scriptText));

            // Start instance
            var keyPair = await InstanceManager.CreateInstance(e2client, Args.Value("ImageId"), Args.Value("KeyName"), Args.Value("InstanceName"), scriptAsBase64);
        }
    }
}
