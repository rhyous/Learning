using System;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4.Models
{
    public interface IRequestClient
    {
        Task<TResponse> SendAsync<TRequest, TResponse>(string name, 
                                             Packet<TRequest> packet, 
                                             IpAddress ip, 
                                             int port, 
                                             Func<TResponse, TResponse> successFunction = null, 
                                             Func<object, TResponse> failureFunction = null, 
                                             Func<object, TResponse> timeoutFunction = null);
    }
}
