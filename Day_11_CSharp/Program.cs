using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC
{
    class Program
    {
        public delegate void ForeachOctopusAction(int x, int y, int[,] octopusMap);

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2021 - Day 11: Dumbo Octopus (https://adventofcode.com/2021/day/11)");
            
            string inputFile = args.Length > 0 ? args[0] : "input.txt";
            
            Console.WriteLine("Part 1: How many total flashes are there after 100 steps?");
            Console.WriteLine("Result: " + CalculateFlashes(inputFile, 100));

            Console.WriteLine("Part 1: What is the first step during which all octopuses flash?");
            Console.WriteLine("Result: " + CalculateSync(inputFile));
        }

        static int CalculateFlashes(string inputFile, int steps)
        {
            var octopusMap = ReadOctopusMap(inputFile);
            int flashes = 0;
            for(var step = 1; step <= steps; step ++) {
                // increase energy by one
                for(var x = 0; x < octopusMap.GetLength(0); x++)
                {
                    for(var y = 0; y < octopusMap.GetLength(1); y++)
                    {
                        octopusMap[x,y]++;
                    }
                }

                // flash octopusses with level above 9
                var flashMap = new int[octopusMap.GetLength(0),octopusMap.GetLength(1)];
                for(var x = 0; x < octopusMap.GetLength(0); x++)
                {
                    for(var y = 0; y < octopusMap.GetLength(1); y++)
                    {
                        flashMap = FlashesForStep(x, y, octopusMap, flashMap);
                    }
                }

                // set flashed octopusses to 0 energy
                for(var x = 0; x < octopusMap.GetLength(0); x++)
                {
                    for(var y = 0; y < octopusMap.GetLength(1); y++)
                    {
                        if (octopusMap[x,y] > 9) {
                            octopusMap[x,y] = 0;
                        }
                    }
                }

                flashes += flashMap.Cast<int>().Sum();
            }
            return flashes;
        }

        static int CalculateSync(string inputFile)
        {
            var octopusMap = ReadOctopusMap(inputFile);
            int steps = 0;
            bool sync = false;
            while(sync == false) {
                steps++;
                // increase energy by one
                for(var x = 0; x < octopusMap.GetLength(0); x++)
                {
                    for(var y = 0; y < octopusMap.GetLength(1); y++)
                    {
                        octopusMap[x,y]++;
                    }
                }

                // flash octopusses with level above 9
                var flashMap = new int[octopusMap.GetLength(0),octopusMap.GetLength(1)];
                for(var x = 0; x < octopusMap.GetLength(0); x++)
                {
                    for(var y = 0; y < octopusMap.GetLength(1); y++)
                    {
                        flashMap = FlashesForStep(x, y, octopusMap, flashMap);
                    }
                }

                // set flashed octopusses to 0 energy
                for(var x = 0; x < octopusMap.GetLength(0); x++)
                {
                    for(var y = 0; y < octopusMap.GetLength(1); y++)
                    {
                        if (octopusMap[x,y] > 9) {
                            octopusMap[x,y] = 0;
                        }
                    }
                }

                int flashes = flashMap.Cast<int>().Sum();
                if (flashes == (octopusMap.GetLength(0) * octopusMap.GetLength(1))) {
                    sync = true;
                }
            }
            return steps;
        }

        static int[,] ReadOctopusMap(string inputFile)
        {
            var lines = File.ReadAllLines(inputFile);
            var octopusMap = new int[lines.First().Length,lines.Count()];
            for(var x = 0; x < octopusMap.GetLength(0); x++)
            {
                for(var y = 0; y < octopusMap.GetLength(1); y++)
                {
                    octopusMap[x,y] = (int)Char.GetNumericValue(lines[y][x]);
                }
            }
            return octopusMap;
        }

        static int[,] FlashesForStep(int x, int y, int[,] octopusMap, int[,] flashMap)
        {
            if (octopusMap[x,y] > 9 && flashMap[x,y] < 1) {
                flashMap[x,y] ++;
                flashMap = IncreaseSurrounding(x, y, octopusMap, flashMap);
            }
            return flashMap;
        }

        static int[,] IncreaseSurrounding(int xPos, int yPos, int[,] octopusMap, int[,] flashMap)
        {
            for(int x = xPos - 1; x <= xPos + 1; x++)
            {
                for(int y = yPos - 1; y <= yPos + 1; y++)
                {
                    if (x < 0 
                    || x > octopusMap.GetLength(0) - 1 
                    || y < 0 
                    || y > octopusMap.GetLength(1) - 1 
                    || (x == xPos && y == yPos)) {
                        continue;
                    }
                    octopusMap[x,y] ++;
                    flashMap = FlashesForStep(x, y, octopusMap, flashMap);
                }
            }
            return flashMap;
        }
    }
}