using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw1.Interfaces
{
    public interface ISendAsync
    {
        Task SendAsync(string messsage);
    }
}
