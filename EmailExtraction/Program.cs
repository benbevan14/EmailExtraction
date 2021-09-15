using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace EmailExtraction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter a top level domain: ");
            string tld = Console.ReadLine();

            string path = "C:\\Users\\benjaminb\\Work\\Training\\EmailExtraction\\Emails.txt";
            string text = File.ReadAllText(path);
            List<string> wordList = new List<string>(text.Split(' ', '\n', '\r'));

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            Regex rgx = new Regex(pattern);

            Dictionary<string, int> addresses = new Dictionary<string, int>();
            Dictionary<string, int> addressesTopLevel = new Dictionary<string, int>();

            foreach (string str in wordList)
            {
                if (rgx.IsMatch(str))
                {
                    // Get the domain of the email
                    string domain = str.Split('@')[1];
                    string domainTopLevel = str.Split('@')[1].Split('.')[0];

                    if (domainTopLevel.Equals(tld))
                    {
                        Console.WriteLine(str);
                    }

                    addresses.TryGetValue(domain, out var domainCount);
                    addresses[domain] = domainCount + 1;

                    addressesTopLevel.TryGetValue(domainTopLevel, out var topLevelCount);
                    addressesTopLevel[domainTopLevel] = topLevelCount + 1;
                }
            }

            Console.WriteLine();

            var sorted = from pair in addresses
                         orderby pair.Value descending
                         select pair;

            var sortedTopLevel = from pair in addressesTopLevel
                                 orderby pair.Value descending
                                 select pair;

            foreach (KeyValuePair<string, int> item in sorted)
            {
                Console.WriteLine(item.Key + ": " + item.Value);
            }

            Console.WriteLine('\n');

            foreach (KeyValuePair<string, int> item in sortedTopLevel)
            {
                Console.WriteLine(item.Key + ": " + item.Value);
            }

            Console.WriteLine("done");

            Console.ReadLine();
        }
    }
}
