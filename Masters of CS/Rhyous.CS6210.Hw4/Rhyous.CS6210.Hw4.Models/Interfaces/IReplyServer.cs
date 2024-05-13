using System;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4.Models
{
    public interface IReplyServer
    {
        Task StartAsync<TRequest, TReply>(IpAddress ip, int port, Func<string, TRequest, TReply> onRequestReceived);
        Task StopAsync();
    }
}