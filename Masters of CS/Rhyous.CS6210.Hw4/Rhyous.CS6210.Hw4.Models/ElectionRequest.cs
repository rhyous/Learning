namespace Rhyous.CS6210.Hw4.Models
{
    public class ElectionRequest
    {
        public ElectionRequestType RequestType { get; set; }
    }

    public enum ElectionRequestType
    {
        Resignation,
        Election
    }
}
