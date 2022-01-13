using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2021 - Day 15: Chiton (https://adventofcode.com/2021/day/15)");
            string inputFile = args.Length > 0 ? args[0] : "input_test.txt";
    
            Console.WriteLine("Part 1: What is the lowest total risk of any path from the top left to the bottom right?");
            Console.WriteLine("Result: " +  FindPath(inputFile));

        }

        static List<List<int>> ParseInputToGrid(string inputFile)
        {
            var lines = File.ReadAllLines(inputFile);            
            return lines.Select(l => l.ToArray().Select(c => int.Parse(c.ToString())).ToList()).ToList();
        }

        static int FindPath(string inputFile)
        {
            
            var grid = ParseInputToGrid(inputFile);
            // Aufsmmieren erste Zeile
            for(int x = 1; x < grid[0].Count; x++) {
                grid[0][x] += grid[0][x - 1];
            }   
            // Aufsmmieren erste Spalte
            for(int y = 1; y < grid.Count; y++) {
                grid[y][0] += grid[y - 1][0];
            }
            // Aufsummieren der min-Werte
            for(int x = 1; x < grid[0].Count; x++)
            {
                for(int y = 1; y < grid.Count; y++)
                {
                    int val1 = grid[x - 1][y];
                    int val2 = grid[x][y - 1];
                    grid[x][y] += Math.Min(val1, val2);
                }
            }
            return grid[grid[0].Count-1][grid.Count-1] - grid[0][0];
        }
    }
}