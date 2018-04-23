using Amazon.S3;
using Newtonsoft.Json;
using Rhyous.CS6210.Hw4.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public class S3UnitOfWorkFileWriter : IUnitOfWorkFileWriter
    {
        private readonly string _Bucket;
        private readonly AmazonS3Client _Client;
        public S3UnitOfWorkFileWriter(AmazonS3Client client, string bucket)
        {
            _Client = client;
            _Bucket = bucket;
        }
        public async Task WriteAsync(string directory, UnitOfWork unitOfWork)
        {
            var folderExists = await BucketManager.FolderExistsAsync(_Client, _Bucket, directory);
            if (!folderExists)
                return;
            var filename = $"{unitOfWork.Id}.json".PadLeft(10, '0');
            var fileFullPath = Path.Combine(directory, filename);
            fileFullPath = fileFullPath.Replace("\\", "/");
            var json = JsonConvert.SerializeObject(unitOfWork);
            await BucketManager.CreateTextFileAsync(_Client, _Bucket, fileFullPath, json);
        }

        public async Task MoveAsync(string source, string destination)
        {
            source = source.Replace("\\", "/");
            destination = destination.Replace("\\", "/");
            await BucketManager.MoveObjectAsync(_Client, _Bucket, _Bucket, source, destination);
        }
    }
}
