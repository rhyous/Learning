using Rhyous.CS6210.Hw4.Models;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public interface IUnitOfWorkFileWriter
    {
        Task WriteAsync(string directory, UnitOfWork unitOfWork);
        Task MoveAsync(string source, string destination);
    }
}
