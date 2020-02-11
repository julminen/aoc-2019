using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc_2019 {
    class Day06 {
        Dictionary<string, OrbitalObject> universe;

        internal Day06(string inputFileName) {
            string[] lines = System.IO.File.ReadAllLines(inputFileName);
            this.universe = new Dictionary<string, OrbitalObject>();
            foreach(string line in lines) {
                string[] ids = line.Split(')');
                OrbitalObject a;
                if (universe.ContainsKey(ids[0])) {
                    a = universe[ids[0]];
                } else {
                    a = new OrbitalObject(ids[0]);
                    universe[ids[0]] = a;
                }
                OrbitalObject b;
                if (universe.ContainsKey(ids[1])) {
                    b = universe[ids[1]];
                } else {
                    b = new OrbitalObject(ids[1]);
                    universe[ids[1]] = b;
                }
                b.parentName = ids[0];
                b.parentObject = a;
            }
        }

        internal void ShowSolution(string[] args) {
            Console.WriteLine("Day 6");
            if (args.Length < 2 || args[1] == "1") {
                Console.WriteLine($"Phase 1: {this.Phase1()}");
            }
            if (args.Length < 2 || args[1] == "2") {
                Console.WriteLine($"Phase 2: {this.Phase2()}");
            }
        }


        class OrbitalObject {
            internal OrbitalObject parentObject;
            internal string name;
            internal string parentName;
            int distance;
            internal OrbitalObject(string name) {
                this.name = name;
                if (name == "COM") {
                    this.distance = 0;
                } else {
                    this.distance = -1;
                }
            }
            
            internal int distanceFromCentre()
            {
                if (this.distance == -1) {
                    if (this.parentObject is null) {
                        Console.WriteLine($"Parent missing! {this.name} <- {this.parentName}");
                    }
                    this.distance = this.parentObject.distanceFromCentre() + 1;
                }
                return this.distance;
            }

        }

        private int Phase1() {
            return universe.Values.Sum(x => x.distanceFromCentre());
        }

        private int Phase2() {
            OrbitalObject me = universe["YOU"];
            OrbitalObject santa = universe["SAN"];

            Stack<OrbitalObject> myWay = new Stack<OrbitalObject>();
            Stack<OrbitalObject> santaWay = new Stack<OrbitalObject>();

            while (me.name != "COM")
            {
                myWay.Push(me);
                me = me.parentObject;
            }
            while (santa.name != "COM")
            {
                santaWay.Push(santa);
                santa = santa.parentObject;
            }
            // find common root and pop others away
            while(santaWay.Pop() == myWay.Pop());
            // difference of paths remains

            return myWay.Count() + santaWay.Count();
        }

    }
}