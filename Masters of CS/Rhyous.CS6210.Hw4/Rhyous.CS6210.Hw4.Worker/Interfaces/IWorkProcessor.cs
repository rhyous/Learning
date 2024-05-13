using System;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public interface IWorkProcessor
    {
        event WorkCompleteEventHandler OnComplete;
        bool IsProcessing { get; set; }
        Task<UnitOfWork> ProcessWorkAsync(IUnitOfWorkFileReader reader, UnitOfWork unitOfWork);
    }
}
