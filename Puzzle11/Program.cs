using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle11
{
    class Program
    {
        static void Main(string[] args)
        {
            Facility facility = new Facility()
            {
                ElevatorFloor = 1,
                Items = new List<FacilityItem>() {
                    new FacilityItem(Element.Pm,true, 1),
                    new FacilityItem(Element.Pm,false, 1),
                    new FacilityItem(Element.Co,true, 2),
                    new FacilityItem(Element.Co,false, 3),
                    new FacilityItem(Element.Cm,true, 2),
                    new FacilityItem(Element.Cm,false, 3),
                    new FacilityItem(Element.Ru,true, 2),
                    new FacilityItem(Element.Ru,false, 3),
                    new FacilityItem(Element.Pu,true, 2),
                    new FacilityItem(Element.Pu,false, 3),
                    //new FacilityItem(Element.El,true, 1),
                    //new FacilityItem(Element.El,false, 1),
                    //new FacilityItem(Element.Di,true, 1),
                    //new FacilityItem(Element.Di,false, 1)

                }
            };

            int steps = 1;

            List<Solution> solutions = new List<Solution>();
            solutions.Add(new Solution() {
                Moves = new List<Move>(),
                CurrentState = facility,
            });
            HashSet<Facility> PreviousStates = new HashSet<Facility>(new FacilityEqualityComparer());
            PreviousStates.Add(facility);

            // Perform a breadth first search for the answer.
            while (solutions.Count > 0 && solutions.All(s => !s.Solved))
            {
                List<Solution> nextSet = new List<Solution>();
                foreach (var solution in solutions)
                {
                    var possibleMoves = solution.CurrentState.GetMoves();
                    foreach (var possibleMove in possibleMoves)
                    {
                        // If the next state has been visited before, we can trim off this branch.
                        var nextState = possibleMove.NextState;
                        if (!PreviousStates.Contains(nextState))
                        {
                            PreviousStates.Add(nextState);
                            var moves = new List<Move>(solution.Moves);
                            moves.Add(possibleMove);
                            nextSet.Add(new Solution()
                            {
                                Moves = moves,
                                CurrentState = nextState
                            });
                        }
                    }
                }
                solutions = nextSet;
                Console.WriteLine("Step " + steps);
                Console.WriteLine("Solution Space " + solutions.Count);
                Console.WriteLine("Tracking Previous States " + PreviousStates.Count);
                steps++;
            }

            var shortestSolution = solutions.First(s => s.Solved);

            int step = 1;
            foreach(var move in shortestSolution.Moves)
            {
                Console.WriteLine(step + ")" +move.ToString());
                move.NextState.Print();
                step++;
            }

            Console.ReadLine();
        }
    }

    class Solution
    {
        public List<Move> Moves { get; set; }
        public Facility CurrentState { get; set; }
        public bool Solved
        {
            get
            {
                return CurrentState.Items.All(i => i.Floor == 4);
            }
        }
    }

    class Facility
    {
        // The floor on which the elevator is at
        public int ElevatorFloor { get; set; }
        public List<FacilityItem> Items;

        public Facility()
        {
        }

        public bool IsValid()
        {
            foreach (var chip in Items.Where(i => !i.IsGenerator))
            {
                // Any generators on the same floor that is not the same element AND
                // corresponding generator not on the same floor 
                if (Items.Any(i => i.IsGenerator && i.Floor == chip.Floor && i.Element != chip.Element) &&
                   !Items.Any(i => i.IsGenerator && i.Floor == chip.Floor && i.Element == chip.Element))
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerable<Move> GetMoves()
        {
            List<Move> possibleMoves = new List<Move>();
            int nextFloor;
            var itemsOnSameFloorAsElevator = Items.Where(i => i.Floor == ElevatorFloor);
            // Going Up
            if(ElevatorFloor < 4)
            {
                nextFloor = ElevatorFloor + 1;
                // Bring 1 Item.
                foreach(var item in itemsOnSameFloorAsElevator)
                {
                    var move = new Move();
                    var nextState = this.Clone();
                    nextState.ElevatorFloor = nextFloor;
                    nextState.Items.Single(i=>i.Element == item.Element && i.IsGenerator == item.IsGenerator).Floor = nextFloor;
                    if(nextState.IsValid())
                    {
                        move.Description = "Move up to " + nextFloor + " with " + item.Display();
                        move.NextState = nextState;
                        possibleMoves.Add(move);
                    }
                }

                // Bring 2 Items.
                foreach(var pair in ChooseTwo(itemsOnSameFloorAsElevator))
                {
                    var move = new Move();
                    var nextState = this.Clone();
                    nextState.ElevatorFloor = nextFloor;
                    nextState.Items.Single(i => i.Element == pair.Item1.Element && i.IsGenerator == pair.Item1.IsGenerator).Floor = nextFloor;
                    nextState.Items.Single(i => i.Element == pair.Item2.Element && i.IsGenerator == pair.Item2.IsGenerator).Floor = nextFloor;
                    if (nextState.IsValid())
                    {
                        move.Description = "Move up to " + nextFloor + " with " + pair.Item1.Display() + " and " + pair.Item2.Display();
                        move.NextState = nextState;
                        possibleMoves.Add(move);
                    };
                }
                
            }
            // Going Down
            if(ElevatorFloor > 1)
            {
                nextFloor = ElevatorFloor - 1;
                // Bring 1 Item.
                foreach (var item in itemsOnSameFloorAsElevator)
                {
                    var move = new Move();
                    var nextState = this.Clone();
                    nextState.ElevatorFloor = nextFloor;
                    nextState.Items.Single(i => i.Element == item.Element && i.IsGenerator == item.IsGenerator).Floor = nextFloor;
                    if (nextState.IsValid())
                    {
                        move.Description = "Move down to " + nextFloor + " with " + item.Display();
                        move.NextState = nextState;
                        possibleMoves.Add(move);
                    }
                }

                // Bring 2 Items.
                foreach (var pair in ChooseTwo(itemsOnSameFloorAsElevator))
                {
                    var move = new Move();
                    var nextState = this.Clone();
                    nextState.ElevatorFloor = nextFloor;
                    nextState.Items.Single(i => i.Element == pair.Item1.Element && i.IsGenerator == pair.Item1.IsGenerator).Floor = nextFloor;
                    nextState.Items.Single(i => i.Element == pair.Item2.Element && i.IsGenerator == pair.Item2.IsGenerator).Floor = nextFloor;
                    if (nextState.IsValid())
                    {
                        move.Description = "Move down to " + nextFloor + " with " + pair.Item1.Display() + " and " + pair.Item2.Display();
                        move.NextState = nextState;
                        possibleMoves.Add(move);
                    };
                }
            }

            return possibleMoves;
        }

        private Facility Clone()
        {
            var clone = new Facility() {
                ElevatorFloor = this.ElevatorFloor
            };
            clone.Items = new List<FacilityItem>();
            foreach(var item in Items)
            {
                clone.Items.Add(new FacilityItem(item.Element, item.IsGenerator, item.Floor));
            }
            return clone;
        }

        private IEnumerable<Tuple<T,T>> ChooseTwo<T>(IEnumerable<T> items)
        {
            List<Tuple<T, T>> pairs = new List<Tuple<T, T>>();
            if(items.Count() <=1)
            {
                return pairs;
            }
            else if(items.Count() == 2)
            {
                pairs.Add(new Tuple<T,T>(items.First(), items.Last()));
                return pairs;
            }
            else
            {
                var list = items.ToList();
                for(int i=0;i<items.Count();i++)
                {
                    for(int j=i+1;j<items.Count();j++)
                    {
                        pairs.Add(new Tuple<T, T>(list[i], list[j]));
                    }
                }
                return pairs;
            }
        }

        public bool Equals(Facility other)
        {
            if(this.ElevatorFloor != other.ElevatorFloor)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if(Items[i].Floor != other.Items[i].Floor)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Print()
        {
            for(int floorNum = 4;floorNum>0;floorNum--)
            {
                Console.Write("F" + floorNum);

                if(ElevatorFloor == floorNum)
                {
                    Console.Write(" E ");
                }
                else
                {
                    Console.Write(" . ");
                }
                foreach(var item in Items)
                {
                    if(item.Floor == floorNum)
                    {
                        Console.Write(item.Display() + " ");
                    }
                    else
                    {
                        Console.Write(" .  ");
                    }
                }

                Console.WriteLine();
            }
        }
    }

    class FacilityEqualityComparer : EqualityComparer<Facility>
    {
        public override bool Equals(Facility x, Facility y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode(Facility obj)
        {
            return (obj.ElevatorFloor + 
                string.Join("",(obj.Items.Select(o=>o.ToString())))).GetHashCode();
        }
    }

    class FacilityItem
    {
        public Element Element { get; }
        // If false, is a chip
        public bool IsGenerator { get; }
        public int Floor { get; set; }

        public FacilityItem(Element element, bool IsGenerator, int floorNum)
        {
            this.Element = element;
            this.IsGenerator = IsGenerator;
            this.Floor = floorNum;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}", Element.ToString(), IsGenerator ? "G" : "M", Floor == 0 ? "" : Floor.ToString());
        }

        public string Display()
        {
            return string.Format("{0}{1}", Element.ToString(), IsGenerator ? "G" : "M");
        }

        public bool Equals(FacilityItem other)
        {
            return this.Element == other.Element && this.IsGenerator == other.IsGenerator && this.Floor == other.Floor;
        }
    }

    class Move
    {
        public Facility NextState { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }

    enum Element
    {
        Pm,
        Co,
        Cm,
        Ru,
        Pu,
        El, // Wait a second, this element doesn't exist!
        Di  // This too!
    }
}
