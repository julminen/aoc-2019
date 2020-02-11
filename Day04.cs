using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc_2019
{
    class Day04
    {
        internal Day04()
        {
        }

        internal void ShowSolution(string[] args)
        {
            int min = 273025;
            int max = 767253;
            if (args.Length >= 4)
            {
                min = int.Parse(args[2]);
                max = int.Parse(args[3]);
            }
            Console.WriteLine("Day 4");
            if (args.Length < 2 || args[1] == "1")
            {
                Console.WriteLine($"  Phase 1: {this.Phase1(min, max)}");
            }
            if (args.Length < 2 || args[1] == "2")
            {
                Console.WriteLine($"  Phase 2: {this.Phase2(min, max)}");
            }
        }

        private bool isGood(int num)
        {
            int[] ns = num.ToString().ToArray().Select(x => (int)x - (int)'0').ToArray();
            return (ns[0] == ns[1] || ns[1] == ns[2] || ns[2] == ns[3] || ns[3] == ns[4] || ns[4] == ns[5])
                && (ns[0] <= ns[1] && ns[1] <= ns[2] && ns[2] <= ns[3] && ns[3] <= ns[4] && ns[4] <= ns[5])
                && ns.Length == 6;
        }

        private bool isGoodStrict(int num)
        {
            int[] ns = num.ToString().ToArray().Select(x => (int)x - (int)'0').ToArray();
            bool increasing = (ns[0] <= ns[1] && ns[1] <= ns[2] && ns[2] <= ns[3] && ns[3] <= ns[4] && ns[4] <= ns[5]);
            bool pairs = 
                                   (ns[0] == ns[1] && ns[1] != ns[2])
              || (ns[0] != ns[1] && ns[1] == ns[2] && ns[2] != ns[3]) 
              || (ns[1] != ns[2] && ns[2] == ns[3] && ns[3] != ns[4])
              || (ns[2] != ns[3] && ns[3] == ns[4] && ns[4] != ns[5])
              || (ns[3] != ns[4] && ns[4] == ns[5]);
            
            return pairs
                && increasing
                && ns.Length == 6;
        }

        private int Phase1(int min, int max)
        {
            int found = 0;
            for (int i = min; i <= max; i++)
            {
                if (isGood(i))
                {
                    found++;
                }
            }
            return found;
        }

        private int Phase2(int min, int max)
        {
            int found = 0;
            for (int i = min; i <= max; i++)
            {
                if (isGoodStrict(i))
                {
                    found++;
                }
            }
            return found;
        }

    }
}