using System.Text;

namespace Rhyous.FacebookScreeningInterview.Parenthesis
{
    internal class BalanceParenthesis
    {
        public string Balance(string str)
        {
            var stack = new Stack<int>();
            bool[] keep = new bool[str.Length];

            for (int i = 0; i < str.Length; i++) 
            {
                var c = str[i];
                if (c == '(')
                {
                    stack.Push(i);
                    continue;
                }
                if (c == ')') 
                {
                    if (stack.Any())
                    {
                        keep[i] = true;
                        var openParenIndex = stack.Pop();
                        keep[openParenIndex] = true;
                    }
                    continue;
                }
                // Any other character
                keep[i] = true;
            }

            var builder = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if (keep[i])
                    builder.Append(str[i]);
            }
            return builder.ToString();
        }
    }
}
