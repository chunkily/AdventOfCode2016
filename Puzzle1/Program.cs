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
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../input.txt");
            string inputs = File.ReadAllText(inputFilePath);

            Location currentLocation = new Location();
            Location EasterHQ = null;
            CompassDirection currentDirection = CompassDirection.North;

            List<Location> previouslyVisitedLocations = new List<Location>(); ;

            foreach (var input in inputs.Split(','))
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

                for (int i = 0; i < distance; i++)
                {
                    followDirections(currentDirection, 1, currentLocation);
                    if (EasterHQ == null && previouslyVisitedLocations.Contains(currentLocation))
                    {
                        Console.WriteLine(string.Format("We've been here before! ({0},{1})", currentLocation.X, currentLocation.Y));
                        EasterHQ = new Location()
                        {
                            X = currentLocation.X,
                            Y = currentLocation.Y
                        };
                    }
                    previouslyVisitedLocations.Add(new Location()
                    {
                        X = currentLocation.X,
                        Y = currentLocation.Y
                    });
                }
            }

            Console.WriteLine("Final Location: " + currentLocation.ToString());
            Console.WriteLine(string.Format("Shortest Distance:{0}", Math.Abs(currentLocation.X) + Math.Abs(currentLocation.Y)));

            Console.WriteLine("Easter Location: " + EasterHQ.ToString());
            Console.WriteLine(string.Format("Shortest Distance:{0}", Math.Abs(EasterHQ.X) + Math.Abs(EasterHQ.Y)));
            Console.ReadLine();

        }

        private static CompassDirection turn(bool turnLeft, CompassDirection currentDirection)
        {

            if (currentDirection == CompassDirection.North)
            {
                if (turnLeft)
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
                if (turnLeft)
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
                if (turnLeft)
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
                if (turnLeft)
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
            if (direction == CompassDirection.North)
            {
                currentLocation.Y += distance;
            }
            else if (direction == CompassDirection.South)
            {
                currentLocation.Y -= distance;
            }
            else if (direction == CompassDirection.West)
            {
                currentLocation.X += distance;
            }
            else if (direction == CompassDirection.East)
            {
                currentLocation.X -= distance;
            }
            else
            {
                throw new Exception("Unknown direction");
            }
        }
    }

    internal class Location : IEquatable<Location>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Location()
        {
            this.X = 0;
            this.Y = 0;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", this.X, this.Y);
        }

        public bool Equals(Location other)
        {
            return other.X == this.X && other.Y == this.Y;
        }
    }

    internal enum CompassDirection
    {
        North,
        East,
        South,
        West
    }
}
