namespace Rhyous.FacebookScreeningInterview.PowerSet
{
    public class PowerSetBuilder
    {
        public List<List<int>> GetPowerSet(List<int> set)
        {
            var empty = new List<int>();
            var first = new List<int> { set[0] };
            var sets = new List<List<int>> { empty, first };
            for (int i = 1; i < set.Count; i++)  // Big O(n)
            {
                var setsCount = sets.Count;
                for (int j = 0; j < setsCount; j++) // Big O (m where m doubles each time)
                {
                    var newSet = new List<int>(sets[j]);
                    newSet.Add(set[i]);
                    sets.Add(newSet);
                }
            }
            return sets;
        }
    }

}