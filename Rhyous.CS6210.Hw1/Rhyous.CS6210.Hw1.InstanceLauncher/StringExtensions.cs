namespace Rhyous.AmazonEc2InstanceManager
{
    public static class StringExtensions
    {
        public static string RemoveFirstLine(this string text, char newLineChar = '\n')
        {
            if (string.IsNullOrEmpty(text))
                return text;
            var i = text.IndexOf(newLineChar);            
            return i > 0 ? text.Substring(i + 1) : "";
        }

        public static string RemoveLastLine(this string text, char newLineChar = '\n')
        {
            var i = text.LastIndexOf(newLineChar);
            return (i > 0) ? text.Substring(0, i) : "";
        }

        public static string Linearize(this string text)
        {
            return text.Replace("\r", "").Replace("\n", "");
        }
    }
}