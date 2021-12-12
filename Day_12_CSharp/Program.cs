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
            Console.WriteLine("Advent of Code 2021 - Day 12: Passage Pathing (https://adventofcode.com/2021/day/12)");
            
            string inputFile = args.Length > 0 ? args[0] : "input.txt";
            
            Console.WriteLine("Part 1: How many paths through this cave system are there that visit small caves at most once?");
            Console.WriteLine("Result: " + CalculatePathsCount(inputFile, false));

            Console.WriteLine("Part 2: Given these new rules, how many paths through this cave system are there?");
            Console.WriteLine("Result: " + CalculatePathsCount(inputFile, true));
        }

        static int CalculatePathsCount(string inputFile, bool isPart2) {
            var connections = GetConnections(inputFile);
            var startConnections = connections.Select(c => c).Where(c => c.Item1 == "start").ToList();
            connections = connections.Select(c => c).Where(c => c.Item1 != "start").ToList();
            var paths = new List<List<string>>();
            foreach (var startConnection in startConnections)
            {
                var path = new List<string>();
                path.Add(startConnection.Item1);
                path.Add(startConnection.Item2);
                paths = BuildPath(paths, path.ToList(), connections.ToList(), false, isPart2);

            }
            return paths.Count();
        }

        static List<List<string>> BuildPath(List<List<string>> paths, List<string> path, List<Tuple<string,string>> connections, bool hasDoubleVisit, bool isPart2)
        {
            foreach(var connection in connections) {
                if (path.Last() == connection.Item1) {
                    var nextPath = path.ToList();
                    bool hasNextPathDoubleVisit = hasDoubleVisit;
                    if(nextPath.Contains(connection.Item2) && Char.IsLower(connection.Item2.First())) {
                        if (isPart2) {
                            if("start" == connection.Item2 || hasNextPathDoubleVisit) {
                                continue;
                            }
                            hasNextPathDoubleVisit = true;
                        } else {
                            continue;
                        }
                    }
                    nextPath.Add(connection.Item2);
                    if (connection.Item2 == "end") {
                        paths.Add(nextPath);
                    } else {
                        paths = BuildPath(paths, nextPath, connections.ToList(), hasNextPathDoubleVisit, isPart2);
                    }
                }
            }
            return paths;
        }

        static List<Tuple<string,string>> GetConnections(string inputFile) {
            var connections = File.ReadLines(inputFile).Select(l => { var p = l.Split('-'); return new Tuple<string, string>(p[0], p[1]);}).ToList();
            foreach(var connection in connections.ToList()) {
                if(connection.Item2 != "end") {
                 connections.Add(new Tuple<string, string>(connection.Item2, connection.Item1)); 
                }
            }
            return connections;
        }
    }
}