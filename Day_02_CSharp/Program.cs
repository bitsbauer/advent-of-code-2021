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
            Console.WriteLine("Advent of Code 2021 - Day 02: Dive! (https://adventofcode.com/2021/day/2)");
            
            string inputFile = args.Length > 0 ? args[0] : "input.txt";
            var diveCommands = ReadInputOfDiveCommands(inputFile);

            Console.WriteLine("Part 1: What do you get if you multiply your final horizontal position by your final depth?");
            var finalPosition = FinalPosition(diveCommands);
            Console.WriteLine("Result: " + finalPosition.Item1 * finalPosition.Item2);

            Console.WriteLine("Part 2: Using aim-calculation, what do you get if you multiply your final horizontal position by your final depth? ");
            var finalPositionWithAim = FinalPositionWithAim(diveCommands);
            Console.WriteLine("Result: " + finalPositionWithAim.Item1 * finalPositionWithAim.Item2);
        }
    
        protected static IEnumerable<Tuple<string, int>> ReadInputOfDiveCommands(string file)
        {
            return File.ReadAllLines(file).Select(item => { var p = item.Split(' '); return new Tuple<string, int>(p[0], int.Parse(p[1])); });
        }

        protected static Tuple<int, int> FinalPosition(IEnumerable<Tuple<string, int>> diveCommands) {
            int horizontalPosition = 0;
            int depth = 0;
            foreach(var diveCommand in diveCommands) {
                switch(diveCommand.Item1) {
                    case "forward": 
                        horizontalPosition += diveCommand.Item2;
                        break;
                    case "down": 
                        depth += diveCommand.Item2;
                        break; 
                    case "up": 
                        depth -= diveCommand.Item2;
                        break;   
                }
            }
            return new Tuple<int, int>(horizontalPosition, depth);
        }

        protected static Tuple<int, int> FinalPositionWithAim(IEnumerable<Tuple<string, int>> diveCommands) {
            int horizontalPosition = 0;
            int depth = 0;
            int aim = 0;
            foreach(var diveCommand in diveCommands) {
                switch(diveCommand.Item1) {
                    case "forward": 
                        horizontalPosition += diveCommand.Item2;
                        depth += aim * diveCommand.Item2;
                        break;
                    case "down": 
                        aim += diveCommand.Item2;
                        break; 
                    case "up": 
                        aim -= diveCommand.Item2;
                        break;   
                }
            }
            return new Tuple<int, int>(horizontalPosition, depth);
        }
    }
}
