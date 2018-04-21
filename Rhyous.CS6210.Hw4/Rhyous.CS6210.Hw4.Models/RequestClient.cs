using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Threading.Tasks;
using ZeroMQ;
using Rhyous.Collections;

namespace Rhyous.CS6210.Hw4.Models
{
    public class RequestClient : IRequestClient
    {
        public virtual TimeSpan RequestTimeout => TimeSpan.FromMilliseconds(ConfigurationManager.AppSettings.Get("RequestTimeoutMilliseconds", 3000));
        public virtual int RequestRetries => ConfigurationManager.AppSettings.Get("RequestRetries", 3);

        static async Task<CreateSocketResult> GetRequestZSocketAsync(ZContext context, string name, IpAddress ip, int port)
        {
            return await Task.Run(() =>
            {
                var requester = new ZSocket(context, ZSocketType.REQ)
                {
                    Linger = TimeSpan.FromMilliseconds(1)
                };
                if (!string.IsNullOrWhiteSpace(name))
                    requester.IdentityString = name;
                requester.Connect($"tcp://{ip}:{port}", out ZError _error);
                return new CreateSocketResult { Socket = requester, Error = _error };
            });
        }

        public virtual async Task<TResponse> SendAsync<TRequest, TResponse>(string name, Packet<TRequest> packet, IpAddress ip, int port, Func<TResponse, TResponse> successFunction = null, Func<object, TResponse> failureFunction = null, Func<object, TResponse> timeoutFunction = null)
        {
            // Validation
            if (packet == null)
                throw new ArgumentException("packet", string.Format(Messages.ObjectNull, "packet"));
            if (ip == null)
                throw new ArgumentException("ip", string.Format(Messages.ObjectNull, "ip"));
            if (port < 1024)
                throw new ArgumentException("port", Messages.PortNoInRange);
            if (successFunction == null)
                successFunction = OnSuccess;
            if (failureFunction == null)
                failureFunction = OnFailure<TResponse>;
            if (timeoutFunction == null)
                timeoutFunction = OnTimeout<TResponse>;

            using (var context = new ZContext())
            {
                ZSocket requester = null;
                try
                {
                    var createSocketResult = await GetRequestZSocketAsync(context, name, ip, port);
                    if (createSocketResult.Error != ZError.None)
                    {
                        return failureFunction.Invoke(createSocketResult.Error);
                    }
                    requester = createSocketResult.Socket;
                    int retries_left = RequestRetries;
                    var poll = ZPollItem.CreateReceiver();
                    while (retries_left > 0)
                    {
                        var json = JsonConvert.SerializeObject(packet);
                        var frame = new ZFrame(json);
                        if (!requester.Send(frame, out ZError error))
                        {
                            if (error != ZError.None)
                                return failureFunction.Invoke(error);
                        }

                        while (true)
                        {
                            if (requester.PollIn(poll, out ZMessage incomingMessage, out error, RequestTimeout))
                            {
                                using (incomingMessage)
                                {
                                    var responseJson = incomingMessage[0].ReadString();
                                    var responsePacket = JsonConvert.DeserializeObject<Packet<TResponse>>(responseJson);
                                    retries_left = RequestRetries;
                                    return successFunction.Invoke(responsePacket.Payload);
                                }
                            }
                            else
                            {
                                if (error == ZError.EAGAIN)
                                {
                                    if (--retries_left == 0)
                                    {
                                        Console.WriteLine($"Worker did not respond: {ip}:{port}.");
                                        break;
                                    };

                                    // Old socket is confused; close it and open a new one
                                    requester.Dispose();
                                    createSocketResult = await GetRequestZSocketAsync(context, name, ip, port);
                                    if (createSocketResult.Error != ZError.None)
                                    {
                                        return failureFunction.Invoke(createSocketResult.Error);
                                    }
                                    requester = createSocketResult.Socket;
                                    var retryFrame = new ZFrame(json);
                                    if (!requester.Send(retryFrame, out error))
                                    {
                                        if (error != ZError.None)
                                            return failureFunction.Invoke(error);
                                    }
                                    continue;
                                }

                                if (error != ZError.None)
                                    return failureFunction(error);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                }
                finally
                {
                    if (requester != null)
                    {
                        requester.Dispose();
                        requester = null;
                    }
                }
                return timeoutFunction(new { Timeout = RequestTimeout.Milliseconds, Retries = RequestRetries });
            }
        }

        protected virtual TResponse OnSuccess<TResponse>(TResponse response)
        {
            return response;
        }

        protected virtual TResponse OnFailure<TResponse>(object o)
        {
            return default;
        }

        protected virtual TResponse OnTimeout<TResponse>(object o)
        {
            return default;
        }
    }
}