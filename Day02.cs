using System;
using System.Linq;

namespace aoc_2019
{
    class Day02
    {

        private readonly int[] program;
        internal Day02(string inputFileName)
        {
            this.program = System.IO.File.ReadAllText(inputFileName).Split(',').Select(x => int.Parse(x)).ToArray();
        }

        internal void ShowSolution(string[] args) {
            Console.WriteLine("Day 2");
            if (args.Length < 2 || args[1] == "1") {
                Console.WriteLine($"Phase 1: {this.Phase1()}");
            }
            if (args.Length < 2 || args[1] == "2") {
                Console.WriteLine($"Phase 2: {this.Phase2(19690720)}");
            }
        }

        int Step(int pc, ref int[] program)
        {
            switch (program[pc])
            {
                case 1:
                    program[program[pc + 3]] = program[program[pc + 2]] + program[program[pc + 1]];
                    pc += 4;
                    break;
                case 2:
                    program[program[pc + 3]] = program[program[pc + 2]] * program[program[pc + 1]];
                    pc += 4;
                    break;
                case 99:
                    pc = -1;
                    break;
                default:
                    Console.WriteLine($"Bad opcode at {pc}: {program[pc]}");
                    pc = -1;
                    break;
            }

            return pc;
        }

        private int Phase1()
        {

            int[] codes = (int[])program.Clone(); // Shallow copy but since elemts are not objects this works

            codes[1] = 12;
            codes[2] = 2;

            int pc = 0;
            while (pc != -1)
            {
                pc = Step(pc, ref codes);
            }
            return codes[0];
        }

        private int Phase2(int targetResult) {

            int[] codes;
            int pc;

            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    codes = (int[])program.Clone();
                    pc = 0;
                    codes[1] = noun;
                    codes[2] = verb;
                    while (pc != -1)
                    {
                        pc = Step(pc, ref codes);
                    }
                    if (codes[0] == targetResult)
                    {
                        // Console.WriteLine($"Phase 2: {noun}, {verb} -> {100 * noun + verb}");
                        return 100 * noun + verb;
                    }
                }
            }
            return 0;
        }
    }
}
