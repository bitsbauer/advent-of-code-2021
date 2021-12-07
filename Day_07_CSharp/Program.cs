using System;
using System.IO;
using System.Linq;

namespace AoC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2021 - Day 07: The Treachery of Whales (https://adventofcode.com/2021/day/7)");
            
            string inputFile = args.Length > 0 ? args[0] : "input.txt";
            
            Console.WriteLine("Part 1: How much fuel must they spend to align to that position?");
            Console.WriteLine("Result: " + CalculateMinFuel(inputFile, false));
            Console.WriteLine("Part 2: How much fuel must they spend to align to that position?");
            Console.WriteLine("Result: " + CalculateMinFuel(inputFile, true));
        }

        static int CalculateMinFuel(string inputFile, bool useGauss)
        {
            int[] positions = File.ReadAllText(inputFile).Split(',').Select(s => int.Parse(s)).ToArray();
            
            int basis = positions.Min();
            int minDeviation = CalculateDeviation(positions, basis, useGauss);
            while(++basis <= positions.Max() ) {
                int deviation = CalculateDeviation(positions, basis, useGauss);
                if (minDeviation > deviation) {
                    minDeviation = deviation;
                } else {
                    return minDeviation;
                }
            }
            return minDeviation;
        }

        static int CalculateDeviation(int[] positions, int basis, bool useGauss) 
        {
            return useGauss 
            ? positions.Select(x => { int n = Math.Abs(x - basis); return (n*n+n)/2; }).Sum() 
            : positions.Select(x => Math.Abs(x - basis)).Sum();
        }
    }
}
