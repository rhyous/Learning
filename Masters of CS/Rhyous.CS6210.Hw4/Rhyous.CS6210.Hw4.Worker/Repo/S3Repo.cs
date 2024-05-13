using Amazon.S3;
using System;

namespace Rhyous.CS6210.Hw4
{
    public class S3Repo : IRepo
    {
        private readonly AmazonS3Client _Client;
        private readonly string _Bucket;

        public S3Repo(AmazonS3Client client, string bucket)
        {
            _Client = client ?? throw new ArgumentNullException("client");
            _Bucket = bucket ?? throw new ArgumentNullException("bucket"); ;
        }

        public IMasterFileReader Reader => new S3MasterFileReader(_Client, _Bucket);
        public IMasterFileWriter Writer => new S3MasterFileWriter(_Client, _Bucket);
        public IUnitOfWorkFileReader UnitOfWorkReader => new S3UnitOfWorkFileReader(_Client, _Bucket);
        public IUnitOfWorkFileWriter UnitOfWorkWriter => new S3UnitOfWorkFileWriter(_Client, _Bucket);
    }
}
