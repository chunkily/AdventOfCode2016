using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle3
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../input.txt");
            string[] lines = File.ReadAllLines(inputFilePath);

            List<Triangle> triangles = new List<Triangle>();
            for(int i=0;i<lines.Length;i+=3)
            {
                string[] numbersText1 = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string[] numbersText2 = lines[i+1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string[] numbersText3 = lines[i+2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                triangles.Add(new Triangle(numbersText1[0] +" "+ numbersText2[0] +" "+ numbersText3[0]));
                triangles.Add(new Triangle(numbersText1[1] +" "+ numbersText2[1] +" "+ numbersText3[1]));
                triangles.Add(new Triangle(numbersText1[2] +" "+ numbersText2[2] +" "+ numbersText3[2]));
            }

            //foreach (var line in lines)
            //{
            //    triangles.Add(new Triangle(line));
            //}

            Console.WriteLine(triangles.Count(t=>t.IsValid));

            Console.ReadLine();
        }

    }

    public class Triangle
    {
        int shortSide;
        int middleSide;
        int longSide;

        public Triangle(string s)
        {
            string[] numbersText = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int[] numbers = new int[] {
                int.Parse(numbersText[0]),
                int.Parse(numbersText[1]),
                int.Parse(numbersText[2])
            };

            var sortedNumbers = numbers.OrderBy(x => x).ToList();
            shortSide = sortedNumbers[0];
            middleSide = sortedNumbers[1];
            longSide = sortedNumbers[2];
        }

        public bool IsValid
        {
            get
            {
                return shortSide + middleSide > longSide;
            }
        }

    }
}
