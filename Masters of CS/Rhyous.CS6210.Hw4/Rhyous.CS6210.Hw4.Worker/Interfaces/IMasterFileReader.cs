using Rhyous.CS6210.Hw4.Models;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public interface IMasterFileReader
    {
        Task<Master> ReadAsync(string masterDirectory);
    }
}