using Rhyous.CS6210.Hw1.Interfaces;
using System;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class PublisherServer : RegistrationClient, IServer<ZFrame>, IReply, IDisposable
    {
        public PublisherServer(string name, string endpoint, string nsEndpoint, ILogger logger) 
            : base(nsEndpoint, new SystemRegistration(name, endpoint), true, logger)
        {
            Name = name;
            Endpoint = endpoint;
        }

        public string Name { get; set; }
        public ZContext Context { get; set; }
        public ISendSocketServer Socket { get; set; }

        public virtual async Task StartAsync(string endpoint, ZSocketType type, Action<ZFrame> receiveAction)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                if (string.IsNullOrWhiteSpace(Endpoint))
                    await RegisterAsync();
                endpoint = Endpoint;
            }
            await Task.Run(() =>
           {
               Context = Context ?? new ZContext();
               Socket = Socket ?? new SendSocketServerAdapter(new ZSocket(Context, ZSocketType.PUB));
               Logger?.WriteLine($"Starting {Name} on {endpoint}.", new VectorTimeStamp().Update(SystemRegistration.Id, DateTime.Now));
               Socket.Bind(endpoint);
           });
        }


        public virtual void Stop()
        {
            Dispose();
        }

        public virtual async Task ReplyAsync(string message)
        {
            await Socket.SendAsync(message);
        }

        #region IDisposable

        internal bool _IsDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_IsDisposed)
            {
                _IsDisposed = true;
                if (disposing)
                {
                    Socket?.Dispose();
                    Context?.Dispose();
                }
            }
        }

        #endregion
    }
}