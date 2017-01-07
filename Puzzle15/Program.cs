using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* INPUT
Disc #1 has 13 positions; at time=0, it is at position 1.
Disc #2 has 19 positions; at time=0, it is at position 10.
Disc #3 has 3 positions; at time=0, it is at position 2.
Disc #4 has 7 positions; at time=0, it is at position 1.
Disc #5 has 5 positions; at time=0, it is at position 3.
Disc #6 has 17 positions; at time=0, it is at position 5.

Final disc has 11 positions and starts at position 0
*/

namespace Puzzle15
{
    class Program
    {
        static void Main(string[] args)
        {
            Disc discOne = new Disc(5, 4);
            Disc discTwo = new Disc(2, 1);

            List<Disc> discs = new List<Disc>() {
                //new Disc(5,4),
                //new Disc(2,1)
                new Disc(13,1),
                new Disc(19,10),
                new Disc(3,2),
                new Disc(7,1),
                new Disc(5,3),
                new Disc(17,5),
                new Disc(11,0)
            };

            int time = -1;
            bool solved = false;
            while(!solved)
            {
                time++;
                bool blocked = false;
                for(int i=0;i<discs.Count;i++)
                {
                    if(discs[i].GetPositionAtTime(time + i + 1) != 0)
                    {
                        blocked = true;
                        // break;
                        i = discs.Count;
                    }
                }
                if(!blocked)
                {
                    solved = true;
                }
            }
            Console.WriteLine(time);
            Console.ReadLine();
        }
    }

    class Disc
    {
        int _numPositions;
        int _initialPos;

        public Disc(int numPositions, int initialPos)
        {
            this._numPositions = numPositions;
            this._initialPos = initialPos;
        }

        public int GetPositionAtTime(int time)
        {
            return (_initialPos + time) % _numPositions;

        }

    }
}
