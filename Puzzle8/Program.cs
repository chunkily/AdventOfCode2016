using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzle8
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../input.txt");
            string[] lines = File.ReadAllLines(inputFilePath);

            //Screen screen = new Screen(7, 3);
            //screen.Rectangle(3, 2);
            //screen.Display();
            //Console.WriteLine();
            //screen.RotateX(1,1);
            //screen.Display();
            //Console.WriteLine();
            //screen.RotateY(0,4);
            //screen.Display();
            //Console.WriteLine();
            //screen.RotateX(1,1);
            //screen.Display();
            //Console.WriteLine();

            //Screen screen = new Screen(50, 8);
            Screen screen = new Screen(50, 6);
            foreach (string instruction in lines)
            {
                var rect = Regex.Match(instruction, @"^rect ([0-9]+)x([0-9]+)");
                var rotateX = Regex.Match(instruction, @"^rotate column x=([0-9]+) by ([0-9]+)");
                var rotateY = Regex.Match(instruction, @"^rotate row y=([0-9]+) by ([0-9]+)");
                if (rect.Success)
                {
                    int width = int.Parse(rect.Groups[1].Value);
                    int height = int.Parse(rect.Groups[2].Value);
                    screen.Rectangle(width, height);
                }
                else if (rotateX.Success)
                {
                    int col = int.Parse(rotateX.Groups[1].Value);
                    int val = int.Parse(rotateX.Groups[2].Value);
                    screen.RotateX(col, val);

                }
                else if (rotateY.Success)
                {
                    int row = int.Parse(rotateY.Groups[1].Value);
                    int val = int.Parse(rotateY.Groups[2].Value);
                    screen.RotateY(row, val);
                }
                else
                {
                    throw new Exception("Unknown instruction!");
                }
                // DEBUG
                //Console.WriteLine(instruction);
                //Console.WriteLine("00000000001111111111222222222233333333334444444444");
                //Console.WriteLine("01234567890123456789012345678901234567890123456789");
                //screen.Display();
            }
            int count = screen.Display();
            Console.WriteLine("Pixels lit: " + count);

            Console.ReadLine();
        }

    }

    class Screen
    {
        int width;
        int height;

        public bool[,] Pixels { get; set; }

        public Screen(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.Pixels = new bool[width,height];
        }

        public int Display()
        {
            int count = 0;
            for(int row=0;row<height;row++)
            {
                for(int col=0;col<width;col++)
                {
                    if(Pixels[col,row])
                    {
                        Console.Write('#');
                        count++;
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
            return count;
        }

        public void Rectangle(int width, int height)
        {
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    Pixels[y, x] = true;
                }
            }
        }

        private void RotateX(int column)
        {
            bool last = Pixels[column, height-1];
            for (int row = height-1; row > 0; row--)
            {
                Pixels[column, row] = Pixels[column, row - 1];
            }
            Pixels[column, 0] = last;
        }

        public void RotateX(int column, int value)
        {
            for(int i=0;i<value;i++)
            {
                RotateX(column);
            }
        }

        private void RotateY(int row)
        {
            bool last = Pixels[width-1, row];
            for (int column = width-1; column > 0; column--)
            {
                Pixels[column, row] = Pixels[column -1, row];
            }
            Pixels[0, row] = last;
        }

        public void RotateY(int row, int value)
        {
            for (int i = 0; i < value; i++)
            {
                RotateY(row);
            }
        }
    }
}
