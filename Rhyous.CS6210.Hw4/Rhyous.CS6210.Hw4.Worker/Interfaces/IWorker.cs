using Rhyous.CS6210.Hw4.Models;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public interface IWorker : IWorkHandler
    {
        WorkerConnection Connection { get; set; }
        Master Master { get; set; }
        bool IsMaster { get; }
        WorkerState State { get; set; }
        Task StartAsync();
        Task<Master> GetMasterAsync();
        Task UpdateMasterAsync();

        Task<Master> UpdateElectionFileAsync();

        Task<string> PingAsync(WorkerConnection connection);

        Task<bool> RegisterAsync();
        Task<Master> TakeOverAsync(Master currentMaster, Master myselfAsMaster, string pong);

        Task<ElectionResponseType> ElectionRequestAsync(ElectionRequestType type);

        Task StartListenerAsync();

        Task<UnitOfWork> QueueNextUnitOfWorkAsync();
    }
}
