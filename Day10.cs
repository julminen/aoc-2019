using System;
using System.Linq;
using System.Collections.Generic;


namespace aoc_2019
{
    class Day10
    {
        class Asteroid
        {
            internal (int x, int y) Coordinates;
            internal HashSet<(int x, int y)> UnknownVisibility;
            internal HashSet<(int x, int y)> KnownVisible;
            internal HashSet<(int x, int y)> KnownHidden;
            internal double AngleFromBase;
            internal double DistanceFromBase;

            internal Asteroid((int x, int y) coordinates)
            {
                this.Coordinates = coordinates;
                this.UnknownVisibility = new HashSet<(int x, int y)>();
                this.KnownVisible = new HashSet<(int x, int y)>();
                this.KnownHidden = new HashSet<(int x, int y)>();
            }

            public override string ToString()
            {
                return $"{this.Coordinates}: {this.UnknownVisibility.Count()} unknowns, {this.KnownVisible.Count()} visible, {this.KnownHidden.Count()} hidden";
            }

            public void ResolveVisibility((int x, int y) bounds, in Dictionary<(int x, int y), Asteroid> Space)
            {
                while (UnknownVisibility.Count > 0)
                {
                    (int x, int y) other = UnknownVisibility.First();
                    int dx = other.x - Coordinates.x;
                    int dy = other.y - Coordinates.y;
                    int gcd = GreatestCommonDivisor(Math.Abs(dx), Math.Abs(dy));
                    (int x, int y) unitVec = (dx / gcd, dy / gcd);
                    bool visibleFound = false;
                    (int x, int y) loc = (Coordinates.x + unitVec.x, Coordinates.y + unitVec.y);
                    // Console.WriteLine($"Other = {other}, loc = {loc}, unitVec = {unitVec}");
                    while (loc.x >= 0 && loc.x < bounds.x && loc.y >= 0 && loc.y < bounds.y)
                    {
                        if (Space.ContainsKey(loc))
                        {
                            Asteroid a = Space[loc];
                            if (!a.UnknownVisibility.Remove(Coordinates)) Console.WriteLine("Remove fail");
                            UnknownVisibility.Remove(loc);
                            if (!visibleFound)
                            {
                                // Console.WriteLine($"{this.ToString()} sees {loc}");
                                visibleFound = true;
                                KnownVisible.Add(loc);
                                a.KnownVisible.Add(Coordinates);
                            }
                            else
                            {
                                // Console.WriteLine($"{this.ToString()} does not see {loc}");
                                KnownHidden.Add(loc);
                                a.KnownHidden.Add(Coordinates);
                            }
                        }
                        loc = (loc.x + unitVec.x, loc.y + unitVec.y);
                        // Console.WriteLine($"Loc = {loc}");
                    }
                }
            }
            private int GreatestCommonDivisor(int a, int b)
            {
                if (b == 0) return a;
                return GreatestCommonDivisor(b, a % b);
            }

            internal static double FindAngle((int x, int y) o, (int x, int y) a)
            {
                // Console.WriteLine($"{a}");

                // Transfer coord to 0, 0
                a.x -= o.x;
                a.y -= o.y;
                // Flip y axis
                a.y *= -1;
                // scale to unit circle, x^2 + y^2 = 1
                double c = Math.Sqrt(Math.Pow(a.x, 2) + Math.Pow(a.y, 2));
                (double x, double y) b = (a.x / c, a.y / c);

                // Make to be positive angle and 0 is up
                double angle;
                if (b.x >= 0)
                {
                    if (b.y >= 0)
                    {
                        angle = Math.Asin(b.y) + Math.PI * 2;
                    }
                    else
                    {
                        angle = 2 * Math.PI + Math.Asin(b.y);
                    }
                }
                else
                {
                    angle = Math.PI - Math.Asin(b.y);
                }
                // Rotate so that 0 is up, and mirror to clockwise
                angle = angle - Math.PI / 2;
                return angle * -1 + Math.PI * 2;
            }

            internal static double Distance((int x, int y) a, (int x, int y) b)
            {
                return Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
            }

            internal void ComputeRelativeToBase(Asteroid killer)
            {
                this.AngleFromBase = FindAngle(killer.Coordinates, this.Coordinates);
                this.DistanceFromBase = Distance(killer.Coordinates, this.Coordinates);
            }

        }

        class AsteroidSorter : Comparer<Asteroid> {
            public override int Compare(Asteroid x, Asteroid y) {
                if (x.DistanceFromBase < y.DistanceFromBase) {
                    return -1;
                } else if (x.DistanceFromBase > y.DistanceFromBase) {
                    return 1;
                }
                return 0;
            }
        }

        private Dictionary<(int x, int y), Asteroid> Space;
        internal (int x, int y) MaxCoord;
        internal Day10(string inputFileName)
        {
            string[] lines = System.IO.File.ReadAllLines(inputFileName);
            Space = new Dictionary<(int x, int y), Asteroid>();
            int y = 0;
            int x = 0;
            foreach (string line in lines)
            {
                x = 0;
                foreach (char c in line)
                {
                    if (c == '#')
                    {
                        Space[(x, y)] = new Asteroid((x, y));
                    }
                    x++;
                }
                y++;
            }
            MaxCoord = (x, y);
            foreach (Asteroid a in Space.Values)
            {
                foreach (Asteroid other in Space.Values.Except(new Asteroid[] { a }))
                {
                    a.UnknownVisibility.Add(other.Coordinates);
                }
            }
        }

        internal void ShowSolution(string[] args)
        {
            Console.WriteLine("Day 10");
            if (args.Length < 2 || args[1] == "1")
            {
                Console.WriteLine($"Phase 1: {this.Phase1()}");
            }
            if (args.Length < 2 || args[1] == "2")
            {
                Console.WriteLine($"Phase 2: {this.Phase2()}");
            }
        }

        private void ShowSpace((int x, int y) baseCoord)
        {
            for (int y = 0; y < MaxCoord.y; y++)
            {
                for (int x = 0; x < MaxCoord.x; x++)
                {
                    if (x == baseCoord.x && y == baseCoord.y)
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write($"{(Space.ContainsKey((x, y)) ? "#" : ".")}");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine($"{Space.Count} known asteroids");
        }

        private Asteroid FindBaseLocation()
        {
            int maxSeen = 0;
            Asteroid newBase = null;
            foreach (Asteroid a in Space.Values)
            {
                a.ResolveVisibility(MaxCoord, in Space);
                if (a.KnownVisible.Count() > maxSeen)
                {
                    maxSeen = a.KnownVisible.Count();
                    newBase = a;
                }
            }
            return newBase;
        }

        private int Phase1()
        {
            Asteroid newBase = FindBaseLocation();
            Console.WriteLine($"Base should be at {newBase}");
            ShowSpace(newBase.Coordinates);
            return newBase.KnownVisible.Count();
        }

        private int Phase2()
        {
            Asteroid newBase = FindBaseLocation();
            //ShowSpace(newBase.Coordinates);
            //Console.WriteLine($"Base at {newBase.Coordinates}");
            // Create Dictionary of angle from base -> queue of asteroids to shoot in that angle
            SortedDictionary<double, List<Asteroid>> shootQueue = new SortedDictionary<double, List<Asteroid>>();
            AsteroidSorter sorter = new AsteroidSorter();
            foreach (Asteroid target in Space.Values)
            {
                if (target.Coordinates != newBase.Coordinates) {
                    target.ComputeRelativeToBase(newBase);
                    double angle = target.AngleFromBase;
                    List<Asteroid> aq;
                    if (shootQueue.ContainsKey(angle)) {
                        aq = shootQueue[angle];
                        aq.Add(target);
                        aq.Sort(sorter);
                    } else {
                        aq = new List<Asteroid>();
                        aq.Add(target);
                        shootQueue[angle] = aq;
                    }
                }
            }
            // Start shooting
            int shotAsteroids = 0;
            (int x, int y) wagedAsteroid = (0, 0);
            // Console.WriteLine($"Angles: {shootQueue.Count()}");
            do {
                foreach(double a in shootQueue.Keys) {
                    // Console.WriteLine(a);
                    List<Asteroid> sa = shootQueue[a];
                    if (sa.Count > 0) {
                        Asteroid target = sa[0]; sa.RemoveAt(0);
                        shotAsteroids++;
                        // Console.WriteLine($"{shotAsteroids}: Shoot {target}");
                        if (shotAsteroids == 200) {
                            wagedAsteroid = target.Coordinates;
                        }
                    }
                }
            } while (shotAsteroids < 200);

            return wagedAsteroid.x * 100 + wagedAsteroid.y;
        }
    }
}