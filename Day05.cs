using System;
using System.Linq;

namespace aoc_2019
{
    class Day05
    {

        private readonly long[] program;
        internal Day05(string inputFileName)
        {
            this.program = System.IO.File.ReadAllText(inputFileName).Split(',').Select(x => long.Parse(x)).ToArray();
        }

        internal void ShowSolution(string[] args) {
            Console.WriteLine("Day 5");
            if (args.Length < 2 || args[1] == "1") {
                Console.WriteLine($"Phase 1: {this.Phase1()}");
            }
            if (args.Length < 2 || args[1] == "2") {
                Console.WriteLine($"Phase 2: {this.Phase2()}");
            }
        }

        private long Phase1()
        {
            Intcode computer = new Intcode();
            computer.load(program);
            computer.run(new long[]{1});
            while(computer.output.Count() > 1)
            {
                long o = computer.output.Take();
                if (o != 0) {
                    Console.WriteLine($"Bad output: {o}");
                }
            }

            return computer.output.Take();
        }

        private long Phase2() {
            Intcode computer = new Intcode();
            computer.load(program);
            computer.run(new long[]{5});

            return computer.output.Take();
        }
    }
}
