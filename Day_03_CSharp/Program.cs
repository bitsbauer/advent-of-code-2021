using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2021 - Day 03: Binary Diagnostic (https://adventofcode.com/2021/day/3)");
            
            string inputFile = args.Length > 0 ? args[0] : "input.txt";            
            var lines = File.ReadLines(inputFile).ToArray();
            var rates = FindRates(lines);

            Console.WriteLine("Part 1: What is the power consumption of the submarine?");
            Console.WriteLine("Result: " + Convert.ToInt32(rates.Item1, 2) * Convert.ToInt32(rates.Item2, 2));
            Console.WriteLine("Part 2: What is the life support rating of the submarine?");
            Console.WriteLine("Result: " + Convert.ToInt32(rates.Item3, 2) * Convert.ToInt32(rates.Item4, 2));
        }

        static Tuple<string, string, string, string> FindRates(string[] lines)
        {
            char[] gammaRate = new char[lines[0].Length];
            char[] epsilonRate = new char[lines[0].Length];
            int[,] map = new int[lines[0].Length, 2];
            foreach(string line in lines) {
                for(int i = 0; i < line.Length; i ++) {
                    switch(line[i]) {
                        case '0':
                            map[i,0] ++;
                            break;
                        default: 
                            map[i,1] ++;
                            break;
                    }
                }
            }
            for(int i = 0; i < lines[0].Length; i ++) {
                gammaRate[i] = map[i,0] > map[i,1] ? '0' : '1';
                epsilonRate[i] = map[i,0] > map[i,1] ? '1' : '0';
            }

            return new Tuple<string, string, string, string> (
                new string(gammaRate),
                new string(epsilonRate),
                FindLifeSupportRate(lines, true),
                FindLifeSupportRate(lines, false)
            );
        }

        static string FindLifeSupportRate(string[] lines, bool isOxygen) 
        {
            var lastList = lines.ToList();
            int[] map;
            for(int i = 0; i < lines[0].Length; i++) {
                map = new int[2];
                foreach(string lastListLine in lastList) {
                    switch(lastListLine[i]) {
                        case '0':
                            map[0] ++;
                            break;
                        default: 
                            map[1] ++;
                            break;
                    }
                }
                char search = map[1] >= map[0] ? isOxygen ? '1' : '0' : isOxygen ? '0' : '1';
                var newList = new List<string>();
                for(int j = 0; j < lastList.Count; j++) {
                    if (lastList.ElementAt(j)[i] == search) {
                        newList.Add(lastList.ElementAt(j));
                    }
                }
                if (newList.Count == 1) {
                    return newList.First();
                }
                 lastList = newList;
            }
            return "";
        }
    }
}
