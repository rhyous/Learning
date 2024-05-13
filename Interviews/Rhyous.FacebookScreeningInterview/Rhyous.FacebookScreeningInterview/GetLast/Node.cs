namespace Rhyous.FacebookScreeningInterview.GetLast
{
    public class Node<TValue>
    {
        public TValue Value { get; set; }
        public Node<TValue> Next { get; set; }
        public Node<TValue> Previous { get; set; }
    }
}
