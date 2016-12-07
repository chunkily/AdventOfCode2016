using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzle4
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../input.txt");
            string[] inputs = File.ReadAllLines(inputFilePath);

            List<EncryptedRoom> encryptedRooms = new List<EncryptedRoom>();
            foreach (var input in inputs)
            {
                var encryptedRoom = new EncryptedRoom(input);
                encryptedRooms.Add(encryptedRoom);
                // DEBUG
                // Console.Write(input);
                // Console.WriteLine(encryptedRoom.EncryptedName);
                // Console.Write(encryptedRoom.Checksum);
                // Console.Write(encryptedRoom.MostCommon);
                // Console.Write(encryptedRoom.SectorId);
                // Console.WriteLine(encryptedRoom.GetDecryptedName());
                // Console.WriteLine();
            }
            // Remove invalid data
            encryptedRooms = encryptedRooms.Where(er=>er.Checksum == er.MostCommon).ToList();

            Console.WriteLine("Sum of SectorIds: " + encryptedRooms.Sum(er=>er.SectorId));

            foreach(var room in encryptedRooms.OrderBy(r=>r.DecryptedName))
            {
                // Look for northpole-object-storage
                Console.Write(room.DecryptedName);
                Console.WriteLine(room.SectorId);
            }

            Console.ReadLine();
        }
    }

    class EncryptedRoom
    {
        public string Checksum { get;set; } 
        public int SectorId { get;set; }
        public string EncryptedName { get; set; }
        public string DecryptedName { get; set; }
        public string MostCommon { get;set; }

        public EncryptedRoom(string s)
        {
            Stack<string> stringList = new Stack<string>(s.Split('-').ToList());

            string sectorIdAndCheckSum = stringList.Pop();
            Checksum = sectorIdAndCheckSum.Substring(sectorIdAndCheckSum.IndexOf('[')+1,5);
            SectorId = int.Parse(sectorIdAndCheckSum.Substring(0, 3));
            EncryptedName = s.Substring(0,s.LastIndexOf('-'));

            IEnumerable<char> remainingChars = stringList.SelectMany(c=>c);

            IEnumerable<char> _mostCommon = null;
            var frequency = new Dictionary<char, int>();
            foreach(char c in remainingChars.Distinct())
            {
                frequency[c] = remainingChars.Count(x => x == c);
            }
            _mostCommon = frequency
                .OrderByDescending(kvp => kvp.Value)
                .ThenBy(kvp=>kvp.Key)
                .Take(5)
                .Select(kvp => kvp.Key);

            MostCommon = new string(_mostCommon.ToArray());

            this.DecryptedName = GetDecryptedName();
        }

        public string GetDecryptedName()
        {
            StringBuilder answerStringBuilder = new StringBuilder();
            string chars = "abcdefghijklmnopqrstuvwxyz";

            foreach(char c in EncryptedName)
            {
                if(c == '-')
                {
                    answerStringBuilder.Append('-');
                }
                else
                {
                    int cIndex = chars.IndexOf(c);
                    cIndex = cIndex + SectorId % 26;
                    if(cIndex > 25)
                    {
                        cIndex -= 26;
                    }
                    char decryptedChar = chars.ElementAt(cIndex);
                    answerStringBuilder.Append(decryptedChar);
                }
            }

            return answerStringBuilder.ToString();
        }
    }
}
