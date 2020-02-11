using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace aoc_2019
{
    class Day07
    {

        private readonly long[] program;
        internal Day07(string inputFileName)
        {
            this.program = System.IO.File.ReadAllText(inputFileName).Split(',').Select(x => long.Parse(x)).ToArray();
        }

        internal void ShowSolution(string[] args)
        {
            Console.WriteLine("Day 7");
            if (args.Length < 2 || args[1] == "1")
            {
                Console.WriteLine($"Phase 1: {this.Phase1()}");
            }
            if (args.Length < 2 || args[1] == "2")
            {
                Console.WriteLine($"Phase 2: {this.Phase2()}");
            }
        }

        bool nextPermutation(ref int[] a)
        {
            // find largest index k such that a[k] < a[k + 1]. If no such index exists, the permutation is the last permutation.
            int k = -1;
            for (int i = a.Length - 2; i >= 0; i--)
            {
                if (a[i] < a[i + 1])
                {
                    k = i;
                    break;
                }
            }
            if (k == -1) return false;
            // Find the largest index l greater than k such that a[k] < a[l].
            int l;
            for (l = a.Length - 1; l > k; l--)
            {
                if (a[k] < a[l]) break;
            }
            // Swap the value of a[k] with that of a[l].
            int x = a[k]; 
            a[k] = a[l]; 
            a[l] = x;
            // reverse the sequence from a[k + 1] up to and including the final element a[n].
            Array.Reverse(a, k+1, a.Length-(k+1));
            return true;
        }

        private long Phase1()
        {
            long max = 0;
            int[] phases = new int[] { 0, 1, 2, 3, 4 };
            do {
                long input = 0;
                for (int amp = 0; amp < 5; amp++)
                {
                    Intcode computer = new Intcode();
                    computer.load(program);
                    computer.run(new long[] { phases[amp], input });
                    input = computer.output.Take();
                }
                if (input > max) {
                    max = input;
                }
            } while (nextPermutation(ref phases));

            return max;
        }

        private long Phase2()
        {
            long max = 0;
            int[] phases = new int[] {5, 6, 7, 8, 9};
            do {
                Intcode ampA = new Intcode();
                ampA.load(program);
                ampA.input.Add(phases[0]);
                ampA.input.Add(0);

                Intcode ampB = new Intcode(program, ampA.output);
                ampB.input.Add(phases[1]);
                
                Intcode ampC = new Intcode(program, ampB.output);
                ampC.input.Add(phases[2]);

                Intcode ampD = new Intcode(program, ampC.output);
                ampD.input.Add(phases[3]);

                Intcode ampE = new Intcode(program, ampD.output);
                ampE.input.Add(phases[4]);
                ampE.output = ampA.input;

                // Run all in parallel
                Parallel.Invoke(
                    () => ampA.run(),
                    () => ampB.run(),
                    () => ampC.run(),
                    () => ampD.run(),
                    () => ampE.run()
                );

                long thrust = ampE.output.Take();
                if (thrust > max) {
                    max = thrust;
                }
            } while (nextPermutation(ref phases));
            return max;
        }
    }
}
