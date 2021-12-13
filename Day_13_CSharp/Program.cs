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
            Console.WriteLine("Advent of Code 2021 - Day 13: Transparent Origami (https://adventofcode.com/2021/day/13)");

            string inputFile = args.Length > 0 ? args[0] : "input.txt";
            var parsedInput = ParseInput(inputFile);
            var foldInstructions = parsedInput.Item2;
            var dotsMap = parsedInput.Item1;

            Console.WriteLine("Part 1: How many dots are visible after completing just the first fold instruction on your transparent paper?");
            Console.WriteLine("Result: " + CountDots(dotsMap, foldInstructions, 1));

            Console.WriteLine("Part 2: What code do you use to activate the infrared thermal imaging camera system?");
            Display(dotsMap, foldInstructions, foldInstructions.Count());
        }

        static int CountDots(HashSet<Tuple<int,int>> dotsMap, Tuple<string, int>[] foldInstructions, int foldSteps) 
        {
            return Fold(dotsMap, foldInstructions, foldSteps).Count();
        }

        static void Display(HashSet<Tuple<int,int>> dotsMap, Tuple<string, int>[] foldInstructions, int foldSteps) 
        {
            dotsMap = Fold(dotsMap, foldInstructions, foldSteps);        
            Tuple<int, int> dimensions = new Tuple<int, int>(dotsMap.Select(x => x.Item1).Max(), dotsMap.Select(x => x.Item2).Max());
            for(int y = 0; y <= dimensions.Item2; y++) {
                var line = new List<char>();
                for(int x = 0; x <= dimensions.Item1; x++) {
                    line.Add(dotsMap.Select(d => d).Where(d => d.Item1 == x && d.Item2 == y).Count() > 0 ? '#' : ' ');
                }
                Console.WriteLine(new string(line.ToArray()));
            }
        }

        static Tuple<HashSet<Tuple<int,int>>,Tuple<string,int>[]> ParseInput(string inputFile)
        {
            var inputParts = File.ReadAllText(inputFile).Split("\n\n");
            var dotsMap = inputParts.First().Split("\n").Select(x => x.Split(',')).Select(p => new Tuple<int,int>(int.Parse(p.First()), int.Parse(p.Last()))).ToHashSet();
            var foldInstructions = inputParts.Last().Split("\n").Select(x => x.Substring("fold along ".Length).Split('=')).Select(p => new Tuple<string,int>(p.First(), int.Parse(p.Last()))).ToArray();

            return new Tuple<HashSet<Tuple<int, int>>, Tuple<string, int>[]>(dotsMap, foldInstructions);
        }

        static HashSet<Tuple<int,int>> Fold(HashSet<Tuple<int,int>> dotsMap, Tuple<string, int>[] foldInstructions, int foldSteps) 
        {
            for(int foldStep = 0; foldStep < foldSteps; foldStep ++) {
                var foldInstruction = foldInstructions[foldStep];
                switch(foldInstruction.Item1) {
                    case "x":
                        dotsMap = FoldVertical(dotsMap, foldInstruction.Item2);
                        break;
                    case "y":
                        dotsMap = FoldHorizontal(dotsMap, foldInstruction.Item2);
                        break;
                    default:
                        break;
                }
            }
            return dotsMap;
        }

        static HashSet<Tuple<int,int>> FoldHorizontal(HashSet<Tuple<int,int>> dotsMap, int y) 
        {
            var foldedMap = dotsMap.ToHashSet();
            for(int i = 1; i + y <= y * 2; i ++) {
                foreach(var dot in dotsMap) {
                    if(dot.Item2 == y + i) {
                        foldedMap.Add(new Tuple<int, int>(dot.Item1, y - i));
                        foldedMap.Remove(dot);
                    }
                }
            }
            return foldedMap;
        }

        static HashSet<Tuple<int,int>> FoldVertical(HashSet<Tuple<int,int>> dotsMap, int x) 
        {
            var foldedMap = dotsMap.ToHashSet();
            for(int i = 1; i + x <= x * 2; i ++) {
                foreach(var dot in dotsMap) {
                    if(dot.Item1 == x + i) {
                        foldedMap.Add(new Tuple<int, int>(x - i, dot.Item2));
                        foldedMap.Remove(dot);
                    }
                }
            }
            return foldedMap;
        }
    }
}