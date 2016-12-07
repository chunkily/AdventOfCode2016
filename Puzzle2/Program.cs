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

            //Dictionary<char, KeyPadNumber> KeyPad = new Dictionary<char, KeyPadNumber>();
            ////                                    UP  DOWN  LEFT RIGHT
            //KeyPad.Add('1', new KeyPadNumber('1', '1', '4', '1', '2'));
            //KeyPad.Add('2', new KeyPadNumber('2', '2', '5', '1', '3'));
            //KeyPad.Add('3', new KeyPadNumber('3', '3', '6', '2', '3'));
            //KeyPad.Add('4', new KeyPadNumber('4', '1', '7', '4', '5'));
            //KeyPad.Add('5', new KeyPadNumber('5', '2', '8', '4', '6'));
            //KeyPad.Add('6', new KeyPadNumber('6', '3', '9', '5', '6'));
            //KeyPad.Add('7', new KeyPadNumber('7', '4', '7', '7', '8'));
            //KeyPad.Add('8', new KeyPadNumber('8', '5', '8', '7', '9'));
            //KeyPad.Add('9', new KeyPadNumber('9', '6', '9', '8', '9'));

            Dictionary<char, KeyPadNumber> KeyPad = new Dictionary<char, KeyPadNumber>();
            //                                    UP  DOWN  LEFT RIGHT
            KeyPad.Add('1', new KeyPadNumber('1', '1', '3', '1', '1'));
            KeyPad.Add('2', new KeyPadNumber('2', '2', '6', '2', '3'));
            KeyPad.Add('3', new KeyPadNumber('3', '1', '7', '2', '4'));
            KeyPad.Add('4', new KeyPadNumber('4', '4', '8', '3', '4'));
            KeyPad.Add('5', new KeyPadNumber('5', '5', '5', '5', '6'));
            KeyPad.Add('6', new KeyPadNumber('6', '2', 'A', '5', '7'));
            KeyPad.Add('7', new KeyPadNumber('7', '3', 'B', '6', '8'));
            KeyPad.Add('8', new KeyPadNumber('8', '4', 'C', '7', '9'));
            KeyPad.Add('9', new KeyPadNumber('9', '9', '9', '8', '9'));
            KeyPad.Add('A', new KeyPadNumber('A', '6', 'A', 'A', 'B'));
            KeyPad.Add('B', new KeyPadNumber('B', '7', 'D', 'A', 'C'));
            KeyPad.Add('C', new KeyPadNumber('C', '8', 'C', 'B', 'C'));
            KeyPad.Add('D', new KeyPadNumber('D', 'B', 'D', 'D', 'D'));

            KeyPadNumber currentNumber = KeyPad['5'];

            foreach(var line in lines)
            {
                foreach(char c in line)
                {
                    // Read the character and change the current position.
                    if(c=='U')
                    {
                        currentNumber = KeyPad[currentNumber.Up];
                    }
                    else if(c=='D')
                    {
                        currentNumber = KeyPad[currentNumber.Down];
                    }
                    else if(c=='R')
                    {
                        currentNumber = KeyPad[currentNumber.Right];
                    }
                    else if(c=='L')
                    {
                        currentNumber = KeyPad[currentNumber.Left];
                    }
                    // DEBUG
                    // Console.WriteLine(c + " " + currentNumber.DisplayNumber);

                }
                // Reached the end of a line, output the number on the keypad.
                Console.Write(currentNumber.DisplayNumber);
            }

            Console.ReadLine();
        }
    }

    class KeyPadNumber
    {
        public char DisplayNumber { get; set; }
        public char Up { get; set; }
        public char Down { get; set; }
        public char Left { get; set; }
        public char Right { get; set; }

        public KeyPadNumber(char thisDisplay, char up, char down, char left, char right)
        {
            this.DisplayNumber = thisDisplay;
            this.Up = up;
            this.Down = down;
            this.Left = left;
            this.Right = right;
        }
    }
}

