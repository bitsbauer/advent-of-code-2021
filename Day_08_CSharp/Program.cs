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
            Console.WriteLine("Advent of Code 2021 - Day 08: Seven Segment Search (https://adventofcode.com/2021/day/8)");
            
            string inputFile = args.Length > 0 ? args[0] : "input.txt";
            
            Console.WriteLine("Part 1: In the output values, how many times do digits 1, 4, 7, or 8 appear?");
            Console.WriteLine("Result: " + SumUniqueDigits(inputFile));
            Console.WriteLine("Part 2: What do you get if you add up all of the output values?");
            Console.WriteLine("Result: " + SumOutputValues(inputFile));
        }

        static int SumUniqueDigits(string inputFile)
        {
            return File.ReadAllLines(inputFile).Select(line => line.Split(" | ").Last().Split(' ').Select(x => x.Length < 5 || x.Length > 6 ? 1 : 0).Sum()).Sum();
        }

        static int SumOutputValues(string inputFile)
        {
            return File.ReadAllLines(inputFile).Select(line => FindOutputValueForLine(line)).Sum();
        }

        static int FindOutputValueForLine(string line) 
        {
            var lineParts = line.Split(" | ");
            var digitPatterns = lineParts.First().Split(' ').Select(s => new string(new SortedSet<char>(s).ToArray()));
            var digitOutputs = lineParts.Last().Split(' ').Select(s => new string(new SortedSet<char>(s).ToArray()));

            var mapping = new Dictionary<char, string>();
            mapping.Add('1', digitPatterns.Single(x => x.Length == 2));
            mapping.Add('7', digitPatterns.Single(x => x.Length == 3));
            mapping.Add('4', digitPatterns.Single(x => x.Length == 4));
            mapping.Add('8', digitPatterns.Single(x => x.Length == 7));

            var twoOrThreeOrFive = digitPatterns.Where(x => x.Length == 5).ToArray();
            mapping.Add('3', twoOrThreeOrFive.Single(x => x.Intersect(mapping['1']).Count() == 2));

            var sixOrNullOrNine = digitPatterns.Where(x => x.Length == 6).ToArray();
            mapping.Add('9', sixOrNullOrNine.Single(x => x.Intersect(mapping['4']).Count() == 4));

            var sixOrNull =  sixOrNullOrNine.Where(s => s != mapping['9']).ToArray(); 
            mapping.Add('0', sixOrNull.Single(x => x.Intersect(mapping['1']).Count() == 2));
            mapping.Add('6', sixOrNull.Single(x => x.Intersect(mapping['1']).Count() != 2));

            var twoOrFive =  twoOrThreeOrFive.Where(s => s != mapping['3']).ToArray();
            mapping.Add('5', twoOrFive.Single(x => x.Intersect(mapping['9']).Count() == 5));
            mapping.Add('2', twoOrFive.Single(x => x.Intersect(mapping['9']).Count() != 5));

            return Convert.ToInt32(new string(digitOutputs.Select(d => mapping.FirstOrDefault(x => x.Value == d).Key).ToArray()));
        }
    }
}
