using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw1.Interfaces
{
    public interface IReply
    {
        Task ReplyAsync(string message);
    }
}
