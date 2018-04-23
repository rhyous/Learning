using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Rhyous.StringLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public class BucketManager
    {
        public static async Task CreateBucketAsync(AmazonS3Client client, string bucket)
        {
            var buckets = await client.ListBucketsAsync();
            if (buckets.Buckets.Any(b => b.BucketName == bucket))
                return;
            await client.PutBucketAsync(bucket);
            Console.WriteLine($"Created S3 bucket: {bucket}");
        }

        public static async Task CreateBucketDirectoryAsync(AmazonS3Client client, string bucket, string directory)
        {
            directory = directory.Replace("\\", "/");
            var dirRequest = new PutObjectRequest
            {
                BucketName = bucket,
                Key = directory.EndsWith("/") ? directory : directory + "/",
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

        public static async Task<List<string>> ListFilesAsync(AmazonS3Client client, string bucket, string dir, string pattern = null)
        {
            dir = dir.Replace("\\", "/");
            dir = dir.EndsWith("/") ? dir : dir + "/";
            ListObjectsV2Response listResponse;
            var files = new List<string>();
            var request = new ListObjectsV2Request { BucketName = bucket, Prefix = dir };
            do
            {
                listResponse = await client.ListObjectsV2Async(request);
                if (listResponse.S3Objects.Count > 0)
                {
                    request.StartAfter = listResponse.S3Objects[listResponse.S3Objects.Count - 1].Key;
                    Console.WriteLine($"Listing items in S3 bucket: {bucket}");
                    if (string.IsNullOrWhiteSpace(pattern))
                    { files.AddRange(listResponse.S3Objects.Select(o => o.Key)); }
                    else
                    {
                        pattern = pattern.Replace("*", "");
                        files.AddRange(listResponse.S3Objects.Where(o => o.Key.Contains(pattern))
                                                             .Select(o => o.Key));
                    }
                }
            } while (listResponse.IsTruncated);
            return files;
        }

        public static async Task<bool> FolderExistsAsync(AmazonS3Client client, string bucket, string path)
        {
            path = path.Replace("\\", "/");
            path = path.EndsWith("/") ? path : path + "/";
            return await FileExistsAsync(client, bucket, path);
        }


        public static async Task<bool> FileExistsAsync(AmazonS3Client client, string bucket, string path)
        {
            path = path.Replace("\\", "/");
            GetObjectResponse obj = null;
            try { obj = await client.GetObjectAsync(new GetObjectRequest { BucketName = bucket, Key = path }); }
            catch (Exception e) { }
            return obj != null;
        }

        public static async Task<bool> MoveObjectAsync(AmazonS3Client client, string srcBucket, string dstBucket, string src, string dst)
        {
            src = src.Replace("\\", "/");
            dst = dst.Replace("\\", "/");

            if (!await FileExistsAsync(client, srcBucket, src))
                return false;
            var request = new CopyObjectRequest
            {
                SourceBucket = srcBucket,
                DestinationBucket = dstBucket,
                SourceKey = src,
                DestinationKey = dst
            };
            var response = await client.CopyObjectAsync(request);
            await DeleteFileAsync(client, srcBucket, src);
            return true;
        }

        public static async Task<string> ReadAllTextAsync(AmazonS3Client client, string bucket, string path)
        {
            path = path.Replace("\\", "/");
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucket,
                Key = path
            };

            using (GetObjectResponse response = await client.GetObjectAsync(request))
            using (Stream responseStream = response.ResponseStream)
            using (StreamReader reader = new StreamReader(responseStream))
            {
                return reader.ReadToEnd();
            }
        }

        public static async Task UploadFileAsync(TransferUtility transferUtility, string bucket, string file, string remoteDirectory = null)
        {
            file = file.Replace("\\", "/");
            remoteDirectory = remoteDirectory.Replace("\\", "/");
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
            int i = 0;
            int batchSize = 5;
            var batches = files.GetBatch(batchSize);
            foreach (var batch in batches)
            {
                var tasks = batch.Select(f => UploadFileAsync(transferUtility, bucket, f, Path.GetDirectoryName(f).Substring(f.IndexOf(directoryName)).Replace('\\', '/')));
                await Task.WhenAll(tasks);
                Console.WriteLine($"Uploaded batch {i}: {i * batchSize} of {files.Count}");
                i++;
            }
        }
    }
}