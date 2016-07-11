using System.Text.RegularExpressions;

namespace uFrame.Editor.Core
{
    public static class StringExtensions
    {
        public static string PrettyLabel(this string label)
        {
            return Regex.Replace(label, @"[^\w\s]|_", "");
        }
    }
}