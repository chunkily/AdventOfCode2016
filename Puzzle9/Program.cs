using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzle9
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../input.txt");
            string input = File.ReadAllText(inputFilePath);

            #region PART 1
            int i = 0;
            StringBuilder outputSB = new StringBuilder();
            while(i<input.Length)
            {
                char c = input[i];
                if (c == '(')
                {
                    int indexOfClosingBracket = input.IndexOf(')', i);
                    string decompress = input.Substring(i + 1, indexOfClosingBracket - i - 1);

                    Match match = Regex.Match(decompress, @"([0-9]+)x([0-9]+)");
                    int decompressLength = int.Parse(match.Groups[1].Value);
                    int decompressTimes = int.Parse(match.Groups[2].Value);

                    for (int j = 0; j < decompressTimes; j++)
                    {
                        outputSB.Append(input.Substring(indexOfClosingBracket + 1, decompressLength));
                    }

                    // Advance reader index
                    i = indexOfClosingBracket + 1;
                    i += decompressLength;
                }
                else
                {
                    outputSB.Append(c);
                    i++;
                }

            }
            Console.WriteLine("V1 Decompressed length: " +outputSB.Length);
            #endregion PART 1

            #region PART 2
            //Console.WriteLine(GetDecompressedLength("X(8x2)(3x3)ABCY")); // 20
            //Console.WriteLine(GetDecompressedLength("(27x12)(20x12)(13x14)(7x10)(1x12)A")); // 241920
            //Console.WriteLine(GetDecompressedLength("(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN")); // 445
            Console.WriteLine("V2 Decompressed length: " + GetDecompressedLength(input));


            #endregion

            Console.ReadLine();

        }

        static long GetDecompressedLength(string input)
        {
            long size = 0;
            Match match = Regex.Match(input, @"([0-9]+)x([0-9]+)");
            if(match.Success)
            {
                // TIME TO DO SOME RECURSION
                int decompressLength = int.Parse(match.Groups[1].Value);
                int decompressTimes = int.Parse(match.Groups[2].Value);
                string toDecompress = input.Substring(match.Index + match.Length + 1,decompressLength);
                long sizeOfSection = GetDecompressedLength(toDecompress);
                size = size + (sizeOfSection * decompressTimes);

                // Add the length of the sections before the section to decompress
                size = size + match.Index-1;

                // And the length of the sections after the section to decompress
                string remaining = input.Substring(match.Index + match.Length + decompressLength + 1);
                size = size + GetDecompressedLength(remaining);
            }
            else
            {
                size = size + input.Length;
            }
            return size;
        }
    }
}
