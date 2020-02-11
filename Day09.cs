using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc_2019
{
    class Day09
    {
        private readonly long[] program;

        internal Day09(string inputFileName)
        {
            this.program = System.IO.File.ReadAllText(inputFileName).Split(',').Select(x => long.Parse(x)).ToArray();
        }

        internal void ShowSolution(string[] args)
        {
            Console.WriteLine("Day 9");
            if (args.Length < 2 || args[1] == "1")
            {
                Console.WriteLine($"Phase 1: {this.Phase1()}");
            }
            if (args.Length < 2 || args[1] == "2")
            {
                Console.WriteLine($"Phase 2: {this.Phase2()}");
            }
        }

        /*
        private void Test()
        {
            long[] quine = new long[] {109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99};
            long[] bigNums = new long[] {1102,34915192,34915192,7,4,7,99,0};
            long[] outputThis = new long[] {104,1125899906842624,99};

            Intcode computer = new Intcode();
            computer.load(quine);
            computer.run();

            Console.WriteLine($"{String.Join(",", quine)} produces:");
            Console.WriteLine($"{String.Join(",", computer.output)}");
            Console.WriteLine();
            computer.clearOutput();

            computer.load(bigNums);
            computer.run();

            Console.WriteLine($"{String.Join(",", bigNums)} produces:");
            Console.WriteLine($"{String.Join(",", computer.output)}");
            Console.WriteLine();
            computer.clearOutput();

            computer.load(outputThis);
            computer.run();

            Console.WriteLine($"{String.Join(",", outputThis)} produces:");
            Console.WriteLine($"{String.Join(",", computer.output)}");
            Console.WriteLine();
            computer.clearOutput();

        }
        */
        private long Phase1()
        {
            Intcode computer = new Intcode();
            computer.load(program);
            computer.run(new long[]{1});

            return computer.output.Take();
        }

        private long Phase2()
        {
            Intcode computer = new Intcode();
            computer.load(program);
            computer.run(new long[]{2});

            return computer.output.Take();
        }
    }
}