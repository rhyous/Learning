using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.CS6210.Hw4.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4.Dictionary
{
    public class RequestHandler : Dictionary<string, Func<IWorker, object, Task<object>>>
    {
        internal RequestHandler() : base(StringComparer.OrdinalIgnoreCase)
        {
            Add("Ping", PingHandler);
            Add("Elect", ElectionHandler);
            Add("WorkRequest", WorkRequestHandler);
            Add("Register", RegistrationHandler);
            Add("WorkCompletion", WorkCompletionHandler);
        }

        public async Task<object> PingHandler(IWorker w, object o) => "pong";

        public async Task<object> WorkCompletionHandler(IWorker w, object o)
        {
            UnitOfWork work = o as UnitOfWork;
            if (work == null && o is JObject)
                work = JsonConvert.DeserializeObject<UnitOfWork>(o.ToString());
            return await w.MarkWorkCompletedAsync(work);
        }

        public async Task<object> ElectionHandler(IWorker w, object o)
        {
            await w.UpdateMasterAsync();
            return new ElectionResponse { };
        }

        public async Task<object> WorkRequestHandler(IWorker w, object o)
        {
            return await w.QueueNextUnitOfWorkAsync();
        }

        public async Task<object> RegistrationHandler(IWorker w, object o)
        {
            if (!w.IsMaster)
                return false;
            if (w.IsMaster)
            {                
                var conn = JsonConvert.DeserializeObject<WorkerConnection>(o.ToString());
                if (conn != null)
                {
                    if (w.Master < new Master { Connection = conn })
                    {
                        await w.ElectionRequestAsync(ElectionRequestType.Resignation);
                        return false;
                    }
                    if (!w.Master.Workers.Contains(conn))
                    {
                        w.Master.Workers.Add(conn);
                        await w.UpdateElectionFileAsync();
                    }
                    return true;
                }                    
            }
            return false;
        }
    }
}
