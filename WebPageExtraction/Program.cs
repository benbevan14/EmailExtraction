using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Linq;

namespace WebPageExtraction
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input to decide whether to look for attributes or tags
            Console.WriteLine("Tags or attributes? (t/a)");
            string tagOrAttrib = Console.ReadLine() == "a" ? "attributes" : "tags";
            Console.WriteLine($"What {tagOrAttrib} would you like to see?");
            string type = Console.ReadLine();

            // Scrape all the html from a hard-coded url
            string content = "";
            using (var client = new HttpClient())
            {
                var result = client.GetStringAsync("https://bulbapedia.bulbagarden.net/wiki/Pikachu_(Pok%C3%A9mon)");
                content = result.Result;
            }

            // Specify different regexes to look for tags vs attributes
            //var tagPattern = @"<([^<]+) ?\/>|<([^<]*) ?([^<]*)?>\s*(.*?)\s*<\/\2>";;
            var tagPattern = @"(<" + type + @"[\s\S]*?>)[\s\S]*?(<\/" + type + @">)";
            var attribPattern = type + @"=[""'][\S]+[""']";

            // Initialize a finder and a dictionary to store the found values
            Finder finder = new Finder(tagOrAttrib == "attributes" ? attribPattern : tagPattern);
            Dictionary<string, int> found = new Dictionary<string, int>();

            // For each match found, add it to a dictionary
            foreach (Match m in finder.FindMatches(content))
            {
                string name = "";
                if (tagOrAttrib == "attributes")
                {
                    name = m.ToString().Split('=')[1].Trim('\'', '"');
                }
                else
                {
                    name = m.ToString();
                }

                found.TryGetValue(name, out var nameCount);
                found[name] = nameCount + 1;
            }

            // Sort the dictionary by number of occurrences more than 10
            var sortedFound = from pair in found
                              orderby pair.Value descending
                              where pair.Value > 10
                              select pair;

            foreach (KeyValuePair<string, int> item in sortedFound)
            {
                Console.WriteLine(item.Key + ": " + item.Value);
            }

            Console.WriteLine("done");

            Console.ReadLine();
        }
    }
}
