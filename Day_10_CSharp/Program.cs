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
            Console.WriteLine("Advent of Code 2021 - Day 10: Syntax Scoring (https://adventofcode.com/2021/day/10)");
            
            string inputFile = args.Length > 0 ? args[0] : "input.txt";
            
            Console.WriteLine("Part 1: What is the total syntax error score for those errors?");
            Console.WriteLine("Result: " + GetCorruptedLinesScore(inputFile));

            Console.WriteLine("Part 2: What is the middle score?");
            Console.WriteLine("Result: " + GetRepairMiddleScore(inputFile));
        }

        static int GetCorruptedLinesScore(string inputFile)
        {
            return File.ReadAllLines(inputFile).Select(l => GetCorruptnessScore(l)).Sum();
        }

        static int GetCorruptnessScore(string line)
        {
            var openCloseMapping = GetOpenCloseMapping();
            var charStack = new Stack<char>();
            foreach(char c in line) {
                if(openCloseMapping.Keys.Contains(c)) {
                    charStack.Push(c);
                } else if(openCloseMapping.GetValueOrDefault(charStack.Pop()) != c) {
                    return GetCorruptionScoreMapping().GetValueOrDefault(c);
                }
            }
            return -1;
        }

        static long GetRepairMiddleScore(string inputFile)
        {
            var sortedRepairScores = new SortedSet<long>(File.ReadAllLines(inputFile).Select(l => GetRepairScore(l)).Where(l => l > -1).ToArray());
            return sortedRepairScores.ElementAt((int)Math.Ceiling((double)(sortedRepairScores.Count() / 2)));
        }

        static long GetRepairScore(string line)
        {
            var openCloseMapping = GetOpenCloseMapping();
            var charStack = new Stack<char>();
            foreach(char c in line) {
                if(openCloseMapping.Keys.Contains(c)) {
                    charStack.Push(c);
                } else if(openCloseMapping.GetValueOrDefault(charStack.Pop()) != c) {
                    return -1;
                }
            }
            var repairScoreMapping = GetRepairScoreMapping();
            long repairScore = 0;
            while(charStack.Count() > 0) {
                var score = repairScoreMapping.GetValueOrDefault(openCloseMapping.GetValueOrDefault(charStack.Pop()));
                repairScore = repairScore * 5 + score;
            }
            return repairScore;
        }

        static Dictionary<char, char> GetOpenCloseMapping()
        {
            return new Dictionary<char, char>() {
                {'(',')'},
                {'[',']'},
                {'{','}'},
                {'<','>'}
            };
        }

        static Dictionary<char, int> GetCorruptionScoreMapping()
        {
            return new Dictionary<char,int>() {
                {')', 3},
                {']', 57},
                {'}', 1197},
                {'>', 25137}
            };
        }

        static Dictionary<char, int> GetRepairScoreMapping()
        {
            return new Dictionary<char,int>() {
                {')', 1},
                {']', 2},
                {'}', 3},
                {'>', 4}
            };
        }
    }
}