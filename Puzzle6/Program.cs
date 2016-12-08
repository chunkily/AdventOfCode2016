using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle6
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../input.txt");
            string[] lines = File.ReadAllLines(inputFilePath);

            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            List<Dictionary<char, int>> Frequencies = new List<Dictionary<char, int>>();
            // Prepare a dictionary for each column
            for(int i=0;i<8;i++)
            {
                var frequency = new Dictionary<char,int>();
                foreach(var letter in alphabet)
                {
                    frequency.Add(letter, 0);
                }
                Frequencies.Add(frequency);
            }

            foreach(var line in lines)
            {
                for(int i=0;i<line.Length;i++)
                {
                    var frequency = Frequencies[i];
                    char c = line[i];
                    frequency[c] += 1;
                }
            }

            foreach(var frequency in Frequencies)
            {
                char mostFrequentChar = frequency.OrderByDescending(kvp => kvp.Value).First().Key;
                Console.Write(mostFrequentChar);
            }
            Console.WriteLine();

            foreach(var frequency in Frequencies)
            {
                char leastFrequentChar = frequency.Where(kvp=>kvp.Value>0).OrderBy(kvp => kvp.Value).First().Key;
                Console.Write(leastFrequentChar);
            }

            Console.ReadLine();
        }
    }
}
