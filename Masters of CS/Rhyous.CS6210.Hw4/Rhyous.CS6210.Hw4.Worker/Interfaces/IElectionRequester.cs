using Rhyous.CS6210.Hw4.Models;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public interface IElectionRequester
    {
        Task<ElectionResponse> ElectMeAsync(WorkerConnection connection);
        Task<ElectionResponse> ResignAsync(WorkerConnection connection);
    }
}
