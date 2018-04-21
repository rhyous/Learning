using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.CS6210.Hw4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4.Automation
{
    [TestClass]
    public class AutomatedTest
    {

        #region Election Ordered
        [TestMethod]
        public void Automated_Election2_Ordered_Test()
        {
            // Arrange
            var primaryDir = "root";
            var masterDir = "root\\Master";

            var mockReader = new Mock<IMasterFileReader>();
            var currentMaster = new Master
            {
                Connection = new WorkerConnection("127.0.0.1", 2600)
            };
            currentMaster.Workers.Add(new WorkerConnection("127.0.0.1", 2500));
            mockReader.Setup(m => m.ReadAsync(It.Is<string>(e => e == masterDir)))
                      .ReturnsAsync(() => { return currentMaster; });

            var mockWriter = new Mock<IMasterFileWriter>();
            mockWriter.Setup(m => m.WriteAsync(It.Is<string>(e => e == masterDir), It.IsAny<Master>()))
                      .Callback((string mdir, Master master) =>
                      {
                          if (master > currentMaster)
                              currentMaster = master;
                      })
                      .Returns(Task.CompletedTask);

            var mockUnitOfWorkReader = new Mock<IUnitOfWorkFileReader>();
            var mockUnitOfWorkWriter = new Mock<IUnitOfWorkFileWriter>();

            var mockDirectoryPrepareAsync = new Mock<IDirectoryPreparerAsync>();

            var worker1 = new Worker("Worker1",
                                     "127.0.0.1",
                                     2700,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object
                                     );

            var worker2 = new Worker("Worker2",
                                     "127.0.0.1",
                                     2800,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            // Act
            var task1 = worker1.StartAsync();
            var task2 = worker2.StartAsync();
            Task.WaitAll(task1, task2);

            // Assert
            Assert.AreEqual(currentMaster, worker2.Master);
        }

        [TestMethod]
        public void Automated_Election3_Ordered_Test()
        {
            // Arrange
            var primaryDir = "root";
            var masterDir = "root\\Master";

            var mockReader = new Mock<IMasterFileReader>();
            var currentMaster = new Master { Connection = new WorkerConnection("127.0.0.1", 2600) };
            currentMaster.Workers.Add(new WorkerConnection("127.0.0.1", 2500));
            mockReader.Setup(m => m.ReadAsync(It.Is<string>(e => e == masterDir))).ReturnsAsync(currentMaster);

            var mockWriter = new Mock<IMasterFileWriter>();
            mockWriter.Setup(m => m.WriteAsync(It.Is<string>(e => e == masterDir), It.IsAny<Master>()))
                      .Callback((string mdir, Master master) =>
                      {
                          if (master > currentMaster)
                              currentMaster = master;
                      })
                      .Returns(Task.CompletedTask);

            var mockUnitOfWorkReader = new Mock<IUnitOfWorkFileReader>();
            var mockUnitOfWorkWriter = new Mock<IUnitOfWorkFileWriter>();
            var mockDirectoryPrepareAsync = new Mock<IDirectoryPreparerAsync>();

            var worker1 = new Worker("Worker1",
                                     "127.0.0.1",
                                     2700,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            var worker2 = new Worker("Worker2",
                                     "127.0.0.1",
                                     2800,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            var worker3 = new Worker("Worker3",
                                     "127.0.0.1",
                                     2900,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            // Act
            var task1 = worker1.StartAsync();
            var task2 = worker2.StartAsync();
            var task3 = worker3.StartAsync();
            Task.WaitAll(task1, task2, task3);

            // Assert
            Assert.AreEqual(currentMaster, worker3.Master);
        }
        #endregion

        #region Election Unordered
        [TestMethod]
        public void Automated_Election2_Unordered_Test()
        {
            // Arrange
            var primaryDir = "root";
            var masterDir = "root\\Master";

            var mockReader = new Mock<IMasterFileReader>();
            var currentMaster = new Master { Connection = new WorkerConnection("127.0.0.1", 2600) };
            currentMaster.Workers.Add(new WorkerConnection("127.0.0.1", 2500));
            mockReader.Setup(m => m.ReadAsync(It.Is<string>(e => e == masterDir)))
                      .ReturnsAsync(currentMaster);

            var mockUnitOfWorkReader = new Mock<IUnitOfWorkFileReader>();
            var mockUnitOfWorkWriter = new Mock<IUnitOfWorkFileWriter>();

            var mockWriter = new Mock<IMasterFileWriter>();
            mockWriter.Setup(m => m.WriteAsync(It.Is<string>(e => e == masterDir), It.IsAny<Master>()))
                      .Callback((string mdir, Master master) =>
                      {
                          if (master > currentMaster)
                              currentMaster = master;
                      })
                      .Returns(Task.CompletedTask);
            var mockDirectoryPrepareAsync = new Mock<IDirectoryPreparerAsync>();

            var worker1 = new Worker("Worker1",
                                     "127.0.0.1",
                                     2700,
                                     new ReplyServer(),
                                     primaryDir, 
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            var worker2 = new Worker("Worker2",
                                     "127.0.0.1",
                                     2800,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            // Act
            var task2 = worker2.StartAsync();
            var task1 = worker1.StartAsync();
            Task.WaitAll(task1, task2);

            // Assert
            Assert.AreEqual(currentMaster, worker2.Master);
        }

        [TestMethod]
        public void Automated_Election3_Unordered_Test()
        {
            // Arrange
            var primaryDir = "root";
            var masterDir = "root\\Master";

            var mockReader = new Mock<IMasterFileReader>();
            var currentMaster = new Master { Connection = new WorkerConnection("127.0.0.1", 2600) };
            currentMaster.Workers.Add(new WorkerConnection("127.0.0.1", 2500));
            mockReader.Setup(m => m.ReadAsync(It.Is<string>(e => e == masterDir)))
                .ReturnsAsync(currentMaster);

            var mockWriter = new Mock<IMasterFileWriter>();
            mockWriter.Setup(m => m.WriteAsync(It.Is<string>(e => e == masterDir), It.IsAny<Master>()))
                      .Callback((string mdir, Master master) =>
                      {
                          if (master > currentMaster)
                              currentMaster = master;
                      })
                      .Returns(Task.CompletedTask);

            var mockUnitOfWorkReader = new Mock<IUnitOfWorkFileReader>();
            var mockUnitOfWorkWriter = new Mock<IUnitOfWorkFileWriter>();
            var mockDirectoryPrepareAsync = new Mock<IDirectoryPreparerAsync>();


            var worker1 = new Worker("Worker1",
                                     "127.0.0.1",
                                     2700,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            var worker2 = new Worker("Worker2",
                                     "127.0.0.1",
                                     2800,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            var worker3 = new Worker("Worker3",
                                     "127.0.0.1",
                                     2900,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            // Act
            var task2 = worker2.StartAsync();
            var task3 = worker3.StartAsync();
            var task1 = worker1.StartAsync();
            Task.WaitAll(task1, task2, task3);

            // Assert
            Assert.AreEqual(currentMaster, worker3.Master);
        }
        #endregion

        #region Election Request
        [TestMethod]
        public void ElectionRequestTest()
        {
            // Arrange
            var primaryDir = "root";
            var masterDir = "root\\Master";

            var mockReader = new Mock<IMasterFileReader>();
            var currentMaster = new Master
            {
                Connection = new WorkerConnection("127.0.0.1", 2600)
            };
            currentMaster.Workers.Add(new WorkerConnection("127.0.0.1", 2500));
            mockReader.Setup(m => m.ReadAsync(It.Is<string>(e => e == masterDir)))
                      .ReturnsAsync(() => { return currentMaster; });

            var mockUnitOfWorkReader = new Mock<IUnitOfWorkFileReader>();
            var mockUnitOfWorkWriter = new Mock<IUnitOfWorkFileWriter>();

            var mockWriter = new Mock<IMasterFileWriter>();
            mockWriter.Setup(m => m.WriteAsync(It.Is<string>(e => e == masterDir), It.IsAny<Master>()))
                      .Callback((string mdir, Master master) =>
                      {
                          if (master > currentMaster)
                              currentMaster = master;
                      })
                      .Returns(Task.CompletedTask);
            var mockDirectoryPrepareAsync = new Mock<IDirectoryPreparerAsync>();

            var worker1 = new Worker("Worker1",
                                     "127.0.0.1",
                                     2700,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            var worker2 = new Worker("Worker2",
                                     "127.0.0.1",
                                     2800,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            var task1 = worker1.StartAsync();
            var task2 = worker2.StartAsync();
            Task.WaitAll(task1, task2);

            // Act
            var task = worker1.ElectionRequestAsync(ElectionRequestType.Election);
            var actual = task.Result;

            // Assert
            Assert.AreEqual(ElectionResponseType.NoResponse, actual);
        }
        #endregion

        #region Ping
        [TestMethod]
        public void Worker_Ping_Test()
        {
            // Arrange
            var primaryDir = "root";
            var masterDir = "root\\Master";

            var mockReader = new Mock<IMasterFileReader>();
            var currentMaster = new Master
            {
                Connection = new WorkerConnection("127.0.0.1", 2600)
            };
            currentMaster.Workers.Add(new WorkerConnection("127.0.0.1", 2500));
            mockReader.Setup(m => m.ReadAsync(It.Is<string>(e => e == masterDir)))
                      .ReturnsAsync(() => { return currentMaster; });

            var mockWriter = new Mock<IMasterFileWriter>();
            mockWriter.Setup(m => m.WriteAsync(It.Is<string>(e => e == masterDir), It.IsAny<Master>()))
                      .Callback((string mdir, Master master) =>
                      {
                          if (master > currentMaster)
                              currentMaster = master;
                      })
                      .Returns(Task.CompletedTask);
            var mockUnitOfWorkReader = new Mock<IUnitOfWorkFileReader>();
            var mockUnitOfWorkWriter = new Mock<IUnitOfWorkFileWriter>();
            var mockDirectoryPrepareAsync = new Mock<IDirectoryPreparerAsync>();

            var worker1 = new Worker("Worker1",
                                     "127.0.0.1",
                                     2700,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            var worker2 = new Worker("Worker2",
                                     "127.0.0.1",
                                     2800,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            var task1 = worker1.StartAsync();
            var task2 = worker2.StartAsync();
            Task.WaitAll(task1, task2);

            // Act
            var task = worker1.PingAsync(worker2.Connection);
            var actual = task.Result;

            // Assert
            Assert.AreEqual("pong", actual);
        }
        #endregion

        #region Ping
        [TestMethod]
        public void Worker_RequestWork_Test()
        {
            // Arrange
            var primaryDir = "root";
            var masterDir = "root\\Master";

            var mockReader = new Mock<IMasterFileReader>();
            var currentMaster = new Master
            {
                Connection = new WorkerConnection("127.0.0.1", 2600)
            };
            currentMaster.Workers.Add(new WorkerConnection("127.0.0.1", 2500));

            object locker = new object();
            var masters = new List<Master>() { currentMaster }; 

            mockReader.Setup(m => m.ReadAsync(It.Is<string>(e => e == masterDir)))
                      .ReturnsAsync(() => 
                      {
                          return masters.Max();
                      });

            var mockWriter = new Mock<IMasterFileWriter>();
            mockWriter.Setup(m => m.WriteAsync(It.Is<string>(e => e == masterDir), It.IsAny<Master>()))
                      .Callback((string mdir, Master master) =>
                      {
                          if (!masters.Contains(master))
                              masters.Add(master);
                      })
                      .Returns(Task.CompletedTask);

            var unitOfWork = new UnitOfWork { Id = 1, Assigned = DateTime.Now, AssociateFile = "file1.txt"};

            var mockUnitOfWorkReader = new Mock<IUnitOfWorkFileReader>();
            mockUnitOfWorkReader.Setup(m => m.GetNextFileAsync(It.IsAny<string>())).ReturnsAsync("file1.json");
            bool workRequested = false;
            mockUnitOfWorkReader.Setup(m => m.ReadAsync(It.IsAny<string>())).ReturnsAsync((string s)=> 
            {
                if (workRequested)
                    return null;
                workRequested = true;
                return unitOfWork;
            });
            mockUnitOfWorkReader.Setup(m => m.ReadAllTextAsync(It.IsAny<string>())).ReturnsAsync("ABCDE\r\nABCEF");

            var mockUnitOfWorkWriter = new Mock<IUnitOfWorkFileWriter>();
            mockUnitOfWorkWriter.Setup(m => m.MoveAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            mockUnitOfWorkWriter.Setup(m => m.WriteAsync(It.IsAny<string>(), It.IsAny<UnitOfWork>())).Returns(Task.CompletedTask);
            var mockDirectoryPrepareAsync = new Mock<IDirectoryPreparerAsync>();

            var worker1 = new Worker("Worker1",
                                     "127.0.0.1",
                                     2700,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            var worker2 = new Worker("Worker2",
                                     "127.0.0.1",
                                     2800,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            var task1 = worker1.StartAsync();
            var task2 = worker2.StartAsync();
            Task.WaitAll(task1, task2);

            // Act
            //var task = worker1.RequestWorkAsync();
            //var actual = task.Result;
            while (worker1.State != WorkerState.Stopped)
            { }

            // Assert
            //Assert.IsTrue(actual);
        }
        #endregion

        #region Ping
        [TestMethod]
        public void Worker_Register_Test()
        {
            // Arrange
            var primaryDir = "root";
            var masterDir = "root\\Master";

            var mockReader = new Mock<IMasterFileReader>();
            var currentMaster = new Master
            {
                Connection = new WorkerConnection("127.0.0.1", 2600)
            };
            currentMaster.Workers.Add(new WorkerConnection("127.0.0.1", 2500));
            mockReader.Setup(m => m.ReadAsync(It.Is<string>(e => e == masterDir)))
                      .ReturnsAsync(() => { return currentMaster; });

            var mockWriter = new Mock<IMasterFileWriter>();
            mockWriter.Setup(m => m.WriteAsync(It.Is<string>(e => e == masterDir), It.IsAny<Master>()))
                      .Callback((string mdir, Master master) =>
                      {
                          if (master > currentMaster)
                              currentMaster = master;
                      })
                      .Returns(Task.CompletedTask);

            var mockUnitOfWorkReader = new Mock<IUnitOfWorkFileReader>();
            var mockUnitOfWorkWriter = new Mock<IUnitOfWorkFileWriter>();
            var mockDirectoryPrepareAsync = new Mock<IDirectoryPreparerAsync>();

            var worker1 = new Worker("Worker1",
                                     "127.0.0.1",
                                     2700,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            var worker2 = new Worker("Worker2",
                                     "127.0.0.1",
                                     2800,
                                     new ReplyServer(),
                                     primaryDir,
                                     mockDirectoryPrepareAsync.Object,
                                     new ElectionRequester(new RequestClient()),
                                     mockReader.Object,
                                     mockWriter.Object,
                                     mockUnitOfWorkReader.Object,
                                     mockUnitOfWorkWriter.Object);

            var task1 = worker1.StartAsync();
            var task2 = worker2.StartAsync();
            Task.WaitAll(task1, task2);
            var taskUpdateMaster = worker1.UpdateMasterAsync();

            // Act
            var task = worker1.RegisterAsync();
            var actual = task.Result;

            // Assert
            Assert.IsTrue(actual);
            Assert.AreEqual(3, worker2.Master.Workers.Count);
        }
        #endregion
    }
}