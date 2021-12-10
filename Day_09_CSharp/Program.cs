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
            Console.WriteLine("Advent of Code 2021 - Day 09: Smoke Basin (https://adventofcode.com/2021/day/9)");
            
            string inputFile = args.Length > 0 ? args[0] : "input.txt";
            
            Console.WriteLine("Part 1: What is the sum of the risk levels of all low points on your heightmap?");
            Console.WriteLine("Result: " + SumRiskLevels(inputFile));

            Console.WriteLine("Part 2: What do you get if you multiply together the sizes of the three largest basins?");
            Console.WriteLine("Result: " + MultiplyLargestBasins(inputFile));
        }

        static int SumRiskLevels(string inputFile)
        {
            var heightMap = ReadHeightMap(inputFile);
            var lowPoints = GetLowPoints(heightMap);
            var riskLevels = 0;
            foreach(var lowPoint in lowPoints) {
                riskLevels += (1 + heightMap[lowPoint.Item1,lowPoint.Item2]);
            }
            return riskLevels;
        }

        static int MultiplyLargestBasins(string inputFile)
        {
            var heightMap = ReadHeightMap(inputFile);
            var lowPoints = GetLowPoints(heightMap);
            var basinSizes = new List<int>();
            foreach(var lowPoint in lowPoints) {
                basinSizes.Add(GetBasinSize(lowPoint.Item1, lowPoint.Item2, heightMap));
            }
            return basinSizes.OrderBy(x => x).TakeLast(3).Aggregate((m,x) => m * x);
        }

        static int[,] ReadHeightMap(string inputFile)
        {
            var lines = File.ReadAllLines(inputFile);
            var heightMap = new int[lines.First().Length,lines.Count()];
            for(var x = 0; x < heightMap.GetLength(0); x++)
            {
                for(var y = 0; y < heightMap.GetLength(1); y++)
                {
                    heightMap[x,y] = (int)Char.GetNumericValue(lines[y][x]);
                }
            }
            return heightMap;
        }

        static bool isLowPoint(int xPos, int yPos, int[,] heightMap)
        {
            for(int x = xPos - 1; x <= xPos + 1; x++)
            {
                for(int y = yPos - 1; y <= yPos + 1; y++)
                {
                    if (x < 0 
                    || x > heightMap.GetLength(0) - 1 
                    || y < 0 
                    || y > heightMap.GetLength(1) - 1 
                    || (x == xPos && y == yPos)) {
                        continue;
                    }
                    if(heightMap[x,y] < heightMap[xPos, yPos]) {
                        return false;
                    }
                }
            }
            return true;
        }

        static List<Tuple<int, int>> GetLowPoints(int[,] heightMap)
        {
            var lowPoints = new List<Tuple<int, int>>();
            for(var x = 0; x < heightMap.GetLength(0); x++)
            {
                for(var y = 0; y < heightMap.GetLength(1); y++)
                {
                    if (isLowPoint(x,y,heightMap)) {
                        lowPoints.Add(new Tuple<int, int>(x,y));
                    }
                }
            }
            return lowPoints;
        }

        static int GetBasinSize(int x, int y, int[,] heightMap)
        {
            var basinMap = new int[heightMap.GetLength(0),heightMap.GetLength(1)];
            basinMap = ValidateSurroundingPointsInBasin(x, y, heightMap, basinMap);
            return basinMap.Cast<int>().Sum();
        }

        static int[,] ValidateSurroundingPointsInBasin(int xPos, int yPos, int[,] heightMap, int[,] basinMap)
        {
            for(int x = xPos - 1; x <= xPos + 1; x++)
            {
                for(int y = yPos - 1; y <= yPos + 1; y++)
                {
                    if (x < 0 
                    || x > heightMap.GetLength(0) - 1 
                    || y < 0 
                    || y > heightMap.GetLength(1) - 1 
                    || (x == xPos && y == yPos) 
                    || ( x != xPos && y != yPos)) {
                        continue;
                    } 
                    if(heightMap[x,y] < 9 && basinMap[x,y] < 1) {
                        basinMap[x,y] = 1;
                        basinMap = ValidateSurroundingPointsInBasin(x, y, heightMap, basinMap);
                    }
                }
            }
            return basinMap;
        }
    }
}
