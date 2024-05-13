using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.CS6210.Hw4.Models;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw4.Worker.Tests
{
    [TestClass]
    public class RequestClientTests
    {
        [TestMethod]
        public void MyTestMethod()
        {

            // Arrange
            IpAddress ip = "127.0.0.1";
            int port = 2701;
            string onRequestReceived(string type, string request)
            {
                return "pong";
            }

            var server = new ReplyServer();
            var task = server.StartAsync<string, string>(ip, port, onRequestReceived);
            Task.Delay(1000); // Let server start
            var requester = new RequestClient();
            var packet = new Packet<string> { Payload = "ping" };

            // Act
            var requestTask = requester.SendAsync<string, string>("test", packet, ip, port);
            var actual = requestTask.Result;

            // Assert
            Assert.AreEqual("pong", actual);
        }
    }
}