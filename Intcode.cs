using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace aoc_2019
{
    class Intcode
    {
        private bool halt;
        private long[] program;
        private long instructionPointer = 0;
        private long relativeBase = 0;
        internal BlockingCollection<long> input;
        internal BlockingCollection<long> output;

        private Dictionary<long, long> extMemory;


        internal Intcode()
        {
            this.output = new BlockingCollection<long>();
            this.input = new BlockingCollection<long>();
            this.extMemory = new Dictionary<long, long>();
            this.program = new long[] { };
            this.instructionPointer = 0;
            this.relativeBase = 0;
            this.halt = true;
        }

        internal Intcode(long[] program, in BlockingCollection<long> input)
        {
            this.output = new BlockingCollection<long>();
            this.input = input;
            this.extMemory = new Dictionary<long, long>();
            this.program = (long[])program.Clone();
            this.instructionPointer = 0;
            this.relativeBase = 0;
            this.halt = true;
        }

        internal void load(ref long[] program)
        {
            this.program = program;
            this.instructionPointer = 0;
            this.relativeBase = 0;
            this.halt = true;
            this.extMemory.Clear();
        }

        internal void load(long[] program)
        {
            this.program = (long[])program.Clone();
            this.instructionPointer = 0;
            this.relativeBase = 0;
            this.halt = true;
            this.extMemory.Clear();
        }

        internal void clearOutput()
        {
            long o;
            while (this.output.TryTake(out o, 10)) ;
        }

        internal void run()
        {
            // Console.WriteLine("Started");
            this.run(null);
        }

        internal void run(IEnumerable<long> input)
        {
            halt = false;
            if (input != null)
            {
                foreach (long i in input)
                {
                    this.input.Add(i);
                }
            }

            while (!this.halt)
            {
                DecodeOpcode(program[instructionPointer])(ref instructionPointer, ref program);
            }
        }

        private long GetValue(long instruction, int parameterIndex, long parameterValue, ref long[] program)
        {
            char[] modes = (instruction / 100).ToString().Reverse().ToArray();
            char mode = parameterIndex >= modes.Length ? '0' : modes[parameterIndex];
            // Console.WriteLine($"Ref Mode {mode} in {instruction} @ {parameterIndex}, modes: {new String(modes)}, pv: {parameterValue}");
            if (mode == '0')
            {
                // position mode
                if (parameterValue >= program.Length)
                {
                    return extMemory.ContainsKey(parameterValue) ? extMemory[parameterValue] : 0;
                }
                return program[parameterValue];
            }
            else if (mode == '1')
            {
                // Immediate mode
                return parameterValue;
            }
            else if (mode == '2')
            {
                // Relative mode
                long memIdx = parameterValue + relativeBase;
                if (memIdx >= program.Length)
                {
                    return extMemory.ContainsKey(memIdx) ? extMemory[memIdx] : 0;
                }
                return program[memIdx];
            }
            Console.WriteLine($"Bad mode in {instruction} for paramter {parameterIndex}");
            return -1;
        }

        private void Store(long instruction, int parameterIndex, long parameterValue, long location, in long[] program)
        {
            char[] modes = (instruction / 100).ToString().Reverse().ToArray();
            char mode = parameterIndex >= modes.Length ? '0' : modes[parameterIndex];
            if (mode == '0')
            {
                // position mode
                if (location >= program.Length)
                {
                    extMemory[location] = parameterValue;
                }
                else
                {
                    program[location] = parameterValue;
                }
            }
            // Immediate mode makes no sense
            else if (mode == '2')
            {
                // Relative mode
                long memIdx = location + relativeBase;
                if (memIdx >= program.Length)
                {
                    extMemory[memIdx] = parameterValue;
                }
                else
                {
                    program[memIdx] = parameterValue;
                }
            } else {
                Console.WriteLine($"Bad mode in {instruction} for parameter {parameterIndex}: {mode}");
            }
        }

        // Opcode 1 adds together numbers read from two positions and stores the result in a third position.
        private void Add(ref long ip, ref long[] program)
        {
            long instruction = program[ip];
            long param_1 = GetValue(instruction, 0, program[ip + 1], ref program);
            long param_2 = GetValue(instruction, 1, program[ip + 2], ref program);
            long storeLocation = program[ip + 3];
            Store(instruction, 2, param_1 + param_2, storeLocation, in program);
            ip += 4;
        }

        // Opcode 2 multiplies numbers read from two positions and stores the result in a third position.
        private void Multiply(ref long ip, ref long[] program)
        {
            long instruction = program[ip];
            long param_1 = GetValue(instruction, 0, program[ip + 1], ref program);
            long param_2 = GetValue(instruction, 1, program[ip + 2], ref program);
            long storeLocation = program[ip + 3];
            Store(instruction, 2, param_1 * param_2, storeLocation, in program);
            ip += 4;
        }

        private void Halt(ref long ip, ref long[] program)
        {
            this.halt = true;
        }

        private void Error(ref long ip, ref long[] program)
        {
            Console.WriteLine($"Bad op at {ip}");
            this.halt = true;
        }

        // Opcode 3 takes a single integer as input and saves it to the position given by its only parameter
        private void OpInput(ref long ip, ref long[] program)
        {
            long storeLocation = program[ip + 1];
            Store(program[ip], 0, this.input.Take(), storeLocation, in program);
            ip += 2;
        }

        // Opcode 4 outputs the value of its only parameter.
        private void OpOutput(ref long ip, ref long[] program)
        {
            long o = this.GetValue(program[ip], 0, program[ip + 1], ref program);
            this.output.Add(o);
            ip += 2;
        }

        // Opcode 5 is jump-if-true: if the first parameter is non-zero, it sets the instruction pointer 
        // to the value from the second parameter.
        // Otherwise, it does nothing.
        private void JumpIfTrue(ref long ip, ref long[] program)
        {
            long condition = GetValue(program[ip], 0, program[ip + 1], ref program);
            long address = GetValue(program[ip], 1, program[ip + 2], ref program);
            if (condition != 0)
            {
                ip = address;
            }
            else
            {
                ip += 3;
            }
        }

        // Opcode 6 is jump-if-false: if the first parameter is zero, it sets the instruction pointer 
        // to the value from the second parameter.
        // Otherwise, it does nothing.
        private void JumpIfFalse(ref long ip, ref long[] program)
        {
            long condition = GetValue(program[ip], 0, program[ip + 1], ref program);
            long address = GetValue(program[ip], 1, program[ip + 2], ref program);
            if (condition == 0)
            {
                ip = address;
            }
            else
            {
                ip += 3;
            }
        }

        // Opcode 7 is less than: if the first parameter is less than the second parameter, it 
        // stores 1 in the position given by the third parameter. Otherwise, it stores 0.
        private void LessThan(ref long ip, ref long[] program)
        {
            long param_1 = GetValue(program[ip], 0, program[ip + 1], ref program);
            long param_2 = GetValue(program[ip], 1, program[ip + 2], ref program);
            long location = program[ip + 3];
            if (param_1 < param_2)
            {
                Store(program[ip], 2, 1, location, in program);
            }
            else
            {
                Store(program[ip], 2, 0, location, in program);
            }
            ip += 4;
        }

        // Opcode 8 is equals: if the first parameter is equal to the second parameter, it 
        // stores 1 in the position given by the third parameter. Otherwise, it stores 0.
        private void Equals(ref long ip, ref long[] program)
        {
            long param_1 = GetValue(program[ip], 0, program[ip + 1], ref program);
            long param_2 = GetValue(program[ip], 1, program[ip + 2], ref program);
            long location = program[ip + 3];
            if (param_1 == param_2)
            {
                Store(program[ip], 2, 1, location, in program);
            }
            else
            {
                Store(program[ip], 2, 0, location, in program);
            }
            ip += 4;
        }

        // Opcode 9 adjusts the relative base by the value of its only parameter. The relative
        // base increases (or decreases, if the value is negative) by the value of the parameter.
        private void SetBase(ref long ip, ref long[] program)
        {
            this.relativeBase += GetValue(program[ip], 0, program[ip + 1], ref program);
            ip += 2;
        }

        private delegate void Instruction(ref long instructionPointer, ref long[] program);

        private Instruction DecodeOpcode(long opcode)
        {
            switch (opcode % 100)
            {
                case 1:
                    return Add;
                case 2:
                    return Multiply;
                case 3:
                    return OpInput;
                case 4:
                    return OpOutput;
                case 5:
                    return JumpIfTrue;
                case 6:
                    return JumpIfFalse;
                case 7:
                    return LessThan;
                case 8:
                    return Equals;
                case 9:
                    return SetBase;
                case 99:
                    return Halt;
            }
            return Error;
        }

    }
}
