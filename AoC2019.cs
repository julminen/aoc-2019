using System;
using Aoc2019;

namespace aoc_2019
{
    class AoC2019
    {

        static void Main(string[] args)
        {
            if (args.Length > 0) {
                switch (args[0])
                {
                    case "1":
                        new Day01("input/day_01.txt").ShowSolution(args);
                        break;
                    case "2":
                        new Day02("input/day_02.txt").ShowSolution(args);
                        break;
                    case "3":
                        new Day03("input/day_03.txt").ShowSolution(args);
                        break;
                    case "4":
                        new Day04().ShowSolution(args);
                        break;
                    case "5":
                        new Day05("input/day_05.txt").ShowSolution(args);
                        break;
                    case "6":
                        new Day06("input/day_06.txt").ShowSolution(args);
                        break;
                    case "7":
                        new Day07("input/day_07.txt").ShowSolution(args);
                        break;
                    case "8":
                        new Day08("input/day_08.txt").ShowSolution(args);
                        break;
                    case "9":
                        new Day09("input/day_09.txt").ShowSolution(args);
                        break;
                    case "10":
                        new Day10("input/day_10.txt").ShowSolution(args);
                        break;
                    case "11":
                        new Day11("input/day_11.txt").ShowSolution(args);
                        break;
                    default:
                        Console.WriteLine($"Not implemented");
                        break;
                }
            }
        }

    }
}

