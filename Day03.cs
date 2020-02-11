using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc_2019 {
    class Day03 {
        private readonly string[][] wirePaths;

        internal Day03(string inputFileName) {
            string[] lines = System.IO.File.ReadAllLines(inputFileName);
            this.wirePaths = new string[lines.Length][];
            for(int i = 0; i < lines.Length; i++) {
                this.wirePaths[i] = lines[i].Trim().Split(',');
            }
        }

        internal void ShowSolution(string[] args) {
            Console.WriteLine("Day 3");
            if (args.Length < 2 || args[1] == "1") {
                Console.WriteLine($"Phase 1: {this.Phase1()}");
            }
            if (args.Length < 2 || args[1] == "2") {
                Console.WriteLine($"Phase 2: {this.Phase2()}");
            }
        }

        private Dictionary<(int, int), int> CreatePath(string[] rules)
        {
            Dictionary<(int, int), int> path = new Dictionary<(int, int), int>();

            var location = (x: 0, y: 0);
            int steps = 0;
            // path[location] = id;
            foreach(string rule in rules) {
                char direction = rule[0];
                int distance = int.Parse(rule.Substring(1));
                var mods = (x: 0, y: 0);
                switch (direction) 
                {
                    case 'R':
                        mods.x = 1;
                        break;
                    case 'L':
                        mods.x = -1;
                        break;
                    case 'U':
                        mods.y = 1;
                        break;
                    case 'D':
                        mods.y = -1;
                        break;
                }
                for(int d = 0 ; d < distance ; d++) {
                    location.x += mods.x;
                    location.y += mods.y;
                    steps++;
                    if (!path.ContainsKey(location)) {
                        path[location] = steps;
                    }
                }
            }

            return path;
        }

        int ManhattanDistance((int x, int y) a, (int x, int y) b) {
            
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        int ManhattanDistanceFromOrigin((int x, int y) a) {
            return ManhattanDistance(a, (0, 0));
        }

        private int Phase1() {
            Dictionary<(int, int), int> path1 = CreatePath(this.wirePaths[0]);
            Dictionary<(int, int), int> path2 = CreatePath(this.wirePaths[1]);
            var commonKeys = path1.Keys.Intersect(path2.Keys);
            return commonKeys.Min(x => ManhattanDistanceFromOrigin(x));
        }

        private int Phase2() {
            Dictionary<(int, int), int> path1 = CreatePath(this.wirePaths[0]);
            Dictionary<(int, int), int> path2 = CreatePath(this.wirePaths[1]);
            var commonKeys = path1.Keys.Intersect(path2.Keys);
            return commonKeys.Select(x => path1[x]+ path2[x]).Min();
        }

    }
}