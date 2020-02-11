using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc_2019
{
    class Day11
    {
        private readonly long[] program;
        internal Day11(string inputFileName) {
            this.program = new long[] {}; // System.IO.File.ReadAllText(inputFileName).Split(',').Select(x => long.Parse(x)).ToArray();
        }

        internal void ShowSolution(string[] args) {
            Console.WriteLine("Day 11");
            if (args.Length < 2 || args[1] == "1") {
                Console.WriteLine($"Phase 1: {this.Phase1()}");
            }
            if (args.Length < 2 || args[1] == "2") {
                Console.WriteLine($"Phase 2: {this.Phase2()}");
            }
        }

        internal class Bot
        {
            enum Direction
            {
                Up = 0, Right, Down, Left
            }
            enum Color : long
            {
                Black = 0, White = 1
            }
            enum Turn : long{
                Left = 0, Right = 1
            }

            private Direction direction;
            private (int x, int y) location;
            private Intcode brain;
            private Dictionary<(int x, int y), Color> colorMap;

            internal Bot(Intcode brain) {
                this.direction = Direction.Up;
                this.location = (0, 0);
                this.brain = brain;
                this.colorMap = new Dictionary<(int x, int y), Color>();

                // Start brain in background
            }

            internal void step() {
                Color currentColor = colorMap.GetValueOrDefault(this.location, Color.Black);
                this.brain.input.Add((long)currentColor);
                Color newColor = (Color)this.brain.output.Take();
                Turn turn = (Turn)this.brain.output.Take();
                this.colorMap[this.location] = newColor;
                if (turn == Turn.Right) {
                    this.direction = (Direction)(((int)this.direction + 1) % 4);
                } else {
                    this.direction = (Direction)(((int)this.direction + 3) % 4);
                }
                if (this.direction == Direction.Up) {
                    this.location.y += 1;
                } else if (this.direction == Direction.Down) {
                    this.location.y -= 1;
                } else if (this.direction == Direction.Right) {
                    this.location.x += 1;
                } else {
                    this.location.x -= 1;
                }
                
            }

        }

        private int Phase1()
        {
            Intcode botBrain = new Intcode();
            botBrain.load(program);


            return 0;
        }
        private int Phase2()
        {
            return 0;
        }
    }
}