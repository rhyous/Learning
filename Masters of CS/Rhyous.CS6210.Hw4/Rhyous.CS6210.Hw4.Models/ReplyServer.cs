using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw4.Models
{
    public class ReplyServer : IReplyServer
    {
        internal bool StopRquested = false;

        public async Task StartAsync<TRequest, TReply>(IpAddress ip, int port, Func<string, TRequest, TReply> onRequestReceived)
        {
            await Task.Run(() =>
            {
                using (var context = new ZContext())
                using (var responder = new ZSocket(context, ZSocketType.REP))
                {
                    responder.Bind($"tcp://{ip}:{port}");
                    while (!StopRquested)
                    {
                        // Receive
                        using (ZFrame frame = responder.ReceiveFrame())
                        {
                            var json = frame.ReadString();
                            var packet = JsonConvert.DeserializeObject<Packet<TRequest>>(json);
                            var result = onRequestReceived(packet.Type, packet.Payload);
                            if (result is Task<TReply>)
                                (result as Task<TReply>).Wait();
                            TReply reply = result is Task<TReply> ? (result as Task<TReply>).Result : result;
                            var replyPacket = new Packet<TReply> { Payload = reply };
                            var replyJson = JsonConvert.SerializeObject(replyPacket);
                            var replyFrame = new ZFrame(replyJson);
                            responder.Send(replyFrame);
                        }
                    }
                }
            });
        }

        public async Task StopAsync()
        {
            await Task.Run(() => { StopRquested = true; });
        }
    }
}
