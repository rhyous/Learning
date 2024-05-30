using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.FacebookScreeningInterview.Tree
{
    //  Given a tree, find the smallest subtree that contains all the deepest nodes.
    //  
    //  
    //         a
    //       / | \ 
    //      b  c  d
    //     /\.     \
    //    e  f      x
    //  
    //  
    //    Input: a
    //    Deepest Nodes: e, f, x
    //    Smallest Subtree: a
    //  

    public class Graph<T>
        where T : notnull
    {
        private readonly Dictionary<T, Node<T>> _Items = new Dictionary<T, Node<T>>();

        public int Count => _Items.Count;

        public Node<T> AddNode(T value)
        {
            if (!_Items.TryGetValue(value, out _))
            {
                _Items.Add(value, new Node<T>(value));
            }
            return _Items[value];
        }

        public void AddChildren(T val1, IEnumerable<T> vals)
        {
            foreach (var v in vals)
            {
                AddChild(val1, v);
            }
        }

        public void AddChild(T val1, T val2)
        {
            if (!_Items.TryGetValue(val1, out Node<T> node1))
            {
                node1 = AddNode(val1);
            }
            if (!_Items.TryGetValue(val2, out Node<T> node2))
            {
                node2 = AddNode(val2);
            }
            node1.Children.Add(node2);
            node2.Parent = node1;
        }

        public List<Node<T>> GetDeepest(T value)
        {
            if (!_Items.TryGetValue(value, out Node<T> root))
            {
                throw new ArgumentException();
            }
            var queue = new Queue<Tuple<int, Node<T>>>();
            queue.Enqueue(new Tuple<int, Node<T>>(0, root));
            var levels = new Dictionary<int, List<Node<T>>>();
            while (queue.Count > 0)
            {
                var tuple = queue.Dequeue();
                var level = tuple.Item1;
                var node = tuple.Item2;
                if (levels.TryGetValue(level, out List<Node<T>> levelNodes))
                {
                    levelNodes.Add(node);
                }
                else
                {
                    levels.Add(level, new List<Node<T>> { node });
                }

                if (node.Children.Count > 0)
                {
                    foreach (var n in node.Children)
                    {
                        var t = new Tuple<int, Node<T>>(level + 1, n);
                        queue.Enqueue(t);
                    }
                }
            }
            var deepestLevel = levels.Keys.Max();
            return levels[deepestLevel];
        }

        public Node<T> GetCommonAncestor(List<Node<T>> list)
        {
            var left = list[0];
            var leftLevelsAbove = 0;
            for (int i = 1; i < list.Count; i++)
            {
                var right = list[i];
                var levelsUp = 0;
                while (left != right)
                {
                    if (left.Parent == null)
                        return left;
                    if (right.Parent == null)
                        return right;
                    if (leftLevelsAbove > 0)
                        leftLevelsAbove--;
                    else
                        left = left.Parent;
                    right = right.Parent;
                    levelsUp++;
                }
                leftLevelsAbove = levelsUp;
            }
            return left;
        }
    }

    public class Node<T>
    {
        public Node(T value)
        {
            Value = value;
            Children = new List<Node<T>>();
        }
        public T Value { get; }
        public Node<T> Parent { get; set; }
        public List<Node<T>> Children { get; }
    }

}
