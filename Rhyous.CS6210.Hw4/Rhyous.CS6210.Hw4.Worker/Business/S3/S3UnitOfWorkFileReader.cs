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
    public class S3UnitOfWorkFileReader : IUnitOfWorkFileReader
    {
        private readonly string _Bucket;
        private readonly AmazonS3Client _Client;
        public S3UnitOfWorkFileReader(AmazonS3Client client, string bucket)
        {
            _Client = client;
            _Bucket = bucket;
        }
        public async Task<List<string>> GetFilesAsync(string queueDir, string filter = null)
        {
            return await BucketManager.ListFilesAsync(_Client, _Bucket, queueDir, filter);
        }

        public async Task<string> GetNextFileAsync(string unitOfWorkDirectory)
        {
            unitOfWorkDirectory = unitOfWorkDirectory.Replace("\\","/");
            var folderExists = await BucketManager.FolderExists(_Client, _Bucket, unitOfWorkDirectory);
            if (!folderExists)
                return null;
            var unitOfWorkFiles = await BucketManager.ListFilesAsync(_Client, _Bucket, unitOfWorkDirectory, "*.json");
            return unitOfWorkFiles?.First();
        }

        public async Task<string> ReadAllTextAsync(string pathToFile)
        {
            pathToFile = pathToFile.Replace("\\", "/");
            var fileExists = await BucketManager.FileExists(_Client, _Bucket, pathToFile);
            if (!fileExists)
                return null;
            var text = await BucketManager.ReadAllTextAsync(_Client, _Bucket, pathToFile);
            return text;
        }

        public async Task<UnitOfWork> ReadAsync(string filepath)
        { 
            filepath = filepath.Replace("\\", "/");
            var json = await ReadAllTextAsync(filepath);
            return JsonConvert.DeserializeObject<UnitOfWork>(json);
        }        
    }
}
