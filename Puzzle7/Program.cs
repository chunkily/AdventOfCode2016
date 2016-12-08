using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzle7
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../input.txt");
            string[] lines = File.ReadAllLines(inputFilePath);

            List<IPV7Address> ipAddresses = new List<IPV7Address>();
            foreach(var line in lines)
            {
                var ipAddress = new IPV7Address(line);
                // DEBUG
                //Console.Write(ipAddress.Address + " ");
                //Console.Write(ipAddress.HasAbba);
                //Console.Write(ipAddress.HasAbbaWithinSquareBrackets);
                //Console.WriteLine();
                ipAddresses.Add(ipAddress);
            }


            Console.WriteLine("Number of TLS addresses: {0}", ipAddresses.Count(i=>i.SupportsTLS));
            Console.WriteLine("Number of SSL addresses: {0}", ipAddresses.Count(i=>i.SupportsSSL));
            Console.ReadLine();
        }
    }

    class IPV7Address
    {
        public bool SupportsTLS
        {
            get
            {
                // Walk through the address and look for ABBA
                bool hasAbba = false;
                bool hasAbbaWithinSquareBrackets = false;
                bool withinSquareBrackets = false;
                for (int i = 0; i < Address.Length - 3; i++)
                {
                    // If the square brackets contain exactly 4 letters I might be screwed. 
                    // Thankfully it doesn't look like any of them do.
                    if (Address[i] == '[')
                    {
                        withinSquareBrackets = true;
                    }
                    if (Address[i + 3] == ']')
                    {
                        withinSquareBrackets = false;
                    }

                    if (Address[i] == Address[i + 3] && Address[i + 1] == Address[i + 2] && Address[i] != Address[i + 1])
                    {
                        hasAbba = true;
                        if (withinSquareBrackets)
                        {
                            hasAbbaWithinSquareBrackets = true;
                        }
                    }
                }
                return hasAbba && !hasAbbaWithinSquareBrackets;
            }
        }
        public bool SupportsSSL {
            get
            {
                return CheckSSL();
            }
        }

        public string Address { get; set; }

        public IPV7Address(string s)
        {
            this.Address = s;
        }

        private bool CheckSSL()
        {
            List<string> hypernetSequences = new List<string>();
            var matches = Regex.Matches(Address, @"\[.+?\]");
            foreach(Match match in matches)
            {
                hypernetSequences.Add(match.Value);                
            }

            bool withinSquareBrackets = false;
            for (int i = 0; i < Address.Length - 2; i++)
            {
                if (Address[i] == '[')
                {
                    withinSquareBrackets = true;
                }
                if (Address[i + 2] == ']')
                {
                    withinSquareBrackets = false;
                }

                if (!withinSquareBrackets && Address[i] == Address[i + 2] && Address[i] != Address[i + 1])
                {
                    string BAB = "" + Address[i + 1] + Address[i] + Address[i + 1];
                    if(hypernetSequences.Any(s=>s.Contains(BAB)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
