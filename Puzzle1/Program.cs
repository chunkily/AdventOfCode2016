using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle1
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"../../input.txt");
            string inputs = File.ReadAllText(inputFilePath);

            Location currentLocation = new Location();
            CompassDirection currentDirection = CompassDirection.North;

            foreach(var input in inputs.Split(','))
            {
                string instruction = input.Trim();
                char leftOrRight = instruction[0];
                int distance = int.Parse(instruction.Substring(1));

                if (leftOrRight == 'L')
                {
                    currentDirection = turn(true, currentDirection);
                }
                else if (leftOrRight == 'R')
                {
                    currentDirection = turn(false, currentDirection);
                }
                else
                {
                    throw new Exception("leftOrRight invalid.");
                }

                followDirections(currentDirection, distance, currentLocation);
            }

            Console.WriteLine(string.Format("x:{0} y:{1}",currentLocation.X, currentLocation.Y));
            Console.WriteLine(string.Format("Shortest Distance:{0}", Math.Abs(currentLocation.X) + Math.Abs(currentLocation.Y)));
            Console.ReadLine();
            //if(leftOrRight == "L")
            //{
            //}
            //else if(leftOrRight == "R")
            //{
            //}
            //else
            //{
            //    throw new ArgumentException("leftOrRight invalid.");
            //}
        }

        private static CompassDirection turn(bool turnLeft, CompassDirection currentDirection)
        {

            if (currentDirection == CompassDirection.North)
            {
                if(turnLeft)
                {
                    return CompassDirection.West;
                }
                else
                {
                    return CompassDirection.East;
                }
            }
            else if (currentDirection == CompassDirection.South)
            {
                if(turnLeft)
                {
                    return CompassDirection.East;
                }
                else
                {
                    return CompassDirection.West;
                }
            }
            else if (currentDirection == CompassDirection.West)
            {
                if(turnLeft)
                {
                    return CompassDirection.South;
                }
                else
                {
                    return CompassDirection.North;
                }
            }
            else if (currentDirection == CompassDirection.East)
            {
                if(turnLeft)
                {
                    return CompassDirection.North;
                }
                else
                {
                    return CompassDirection.South;
                }
            }
            else
            {
                throw new Exception("Unknown direction");
            }
        }

        private static void followDirections(CompassDirection direction, int distance, Location currentLocation)
        {
            if(direction == CompassDirection.North)
            {
                currentLocation.Y += distance; 
            }
            else if(direction == CompassDirection.South)
            {
                currentLocation.Y -= distance;
            }
            else if(direction == CompassDirection.West)
            {
                currentLocation.X += distance;
            }
            else if(direction == CompassDirection.East)
            {
                currentLocation.X -= distance;
            }else
            {
                throw new Exception("Unknown direction");
            }
        }
    }

    internal class Location
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Location()
        {
            this.X = 0;
            this.Y = 0;
        }
    }

    internal enum CompassDirection
    {
        North,
        East,
        South,
        West,
    }
}
