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
            Console.WriteLine("Advent of Code 2021 - Day 14: Transparent Origami (https://adventofcode.com/2021/day/14)");
            string inputFile = args.Length > 0 ? args[0] : "input.txt";
    
            Console.WriteLine("Part 1: What do you get if you take the quantity of the most common element and subtract the quantity of the least common element? (10 steps)");
            Console.WriteLine("Result: " +  Polymerize(inputFile, 10));
           
            Console.WriteLine("Part 2: What do you get if you take the quantity of the most common element and subtract the quantity of the least common element? (40 steps)");
            Console.WriteLine("Result: " +  Polymerize(inputFile, 40));
        }

        static (string, Dictionary<string, Tuple<string, string>>) ParseInput(string inputFile)
        {
            var lines = File.ReadAllLines(inputFile);
            
            var template = lines.First();
            var mapping = lines.Select(l => l.Split(" -> ")).Where((l,x) => x > 1).ToDictionary(x => x.First(), x => new Tuple<string,string>(x.First().Insert(1,x.Last()).Substring(0,2), x.First().Insert(1,x.Last()).Substring(1,2)));

            return (template, mapping);
        }

        static long Polymerize(string inputFile, int steps)
        {
            var parsedInput = ParseInput(inputFile);

            var template = parsedInput.Item1;
            var mapping = parsedInput.Item2;

            Dictionary<string,long> counting = mapping.Keys.ToDictionary(x => x, x => (long)0);
            for (int i = 0; i < template.Length - 1; i++) {
                counting[template.Substring(i,2)] = counting.GetValueOrDefault(template.Substring(i,2)) + 1;
            } 

            for(int step = 1; step <= steps; step++) {
                counting = CalculateCounting(counting, mapping);
            }

            var charCounting = new Dictionary<string, long>();
            foreach(var item in counting) {
                charCounting[item.Key.Substring(0,1)] = charCounting.GetValueOrDefault(item.Key.Substring(0,1)) + item.Value;
                charCounting[item.Key.Substring(1,1)] = charCounting.GetValueOrDefault(item.Key.Substring(1,1)) + item.Value;
            };
            charCounting = charCounting.Select(x => x).ToDictionary(x => x.Key, x => (long)Math.Ceiling((decimal)x.Value/(decimal)2));

            return charCounting.Select(c => c.Value).Max() - charCounting.Select(c => c.Value).Min();
        }

        static Dictionary<string, long> CalculateCounting(Dictionary<string, long> lastCounting, Dictionary<string, Tuple<string, string>> mapping)
        {
            Dictionary<string,long> counting = mapping.Keys.ToDictionary(x => x, x => (long)0);
            foreach(var last in lastCounting) {
                if(last.Value > 0) {
                    counting[mapping[last.Key].Item1] = counting.GetValueOrDefault(mapping[last.Key].Item1) + last.Value;
                    counting[mapping[last.Key].Item2] = counting.GetValueOrDefault(mapping[last.Key].Item2) + last.Value;
                }
            }
            return counting;
        }
    }
}