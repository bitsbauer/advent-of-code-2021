using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2021 - Day 01: Sonar Sweep (https://adventofcode.com/2021/day/1)");
            
            List<int> measurements = ReadFrom("input.txt").Select(x => Int32.Parse(x)).ToList();

            Console.WriteLine("Part 1: How many measurements are larger than the previous measurement?");
            Console.WriteLine("Result: " + GetIncreasingMeasurementsCount(measurements));
            Console.WriteLine("Part 2: How many sums are larger than the previous sum?");
            Console.WriteLine("Result: " + GetSlidingWindowIncreasingMeasurementsCount(measurements));
        }

        static int GetIncreasingMeasurementsCount(List<int> measurements)
        {
            int increasingMeasurementsCount = 0;
            int lastMeasurement = measurements[0];
            for (int x = 1; x < measurements.Count; x ++) {
                int currentMeasurement = measurements[x];
                if (lastMeasurement < currentMeasurement) {
                    increasingMeasurementsCount ++;
                }
                lastMeasurement = currentMeasurement;
            }
            return increasingMeasurementsCount;
        }
        
        static int GetSlidingWindowIncreasingMeasurementsCount(List<int> measurements)
        {
            int increasingMeasurementsCount = 0;
            int lastMeasurement = SumSlidingWindowInMeasurements(measurements, 2, 3);
            for (int x = 3; x < measurements.Count; x ++) {
                int currentMeasurement = SumSlidingWindowInMeasurements(measurements, x, 3);
                if (lastMeasurement < currentMeasurement) {
                    increasingMeasurementsCount ++;
                }
                lastMeasurement = currentMeasurement;
            }
            return increasingMeasurementsCount;
        }

        static int SumSlidingWindowInMeasurements(List<int> measurements, int lastIndexInWindow, int windowSize) {
            int sum = 0;
            for(int x = lastIndexInWindow; x > lastIndexInWindow - windowSize; x--) {
                sum += measurements[x];
            }
            return sum;
        }

        static IEnumerable<string> ReadFrom(string file)
        {
            string line;
            using (var reader = File.OpenText(file))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
