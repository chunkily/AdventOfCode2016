using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Puzzle5
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "cxdnnyjw";
            //string input = "abc";
            string hash = "";
            string answerFirst = "";
            List<char> answerSecond = new List<char>(8) {
                '-','-','-','-','-','-','-','-'
            };

            int count = 0;
            int i = 0;
            while(count < 8 || answerSecond.Contains('-'))
            {
                hash = MD5Hash(input + i);
                if(hash.Substring(0,5) == "00000")
                {
                    count++;
                    Console.WriteLine(i + " " + hash);
                    char sixthChar = hash.ElementAt(5);
                    if(count < 8)
                    {
                        answerFirst += sixthChar;
                    }
                    if(char.IsDigit(sixthChar))
                    {
                        int position = int.Parse(sixthChar.ToString());
                        if(position < 8 && answerSecond[position] == '-')
                        {
                            char seventhChar = hash.ElementAt(6);
                            Console.WriteLine("Placing {0} in position {1}", seventhChar, position);
                            answerSecond[position] = seventhChar;
                            Console.WriteLine("Answer Second: " + new string(answerSecond.ToArray()));
                        }
                    }
                }

                i++;
            }
            Console.WriteLine("Answer First : " + answerFirst);
            Console.ReadLine();
        }

        static string MD5Hash(string input)
        {
            // Source: Jani Järvinen, https://blogs.msdn.microsoft.com/csharpfaq/2006/10/09/how-do-i-calculate-a-md5-hash-from-a-string/
            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("X2"));

            }

            return sb.ToString();
        }
    }
}
