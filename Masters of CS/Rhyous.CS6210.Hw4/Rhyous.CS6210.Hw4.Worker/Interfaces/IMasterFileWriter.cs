using Rhyous.CS6210.Hw4.Models;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public interface IMasterFileWriter
    {
        Task WriteAsync(string masterDirectory, Master master);
    }
}
