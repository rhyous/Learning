using Rhyous.CS6210.Hw4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public interface IUnitOfWorkFileReader
    {
        Task<UnitOfWork> ReadAsync(string pathToFile);
        Task<string> ReadAllTextAsync(string pathToFile);
        Task<string> GetNextFileAsync(string unitOfWorkDirectory);
        Task<List<string>> GetFilesAsync(string queueDir, string filter = null);
    }
}