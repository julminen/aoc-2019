using System;
using System.Linq;

namespace aoc_2019
{
    class Day01
    {
        private int[] modules;
        internal Day01(string inputFileName) {
            this.modules = System.IO.File.ReadAllLines(inputFileName).Select(x => int.Parse(x)).ToArray();
        }

        internal void ShowSolution(string[] args) {
            Console.WriteLine("Day 1");
            if (args.Length < 2 || args[1] == "1") {
                Console.WriteLine($"Phase 1: {this.Phase1}");
            }
            if (args.Length < 2 || args[1] == "2") {
                Console.WriteLine($"Phase 2: {this.Phase2}");
            }
        }

        int fuel_requirement(int module_mass)
        {
            return (module_mass / 3) - 2;
        }

        int total_fuel_requirement(int module_mass)
        {
            int total = 0;
            for (int remaining = fuel_requirement(module_mass); remaining > 0; remaining = fuel_requirement(remaining))
                total += remaining;
            return total;
        }

        private int Phase1
        {
            get
            {
                return this.modules.Sum(fuel_requirement);
            }
        }
        private int Phase2
        {
            get
            {
                return this.modules.Sum(total_fuel_requirement);
            }
        }
    }
}