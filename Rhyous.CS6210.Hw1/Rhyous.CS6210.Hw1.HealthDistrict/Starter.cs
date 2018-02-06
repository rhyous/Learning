using Rhyous.SimpleArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.HealthDistrict
{
    internal class Starter
    {
        internal static void Start()
        {
            Console.WriteLine("Health District: " + Args.Value(Constants.DistrictServerName));
            var endpoint = Args.Value(Constants.DistrictServerEndPoint);
            Console.WriteLine("Endpoint: " + endpoint);
            var districtServer = new DistrictServer();
            districtServer.Start(endpoint);


        }
    }
}