using System;

namespace Rhyous.StringAlgorithms
{
    public class Levenshtein
    {
        public static int[,] WagnerFischer(string miStr, string njStr)
        {
            int[,] d = new int[miStr.Length + 1, njStr.Length + 1];
            int m = miStr.Length;
            int n = njStr.Length;
            for (int i = 1; i <= m; i++)
            {
                d[i, 0] = i;
            }
            for (int j = 1; j <= n; j++)
            {
                d[0, j] = j;
            }
            for (int j = 1; j <= n; j++)
            {
                for (int i = 1; i <= m; i++)
                {
                    if (miStr[i - 1] == njStr[j - 1])
                        d[i, j] = d[i - 1, j - 1];        // no operation required
                    else
                    {
                        var deletion = d[i - 1, j] + 1;
                        var insertion = d[i, j - 1] + 1;
                        var substitution = d[i - 1, j - 1] + 1;
                        d[i, j] = Math.Min(Math.Min(deletion, insertion), substitution);
                    }
                }
            }
            return d;
        }
    }
}
