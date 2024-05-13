using Rhyous.CS6210.Hw4.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{

    public interface IWorkHandler
    {
        Task DoWorkAsync(UnitOfWork unitOfWork);
        Task<bool> RequestWorkAsync();
        Task<bool> ReportWorkProgressAsync(UnitOfWork unitOfWork);
        Task<bool> ReportWorkCompletionAsync(UnitOfWork unitOfWork);
        Task<bool> MarkWorkCompletedAsync(UnitOfWork unitOfWork);
    }
}
