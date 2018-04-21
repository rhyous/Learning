using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4
{
    public interface IDirectoryPreparerAsync
    {
        Task<bool> PrepareAync(string primaryDirectory, IEnumerable<string> subdirs);
        Task<bool> DirectoryExistsAsync(string dir);
        Task<bool> FileExistsAsync(string fileFullPath);
    }
}