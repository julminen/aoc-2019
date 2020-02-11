using System;
using System.Linq;
using System.Collections.Generic;

namespace Aoc2019
{
    class Day08
    {
        private byte[] fileContents;
        private (int w, int h) dim = (25, 6);

        internal Day08(string inputFileName)
        {
            this.fileContents = System.IO.File.ReadAllBytes(inputFileName);
        }

        internal void ShowSolution(string[] args)
        {
            Console.WriteLine("Day 8");
            if (args.Length < 2 || args[1] == "1")
            {
                Console.WriteLine($"Phase 1: {this.Phase1()}");
            }
            if (args.Length < 2 || args[1] == "2")
            {
                Console.WriteLine($"Phase 2: {this.Phase2()}");
            }
        }

        internal class Layer
        {
            private (int w, int h) dimensions;
            internal uint[,] digits;
            internal Layer((int w, int h) dimensions, in byte[] bytes, int indexBase)
            {
                this.dimensions = dimensions;
                this.digits = new uint[dimensions.h, dimensions.w];
                for (int h = 0; h < dimensions.h; h++)
                {
                    for (int w = 0; w < dimensions.w; w++)
                    {
                        byte b = bytes[indexBase + h * dimensions.w + w];
                        if (b >= '0' && b <= '2')
                        {
                            digits[h, w] = b - (uint)'0';
                        }
                        else
                        {
                            Console.WriteLine($"Bad byte {b} at {h * w + w}!");
                        }
                    }
                }
            }

            public int Count(int num)
            {
                int nc = 0;
                for (int r = 0 ; r < dimensions.h ; r++) {
                    for (int c = 0 ; c < dimensions.w ; c++) {
                        if (digits[r, c] == num) nc++;
                    }
                }
                return nc;
            }

            internal void Print() {
                for (int r = 0 ; r < dimensions.h ; r++) {
                    for (int c = 0 ; c < dimensions.w ; c++) {
                        Console.Write($"{digits[r, c]} ");
                    }
                    Console.WriteLine();
                }
            }
        }

        private int Phase1()
        {
            int layerSize = dim.w * dim.h;
            int minZeros = layerSize + 1;
            int mzLayer = -1;
            List<Layer> layers = new List<Layer>();
            for (int i = 0; i + layerSize <= fileContents.Length; i = i + layerSize)
            {
                Layer nl = new Layer(dim, fileContents, i);
                layers.Add(nl);
                int zeros = nl.Count(0);
                if (zeros < minZeros) {
                    minZeros = zeros;
                    mzLayer = i/layerSize;
                }
            }

            return layers[mzLayer].Count(1) * layers[mzLayer].Count(2);
        }
        private string Phase2()
        {
            int layerSize = dim.w * dim.h;
            int minZeros = layerSize + 1;
            int mzLayer = -1;
            List<Layer> layers = new List<Layer>();
            for (int i = 0; i + layerSize <= fileContents.Length; i = i + layerSize)
            {
                Layer nl = new Layer(dim, fileContents, i);
                layers.Add(nl);
                int zeros = nl.Count(0);
                if (zeros < minZeros) {
                    minZeros = zeros;
                    mzLayer = i/layerSize;
                }
            }
            char[,] image = new char[dim.h,dim.w];
            for (int h = 0 ; h < dim.h ; h++) {
                for (int w = 0 ; w < dim.w ; w++) {
                    image[h, w] = ' '; // transparent
                    foreach(Layer l in layers) {
                        if(l.digits[h, w] < 2) {
                            switch (l.digits[h, w]) {
                                case 0:
                                    image[h, w] = ' '; // black
                                    break;
                                case 1:
                                    image[h, w] = '█'; // white
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
            for (int r = 0 ; r < dim.h ; r++) {
                for (int c = 0 ; c < dim.w ; c++) {
                    Console.Write($"{image[r, c]}");
                }
                Console.WriteLine();
            }

            return "See above";
        }
    }
}