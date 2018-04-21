using ZeroMQ;

namespace Rhyous.CS6210.Hw4.Models
{
    public class CreateSocketResult
    {
        public ZSocket Socket { get; set; }
        public ZError Error { get; set; }
    }
}