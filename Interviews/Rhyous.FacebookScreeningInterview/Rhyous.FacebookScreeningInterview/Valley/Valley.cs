using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.FacebookScreeningInterview.Valley
{

    public class IndexValue(int index, int value)
    {
        public int Index { get; } = index;
        public int Value { get; } = value;
    }

    // Find a valley in an array of numbers, where the adjacent numbers are greater than or equal.
    internal class Valley
    {
        public IndexValue GetValley(int[] array, int offset = 0)
        {
            if (array is null) { throw new ArgumentNullException(nameof(array)); }
            if (!array.Any()) { throw new ArgumentException(nameof(array)); }

            if (array.Length == 1)
                return new IndexValue(0 + offset, array[0]);

            if (array.Length == 2)
            {
                if (array[0] < array[1])
                {
                    return new IndexValue(0 + offset, array[0]);
                }
                else
                {
                    return new IndexValue(1 + offset, array[1]);
                }
            }

            var mid = array.Length / 2;
            var left = mid - 1;
            var right = mid + 1;
            if (array[mid] < array[left] && array[mid] < array[right])
            {
                return new IndexValue(mid + offset, array[mid]);
            }
            if (array[mid] < array[left])
            {
                var subArray = GetRightSubArray(array, mid);
                return GetValley(subArray, offset + mid + 1);
            }
            else
            {
                var subArray = GetLeftSubArray(array, mid);
                return GetValley(subArray, offset);
            }
        }

        public int[] GetLeftSubArray(int[] array, int mid)
        {
            var segment = new ArraySegment<int>(array, 0, array.Length - mid - 1);
            return segment.ToArray();
        }

        public int[] GetRightSubArray(int[] array, int mid)
        {
            var startPos = array.Length - mid + 1; // Remember array.Length is always 1 greater than the index
            var segment = new ArraySegment<int>(array, startPos, array.Length - startPos);
            return segment.ToArray();
        }
    }
}
