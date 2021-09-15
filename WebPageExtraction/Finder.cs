using System.Text.RegularExpressions;

namespace WebPageExtraction
{
    public class Finder
    {
        private string RegexPattern { get; set; }

        public Finder(string regex)
        {
            this.RegexPattern = regex;
        }

        public MatchCollection FindMatches(string input)
        {
            return Regex.Matches(input, this.RegexPattern);
        }
    }
}
