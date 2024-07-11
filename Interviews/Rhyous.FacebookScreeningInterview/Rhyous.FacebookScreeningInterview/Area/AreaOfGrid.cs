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

        public long GetArea(HashSet<Node<T>> alreadyTraversed)
        {
            alreadyTraversed.Add(this);
            long totalArea = 1;
            foreach (var node in Adjacents)
            {
                if (alreadyTraversed.Contains(node))
                    continue;
                var area = node.GetArea(alreadyTraversed);
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
    internal class AreaOfGrid
    {
        public long Get(int[,] grid)
        {
            if (AreAllSameNumber(grid, 0))
                return 1;
            if (AreAllSameNumber(grid, 1))
                return grid.LongLength;
            var map = new Dictionary<Point, Node<int>>();
            var nodes = BuildNodes(grid, map);

            var zeroNodes = nodes.Where(n => n.Value == 0);
            long maxArea = 0;
            foreach (var zeroNode in zeroNodes)
            {
                if (zeroNode.Area > maxArea)
                    maxArea = zeroNode.Area;
            }
            return maxArea;
        }

        private static List<Node<int>> BuildNodes(int[,] grid, Dictionary<Point, Node<int>> map)
        {
            var nodes = new List<Node<int>>();
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
            foreach (var node in nodes)
            {
                // Get above
                var abovePoint = new Point(node.Point.X, node.Point.Y - 1);
                if (map.TryGetValue(abovePoint, out Node<int> aboveNode) && aboveNode.Value == 1)
                {
                    node.Adjacents.Add(aboveNode);
                }
                // Get Below
                var belowPoint = new Point(node.Point.X, node.Point.Y + 1);
                if (map.TryGetValue(belowPoint, out Node<int> belowNode) && belowNode.Value == 1)
                {
                    node.Adjacents.Add(belowNode);
                }
                // Get left
                var leftPoint = new Point(node.Point.X - 1, node.Point.Y);
                if (map.TryGetValue(leftPoint, out Node<int> leftNode) && leftNode.Value == 1)
                {
                    node.Adjacents.Add(leftNode);
                }
                // Get Below
                var rightPoint = new Point(node.Point.X + 1, node.Point.Y);
                if (map.TryGetValue(rightPoint, out Node<int> rightNode) && rightNode.Value == 1)
                {
                    node.Adjacents.Add(rightNode);
                }
            }
            foreach (var node in nodes)
            {
                if (node.Area == 0)
                {
                    var area = node.GetArea(new HashSet<Node<int>> { node });
                    if (node.Value == 1)
                        node.UpdateAreaOfAdjacents(new HashSet<Node<int>> { node }, area);
                }
            }
            return nodes;
        }

        public bool AreAllSameNumber(int[,] grid, int value)
        {
            for (int col = 0; col < grid.GetLength(0); col++)
            {
                bool isValue = true;
                for (int row = 0; row < grid.GetLength(1); row++)
                {
                    isValue = grid[col, row] == value;
                    if (!isValue)
                        break;
                }
                if (!isValue)
                    break;
            }
            return false;
        }
    }



}
