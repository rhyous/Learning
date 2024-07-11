using System.Numerics;

namespace Rhyous.FacebookScreeningInterview.Area
{
    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;
    }

    public class Node<T>
    {
        public Node(T value, Point point)
        {
            Value = value;
            Point = point;
        }

        public T Value { get; }
        public Point Point { get; }
        public List<Node<T>> Adjacents = new List<Node<T>>(4);
        public long Area { get; internal set; }


        // Big O (n) - the already visited property makes sure that a maximum of n nodes are processed
        public long GetArea(HashSet<Node<T>> visited) // Big O (n)
        {
            visited.Add(this);
            long totalArea = 1;
            foreach (var node in Adjacents) // Big O (n)
            {
                if (visited.Contains(node)) // Big O (1)
                    continue;
                var area = node.GetArea(visited);  // Big O (n - already processed)
                totalArea += area;
            }
            return Area = totalArea;
        }

        public void UpdateAreaOfAdjacents(HashSet<Node<T>> alreadyTraversed, long area)
        {
            if (Area == area)
                return; // Nothing to update
            Area = area;
            alreadyTraversed.Add(this);
            foreach (var node in Adjacents)
            {
                if (!alreadyTraversed.Contains(node))
                    node.UpdateAreaOfAdjacents(alreadyTraversed, area);
            }
        }

        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = hashCode * 47 * Point.X;
            hashCode = hashCode * 53 * Point.Y;
            hashCode = hashCode * 101 * Value!.GetHashCode();
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Return area of Grid of all 1s after flipping a 0 to 1. Choose the 0 that gives the max area.
    /// Area is only on an edge, not a diagnal.
    /// [1, 0]
    /// [0, 1]
    /// [1, 1]
    /// In the above example, the answer is 5 and can be done by flipping either point 0,1 from 0 to 1
    /// or point 1,0 from 0 to 1.
    /// </summary>
    /// <remarks>
    /// Proc: Big O (n)
    /// Mem: Big O (n)
    /// </remarks>
    internal class AreaOfGrid
    {
        public long Get(int[,] grid)
        {
            if (AreAllSameNumber(grid, 0)) // Big O (n)
                return 1;
            if (AreAllSameNumber(grid, 1)) // Big O (n)
                return grid.LongLength;
            var map = new Dictionary<Point, Node<int>>();
            var nodes = BuildNodes(grid, map); // Big O (n)

            var zeroNodes = nodes.Where(n => n.Value == 0); // Big O (n)
            long maxArea = 0;
            foreach (var zeroNode in zeroNodes) // Big O (n)
            {
                if (zeroNode.Area > maxArea)
                    maxArea = zeroNode.Area;
            }
            return maxArea;
        }

        // Big O (n)
        private static List<Node<int>> BuildNodes(int[,] grid, Dictionary<Point, Node<int>> map)
        {
            var nodes = new List<Node<int>>();
            // Create nodes - Big O (n) where n is the number of items in the grid
            for (int col = 0; col < grid.GetLength(0); col++)
            {
                for (int row = 0; row < grid.GetLength(1); row++)
                {
                    var point = new Point(col, row);
                    var node = new Node<int>(grid[col, row], point);
                    nodes.Add(node);
                    map.Add(point, node);
                }
            }
            // Create adjacents - Big O (n) where n is the number of items in the grid
            foreach (var node in nodes)
            {
                // Get above - Big O (1)
                var abovePoint = new Point(node.Point.X, node.Point.Y - 1); 
                if (map.TryGetValue(abovePoint, out Node<int> aboveNode) && aboveNode.Value == 1) // Big O (1)
                {
                    node.Adjacents.Add(aboveNode);
                }
                // Get Below- Big O (1)
                var belowPoint = new Point(node.Point.X, node.Point.Y + 1);
                if (map.TryGetValue(belowPoint, out Node<int> belowNode) && belowNode.Value == 1) // Big O (1)
                {
                    node.Adjacents.Add(belowNode);
                }
                // Get left- Big O (1)
                var leftPoint = new Point(node.Point.X - 1, node.Point.Y);
                if (map.TryGetValue(leftPoint, out Node<int> leftNode) && leftNode.Value == 1) // Big O (1)
                {
                    node.Adjacents.Add(leftNode);
                }
                // Get Below- Big O (1)
                var rightPoint = new Point(node.Point.X + 1, node.Point.Y);
                if (map.TryGetValue(rightPoint, out Node<int> rightNode) && rightNode.Value == 1) // Big O (1)
                {
                    node.Adjacents.Add(rightNode);
                }
            }
            // Populate Area - Big O (n^2)
            foreach (var node in nodes) // No filter on 0 Nodes as they also have calclate their area as if they are 1 nodes
            {
                if (node.Area == 0)
                {                    
                    var area = node.GetArea(new HashSet<Node<int>> { node }); // Big O (n) - this is what makes this method n^2 at worst case
                    if (node.Value == 1) // Don't do this next for zero nodes
                    {
                        // Populates all nodes of 1 in an area so the Area of every node is only calculated once.
                        // which alleviates the Big O(n^2) in most cases so it is usually closer to Big O (n)
                        node.UpdateAreaOfAdjacents(new HashSet<Node<int>> { node }, area);
                    }
                }
            }
            return nodes;
        }

        // Big O (n)
        public bool AreAllSameNumber(int[,] grid, int value)
        {
            bool isValue = true;
            for (int col = 0; col < grid.GetLength(0); col++)
            {
                for (int row = 0; row < grid.GetLength(1); row++)
                {
                    isValue = grid[col, row] == value;
                    if (!isValue)
                        break;
                }
                if (!isValue)
                    break;
            }
            return isValue;
        }
    }



}
