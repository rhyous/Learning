using Amazon.S3;
using Rhyous.CS6210.Hw4.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public class S3DirectoryPreparerAsync : IDirectoryPreparerAsync
    {
        private readonly string _Bucket;
        private readonly AmazonS3Client _Client;

        public S3DirectoryPreparerAsync(AmazonS3Client client, string bucket)
        {
            _Bucket = bucket;
            _Client = client;
        }

        public async Task<bool> DirectoryExistsAsync(string dir)
        {
            return await BucketManager.FolderExists(_Client, _Bucket, dir);
        }

        public async Task<bool> FileExistsAsync(string file)
        {
            return await BucketManager.FileExists(_Client, _Bucket, file);
        }

        public async Task<bool> PrepareAync(string primaryDirectory, IEnumerable<string> subdirs)
        {
            await BucketManager.CreateBucketAsync(_Client, _Bucket);
            if (string.IsNullOrWhiteSpace(primaryDirectory))
                throw new ArgumentException("primaryDirectory", string.Format(Messages.ListNullOrEmpty, "primaryDirectory"));
            if (subdirs == null || !subdirs.Any())
                throw new ArgumentException("subdirs", string.Format(Messages.ListNullOrEmpty, "subdirs"));
            if (subdirs.Any(d => string.IsNullOrWhiteSpace(d)))
                throw new ArgumentException("subdirs", string.Format(Messages.StringListNullEmptyOrWhiteSpace, "sbudirs"));
            var created = await CreateDirectoryIfNotExists(primaryDirectory);
            if (!created)
                return false;
            foreach (var subdir in subdirs)
            {
                var dir = Path.Combine(primaryDirectory, subdir);
                created = await CreateDirectoryIfNotExists(dir);
                if (!created)
                    return false;
            }
            return true;
        }

        internal async Task<bool> CreateDirectoryIfNotExists(string dir)
        {
            dir = dir.Replace("\\", "/");
            var fileExists = await BucketManager.FolderExists(_Client, _Bucket, dir);
            if (!fileExists)
                await BucketManager.CreateBucketDirectoryAsync(_Client, _Bucket, dir);
            return await BucketManager.FolderExists(_Client, _Bucket, dir);
        }
    }
}