using Amazon.S3;
using Newtonsoft.Json;
using Rhyous.CS6210.Hw4.Models;
using System.IO;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    class S3MasterFileWriter : IMasterFileWriter
    {
        private readonly string _Bucket;
        private readonly AmazonS3Client _Client;
        public S3MasterFileWriter(AmazonS3Client client, string bucket)
        {
            _Client = client;
            _Bucket = bucket;
        }

        public async Task WriteAsync(string masterDirectory, Master master)
        {
            var folderExists = await BucketManager.FolderExistsAsync(_Client, _Bucket, masterDirectory);
            if (!folderExists)
                return;
            var filename = $"{master.ToString()}.json";
            var objKey = Path.Combine(masterDirectory, filename);
            objKey = objKey.Replace("\\", "/");
            var json = JsonConvert.SerializeObject(master);
            await BucketManager.CreateTextFileAsync(_Client, _Bucket, objKey, json);
        }
    }
}