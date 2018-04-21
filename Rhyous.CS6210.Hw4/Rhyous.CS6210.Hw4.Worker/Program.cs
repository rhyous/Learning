using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using Rhyous.CS6210.Hw4.Models;
using Rhyous.SimpleArgs;
using Rhyous.StringLibrary;
using System;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            new ArgsManager<ArgsHandler>().Start(args);
            var appender = new FileAppender
            {
                Layout = new SimpleLayout(),
                File = Assembly.GetExecutingAssembly().Location + ".log",
                Encoding = Encoding.UTF8,
                AppendToFile = true,
                LockingModel = new FileAppender.MinimalLock()
            };
            appender.ActivateOptions();
            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            BasicConfigurator.Configure(logRepo, appender);
            var primaryDirectory = Args.Value(ArgsHandler.PrimaryDirectory);
            var name = Args.Value(ArgsHandler.Name);
            var ip = Args.Value(ArgsHandler.IpAddress);
            var port = Args.Value(ArgsHandler.Port).To<int>();
            if (string.IsNullOrWhiteSpace(name))
                name = $"{ip}:{port}";

            var repoType = Args.Value(ArgsHandler.Repo);
            IDirectoryPreparerAsync directoryPreparer = null;
            IRepo repo = null;
            if (repoType == "Local")
            {
                directoryPreparer = new LocalDirectoryPreparerAsync();
                repo = new LocalRepo();
            }
            if (repoType == "S3")
            {
                var region = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["AWSRegion"]);
                var client = new AmazonS3Client(Args.Value("AccessKey"), Args.Value("SecretKey"), region);
                var bucket = Args.Value(ArgsHandler.Bucket);
                repo = new S3Repo(client, bucket);
                directoryPreparer = new S3DirectoryPreparerAsync(client, bucket);
            }

            var isPrepared = await directoryPreparer.PrepareAync(primaryDirectory, Folders.SubDirs);
            if (!isPrepared)
                throw new Exception(Messages.FolderStructureNotPrepared);


            IWorker worker = new Worker(name, ip, port, new ReplyServer(), primaryDirectory,
                directoryPreparer,
                new ElectionRequester(new RequestClient()), 
                repo.Reader, 
                repo.Writer, 
                repo.UnitOfWorkReader, 
                repo.UnitOfWorkWriter);
            await worker.StartAsync();
            var task = (worker as Worker)?.ListenerTask;
            if (task != null)
            await Task.WhenAll(task);
        }
    }
}
