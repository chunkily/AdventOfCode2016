using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle13
{
    class Program
    {
        static void Main(string[] args)
        {
            int input = 1352; // designersNumber
            Maze maze = new Maze(input);

            // Using Astar pathfinding.
            Node targetNode = new Node(31, 39);
            // The open list should initially only contain our start position.
            List<Node> OpenList = new List<Node>() {
                new Node(1,1)
            };
            List<Node> ClosedList = new List<Node>();

            NodeComparer nodeComparer = new NodeComparer();

            bool solved = false;
            while (!solved && OpenList.Count > 0)
            {
                // First ensure the list is sorted.
                OpenList = OpenList.OrderBy(a => a.FValue).ToList();
                // Take the first node (with the lowest F value) from the open list and add it to the closed list.
                var currentNode = OpenList.First();
                OpenList.RemoveAt(0);
                ClosedList.Add(currentNode);
                foreach(var neighbour in currentNode.GetNeighbours())
                {
                    if(nodeComparer.Equals(neighbour, targetNode))
                    {
                        // We are done!
                        targetNode.Parent = currentNode;
                        solved = true;
                    }
                    else if(maze.IsLocationOpenSpace(neighbour.X, neighbour.Y))
                    {
                        // For each neighbour of that node that is not impassable,
                        int cIndex = ClosedList.FindIndex(o => o.X == neighbour.X && o.Y == neighbour.Y);
                        if (cIndex >= 0)
                        {
                            // if neighbour is in Closed we should be able to safely ignore it*.
                            continue;
                        }
                        // Calculate it's tentative GValue
                        int gValue = currentNode.GValue + Node.GetMovementCostBetween(currentNode, neighbour);
                        
                        if(gValue > 50)
                        {
                            // Max movement.
                            //continue;
                        }

                        int oIndex = OpenList.FindIndex(o => o.X == neighbour.X && o.Y == neighbour.Y);
                        if(oIndex >= 0)
                        {
                            // If neighbour is in Open and the new cost is less than that's cost
                            // Remove that node from the open list and use this one instead.
                            var openNode = OpenList[oIndex];
                            if(gValue < openNode.GValue)
                            {
                                OpenList.RemoveAt(oIndex);
                                neighbour.HValue = openNode.HValue;
                                neighbour.Parent = currentNode;
                                OpenList.Add(neighbour);
                            }
                        }
                        else
                        {
                            // If neighbour is in neither list, 
                            // Set it's GValue to the tentative GValue
                            neighbour.GValue = gValue;
                            // Calculate and set it's HValue.
                            neighbour.HValue = Node.GetHeuristicValueOf(neighbour, targetNode);
                            // Set it's parent
                            neighbour.Parent = currentNode;
                            // Add it to the open list.
                            OpenList.Add(neighbour);
                        }
                    }
                }
            }

            List<Node> highlightedNodes = new List<Node>();
            if(!solved)
            {
                highlightedNodes = ClosedList;
                Console.WriteLine("No Path found! Displaying " + highlightedNodes.Count + " possible Locations");
            }
            else
            {
                // Reconstruct the path from target back to start by following the parent nodes.
                Node nextNode = targetNode;
                while(nextNode.Parent != null)
                {
                    highlightedNodes.Add(nextNode);
                    nextNode = nextNode.Parent;
                }

                Console.WriteLine("Solution found! in " + highlightedNodes.Count + " steps.");
            }

            int maxI = highlightedNodes.Max(n => n.X) + 2;
            int maxJ = highlightedNodes.Max(n => n.Y) + 2;
            // Draw out the map.
            for (int i=0;i<maxI;i++)
            {
                for(int j=0;j<maxJ;j++)
                {
                    if(maze.IsLocationOpenSpace(i,j))
                    {
                        if(highlightedNodes.Any(n=>n.X == i && n.Y == j))
                        {
                            Console.Write("O");
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                    else
                    {
                        Console.Write("#");
                    }

                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }

    class Node 
    {
        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public int HValue { get; set; } // Also known as the Heuristic Value
        public int GValue { get; set; } // Also known as the Cost Value
        public int FValue { get { return HValue + GValue; } }

        public Node Parent { get; set; }

        public IEnumerable<Node> GetNeighbours()
        {
            List<Node> neighbours = new List<Node>();
            neighbours.Add(new Node(X + 1, Y));
            neighbours.Add(new Node(X - 1, Y));
            neighbours.Add(new Node(X, Y + 1));
            neighbours.Add(new Node(X, Y - 1));

            return neighbours;
        }

        public static int GetHeuristicValueOf(Node node, Node target)
        {
            // Using the manhattan heuristic value.
            int dx = Math.Abs(node.X - target.X);
            int dy = Math.Abs(node.Y - target.Y);
            int D = 1; // D is the distance between two nodes.
            return D * (dx + dy);
        }

        public static int GetMovementCostBetween(Node node, Node neighbour)
        {
            // This should find the cost of moving between two nodes.
            // Here we use a static value between adjacent nodes.
            return 1;
        }
    }

    class NodeComparer : IEqualityComparer<Node>
    {
        public bool Equals(Node node1, Node node2)
        {
            return node1.X == node2.X && node1.Y == node2.Y;
        }

        public int GetHashCode(Node obj)
        {
            return (obj.X.ToString() + obj.Y.ToString()).GetHashCode();
        }
    }

    class Maze
    {
        int _designersNumber;

        public Maze(int designersNumber)
        {
            _designersNumber = designersNumber;
        }

        public bool IsLocationOpenSpace(int x, int y)
        {
            if(x < 0 || y < 0)
            {
                return false;
            }

            // Juuuust in case, lets handle integer overflow.
            int sum = checked(x * x + 3 * x + 2 * x * y + y + y * y + _designersNumber);

            int bitCount = 0;
            while(sum > 0)
            {
                bitCount += sum % 2;
                sum /= 2;
            }

            return bitCount % 2 == 0;
        }
    }
}
