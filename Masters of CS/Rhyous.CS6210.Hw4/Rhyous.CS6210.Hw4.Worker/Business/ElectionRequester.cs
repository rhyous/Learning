using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Rhyous.CS6210.Hw4.Models;
using ZeroMQ;

namespace Rhyous.CS6210.Hw4
{
    public class ElectionRequester : IElectionRequester
    {
        private readonly IRequestClient _Client;
        public string Name;

        public ElectionRequester(IRequestClient client, string name = null)
        {
            _Client = client;
            Name = name;
        }

        public Task<ElectionResponse> ElectMeAsync(WorkerConnection connection)
        {
            var packet = new Packet<ElectionRequest>
            {
                Type = "Elect",
                Payload = new ElectionRequest { RequestType = ElectionRequestType.Election }
            };
            var result = _Client.SendAsync(connection.Name, packet, connection.IpAddress, 
                                           connection.Port, OnSuccess, OnFailure, OnTimeout);
            return result;
        }

        internal ElectionResponse OnSuccess(ElectionResponse ElectionResponse)
        {
            return ElectionResponse;
        }

        internal ElectionResponse OnFailure(object error)
        {
            return new ElectionResponse { ResponseType = ElectionResponseType.Failure };
        }

        internal ElectionResponse OnTimeout(object error)
        {
            return new ElectionResponse { ResponseType = ElectionResponseType.NoResponse };
        }

        public Task<ElectionResponse> ResignAsync(WorkerConnection connection)
        {
                var packet = new Packet<ElectionRequest>
                {
                    Type = "Elect",
                    Payload = new ElectionRequest { RequestType = ElectionRequestType.Resignation }
                };
                var result = _Client.SendAsync(null, packet, connection.IpAddress,
                                               connection.Port, OnSuccess, OnFailure, OnTimeout);
                return result;
        }
    }
}
