namespace Rhyous.FacebookScreeningInterview.GetLast
{
    public class GetLastWorker<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _Dictionary = new Dictionary<TKey, TValue>();
        private readonly Dictionary<TValue, Node<TValue>> _Nodes = new Dictionary<TValue, Node<TValue>>();
        Node<TValue> _FirstNode;

        public void Set(TKey key, TValue value)
        {
            _Dictionary[key] = value;
            UpdateLast(value);
        }

        public TValue Get(TKey key)  // Big O (1)
        {
            if (_Dictionary.TryGetValue(key, out TValue value)) // Big O (1)
            {
                UpdateLast(value); // Big O (1)
                return value;
            }
            return default;
        }

        public TValue Last() // Big O (1)
        {
            return _FirstNode is null ? default : _FirstNode.Value; // Big O (1)
        }

        public void Remove(TKey key) // Big O (1)
        {
            if (_Dictionary.TryGetValue(key, out TValue value)) // Big O (1)
            {
                _Dictionary.Remove(key); // Big O (1)
                if (_Nodes.TryGetValue(value, out Node<TValue> n)) // Big O (1)
                {
                    RemoveNode(n);
                }
            }
        }

        public void UpdateLast(TValue value) // Big O (1)
        {
            // Move it to the front if it exists
            if (_Nodes.TryGetValue(value, out Node<TValue> n)) // Big O (1)
            {
                // If it is already the front node, nothing to do
                if (n == _FirstNode) // Big O (1)
                    return;
                RemoveNode(n); // Big O (1)
                AddAsFirstNode(n); // Big O (1)
            }
            else // Create a node and add it to the front
            {
                n = new Node<TValue> { Value = value }; // Big O (1)
                _Nodes.Add(value, n); // Big O (1)

                if (_FirstNode == null) // Big O (1)
                {
                    _FirstNode = n; // Big O (1)
                }
                else
                {
                    AddAsFirstNode(n); // Big O (1)
                }
            }
        }

        private void AddAsFirstNode(Node<TValue> n) // Big O (1)
        {
            n.Next = _FirstNode; // Big O (1)
            _FirstNode.Previous = n; // Big O (1)
            _FirstNode = n; // Big O (1)
        }
        private void RemoveNode(Node<TValue> n) // Big O (1)
        {
            if (n == _FirstNode) // Big O (1)
            {
                n.Next.Previous = null; // Big O (1)
                _FirstNode = n.Next; // Big O (1)
                return;
            }
            if (n.Next == null) // it is the last node
            {
                n.Previous.Next = null; // Big O (1)
                return;
            }
            else
            {
                n.Previous.Next = n.Next; // Big O (1)
                n.Next.Previous = n.Previous; // Big O (1)
                n.Previous = null; // Big O (1)
            }
        }
    }
}
