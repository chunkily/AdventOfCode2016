using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle16
{
    class Program
    {
        // *raises hand* ... Can i work with strings instead of bits?
        static void Main(string[] args)
        {
            string data = "11110010111001001";
            int toFill = 272;
            toFill = 35651584;
            //data = "10000";
            //toFill = 20;
            while (data.Length < toFill)
            {
                data = DragonCurve(data);
            }
            data = data.Substring(0, toFill);
            Console.WriteLine(GetChecksum(data));
            Console.ReadLine();
        }

        static string ReverseAndInvert(string input)
        {
            // Reverse
            char[] reversed = input.Reverse().ToArray();
            input = new string(reversed);
            // Invert
            input = input.Replace("1", "-");
            input = input.Replace("0", "1");
            input = input.Replace("-", "0");
            return input;
        }

        static string DragonCurve(string a)
        {
            string b = ReverseAndInvert(a);
            return a + "0" + b;
        }

        static string GetChecksum(string input)
        {
            var checksum = new StringBuilder();
            for(int i=0;i<input.Length;i+=2)
            {
                string pair = input.Substring(i, 2);
                if(pair[0] == pair[1])
                {
                    checksum.Append("1");

                }
                else
                {
                    checksum.Append("0");
                }
            }
            if(checksum.Length % 2 == 0)
            {
                // Even checksum, Get the checksum again.
                return GetChecksum(checksum.ToString());
            }
            else
            {
                // Odd checksum, we are done!
                return checksum.ToString();
            }
        }
    }
}
