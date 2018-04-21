using log4net;
using Newtonsoft.Json;
using Rhyous.CS6210.Hw4.Dictionary;
using Rhyous.CS6210.Hw4.Models;
using Rhyous.StringLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public class Worker : IWorker
    {
        private readonly IReplyServer _ReplyServer;
        private readonly IElectionRequester _ElectionRequester;
        private readonly IDirectoryPreparerAsync _DirectoryPreparer;
        private readonly string _PrimaryDirectory;
        private readonly IMasterFileReader _MasterFileReader;
        private readonly IMasterFileWriter _MasterFileWriter;
        private readonly IUnitOfWorkFileReader _UnitOfWorkFileReader;
        private readonly IUnitOfWorkFileWriter _UnitOfWorkFileWriter;
        private readonly IWorkProcessor _WorkProcessor;
        private readonly IWorkerLogger _Logger;

        public Worker(string name,
                      IpAddress ipAddress,
                      int port,
                      IReplyServer electionReplyServer,
                      string primaryDirectory,
                      IDirectoryPreparerAsync directoryPreparer,
                      IElectionRequester electionRequester,
                      IMasterFileReader masterFileReader,
                      IMasterFileWriter masterFileWriter,
                      IUnitOfWorkFileReader unitOfWorkFileReader,
                      IUnitOfWorkFileWriter unitOfWorkFileWriter,
                      IWorkerLogger logger = null,
                      IWorkProcessor workProcessor = null
                     )
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name", string.Format(Messages.StringNullEmptyOrWhiteSpace, "name"));
            Connection = new WorkerConnection
            {
                Name = name,
                IpAddress = ipAddress ?? throw new ArgumentNullException("ipAddress", string.Format(Messages.ObjectNull, "ipAddress")),
                Port = (port > 1024 && port < 65536) ? port : throw new ArgumentNullException("port", Messages.PortNoInRange)
            };
            _DirectoryPreparer = directoryPreparer ?? throw new ArgumentNullException("directoryPreparer", string.Format(Messages.ObjectNull, "directoryPreparer")); ;
            _PrimaryDirectory = primaryDirectory;
            _ReplyServer = electionReplyServer ?? throw new ArgumentNullException("electionReplyServer", string.Format(Messages.ObjectNull, "electionReplyServer"));
            _ElectionRequester = electionRequester ?? throw new ArgumentNullException("electionRequester", string.Format(Messages.ObjectNull, "electionRequester"));
            _MasterFileReader = masterFileReader ?? throw new ArgumentNullException("masterFileReader", string.Format(Messages.ObjectNull, "masterFileReader"));
            _MasterFileWriter = masterFileWriter ?? throw new ArgumentNullException("masterFileWriter", string.Format(Messages.ObjectNull, "masterFileWriter"));
            _UnitOfWorkFileReader = unitOfWorkFileReader ?? throw new ArgumentNullException("unitOfWorkFileReader", string.Format(Messages.ObjectNull, "unitOfWorkFileReader"));
            _UnitOfWorkFileWriter = unitOfWorkFileWriter ?? throw new ArgumentNullException("unitOfWorkFileWriter", string.Format(Messages.ObjectNull, "unitOfWorkFileWriter"));

            _Logger = logger;
            if (_Logger == null)
            {
                _Logger = new MultiLogger(name, new ConsoleLogger(),
                    new Log4NetLogger(LogManager.GetLogger(Assembly.GetExecutingAssembly(), $"{Connection.Name}.log")));
            }

            _WorkProcessor = workProcessor ?? new WorkProcessor(_Logger);
            _WorkProcessor.OnComplete += OnWorkComplete;
        }

        private void OnWorkComplete(object sender, WorkCompleteEventArgs e)
        {
            _Logger?.Debug("Work completed: " + e.UnitOfWork.Id);
            ReportWorkCompletionAsync(e.UnitOfWork);
            RequestWorkAsync();
        }

        public WorkerConnection Connection { get; set; }
        public bool IsMaster { get { return Connection == Master?.Connection; } }
        public WorkerState State { get; set; }

        public Master Master { get; set; }

        public async Task<string> PingAsync(WorkerConnection connection)
        {
            _Logger?.Debug("Sending ping: " + connection.ToString());
            var client = new Pinger();
            var packet = new Packet<string> { Payload = "ping", Type = "ping" };
            var pong = await client.SendAsync<string, string>(Connection.Name, packet, connection.IpAddress, connection.Port);
            _Logger?.Debug("Response: " + (string.IsNullOrWhiteSpace(pong) ? "none" : pong));
            return pong;
        }

        public async Task<ElectionResponseType> ElectionRequestAsync(ElectionRequestType type)
        {
            _Logger?.Debug("Requesting election: " + type.ToString());
            var tasks = new List<Task<ElectionResponse>>();
            foreach (var worker in Master.Workers)
            {
                Task<ElectionResponse> task = null;
                if (type == ElectionRequestType.Election)
                    task = _ElectionRequester.ElectMeAsync(worker);
                if (type == ElectionRequestType.Resignation)
                    task = _ElectionRequester.ResignAsync(worker);
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
            return tasks.Max(t => t.Result.ResponseType);
        }

        public async Task StopAsync()
        {
            _Logger?.Debug("Stop requested");
            State = WorkerState.Stopping;
            if (IsMaster)
                await ElectionRequestAsync(ElectionRequestType.Resignation);
            await _ReplyServer.StopAsync();
            State = WorkerState.Stopped;
            _Logger?.Debug("Stopped.");
        }

        public async Task StartAsync()
        {
            _Logger?.Debug("Start requested");
            State = WorkerState.Starting;
            ListenerTask = StartListenerAsync();
            await UpdateMasterAsync();
            State = WorkerState.Started;
            _Logger?.Debug("Started");
        }
        internal Task ListenerTask;

        public async Task UpdateMasterAsync()
        {
            _Logger?.Debug("Updating master.");
            Master = await GetMasterAsync();
            await UpdateElectionFileAsync();
            _Logger?.Debug("Master updated.");
            if (IsMaster)
            {
                await QueueWorkAsync();
            }
            else
            {
                await RegisterAsync();
                await RequestWorkAsync();
            }
        }

        public async Task<Master> GetMasterAsync()
        {
            var masterDirectory = Path.Combine(_PrimaryDirectory, Folders.Master);
            var currentMaster = await _MasterFileReader.ReadAsync(masterDirectory);
            var myselfAsMaster = new Master { Connection = Connection, Workers = currentMaster?.Workers };
            if (currentMaster == null || myselfAsMaster == currentMaster)
            {
                myselfAsMaster = currentMaster ?? new Master();
                myselfAsMaster.Connection = Connection;
                myselfAsMaster.Workers = currentMaster?.Workers;
                _Logger?.Debug("I am the master.");
                return myselfAsMaster;
            }
            string pong = await PingAsync(currentMaster.Connection);
            if (myselfAsMaster > currentMaster || string.IsNullOrWhiteSpace(pong))
            {
                await TakeOverAsync(currentMaster, myselfAsMaster, pong);
                _Logger?.Debug("I am the master.");
                return myselfAsMaster;
            }
            _Logger?.Debug("Current master is: " + currentMaster.ToString());
            return currentMaster;
        }

        public async Task StartListenerAsync()
        {
            await _ReplyServer.StartAsync(Connection.IpAddress, Connection.Port,
                    (string type, object request) =>
                    {
                        if (RequestHandler.TryGetValue(type, out Func<IWorker, object, Task<object>> method))
                        {
                            try { return method.Invoke(this, request)?.Result; }
                            catch (Exception e) { _Logger?.Debug(e.ToString()); }
                        }
                        return null;
                    }
                );
        }

        public RequestHandler RequestHandler
        {
            get { return _RequestHandler ?? (_RequestHandler = new RequestHandler()); }
            set { _RequestHandler = value; }
        }
        private RequestHandler _RequestHandler;

        public async Task StopListenerAsync()
        {
            await _ReplyServer.StopAsync();
        }

        public async Task<Master> TakeOverAsync(Master currentMaster, Master myselfAsMaster, string pong)
        {
            _Logger?.Debug("Taking over as master.");
            if (currentMaster != null)
            {
                var list = new List<WorkerConnection> { currentMaster.Connection };
                if (currentMaster.Workers.Any())
                    list.AddRange(currentMaster.Workers);
                myselfAsMaster.Workers.AddRange(list.Distinct());
                myselfAsMaster.Workers = myselfAsMaster.Workers.Distinct().ToList();
            }
            var masterDirectory = Path.Combine(_PrimaryDirectory, Folders.Master);
            await _MasterFileWriter.WriteAsync(masterDirectory, myselfAsMaster);
            if (string.IsNullOrWhiteSpace(pong))
            {
                _Logger?.Debug($"Moving master file to the {Folders.Sleep} directory.");
                var currentMasterFile = $"{currentMaster.ToString()}.json";
                var src = Path.Combine(masterDirectory, currentMasterFile);
                var dst = Path.Combine(_PrimaryDirectory, Folders.Sleep, currentMasterFile);
                await _UnitOfWorkFileWriter.MoveAsync(src, dst);
            }
            _Logger?.Debug("Taking over as master.");
            ElectInN(3000);
            return myselfAsMaster;
        }

        public async Task ElectInN(int milliseconds)
        {
            await Task.Delay(milliseconds);
            await UpdateMasterAsync();
        }

        public async Task UpdateElectionFileAsync()
        {
            _Logger?.Debug("Updating my election file.");
            var masterDirectory = Path.Combine(_PrimaryDirectory, Folders.Master);
            await _MasterFileWriter.WriteAsync(masterDirectory, IsMaster ? Master : new Master { Connection = Connection, Workers = Master?.Workers });
            _Logger?.Debug("Election file upated.");
        }

        #region IWorkHandler
        public async Task<bool> RequestWorkAsync()
        {
            _Logger?.Debug("Requesting work.");
            var client = new WorkRequestor(this);
            var packet = new Packet<UnitOfWork> { Type = "WorkRequest" };
            var unitOfWork = await client.SendAsync<UnitOfWork, UnitOfWork>(Connection.Name, packet, Master.Connection.IpAddress, Master.Connection.Port);
            _Logger?.Debug("Received task: " + unitOfWork?.Id);
            if (unitOfWork == null)
            {
                var pong = await PingAsync(Master.Connection);
                if (string.IsNullOrWhiteSpace(pong))
                    TakeOverAsync(Master, new Master { Connection = Connection, Workers = Master.Workers }, pong);
                else
                {
                    if (NullWorkResponseCounter < 3)
                    {
                        await RequestWorkAsync();
                    }
                    StopAsync(); // Master is up, no work left
                    return false;
                }
            }
            else
                DoWorkAsync(unitOfWork);
            return unitOfWork != null;
        } private int NullWorkResponseCounter = 0;

        public async Task DoWorkAsync(UnitOfWork unitOfWork)
        {
            await _WorkProcessor.ProcessWorkAsync(_UnitOfWorkFileReader, unitOfWork);
        }

        public async Task<bool> ReportWorkProgressAsync(UnitOfWork unitOfWork)
        {
            return true;
        }

        public async Task<bool> ReportWorkCompletionAsync(UnitOfWork unitOfWork)
        {
            var client = new WorkCompletionReporter();
            var packet = new Packet<UnitOfWork> { Payload = unitOfWork, Type = "WorkCompletion" };
            return await client.SendAsync<UnitOfWork, bool>(Connection.Name, packet, Master.Connection.IpAddress, Master.Connection.Port);
        }

        public async Task<bool> RegisterAsync()
        {
            var client = new Registrar();
            var packet = new Packet<WorkerConnection> { Payload = Connection, Type = "Register" };
            return await client.SendAsync<WorkerConnection, bool>(Connection.Name, packet, Master.Connection.IpAddress, Master.Connection.Port);
        }

        public async Task<UnitOfWork> QueueNextUnitOfWorkAsync()
        {
            var workQueueDir = Path.Combine(_PrimaryDirectory, Folders.WorkQueue);
            var nextFile = await _UnitOfWorkFileReader.GetNextFileAsync(workQueueDir);
            if (nextFile == null)
                return null;
            var workInProgressDir = Path.Combine(_PrimaryDirectory, Folders.WorkInProgress);
            var file = Path.GetFileName(nextFile);
            var dest = Path.Combine(workInProgressDir, file);
            await _UnitOfWorkFileWriter.MoveAsync(nextFile, dest);
            var unitOfWork = await _UnitOfWorkFileReader.ReadAsync(dest);
            var associatedFile = Path.GetFileName(unitOfWork.AssociateFile);
            var assocaitedFileDst = Path.Combine(_PrimaryDirectory, Folders.WorkInProgress, associatedFile);
            await _UnitOfWorkFileWriter.MoveAsync(unitOfWork.AssociateFile, assocaitedFileDst);
            unitOfWork.Assigned = DateTime.Now;
            unitOfWork.AssociateFile = assocaitedFileDst;
            await _UnitOfWorkFileWriter.WriteAsync(dest, unitOfWork);
            return unitOfWork;
        }

        public async Task QueueWorkAsync()
        {
            var queueDir = Path.Combine(_PrimaryDirectory, Folders.WorkQueue);
            var dirExists = await _DirectoryPreparer.DirectoryExistsAsync(queueDir);
            if (!dirExists)
                return;
            var files = await _UnitOfWorkFileReader.GetFilesAsync(queueDir);
            var stringFiles = files.Where(f => f.EndsWith(".txt"));
            var tasks = new List<Task>();
            var batches = stringFiles.GetBatch(10);
            int i = 0;
            foreach (var batch in batches)
            {
                foreach (var path in batch)
                {
                    _Logger?.Debug($"Batch {i}: {path}");
                    var filename = Path.GetFileNameWithoutExtension(path);
                    var id = filename.Substring(filename.Length - 5, 5).To<int>();
                    var unitOfWork = new UnitOfWork
                    {
                        Id = id,
                        AssociateFile = path,
                        Status = WorkStatus.Queued
                    };
                    var fileName = $"{unitOfWork.Id}.json";
                    var fileExists = files.Any(f=>f.EndsWith(fileName));
                    if (fileExists)
                    {
                        _Logger?.Debug($"Batch {i}: Unit of work already exists for: {path}");
                        continue;
                    }
                    await _UnitOfWorkFileWriter.WriteAsync(queueDir, unitOfWork);
                    _Logger?.Debug($"Batch {i}: Created Unit of work for: {path}");
                }
                i++;
            }
        }

        public async Task<bool> MarkWorkCompletedAsync(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new Exception("Unit of work cannot be null.");
            var file = $"{unitOfWork.Id}.json";
            var src = Path.Combine(_PrimaryDirectory, Folders.WorkInProgress, file);
            var dst = Path.Combine(_PrimaryDirectory, Folders.WorkCompleted, file);
            await _UnitOfWorkFileWriter.MoveAsync(src, dst);
            var associatedFile = Path.GetFileName(unitOfWork.AssociateFile);
            var assocaitedFileDst = Path.Combine(_PrimaryDirectory, Folders.WorkCompleted, associatedFile);
            await _UnitOfWorkFileWriter.MoveAsync(unitOfWork.AssociateFile, assocaitedFileDst);
            unitOfWork.Progress = 100;
            unitOfWork.Status = WorkStatus.Completed;
            await _UnitOfWorkFileWriter.WriteAsync(dst, unitOfWork);
            return true;
        }

        #endregion
    }
}