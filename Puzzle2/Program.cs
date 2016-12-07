using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle2
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../input.txt");
            string[] lines = File.ReadAllLines(inputFilePath);

            int xPos = 1;
            int yPos = 1;
            int keyPadHeight = 3;
            int keyPadWidth = 3;
            KeyPad keyPad = new KeyPad(keyPadWidth,keyPadHeight);

            foreach(var line in lines)
            {
                foreach(char c in line)
                {
                    // Read the character and change the current position.
                    if(c=='U')
                    {
                        yPos += 1;
                    }
                    else if(c=='D')
                    {
                        yPos -= 1;
                    }
                    else if(c=='R')
                    {
                        xPos += 1;
                    }
                    else if(c=='L')
                    {
                        xPos -= 1;
                    }

                    // Reset if out of bounds.
                    if(xPos < 0)
                    {
                        xPos = 0;
                    }
                    if(xPos >= keyPadWidth)
                    {
                        xPos = keyPadWidth - 1;
                    }
                    if(yPos < 0)
                    {
                        yPos = 0;
                    }
                    if(yPos >= keyPadHeight)
                    {
                        yPos = keyPadHeight - 1;
                    }
                    // DEBUG
                    //Console.WriteLine(c + " " + xPos + " " + yPos + " " + keyPad.GetDisplayNumber(xPos, yPos));

                }
                // Reached the end of a line, Get the number on the keypad
                Console.WriteLine(keyPad.GetDisplayNumber(xPos,yPos));
            }

            Console.ReadLine();
        }
    }

    class KeyPad
    {
        int keyPadWidth = 3;
        int keyPadHeight = 3;

        public int GetDisplayNumber(int xPos, int yPos)
        {
            return (keyPadHeight - yPos - 1) * keyPadWidth + xPos + 1;
        }

        public KeyPad(int width, int height)
        {
            this.keyPadWidth = width;
            this.keyPadHeight = height;
        }
    }
}

