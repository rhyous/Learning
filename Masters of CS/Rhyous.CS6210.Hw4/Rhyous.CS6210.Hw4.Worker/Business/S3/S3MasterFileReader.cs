using Amazon.S3;
using Newtonsoft.Json;
using Rhyous.CS6210.Hw4.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public class S3MasterFileReader : IMasterFileReader
    {
        private readonly AmazonS3Client _Client;
        private readonly string _Bucket;

        public S3MasterFileReader(AmazonS3Client client, string bucket)
        {
            _Client = client;
            _Bucket = bucket;
        }

        public async Task<Master> ReadAsync(string masterDirectory)
        {
            var result = await BucketManager.FolderExistsAsync(_Client, _Bucket, masterDirectory);
            if (!result)
                return null;
            var masterFiles = await BucketManager.ListFilesAsync(_Client, _Bucket, masterDirectory, ".json");
            if (masterFiles == null || !masterFiles.Any())
                return null;
            var currentMasterFile = masterFiles.OrderByDescending(f=>f).First();
            var text = await BucketManager.ReadAllTextAsync(_Client, _Bucket, currentMasterFile);
            var master = JsonConvert.DeserializeObject<Master>(text);
            return master;
        }
    }
}