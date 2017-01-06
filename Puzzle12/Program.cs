using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzle12
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../input.txt");
            string[] lines = File.ReadAllLines(inputFilePath);

            int reader = 0;

            int[] register = new int[4];

            // Initialise register c to be 1.
            register[2] = 1;

            // Let's assume the program halts...
            while (reader < lines.Length)
            {
                string instruction = lines[reader];

                var cpy = Regex.Match(instruction, @"^cpy (-?[0-9]+|[abcd]) ([abcd])");
                var inc = Regex.Match(instruction, @"^inc ([abcd])");
                var dec = Regex.Match(instruction, @"^dec ([abcd])");
                var jnz = Regex.Match(instruction, @"^jnz (-?[0-9]+|[abcd]) (-?[0-9]+)");

                if (cpy.Success)
                {
                    int y = getRegisterIndexFromLetter(cpy.Groups[2].Value);

                    if(!int.TryParse(cpy.Groups[1].Value, out register[y]))
                    {
                        int x = getRegisterIndexFromLetter(cpy.Groups[1].Value);
                        register[y] = register[x];
                    }
                    reader++;
                }
                else if(inc.Success)
                {
                    int x = getRegisterIndexFromLetter(inc.Groups[1].Value);
                    register[x]++;
                    reader++;
                }
                else if (dec.Success)
                {
                    int x = getRegisterIndexFromLetter(dec.Groups[1].Value);
                    register[x]--;
                    reader++;
                }
                else if (jnz.Success)
                {
                    int xVal;
                    if(!int.TryParse(jnz.Groups[1].Value, out xVal))
                    {
                        int x = getRegisterIndexFromLetter(jnz.Groups[1].Value);
                        xVal = register[x];
                    }

                    int y = int.Parse(jnz.Groups[2].Value);

                    if(xVal != 0)
                    {
                        // offset the reader by y
                        reader += y;
                    }
                    else
                    {
                        reader++;
                    }
                }
                else
                {
                    throw new InvalidOperationException("Instruction not understood. " + instruction);
                }
            }

            Console.WriteLine("a:" + register[0]);
            Console.WriteLine("b:" + register[1]);
            Console.WriteLine("c:" + register[2]);
            Console.WriteLine("d:" + register[3]);

            Console.ReadLine();
        }

        private static int getRegisterIndexFromLetter(string letter)
        {
            return letter[0] - 97;
        }
    }
}
