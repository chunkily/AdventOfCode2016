using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzle14
{
    class Program
    {
        static void Main(string[] args)
        {
            string salt = "cuanljph";
            //string salt = "abc";
            List<string> keys = new List<string>();
            List<string> next1000 = new List<string>(1000);
            int i;

            for (i = 0; i < 1000; i++)
            {
                string hash = MD5Hash(salt + i);
                hash = StretchKey(hash);
                next1000.Add(hash);
            }

            i = 0;

            while (keys.Count < 64)
            {
                string possibleKey = next1000[0];
                next1000.RemoveAt(0);
                string hash = MD5Hash(salt + (i + 1000));
                hash = StretchKey(hash);
                next1000.Add(hash);
                i++;

                char tripletChar;
                if (HasTriplet(possibleKey, out tripletChar))
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        if (HasQuintuplet(next1000[j], tripletChar))
                        {
                            keys.Add(possibleKey);
                            Console.WriteLine(keys.Count + ":" + possibleKey);
                            j = 1000;
                        }
                    }
                }
            }

            Console.WriteLine(i-1);
            Console.ReadLine();
        }

        static string MD5Hash(string input)
        {
            // Source: Jani Järvinen, https://blogs.msdn.microsoft.com/csharpfaq/2006/10/09/how-do-i-calculate-a-md5-hash-from-a-string/
            // step 1, calculate MD5 hash from input

            MD5 md5 = MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("x2"));

            }

            return sb.ToString();
        }

        static string StretchKey(string key)
        {
            for(int i=0;i<2016;i++)
            {
                key = MD5Hash(key);
            }
            return key;
        }

        static bool HasTriplet(string input, out char tripletChar)
        {
            tripletChar = '-';
            var match = Regex.Match(input, @"([0123456789abcdef])\1{2}");
            if(match.Success)
            {
                tripletChar = match.Value[0];
            }
            return match.Success;
        }

        static bool HasQuintuplet(string input, char tripletChar)
        {
            var match = Regex.Match(input, @"(["+ tripletChar +@"])\1{4}");
            return match.Success;
        }
    }
}
