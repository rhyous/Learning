using System;

namespace Rhyous.CS6210.Hw1.HealthDistrict
{
    public static class StringExtensions
    {
        public static string[] ToArray(this string str, params char[] splitCharacters)
        {
            if (splitCharacters == null)
                splitCharacters = new[] { ',' };
            return str.Split(splitCharacters, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
