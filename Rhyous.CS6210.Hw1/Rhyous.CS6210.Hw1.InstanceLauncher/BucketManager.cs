using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Rhyous.StringLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.AmazonS3BucketManager
{
    public class BucketManager
    {
        public static async Task CreateBucketAsync(AmazonS3Client client, string bucket)
        {
            await client.PutBucketAsync(bucket);
            Console.WriteLine($"Created S3 bucket: {bucket}");
        }

        public static async Task CreateBucketDirectoryAsync(AmazonS3Client client, string bucket, string directory)
        {
            var dirRequest = new PutObjectRequest
            {
                BucketName = bucket,
                Key = directory + "/",
                InputStream = new MemoryStream(new byte[0])
            };
            await client.PutObjectAsync(dirRequest);
            Console.WriteLine($"Created S3 bucket folder: {bucket}/{directory}/");
        }
        public static async Task DeleteBucketDirectoryAsync(AmazonS3Client client, string bucket, string directory)
        {
            if (!directory.EndsWith("/"))
                directory = directory += "/";
            await DeleteFileAsync(client, bucket, directory);
        }

        public static async Task CreateTextFileAsync(AmazonS3Client client, string bucket, string filename, string text)
        {
            var dirRequest = new PutObjectRequest
            {
                BucketName = bucket,
                Key = filename,
                InputStream = text.ToStream()
            };
            await client.PutObjectAsync(dirRequest);
            Console.WriteLine($"Created text file in S3 bucket: {bucket}/{filename}");
        }

        public static async Task DeleteFileAsync(AmazonS3Client client, string bucket, string filename)
        {
            var dirRequest = new DeleteObjectRequest
            {
                BucketName = bucket,
                Key = filename
            };
            await client.DeleteObjectAsync(dirRequest);
            Console.WriteLine($"Deleted item from S3 bucket: {bucket}/{filename}");
        }

        public static async Task DeleteBucketAsync(AmazonS3Client client, string bucket)
        {
            await AmazonS3Util.DeleteS3BucketWithObjectsAsync(client, bucket);
            Console.WriteLine($"Deleted S3 bucket: {bucket}");
        }

        public static async Task ListFilesAsync(AmazonS3Client client, string bucket)
        {
            var listResponse = await client.ListObjectsV2Async(new ListObjectsV2Request { BucketName = bucket });
            if (listResponse.S3Objects.Count > 0)
            {
                Console.WriteLine($"Listing items in S3 bucket: {bucket}");
                listResponse.S3Objects.ForEach(o => Console.WriteLine(o.Key));
            }
        }

        public static async Task UploadFileAsync(TransferUtility transferUtility, string bucket, string file, string remoteDirectory = null)
        {
            var key = Path.GetFileName(file);
            if (!string.IsNullOrWhiteSpace(remoteDirectory))
            {
                remoteDirectory = remoteDirectory.EndsWith("/") ? remoteDirectory : remoteDirectory + "/";
                key = remoteDirectory + key;
            }
            await Task.Run(() => transferUtility.Upload(file, bucket, key));
        }

        public static async Task UploadFilesAsync(TransferUtility transferUtility, string bucket, string localDirectory)
        {
            var files = await FileUtils.GetFiles(localDirectory, true);
            var directoryName = Path.GetFileName(localDirectory); // This is not a typo. GetFileName is correctly used.            
            var tasks = files.Select(f => UploadFileAsync(transferUtility, bucket, f, Path.GetDirectoryName(f).Substring(f.IndexOf(directoryName)).Replace('\\', '/')));
            await Task.WhenAll(tasks);
        }
    }
}